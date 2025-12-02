using AutoMapper;
using BillingFile.Application.Common;
using BillingFile.Application.DTOs;
using BillingFile.Application.Interfaces;
using BillingFile.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace BillingFile.Application.Services;

/// <summary>
/// Service implementation for hotel operations (read-only)
/// </summary>
public class HotelService : IHotelService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<HotelService> _logger;

    public HotelService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<HotelService> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Result<IEnumerable<HotelDto>>> GetAllHotelsAsync(
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all hotels");

            var hotels = await _unitOfWork.Hotels.GetAllAsync(cancellationToken);
            var dtos = _mapper.Map<IEnumerable<HotelDto>>(hotels);

            _logger.LogInformation("Successfully retrieved {Count} hotels", dtos.Count());
            return Result<IEnumerable<HotelDto>>.Success(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving hotels");
            return Result<IEnumerable<HotelDto>>.Failure("An error occurred while retrieving hotels");
        }
    }

    public async Task<Result<HotelDto>> GetHotelByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving hotel with ID: {Id}", id);

            var hotel = await _unitOfWork.Hotels.GetByIdAsync(id, cancellationToken);
            
            if (hotel == null)
            {
                _logger.LogWarning("Hotel with ID {Id} not found", id);
                return Result<HotelDto>.Failure($"Hotel with ID {id} not found");
            }

            var dto = _mapper.Map<HotelDto>(hotel);
            return Result<HotelDto>.Success(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving hotel with ID: {Id}", id);
            return Result<HotelDto>.Failure("An error occurred while retrieving the hotel");
        }
    }

    public async Task<Result<HotelDto>> GetHotelByTrustCodeAsync(
        string trustCode,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving hotel with TrustCode: {TrustCode}", trustCode);

            var hotels = await _unitOfWork.Hotels.FindAsync(
                h => h.TrustCode == trustCode,
                cancellationToken);

            var hotel = hotels.FirstOrDefault();
            
            if (hotel == null)
            {
                _logger.LogWarning("Hotel with TrustCode {TrustCode} not found", trustCode);
                return Result<HotelDto>.Failure($"Hotel with TrustCode {trustCode} not found");
            }

            var dto = _mapper.Map<HotelDto>(hotel);
            return Result<HotelDto>.Success(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving hotel with TrustCode: {TrustCode}", trustCode);
            return Result<HotelDto>.Failure("An error occurred while retrieving the hotel");
        }
    }

    public async Task<Result<IEnumerable<HotelDto>>> GetActiveHotelsAsync(
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving active hotels");

            var hotels = await _unitOfWork.Hotels.FindAsync(
                h => h.Active != 0,
                cancellationToken);

            var dtos = _mapper.Map<IEnumerable<HotelDto>>(hotels);

            _logger.LogInformation("Successfully retrieved {Count} active hotels", dtos.Count());
            return Result<IEnumerable<HotelDto>>.Success(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving active hotels");
            return Result<IEnumerable<HotelDto>>.Failure("An error occurred while retrieving active hotels");
        }
    }

    public async Task<Result<IEnumerable<HotelDto>>> GetHotelsByCountryAsync(
        string countryCode,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving hotels for country: {CountryCode}", countryCode);

            var hotels = await _unitOfWork.Hotels.FindAsync(
                h => h.CountryCode == countryCode,
                cancellationToken);

            var dtos = _mapper.Map<IEnumerable<HotelDto>>(hotels);

            _logger.LogInformation("Successfully retrieved {Count} hotels for country: {CountryCode}", 
                dtos.Count(), countryCode);

            return Result<IEnumerable<HotelDto>>.Success(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving hotels for country: {CountryCode}", countryCode);
            return Result<IEnumerable<HotelDto>>.Failure("An error occurred while retrieving hotels");
        }
    }
}

