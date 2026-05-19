import { CalendarClock, ChefHat, ClipboardList, IndianRupee, LogOut, RefreshCw, Sofa } from 'lucide-react';
import { MetricCard } from '../components/MetricCard.jsx';
import { LoginPage } from '../features/auth/LoginPage.jsx';
import { OrdersList } from '../features/orders/OrdersList.jsx';
import { OrderComposer } from '../features/orders/OrderComposer.jsx';
import { ReservationsList } from '../features/reservations/ReservationsList.jsx';
import { TablesPanel } from '../features/tables/TablesPanel.jsx';
import { useAuth } from '../hooks/useAuth.js';
import { useRestaurantOperations } from '../hooks/useRestaurantOperations.js';
import { money } from '../utils/formatters.js';
import { getPermissions } from '../utils/permissions.js';

export function App() {
  const {
    authError,
    currentUser,
    isAuthenticated,
    isSigningIn,
    login,
    logout
  } = useAuth();
  const {
    cart,
    cartTotal,
    createOrder,
    customerName,
    dashboard,
    groupedMenu,
    isLoading,
    loadData,
    message,
    orders,
    reservations,
    selectedTable,
    setCustomerName,
    setSelectedTable,
    tables,
    updateCartQuantity,
    updateTableStatus
  } = useRestaurantOperations(isAuthenticated);

  if (!isAuthenticated) {
    return <LoginPage error={authError} isSigningIn={isSigningIn} onLogin={login} />;
  }

  const permissions = getPermissions(currentUser.role);

  return (
    <main className="app-shell">
      <header className="topbar">
        <div>
          <p className="eyebrow">Restaurant Management</p>
          <h1>{permissions.canViewDashboard ? 'Operations dashboard' : `${currentUser.role} workspace`}</h1>
          <div className="session-line">
            <span>{currentUser.name}</span>
            <b>{currentUser.role}</b>
          </div>
        </div>
        <div className="topbar-actions">
          <button className="icon-button" onClick={loadData} title="Refresh data" aria-label="Refresh data">
            <RefreshCw size={20} />
          </button>
          <button className="icon-button secondary" onClick={logout} title="Sign out" aria-label="Sign out">
            <LogOut size={20} />
          </button>
        </div>
      </header>

      {message && <div className="notice">{message}</div>}

      {permissions.canViewDashboard && (
        <section className="metrics">
          <MetricCard icon={<IndianRupee />} label="Revenue today" value={dashboard ? money(dashboard.revenueToday) : '-'} />
          <MetricCard icon={<ClipboardList />} label="Active orders" value={dashboard?.activeOrders ?? '-'} />
          <MetricCard icon={<CalendarClock />} label="Reservations" value={dashboard?.reservationsToday ?? '-'} />
          <MetricCard icon={<Sofa />} label="Tables occupied" value={dashboard ? `${dashboard.occupiedTables}/${dashboard.totalTables}` : '-'} />
        </section>
      )}

      <section className="workspace">
        {permissions.canManageTables && (
          <TablesPanel tables={tables} isLoading={isLoading} onStatusChange={updateTableStatus} />
        )}
        {permissions.canCreateOrders && (
          <OrderComposer
            cart={cart}
            cartTotal={cartTotal}
            customerName={customerName}
            groupedMenu={groupedMenu}
            onCustomerNameChange={setCustomerName}
            onQuantityChange={updateCartQuantity}
            onSubmit={createOrder}
            selectedTable={selectedTable}
            tables={tables}
            onSelectedTableChange={setSelectedTable}
            titleIcon={<ChefHat size={20} />}
          />
        )}
      </section>

      <section className="lists">
        {permissions.canViewOrders && <OrdersList orders={orders} />}
        {permissions.canViewReservations && <ReservationsList reservations={reservations} />}
      </section>
    </main>
  );
}
