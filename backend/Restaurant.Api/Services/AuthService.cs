using Restaurant.Api.Contracts.Auth;
using Restaurant.Api.Repositories;

namespace Restaurant.Api.Services;

public sealed class AuthService(IUserRepository users) : IAuthService
{
    public OperationResult<LoginResponse> Login(LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
        {
            return OperationResult<LoginResponse>.Failure("Username and password are required.");
        }

        var user = users.GetByUsername(request.Username);
        if (user is null || user.Password != request.Password)
        {
            return OperationResult<LoginResponse>.Failure("Invalid username or password.");
        }

        return OperationResult<LoginResponse>.Success(new LoginResponse(user.Id, user.Name, user.Username, user.Role));
    }
}
