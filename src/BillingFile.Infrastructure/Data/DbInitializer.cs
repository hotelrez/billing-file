using BillingFile.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BillingFile.Infrastructure.Data;

/// <summary>
/// Database initializer for seeding sample data
/// </summary>
public static class DbInitializer
{
    public static async Task InitializeAsync(ApplicationDbContext context, ILogger logger)
    {
        try
        {
            // Ensure database is created
            await context.Database.MigrateAsync();

            // Check if data already exists
            if (await context.BillingRecords.AnyAsync())
            {
                logger.LogInformation("Database already contains data. Skipping seeding.");
                return;
            }

            // Seed sample data
            var sampleRecords = new[]
            {
                new BillingRecord
                {
                    CustomerName = "Acme Corporation",
                    CustomerEmail = "billing@acme.com",
                    InvoiceNumber = "INV-2024-001",
                    InvoiceDate = DateTime.UtcNow.AddDays(-30),
                    Amount = 2500.00m,
                    Currency = "USD",
                    Status = BillingStatus.Paid,
                    Description = "Annual software license",
                    PaidDate = DateTime.UtcNow.AddDays(-25)
                },
                new BillingRecord
                {
                    CustomerName = "TechStart Inc",
                    CustomerEmail = "accounts@techstart.com",
                    InvoiceNumber = "INV-2024-002",
                    InvoiceDate = DateTime.UtcNow.AddDays(-15),
                    Amount = 1200.00m,
                    Currency = "USD",
                    Status = BillingStatus.Pending,
                    Description = "Monthly subscription - Premium plan"
                },
                new BillingRecord
                {
                    CustomerName = "Global Systems Ltd",
                    CustomerEmail = "finance@globalsystems.com",
                    InvoiceNumber = "INV-2024-003",
                    InvoiceDate = DateTime.UtcNow.AddDays(-45),
                    Amount = 5000.00m,
                    Currency = "USD",
                    Status = BillingStatus.Overdue,
                    Description = "Consulting services - Q4 2023"
                },
                new BillingRecord
                {
                    CustomerName = "Innovate Labs",
                    CustomerEmail = "billing@innovatelabs.com",
                    InvoiceNumber = "INV-2024-004",
                    InvoiceDate = DateTime.UtcNow.AddDays(-5),
                    Amount = 750.00m,
                    Currency = "USD",
                    Status = BillingStatus.Pending,
                    Description = "API usage fees - January 2024"
                },
                new BillingRecord
                {
                    CustomerName = "Enterprise Solutions",
                    CustomerEmail = "ap@enterprise.com",
                    InvoiceNumber = "INV-2024-005",
                    InvoiceDate = DateTime.UtcNow.AddDays(-20),
                    Amount = 3200.00m,
                    Currency = "USD",
                    Status = BillingStatus.Paid,
                    Description = "Custom development work",
                    PaidDate = DateTime.UtcNow.AddDays(-10)
                }
            };

            await context.BillingRecords.AddRangeAsync(sampleRecords);
            await context.SaveChangesAsync();

            logger.LogInformation("Database seeded successfully with {Count} records", sampleRecords.Length);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database");
            throw;
        }
    }
}

