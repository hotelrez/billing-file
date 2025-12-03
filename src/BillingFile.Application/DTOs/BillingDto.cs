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
    
    // ===== ADD MORE FIELDS BELOW =====
    // Note: Fields must exist in GetBillingFileReservations SP output
}

