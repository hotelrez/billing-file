namespace BillingFile.Application.DTOs;

/// <summary>
/// Data Transfer Object for Hotel Billing Currency information
/// </summary>
public class HotelBillingCurrencyDto
{
    public int HotelID { get; set; }
    public bool Enabled { get; set; }
    public string Currency { get; set; } = string.Empty;
}

