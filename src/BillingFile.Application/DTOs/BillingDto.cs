using System.Text.Json.Serialization;

namespace BillingFile.Application.DTOs;

/// <summary>
/// Billing DTO - contains only mapped fields for billing response
/// Add or remove properties here to control what is returned by the billing API
/// </summary>
public class BillingDto
{
    // ===== MAPPING TABLE =====
    // Add fields here to include them in the billing API response
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
    
    // ===== ADD MORE FIELDS BELOW =====
    // Example:
    // [JsonPropertyName("Hotel_Code")]
    // public string? Hotel_Code { get; set; }
}

