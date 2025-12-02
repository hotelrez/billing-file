using BillingFile.Application.Common;
using BillingFile.Application.DTOs;

namespace BillingFile.Application.Interfaces;

/// <summary>
/// Service interface for hotel operations (read-only)
/// </summary>
public interface IHotelService
{
    Task<Result<IEnumerable<HotelDto>>> GetAllHotelsAsync(CancellationToken cancellationToken = default);
    Task<Result<HotelDto>> GetHotelByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Result<HotelDto>> GetHotelByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<HotelDto>>> GetActiveHotelsAsync(CancellationToken cancellationToken = default);
}

