import { apiRequest } from './httpClient.js';

export const restaurantApi = {
  login: (payload) => apiRequest('/auth/login', {
    method: 'POST',
    body: JSON.stringify(payload)
  }),
  getDashboard: () => apiRequest('/dashboard'),
  getTables: () => apiRequest('/tables'),
  getMenu: () => apiRequest('/menu'),
  getOrders: () => apiRequest('/orders'),
  getReservations: () => apiRequest('/reservations'),
  updateTableStatus: (tableId, status) => apiRequest(`/tables/${tableId}/status`, {
    method: 'PATCH',
    body: JSON.stringify({ status })
  }),
  createOrder: (payload) => apiRequest('/orders', {
    method: 'POST',
    body: JSON.stringify(payload)
  })
};
