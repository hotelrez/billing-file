using BillingFile.Application.Common;
using BillingFile.Application.DTOs;

namespace BillingFile.Application.Interfaces;

/// <summary>
/// Service interface for reservation operations (read-only)
/// </summary>
public interface IReservationService
{
    Task<Result<IEnumerable<ReservationDto>>> GetAllReservationsAsync(CancellationToken cancellationToken = default);
    Task<Result<ReservationDto>> GetReservationByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Result<ReservationDto>> GetReservationByNumberAsync(string reservationNumber, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<ReservationDto>>> GetReservationsByHotelCodeAsync(string hotelCode, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<ReservationDto>>> GetReservationsByStatusAsync(string status, CancellationToken cancellationToken = default);
}

