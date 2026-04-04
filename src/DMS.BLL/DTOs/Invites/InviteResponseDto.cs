using DMS.Domain.Enums;

namespace DMS.BLL.DTOs.Invites;

public class InviteResponseDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public DateTime ExpiresAtUtc { get; set; }
    public bool IsUsed { get; set; }
}
