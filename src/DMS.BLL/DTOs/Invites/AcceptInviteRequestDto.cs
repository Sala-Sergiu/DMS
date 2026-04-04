namespace DMS.BLL.DTOs.Invites;

public class AcceptInviteRequestDto
{
    public string Token { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? Location { get; set; }
}