using BillingFile.Application.Common;
using BillingFile.Application.DTOs;

namespace BillingFile.Application.Interfaces;

/// <summary>
/// Service interface for hotel operations (read-only) - based on actual MemberPortal.dbo.Hotel schema
/// </summary>
public interface IHotelService
{
    Task<Result<IEnumerable<HotelDto>>> GetAllHotelsAsync(CancellationToken cancellationToken = default);
    Task<Result<HotelDto>> GetHotelByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Result<HotelDto>> GetHotelByTrustCodeAsync(string trustCode, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<HotelDto>>> GetActiveHotelsAsync(CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<HotelDto>>> GetHotelsByCountryAsync(string countryCode, CancellationToken cancellationToken = default);
}

