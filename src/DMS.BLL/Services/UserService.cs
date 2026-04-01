using DMS.BLL.DTOs.User;
using DMS.BLL.Exceptions;
using DMS.BLL.Mappers;
using DMS.DAL.UnitOfWork;
using DMS.Domain.Entities;

namespace DMS.BLL.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _uow;

    public UserService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<UserResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await _uow.Users.GetByIdAsync(id, cancellationToken)
            ?? throw NotFoundException.For<User>(id);

        return UserMapper.ToResponseDto(user);
    }

    public async Task<UserResponseDto> GetCurrentUserAsync(Guid currentUserId, CancellationToken cancellationToken = default)
    {
        var user = await _uow.Users.GetByIdAsync(currentUserId, cancellationToken)
            ?? throw NotFoundException.For<User>(currentUserId);

        return UserMapper.ToResponseDto(user);
    }

    public async Task<UserResponseDto> ChangeRoleAsync(
        Guid userId,
        ChangeUserRoleRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var user = await _uow.Users.GetByIdAsync(userId, cancellationToken)
            ?? throw NotFoundException.For<User>(userId);

        user.ChangeRole(request.Role);

        _uow.Users.Update(user);
        await _uow.SaveChangesAsync(cancellationToken);

        return UserMapper.ToResponseDto(user);
    }

    public async Task<UserResponseDto> DeactivateAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await _uow.Users.GetByIdAsync(userId, cancellationToken)
            ?? throw NotFoundException.For<User>(userId);

        user.Deactivate();

        _uow.Users.Update(user);
        await _uow.SaveChangesAsync(cancellationToken);

        return UserMapper.ToResponseDto(user);
    }

    public async Task<UserResponseDto> ActivateAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await _uow.Users.GetByIdAsync(userId, cancellationToken)
            ?? throw NotFoundException.For<User>(userId);

        user.Activate();

        _uow.Users.Update(user);
        await _uow.SaveChangesAsync(cancellationToken);

        return UserMapper.ToResponseDto(user);
    }
}