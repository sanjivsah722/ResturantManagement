import { useCallback, useEffect, useMemo, useState } from 'react';
import { restaurantApi } from '../api/restaurantApi.js';

export function useRestaurantOperations(enabled = true) {
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

  const loadData = useCallback(async () => {
    setIsLoading(true);
    setMessage('');

    try {
      const [dashboardData, tableData, menuData, orderData, reservationData] = await Promise.all([
        restaurantApi.getDashboard(),
        restaurantApi.getTables(),
        restaurantApi.getMenu(),
        restaurantApi.getOrders(),
        restaurantApi.getReservations()
      ]);

      setDashboard(dashboardData);
      setTables(tableData);
      setMenu(menuData);
      setOrders(orderData);
      setReservations(reservationData);
    } catch {
      setMessage('Could not reach the API. Start the ASP.NET Core backend on port 5087.');
    } finally {
      setIsLoading(false);
    }
  }, []);

  useEffect(() => {
    if (enabled) {
      loadData();
    }
  }, [enabled, loadData]);

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

  function updateCartQuantity(menuItemId, quantityChange) {
    setCart((current) => ({
      ...current,
      [menuItemId]: (current[menuItemId] || 0) + quantityChange
    }));
  }

  async function updateTableStatus(tableId, status) {
    try {
      await restaurantApi.updateTableStatus(tableId, status);
      await loadData();
    } catch (error) {
      setMessage(error.message);
    }
  }

  async function createOrder(event) {
    event.preventDefault();
    const items = Object.entries(cart)
      .filter(([, quantity]) => quantity > 0)
      .map(([menuItemId, quantity]) => ({ menuItemId: Number(menuItemId), quantity }));

    if (!items.length) {
      setMessage('Add at least one menu item before creating an order.');
      return;
    }

    try {
      await restaurantApi.createOrder({ tableId: Number(selectedTable), customerName, items });
      setCart({});
      setCustomerName('Walk-in');
      setMessage('Order created successfully.');
      await loadData();
    } catch (error) {
      setMessage(error.message);
    }
  }

  return {
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
  };
}
