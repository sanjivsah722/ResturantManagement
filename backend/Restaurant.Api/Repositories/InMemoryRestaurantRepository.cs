using Restaurant.Api.Contracts.Orders;
using Restaurant.Api.Domain.Menu;
using Restaurant.Api.Domain.Orders;
using Restaurant.Api.Domain.Reservations;
using Restaurant.Api.Domain.Tables;

namespace Restaurant.Api.Repositories;

public sealed class InMemoryRestaurantRepository : IRestaurantRepository
{
    private readonly Lock _lock = new();
    private readonly List<RestaurantTable> _tables =
    [
        new(1, "T1", 2, TableStatus.Available),
        new(2, "T2", 4, TableStatus.Occupied),
        new(3, "T3", 4, TableStatus.Reserved),
        new(4, "T4", 6, TableStatus.Available),
        new(5, "Patio 1", 2, TableStatus.Cleaning),
        new(6, "Family", 8, TableStatus.Occupied)
    ];

    private readonly List<MenuItem> _menu =
    [
        new(1, "Paneer Tikka", "Starters", 249, true, "Charred cottage cheese with peppers"),
        new(2, "Chicken 65", "Starters", 279, true, "Crisp spicy chicken bites"),
        new(3, "Veg Biryani", "Mains", 299, true, "Aromatic rice with seasonal vegetables"),
        new(4, "Butter Chicken", "Mains", 389, true, "Creamy tomato gravy with tandoori chicken"),
        new(5, "Masala Dosa", "Mains", 189, true, "Crisp dosa with potato masala"),
        new(6, "Gulab Jamun", "Desserts", 129, true, "Warm syrup-soaked dumplings"),
        new(7, "Fresh Lime Soda", "Beverages", 99, true, "Sweet, salted, or mixed")
    ];

    private readonly List<Order> _orders =
    [
        new(1001, 2, "Walk-in", OrderStatus.Preparing, DateTime.Today.AddHours(12).AddMinutes(15),
        [
            new(1, "Paneer Tikka", 1, 249),
            new(3, "Veg Biryani", 2, 299)
        ]),
        new(1002, 6, "Aarav Mehta", OrderStatus.Served, DateTime.Today.AddHours(13).AddMinutes(5),
        [
            new(4, "Butter Chicken", 2, 389),
            new(7, "Fresh Lime Soda", 4, 99)
        ])
    ];

    private readonly List<Reservation> _reservations =
    [
        new(501, "Nisha Rao", "98765 43210", 4, DateTime.Today.AddHours(19), 3),
        new(502, "Karan Shah", "91234 56780", 2, DateTime.Today.AddHours(20).AddMinutes(30), null),
        new(503, "Priya Iyer", "99887 76655", 6, DateTime.Today.AddDays(1).AddHours(18), 4)
    ];

    private int _nextOrderId = 1003;

    public IReadOnlyList<RestaurantTable> GetTables() => _tables;

    public IReadOnlyList<MenuItem> GetMenu() => _menu;

    public IReadOnlyList<Order> GetOrders() => _orders.OrderByDescending(order => order.CreatedAt).ToList();

    public IReadOnlyList<Reservation> GetReservations() => _reservations.OrderBy(reservation => reservation.ReservationTime).ToList();

    public RestaurantTable? UpdateTableStatus(int tableId, TableStatus status)
    {
        lock (_lock)
        {
            return SetTableStatus(tableId, status);
        }
    }

    public Order CreateOrder(CreateOrderRequest request)
    {
        lock (_lock)
        {
            var items = request.Items
                .Where(item => item.Quantity > 0)
                .Select(item =>
                {
                    var menuItem = _menu.Single(menu => menu.Id == item.MenuItemId);
                    return new OrderItem(menuItem.Id, menuItem.Name, item.Quantity, menuItem.Price);
                })
                .ToList();

            var order = new Order(_nextOrderId++, request.TableId, request.CustomerName.Trim(), OrderStatus.New, DateTime.Now, items);
            _orders.Add(order);
            SetTableStatus(request.TableId, TableStatus.Occupied);

            return order;
        }
    }

    public bool TableExists(int tableId) => _tables.Any(table => table.Id == tableId);

    public bool MenuItemsExist(IEnumerable<CreateOrderItemRequest> items)
    {
        var menuIds = _menu.Select(menuItem => menuItem.Id).ToHashSet();
        return items.All(item => menuIds.Contains(item.MenuItemId));
    }

    private RestaurantTable? SetTableStatus(int tableId, TableStatus status)
    {
        var index = _tables.FindIndex(table => table.Id == tableId);
        if (index < 0)
        {
            return null;
        }

        _tables[index] = _tables[index] with { Status = status };
        return _tables[index];
    }
}
