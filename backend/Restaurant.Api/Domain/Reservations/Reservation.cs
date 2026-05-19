namespace Restaurant.Api.Domain.Reservations;

public sealed record Reservation(
    int Id,
    string CustomerName,
    string Phone,
    int Guests,
    DateTime ReservationTime,
    int? TableId);
