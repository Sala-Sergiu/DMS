namespace DMS.BLL.DTOs.Auth;

public class LoginRequestDto
{
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}