namespace BillingFile.Domain.Entities;

/// <summary>
/// Represents a billing record in the system
/// </summary>
public class BillingRecord : BaseEntity
{
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public string InvoiceNumber { get; set; } = string.Empty;
    public DateTime InvoiceDate { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "USD";
    public BillingStatus Status { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime? PaidDate { get; set; }
}

public enum BillingStatus
{
    Pending = 0,
    Paid = 1,
    Overdue = 2,
    Cancelled = 3,
    Refunded = 4
}

