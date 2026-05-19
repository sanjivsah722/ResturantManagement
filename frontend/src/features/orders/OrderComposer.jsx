import { Plus } from 'lucide-react';
import { money } from '../../utils/formatters.js';

export function OrderComposer({
  cart,
  cartTotal,
  customerName,
  groupedMenu,
  onCustomerNameChange,
  onQuantityChange,
  onSelectedTableChange,
  onSubmit,
  selectedTable,
  tables,
  titleIcon
}) {
  const itemCount = Object.values(cart).reduce((sum, quantity) => sum + quantity, 0);

  return (
    <form className="panel order-panel" onSubmit={onSubmit}>
      <div className="panel-heading">
        <h2>New order</h2>
        {titleIcon}
      </div>
      <div className="form-row">
        <label>
          Table
          <select value={selectedTable} onChange={(event) => onSelectedTableChange(event.target.value)}>
            {tables.map((table) => <option value={table.id} key={table.id}>{table.name}</option>)}
          </select>
        </label>
        <label>
          Customer
          <input value={customerName} onChange={(event) => onCustomerNameChange(event.target.value)} />
        </label>
      </div>

      <div className="menu-list">
        {Object.entries(groupedMenu).map(([category, items]) => (
          <div key={category}>
            <h3>{category}</h3>
            {items.map((item) => (
              <button type="button" className="menu-row" key={item.id} onClick={() => onQuantityChange(item.id, 1)}>
                <span>
                  <strong>{item.name}</strong>
                  <small>{item.description}</small>
                </span>
                <span>{money(item.price)}</span>
                <Plus size={17} />
              </button>
            ))}
          </div>
        ))}
      </div>

      <div className="cart-summary">
        <span>{itemCount} items</span>
        <strong>{money(cartTotal)}</strong>
        <button type="submit">Create order</button>
      </div>
    </form>
  );
}
