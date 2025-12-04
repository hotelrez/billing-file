namespace BillingFile.Domain.Entities;

/// <summary>
/// Entity mapping to Play.dbo.HotelBillingCurrency table
/// Table has only 3 columns: HotelID, Enabled, Currency
/// </summary>
public class HotelBillingCurrency
{
    // Primary key - maps to CSV column "ID"
    public int HotelID { get; set; }
    
    // Enabled flag - maps to CSV column "Status" (ACTIVE = 1, anything else = 0)
    public int Enabled { get; set; }
    
    // Currency code - maps to CSV column "Currency"
    public string Currency { get; set; } = string.Empty;
}

