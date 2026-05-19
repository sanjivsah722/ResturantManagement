namespace Restaurant.Api.Domain.Users;

public sealed record UserAccount(
    int Id,
    string Name,
    string Username,
    string Password,
    UserRole Role,
    bool IsActive);
