namespace BillingFile.Domain.Entities;

/// <summary>
/// Entity mapping to Play.dbo.FullReservation table (106 columns)
/// Exact mapping from actual database schema
/// </summary>
public class FullReservation
{
    // Primary Key
    public int ID { get; set; }
    
    // Chain Information
    public string? Chain_Name { get; set; }
    public int? Chain_ID { get; set; }
    
    // Hotel Information
    public string? Hotel_Name { get; set; }
    public int? Hotel_ID { get; set; }
    public int? SAP_ID { get; set; }
    public string? Hotel_Code { get; set; }
    
    // Transaction Information
    public string? Billing_Description { get; set; }
    public DateTime? Transaction_Time_Stamp { get; set; }
    public int? Fax_Notification_Count { get; set; }
    
    // Channel Information
    public string? Channel { get; set; }
    public string? Secondary_Source { get; set; }
    public string? Sub_Source { get; set; }
    public string? Sub_Source_Code { get; set; }
    
    // PMS Codes
    public string? PMS_Rate_Type_Code { get; set; }
    public string? PMS_Room_Type_Code { get; set; }
    public string? Market_Source_Code { get; set; }
    public string? Market_Segment_Code { get; set; }
    public string? User_Login { get; set; }
    
    // Status Information
    public string? Status { get; set; }
    public string? Confirm_Number { get; set; }
    public DateTime? Confirm_Date { get; set; }
    public string? Cancel_Number { get; set; }
    public string? Cancel_Date { get; set; }
    
    // Guest Information
    public string? Salutation { get; set; }
    public string? Guest_First_Name { get; set; }
    public string? Guest_Last_Name { get; set; }
    public string? Customer_ID { get; set; }
    
    // Customer Address
    public string? Customer_Street_Address1 { get; set; }
    public string? Customer_Street_Address2 { get; set; }
    public string? Customer_City { get; set; }
    public string? Customer_State { get; set; }
    public string? Customer_Zip_Postal_Code { get; set; }
    public string? Customer_Phone_Number { get; set; }
    public string? Customer_Country { get; set; }
    public string? Customer_Area { get; set; }
    public string? Customer_Region { get; set; }
    public string? Customer_Company { get; set; }
    
    // Reservation Dates
    public DateTime? Arrival_Date { get; set; }
    public DateTime? Departure_Date { get; set; }
    public int? Booking_Lead_Time { get; set; }
    
    // Rate Information
    public string? Rate_Category_Name { get; set; }
    public string? Rate_Category_Code { get; set; }
    public string? Rate_Type_Name { get; set; }
    public string? Rate_Type_Code { get; set; }
    
    // Room Information
    public string? Room_Type_Name { get; set; }
    public string? Room_Type_Code { get; set; }
    
    // Reservation Details
    public int? Nights { get; set; }
    public decimal? ADR { get; set; }
    public int? Rooms { get; set; }
    public decimal? Reservation_Revenue { get; set; }
    public string? Currency { get; set; }
    
    // Travel Agency Information
    public string? Travel_Industry_ID { get; set; }
    public string? Travel_Agency_Name { get; set; }
    public string? Travel_Agency_Street_Address { get; set; }
    public string? Travel_Agency_Street_Address2 { get; set; }
    public string? Travel_Agency_City { get; set; }
    public string? Travel_Agency_State { get; set; }
    public string? Travel_Agency_Zip_Postal_Code { get; set; }
    public string? Travel_Agency_Phone { get; set; }
    public string? Travel_Agency_Fax { get; set; }
    public string? Travel_Agency_Country { get; set; }
    public string? Travel_Agent_Area { get; set; }
    public string? Travel_Agent_Region { get; set; }
    public string? Travel_Agent_Email { get; set; }
    
    // Consortia Information
    public int? Consortia_Count { get; set; }
    public string? Consortia_Name_If_Part_Of_One_Consortia_Only { get; set; }
    
    // Revenue Information
    public decimal? Total_Dynamic_Package_Revenue { get; set; }
    
    // Customer Contact
    public string? Opt_In { get; set; }
    public string? Customer_Email { get; set; }
    
    // Guest Count
    public int? Total_Guest_Count { get; set; }
    public int? Adult_Count { get; set; }
    public int? Children_Count { get; set; }
    
    // Payment Information
    public string? Credit_Card_Type { get; set; }
    
    // Action Information
    public string? Action_Type { get; set; }
    public string? Share_With { get; set; }
    
    // Day of Week
    public string? Arrival_DOW { get; set; }
    public string? Departure_DOW { get; set; }
    
    // Itinerary
    public string? Itinerary_Number { get; set; }
    
    // Secondary Currency
    public string? Secondary_Currency { get; set; }
    public string? Secondary_Currency_Exchange_Rate { get; set; }
    public string? Secondary_Currency_ADR { get; set; }
    public string? Secondary_Currency_Reservation_Revenue { get; set; }
    public string? Secondary_Currency_Total_Dynamic_Package_Revenue { get; set; }
    
    // Commission
    public int? Commission_Percent { get; set; }
    
    // Codes
    public string? Membership_Number { get; set; }
    public string? Corporate_Code { get; set; }
    public string? Promotional_Code { get; set; }
    public string? CRO_Code { get; set; }
    public string? Channel_Connect_Confirm_NO { get; set; }
    
    // Guest Details
    public string? Primary_Guest { get; set; }
    
    // Loyalty Program
    public string? Loyalty_Program { get; set; }
    public string? Loyalty_Number { get; set; }
    public string? VIP_Level { get; set; }
    
    // Template Information
    public string? XBE_Template_Name { get; set; }
    public string? XBE_Shell_Name { get; set; }
    public string? Profile_Type_Selection { get; set; }
    
    // Loyalty Level
    public string? Loyalty_Level_Code { get; set; }
    public string? Loyalty_Level_Name { get; set; }
    
    // Brand Information
    public string? Brand_Code { get; set; }
    public string? Brand_Name { get; set; }
    
    // Original Room Type
    public string? Original_Room_Type_Code { get; set; }
    public string? Original_Room_Type_Name { get; set; }
    
    // Room Upsell
    public string? Room_Upsell { get; set; }
    public decimal? Room_Upsell_Revenue { get; set; }
    public string? Sec_Cur_Room_Upsell_Revenue { get; set; }
}

