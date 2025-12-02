namespace BillingFile.Domain.Entities;

/// <summary>
/// Entity mapping to MemberPortal.dbo.Hotel table
/// Mapped to ACTUAL table structure from database
/// </summary>
public class Hotel
{
    // Exact mapping to MemberPortal.dbo.Hotel table columns
    public int ID { get; set; }
    public string? TrustCode { get; set; }
    public string Name { get; set; } = string.Empty;
    public string CountryCode { get; set; } = string.Empty;
    public string? CityCode { get; set; }
    public int fkHotelChainID { get; set; }
    public string? BookingEngineBaseURL { get; set; }
    public bool? CanGeneratePromotionalUrl { get; set; }
    public bool? IsEnterpriseMember { get; set; }
    public bool HasGoogleAnalytics { get; set; }
    public string? REZMedia { get; set; }
    public string? REZbooker { get; set; }
    public string? TrustYouId { get; set; }
    public int Active { get; set; }
}

