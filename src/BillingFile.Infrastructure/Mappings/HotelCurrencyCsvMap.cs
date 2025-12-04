using BillingFile.Application.DTOs;
using CsvHelper.Configuration;

namespace BillingFile.Infrastructure.Mappings;

/// <summary>
/// CsvHelper mapping configuration for HotelCurrency CSV file
/// Handles semicolon delimiter and specific column mappings
/// </summary>
public class HotelCurrencyCsvMap : ClassMap<HotelCurrencyCsvRecord>
{
    public HotelCurrencyCsvMap()
    {
        Map(m => m.ID).Name("ID");
        Map(m => m.HotelREZHotelCode).Name("HotelREZHotelCode");
        Map(m => m.HotelName).Name("Hotel Name");
        Map(m => m.Status).Name("Status");
        Map(m => m.ChainName).Name("Chain Name");
        Map(m => m.Country).Name("Country");
        Map(m => m.Currency).Name("Currency");
    }
}

