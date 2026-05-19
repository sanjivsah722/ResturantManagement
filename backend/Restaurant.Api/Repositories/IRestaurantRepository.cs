using Restaurant.Api.Contracts.Orders;
using Restaurant.Api.Domain.Menu;
using Restaurant.Api.Domain.Orders;
using Restaurant.Api.Domain.Reservations;
using Restaurant.Api.Domain.Tables;

namespace Restaurant.Api.Repositories;

public interface IRestaurantRepository
{
    IReadOnlyList<RestaurantTable> GetTables();

    IReadOnlyList<MenuItem> GetMenu();

    IReadOnlyList<Order> GetOrders();

    IReadOnlyList<Reservation> GetReservations();

    RestaurantTable? UpdateTableStatus(int tableId, TableStatus status);

    Order CreateOrder(CreateOrderRequest request);

    bool TableExists(int tableId);

    bool MenuItemsExist(IEnumerable<CreateOrderItemRequest> items);
}
