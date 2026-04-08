using DMS.DAL.Repositories;

namespace DMS.DAL.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    IDeviceRepository Devices { get; }
    IUserRepository Users { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}