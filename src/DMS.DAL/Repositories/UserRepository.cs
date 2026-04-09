using DMS.DAL.Persistence;
using DMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DMS.DAL.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(DmsDbContext context) : base(context) { }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        => await Context.Users
            .FirstOrDefaultAsync(u => u.Email == email.ToLowerInvariant(), cancellationToken);

    public async Task<bool> ExistsByEmailAsync(string email, Guid? excludeUserId = null, CancellationToken cancellationToken = default)
        => await Context.Users
            .AnyAsync(u => u.Email == email.ToLowerInvariant() && u.Id != excludeUserId, cancellationToken);

    public async Task<User?> GetWithAssignmentsAsync(Guid id, CancellationToken cancellationToken = default)
        => await Context.Users
            .Include(u => u.Assignments)
                .ThenInclude(a => a.Device)
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
}