using DMS.BLL.DTOs.Auth;
using DMS.BLL.Exceptions;
using DMS.BLL.Services;
using DMS.DAL.Repositories;
using DMS.DAL.UnitOfWork;
using DMS.Domain.Entities;
using DMS.Domain.Enums;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;

namespace DMS.BLL.Tests.Services;

public class AuthServiceTests
{
    private readonly Mock<IUnitOfWork> _uow;
    private readonly Mock<IUserRepository> _userRepo;
    private readonly Mock<IDeviceRepository> _deviceRepo;
    private readonly IConfiguration _configuration;
    private readonly AuthService _sut;

    public AuthServiceTests()
    {
        _uow = new Mock<IUnitOfWork>();
        _userRepo = new Mock<IUserRepository>();
        _deviceRepo = new Mock<IDeviceRepository>();

        _uow.Setup(u => u.Users).Returns(_userRepo.Object);
        _uow.Setup(u => u.Devices).Returns(_deviceRepo.Object);

        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Secret"] = "super-secret-key-for-unit-tests-minimum-32-chars!!",
                ["Jwt:Issuer"] = "DMS.API",
                ["Jwt:Audience"] = "DMS.Client",
                ["Jwt:ExpirationHours"] = "8"
            })
            .Build();

        _sut = new AuthService(_uow.Object, _configuration);
    }

    // ─── RegisterAsync ─────────────────────────────────────────────────

    [Fact]
    public async Task RegisterAsync_FirstUser_IsAssignedAdminRole()
    {
        var request = new RegisterRequestDto
        {
            FullName = "Admin User",
            Email = "admin@dms.com",
            Password = "SecurePass1!"
        };

        _userRepo.Setup(r => r.ExistsByEmailAsync(request.Email, null, default)).ReturnsAsync(false);
        _userRepo.Setup(r => r.AnyAsync(It.IsAny<System.Linq.Expressions.Expression<Func<User, bool>>>(), default)).ReturnsAsync(false);
        _userRepo.Setup(r => r.AddAsync(It.IsAny<User>(), default)).Returns(Task.CompletedTask);
        _uow.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);

        var result = await _sut.RegisterAsync(request);

        result.Role.Should().Be(UserRole.Admin);
    }

    [Fact]
    public async Task RegisterAsync_SubsequentUser_IsAssignedEmployeeRole()
    {
        var request = new RegisterRequestDto
        {
            FullName = "Regular User",
            Email = "user@dms.com",
            Password = "SecurePass1!"
        };

        _userRepo.Setup(r => r.ExistsByEmailAsync(request.Email, null, default)).ReturnsAsync(false);
        _userRepo.Setup(r => r.AnyAsync(It.IsAny<System.Linq.Expressions.Expression<Func<User, bool>>>(), default)).ReturnsAsync(true);
        _userRepo.Setup(r => r.AddAsync(It.IsAny<User>(), default)).Returns(Task.CompletedTask);
        _uow.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);

        var result = await _sut.RegisterAsync(request);

        result.Role.Should().Be(UserRole.Employee);
    }

    [Fact]
    public async Task RegisterAsync_WhenEmailAlreadyExists_ThrowsConflictException()
    {
        var request = new RegisterRequestDto
        {
            FullName = "Test",
            Email = "existing@dms.com",
            Password = "SecurePass1!"
        };

        _userRepo.Setup(r => r.ExistsByEmailAsync(request.Email, null, default)).ReturnsAsync(true);

        var act = () => _sut.RegisterAsync(request);

        await act.Should().ThrowAsync<ConflictException>()
            .WithMessage("*already registered*");
    }

    [Theory]
    [InlineData("", "email@test.com", "Password1")]
    [InlineData("Name", "", "Password1")]
    [InlineData("Name", "email@test.com", "")]
    [InlineData("Name", "email@test.com", "12345")]  // too short
    public async Task RegisterAsync_WithInvalidInput_ThrowsValidationException(
        string fullName, string email, string password)
    {
        var request = new RegisterRequestDto
        {
            FullName = fullName,
            Email = email,
            Password = password
        };

        var act = () => _sut.RegisterAsync(request);

        await act.Should().ThrowAsync<ValidationException>();
    }

    // ─── LoginAsync ────────────────────────────────────────────────────

    [Fact]
    public async Task LoginAsync_WithValidCredentials_ReturnsToken()
    {
        var password = "ValidPass123!";
        var hash = BCrypt.Net.BCrypt.HashPassword(password);
        var user = new User("Test User", "test@dms.com", hash, UserRole.Employee);

        _userRepo.Setup(r => r.GetByEmailAsync("test@dms.com", default)).ReturnsAsync(user);

        var result = await _sut.LoginAsync(new LoginRequestDto
        {
            Email = "test@dms.com",
            Password = password
        });

        result.Should().NotBeNull();
        result.Token.Should().NotBeNullOrWhiteSpace();
        result.ExpiresAt.Should().BeAfter(DateTime.UtcNow);
    }

    [Fact]
    public async Task LoginAsync_WhenUserNotFound_ThrowsUnauthorizedException()
    {
        _userRepo.Setup(r => r.GetByEmailAsync("unknown@dms.com", default)).ReturnsAsync((User?)null);

        var act = () => _sut.LoginAsync(new LoginRequestDto
        {
            Email = "unknown@dms.com",
            Password = "AnyPass123"
        });

        await act.Should().ThrowAsync<UnauthorizedException>();
    }

    [Fact]
    public async Task LoginAsync_WithWrongPassword_ThrowsUnauthorizedException()
    {
        var hash = BCrypt.Net.BCrypt.HashPassword("CorrectPassword");
        var user = new User("Test", "test@dms.com", hash, UserRole.Employee);

        _userRepo.Setup(r => r.GetByEmailAsync("test@dms.com", default)).ReturnsAsync(user);

        var act = () => _sut.LoginAsync(new LoginRequestDto
        {
            Email = "test@dms.com",
            Password = "WrongPassword"
        });

        await act.Should().ThrowAsync<UnauthorizedException>();
    }

    [Fact]
    public async Task LoginAsync_WhenUserIsInactive_ThrowsUnauthorizedException()
    {
        var hash = BCrypt.Net.BCrypt.HashPassword("Pass123!");
        var user = new User("Inactive User", "inactive@dms.com", hash, UserRole.Employee);
        user.Deactivate();

        _userRepo.Setup(r => r.GetByEmailAsync("inactive@dms.com", default)).ReturnsAsync(user);

        var act = () => _sut.LoginAsync(new LoginRequestDto
        {
            Email = "inactive@dms.com",
            Password = "Pass123!"
        });

        await act.Should().ThrowAsync<UnauthorizedException>()
            .WithMessage("*inactive*");
    }

    [Theory]
    [InlineData("", "Password1")]
    [InlineData("email@test.com", "")]
    public async Task LoginAsync_WithMissingCredentials_ThrowsValidationException(
        string email, string password)
    {
        var act = () => _sut.LoginAsync(new LoginRequestDto
        {
            Email = email,
            Password = password
        });

        await act.Should().ThrowAsync<ValidationException>();
    }
}