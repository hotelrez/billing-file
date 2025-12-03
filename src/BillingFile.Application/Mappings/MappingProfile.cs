using System.Xml.Linq;
using AutoMapper;
using BillingFile.Application.DTOs;
using BillingFile.Domain.Entities;

namespace BillingFile.Application.Mappings;

/// <summary>
/// AutoMapper profile for entity-DTO mappings - matching actual database schema
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Hotel mappings (read-only) - map actual database columns
        CreateMap<Hotel, HotelDto>()
            .ForMember(dest => dest.HotelChainID, opt => opt.MapFrom(src => src.fkHotelChainID));

        // Reservation mappings (read-only) - direct mapping
        CreateMap<FullReservation, ReservationDto>();

        // Billing mappings from Stored Procedure results
        // Maps GetBillingFileReservations SP output to BillingDto
        CreateMap<BillingSpResult, BillingDto>()
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => ParseDescriptionFromXml(src.xml)))
            .ForMember(dest => dest.Fax_Notification_Count, opt => opt.MapFrom(src => ParseFaxCountFromXml(src.xml)))
            .ForMember(dest => dest.Channel, opt => opt.MapFrom(src => ParseChannelFromXml(src.xml)))
            .ForMember(dest => dest.Secondary_Source, opt => opt.MapFrom(src => ParseSecondarySourceFromXml(src.xml)))
            .ForMember(dest => dest.Sub_Source, opt => opt.MapFrom(src => ParseSubSourceFromXml(src.xml)))
            .ForMember(dest => dest.Sub_Source_Code, opt => opt.MapFrom(src => ParseSubSourceCodeFromXml(src.xml)));
    }
    
    /// <summary>
    /// Parse Description from XML field
    /// Extracts: /OTA_HotelResNotifRQ/POS/Source/BookingChannel/CompanyName
    /// </summary>
    private static string? ParseDescriptionFromXml(string? xml)
    {
        if (string.IsNullOrEmpty(xml))
            return null;
            
        try
        {
            var doc = XDocument.Parse(xml);
            XNamespace ns = "http://www.opentravel.org/OTA/2003/05";
            
            var companyName = doc.Root?
                .Element(ns + "POS")?
                .Element(ns + "Source")?
                .Element(ns + "BookingChannel")?
                .Element(ns + "CompanyName")?
                .Value;
                
            return companyName;
        }
        catch
        {
            return null;
        }
    }
    
    /// <summary>
    /// Parse Fax_Notification_Count from XML field
    /// Extracts: /OTA_HotelResNotifRQ/HotelReservations/HotelReservation/TPA_Extensions/FaxCount/@Count
    /// </summary>
    private static int? ParseFaxCountFromXml(string? xml)
    {
        if (string.IsNullOrEmpty(xml))
            return null;
            
        try
        {
            var doc = XDocument.Parse(xml);
            XNamespace ns = "http://www.opentravel.org/OTA/2003/05";
            
            var countAttr = doc.Root?
                .Element(ns + "HotelReservations")?
                .Element(ns + "HotelReservation")?
                .Element(ns + "TPA_Extensions")?
                .Element(ns + "FaxCount")?
                .Attribute("Count")?
                .Value;
                
            if (int.TryParse(countAttr, out var count))
                return count;
                
            return null;
        }
        catch
        {
            return null;
        }
    }
    
    /// <summary>
    /// Parse Channel from XML field
    /// Extracts: /OTA_HotelResNotifRQ/POS/TPA_Extensions/ChannelInfo/Book/@ChannelName
    /// </summary>
    private static string? ParseChannelFromXml(string? xml)
    {
        if (string.IsNullOrEmpty(xml))
            return null;
            
        try
        {
            var doc = XDocument.Parse(xml);
            XNamespace ns = "http://www.opentravel.org/OTA/2003/05";
            
            var channelName = doc.Root?
                .Element(ns + "POS")?
                .Element(ns + "TPA_Extensions")?
                .Element(ns + "ChannelInfo")?
                .Element(ns + "Book")?
                .Attribute("ChannelName")?
                .Value;
                
            return channelName;
        }
        catch
        {
            return null;
        }
    }
    
    /// <summary>
    /// Parse Secondary_Source from XML field
    /// Extracts: /OTA_HotelResNotifRQ/POS/TPA_Extensions/ChannelInfo/Book/@SecondaryChannelName
    /// </summary>
    private static string? ParseSecondarySourceFromXml(string? xml)
    {
        if (string.IsNullOrEmpty(xml))
            return null;
            
        try
        {
            var doc = XDocument.Parse(xml);
            XNamespace ns = "http://www.opentravel.org/OTA/2003/05";
            
            var secondaryChannelName = doc.Root?
                .Element(ns + "POS")?
                .Element(ns + "TPA_Extensions")?
                .Element(ns + "ChannelInfo")?
                .Element(ns + "Book")?
                .Attribute("SecondaryChannelName")?
                .Value;
                
            return secondaryChannelName;
        }
        catch
        {
            return null;
        }
    }
    
    /// <summary>
    /// Parse Sub_Source from XML field
    /// Extracts: /OTA_HotelResNotifRQ/POS/TPA_Extensions/ChannelInfo/Book/@SubChannelName
    /// </summary>
    private static string? ParseSubSourceFromXml(string? xml)
    {
        if (string.IsNullOrEmpty(xml))
            return null;
            
        try
        {
            var doc = XDocument.Parse(xml);
            XNamespace ns = "http://www.opentravel.org/OTA/2003/05";
            
            var subChannelName = doc.Root?
                .Element(ns + "POS")?
                .Element(ns + "TPA_Extensions")?
                .Element(ns + "ChannelInfo")?
                .Element(ns + "Book")?
                .Attribute("SubChannelName")?
                .Value;
                
            return subChannelName;
        }
        catch
        {
            return null;
        }
    }
    
    /// <summary>
    /// Parse Sub_Source_Code from XML field
    /// Extracts: /OTA_HotelResNotifRQ/POS/TPA_Extensions/ChannelInfo/Book/@SubSourceCode
    /// </summary>
    private static string? ParseSubSourceCodeFromXml(string? xml)
    {
        if (string.IsNullOrEmpty(xml))
            return null;
            
        try
        {
            var doc = XDocument.Parse(xml);
            XNamespace ns = "http://www.opentravel.org/OTA/2003/05";
            
            var subSourceCode = doc.Root?
                .Element(ns + "POS")?
                .Element(ns + "TPA_Extensions")?
                .Element(ns + "ChannelInfo")?
                .Element(ns + "Book")?
                .Attribute("SubSourceCode")?
                .Value;
                
            return subSourceCode;
        }
        catch
        {
            return null;
        }
    }
}

