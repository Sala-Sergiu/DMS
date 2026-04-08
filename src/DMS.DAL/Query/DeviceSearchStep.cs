using DMS.DAL.Repositories;
using DMS.Domain.Entities;

namespace DMS.DAL.Query;

public sealed class DeviceSearchStep : IDeviceQueryStep
{
    public IQueryable<Device> Apply(IQueryable<Device> query, DeviceQueryParameters parameters)
    {
        if (string.IsNullOrWhiteSpace(parameters.SearchTerm))
            return query;

        // Tokenizăm query-ul: "Dell Latitude" → ["dell", "latitude"]
        var tokens = parameters.SearchTerm
            .ToLower()
            .Split([' ', '-', '_', ',', '.'], StringSplitOptions.RemoveEmptyEntries);

        if (tokens.Length == 0)
            return query;

        // Filtrare în SQL — un device trebuie să conțină cel puțin un token
        // în Name sau Brand (Manufacturer în contextul task-ului)
        foreach (var token in tokens)
        {
            var t = token;
            query = query.Where(d =>
                d.Name.ToLower().Contains(t) ||
                d.Brand.ToLower().Contains(t) ||
                d.Model.ToLower().Contains(t));
        }

        return query;
    }
}