using BillingFile.Application.DTOs;
using BillingFile.Domain.Entities;
using BillingFile.Domain.Interfaces;
using BillingFile.Infrastructure.Data;
using BillingFile.Infrastructure.Mappings;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Globalization;

namespace BillingFile.Infrastructure.Services;

/// <summary>
/// Service for importing hotel currency data from CSV files
/// Handles the currencies.csv format with semicolon delimiter
/// Imports into Play.dbo.HotelBillingCurrency table
/// </summary>
public class HotelCurrencyCsvImportService : ICsvImportService<HotelBillingCurrency>
{
    private readonly PlayDbContext _context;
    private readonly ILogger<HotelCurrencyCsvImportService> _logger;

    public HotelCurrencyCsvImportService(
        PlayDbContext context,
        ILogger<HotelCurrencyCsvImportService> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<ImportResult> ImportFromCsvAsync(
        string filePath, 
        CancellationToken cancellationToken = default)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"CSV file not found: {filePath}");

        using var stream = File.OpenRead(filePath);
        var result = await ImportFromStreamAsync(stream, Path.GetFileName(filePath), cancellationToken);
        
        // Archive the file after processing
        if (result.IsSuccess)
        {
            result.ArchiveFilePath = ArchiveFile(filePath, "processed");
        }
        else
        {
            result.ArchiveFilePath = ArchiveFile(filePath, "failed");
            await WriteErrorLogAsync(result.ArchiveFilePath, result);
        }
        
        return result;
    }

    public async Task<ImportResult> ImportFromStreamAsync(
        Stream stream, 
        string fileName,
        CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();
        var result = new ImportResult();
        var batchId = $"{DateTime.UtcNow:yyyyMMdd_HHmmss}_{Path.GetFileNameWithoutExtension(fileName)}";
        
        try
        {
            _logger.LogInformation("Starting hotel currency CSV import from {FileName}", fileName);
            
            // TRUNCATE TABLE before import to ensure clean data
            _logger.LogInformation("Truncating HotelBillingCurrency table before import...");
            await _context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE dbo.HotelBillingCurrency", cancellationToken);
            _logger.LogInformation("Table truncated successfully");
            
            using var reader = new StreamReader(stream);
            
            // Configure for SEMICOLON delimiter (not comma)
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",  // Important: semicolon delimiter
                HasHeaderRecord = true,
                MissingFieldFound = null,
                HeaderValidated = null,
                TrimOptions = TrimOptions.Trim,
                BadDataFound = context =>
                {
                    _logger.LogWarning("Bad data at row {Row}: {RawRecord}", 
                        context.Context.Parser.Row, context.RawRecord);
                    result.Warnings.Add($"Row {context.Context.Parser.Row}: Malformed data");
                }
            };
            
            using var csv = new CsvReader(reader, config);
            
            // Register the CSV mapping
            csv.Context.RegisterClassMap<HotelCurrencyCsvMap>();
            
            var entitiesToAdd = new List<HotelBillingCurrency>();
            
            // Read records manually to handle parsing errors gracefully
            csv.Read();
            csv.ReadHeader();
            
            while (csv.Read())
            {
                result.TotalRows++;
                
                try
                {
                    // Try to parse ID safely
                    var idString = csv.GetField("ID")?.Trim();
                    if (string.IsNullOrWhiteSpace(idString) || !int.TryParse(idString, out var hotelId) || hotelId <= 0)
                    {
                        result.SkippedRows++;
                        result.Warnings.Add($"Row {result.TotalRows}: Invalid or missing ID '{idString}'");
                        continue;
                    }
                    
                    // Get currency
                    var currency = csv.GetField("Currency")?.Trim();
                    if (string.IsNullOrWhiteSpace(currency))
                    {
                        result.SkippedRows++;
                        result.Warnings.Add($"Row {result.TotalRows}: Missing Currency");
                        continue;
                    }
                    
                    // Get status
                    var status = csv.GetField("Status")?.Trim();
                    var enabled = status?.Equals("ACTIVE", StringComparison.OrdinalIgnoreCase) == true ? 1 : 0;
                    
                    // Map CSV record to entity (Play.dbo.HotelBillingCurrency - 3 columns)
                    // CSV "ID" -> HotelID
                    // CSV "Status" -> Enabled (ACTIVE = 1, anything else = 0)
                    // CSV "Currency" -> Currency
                    var entity = new HotelBillingCurrency
                    {
                        HotelID = hotelId,
                        Enabled = enabled,
                        Currency = currency
                    };
                    
                    entitiesToAdd.Add(entity);
                    result.SuccessfulRows++;
                    
                    // Batch save every 500 records for performance
                    if (entitiesToAdd.Count >= 500)
                    {
                        await SaveBatchAsync(entitiesToAdd, cancellationToken);
                        _logger.LogInformation("Processed {Count} records...", result.SuccessfulRows);
                        entitiesToAdd.Clear();
                    }
                }
                catch (Exception ex)
                {
                    result.FailedRows++;
                    result.Errors.Add($"Row {result.TotalRows}: {ex.Message}");
                    _logger.LogError(ex, "Error importing row {Row}", result.TotalRows);
                }
            }
            
            // Save remaining records
            if (entitiesToAdd.Any())
            {
                await SaveBatchAsync(entitiesToAdd, cancellationToken);
            }
            
            result.IsSuccess = result.FailedRows == 0;
            
            _logger.LogInformation(
                "CSV import completed: {Total} total, {Success} successful, {Failed} failed, {Skipped} skipped",
                result.TotalRows, result.SuccessfulRows, result.FailedRows, result.SkippedRows);
        }
        catch (Exception ex)
        {
            result.IsSuccess = false;
            result.Errors.Add($"Critical error: {ex.Message}");
            _logger.LogError(ex, "Critical error during CSV import from {FileName}", fileName);
        }
        finally
        {
            stopwatch.Stop();
            result.Duration = stopwatch.Elapsed;
        }
        
        return result;
    }
    
    private async Task SaveBatchAsync(List<HotelBillingCurrency> entities, CancellationToken cancellationToken)
    {
        // Use bulk insert for better performance into Play.dbo.HotelBillingCurrency
        await _context.HotelBillingCurrencies.AddRangeAsync(entities, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
    
    private string ArchiveFile(string sourceFilePath, string status)
    {
        try
        {
            var fileName = Path.GetFileNameWithoutExtension(sourceFilePath);
            var extension = Path.GetExtension(sourceFilePath);
            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var archiveFileName = $"{fileName}_{timestamp}_{status}{extension}";
            
            var baseDirectory = Path.GetDirectoryName(sourceFilePath);
            var archiveDirectory = Path.Combine(baseDirectory!, "..", status);
            var archivePath = Path.Combine(archiveDirectory, archiveFileName);
            
            Directory.CreateDirectory(archiveDirectory);
            File.Move(sourceFilePath, archivePath, overwrite: true);
            
            _logger.LogInformation("Archived file to: {ArchivePath}", archivePath);
            return archivePath;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to archive file: {FilePath}", sourceFilePath);
            return sourceFilePath;
        }
    }
    
    private async Task WriteErrorLogAsync(string csvFilePath, ImportResult result)
    {
        try
        {
            var logFilePath = Path.ChangeExtension(csvFilePath, ".log");
            var logLines = new List<string>
            {
                $"Import Failed: {DateTime.Now}",
                $"Total Rows: {result.TotalRows}",
                $"Successful: {result.SuccessfulRows}",
                $"Failed: {result.FailedRows}",
                $"Skipped: {result.SkippedRows}",
                $"Duration: {result.Duration}",
                "",
                "Errors:",
                "-------"
            };
            
            logLines.AddRange(result.Errors);
            
            if (result.Warnings.Any())
            {
                logLines.Add("");
                logLines.Add("Warnings:");
                logLines.Add("---------");
                logLines.AddRange(result.Warnings);
            }
            
            await File.WriteAllLinesAsync(logFilePath, logLines);
            _logger.LogInformation("Error log written to: {LogFilePath}", logFilePath);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to write error log");
        }
    }
}

