using AutoMapper;
using BillingFile.Application.Common;
using BillingFile.Application.DTOs;
using BillingFile.Application.Interfaces;
using BillingFile.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace BillingFile.Application.Services;

/// <summary>
/// Service implementation for reservation operations (read-only)
/// </summary>
public class ReservationService : IReservationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<ReservationService> _logger;

    public ReservationService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<ReservationService> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Result<IEnumerable<ReservationDto>>> GetAllReservationsAsync(
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all reservations");

            var reservations = await _unitOfWork.Reservations.GetAllAsync(cancellationToken);
            var dtos = _mapper.Map<IEnumerable<ReservationDto>>(reservations);

            _logger.LogInformation("Successfully retrieved {Count} reservations", dtos.Count());
            return Result<IEnumerable<ReservationDto>>.Success(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving reservations");
            return Result<IEnumerable<ReservationDto>>.Failure("An error occurred while retrieving reservations");
        }
    }

    public async Task<Result<ReservationDto>> GetReservationByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving reservation with ID: {Id}", id);

            var reservation = await _unitOfWork.Reservations.GetByIdAsync(id, cancellationToken);
            
            if (reservation == null)
            {
                _logger.LogWarning("Reservation with ID {Id} not found", id);
                return Result<ReservationDto>.Failure($"Reservation with ID {id} not found");
            }

            var dto = _mapper.Map<ReservationDto>(reservation);
            return Result<ReservationDto>.Success(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving reservation with ID: {Id}", id);
            return Result<ReservationDto>.Failure("An error occurred while retrieving the reservation");
        }
    }

    public async Task<Result<ReservationDto>> GetReservationByConfirmNumberAsync(
        string confirmNumber,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving reservation with confirm number: {ConfirmNumber}", confirmNumber);

            var reservations = await _unitOfWork.Reservations.FindAsync(
                r => r.Confirm_Number == confirmNumber,
                cancellationToken);

            var reservation = reservations.FirstOrDefault();
            
            if (reservation == null)
            {
                _logger.LogWarning("Reservation with confirm number {ConfirmNumber} not found", confirmNumber);
                return Result<ReservationDto>.Failure($"Reservation with confirm number {confirmNumber} not found");
            }

            var dto = _mapper.Map<ReservationDto>(reservation);
            return Result<ReservationDto>.Success(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving reservation with confirm number: {ConfirmNumber}", confirmNumber);
            return Result<ReservationDto>.Failure("An error occurred while retrieving the reservation");
        }
    }

    public async Task<Result<IEnumerable<ReservationDto>>> GetReservationsByHotelCodeAsync(
        string hotelCode,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving reservations for hotel code: {HotelCode}", hotelCode);

            var reservations = await _unitOfWork.Reservations.FindAsync(
                r => r.Hotel_Code == hotelCode,
                cancellationToken);

            var dtos = _mapper.Map<IEnumerable<ReservationDto>>(reservations);

            _logger.LogInformation("Successfully retrieved {Count} reservations for hotel code: {HotelCode}", 
                dtos.Count(), hotelCode);

            return Result<IEnumerable<ReservationDto>>.Success(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving reservations for hotel code: {HotelCode}", hotelCode);
            return Result<IEnumerable<ReservationDto>>.Failure("An error occurred while retrieving reservations");
        }
    }

    public async Task<Result<IEnumerable<ReservationDto>>> GetReservationsByHotelIdAsync(
        int hotelId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving reservations for hotel ID: {HotelId}", hotelId);

            var reservations = await _unitOfWork.Reservations.FindAsync(
                r => r.Hotel_ID == hotelId,
                cancellationToken);

            var dtos = _mapper.Map<IEnumerable<ReservationDto>>(reservations);

            _logger.LogInformation("Successfully retrieved {Count} reservations for hotel ID: {HotelId}", 
                dtos.Count(), hotelId);

            return Result<IEnumerable<ReservationDto>>.Success(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving reservations for hotel ID: {HotelId}", hotelId);
            return Result<IEnumerable<ReservationDto>>.Failure("An error occurred while retrieving reservations");
        }
    }

    public async Task<Result<IEnumerable<ReservationDto>>> GetReservationsByStatusAsync(
        string status,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving reservations with status: {Status}", status);

            var reservations = await _unitOfWork.Reservations.FindAsync(
                r => r.Status == status,
                cancellationToken);

            var dtos = _mapper.Map<IEnumerable<ReservationDto>>(reservations);

            _logger.LogInformation("Successfully retrieved {Count} reservations with status: {Status}", 
                dtos.Count(), status);

            return Result<IEnumerable<ReservationDto>>.Success(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving reservations by status: {Status}", status);
            return Result<IEnumerable<ReservationDto>>.Failure("An error occurred while retrieving reservations");
        }
    }
}

