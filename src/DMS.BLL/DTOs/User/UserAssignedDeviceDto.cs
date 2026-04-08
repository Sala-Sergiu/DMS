namespace DMS.BLL.DTOs.User;

public class UserAssignedDeviceDto
{
    public Guid AssignmentId { get; init; }
    public Guid DeviceId { get; init; }
    public string DeviceName { get; init; } = string.Empty;
    public string Brand { get; init; } = string.Empty;
    public string Model { get; init; } = string.Empty;
    public DateTime AssignedAtUtc { get; init; }
}