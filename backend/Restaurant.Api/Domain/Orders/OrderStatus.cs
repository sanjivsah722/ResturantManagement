namespace Restaurant.Api.Domain.Orders;

public enum OrderStatus
{
    New,
    Preparing,
    Served,
    Paid,
    Cancelled
}
