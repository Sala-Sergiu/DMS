using DMS.Domain.Enums;

namespace DMS.Domain.Entities;

public class Invite
{
    public Guid Id { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public string Token { get; private set; } = string.Empty;
    public UserRole Role { get; private set; }
    public bool IsUsed { get; private set; }
    public DateTime ExpiresAtUtc { get; private set; }
    public Guid CreatedByUserId { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }

    public User CreatedByUser { get; private set; } = null!;

    // Required by EF Core
    private Invite() { }

    public Invite(string email, string token, UserRole role, Guid createdByUserId, DateTime expiresAtUtc)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required.", nameof(email));

        if (string.IsNullOrWhiteSpace(token))
            throw new ArgumentException("Token is required.", nameof(token));

        Id = Guid.NewGuid();
        Email = email.ToLowerInvariant();
        Token = token;
        Role = role;
        IsUsed = false;
        CreatedByUserId = createdByUserId;
        ExpiresAtUtc = expiresAtUtc;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public void MarkAsUsed()
    {
        IsUsed = true;
    }

    public bool IsExpired() => DateTime.UtcNow > ExpiresAtUtc;
}