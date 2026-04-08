using DMS.BLL.DTOs.User;
using DMS.Domain.Entities;

internal static class UserMapper
{
    internal static UserResponseDto ToResponseDto(User user) => new()
    {
        Id = user.Id,
        FullName = user.FullName,
        Email = user.Email,
        Location = user.Location,
        CreatedAtUtc = user.CreatedAtUtc,
        ActiveAssignments = user.Assignments
            .Where(a => a.IsActive)
            .Select(a => new UserAssignedDeviceDto
            {
                AssignmentId = a.Id,
                DeviceId = a.DeviceId,
                DeviceName = a.Device?.Name ?? string.Empty,
                Brand = a.Device?.Brand ?? string.Empty,
                Model = a.Device?.Model ?? string.Empty,
                AssignedAtUtc = a.AssignedAtUtc
            })
            .ToList()
    };
}