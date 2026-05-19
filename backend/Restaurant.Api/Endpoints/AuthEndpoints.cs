using Restaurant.Api.Contracts.Auth;
using Restaurant.Api.Services;

namespace Restaurant.Api.Endpoints;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth");

        group.MapPost("/login", (LoginRequest request, IAuthService service) =>
        {
            var result = service.Login(request);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.Unauthorized();
        });

        return app;
    }
}
