# Quick Start Guide

This guide will help you get the Billing File API up and running in under 5 minutes.

## Prerequisites

- .NET 8.0 SDK ([Download](https://dotnet.microsoft.com/download/dotnet/8.0))
- SQL Server (LocalDB, Express, or Docker)

## Option 1: Quick Start with Docker (Recommended)

### 1. Clone and run with Docker Compose

```bash
# Clone the repository
git clone <repository-url>
cd billing-file

# Start everything with Docker
docker-compose up -d

# Wait 30 seconds for SQL Server to initialize
# API will be available at http://localhost:5000
```

### 2. Test the API

Open your browser and navigate to:
```
http://localhost:5000/swagger
```

Or test with curl:
```bash
# Get all billing records
curl http://localhost:5000/api/billing

# Get health status
curl http://localhost:5000/health
```

## Option 2: Run Locally with Local SQL Server

### 1. Update connection string

Edit `src/BillingFile.API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=BillingFileDb;Trusted_Connection=true;"
  }
}
```

### 2. Run the application

```bash
# Restore packages and build
dotnet restore
dotnet build

# Run migrations
cd src/BillingFile.API
dotnet ef database update --project ../BillingFile.Infrastructure

# Run the API
dotnet run
```

### 3. Access Swagger UI

Navigate to: `https://localhost:5001/swagger`

## Sample API Calls

### Get All Billing Records

```bash
curl -X GET "https://localhost:5001/api/billing" -H "accept: application/json"
```

### Create a Billing Record

```bash
curl -X POST "https://localhost:5001/api/billing" \
  -H "Content-Type: application/json" \
  -d '{
    "customerName": "Acme Corp",
    "customerEmail": "billing@acme.com",
    "invoiceNumber": "INV-001",
    "invoiceDate": "2024-01-15T00:00:00Z",
    "amount": 1500.00,
    "currency": "USD",
    "status": "Pending",
    "description": "Monthly subscription"
  }'
```

### Get Record by ID

```bash
curl -X GET "https://localhost:5001/api/billing/1" -H "accept: application/json"
```

### Get Records by Status

```bash
curl -X GET "https://localhost:5001/api/billing/status/Pending" -H "accept: application/json"
```

### Update a Record

```bash
curl -X PUT "https://localhost:5001/api/billing/1" \
  -H "Content-Type: application/json" \
  -d '{
    "customerName": "Acme Corp",
    "customerEmail": "billing@acme.com",
    "invoiceNumber": "INV-001",
    "invoiceDate": "2024-01-15T00:00:00Z",
    "amount": 1500.00,
    "currency": "USD",
    "status": "Paid",
    "description": "Monthly subscription - PAID"
  }'
```

### Delete a Record (Soft Delete)

```bash
curl -X DELETE "https://localhost:5001/api/billing/1"
```

## Available Statuses

- `Pending` - Invoice created, awaiting payment
- `Paid` - Invoice has been paid
- `Overdue` - Invoice is past due date
- `Cancelled` - Invoice has been cancelled
- `Refunded` - Payment has been refunded

## Stopping the Application

### Docker:
```bash
docker-compose down
```

### Local:
Press `Ctrl+C` in the terminal

## Next Steps

- Read the full [README.md](README.md) for architecture details
- Review [AZURE-DEPLOYMENT.md](AZURE-DEPLOYMENT.md) for production deployment
- Explore the Swagger UI to test all endpoints
- Check the logs in `logs/` directory

## Troubleshooting

### Port already in use
```bash
# Change ports in docker-compose.yml or launchSettings.json
```

### SQL Server connection failed
```bash
# For Docker: Wait 30-60 seconds for SQL Server to fully start
# For LocalDB: Ensure SQL Server LocalDB is installed
```

### Entity Framework Tools missing
```bash
dotnet tool install --global dotnet-ef
```

## Support

For issues or questions:
1. Check the logs in `logs/` directory
2. Review the full README.md
3. Open an issue on GitHub

