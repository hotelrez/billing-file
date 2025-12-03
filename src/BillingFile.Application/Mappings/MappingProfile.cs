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
            .ForMember(dest => dest.Sub_Source_Code, opt => opt.MapFrom(src => ParseSubSourceCodeFromXml(src.xml)))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => ParseStatusFromXml(src.xml)))
            .ForMember(dest => dest.Confirm_Date, opt => opt.MapFrom(src => ParseConfirmDateFromXml(src.xml)))
            .ForMember(dest => dest.Cancel_Number, opt => opt.MapFrom(src => ParseCancelNumberFromXml(src.xml)))
            .ForMember(dest => dest.Cancel_Date, opt => opt.MapFrom(src => ParseXmlAttribute(src.xml, "HotelReservations/HotelReservation", "LastModifyDateTime")))
            .ForMember(dest => dest.Cancellation_Channel, opt => opt.MapFrom(src => ParseXmlAttribute(src.xml, "HotelReservations/HotelReservation/TPA_Extensions/LastModifyingChannel", "Type")))
            .ForMember(dest => dest.Cancellation_Secondary_Channel, opt => opt.MapFrom(src => ParseXmlAttribute(src.xml, "HotelReservations/HotelReservation/TPA_Extensions/LastModifyingChannel/CompanyName", "Code")))
            .ForMember(dest => dest.Reinstatement_Date, opt => opt.MapFrom(src => ParseXmlAttribute(src.xml, "HotelReservations/HotelReservation/TPA_Extensions/ReinstatementInfo", "LastReinstatementDateTime")))
            .ForMember(dest => dest.Salutation, opt => opt.MapFrom(src => ParseXmlElement(src.xml, "HotelReservations/HotelReservation/ResGuests/ResGuest/TPA_Extensions/ResGuestInfo/PersonName/NamePrefix")))
            .ForMember(dest => dest.Guest_First_Name, opt => opt.MapFrom(src => ParseXmlElement(src.xml, "HotelReservations/HotelReservation/ResGuests/ResGuest/TPA_Extensions/ResGuestInfo/PersonName/GivenName")))
            .ForMember(dest => dest.Guest_Last_Name, opt => opt.MapFrom(src => ParseXmlElement(src.xml, "HotelReservations/HotelReservation/ResGuests/ResGuest/TPA_Extensions/ResGuestInfo/PersonName/Surname")))
            .ForMember(dest => dest.Arrival_Date, opt => opt.MapFrom(src => ParseXmlAttribute(src.xml, "HotelReservations/HotelReservation/RoomStays/RoomStay/TimeSpan", "Start")))
            .ForMember(dest => dest.Depart_Date, opt => opt.MapFrom(src => ParseXmlAttribute(src.xml, "HotelReservations/HotelReservation/RoomStays/RoomStay/TimeSpan", "End")))
            .ForMember(dest => dest.Rate_Category_Name, opt => opt.MapFrom(src => ParseRateCategoryNameFromXml(src.xml)))
            .ForMember(dest => dest.Rate_Category_Code, opt => opt.MapFrom(src => ParseXmlAttribute(src.xml, "HotelReservations/HotelReservation/RoomStays/RoomStay/RoomRates/RoomRate", "RatePlanCategory")))
            .ForMember(dest => dest.Rate_Type_Name, opt => opt.MapFrom(src => ParseRateTypeNameFromXml(src.xml)))
            .ForMember(dest => dest.Rate_Type_Code, opt => opt.MapFrom(src => ParseXmlAttribute(src.xml, "HotelReservations/HotelReservation/RoomStays/RoomStay/RatePlans/RatePlan", "RatePlanCode")))
            .ForMember(dest => dest.Room_Type_Name, opt => opt.MapFrom(src => ParseRoomTypeNameFromXml(src.xml)))
            .ForMember(dest => dest.Room_Type_Code, opt => opt.MapFrom(src => ParseXmlAttribute(src.xml, "HotelReservations/HotelReservation/RoomStays/RoomStay/RoomTypes/RoomType", "RoomTypeCode")))
            .ForMember(dest => dest.Nights, opt => opt.MapFrom(src => CalculateNightsFromXml(src.xml)))
            .ForMember(dest => dest.Rooms, opt => opt.MapFrom(src => ParseXmlAttributeAsInt(src.xml, "HotelReservations/HotelReservation/RoomStays/RoomStay/RoomRates/RoomRate", "NumberOfUnits")))
            .ForMember(dest => dest.Reservation_Revenue_Before_Tax, opt => opt.MapFrom(src => ParseXmlAttributeAsDecimal(src.xml, "HotelReservations/HotelReservation/RoomStays/RoomStay/Total", "AmountBeforeTax")))
            .ForMember(dest => dest.Reservation_Revenue_After_Tax, opt => opt.MapFrom(src => ParseXmlAttributeAsDecimal(src.xml, "HotelReservations/HotelReservation/RoomStays/RoomStay/Total", "AmountAfterTax")))
            .ForMember(dest => dest.Rate_Revenue_With_Inclusive_Tax_Amt, opt => opt.MapFrom(src => ParseXmlAttributeAsDecimal(src.xml, "HotelReservations/HotelReservation/RoomStays/RoomStay/RoomRates/RoomRate/Rates/Rate/Base", "AmountAfterTax")))
            .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => ParseXmlAttribute(src.xml, "HotelReservations/HotelReservation/RoomStays/RoomStay/Total", "CurrencyCode")))
            .ForMember(dest => dest.Loyalty_Points_Payment, opt => opt.MapFrom(src => ParseXmlAttribute(src.xml, "HotelReservations/HotelReservation/RoomStays/RoomStay/Guarantee/GuaranteesAccepted/GuaranteeAccepted/LoyaltyRedemption", "RedemptionQuantity")))
            .ForMember(dest => dest.Total_Rate_Loyalty_Points, opt => opt.MapFrom(src => ParseXmlAttribute(src.xml, "HotelReservations/HotelReservation/RoomStays/RoomStay/Guarantee/GuaranteesAccepted/GuaranteeAccepted/LoyaltyRedemption", "RedemptionQuantity")))
            .ForMember(dest => dest.Travel_Industry_ID, opt => opt.MapFrom(src => ParseTravelIndustryIdFromXml(src.xml)))
            .ForMember(dest => dest.Travel_Agency_Name, opt => opt.MapFrom(src => ParseTravelAgencyFieldFromXml(src.xml, "CompanyName", null)))
            .ForMember(dest => dest.Travel_Agency_Street_Address, opt => opt.MapFrom(src => ParseTravelAgencyAddressFieldFromXml(src.xml, "AddressLine", null)))
            .ForMember(dest => dest.Travel_Agency_City, opt => opt.MapFrom(src => ParseTravelAgencyAddressFieldFromXml(src.xml, "CityName", null)))
            .ForMember(dest => dest.Travel_Agency_State, opt => opt.MapFrom(src => ParseTravelAgencyAddressFieldFromXml(src.xml, "StateProv", "StateCode")))
            .ForMember(dest => dest.Travel_Agency_Zip_Postal_Code, opt => opt.MapFrom(src => ParseTravelAgencyAddressFieldFromXml(src.xml, "PostalCode", null)))
            .ForMember(dest => dest.Travel_Agency_Country, opt => opt.MapFrom(src => ParseTravelAgencyAddressFieldFromXml(src.xml, "CountryName", "Code")))
            .ForMember(dest => dest.Total_Dynamic_Package_Revenue, opt => opt.MapFrom(src => ParseXmlAttribute(src.xml, "HotelReservations/HotelReservation/TPA_Extensions/Packages", "PackageTotalAmount")))
            .ForMember(dest => dest.Itinerary_Number, opt => opt.MapFrom(src => ParseHotelReservationIdByType(src.xml, "34")))
            .ForMember(dest => dest.Channel_Connect_Confirm_Number, opt => opt.MapFrom(src => ParseHotelReservationIdByType(src.xml, "13")))
            .ForMember(dest => dest.Commissionable_Percent, opt => opt.MapFrom(src => ParseXmlAttribute(src.xml, "HotelReservations/HotelReservation/RoomStays/RoomStay/RatePlans/RatePlan/Commission", "Percent")))
            .ForMember(dest => dest.Guest_Country, opt => opt.MapFrom(src => ParseGuestCountryFromXml(src.xml)))
            .ForMember(dest => dest.Template_Name, opt => opt.MapFrom(src => ParseXmlAttribute(src.xml, "POS/Source/TPA_Extensions/Template", "Name")))
            .ForMember(dest => dest.Shell_Name, opt => opt.MapFrom(src => ParseXmlAttribute(src.xml, "POS/Source/TPA_Extensions/Shell", "Name")))
            .ForMember(dest => dest.Loyalty_Type, opt => opt.MapFrom(src => ParseCustLoyaltyAttribute(src.xml, "Remark")))
            .ForMember(dest => dest.Loyalty_Program, opt => opt.MapFrom(src => ParseXmlAttribute(src.xml, "HotelReservations/HotelReservation/RoomStays/RoomStay/Memberships/Membership", "ProgramCode")))
            .ForMember(dest => dest.Loyalty_Number, opt => opt.MapFrom(src => ParseXmlAttribute(src.xml, "HotelReservations/HotelReservation/RoomStays/RoomStay/Memberships/Membership", "AccountID")))
            .ForMember(dest => dest.Loyalty_Level_Name, opt => opt.MapFrom(src => ParseCustLoyaltyAttribute(src.xml, "AllianceLoyaltyLevelName")))
            .ForMember(dest => dest.Loyalty_Level_Code, opt => opt.MapFrom(src => ParseCustLoyaltyAttribute(src.xml, "LoyalLevel")))
            .ForMember(dest => dest.Profile_Type_Selection, opt => opt.MapFrom(src => ParseProfileTypeFromXml(src.xml)))
            .ForMember(dest => dest.Visa_Information, opt => opt.MapFrom(src => ParseVisaInformationFromXml(src.xml)))
            .ForMember(dest => dest.Room_Upsell, opt => opt.MapFrom(src => ParseRoomUpsellFromXml(src.xml)))
            .ForMember(dest => dest.Room_Upsell_Revenue, opt => opt.MapFrom(src => ParseXmlAttribute(src.xml, "HotelReservations/HotelReservation/TPA_Extensions/RoomUpsell", "Revenue")))
            .ForMember(dest => dest.Coupon_Offer_Code, opt => opt.MapFrom(src => ParseXmlAttribute(src.xml, "HotelReservations/HotelReservation/TPA_Extensions/CouponOffers/CouponOffer", "CouponCode")))
            .ForMember(dest => dest.ADR, opt => opt.MapFrom(src => CalculateADRFromXml(src.xml)));
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
    
    /// <summary>
    /// Parse Status from XML field
    /// Extracts: /OTA_HotelResNotifRQ/HotelReservations/HotelReservation/@ResStatus
    /// </summary>
    private static string? ParseStatusFromXml(string? xml)
    {
        if (string.IsNullOrEmpty(xml))
            return null;
            
        try
        {
            var doc = XDocument.Parse(xml);
            XNamespace ns = "http://www.opentravel.org/OTA/2003/05";
            
            var status = doc.Root?
                .Element(ns + "HotelReservations")?
                .Element(ns + "HotelReservation")?
                .Attribute("ResStatus")?
                .Value;
                
            return status;
        }
        catch
        {
            return null;
        }
    }
    
    /// <summary>
    /// Parse Confirm_Date from XML field
    /// Extracts: /OTA_HotelResNotifRQ/HotelReservations/HotelReservation/TPA_Extensions/ConfirmDate/@ConfirmDateTime
    /// </summary>
    private static string? ParseConfirmDateFromXml(string? xml)
    {
        if (string.IsNullOrEmpty(xml))
            return null;
            
        try
        {
            var doc = XDocument.Parse(xml);
            XNamespace ns = "http://www.opentravel.org/OTA/2003/05";
            
            var confirmDate = doc.Root?
                .Element(ns + "HotelReservations")?
                .Element(ns + "HotelReservation")?
                .Element(ns + "TPA_Extensions")?
                .Element(ns + "ConfirmDate")?
                .Attribute("ConfirmDateTime")?
                .Value;
                
            return confirmDate;
        }
        catch
        {
            return null;
        }
    }
    
    /// <summary>
    /// Parse Cancel_Number from XML field
    /// Extracts: /OTA_HotelResNotifRQ/HotelReservations/HotelReservation/ResGlobalInfo/HotelReservationIDs/HotelReservationID[@ResID_Type="15"]/@ResID_Value
    /// </summary>
    private static string? ParseCancelNumberFromXml(string? xml)
    {
        if (string.IsNullOrEmpty(xml))
            return null;
            
        try
        {
            var doc = XDocument.Parse(xml);
            XNamespace ns = "http://www.opentravel.org/OTA/2003/05";
            
            var hotelReservationIds = doc.Root?
                .Element(ns + "HotelReservations")?
                .Element(ns + "HotelReservation")?
                .Element(ns + "ResGlobalInfo")?
                .Element(ns + "HotelReservationIDs")?
                .Elements(ns + "HotelReservationID");
                
            var cancelNumber = hotelReservationIds?
                .FirstOrDefault(e => e.Attribute("ResID_Type")?.Value == "15")?
                .Attribute("ResID_Value")?
                .Value;
                
            return cancelNumber;
        }
        catch
        {
            return null;
        }
    }
    
    /// <summary>
    /// Generic helper to parse an attribute from a path
    /// </summary>
    private static string? ParseXmlAttribute(string? xml, string path, string attributeName)
    {
        if (string.IsNullOrEmpty(xml))
            return null;
            
        try
        {
            var doc = XDocument.Parse(xml);
            XNamespace ns = "http://www.opentravel.org/OTA/2003/05";
            
            var pathParts = path.Split('/');
            XElement? current = doc.Root;
            
            foreach (var part in pathParts)
            {
                current = current?.Element(ns + part);
                if (current == null) return null;
            }
            
            return current?.Attribute(attributeName)?.Value;
        }
        catch
        {
            return null;
        }
    }
    
    /// <summary>
    /// Generic helper to parse an element value from a path
    /// </summary>
    private static string? ParseXmlElement(string? xml, string path)
    {
        if (string.IsNullOrEmpty(xml))
            return null;
            
        try
        {
            var doc = XDocument.Parse(xml);
            XNamespace ns = "http://www.opentravel.org/OTA/2003/05";
            
            var pathParts = path.Split('/');
            XElement? current = doc.Root;
            
            foreach (var part in pathParts)
            {
                current = current?.Element(ns + part);
                if (current == null) return null;
            }
            
            return current?.Value;
        }
        catch
        {
            return null;
        }
    }
    
    /// <summary>
    /// Generic helper to parse an attribute as int from a path
    /// </summary>
    private static int? ParseXmlAttributeAsInt(string? xml, string path, string attributeName)
    {
        var value = ParseXmlAttribute(xml, path, attributeName);
        if (int.TryParse(value, out var result))
            return result;
        return null;
    }
    
    /// <summary>
    /// Generic helper to parse an attribute as decimal from a path
    /// </summary>
    private static decimal? ParseXmlAttributeAsDecimal(string? xml, string path, string attributeName)
    {
        var value = ParseXmlAttribute(xml, path, attributeName);
        if (decimal.TryParse(value, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var result))
            return result;
        return null;
    }
    
    /// <summary>
    /// Parse Rate_Category_Name from XML
    /// Path: .../RatePlans/RatePlan/AdditionalDetails/AdditionalDetail[@Type="CategoryName"]/DetailDescription/Text
    /// </summary>
    private static string? ParseRateCategoryNameFromXml(string? xml)
    {
        if (string.IsNullOrEmpty(xml))
            return null;
            
        try
        {
            var doc = XDocument.Parse(xml);
            XNamespace ns = "http://www.opentravel.org/OTA/2003/05";
            
            var additionalDetails = doc.Root?
                .Element(ns + "HotelReservations")?
                .Element(ns + "HotelReservation")?
                .Element(ns + "RoomStays")?
                .Element(ns + "RoomStay")?
                .Element(ns + "RatePlans")?
                .Element(ns + "RatePlan")?
                .Element(ns + "AdditionalDetails")?
                .Elements(ns + "AdditionalDetail");
                
            return additionalDetails?
                .FirstOrDefault(e => e.Attribute("Type")?.Value == "CategoryName")?
                .Element(ns + "DetailDescription")?
                .Element(ns + "Text")?
                .Value;
        }
        catch
        {
            return null;
        }
    }
    
    /// <summary>
    /// Parse Rate_Type_Name from XML
    /// Path: .../RatePlans/RatePlan/AdditionalDetails/AdditionalDetail[@Type="Name"]/DetailDescription/Text
    /// </summary>
    private static string? ParseRateTypeNameFromXml(string? xml)
    {
        if (string.IsNullOrEmpty(xml))
            return null;
            
        try
        {
            var doc = XDocument.Parse(xml);
            XNamespace ns = "http://www.opentravel.org/OTA/2003/05";
            
            var additionalDetails = doc.Root?
                .Element(ns + "HotelReservations")?
                .Element(ns + "HotelReservation")?
                .Element(ns + "RoomStays")?
                .Element(ns + "RoomStay")?
                .Element(ns + "RatePlans")?
                .Element(ns + "RatePlan")?
                .Element(ns + "AdditionalDetails")?
                .Elements(ns + "AdditionalDetail");
                
            return additionalDetails?
                .FirstOrDefault(e => e.Attribute("Type")?.Value == "Name")?
                .Element(ns + "DetailDescription")?
                .Element(ns + "Text")?
                .Value;
        }
        catch
        {
            return null;
        }
    }
    
    /// <summary>
    /// Parse Room_Type_Name from XML
    /// Path: .../RoomTypes/RoomType/AdditionalDetails/AdditionalDetail[@Type="Name"]/DetailDescription/Text
    /// </summary>
    private static string? ParseRoomTypeNameFromXml(string? xml)
    {
        if (string.IsNullOrEmpty(xml))
            return null;
            
        try
        {
            var doc = XDocument.Parse(xml);
            XNamespace ns = "http://www.opentravel.org/OTA/2003/05";
            
            var additionalDetails = doc.Root?
                .Element(ns + "HotelReservations")?
                .Element(ns + "HotelReservation")?
                .Element(ns + "RoomStays")?
                .Element(ns + "RoomStay")?
                .Element(ns + "RoomTypes")?
                .Element(ns + "RoomType")?
                .Element(ns + "AdditionalDetails")?
                .Elements(ns + "AdditionalDetail");
                
            return additionalDetails?
                .FirstOrDefault(e => e.Attribute("Type")?.Value == "Name")?
                .Element(ns + "DetailDescription")?
                .Element(ns + "Text")?
                .Value;
        }
        catch
        {
            return null;
        }
    }
    
    /// <summary>
    /// Calculate Nights as date difference between Depart_Date and Arrival_Date
    /// </summary>
    private static int? CalculateNightsFromXml(string? xml)
    {
        if (string.IsNullOrEmpty(xml))
            return null;
            
        try
        {
            var arrivalDateStr = ParseXmlAttribute(xml, "HotelReservations/HotelReservation/RoomStays/RoomStay/TimeSpan", "Start");
            var departDateStr = ParseXmlAttribute(xml, "HotelReservations/HotelReservation/RoomStays/RoomStay/TimeSpan", "End");
            
            if (string.IsNullOrEmpty(arrivalDateStr) || string.IsNullOrEmpty(departDateStr))
                return null;
                
            if (DateTime.TryParse(arrivalDateStr, out var arrivalDate) && 
                DateTime.TryParse(departDateStr, out var departDate))
            {
                return (int)(departDate - arrivalDate).TotalDays;
            }
            
            return null;
        }
        catch
        {
            return null;
        }
    }
    
    /// <summary>
    /// Parse Travel_Industry_ID from XML
    /// Path: .../ResGuests/ResGuest/Profiles/ProfileInfo/UniqueID[@Type="5"]/@ID
    /// </summary>
    private static string? ParseTravelIndustryIdFromXml(string? xml)
    {
        if (string.IsNullOrEmpty(xml))
            return null;
            
        try
        {
            var doc = XDocument.Parse(xml);
            XNamespace ns = "http://www.opentravel.org/OTA/2003/05";
            
            var uniqueIds = doc.Root?
                .Element(ns + "HotelReservations")?
                .Element(ns + "HotelReservation")?
                .Element(ns + "ResGuests")?
                .Element(ns + "ResGuest")?
                .Element(ns + "Profiles")?
                .Element(ns + "ProfileInfo")?
                .Elements(ns + "UniqueID");
                
            return uniqueIds?
                .FirstOrDefault(e => e.Attribute("Type")?.Value == "5")?
                .Attribute("ID")?
                .Value;
        }
        catch
        {
            return null;
        }
    }
    
    /// <summary>
    /// Parse Travel Agency field from Profile[@ProfileType="4"]/CompanyInfo
    /// </summary>
    private static string? ParseTravelAgencyFieldFromXml(string? xml, string elementName, string? attributeName)
    {
        if (string.IsNullOrEmpty(xml))
            return null;
            
        try
        {
            var doc = XDocument.Parse(xml);
            XNamespace ns = "http://www.opentravel.org/OTA/2003/05";
            
            var profiles = doc.Root?
                .Element(ns + "HotelReservations")?
                .Element(ns + "HotelReservation")?
                .Element(ns + "ResGuests")?
                .Element(ns + "ResGuest")?
                .Element(ns + "Profiles")?
                .Element(ns + "ProfileInfo")?
                .Elements(ns + "Profile");
                
            var companyInfo = profiles?
                .FirstOrDefault(e => e.Attribute("ProfileType")?.Value == "4")?
                .Element(ns + "CompanyInfo");
                
            if (companyInfo == null) return null;
            
            var element = companyInfo.Element(ns + elementName);
            if (element == null) return null;
            
            return attributeName != null ? element.Attribute(attributeName)?.Value : element.Value;
        }
        catch
        {
            return null;
        }
    }
    
    /// <summary>
    /// Parse Travel Agency address field from Profile[@ProfileType="4"]/CompanyInfo/AddressInfo
    /// </summary>
    private static string? ParseTravelAgencyAddressFieldFromXml(string? xml, string elementName, string? attributeName)
    {
        if (string.IsNullOrEmpty(xml))
            return null;
            
        try
        {
            var doc = XDocument.Parse(xml);
            XNamespace ns = "http://www.opentravel.org/OTA/2003/05";
            
            var profiles = doc.Root?
                .Element(ns + "HotelReservations")?
                .Element(ns + "HotelReservation")?
                .Element(ns + "ResGuests")?
                .Element(ns + "ResGuest")?
                .Element(ns + "Profiles")?
                .Element(ns + "ProfileInfo")?
                .Elements(ns + "Profile");
                
            var addressInfo = profiles?
                .FirstOrDefault(e => e.Attribute("ProfileType")?.Value == "4")?
                .Element(ns + "CompanyInfo")?
                .Element(ns + "AddressInfo");
                
            if (addressInfo == null) return null;
            
            var element = addressInfo.Element(ns + elementName);
            if (element == null) return null;
            
            return attributeName != null ? element.Attribute(attributeName)?.Value : element.Value;
        }
        catch
        {
            return null;
        }
    }
    
    /// <summary>
    /// Parse HotelReservationID by ResID_Type
    /// </summary>
    private static string? ParseHotelReservationIdByType(string? xml, string resIdType)
    {
        if (string.IsNullOrEmpty(xml))
            return null;
            
        try
        {
            var doc = XDocument.Parse(xml);
            XNamespace ns = "http://www.opentravel.org/OTA/2003/05";
            
            var hotelReservationIds = doc.Root?
                .Element(ns + "HotelReservations")?
                .Element(ns + "HotelReservation")?
                .Element(ns + "ResGlobalInfo")?
                .Element(ns + "HotelReservationIDs")?
                .Elements(ns + "HotelReservationID");
                
            return hotelReservationIds?
                .FirstOrDefault(e => e.Attribute("ResID_Type")?.Value == resIdType)?
                .Attribute("ResID_Value")?
                .Value;
        }
        catch
        {
            return null;
        }
    }
    
    /// <summary>
    /// Parse Guest Country from ResGuestInfo/Address/CountryName/@Code
    /// </summary>
    private static string? ParseGuestCountryFromXml(string? xml)
    {
        if (string.IsNullOrEmpty(xml))
            return null;
            
        try
        {
            var doc = XDocument.Parse(xml);
            XNamespace ns = "http://www.opentravel.org/OTA/2003/05";
            
            return doc.Root?
                .Element(ns + "HotelReservations")?
                .Element(ns + "HotelReservation")?
                .Element(ns + "ResGuests")?
                .Element(ns + "ResGuest")?
                .Element(ns + "TPA_Extensions")?
                .Element(ns + "ResGuestInfo")?
                .Element(ns + "Address")?
                .Element(ns + "CountryName")?
                .Attribute("Code")?
                .Value;
        }
        catch
        {
            return null;
        }
    }
    
    /// <summary>
    /// Parse CustLoyalty attribute
    /// </summary>
    private static string? ParseCustLoyaltyAttribute(string? xml, string attributeName)
    {
        if (string.IsNullOrEmpty(xml))
            return null;
            
        try
        {
            var doc = XDocument.Parse(xml);
            XNamespace ns = "http://www.opentravel.org/OTA/2003/05";
            
            return doc.Root?
                .Element(ns + "HotelReservations")?
                .Element(ns + "HotelReservation")?
                .Element(ns + "ResGuests")?
                .Element(ns + "ResGuest")?
                .Element(ns + "Profiles")?
                .Element(ns + "ProfileInfo")?
                .Element(ns + "Profile")?
                .Element(ns + "Customer")?
                .Element(ns + "CustLoyalty")?
                .Attribute(attributeName)?
                .Value;
        }
        catch
        {
            return null;
        }
    }
    
    /// <summary>
    /// Parse Profile Type from first Profile element
    /// </summary>
    private static string? ParseProfileTypeFromXml(string? xml)
    {
        if (string.IsNullOrEmpty(xml))
            return null;
            
        try
        {
            var doc = XDocument.Parse(xml);
            XNamespace ns = "http://www.opentravel.org/OTA/2003/05";
            
            return doc.Root?
                .Element(ns + "HotelReservations")?
                .Element(ns + "HotelReservation")?
                .Element(ns + "ResGuests")?
                .Element(ns + "ResGuest")?
                .Element(ns + "Profiles")?
                .Element(ns + "ProfileInfo")?
                .Element(ns + "Profile")?
                .Attribute("ProfileType")?
                .Value;
        }
        catch
        {
            return null;
        }
    }
    
    /// <summary>
    /// Parse Visa Information from Document[@DocType="1"]
    /// </summary>
    private static string? ParseVisaInformationFromXml(string? xml)
    {
        if (string.IsNullOrEmpty(xml))
            return null;
            
        try
        {
            var doc = XDocument.Parse(xml);
            XNamespace ns = "http://www.opentravel.org/OTA/2003/05";
            
            var documents = doc.Root?
                .Element(ns + "HotelReservations")?
                .Element(ns + "HotelReservation")?
                .Element(ns + "ResGuests")?
                .Element(ns + "ResGuest")?
                .Element(ns + "TPA_Extensions")?
                .Element(ns + "ResGuestInfo")?
                .Elements(ns + "Document");
                
            var visaDoc = documents?.FirstOrDefault(e => e.Attribute("DocType")?.Value == "1");
            return visaDoc?.Value;
        }
        catch
        {
            return null;
        }
    }
    
    /// <summary>
    /// Parse Room Upsell element value
    /// </summary>
    private static string? ParseRoomUpsellFromXml(string? xml)
    {
        if (string.IsNullOrEmpty(xml))
            return null;
            
        try
        {
            var doc = XDocument.Parse(xml);
            XNamespace ns = "http://www.opentravel.org/OTA/2003/05";
            
            var roomUpsell = doc.Root?
                .Element(ns + "HotelReservations")?
                .Element(ns + "HotelReservation")?
                .Element(ns + "TPA_Extensions")?
                .Element(ns + "RoomUpsell");
                
            // Return "Yes" if element exists, null otherwise
            return roomUpsell != null ? "Yes" : null;
        }
        catch
        {
            return null;
        }
    }
    
    /// <summary>
    /// Calculate ADR: Total Amount / Nights
    /// Uses Total/@AmountBeforeTax or Total/@AmountAfterTax
    /// </summary>
    private static decimal? CalculateADRFromXml(string? xml)
    {
        if (string.IsNullOrEmpty(xml))
            return null;
            
        try
        {
            // Get TOTAL amount - prefer AmountBeforeTax, fallback to AmountAfterTax
            var totalBeforeTax = ParseXmlAttributeAsDecimal(xml, "HotelReservations/HotelReservation/RoomStays/RoomStay/Total", "AmountBeforeTax");
            var totalAfterTax = ParseXmlAttributeAsDecimal(xml, "HotelReservations/HotelReservation/RoomStays/RoomStay/Total", "AmountAfterTax");
            
            var totalAmount = totalBeforeTax ?? totalAfterTax;
            if (totalAmount == null) return null;
            
            // Get nights
            var nights = CalculateNightsFromXml(xml);
            if (nights == null || nights == 0) return totalAmount;
            
            // Calculate ADR = Total / Nights
            return Math.Round(totalAmount.Value / nights.Value, 2);
        }
        catch
        {
            return null;
        }
    }
}

