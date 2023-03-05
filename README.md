# Marketplace HTTP Server

Lightweight HTTP server for marketplace application built with .NET and raw `HttpListener`. Pure vanilla stack - no frameworks on either frontend or backend.

## Features

- **Full vanilla stack**: No frameworks on backend (.NET HttpListener) or frontend (raw HTML/JS/CSS)
- Handles 20+ API endpoints
- Session management with cookies
- SQL Server database integration
- Static file serving for HTML/CSS/JS assets
- Concurrent request processing

## Core Technologies

### Backend
- `.NET 6` with raw `HttpListener`
- `Microsoft.Data.SqlClient` (SQL Server access)
- Async/await pattern

### Frontend
- Pure HTML5 (no templating engines)
- Vanilla JavaScript (no React/Vue/Angular)
- Raw CSS (no Bootstrap/Tailwind)
- Classic DOM manipulation

## Endpoints

### Public Routes
| Path | Description |
|------|-------------|
| `/` | Serves index.html |
| `/register` | User registration |
| `/signIn` | User authentication |
| `/main.css` | Raw CSS styles |
| `/app.js` | Vanilla JS frontend |

### Authenticated Routes
| Path | Description |
|------|-------------|
| `/products` | Product listings (returns HTML) |
| `/myProducts` | User's products (JSON API) |
| `/balancePage` | Serves balance.html |

## Frontend Structure
```plaintext
/static
   ├── index.html    # Pure HTML
   ├── products.html # No templating
   ├── main.css      # Framework-free styles
   └── app.js        # Vanilla ES6 JavaScript