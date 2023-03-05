# Marketplace HTTP Server

Lightweight HTTP server for marketplace application built with .NET and raw `HttpListener`. No frameworks, pure performance.

## Features

- Handles 20+ API endpoints
- Session management with cookies
- SQL Server database integration
- Static file serving
- Concurrent request processing

## Core Technologies

- `.NET 6`
- `HttpListener` (raw HTTP server)
- `Microsoft.Data.SqlClient` (SQL Server access)
- Async/await pattern

## Endpoints

### Public Routes
| Path | Description |
|------|-------------|
| `/` | Home page |
| `/register` | User registration |
| `/signIn` | User authentication |

### Authenticated Routes
| Path | Description |
|------|-------------|
| `/products` | Product listings |
| `/myProducts` | User's products |
| `/balancePage` | Balance management |
| `/buyProductsPage` | Purchase interface |

## Getting Started

1. **Prerequisites**:
   - SQL Server with `MyDataBase` schema
   - .NET 6 SDK

2. **Configuration**:
   ```csharp
   // Update connection string
   SqlConnection connection = new(@"Data source=YOUR_SERVER;Initial Catalog=MyDataBase;Integrated Security=True");
   
   // Set listening port
   listener.Prefixes.Add("http://localhost:1111/");