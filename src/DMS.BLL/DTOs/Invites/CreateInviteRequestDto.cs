using DMS.Domain.Enums;

namespace DMS.BLL.DTOs.Invites;

public class CreateInviteRequestDto
{
    public string Email { get; set; } = string.Empty;
    public UserRole Role { get; set; }
}