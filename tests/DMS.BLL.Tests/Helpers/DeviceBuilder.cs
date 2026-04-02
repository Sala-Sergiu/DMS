using DMS.Domain.Entities;
using DMS.Domain.Enums;

namespace DMS.BLL.Tests.Helpers;

internal static class DeviceBuilder
{
    public static Device CreateAvailable(
        string name = "Test Device",
        string serialNumber = "SN-001",
        string assetTag = "AT-001",
        string brand = "TestBrand",
        string model = "TestModel",
        DeviceType type = DeviceType.Laptop)
    {
        return new Device(name, serialNumber, assetTag, brand, model, type);
    }
}