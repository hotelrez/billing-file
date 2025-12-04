namespace BillingFile.Domain.Interfaces;

/// <summary>
/// Service interface for CSV import operations
/// </summary>
public interface ICsvImportService<T> where T : class
{
    Task<ImportResult> ImportFromCsvAsync(
        string filePath, 
        CancellationToken cancellationToken = default);
    
    Task<ImportResult> ImportFromStreamAsync(
        Stream stream, 
        string fileName,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// Result object for import operations
/// </summary>
public class ImportResult
{
    public bool IsSuccess { get; set; }
    public int TotalRows { get; set; }
    public int SuccessfulRows { get; set; }
    public int FailedRows { get; set; }
    public int SkippedRows { get; set; }
    public List<string> Errors { get; set; } = new();
    public List<string> Warnings { get; set; } = new();
    public TimeSpan Duration { get; set; }
    public string? ArchiveFilePath { get; set; }
}

