namespace BillingFile.Application.DTOs;

/// <summary>
/// Data Transfer Object for FullReservation - matches actual Play.dbo.FullReservation schema
/// </summary>
public class ReservationDto
{
    public int Id { get; set; }
    public string ReservationNumber { get; set; } = string.Empty;
    public string GuestName { get; set; } = string.Empty;
    public string? GuestEmail { get; set; }
    public string? GuestPhone { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public int NumberOfGuests { get; set; }
    public int NumberOfNights { get; set; }
    public string? RoomType { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? HotelCode { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
}

