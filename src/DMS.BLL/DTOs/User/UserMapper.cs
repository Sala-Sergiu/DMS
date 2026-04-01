using DMS.BLL.DTOs.User;
using DMS.Domain.Entities;

namespace DMS.BLL.Mappers;

internal static class UserMapper
{
    internal static UserResponseDto ToResponseDto(User user) => new()
    {
        Id = user.Id,
        FullName = user.FullName,
        Email = user.Email,
        Role = user.Role,
        Location = user.Location,
        IsActive = user.IsActive,
        CreatedAtUtc = user.CreatedAtUtc
    };
}