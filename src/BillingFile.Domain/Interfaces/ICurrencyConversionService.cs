namespace BillingFile.Domain.Interfaces;

/// <summary>
/// Service for currency conversion operations
/// </summary>
public interface ICurrencyConversionService
{
    /// <summary>
    /// Get exchange rate for a specific date
    /// </summary>
    /// <param name="fromCurrency">Source currency code (e.g., USD)</param>
    /// <param name="toCurrency">Target currency code (e.g., EUR)</param>
    /// <param name="date">Date for the exchange rate</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Exchange rate</returns>
    Task<decimal> GetExchangeRateAsync(
        string fromCurrency, 
        string toCurrency, 
        DateTime date,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Convert amount from one currency to another
    /// </summary>
    /// <param name="amount">Amount to convert</param>
    /// <param name="fromCurrency">Source currency</param>
    /// <param name="toCurrency">Target currency</param>
    /// <param name="date">Date for exchange rate</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Converted amount</returns>
    Task<decimal> ConvertAmountAsync(
        decimal amount,
        string fromCurrency,
        string toCurrency,
        DateTime date,
        CancellationToken cancellationToken = default);
}

