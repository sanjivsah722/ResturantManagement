# Restaurant Management

Full-stack restaurant management starter built with ASP.NET Core Web API and React.

## Features

- Dashboard metrics for today's revenue, active orders, reservations, and occupied tables
- Table status management
- Menu catalog grouped by category
- Order creation with line items and totals
- Reservation list
- In-memory sample data for fast local development

## Run the backend

```powershell
cd "backend/Restaurant.Api"
dotnet run
```

The API runs on `http://localhost:5087`.

## Run the frontend

```powershell
cd "frontend"
npm install
npm run dev
```

The React app runs on `http://localhost:5173`.
