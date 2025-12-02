namespace BillingFile.Application.DTOs;

/// <summary>
/// Data Transfer Object for Hotel - matches actual MemberPortal.dbo.Hotel schema
/// </summary>
public class HotelDto
{
    public int ID { get; set; }
    public string? TrustCode { get; set; }
    public string Name { get; set; } = string.Empty;
    public string CountryCode { get; set; } = string.Empty;
    public string? CityCode { get; set; }
    public int HotelChainID { get; set; }
    public string? BookingEngineBaseURL { get; set; }
    public bool? CanGeneratePromotionalUrl { get; set; }
    public bool? IsEnterpriseMember { get; set; }
    public bool HasGoogleAnalytics { get; set; }
    public string? REZMedia { get; set; }
    public string? REZbooker { get; set; }
    public string? TrustYouId { get; set; }
    public int Active { get; set; }
}

