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
    
    [JsonPropertyName("Status")]
    public string? Status { get; set; }  // Parsed from xml: /OTA_HotelResNotifRQ/HotelReservations/HotelReservation/@ResStatus
    
    [JsonPropertyName("Confirm_Date")]
    public string? Confirm_Date { get; set; }  // Parsed from xml: /OTA_HotelResNotifRQ/HotelReservations/HotelReservation/TPA_Extensions/ConfirmDate/@ConfirmDateTime
    
    [JsonPropertyName("Cancel_Number")]
    public string? Cancel_Number { get; set; }  // Parsed from xml: /OTA_HotelResNotifRQ/HotelReservations/HotelReservation/ResGlobalInfo/HotelReservationIDs/HotelReservationID[@ResID_Type="15"]/@ResID_Value
    
    [JsonPropertyName("Cancel_Date")]
    public string? Cancel_Date { get; set; }  // Parsed from xml: /OTA_HotelResNotifRQ/HotelReservations/HotelReservation/@LastModifyDateTime
    
    [JsonPropertyName("Cancellation_Channel")]
    public string? Cancellation_Channel { get; set; }  // Parsed from xml: /OTA_HotelResNotifRQ/HotelReservations/HotelReservation/TPA_Extensions/LastModifyingChannel/@Type
    
    [JsonPropertyName("Cancellation_Secondary_Channel")]
    public string? Cancellation_Secondary_Channel { get; set; }  // Parsed from xml: /OTA_HotelResNotifRQ/HotelReservations/HotelReservation/TPA_Extensions/LastModifyingChannel/CompanyName/@Code
    
    [JsonPropertyName("Reinstatement_Date")]
    public string? Reinstatement_Date { get; set; }  // Parsed from xml: /OTA_HotelResNotifRQ/HotelReservations/HotelReservation/TPA_Extensions/ReinstatementInfo/@LastReinstatementDateTime
    
    [JsonPropertyName("Salutation")]
    public string? Salutation { get; set; }  // Parsed from xml: /OTA_HotelResNotifRQ/HotelReservations/HotelReservation/ResGuests/ResGuest/TPA_Extensions/ResGuestInfo/PersonName/NamePrefix
    
    [JsonPropertyName("Guest_First_Name")]
    public string? Guest_First_Name { get; set; }  // Parsed from xml: /OTA_HotelResNotifRQ/HotelReservations/HotelReservation/ResGuests/ResGuest/TPA_Extensions/ResGuestInfo/PersonName/GivenName
    
    [JsonPropertyName("Guest_Last_Name")]
    public string? Guest_Last_Name { get; set; }  // Parsed from xml: /OTA_HotelResNotifRQ/HotelReservations/HotelReservation/ResGuests/ResGuest/TPA_Extensions/ResGuestInfo/PersonName/Surname
    
    [JsonPropertyName("Arrival_Date")]
    public string? Arrival_Date { get; set; }  // Parsed from xml: /OTA_HotelResNotifRQ/HotelReservations/HotelReservation/RoomStays/RoomStay/TimeSpan/@Start
    
    [JsonPropertyName("Depart_Date")]
    public string? Depart_Date { get; set; }  // Parsed from xml: /OTA_HotelResNotifRQ/HotelReservations/HotelReservation/RoomStays/RoomStay/TimeSpan/@End
    
    [JsonPropertyName("Rate_Category_Name")]
    public string? Rate_Category_Name { get; set; }  // Parsed from xml: .../AdditionalDetail[@Type="CategoryName"]/DetailDescription/Text
    
    [JsonPropertyName("Rate_Category_Code")]
    public string? Rate_Category_Code { get; set; }  // Parsed from xml: /OTA_HotelResNotifRQ/HotelReservations/HotelReservation/RoomStays/RoomStay/RoomRates/RoomRate/@RatePlanCategory
    
    [JsonPropertyName("Rate_Type_Name")]
    public string? Rate_Type_Name { get; set; }  // Parsed from xml: .../AdditionalDetail[@Type="Name"]/DetailDescription/Text (RatePlans)
    
    [JsonPropertyName("Rate_Type_Code")]
    public string? Rate_Type_Code { get; set; }  // Parsed from xml: /OTA_HotelResNotifRQ/HotelReservations/HotelReservation/RoomStays/RoomStay/RatePlans/RatePlan/@RatePlanCode
    
    [JsonPropertyName("Room_Type_Name")]
    public string? Room_Type_Name { get; set; }  // Parsed from xml: .../AdditionalDetail[@Type="Name"]/DetailDescription/Text (RoomTypes)
    
    [JsonPropertyName("Room_Type_Code")]
    public string? Room_Type_Code { get; set; }  // Parsed from xml: /OTA_HotelResNotifRQ/HotelReservations/HotelReservation/RoomStays/RoomStay/RoomTypes/RoomType/@RoomTypeCode
    
    [JsonPropertyName("Nights")]
    public int? Nights { get; set; }  // Calculated: Depart_Date - Arrival_Date in days
    
    // ===== ADD MORE FIELDS BELOW =====
    // Note: Fields must exist in GetBillingFileReservations SP output
}

