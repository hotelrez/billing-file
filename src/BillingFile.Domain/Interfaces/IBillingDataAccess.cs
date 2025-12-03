using BillingFile.Domain.Entities;

namespace BillingFile.Domain.Interfaces;

/// <summary>
/// Interface for billing data access operations (stored procedures)
/// </summary>
public interface IBillingDataAccess
{
    /// <summary>
    /// Execute GetBillingFileReservations stored procedure
    /// </summary>
    /// <param name="fromDate">Start date</param>
    /// <param name="toDate">End date</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of billing records from stored procedure</returns>
    Task<List<BillingSpResult>> GetBillingFileReservationsAsync(
        DateTime fromDate, 
        DateTime toDate, 
        CancellationToken cancellationToken = default);
}

