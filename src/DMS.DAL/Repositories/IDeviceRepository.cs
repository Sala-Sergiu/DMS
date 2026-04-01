using DMS.Domain.Entities;

namespace DMS.DAL.Repositories;

public interface IDeviceRepository : IRepository<Device>
{
    Task<Device?> GetWithAssignmentsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> IsSerialNumberTakenAsync(string serialNumber, Guid? excludeDeviceId = null, CancellationToken cancellationToken = default);
    Task<bool> IsAssetTagTakenAsync(string assetTag, Guid? excludeDeviceId = null, CancellationToken cancellationToken = default);
    Task<(IReadOnlyList<Device> Items, int TotalCount)> GetPagedAsync(DeviceQueryParameters parameters, CancellationToken cancellationToken = default);
    Task AddAssignmentAsync(DeviceAssignment assignment, CancellationToken cancellationToken = default);
}