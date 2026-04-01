using DMS.Domain.Enums;

namespace DMS.BLL.DTOs.User;

public class ChangeUserRoleRequestDto
{
    public UserRole Role { get; init; }
}