using DMS.BLL.Tests.Helpers;
using DMS.Domain.Entities;
using DMS.Domain.Enums;
using FluentAssertions;

namespace DMS.BLL.Tests.Domain;

public class DeviceEntityTests
{
    [Fact]
    public void Constructor_WithValidArguments_CreatesDeviceWithAvailableStatus()
    {
        var device = new Device("Laptop", "SN-1", "AT-1", "Dell", "Latitude", DeviceType.Laptop);

        device.Id.Should().NotBeEmpty();
        device.Status.Should().Be(DeviceStatus.Available);
        device.IsAssigned.Should().BeFalse();
    }

    [Theory]
    [InlineData("", "SN", "AT", "Brand", "Model")]
    [InlineData("Name", "", "AT", "Brand", "Model")]
    [InlineData("Name", "SN", "", "Brand", "Model")]
    [InlineData("Name", "SN", "AT", "", "Model")]
    [InlineData("Name", "SN", "AT", "Brand", "")]
    public void Constructor_WithMissingField_ThrowsArgumentException(
        string name, string serial, string asset, string brand, string model)
    {
        var act = () => new Device(name, serial, asset, brand, model, DeviceType.Laptop);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void MarkAsInUse_TransitionsStatusToInUse()
    {
        var device = DeviceBuilder.CreateAvailable();

        device.MarkAsInUse();

        device.Status.Should().Be(DeviceStatus.InUse);
    }

    [Fact]
    public void MarkAsInUse_WhenRetired_ThrowsInvalidOperationException()
    {
        var device = DeviceBuilder.CreateAvailable();
        device.MarkAsRetired();

        var act = () => device.MarkAsInUse();

        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void MarkAsRetired_WhenAssigned_ThrowsInvalidOperationException()
    {
        var device = DeviceBuilder.CreateAvailable();
        var user = UserBuilder.CreateActive();
        var _ = new DeviceAssignment(device, user);
        device.MarkAsInUse();

        var act = () => device.MarkAsRetired();

        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void MarkAsAvailable_TransitionsStatusBackToAvailable()
    {
        var device = DeviceBuilder.CreateAvailable();
        device.MarkAsInUse();

        device.MarkAsAvailable();

        device.Status.Should().Be(DeviceStatus.Available);
    }

    [Fact]
    public void UpdateDetails_WithValidData_UpdatesProperties()
    {
        var device = DeviceBuilder.CreateAvailable();

        device.UpdateDetails("New Name", "HP", "ProBook", DeviceType.Laptop,
            operatingSystem: "Windows", osVersion: "11 Pro",
            processor: "Intel Core i5", ramGb: 16, description: null);

        device.Name.Should().Be("New Name");
        device.Brand.Should().Be("HP");
        device.Model.Should().Be("ProBook");
        device.OperatingSystem.Should().Be("Windows");
        device.RamGb.Should().Be(16);
    }

    [Fact]
    public void UpdateDetails_WithEmptyName_ThrowsArgumentException()
    {
        var device = DeviceBuilder.CreateAvailable();

        var act = () => device.UpdateDetails("", "HP", "ProBook", DeviceType.Laptop,
            operatingSystem: null, osVersion: null,
            processor: null, ramGb: null, description: null);

        act.Should().Throw<ArgumentException>();
    }
}