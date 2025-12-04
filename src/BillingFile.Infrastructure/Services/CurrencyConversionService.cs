using BillingFile.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace BillingFile.Infrastructure.Services;

/// <summary>
/// Currency conversion service using Exchange Rate API
/// Uses exchangerate-api.com (free tier) for historical exchange rates
/// </summary>
public class CurrencyConversionService : ICurrencyConversionService
{
    private readonly ILogger<CurrencyConversionService> _logger;
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://api.exchangerate-api.com/v4/latest/";

    public CurrencyConversionService(
        ILogger<CurrencyConversionService> logger,
        HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
    }

    public async Task<decimal> GetExchangeRateAsync(
        string fromCurrency,
        string toCurrency,
        DateTime date,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // If same currency, return 1
            if (fromCurrency.Equals(toCurrency, StringComparison.OrdinalIgnoreCase))
            {
                return 1.0m;
            }

            _logger.LogInformation(
                "Fetching exchange rate from {FromCurrency} to {ToCurrency} for date {Date}",
                fromCurrency, toCurrency, date);

            // Note: Free API doesn't support historical dates, using latest rates
            // For production, use a paid API that supports historical data
            var url = $"{BaseUrl}{fromCurrency}";
            
            var response = await _httpClient.GetAsync(url, cancellationToken);
            response.EnsureSuccessStatusCode();
            
            var json = await response.Content.ReadAsStringAsync(cancellationToken);
            var data = JsonSerializer.Deserialize<ExchangeRateResponse>(json, new JsonSerializerOptions 
            { 
                PropertyNameCaseInsensitive = true 
            });
            
            if (data?.Rates != null)
            {
                // Try to find the rate (case-insensitive search)
                var rateEntry = data.Rates.FirstOrDefault(kvp => 
                    kvp.Key.Equals(toCurrency, StringComparison.OrdinalIgnoreCase));
                
                if (rateEntry.Key != null)
                {
                    _logger.LogInformation(
                        "Exchange rate {FromCurrency} to {ToCurrency}: {Rate}",
                        fromCurrency, toCurrency, rateEntry.Value);
                    return rateEntry.Value;
                }
            }

            _logger.LogWarning(
                "Exchange rate not found for {FromCurrency} to {ToCurrency}. Available currencies: {Currencies}",
                fromCurrency, toCurrency, 
                data?.Rates != null ? string.Join(", ", data.Rates.Keys.Take(10)) : "none");
            return 1.0m; // Fallback to 1:1 if rate not found
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, 
                "Error fetching exchange rate from {FromCurrency} to {ToCurrency}",
                fromCurrency, toCurrency);
            return 1.0m; // Fallback to 1:1 on error
        }
    }

    public async Task<decimal> ConvertAmountAsync(
        decimal amount,
        string fromCurrency,
        string toCurrency,
        DateTime date,
        CancellationToken cancellationToken = default)
    {
        var rate = await GetExchangeRateAsync(fromCurrency, toCurrency, date, cancellationToken);
        var convertedAmount = amount * rate;
        
        _logger.LogInformation(
            "Converted {Amount} {FromCurrency} to {ConvertedAmount} {ToCurrency} (rate: {Rate})",
            amount, fromCurrency, convertedAmount, toCurrency, rate);
        
        return Math.Round(convertedAmount, 2);
    }

    private class ExchangeRateResponse
    {
        public Dictionary<string, decimal>? Rates { get; set; }
    }
}

