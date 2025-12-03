using System.Text.Json.Serialization;

namespace BillingFile.Application.DTOs;

/// <summary>
/// Billing DTO - maps to GetBillingFileReservations stored procedure output
/// Add or remove properties here to control what is returned by the billing API
/// </summary>
public class BillingDto
{
    // ===== MAPPING TABLE =====
    // Fields from GetBillingFileReservations SP
    // Format: [JsonPropertyName("OutputName")] for the JSON output name
    
    [JsonPropertyName("ID")]
    public int ID { get; set; }
    
    [JsonPropertyName("Chain_Name")]
    public string? Chain_Name { get; set; }
    
    [JsonPropertyName("Chain_ID")]
    public int? Chain_ID { get; set; }
    
    [JsonPropertyName("Hotel_Name")]
    public string? Hotel_Name { get; set; }
    
    [JsonPropertyName("Hotel_ID")]
    public int? Hotel_ID { get; set; }
    
    [JsonPropertyName("SAP_ID")]
    public int? SAP_ID { get; set; }
    
    [JsonPropertyName("Confirm_Number")]
    public string? Confirm_Number { get; set; }
    
    [JsonPropertyName("Description")]
    public string? Description { get; set; }  // Parsed from xml: /OTA_HotelResNotifRQ/POS/Source/BookingChannel/CompanyName
    
    [JsonPropertyName("Fax_Notification_Count")]
    public int? Fax_Notification_Count { get; set; }  // Parsed from xml: /OTA_HotelResNotifRQ/HotelReservations/HotelReservation/TPA_Extensions/FaxCount/@Count
    
    [JsonPropertyName("Channel")]
    public string? Channel { get; set; }  // Parsed from xml: /OTA_HotelResNotifRQ/POS/TPA_Extensions/ChannelInfo/Book/@ChannelName
    
    [JsonPropertyName("Secondary_Source")]
    public string? Secondary_Source { get; set; }  // Parsed from xml: /OTA_HotelResNotifRQ/POS/TPA_Extensions/ChannelInfo/Book/@SecondaryChannelName
    
    [JsonPropertyName("Sub_Source")]
    public string? Sub_Source { get; set; }  // Parsed from xml: /OTA_HotelResNotifRQ/POS/TPA_Extensions/ChannelInfo/Book/@SubChannelName
    
    [JsonPropertyName("Sub_Source_Code")]
    public string? Sub_Source_Code { get; set; }  // Parsed from xml: /OTA_HotelResNotifRQ/POS/TPA_Extensions/ChannelInfo/Book/@SubSourceCode
    
    // ===== ADD MORE FIELDS BELOW =====
    // Note: Fields must exist in GetBillingFileReservations SP output
}

