using DMS.Domain.Enums;

namespace DMS.Domain.Entities;

public class DeviceAssignment
{
    public Guid Id { get; private set; }
    public Guid DeviceId { get; private set; }
    public Guid UserId { get; private set; }
    public DateTime AssignedAtUtc { get; private set; }
    public DateTime? ReturnedAt { get; private set; }
    public string? Notes { get; private set; }

    // Navigation — populated by EF Core or set during construction
    public Device Device { get; private set; } = null!;
    public User User { get; private set; } = null!;

    /// <summary>True when the device has not yet been returned.</summary>
    public bool IsActive => ReturnedAt is null;

    // Required by EF Core — not for direct use in application code
    private DeviceAssignment() { }

    /// <summary>
    /// Creates a new active assignment between a device and a user.
    /// The device and user must both be valid and the device must not already be assigned.
    /// </summary>
    public DeviceAssignment(Device device, User user, string? notes = null)
    {
        ArgumentNullException.ThrowIfNull(device);
        ArgumentNullException.ThrowIfNull(user);

        if (!user.IsActive)
            throw new InvalidOperationException("Cannot assign a device to an inactive user.");

        if (device.IsAssigned)
            throw new InvalidOperationException("Device is already assigned to another user.");

        if (device.Status == DeviceStatus.Retired)
            throw new InvalidOperationException("Cannot assign a retired device.");

        Id = Guid.NewGuid();
        DeviceId = device.Id;
        UserId = user.Id;
        Device = device;
        User = user;
        AssignedAtUtc = DateTime.UtcNow;
        Notes = notes;

        // Register this assignment on both sides of the relationship
        device.AddAssignment(this);
        user.AddAssignment(this);

        device.MarkAsInUse();
    }

    /// <summary>
    /// Closes the active assignment by recording the return date.
    /// The device is transitioned back to Available.
    /// </summary>
    public void Return(string? notes = null)
    {
        if (!IsActive)
            throw new InvalidOperationException("This assignment has already been returned.");

        ReturnedAt = DateTime.UtcNow;

        if (notes is not null)
            Notes = notes;

        Device.MarkAsAvailable();
    }
}