namespace Restaurant.Api.Contracts.Orders;

public sealed record CreateOrderRequest(int TableId, string CustomerName, List<CreateOrderItemRequest> Items);
