using DMS.BLL.DTOs.User;

namespace DMS.BLL.Services;

public interface IUserService
{
    Task<UserResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<UserResponseDto> GetCurrentUserAsync(Guid currentUserId, CancellationToken cancellationToken = default);
}