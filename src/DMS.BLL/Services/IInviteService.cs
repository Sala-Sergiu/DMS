using DMS.BLL.DTOs.Invites;
using DMS.BLL.DTOs.User;

namespace DMS.BLL.Services;

public interface IInviteService
{
    Task<InviteResponseDto> CreateInviteAsync(CreateInviteRequestDto request, Guid createdByUserId, CancellationToken cancellationToken = default);
    Task<UserResponseDto> AcceptInviteAsync(AcceptInviteRequestDto request, CancellationToken cancellationToken = default);
}