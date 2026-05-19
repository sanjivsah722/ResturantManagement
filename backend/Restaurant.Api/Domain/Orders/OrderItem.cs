namespace Restaurant.Api.Domain.Orders;

public sealed record OrderItem(int MenuItemId, string Name, int Quantity, decimal UnitPrice)
{
    public decimal Total => Quantity * UnitPrice;
}
