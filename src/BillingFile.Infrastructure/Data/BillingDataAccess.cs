using BillingFile.Domain.Entities;
using BillingFile.Domain.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BillingFile.Infrastructure.Data;

/// <summary>
/// Implementation of billing data access (stored procedure calls)
/// </summary>
public class BillingDataAccess : IBillingDataAccess
{
    private readonly PlayDbContext _playDbContext;

    public BillingDataAccess(PlayDbContext playDbContext)
    {
        _playDbContext = playDbContext ?? throw new ArgumentNullException(nameof(playDbContext));
    }

    public async Task<List<BillingSpResult>> GetBillingFileReservationsAsync(
        DateTime fromDate, 
        DateTime toDate, 
        CancellationToken cancellationToken = default)
    {
        var fromDateParam = new SqlParameter("@FromDate", fromDate);
        var toDateParam = new SqlParameter("@ToDate", toDate);
        
        return await _playDbContext.BillingSpResults
            .FromSqlRaw("EXEC dbo.GetBillingFileReservations @FromDate, @ToDate", fromDateParam, toDateParam)
            .ToListAsync(cancellationToken);
    }
}

