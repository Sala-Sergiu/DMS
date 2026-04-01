using DMS.DAL.Persistence;
using DMS.DAL.Query;
using DMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DMS.DAL.Repositories;

public class DeviceRepository : Repository<Device>, IDeviceRepository
{
    private static readonly DeviceQueryPipeline QueryPipeline = new();

    public DeviceRepository(DmsDbContext context) : base(context) { }

    public async Task<Device?> GetWithAssignmentsAsync(Guid id, CancellationToken cancellationToken = default)
        => await Context.Devices
            .Include(d => d.Assignments)
                .ThenInclude(a => a.User)
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);

    public async Task<bool> IsSerialNumberTakenAsync(string serialNumber, Guid? excludeDeviceId = null, CancellationToken cancellationToken = default)
        => await Context.Devices
            .AnyAsync(d => d.SerialNumber == serialNumber && d.Id != excludeDeviceId, cancellationToken);

    public async Task<bool> IsAssetTagTakenAsync(string assetTag, Guid? excludeDeviceId = null, CancellationToken cancellationToken = default)
        => await Context.Devices
            .AnyAsync(d => d.AssetTag == assetTag && d.Id != excludeDeviceId, cancellationToken);

    public async Task AddAssignmentAsync(DeviceAssignment assignment, CancellationToken cancellationToken = default)
        => await Context.DeviceAssignments.AddAsync(assignment, cancellationToken);

    public async Task<(IReadOnlyList<Device> Items, int TotalCount)> GetPagedAsync(
        DeviceQueryParameters parameters,
        CancellationToken cancellationToken = default)
    {
        var query = QueryPipeline.Apply(Context.Devices.AsQueryable(), parameters);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}