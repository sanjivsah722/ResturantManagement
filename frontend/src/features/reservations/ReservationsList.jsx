export function ReservationsList({ reservations }) {
  return (
    <div className="panel">
      <div className="panel-heading">
        <h2>Reservations</h2>
      </div>
      {reservations.map((reservation) => (
        <article className="list-row" key={reservation.id}>
          <div>
            <strong>{reservation.customerName}</strong>
            <span>{new Date(reservation.reservationTime).toLocaleString()} - {reservation.guests} guests</span>
          </div>
          <b>{reservation.tableId ? `T${reservation.tableId}` : 'Waitlist'}</b>
        </article>
      ))}
    </div>
  );
}
