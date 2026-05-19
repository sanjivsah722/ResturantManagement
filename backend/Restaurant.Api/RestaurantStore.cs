namespace Restaurant.Api;

public sealed class RestaurantStore
{
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

    public IReadOnlyList<RestaurantTable> Tables => _tables;

    public IReadOnlyList<MenuItem> Menu => _menu;

    public IReadOnlyList<Order> Orders => _orders.OrderByDescending(order => order.CreatedAt).ToList();

    public IReadOnlyList<Reservation> Reservations => _reservations.OrderBy(reservation => reservation.ReservationTime).ToList();

    public object GetDashboard()
    {
        var todayOrders = _orders.Where(order => order.CreatedAt.Date == DateTime.Today).ToList();

        return new
        {
            revenueToday = todayOrders.Where(order => order.Status != OrderStatus.Cancelled).Sum(order => order.Total),
            activeOrders = todayOrders.Count(order => order.Status is OrderStatus.New or OrderStatus.Preparing or OrderStatus.Served),
            reservationsToday = _reservations.Count(reservation => reservation.ReservationTime.Date == DateTime.Today),
            occupiedTables = _tables.Count(table => table.Status == TableStatus.Occupied),
            totalTables = _tables.Count
        };
    }

    public RestaurantTable? UpdateTableStatus(int tableId, TableStatus status)
    {
        var index = _tables.FindIndex(table => table.Id == tableId);
        if (index < 0)
        {
            return null;
        }

        _tables[index] = _tables[index] with { Status = status };
        return _tables[index];
    }

    public Order CreateOrder(CreateOrderRequest request)
    {
        var items = request.Items
            .Where(item => item.Quantity > 0)
            .Select(item =>
            {
                var menuItem = _menu.Single(menu => menu.Id == item.MenuItemId);
                return new OrderItem(menuItem.Id, menuItem.Name, item.Quantity, menuItem.Price);
            })
            .ToList();

        var order = new Order(_nextOrderId++, request.TableId, request.CustomerName, OrderStatus.New, DateTime.Now, items);
        _orders.Add(order);
        UpdateTableStatus(request.TableId, TableStatus.Occupied);
        return order;
    }

    public bool MenuItemsExist(IEnumerable<CreateOrderItemRequest> items)
    {
        var menuIds = _menu.Select(menuItem => menuItem.Id).ToHashSet();
        return items.All(item => menuIds.Contains(item.MenuItemId));
    }
}
