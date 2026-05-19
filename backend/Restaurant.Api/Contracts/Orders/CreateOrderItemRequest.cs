namespace Restaurant.Api.Contracts.Orders;

public sealed record CreateOrderItemRequest(int MenuItemId, int Quantity);
