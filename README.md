# Billing File API

A production-ready ASP.NET Core 8 Web API for managing billing records with SQL Server database using Entity Framework Core.

## Architecture

This solution follows Clean Architecture principles with clear separation of concerns:

```
BillingFile/
├── BillingFile.API/          # Web API layer (Controllers, Middleware)
├── BillingFile.Application/  # Business logic layer (Services, DTOs)
├── BillingFile.Domain/        # Domain layer (Entities, Interfaces)
└── BillingFile.Infrastructure/# Data access layer (EF Core, Repositories)
```

## Design Patterns & Best Practices

- **Clean Architecture**: Separation of concerns with dependency inversion
- **Repository Pattern**: Abstraction of data access logic
- **Unit of Work Pattern**: Transaction management across multiple repositories
- **Dependency Injection**: Loose coupling and testability
- **DTOs**: Separation between domain models and API contracts
- **AutoMapper**: Object-to-object mapping
- **Result Pattern**: Consistent error handling
- **Soft Delete**: Data preservation with IsDeleted flag
- **Audit Fields**: Automatic CreatedAt/UpdatedAt timestamps
- **Async/Await**: Non-blocking I/O operations
- **Logging**: Structured logging with Serilog

## Security Features

- **HTTPS**: Enforced secure communication
- **Connection String Security**: Use Azure Key Vault in production
- **SQL Injection Prevention**: Parameterized queries via EF Core
- **Model Validation**: Input validation on DTOs
- **Error Handling**: No sensitive information in error responses
- **CORS**: Configurable cross-origin resource sharing

## Prerequisites

- .NET 8.0 SDK
- SQL Server (LocalDB, Express, or Full)
- Visual Studio 2022 / VS Code / Rider

## Getting Started

### 1. Clone the repository

```bash
git clone <repository-url>
cd billing-file
```

### 2. Update the connection string

Edit `src/BillingFile.API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=BillingFileDb;Trusted_Connection=true;TrustServerCertificate=true"
  }
}
```

### 3. Create the database

```bash
cd src/BillingFile.API
dotnet ef migrations add InitialCreate --project ../BillingFile.Infrastructure
dotnet ef database update
```

### 4. Run the application

```bash
dotnet run
```

The API will be available at:
- HTTP: `http://localhost:5000`
- HTTPS: `https://localhost:5001`
- Swagger UI: `https://localhost:5001/swagger`

## API Endpoints

### Billing Records

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/billing` | Get all billing records |
| GET | `/api/billing/{id}` | Get billing record by ID |
| GET | `/api/billing/status/{status}` | Get records by status |
| POST | `/api/billing` | Create new billing record |
| PUT | `/api/billing/{id}` | Update billing record |
| DELETE | `/api/billing/{id}` | Delete billing record (soft delete) |

### Health Check

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/health` | API and database health status |

## Sample Requests

### Create Billing Record

```json
POST /api/billing
{
  "customerName": "John Doe",
  "customerEmail": "john.doe@example.com",
  "invoiceNumber": "INV-001",
  "invoiceDate": "2024-01-15T00:00:00Z",
  "amount": 1500.00,
  "currency": "USD",
  "status": "Pending",
  "description": "Monthly subscription fee"
}
```

### Get All Records

```bash
GET /api/billing
```

## Project Structure Details

### Domain Layer (`BillingFile.Domain`)
- **Entities**: Domain models (BillingRecord, BaseEntity)
- **Interfaces**: Repository and Unit of Work contracts
- **Enums**: BillingStatus enumeration

### Infrastructure Layer (`BillingFile.Infrastructure`)
- **Data**: DbContext and entity configurations
- **Repositories**: Generic repository and UnitOfWork implementations
- **Migrations**: EF Core database migrations

### Application Layer (`BillingFile.Application`)
- **Services**: Business logic implementation
- **Interfaces**: Service contracts
- **DTOs**: Data transfer objects
- **Mappings**: AutoMapper profiles
- **Common**: Shared classes (Result pattern)

### API Layer (`BillingFile.API`)
- **Controllers**: REST API endpoints
- **Program.cs**: Application configuration and startup
- **appsettings.json**: Configuration settings

## Deployment to Azure

See [AZURE-DEPLOYMENT.md](AZURE-DEPLOYMENT.md) for detailed deployment instructions.

### Quick Azure Deployment

```bash
# Login to Azure
az login

# Create resources (see AZURE-DEPLOYMENT.md)
# Deploy using Azure DevOps, GitHub Actions, or Visual Studio
```

## Development

### Adding a New Migration

```bash
cd src/BillingFile.API
dotnet ef migrations add MigrationName --project ../BillingFile.Infrastructure
dotnet ef database update
```

### Running Tests

```bash
dotnet test
```

## Technologies

- **ASP.NET Core 8**: Web API framework
- **Entity Framework Core 8**: ORM for data access
- **SQL Server**: Relational database
- **AutoMapper**: Object mapping
- **Serilog**: Structured logging
- **Swashbuckle**: Swagger/OpenAPI documentation
- **Azure SDK**: Azure service integration

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## License

This project is licensed under the MIT License.

