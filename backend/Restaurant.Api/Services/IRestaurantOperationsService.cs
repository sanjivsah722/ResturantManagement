using Restaurant.Api.Contracts;
using Restaurant.Api.Contracts.Orders;
using Restaurant.Api.Domain.Menu;
using Restaurant.Api.Domain.Orders;
using Restaurant.Api.Domain.Reservations;
using Restaurant.Api.Domain.Tables;

namespace Restaurant.Api.Services;

public interface IRestaurantOperationsService
{
    DashboardSummary GetDashboard();

    IReadOnlyList<RestaurantTable> GetTables();

    IReadOnlyList<MenuItem> GetMenu();

    IReadOnlyList<Order> GetOrders();

    IReadOnlyList<Reservation> GetReservations();

    RestaurantTable? UpdateTableStatus(int tableId, TableStatus status);

    OperationResult<Order> CreateOrder(CreateOrderRequest request);
}
