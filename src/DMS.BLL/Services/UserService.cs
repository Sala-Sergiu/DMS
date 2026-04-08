using DMS.BLL.DTOs.User;
using DMS.BLL.Exceptions;
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
        var user = await _uow.Users.GetWithAssignmentsAsync(id, cancellationToken)
            ?? throw NotFoundException.For<User>(id);

        return UserMapper.ToResponseDto(user);
    }

    public async Task<UserResponseDto> GetCurrentUserAsync(Guid currentUserId, CancellationToken cancellationToken = default)
    {
        var user = await _uow.Users.GetWithAssignmentsAsync(currentUserId, cancellationToken)
            ?? throw NotFoundException.For<User>(currentUserId);

        return UserMapper.ToResponseDto(user);
    }
}