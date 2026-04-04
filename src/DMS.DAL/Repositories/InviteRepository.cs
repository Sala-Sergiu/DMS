using DMS.DAL.Persistence;
using DMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DMS.DAL.Repositories;

public class InviteRepository : Repository<Invite>, IInviteRepository
{
    public InviteRepository(DmsDbContext context) : base(context) { }

    public async Task<Invite?> GetByTokenAsync(string token, CancellationToken cancellationToken = default)
        => await Context.Invites
            .FirstOrDefaultAsync(i => i.Token == token, cancellationToken);

    public async Task<bool> HasPendingInviteForEmailAsync(string email, CancellationToken cancellationToken = default)
        => await Context.Invites
            .AnyAsync(i => i.Email == email.ToLowerInvariant()
                        && !i.IsUsed
                        && i.ExpiresAtUtc > DateTime.UtcNow,
                      cancellationToken);
}