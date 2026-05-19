namespace Restaurant.Api.Domain.Menu;

public sealed record MenuItem(
    int Id,
    string Name,
    string Category,
    decimal Price,
    bool IsAvailable,
    string Description);
