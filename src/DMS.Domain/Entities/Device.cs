using DMS.Domain.Enums;

namespace DMS.Domain.Entities;

public class Device
{
    private readonly List<DeviceAssignment> _assignments = new();

    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string SerialNumber { get; private set; }
    public string AssetTag { get; private set; }
    public string Brand { get; private set; }
    public string Model { get; private set; }
    public DeviceType Type { get; private set; }
    public DeviceStatus Status { get; private set; }
    public DateTime? PurchasedAtUtc { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime UpdatedAtUtc { get; private set; }

    /// <summary>Read-only view of this device's full assignment history.</summary>
    public IReadOnlyCollection<DeviceAssignment> Assignments => _assignments.AsReadOnly();

    /// <summary>Returns the currently active assignment, or null if unassigned.</summary>
    public DeviceAssignment? ActiveAssignment =>
        _assignments.SingleOrDefault(a => a.ReturnedAt is null);

    public bool IsAssigned => ActiveAssignment is not null;

    // Required by EF Core — not for direct use in application code
    private Device() { }

    public Device(
        string name,
        string serialNumber,
        string assetTag,
        string brand,
        string model,
        DeviceType type,
        DateTime? purchasedAtUtc = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Device name is required.", nameof(name));

        if (string.IsNullOrWhiteSpace(serialNumber))
            throw new ArgumentException("Serial number is required.", nameof(serialNumber));

        if (string.IsNullOrWhiteSpace(assetTag))
            throw new ArgumentException("Asset tag is required.", nameof(assetTag));

        if (string.IsNullOrWhiteSpace(brand))
            throw new ArgumentException("Brand is required.", nameof(brand));

        if (string.IsNullOrWhiteSpace(model))
            throw new ArgumentException("Model is required.", nameof(model));

        Id = Guid.NewGuid();
        Name = name;
        SerialNumber = serialNumber;
        AssetTag = assetTag;
        Brand = brand;
        Model = model;
        Type = type;
        Status = DeviceStatus.Available;
        PurchasedAtUtc = purchasedAtUtc;
        CreatedAtUtc = DateTime.UtcNow;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void MarkAsAvailable()
    {
        if (Status == DeviceStatus.Available) return;

        Status = DeviceStatus.Available;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void MarkAsInUse()
    {
        if (Status == DeviceStatus.Retired)
            throw new InvalidOperationException("A retired device cannot be marked as in use.");

        Status = DeviceStatus.InUse;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void MarkAsUnderMaintenance()
    {
        if (Status == DeviceStatus.Retired)
            throw new InvalidOperationException("A retired device cannot be sent for maintenance.");

        Status = DeviceStatus.UnderMaintenance;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void MarkAsRetired()
    {
        if (IsAssigned)
            throw new InvalidOperationException("Cannot retire a device that is currently assigned.");

        Status = DeviceStatus.Retired;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void UpdateDetails(string name, string brand, string model, DeviceType type)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Device name is required.", nameof(name));

        Name = name;
        Brand = brand;
        Model = model;
        Type = type;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    /// <summary>
    /// Called by DeviceAssignment to register itself under this device.
    /// Assignment orchestration lives in the Application layer.
    /// </summary>
    internal void AddAssignment(DeviceAssignment assignment)
    {
        ArgumentNullException.ThrowIfNull(assignment);

        if (IsAssigned)
            throw new InvalidOperationException("Device already has an active assignment.");

        _assignments.Add(assignment);
    }
}