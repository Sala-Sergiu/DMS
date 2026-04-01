using DMS.BLL.DTOs.User;

namespace DMS.BLL.Services;

public interface IUserService
{
    Task<UserResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<UserResponseDto> GetCurrentUserAsync(Guid currentUserId, CancellationToken cancellationToken = default);
    Task<UserResponseDto> ChangeRoleAsync(Guid userId, ChangeUserRoleRequestDto request, CancellationToken cancellationToken = default);
    Task<UserResponseDto> DeactivateAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<UserResponseDto> ActivateAsync(Guid userId, CancellationToken cancellationToken = default);
}