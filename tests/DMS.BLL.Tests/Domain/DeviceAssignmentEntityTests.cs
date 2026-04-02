using DMS.BLL.Tests.Helpers;
using DMS.Domain.Entities;
using FluentAssertions;

namespace DMS.BLL.Tests.Domain;

public class DeviceAssignmentEntityTests
{
    [Fact]
    public void Constructor_WithValidArguments_CreatesActiveAssignment()
    {
        var device = DeviceBuilder.CreateAvailable();
        var user = UserBuilder.CreateActive();

        var assignment = new DeviceAssignment(device, user, "Initial note");

        assignment.Id.Should().NotBeEmpty();
        assignment.DeviceId.Should().Be(device.Id);
        assignment.UserId.Should().Be(user.Id);
        assignment.IsActive.Should().BeTrue();
        assignment.ReturnedAt.Should().BeNull();
        assignment.Notes.Should().Be("Initial note");
    }

    [Fact]
    public void Constructor_WithNullDevice_ThrowsArgumentNullException()
    {
        var user = UserBuilder.CreateActive();

        var act = () => new DeviceAssignment(null!, user);

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Constructor_WithNullUser_ThrowsArgumentNullException()
    {
        var device = DeviceBuilder.CreateAvailable();

        var act = () => new DeviceAssignment(device, null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Return_ClosesActiveAssignment()
    {
        var device = DeviceBuilder.CreateAvailable();
        var user = UserBuilder.CreateActive();
        var assignment = new DeviceAssignment(device, user);

        assignment.Return("Returned cleanly");

        assignment.IsActive.Should().BeFalse();
        assignment.ReturnedAt.Should().NotBeNull();
        assignment.Notes.Should().Be("Returned cleanly");
    }

    [Fact]
    public void Return_WhenAlreadyReturned_ThrowsInvalidOperationException()
    {
        var device = DeviceBuilder.CreateAvailable();
        var user = UserBuilder.CreateActive();
        var assignment = new DeviceAssignment(device, user);
        assignment.Return();

        var act = () => assignment.Return();

        act.Should().Throw<InvalidOperationException>();
    }
}