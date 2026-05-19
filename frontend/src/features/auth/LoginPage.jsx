import { LockKeyhole, LogIn } from 'lucide-react';
import { useState } from 'react';

const demoUsers = [
  { role: 'Admin', username: 'admin', password: 'admin123', access: 'Dashboard, tables, orders, reservations' },
  { role: 'Manager', username: 'manager', password: 'manager123', access: 'Tables, orders, reservations' },
  { role: 'Staff', username: 'staff', password: 'staff123', access: 'Tables and order creation' }
];

export function LoginPage({ error, isSigningIn, onLogin }) {
  const [username, setUsername] = useState('admin');
  const [password, setPassword] = useState('admin123');

  function handleSubmit(event) {
    event.preventDefault();
    onLogin({ username, password });
  }

  function fillDemoUser(user) {
    setUsername(user.username);
    setPassword(user.password);
  }

  return (
    <main className="login-shell">
      <section className="login-panel">
        <div className="login-brand">
          <div className="metric-icon">
            <LockKeyhole size={20} />
          </div>
          <p className="eyebrow">Restaurant Management</p>
          <h1>Role based login</h1>
        </div>

        <form className="login-form" onSubmit={handleSubmit}>
          {error && <div className="notice error">{error}</div>}
          <label>
            Username
            <input value={username} onChange={(event) => setUsername(event.target.value)} autoComplete="username" />
          </label>
          <label>
            Password
            <input
              type="password"
              value={password}
              onChange={(event) => setPassword(event.target.value)}
              autoComplete="current-password"
            />
          </label>
          <button className="primary-action" type="submit" disabled={isSigningIn}>
            <LogIn size={18} />
            {isSigningIn ? 'Signing in' : 'Sign in'}
          </button>
        </form>
      </section>

      <section className="login-demo panel">
        <div className="panel-heading">
          <h2>Demo roles</h2>
        </div>
        {demoUsers.map((user) => (
          <button className="role-row" type="button" key={user.role} onClick={() => fillDemoUser(user)}>
            <span>
              <strong>{user.role}</strong>
              <small>{user.access}</small>
            </span>
            <b>{user.username}</b>
          </button>
        ))}
      </section>
    </main>
  );
}
