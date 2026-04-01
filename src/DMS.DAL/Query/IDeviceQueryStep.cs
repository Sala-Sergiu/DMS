using DMS.DAL.Repositories;
using DMS.Domain.Entities;

namespace DMS.DAL.Query;

public interface IDeviceQueryStep
{
    IQueryable<Device> Apply(IQueryable<Device> query, DeviceQueryParameters parameters);
}