namespace BillingFile.Application.DTOs;

/// <summary>
/// Data Transfer Object for FullReservation (106 columns total)
/// Includes most commonly used fields from actual Play.dbo.FullReservation schema
/// </summary>
public class ReservationDto
{
    // Primary Key
    public int ID { get; set; }
    
    // Hotel Information
    public string? Chain_Name { get; set; }
    public int? Chain_ID { get; set; }
    public string? Hotel_Name { get; set; }
    public int? Hotel_ID { get; set; }
    public string? Hotel_Code { get; set; }
    
    // Status
    public string? Status { get; set; }
    public string? Confirm_Number { get; set; }
    public DateTime? Confirm_Date { get; set; }
    public string? Cancel_Number { get; set; }
    public string? Cancel_Date { get; set; }
    
    // Guest Information
    public string? Guest_First_Name { get; set; }
    public string? Guest_Last_Name { get; set; }
    public string? Customer_Email { get; set; }
    public string? Customer_Phone_Number { get; set; }
    public string? Customer_Company { get; set; }
    
    // Customer Address
    public string? Customer_Street_Address1 { get; set; }
    public string? Customer_City { get; set; }
    public string? Customer_State { get; set; }
    public string? Customer_Country { get; set; }
    
    // Reservation Dates
    public DateTime? Arrival_Date { get; set; }
    public DateTime? Departure_Date { get; set; }
    public int? Nights { get; set; }
    public string? Arrival_DOW { get; set; }
    public string? Departure_DOW { get; set; }
    
    // Room Information
    public string? Room_Type_Name { get; set; }
    public string? Room_Type_Code { get; set; }
    public int? Rooms { get; set; }
    
    // Rate Information
    public string? Rate_Type_Name { get; set; }
    public string? Rate_Category_Name { get; set; }
    public decimal? ADR { get; set; }
    public decimal? Reservation_Revenue { get; set; }
    public string? Currency { get; set; }
    
    // Guest Count
    public int? Total_Guest_Count { get; set; }
    public int? Adult_Count { get; set; }
    public int? Children_Count { get; set; }
    
    // Channel Information
    public string? Channel { get; set; }
    public string? Sub_Source { get; set; }
    
    // Travel Agency
    public string? Travel_Agency_Name { get; set; }
    public string? Travel_Industry_ID { get; set; }
    
    // Loyalty
    public string? Loyalty_Program { get; set; }
    public string? Loyalty_Number { get; set; }
    public string? Loyalty_Level_Name { get; set; }
    
    // Codes
    public string? Corporate_Code { get; set; }
    public string? Promotional_Code { get; set; }
    
    // Transaction
    public DateTime? Transaction_Time_Stamp { get; set; }
    public int? Booking_Lead_Time { get; set; }
}
