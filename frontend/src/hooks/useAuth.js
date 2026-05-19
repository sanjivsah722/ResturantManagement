import { useMemo, useState } from 'react';
import { restaurantApi } from '../api/restaurantApi.js';

const AUTH_STORAGE_KEY = 'restaurant:user';

function getStoredUser() {
  const rawUser = localStorage.getItem(AUTH_STORAGE_KEY);

  if (!rawUser) {
    return null;
  }

  try {
    return JSON.parse(rawUser);
  } catch {
    localStorage.removeItem(AUTH_STORAGE_KEY);
    return null;
  }
}

export function useAuth() {
  const [currentUser, setCurrentUser] = useState(getStoredUser);
  const [authError, setAuthError] = useState('');
  const [isSigningIn, setIsSigningIn] = useState(false);

  const isAuthenticated = useMemo(() => Boolean(currentUser), [currentUser]);

  async function login(credentials) {
    setIsSigningIn(true);
    setAuthError('');

    try {
      const user = await restaurantApi.login(credentials);
      localStorage.setItem(AUTH_STORAGE_KEY, JSON.stringify(user));
      setCurrentUser(user);
    } catch {
      setAuthError('Invalid username or password.');
    } finally {
      setIsSigningIn(false);
    }
  }

  function logout() {
    localStorage.removeItem(AUTH_STORAGE_KEY);
    setCurrentUser(null);
  }

  return {
    authError,
    currentUser,
    isAuthenticated,
    isSigningIn,
    login,
    logout
  };
}
