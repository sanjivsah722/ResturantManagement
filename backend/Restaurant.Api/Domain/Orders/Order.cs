namespace Restaurant.Api.Domain.Orders;

public sealed record Order(
    int Id,
    int TableId,
    string CustomerName,
    OrderStatus Status,
    DateTime CreatedAt,
    List<OrderItem> Items)
{
    public decimal Total => Items.Sum(item => item.Total);
}
