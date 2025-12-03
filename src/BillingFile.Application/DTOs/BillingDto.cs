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
    
    [JsonPropertyName("Rooms")]
    public int? Rooms { get; set; }  // Parsed from xml: .../RoomRates/RoomRate/@NumberOfUnits
    
    [JsonPropertyName("Reservation_Revenue_Before_Tax")]
    public decimal? Reservation_Revenue_Before_Tax { get; set; }  // Parsed from xml: .../Total/@AmountBeforeTax
    
    [JsonPropertyName("Reservation_Revenue_After_Tax")]
    public decimal? Reservation_Revenue_After_Tax { get; set; }  // Parsed from xml: .../Total/@AmountAfterTax
    
    [JsonPropertyName("Rate_Revenue_With_Inclusive_Tax_Amt")]
    public decimal? Rate_Revenue_With_Inclusive_Tax_Amt { get; set; }  // Parsed from xml: .../Rates/Rate/Base/@AmountAfterTax
    
    [JsonPropertyName("Currency")]
    public string? Currency { get; set; }  // Parsed from xml: .../Total/@CurrencyCode
    
    [JsonPropertyName("Loyalty_Points_Payment")]
    public string? Loyalty_Points_Payment { get; set; }  // Parsed from xml: .../LoyaltyRedemption/@RedemptionQuantity
    
    [JsonPropertyName("Total_Rate_Loyalty_Points")]
    public string? Total_Rate_Loyalty_Points { get; set; }  // Parsed from xml: .../LoyaltyRedemption/@RedemptionQuantity
    
    [JsonPropertyName("Travel_Industry_ID")]
    public string? Travel_Industry_ID { get; set; }  // Parsed from xml: .../UniqueID[@Type="5"]/@ID
    
    [JsonPropertyName("Travel_Agency_Name")]
    public string? Travel_Agency_Name { get; set; }  // Parsed from xml: .../Profile[@ProfileType="4"]/CompanyInfo/CompanyName
    
    [JsonPropertyName("Travel_Agency_Street_Address")]
    public string? Travel_Agency_Street_Address { get; set; }  // Parsed from xml: .../AddressInfo/AddressLine
    
    [JsonPropertyName("Travel_Agency_City")]
    public string? Travel_Agency_City { get; set; }  // Parsed from xml: .../AddressInfo/CityName
    
    [JsonPropertyName("Travel_Agency_State")]
    public string? Travel_Agency_State { get; set; }  // Parsed from xml: .../AddressInfo/StateProv/@StateCode
    
    [JsonPropertyName("Travel_Agency_Zip_Postal_Code")]
    public string? Travel_Agency_Zip_Postal_Code { get; set; }  // Parsed from xml: .../AddressInfo/PostalCode
    
    [JsonPropertyName("Travel_Agency_Country")]
    public string? Travel_Agency_Country { get; set; }  // Parsed from xml: .../AddressInfo/CountryName/@Code
    
    [JsonPropertyName("Total_Dynamic_Package_Revenue")]
    public string? Total_Dynamic_Package_Revenue { get; set; }  // Parsed from xml: .../Packages/@PackageTotalAmount
    
    [JsonPropertyName("Itinerary_Number")]
    public string? Itinerary_Number { get; set; }  // Parsed from xml: .../HotelReservationID[@ResID_Type="34"]/@ResID_Value
    
    [JsonPropertyName("Channel_Connect_Confirm_Number")]
    public string? Channel_Connect_Confirm_Number { get; set; }  // Parsed from xml: .../HotelReservationID[@ResID_Type="13"]/@ResID_Value
    
    [JsonPropertyName("Commissionable_Percent")]
    public string? Commissionable_Percent { get; set; }  // Parsed from xml: .../RatePlan/Commission/@Percent
    
    [JsonPropertyName("Guest_Country")]
    public string? Guest_Country { get; set; }  // Parsed from xml: .../ResGuestInfo/Address/CountryName/@Code
    
    [JsonPropertyName("Template_Name")]
    public string? Template_Name { get; set; }  // Parsed from xml: .../POS/Source/TPA_Extensions/Template/@Name
    
    [JsonPropertyName("Shell_Name")]
    public string? Shell_Name { get; set; }  // Parsed from xml: .../POS/Source/TPA_Extensions/Shell/@Name
    
    [JsonPropertyName("Loyalty_Type")]
    public string? Loyalty_Type { get; set; }  // Parsed from xml: .../CustLoyalty/@Remark
    
    [JsonPropertyName("Loyalty_Program")]
    public string? Loyalty_Program { get; set; }  // Parsed from xml: .../Membership/@ProgramCode
    
    [JsonPropertyName("Loyalty_Number")]
    public string? Loyalty_Number { get; set; }  // Parsed from xml: .../Membership/@AccountID
    
    [JsonPropertyName("Loyalty_Level_Name")]
    public string? Loyalty_Level_Name { get; set; }  // Parsed from xml: .../CustLoyalty/@AllianceLoyaltyLevelName
    
    [JsonPropertyName("Loyalty_Level_Code")]
    public string? Loyalty_Level_Code { get; set; }  // Parsed from xml: .../CustLoyalty/@LoyalLevel
    
    [JsonPropertyName("Profile_Type_Selection")]
    public string? Profile_Type_Selection { get; set; }  // Parsed from xml: .../Profile/@ProfileType
    
    [JsonPropertyName("Visa_Information")]
    public string? Visa_Information { get; set; }  // Parsed from xml: .../Document[@DocType="1"]
    
    [JsonPropertyName("Room_Upsell")]
    public string? Room_Upsell { get; set; }  // Parsed from xml: .../TPA_Extensions/RoomUpsell
    
    [JsonPropertyName("Room_Upsell_Revenue")]
    public string? Room_Upsell_Revenue { get; set; }  // Parsed from xml: .../RoomUpsell/@Revenue
    
    [JsonPropertyName("Coupon_Offer_Code")]
    public string? Coupon_Offer_Code { get; set; }  // Parsed from xml: .../CouponOffer/@CouponCode
    
    [JsonPropertyName("ADR")]
    public decimal? ADR { get; set; }  // Parsed from xml: .../Rates/Rate/Base/@AmountBeforeTax
    
    // ===== ADD MORE FIELDS BELOW =====
    // Note: Fields must exist in GetBillingFileReservations SP output
}

