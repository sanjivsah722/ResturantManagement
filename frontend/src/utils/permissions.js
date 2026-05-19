export const ROLE_PERMISSIONS = {
  Admin: {
    canViewDashboard: true,
    canManageTables: true,
    canCreateOrders: true,
    canViewOrders: true,
    canViewReservations: true
  },
  Manager: {
    canViewDashboard: false,
    canManageTables: true,
    canCreateOrders: true,
    canViewOrders: true,
    canViewReservations: true
  },
  Staff: {
    canViewDashboard: false,
    canManageTables: true,
    canCreateOrders: true,
    canViewOrders: true,
    canViewReservations: false
  }
};

export function getPermissions(role) {
  return ROLE_PERMISSIONS[role] ?? ROLE_PERMISSIONS.Staff;
}
