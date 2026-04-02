using DMS.Domain.Entities;
using DMS.Domain.Enums;

namespace DMS.BLL.Tests.Helpers;

internal static class UserBuilder
{
    public static User CreateActive(
        string fullName = "John Doe",
        string email = "john@example.com",
        string passwordHash = "$2a$11$fakehashfakehashfakehashfakehashfakehashfakehashfakehash",
        UserRole role = UserRole.Employee,
        string? location = null)
    {
        return new User(fullName, email, passwordHash, role, location);
    }

    public static User CreateInactive(string email = "inactive@example.com")
    {
        var user = CreateActive(email: email);
        user.Deactivate();
        return user;
    }
}