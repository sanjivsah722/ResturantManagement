import { money } from '../../utils/formatters.js';

export function OrdersList({ orders }) {
  return (
    <div className="panel">
      <div className="panel-heading">
        <h2>Orders</h2>
      </div>
      {orders.map((order) => (
        <article className="list-row" key={order.id}>
          <div>
            <strong>#{order.id} - {order.customerName}</strong>
            <span>Table {order.tableId} - {order.status}</span>
          </div>
          <b>{money(order.total)}</b>
        </article>
      ))}
    </div>
  );
}
