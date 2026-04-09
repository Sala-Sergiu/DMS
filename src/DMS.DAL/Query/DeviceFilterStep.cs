using DMS.DAL.Repositories;
using DMS.Domain.Entities;

namespace DMS.DAL.Query;

public sealed class DeviceFilterStep : IDeviceQueryStep
{
    public IQueryable<Device> Apply(IQueryable<Device> query, DeviceQueryParameters parameters)
    {
        if (parameters.Type.HasValue)
            query = query.Where(d => d.Type == parameters.Type.Value);

        if (parameters.Status.HasValue)
            query = query.Where(d => d.Status == parameters.Status.Value);

        return query;
    }
}