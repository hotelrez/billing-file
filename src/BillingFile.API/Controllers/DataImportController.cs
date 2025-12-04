using BillingFile.Domain.Entities;
using BillingFile.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BillingFile.API.Controllers;

/// <summary>
/// API controller for CSV data imports
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class DataImportController : ControllerBase
{
    private readonly ICsvImportService<HotelBillingCurrency> _hotelCurrencyImporter;
    private readonly ILogger<DataImportController> _logger;

    public DataImportController(
        ICsvImportService<HotelBillingCurrency> hotelCurrencyImporter,
        ILogger<DataImportController> logger)
    {
        _hotelCurrencyImporter = hotelCurrencyImporter ?? throw new ArgumentNullException(nameof(hotelCurrencyImporter));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Upload and import hotel currency data from CSV file
    /// </summary>
    /// <param name="file">CSV file with hotel currency data (semicolon-delimited)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Import result with statistics</returns>
    /// <remarks>
    /// Expected CSV format (semicolon-delimited):
    /// ID;HotelREZHotelCode;Hotel Name;Status;Chain Name;Country;Currency
    /// 
    /// Only 3 columns are imported:
    /// - ID -> HotelID
    /// - Status -> Enabled (ACTIVE = true, anything else = false)
    /// - Currency -> Currency
    /// 
    /// Example:
    /// 35875;HTO28631;Ardoe House Hotel and Spa;ACTIVE;7 Hospitality Management;United Kingdom;GBP
    /// </remarks>
    [HttpPost("hotel-currencies")]
    [ProducesResponseType(typeof(ImportResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ImportHotelCurrencies(
        IFormFile file,
        CancellationToken cancellationToken)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest(new { error = "No file uploaded" });
        }

        if (!file.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
        {
            return BadRequest(new { error = "Only CSV files are allowed" });
        }

        if (file.Length > 10 * 1024 * 1024) // 10MB limit
        {
            return BadRequest(new { error = "File size exceeds 10MB limit" });
        }

        try
        {
            using var stream = file.OpenReadStream();
            var result = await _hotelCurrencyImporter.ImportFromStreamAsync(
                stream, 
                file.FileName, 
                cancellationToken);

            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    success = false,
                    message = "Import completed with errors",
                    result
                });
            }

            return Ok(new
            {
                success = true,
                message = "Import completed successfully",
                result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during hotel currency import");
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                error = "An error occurred during import",
                details = ex.Message
            });
        }
    }

    /// <summary>
    /// Import hotel currencies from pending folder (for scheduled/background processing)
    /// </summary>
    /// <param name="fileName">Name of the CSV file in the pending folder</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Import result</returns>
    [HttpPost("hotel-currencies/from-pending/{fileName}")]
    [ProducesResponseType(typeof(ImportResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ImportHotelCurrenciesFromPending(
        string fileName,
        CancellationToken cancellationToken)
    {
        try
        {
            // Path to pending folder
            var baseDirectory = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "Data", "Imports", "pending");
            var filePath = Path.Combine(baseDirectory, fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound(new { error = $"File not found: {fileName}" });
            }

            _logger.LogInformation("Starting import from pending folder: {FileName}", fileName);
            
            var result = await _hotelCurrencyImporter.ImportFromCsvAsync(filePath, cancellationToken);

            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    success = false,
                    message = "Import completed with errors",
                    result
                });
            }

            return Ok(new
            {
                success = true,
                message = "Import completed successfully",
                result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during hotel currency import from pending folder");
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                error = "An error occurred during import",
                details = ex.Message
            });
        }
    }
}

