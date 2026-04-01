using DMS.DAL.Repositories;
using DMS.Domain.Entities;

namespace DMS.DAL.Query;

public sealed class DeviceSearchStep : IDeviceQueryStep
{
    public IQueryable<Device> Apply(IQueryable<Device> query, DeviceQueryParameters parameters)
    {
        if (string.IsNullOrWhiteSpace(parameters.SearchTerm))
            return query;

        var term = parameters.SearchTerm.ToLower();

        return query.Where(d =>
            d.Name.ToLower().Contains(term) ||
            d.Brand.ToLower().Contains(term) ||
            d.Model.ToLower().Contains(term) ||
            d.SerialNumber.ToLower().Contains(term));
    }
}