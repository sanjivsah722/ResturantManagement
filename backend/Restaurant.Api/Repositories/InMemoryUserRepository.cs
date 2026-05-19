using Restaurant.Api.Domain.Users;

namespace Restaurant.Api.Repositories;

public sealed class InMemoryUserRepository : IUserRepository
{
    private readonly List<UserAccount> _users =
    [
        new(1, "Admin User", "admin", "admin123", UserRole.Admin, true),
        new(2, "Restaurant Manager", "manager", "manager123", UserRole.Manager, true),
        new(3, "Service Staff", "staff", "staff123", UserRole.Staff, true)
    ];

    public UserAccount? GetByUsername(string username)
    {
        return _users.FirstOrDefault(user =>
            user.IsActive &&
            string.Equals(user.Username, username.Trim(), StringComparison.OrdinalIgnoreCase));
    }
}
