using Restaurant.Api.Contracts.Auth;

namespace Restaurant.Api.Services;

public interface IAuthService
{
    OperationResult<LoginResponse> Login(LoginRequest request);
}
