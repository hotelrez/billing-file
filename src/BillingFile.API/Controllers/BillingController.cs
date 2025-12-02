using BillingFile.Application.DTOs;
using BillingFile.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;

namespace BillingFile.API.Controllers;

// NOTE: To add or remove fields from the billing API response,
// edit the file: src/BillingFile.Application/DTOs/BillingDto.cs

/// <summary>
/// API controller for billing operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class BillingController : ControllerBase
{
    private readonly IReservationService _reservationService;
    private readonly ILogger<BillingController> _logger;

    public BillingController(
        IReservationService reservationService,
        ILogger<BillingController> logger)
    {
        _reservationService = reservationService ?? throw new ArgumentNullException(nameof(reservationService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Get billing records by arrival date range
    /// </summary>
    /// <param name="startDate">Start date (inclusive)</param>
    /// <param name="endDate">End date (inclusive)</param>
    /// <param name="type">Output type: json (default) or csv</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of billing records as JSON or CSV file download</returns>
    /// <remarks>
    /// Sample requests:
    ///     GET /api/billing?startDate=2025-12-01&amp;endDate=2025-12-31
    ///     GET /api/billing?startDate=2025-12-01&amp;endDate=2025-12-31&amp;type=json
    ///     GET /api/billing?startDate=2025-12-01&amp;endDate=2025-12-31&amp;type=csv
    /// </remarks>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<BillingDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetBillingByDateRange(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate,
        [FromQuery] string type = "json",
        CancellationToken cancellationToken = default)
    {
        if (startDate == default || endDate == default)
        {
            return BadRequest(new { error = "Both startDate and endDate are required" });
        }

        var result = await _reservationService.GetBillingByArrivalDateRangeAsync(startDate, endDate, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(new { error = result.ErrorMessage });
        }

        // Return CSV file download
        if (type.Equals("csv", StringComparison.OrdinalIgnoreCase))
        {
            var csv = GenerateCsv(result.Data!);
            var fileName = $"billing_{startDate:yyyy-MM-dd}_to_{endDate:yyyy-MM-dd}.csv";
            return File(Encoding.UTF8.GetBytes(csv), "text/csv", fileName);
        }

        // Return JSON (default)
        return Ok(result.Data);
    }

    private static string GenerateCsv(IEnumerable<BillingDto> data)
    {
        var sb = new StringBuilder();
        
        // Get all properties from BillingDto using reflection
        var properties = typeof(BillingDto).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        
        // Get column names from JsonPropertyName attribute or property name
        var columnNames = properties.Select(p =>
        {
            var jsonAttr = p.GetCustomAttribute<JsonPropertyNameAttribute>();
            return jsonAttr?.Name ?? p.Name;
        }).ToArray();
        
        // Header row - automatically generated from BillingDto properties
        sb.AppendLine(string.Join(",", columnNames));
        
        // Data rows
        foreach (var item in data)
        {
            var values = properties.Select(p =>
            {
                var value = p.GetValue(item);
                return EscapeCsvField(value?.ToString());
            });
            sb.AppendLine(string.Join(",", values));
        }
        
        return sb.ToString();
    }

    private static string EscapeCsvField(string? value)
    {
        if (string.IsNullOrEmpty(value))
            return "";
        
        // Escape quotes and wrap in quotes if contains comma, quote, or newline
        if (value.Contains(',') || value.Contains('"') || value.Contains('\n'))
        {
            return $"\"{value.Replace("\"", "\"\"")}\"";
        }
        
        return value;
    }
}

