using CsvHelper.Configuration.Attributes;

namespace BillingFile.Application.DTOs;

/// <summary>
/// CSV record mapping for currencies.csv file
/// Maps to the exact structure of the CSV file with semicolon delimiter
/// </summary>
public class HotelCurrencyCsvRecord
{
    [Name("ID")]
    public int ID { get; set; }
    
    [Name("HotelREZHotelCode")]
    public string HotelREZHotelCode { get; set; } = string.Empty;
    
    [Name("Hotel Name")]
    public string HotelName { get; set; } = string.Empty;
    
    [Name("Status")]
    public string Status { get; set; } = string.Empty;
    
    [Name("Chain Name")]
    public string ChainName { get; set; } = string.Empty;
    
    [Name("Country")]
    public string Country { get; set; } = string.Empty;
    
    [Name("Currency")]
    public string Currency { get; set; } = string.Empty;
}

