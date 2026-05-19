import { TABLE_STATUSES } from '../../utils/constants.js';

export function TablesPanel({ tables, isLoading, onStatusChange }) {
  return (
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
            <select value={table.status} onChange={(event) => onStatusChange(table.id, event.target.value)}>
              {TABLE_STATUSES.map((status) => <option key={status}>{status}</option>)}
            </select>
          </article>
        ))}
      </div>
    </div>
  );
}
