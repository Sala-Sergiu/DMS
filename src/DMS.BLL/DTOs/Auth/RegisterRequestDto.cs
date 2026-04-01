namespace DMS.BLL.DTOs.Auth;

public class RegisterRequestDto
{
    public string FullName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string? Location { get; init; }
}