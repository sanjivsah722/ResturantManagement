namespace Restaurant.Api.Contracts;

public sealed record DashboardSummary(
    decimal RevenueToday,
    int ActiveOrders,
    int ReservationsToday,
    int OccupiedTables,
    int TotalTables);
