namespace Restaurant.Api.Domain.Tables;

public sealed record RestaurantTable(int Id, string Name, int Seats, TableStatus Status);
