namespace Restaurant.Api;

public enum TableStatus
{
    Available,
    Occupied,
    Reserved,
    Cleaning
}

public enum OrderStatus
{
    New,
    Preparing,
    Served,
    Paid,
    Cancelled
}

public sealed record RestaurantTable(int Id, string Name, int Seats, TableStatus Status);

public sealed record MenuItem(int Id, string Name, string Category, decimal Price, bool IsAvailable, string Description);

public sealed record OrderItem(int MenuItemId, string Name, int Quantity, decimal UnitPrice)
{
    public decimal Total => Quantity * UnitPrice;
}

public sealed record Order(int Id, int TableId, string CustomerName, OrderStatus Status, DateTime CreatedAt, List<OrderItem> Items)
{
    public decimal Total => Items.Sum(item => item.Total);
}

public sealed record Reservation(int Id, string CustomerName, string Phone, int Guests, DateTime ReservationTime, int? TableId);

public sealed record CreateOrderRequest(int TableId, string CustomerName, List<CreateOrderItemRequest> Items);

public sealed record CreateOrderItemRequest(int MenuItemId, int Quantity);

public sealed record UpdateTableStatusRequest(TableStatus Status);
