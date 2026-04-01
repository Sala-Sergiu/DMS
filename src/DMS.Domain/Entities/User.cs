using DMS.Domain.Enums;

namespace DMS.Domain.Entities;

public class User
{
    private readonly List<DeviceAssignment> _assignments = new();

    public Guid Id { get; private set; }
    public string FullName { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public UserRole Role { get; private set; }
    public string? Location { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime UpdatedAtUtc { get; private set; }

    /// <summary>Read-only view of this user's assignment history.</summary>
    public IReadOnlyCollection<DeviceAssignment> Assignments => _assignments.AsReadOnly();

    // Required by EF Core — not for direct use in application code
    private User() { }

    public User(string fullName, string email, string passwordHash, UserRole role, string? location = null)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Full name is required.", nameof(fullName));

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required.", nameof(email));

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("Password hash is required.", nameof(passwordHash));

        Id = Guid.NewGuid();
        FullName = fullName;
        Email = email.ToLowerInvariant();
        PasswordHash = passwordHash;
        Role = role;
        Location = location;
        IsActive = true;
        CreatedAtUtc = DateTime.UtcNow;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void Activate()
    {
        if (IsActive) return;

        IsActive = true;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        if (!IsActive) return;

        IsActive = false;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void UpdateProfile(string fullName, string? location)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Full name is required.", nameof(fullName));

        FullName = fullName;
        Location = location;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void ChangeRole(UserRole role)
    {
        Role = role;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    /// <summary>
    /// Called by DeviceAssignment to register itself under this user.
    /// Assignment orchestration lives in the Application layer.
    /// </summary>
    internal void AddAssignment(DeviceAssignment assignment)
    {
        ArgumentNullException.ThrowIfNull(assignment);
        _assignments.Add(assignment);
    }
}