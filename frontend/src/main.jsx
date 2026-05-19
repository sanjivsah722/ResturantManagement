import React, { useEffect, useMemo, useState } from 'react';
import { createRoot } from 'react-dom/client';
import { CalendarClock, ChefHat, ClipboardList, IndianRupee, Plus, RefreshCw, Sofa } from 'lucide-react';
import './styles.css';

const API_URL = 'http://localhost:5087/api';

const tableStatuses = ['Available', 'Occupied', 'Reserved', 'Cleaning'];

function money(value) {
  return new Intl.NumberFormat('en-IN', { style: 'currency', currency: 'INR', maximumFractionDigits: 0 }).format(value);
}

function App() {
  const [dashboard, setDashboard] = useState(null);
  const [tables, setTables] = useState([]);
  const [menu, setMenu] = useState([]);
  const [orders, setOrders] = useState([]);
  const [reservations, setReservations] = useState([]);
  const [selectedTable, setSelectedTable] = useState(1);
  const [customerName, setCustomerName] = useState('Walk-in');
  const [cart, setCart] = useState({});
  const [isLoading, setIsLoading] = useState(true);
  const [message, setMessage] = useState('');

  async function loadData() {
    setIsLoading(true);
    const [dashboardData, tableData, menuData, orderData, reservationData] = await Promise.all([
      fetch(`${API_URL}/dashboard`).then((response) => response.json()),
      fetch(`${API_URL}/tables`).then((response) => response.json()),
      fetch(`${API_URL}/menu`).then((response) => response.json()),
      fetch(`${API_URL}/orders`).then((response) => response.json()),
      fetch(`${API_URL}/reservations`).then((response) => response.json())
    ]);

    setDashboard(dashboardData);
    setTables(tableData);
    setMenu(menuData);
    setOrders(orderData);
    setReservations(reservationData);
    setIsLoading(false);
  }

  useEffect(() => {
    loadData().catch(() => {
      setMessage('Could not reach the API. Start the ASP.NET Core backend on port 5087.');
      setIsLoading(false);
    });
  }, []);

  const groupedMenu = useMemo(() => {
    return menu.reduce((groups, item) => {
      groups[item.category] = groups[item.category] || [];
      groups[item.category].push(item);
      return groups;
    }, {});
  }, [menu]);

  const cartTotal = useMemo(() => {
    return Object.entries(cart).reduce((total, [menuItemId, quantity]) => {
      const item = menu.find((menuItem) => menuItem.id === Number(menuItemId));
      return total + (item ? item.price * quantity : 0);
    }, 0);
  }, [cart, menu]);

  function addToCart(menuItemId) {
    setCart((current) => ({ ...current, [menuItemId]: (current[menuItemId] || 0) + 1 }));
  }

  async function updateTableStatus(tableId, status) {
    await fetch(`${API_URL}/tables/${tableId}/status`, {
      method: 'PATCH',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ status })
    });
    await loadData();
  }

  async function createOrder(event) {
    event.preventDefault();
    const items = Object.entries(cart).map(([menuItemId, quantity]) => ({ menuItemId: Number(menuItemId), quantity }));

    if (!items.length) {
      setMessage('Add at least one menu item before creating an order.');
      return;
    }

    const response = await fetch(`${API_URL}/orders`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ tableId: Number(selectedTable), customerName, items })
    });

    if (!response.ok) {
      setMessage(await response.text());
      return;
    }

    setCart({});
    setCustomerName('Walk-in');
    setMessage('Order created successfully.');
    await loadData();
  }

  return (
    <main className="app-shell">
      <header className="topbar">
        <div>
          <p className="eyebrow">Restaurant Management</p>
          <h1>Operations dashboard</h1>
        </div>
        <button className="icon-button" onClick={loadData} title="Refresh data" aria-label="Refresh data">
          <RefreshCw size={20} />
        </button>
      </header>

      {message && <div className="notice">{message}</div>}

      <section className="metrics">
        <Metric icon={<IndianRupee />} label="Revenue today" value={dashboard ? money(dashboard.revenueToday) : '-'} />
        <Metric icon={<ClipboardList />} label="Active orders" value={dashboard?.activeOrders ?? '-'} />
        <Metric icon={<CalendarClock />} label="Reservations" value={dashboard?.reservationsToday ?? '-'} />
        <Metric icon={<Sofa />} label="Tables occupied" value={dashboard ? `${dashboard.occupiedTables}/${dashboard.totalTables}` : '-'} />
      </section>

      <section className="workspace">
        <div className="panel table-panel">
          <div className="panel-heading">
            <h2>Tables</h2>
            <span>{isLoading ? 'Loading' : `${tables.length} tables`}</span>
          </div>
          <div className="table-grid">
            {tables.map((table) => (
              <article className={`table-card ${table.status.toLowerCase()}`} key={table.id}>
                <div>
                  <strong>{table.name}</strong>
                  <span>{table.seats} seats</span>
                </div>
                <select value={table.status} onChange={(event) => updateTableStatus(table.id, event.target.value)}>
                  {tableStatuses.map((status) => <option key={status}>{status}</option>)}
                </select>
              </article>
            ))}
          </div>
        </div>

        <form className="panel order-panel" onSubmit={createOrder}>
          <div className="panel-heading">
            <h2>New order</h2>
            <ChefHat size={20} />
          </div>
          <div className="form-row">
            <label>
              Table
              <select value={selectedTable} onChange={(event) => setSelectedTable(event.target.value)}>
                {tables.map((table) => <option value={table.id} key={table.id}>{table.name}</option>)}
              </select>
            </label>
            <label>
              Customer
              <input value={customerName} onChange={(event) => setCustomerName(event.target.value)} />
            </label>
          </div>

          <div className="menu-list">
            {Object.entries(groupedMenu).map(([category, items]) => (
              <div key={category}>
                <h3>{category}</h3>
                {items.map((item) => (
                  <button type="button" className="menu-row" key={item.id} onClick={() => addToCart(item.id)}>
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
            <span>{Object.values(cart).reduce((sum, quantity) => sum + quantity, 0)} items</span>
            <strong>{money(cartTotal)}</strong>
            <button type="submit">Create order</button>
          </div>
        </form>
      </section>

      <section className="lists">
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
      </section>
    </main>
  );
}

function Metric({ icon, label, value }) {
  return (
    <article className="metric-card">
      <div className="metric-icon">{icon}</div>
      <span>{label}</span>
      <strong>{value}</strong>
    </article>
  );
}

createRoot(document.getElementById('root')).render(<App />);
