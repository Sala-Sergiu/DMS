namespace DMS.BLL.DTOs.User;

public class UserResponseDto
{
    public Guid Id { get; init; }
    public string FullName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string? Location { get; init; }
    public DateTime CreatedAtUtc { get; init; }
    public IReadOnlyList<UserAssignedDeviceDto> ActiveAssignments { get; init; } = [];
}