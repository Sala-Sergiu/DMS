using DMS.BLL.DTOs.Invites;
using DMS.BLL.DTOs.User;
using DMS.BLL.Exceptions;
using DMS.BLL.Mappers;
using DMS.DAL.UnitOfWork;
using DMS.Domain.Entities;

namespace DMS.BLL.Services;

public class InviteService : IInviteService
{
    private const int InviteExpirationHours = 24;

    private readonly IUnitOfWork _uow;

    public InviteService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<InviteResponseDto> CreateInviteAsync(
        CreateInviteRequestDto request,
        Guid createdByUserId,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
            throw new ValidationException("Email is required.");

        if (await _uow.Users.ExistsByEmailAsync(request.Email, cancellationToken: cancellationToken))
            throw new ConflictException($"A user with email '{request.Email}' already exists.");

        if (await _uow.Invites.HasPendingInviteForEmailAsync(request.Email, cancellationToken))
            throw new ConflictException($"A pending invite for '{request.Email}' already exists.");

        var token = Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N"); // 64-char raw token
        var expiresAt = DateTime.UtcNow.AddHours(InviteExpirationHours);

        var invite = new Invite(request.Email, token, request.Role, createdByUserId, expiresAt);

        await _uow.Invites.AddAsync(invite, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        return MapToResponseDto(invite);
    }

    public async Task<UserResponseDto> AcceptInviteAsync(
        AcceptInviteRequestDto request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Token))
            throw new ValidationException("Invite token is required.");

        if (string.IsNullOrWhiteSpace(request.FullName))
            throw new ValidationException("Full name is required.");

        if (string.IsNullOrWhiteSpace(request.Password) || request.Password.Length < 6)
            throw new ValidationException("Password must be at least 6 characters.");

        var invite = await _uow.Invites.GetByTokenAsync(request.Token, cancellationToken)
            ?? throw new NotFoundException("Invite not found or invalid.");

        if (invite.IsUsed)
            throw new ConflictException("This invite has already been used.");

        if (invite.IsExpired())
            throw new ConflictException("This invite has expired.");

        if (await _uow.Users.ExistsByEmailAsync(invite.Email, cancellationToken: cancellationToken))
            throw new ConflictException($"A user with email '{invite.Email}' already exists.");

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var user = new User(request.FullName, invite.Email, passwordHash, invite.Role, request.Location);

        invite.MarkAsUsed();

        await _uow.Users.AddAsync(user, cancellationToken);
        _uow.Invites.Update(invite);
        await _uow.SaveChangesAsync(cancellationToken);

        return UserMapper.ToResponseDto(user);
    }

    private static InviteResponseDto MapToResponseDto(Invite invite) => new()
    {
        Id = invite.Id,
        Email = invite.Email,
        Role = invite.Role,
        ExpiresAtUtc = invite.ExpiresAtUtc,
        IsUsed = invite.IsUsed
    };
}