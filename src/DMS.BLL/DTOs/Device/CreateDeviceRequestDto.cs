using DMS.Domain.Enums;

namespace DMS.BLL.DTOs.Device;

public class CreateDeviceRequestDto
{
    public string Name { get; init; } = string.Empty;
    public string SerialNumber { get; init; } = string.Empty;
    public string AssetTag { get; init; } = string.Empty;
    public string Brand { get; init; } = string.Empty;
    public string Model { get; init; } = string.Empty;
    public DeviceType Type { get; init; }
    public DateTime? PurchasedAtUtc { get; init; }
}