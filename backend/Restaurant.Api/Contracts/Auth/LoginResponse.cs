using Restaurant.Api.Domain.Users;

namespace Restaurant.Api.Contracts.Auth;

public sealed record LoginResponse(int Id, string Name, string Username, UserRole Role);
