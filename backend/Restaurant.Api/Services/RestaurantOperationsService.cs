using Restaurant.Api.Contracts;
using Restaurant.Api.Contracts.Orders;
using Restaurant.Api.Domain.Menu;
using Restaurant.Api.Domain.Orders;
using Restaurant.Api.Domain.Reservations;
using Restaurant.Api.Domain.Tables;
using Restaurant.Api.Repositories;

namespace Restaurant.Api.Services;

public sealed class RestaurantOperationsService(IRestaurantRepository repository) : IRestaurantOperationsService
{
    public DashboardSummary GetDashboard()
    {
        var orders = repository.GetOrders();
        var reservations = repository.GetReservations();
        var tables = repository.GetTables();
        var todayOrders = orders.Where(order => order.CreatedAt.Date == DateTime.Today).ToList();

        return new DashboardSummary(
            RevenueToday: todayOrders.Where(order => order.Status != OrderStatus.Cancelled).Sum(order => order.Total),
            ActiveOrders: todayOrders.Count(order => order.Status is OrderStatus.New or OrderStatus.Preparing or OrderStatus.Served),
            ReservationsToday: reservations.Count(reservation => reservation.ReservationTime.Date == DateTime.Today),
            OccupiedTables: tables.Count(table => table.Status == TableStatus.Occupied),
            TotalTables: tables.Count);
    }

    public IReadOnlyList<RestaurantTable> GetTables() => repository.GetTables();

    public IReadOnlyList<MenuItem> GetMenu() => repository.GetMenu();

    public IReadOnlyList<Order> GetOrders() => repository.GetOrders();

    public IReadOnlyList<Reservation> GetReservations() => repository.GetReservations();

    public RestaurantTable? UpdateTableStatus(int tableId, TableStatus status) => repository.UpdateTableStatus(tableId, status);

    public OperationResult<Order> CreateOrder(CreateOrderRequest request)
    {
        if (request.TableId <= 0 || string.IsNullOrWhiteSpace(request.CustomerName))
        {
            return OperationResult<Order>.Failure("Table and customer name are required.");
        }

        if (!repository.TableExists(request.TableId))
        {
            return OperationResult<Order>.Failure("Selected table was not found.");
        }

        if (request.Items.Count == 0 || request.Items.All(item => item.Quantity <= 0))
        {
            return OperationResult<Order>.Failure("Add at least one item with a valid quantity.");
        }

        if (!repository.MenuItemsExist(request.Items))
        {
            return OperationResult<Order>.Failure("One or more menu items were not found.");
        }

        return OperationResult<Order>.Success(repository.CreateOrder(request));
    }
}
