using DMS.Domain.Entities;

namespace DMS.DAL.Repositories;

public interface IInviteRepository : IRepository<Invite>
{
    Task<Invite?> GetByTokenAsync(string token, CancellationToken cancellationToken = default);
    Task<bool> HasPendingInviteForEmailAsync(string email, CancellationToken cancellationToken = default);
}
