using DMS.Domain.Entities;

namespace DMS.DAL.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> ExistsByEmailAsync(string email, Guid? excludeUserId = null, CancellationToken cancellationToken = default);
    Task<User?> GetWithAssignmentsAsync(Guid id, CancellationToken cancellationToken = default);
}