using DMS.DAL.Repositories;
using DMS.Domain.Entities;

namespace DMS.DAL.Query;

public sealed class DeviceSortStep : IDeviceQueryStep
{
    public IQueryable<Device> Apply(IQueryable<Device> query, DeviceQueryParameters parameters)
    {
        return parameters.SortBy?.ToLower() switch
        {
            "name" => parameters.SortDescending ? query.OrderByDescending(d => d.Name) : query.OrderBy(d => d.Name),
            "brand" => parameters.SortDescending ? query.OrderByDescending(d => d.Brand) : query.OrderBy(d => d.Brand),
            "status" => parameters.SortDescending ? query.OrderByDescending(d => d.Status) : query.OrderBy(d => d.Status),
            "createdat" => parameters.SortDescending ? query.OrderByDescending(d => d.CreatedAtUtc) : query.OrderBy(d => d.CreatedAtUtc),
            _ => query.OrderBy(d => d.Name)
        };
    }
}