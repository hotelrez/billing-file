using BillingFile.Application.Common;
using BillingFile.Application.DTOs;

namespace BillingFile.Application.Interfaces;

/// <summary>
/// Service interface for reservation operations (read-only) - based on actual FullReservation schema
/// </summary>
public interface IReservationService
{
    Task<Result<IEnumerable<ReservationDto>>> GetAllReservationsAsync(CancellationToken cancellationToken = default);
    Task<Result<ReservationDto>> GetReservationByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Result<ReservationDto>> GetReservationByConfirmNumberAsync(string confirmNumber, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<ReservationDto>>> GetReservationsByHotelCodeAsync(string hotelCode, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<ReservationDto>>> GetReservationsByStatusAsync(string status, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<ReservationDto>>> GetReservationsByHotelIdAsync(int hotelId, CancellationToken cancellationToken = default);
}

