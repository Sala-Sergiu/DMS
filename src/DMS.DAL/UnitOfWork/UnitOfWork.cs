using DMS.DAL.Persistence;
using DMS.DAL.Repositories;

namespace DMS.DAL.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly DmsDbContext _context;

    public IDeviceRepository Devices { get; }
    public IUserRepository Users { get; }
    public IInviteRepository Invites { get; }

    public UnitOfWork(DmsDbContext context, IDeviceRepository devices, IUserRepository users, IInviteRepository invites)
    {
        _context = context;
        Devices = devices;
        Users = users;
        Invites = invites;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);

    public void Dispose()
        => _context.Dispose();
}