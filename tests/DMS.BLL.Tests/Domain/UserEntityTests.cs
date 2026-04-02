using DMS.Domain.Entities;
using DMS.Domain.Enums;
using FluentAssertions;

namespace DMS.BLL.Tests.Domain;

public class UserEntityTests
{
    [Fact]
    public void Constructor_WithValidArguments_CreatesActiveUser()
    {
        var user = new User("John Doe", "john@test.com", "hashedpwd", UserRole.Employee);

        user.Id.Should().NotBeEmpty();
        user.FullName.Should().Be("John Doe");
        user.Email.Should().Be("john@test.com");
        user.IsActive.Should().BeTrue();
        user.Role.Should().Be(UserRole.Employee);
    }

    [Fact]
    public void Constructor_EmailIsStoredLowerCase()
    {
        var user = new User("Test", "Test@UPPER.COM", "hash", UserRole.Employee);

        user.Email.Should().Be("test@upper.com");
    }

    [Theory]
    [InlineData("", "email@test.com", "hash")]
    [InlineData("Name", "", "hash")]
    [InlineData("Name", "email@test.com", "")]
    public void Constructor_WithMissingField_ThrowsArgumentException(
        string fullName, string email, string passwordHash)
    {
        var act = () => new User(fullName, email, passwordHash, UserRole.Employee);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Deactivate_SetsIsActiveToFalse()
    {
        var user = new User("Test", "test@test.com", "hash", UserRole.Employee);

        user.Deactivate();

        user.IsActive.Should().BeFalse();
    }

    [Fact]
    public void Activate_SetsIsActiveToTrue()
    {
        var user = new User("Test", "test@test.com", "hash", UserRole.Employee);
        user.Deactivate();

        user.Activate();

        user.IsActive.Should().BeTrue();
    }

    [Fact]
    public void UpdateProfile_WithValidData_UpdatesNameAndLocation()
    {
        var user = new User("Old Name", "test@test.com", "hash", UserRole.Employee, "Bucharest");

        user.UpdateProfile("New Name", "Cluj");

        user.FullName.Should().Be("New Name");
        user.Location.Should().Be("Cluj");
    }

    [Fact]
    public void UpdateProfile_WithEmptyName_ThrowsArgumentException()
    {
        var user = new User("Name", "test@test.com", "hash", UserRole.Employee);

        var act = () => user.UpdateProfile("", null);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void ChangeRole_UpdatesRole()
    {
        var user = new User("Name", "test@test.com", "hash", UserRole.Employee);

        user.ChangeRole(UserRole.Admin);

        user.Role.Should().Be(UserRole.Admin);
    }
}