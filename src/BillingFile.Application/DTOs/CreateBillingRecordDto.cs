namespace BillingFile.Application.DTOs;

/// <summary>
/// DTO for creating a new billing record
/// </summary>
public class CreateBillingRecordDto
{
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public string InvoiceNumber { get; set; } = string.Empty;
    public DateTime InvoiceDate { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "USD";
    public string Status { get; set; } = "Pending";
    public string Description { get; set; } = string.Empty;
}

