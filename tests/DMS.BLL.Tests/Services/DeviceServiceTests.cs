using DMS.BLL.DTOs.Assignment;
using DMS.BLL.DTOs.Device;
using DMS.BLL.Exceptions;
using DMS.BLL.Services;
using DMS.BLL.Tests.Helpers;
using DMS.DAL.Repositories;
using DMS.DAL.UnitOfWork;
using DMS.Domain.Entities;
using DMS.Domain.Enums;
using FluentAssertions;
using Moq;

namespace DMS.BLL.Tests.Services;

public class DeviceServiceTests
{
    private readonly Mock<IUnitOfWork> _uow;
    private readonly Mock<IDeviceRepository> _deviceRepo;
    private readonly Mock<IUserRepository> _userRepo;
    private readonly DeviceService _sut;

    public DeviceServiceTests()
    {
        _uow = new Mock<IUnitOfWork>();
        _deviceRepo = new Mock<IDeviceRepository>();
        _userRepo = new Mock<IUserRepository>();

        _uow.Setup(u => u.Devices).Returns(_deviceRepo.Object);
        _uow.Setup(u => u.Users).Returns(_userRepo.Object);

        _sut = new DeviceService(_uow.Object);
    }

    // ─── GetByIdAsync ──────────────────────────────────────────────────

    [Fact]
    public async Task GetByIdAsync_WhenDeviceExists_ReturnsDto()
    {
        var device = DeviceBuilder.CreateAvailable();
        _deviceRepo
            .Setup(r => r.GetWithAssignmentsAsync(device.Id, default))
            .ReturnsAsync(device);

        var result = await _sut.GetByIdAsync(device.Id);

        result.Should().NotBeNull();
        result.Id.Should().Be(device.Id);
        result.Name.Should().Be(device.Name);
    }

    [Fact]
    public async Task GetByIdAsync_WhenDeviceNotFound_ThrowsNotFoundException()
    {
        var id = Guid.NewGuid();
        _deviceRepo
            .Setup(r => r.GetWithAssignmentsAsync(id, default))
            .ReturnsAsync((Device?)null);

        var act = () => _sut.GetByIdAsync(id);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    // ─── CreateAsync ───────────────────────────────────────────────────

    [Fact]
    public async Task CreateAsync_WithValidRequest_CreatesAndReturnsDevice()
    {
        var request = new CreateDeviceRequestDto
        {
            Name = "Laptop X1",
            SerialNumber = "SN-999",
            AssetTag = "AT-999",
            Brand = "Dell",
            Model = "Latitude",
            Type = DeviceType.Laptop
        };

        _deviceRepo.Setup(r => r.IsSerialNumberTakenAsync(request.SerialNumber, null, default)).ReturnsAsync(false);
        _deviceRepo.Setup(r => r.IsAssetTagTakenAsync(request.AssetTag, null, default)).ReturnsAsync(false);
        _deviceRepo.Setup(r => r.AddAsync(It.IsAny<Device>(), default)).Returns(Task.CompletedTask);
        _uow.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);

        var result = await _sut.CreateAsync(request);

        result.Should().NotBeNull();
        result.Name.Should().Be("Laptop X1");
        result.SerialNumber.Should().Be("SN-999");
        _deviceRepo.Verify(r => r.AddAsync(It.IsAny<Device>(), default), Times.Once);
        _uow.Verify(u => u.SaveChangesAsync(default), Times.Once);
    }

    [Theory]
    [InlineData("", "SN-1", "AT-1", "Brand", "Model")]
    [InlineData("Name", "", "AT-1", "Brand", "Model")]
    [InlineData("Name", "SN-1", "", "Brand", "Model")]
    [InlineData("Name", "SN-1", "AT-1", "", "Model")]
    [InlineData("Name", "SN-1", "AT-1", "Brand", "")]
    public async Task CreateAsync_WithMissingRequiredField_ThrowsValidationException(
        string name, string serial, string asset, string brand, string model)
    {
        var request = new CreateDeviceRequestDto
        {
            Name = name,
            SerialNumber = serial,
            AssetTag = asset,
            Brand = brand,
            Model = model,
            Type = DeviceType.Laptop
        };

        var act = () => _sut.CreateAsync(request);

        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task CreateAsync_WhenSerialNumberAlreadyTaken_ThrowsConflictException()
    {
        var request = new CreateDeviceRequestDto
        {
            Name = "Laptop",
            SerialNumber = "DUPLICATE-SN",
            AssetTag = "AT-1",
            Brand = "Dell",
            Model = "Latitude",
            Type = DeviceType.Laptop
        };

        _deviceRepo.Setup(r => r.IsSerialNumberTakenAsync("DUPLICATE-SN", null, default)).ReturnsAsync(true);

        var act = () => _sut.CreateAsync(request);

        await act.Should().ThrowAsync<ConflictException>()
            .WithMessage("*serial number*");
    }

    [Fact]
    public async Task CreateAsync_WhenAssetTagAlreadyTaken_ThrowsConflictException()
    {
        var request = new CreateDeviceRequestDto
        {
            Name = "Laptop",
            SerialNumber = "SN-UNIQUE",
            AssetTag = "DUPLICATE-AT",
            Brand = "Dell",
            Model = "Latitude",
            Type = DeviceType.Laptop
        };

        _deviceRepo.Setup(r => r.IsSerialNumberTakenAsync("SN-UNIQUE", null, default)).ReturnsAsync(false);
        _deviceRepo.Setup(r => r.IsAssetTagTakenAsync("DUPLICATE-AT", null, default)).ReturnsAsync(true);

        var act = () => _sut.CreateAsync(request);

        await act.Should().ThrowAsync<ConflictException>()
            .WithMessage("*asset tag*");
    }

    // ─── UpdateAsync ───────────────────────────────────────────────────

    [Fact]
    public async Task UpdateAsync_WithValidRequest_UpdatesAndReturnsDevice()
    {
        var device = DeviceBuilder.CreateAvailable();
        var request = new UpdateDeviceRequestDto
        {
            Name = "Updated Name",
            Brand = "HP",
            Model = "EliteBook",
            Type = DeviceType.Laptop
        };

        _deviceRepo.Setup(r => r.GetByIdAsync(device.Id, default)).ReturnsAsync(device);
        _deviceRepo.Setup(r => r.Update(device));
        _uow.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);

        var result = await _sut.UpdateAsync(device.Id, request);

        result.Name.Should().Be("Updated Name");
        result.Brand.Should().Be("HP");
        _deviceRepo.Verify(r => r.Update(device), Times.Once);
        _uow.Verify(u => u.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WhenDeviceNotFound_ThrowsNotFoundException()
    {
        var id = Guid.NewGuid();
        _deviceRepo.Setup(r => r.GetByIdAsync(id, default)).ReturnsAsync((Device?)null);

        var request = new UpdateDeviceRequestDto
        {
            Name = "X",
            Brand = "Y",
            Model = "Z",
            Type = DeviceType.Laptop
        };

        var act = () => _sut.UpdateAsync(id, request);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Theory]
    [InlineData("", "Brand", "Model")]
    [InlineData("Name", "", "Model")]
    [InlineData("Name", "Brand", "")]
    public async Task UpdateAsync_WithMissingRequiredField_ThrowsValidationException(
        string name, string brand, string model)
    {
        var request = new UpdateDeviceRequestDto
        {
            Name = name,
            Brand = brand,
            Model = model,
            Type = DeviceType.Laptop
        };

        var act = () => _sut.UpdateAsync(Guid.NewGuid(), request);

        await act.Should().ThrowAsync<ValidationException>();
    }

    // ─── DeleteAsync ───────────────────────────────────────────────────

    [Fact]
    public async Task DeleteAsync_WhenDeviceIsUnassigned_DeletesDevice()
    {
        var device = DeviceBuilder.CreateAvailable();
        _deviceRepo.Setup(r => r.GetWithAssignmentsAsync(device.Id, default)).ReturnsAsync(device);
        _deviceRepo.Setup(r => r.Delete(device));
        _uow.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);

        await _sut.DeleteAsync(device.Id);

        _deviceRepo.Verify(r => r.Delete(device), Times.Once);
        _uow.Verify(u => u.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WhenDeviceIsAssigned_ThrowsConflictException()
    {
        var device = DeviceBuilder.CreateAvailable();
        var user = UserBuilder.CreateActive();
        var assignment = new DeviceAssignment(device, user);
        device.MarkAsInUse();

        _deviceRepo.Setup(r => r.GetWithAssignmentsAsync(device.Id, default)).ReturnsAsync(device);

        var act = () => _sut.DeleteAsync(device.Id);

        await act.Should().ThrowAsync<ConflictException>()
            .WithMessage("*assigned*");
    }

    [Fact]
    public async Task DeleteAsync_WhenDeviceNotFound_ThrowsNotFoundException()
    {
        var id = Guid.NewGuid();
        _deviceRepo.Setup(r => r.GetWithAssignmentsAsync(id, default)).ReturnsAsync((Device?)null);

        var act = () => _sut.DeleteAsync(id);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    // ─── AssignAsync ───────────────────────────────────────────────────

    [Fact]
    public async Task AssignAsync_WhenDeviceAvailableAndUserActive_AssignsSuccessfully()
    {
        var device = DeviceBuilder.CreateAvailable();
        var user = UserBuilder.CreateActive();
        var request = new AssignDeviceRequestDto { UserId = user.Id, Notes = "Test assignment" };

        _deviceRepo.Setup(r => r.GetWithAssignmentsAsync(device.Id, default)).ReturnsAsync(device);
        _userRepo.Setup(r => r.GetByIdAsync(user.Id, default)).ReturnsAsync(user);
        _deviceRepo.Setup(r => r.AddAssignmentAsync(It.IsAny<DeviceAssignment>(), default)).Returns(Task.CompletedTask);
        _uow.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);

        var result = await _sut.AssignAsync(device.Id, request);

        result.Should().NotBeNull();
        result.ActiveAssignment.Should().NotBeNull();
        device.Status.Should().Be(DeviceStatus.InUse);
        _deviceRepo.Verify(r => r.AddAssignmentAsync(It.IsAny<DeviceAssignment>(), default), Times.Once);
    }

    [Fact]
    public async Task AssignAsync_WhenDeviceNotFound_ThrowsNotFoundException()
    {
        var id = Guid.NewGuid();
        _deviceRepo.Setup(r => r.GetWithAssignmentsAsync(id, default)).ReturnsAsync((Device?)null);

        var act = () => _sut.AssignAsync(id, new AssignDeviceRequestDto { UserId = Guid.NewGuid() });

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task AssignAsync_WhenUserNotFound_ThrowsNotFoundException()
    {
        var device = DeviceBuilder.CreateAvailable();
        var userId = Guid.NewGuid();

        _deviceRepo.Setup(r => r.GetWithAssignmentsAsync(device.Id, default)).ReturnsAsync(device);
        _userRepo.Setup(r => r.GetByIdAsync(userId, default)).ReturnsAsync((User?)null);

        var act = () => _sut.AssignAsync(device.Id, new AssignDeviceRequestDto { UserId = userId });

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task AssignAsync_WhenUserIsInactive_ThrowsConflictException()
    {
        var device = DeviceBuilder.CreateAvailable();
        var user = UserBuilder.CreateInactive();

        _deviceRepo.Setup(r => r.GetWithAssignmentsAsync(device.Id, default)).ReturnsAsync(device);
        _userRepo.Setup(r => r.GetByIdAsync(user.Id, default)).ReturnsAsync(user);

        var act = () => _sut.AssignAsync(device.Id, new AssignDeviceRequestDto { UserId = user.Id });

        await act.Should().ThrowAsync<ConflictException>()
            .WithMessage("*inactive*");
    }

    [Fact]
    public async Task AssignAsync_WhenDeviceAlreadyAssigned_ThrowsConflictException()
    {
        var device = DeviceBuilder.CreateAvailable();
        var user1 = UserBuilder.CreateActive(email: "user1@test.com");
        var user2 = UserBuilder.CreateActive(email: "user2@test.com");

        // Simulate existing active assignment
        var existingAssignment = new DeviceAssignment(device, user1);
        device.MarkAsInUse();

        _deviceRepo.Setup(r => r.GetWithAssignmentsAsync(device.Id, default)).ReturnsAsync(device);
        _userRepo.Setup(r => r.GetByIdAsync(user2.Id, default)).ReturnsAsync(user2);

        var act = () => _sut.AssignAsync(device.Id, new AssignDeviceRequestDto { UserId = user2.Id });

        await act.Should().ThrowAsync<ConflictException>()
            .WithMessage("*already assigned*");
    }

    [Fact]
    public async Task AssignAsync_WhenDeviceIsRetired_ThrowsConflictException()
    {
        var device = DeviceBuilder.CreateAvailable();
        device.MarkAsRetired();

        var user = UserBuilder.CreateActive();

        _deviceRepo.Setup(r => r.GetWithAssignmentsAsync(device.Id, default)).ReturnsAsync(device);
        _userRepo.Setup(r => r.GetByIdAsync(user.Id, default)).ReturnsAsync(user);

        var act = () => _sut.AssignAsync(device.Id, new AssignDeviceRequestDto { UserId = user.Id });

        await act.Should().ThrowAsync<ConflictException>()
            .WithMessage("*retired*");
    }

    // ─── ReturnAsync ───────────────────────────────────────────────────

    [Fact]
    public async Task ReturnAsync_WhenDeviceIsAssigned_ReturnsSuccessfully()
    {
        var device = DeviceBuilder.CreateAvailable();
        var user = UserBuilder.CreateActive();
        var assignment = new DeviceAssignment(device, user);
        device.MarkAsInUse();

        _deviceRepo.Setup(r => r.GetWithAssignmentsAsync(device.Id, default)).ReturnsAsync(device);
        _uow.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);

        var result = await _sut.ReturnAsync(device.Id, new ReturnDeviceRequestDto { Notes = "Returned in good condition" });

        result.ActiveAssignment.Should().BeNull();
        device.Status.Should().Be(DeviceStatus.Available);
        assignment.IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task ReturnAsync_WhenDeviceIsNotAssigned_ThrowsConflictException()
    {
        var device = DeviceBuilder.CreateAvailable();

        _deviceRepo.Setup(r => r.GetWithAssignmentsAsync(device.Id, default)).ReturnsAsync(device);

        var act = () => _sut.ReturnAsync(device.Id, new ReturnDeviceRequestDto());

        await act.Should().ThrowAsync<ConflictException>()
            .WithMessage("*not currently assigned*");
    }

    [Fact]
    public async Task ReturnAsync_WhenDeviceNotFound_ThrowsNotFoundException()
    {
        var id = Guid.NewGuid();
        _deviceRepo.Setup(r => r.GetWithAssignmentsAsync(id, default)).ReturnsAsync((Device?)null);

        var act = () => _sut.ReturnAsync(id, new ReturnDeviceRequestDto());

        await act.Should().ThrowAsync<NotFoundException>();
    }
}