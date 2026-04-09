using DMS.DAL.Repositories;
using DMS.Domain.Entities;

namespace DMS.DAL.Query;

public sealed class DeviceQueryPipeline
{
    private readonly IEnumerable<IDeviceQueryStep> _steps;

    public DeviceQueryPipeline()
    {
        _steps =
        [
            new DeviceSearchStep(),
            new DeviceFilterStep(),
            new DeviceSortStep()
        ];
    }

    public IQueryable<Device> Apply(IQueryable<Device> query, DeviceQueryParameters parameters)
    {
        return _steps.Aggregate(query, (current, step) => step.Apply(current, parameters));
    }
}