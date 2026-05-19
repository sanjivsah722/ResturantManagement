using Restaurant.Api.Domain.Users;

namespace Restaurant.Api.Repositories;

public interface IUserRepository
{
    UserAccount? GetByUsername(string username);
}
