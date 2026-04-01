using DMS.Domain.Enums;

namespace DMS.BLL.DTOs.Device;

public class DeviceResponseDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string SerialNumber { get; init; } = string.Empty;
    public string AssetTag { get; init; } = string.Empty;
    public string Brand { get; init; } = string.Empty;
    public string Model { get; init; } = string.Empty;
    public DeviceType Type { get; init; }
    public DeviceStatus Status { get; init; }
    public DateTime? PurchasedAtUtc { get; init; }
    public DateTime CreatedAtUtc { get; init; }
    public DateTime UpdatedAtUtc { get; init; }
    public ActiveAssignmentDto? ActiveAssignment { get; init; }
}