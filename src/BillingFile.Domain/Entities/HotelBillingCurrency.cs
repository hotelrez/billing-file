namespace BillingFile.Domain.Entities;

/// <summary>
/// Entity mapping to Play.dbo.HotelBillingCurrency table
/// Table has only 3 columns: HotelID, Enabled, Currency
/// </summary>
public class HotelBillingCurrency
{
    // Primary key - maps to CSV column "ID"
    public int HotelID { get; set; }
    
    // Enabled flag (bit/boolean in DB) - maps to CSV column "Status" (ACTIVE = true, anything else = false)
    public bool Enabled { get; set; }
    
    // Currency code - maps to CSV column "Currency"
    public string Currency { get; set; } = string.Empty;
}

