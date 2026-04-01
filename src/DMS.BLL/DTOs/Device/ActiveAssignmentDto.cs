namespace DMS.BLL.DTOs.Device;

public class ActiveAssignmentDto
{
    public Guid AssignmentId { get; init; }
    public Guid UserId { get; init; }
    public string UserFullName { get; init; } = string.Empty;
    public DateTime AssignedAtUtc { get; init; }
}