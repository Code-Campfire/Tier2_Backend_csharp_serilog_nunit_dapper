# Tier 2 Backend - C# with Serilog, nUnit, and Dapper

This is a starter template for a C# (.NET) backend with Serilog for logging, nUnit for testing, and Dapper as a lightweight ORM.

## Technologies Used

- [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) - Framework
- [ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core) - Web API framework
- [Serilog](https://serilog.net/) - Structured logging
- [Dapper](https://github.com/DapperLib/Dapper) - Lightweight ORM
- [nUnit](https://nunit.org/) - Testing framework
- [SQLite](https://www.sqlite.org/) - Database

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

### Installation

1. Clone the repository

2. Run the appropriate setup script from the project root:

   **For Mac/Linux:**
   ```
   # Run from the project root
   sh scripts/setup-mac.sh
   ```

   **For Windows:**
   ```batch
   # Run from the project root
   scripts\setup-windows.bat
   ```

   This will:
   - Check if you have .NET 8 installed (and provide download link if needed)
   - Restore packages
   - Build the solution
   - Run tests
   - Start and stop the API to verify it works

3. Alternatively, you can set up the project manually:

   **For Mac/Linux:**
   ```
   # Make sure you're in the project root directory
   
   # Restore packages
   dotnet restore
   
   # Build the solution
   dotnet build
   
   # Run tests
   dotnet test
   ```

   **For Windows:**
   ```batch
   # Make sure you're in the project root directory
   
   :: Restore packages
   dotnet restore
   
   :: Build the solution
   dotnet build
   
   :: Run tests
   dotnet test
   ```

### Development

Run the API:

**For Mac/Linux:**
```
cd src/CodeFire.API
dotnet run
```

**For Windows:**
```
cd src\CodeFire.API
dotnet run
```

The API will be available at:
- https://localhost:5001
- http://localhost:5000

### Project Structure

```
/
├── src/                         # Source code
│   ├── CodeFire.API/            # API project (controllers, middleware)
│   ├── CodeFire.Core/           # Core project (domain models, interfaces)
│   └── CodeFire.Infrastructure/ # Infrastructure project (data access, external services)
├── tests/                       # Test projects
│   └── CodeFire.UnitTests/      # Unit tests
└── scripts/                     # Setup scripts
```

## API Endpoints

### WeatherForecast

- `GET /api/weatherforecast` - Get random weather forecast data (sample endpoint)

### Todo

- `GET /api/todo` - Get all todo items
- `GET /api/todo/{id}` - Get a specific todo item
- `POST /api/todo` - Create a new todo item
- `PUT /api/todo/{id}` - Update a todo item
- `DELETE /api/todo/{id}` - Delete a todo item

## Architecture

This project follows a clean architecture approach with three main layers:

1. **API Layer** (CodeFire.API): Contains controllers, middleware, and API configurations. Handles HTTP requests and responses.

2. **Core Layer** (CodeFire.Core): Contains domain models, interfaces, and business logic. This layer is independent of external frameworks.

3. **Infrastructure Layer** (CodeFire.Infrastructure): Contains implementations of interfaces defined in the Core layer, such as repositories, database access, and external service clients.

## Logging

Logs are generated using Serilog and are stored in the `logs` directory. The logging configuration can be adjusted in the `SerilogExtensions.cs` file and `appsettings.json`.

## Testing

The project uses nUnit for unit testing. Tests can be run with:

```
dotnet test
```

## Database

The application uses SQLite as a database, which is a file-based database that doesn't require a separate server. The database file is created automatically when the application is first run.

## Further Documentation

- [ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core)
- [Serilog Documentation](https://github.com/serilog/serilog/wiki)
- [Dapper Documentation](https://github.com/DapperLib/Dapper)
- [nUnit Documentation](https://docs.nunit.org/) 