namespace DMS.BLL.DTOs.Auth;

public class LoginResponseDto
{
    public string Token { get; init; } = string.Empty;
    public DateTime ExpiresAt { get; init; }
}