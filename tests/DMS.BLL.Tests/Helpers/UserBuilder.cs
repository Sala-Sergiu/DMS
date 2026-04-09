using DMS.Domain.Entities;

namespace DMS.BLL.Tests.Helpers;

internal static class UserBuilder
{
    public static User CreateActive(
        string fullName = "John Doe",
        string email = "john@example.com",
        string passwordHash = "$2a$11$fakehashfakehashfakehashfakehashfakehashfakehashfakehash",
        string? location = null)
    {
        return new User(fullName, email, passwordHash, location);
    }
}