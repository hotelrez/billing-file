using AutoMapper;
using BillingFile.Application.Common;
using BillingFile.Application.DTOs;
using BillingFile.Application.Interfaces;
using BillingFile.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using IBillingDataAccess = BillingFile.Domain.Interfaces.IBillingDataAccess;

namespace BillingFile.Application.Services;

/// <summary>
/// Service implementation for reservation operations (read-only)
/// </summary>
public class ReservationService : IReservationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBillingDataAccess _billingDataAccess;
    private readonly ICurrencyConversionService _currencyConversionService;
    private readonly IMapper _mapper;
    private readonly ILogger<ReservationService> _logger;

    public ReservationService(
        IUnitOfWork unitOfWork,
        IBillingDataAccess billingDataAccess,
        ICurrencyConversionService currencyConversionService,
        IMapper mapper,
        ILogger<ReservationService> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _billingDataAccess = billingDataAccess ?? throw new ArgumentNullException(nameof(billingDataAccess));
        _currencyConversionService = currencyConversionService ?? throw new ArgumentNullException(nameof(currencyConversionService));
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

    public async Task<Result<IEnumerable<BillingDto>>> GetBillingByArrivalDateRangeAsync(
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Executing GetBillingFileReservations SP with FromDate={StartDate} and ToDate={EndDate}", 
                startDate, endDate);

            // Validate date range
            if (startDate > endDate)
            {
                return Result<IEnumerable<BillingDto>>.Failure("Start date must be before or equal to end date");
            }

            // Execute stored procedure instead of direct table query
            var spResults = await _billingDataAccess.GetBillingFileReservationsAsync(startDate, endDate, cancellationToken);

            // Map SP results to BillingDto
            var dtos = _mapper.Map<List<BillingDto>>(spResults);

            // Get all hotel currencies for matching
            var hotelCurrencies = await _unitOfWork.HotelBillingCurrencies.GetAllAsync(cancellationToken);
            var currencyLookup = hotelCurrencies.ToDictionary(h => h.HotelID, h => h.Currency);
            
            _logger.LogInformation("Loaded {Count} hotel currency mappings for conversion", currencyLookup.Count);

            // Process each billing record for currency conversion
            foreach (var dto in dtos)
            {
                try
                {
                    // Copy revenue values: if one field has value and the other is null, copy to the null field
                    dto.Reservation_Revenue_Before_Tax ??= dto.Reservation_Revenue_After_Tax;
                    dto.Reservation_Revenue_After_Tax ??= dto.Reservation_Revenue_Before_Tax;
                    
                    // Check if we have currency info for this hotel
                    if (dto.Hotel_ID.HasValue && currencyLookup.TryGetValue(dto.Hotel_ID.Value, out var expectedCurrency))
                    {
                        var billingCurrency = dto.Currency;
                        
                        _logger.LogDebug(
                            "Checking Hotel_ID {HotelId}: Billing Currency={BillingCurrency}, Expected Currency={ExpectedCurrency}",
                            dto.Hotel_ID, billingCurrency, expectedCurrency);
                        
                        // If currencies don't match, convert all prices
                        if (!string.IsNullOrEmpty(billingCurrency) && 
                            !string.IsNullOrEmpty(expectedCurrency) &&
                            !billingCurrency.Equals(expectedCurrency, StringComparison.OrdinalIgnoreCase))
                        {
                            _logger.LogInformation(
                                "Currency mismatch for Hotel_ID {HotelId}: Billing={BillingCurrency}, Expected={ExpectedCurrency}. Converting prices...",
                                dto.Hotel_ID, billingCurrency, expectedCurrency);

                            // Parse Confirm_Date to get the exchange rate date
                            var confirmDate = DateTime.TryParse(dto.Confirm_Date, out var parsedDate) 
                                ? parsedDate 
                                : DateTime.UtcNow;

                            // Convert all monetary fields
                            if (dto.Reservation_Revenue_Before_Tax.HasValue)
                            {
                                dto.Reservation_Revenue_Before_Tax = await _currencyConversionService.ConvertAmountAsync(
                                    dto.Reservation_Revenue_Before_Tax.Value,
                                    billingCurrency,
                                    expectedCurrency,
                                    confirmDate,
                                    cancellationToken);
                            }

                            if (dto.Reservation_Revenue_After_Tax.HasValue)
                            {
                                dto.Reservation_Revenue_After_Tax = await _currencyConversionService.ConvertAmountAsync(
                                    dto.Reservation_Revenue_After_Tax.Value,
                                    billingCurrency,
                                    expectedCurrency,
                                    confirmDate,
                                    cancellationToken);
                            }

                            if (dto.Rate_Revenue_With_Inclusive_Tax_Amt.HasValue)
                            {
                                dto.Rate_Revenue_With_Inclusive_Tax_Amt = await _currencyConversionService.ConvertAmountAsync(
                                    dto.Rate_Revenue_With_Inclusive_Tax_Amt.Value,
                                    billingCurrency,
                                    expectedCurrency,
                                    confirmDate,
                                    cancellationToken);
                            }

                            if (dto.ADR.HasValue)
                            {
                                dto.ADR = await _currencyConversionService.ConvertAmountAsync(
                                    dto.ADR.Value,
                                    billingCurrency,
                                    expectedCurrency,
                                    confirmDate,
                                    cancellationToken);
                            }

                            // Update the currency field to reflect the conversion
                            dto.Currency = expectedCurrency;

                            _logger.LogInformation(
                                "Converted prices for Hotel_ID {HotelId} from {FromCurrency} to {ToCurrency}",
                                dto.Hotel_ID, billingCurrency, expectedCurrency);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, 
                        "Error converting currency for Hotel_ID {HotelId}. Skipping conversion for this record.",
                        dto.Hotel_ID);
                    // Continue processing other records even if one fails
                }
            }

            _logger.LogInformation("Successfully retrieved and processed {Count} billing records from GetBillingFileReservations SP", 
                dtos.Count);

            return Result<IEnumerable<BillingDto>>.Success(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing GetBillingFileReservations SP with FromDate={StartDate}, ToDate={EndDate}", 
                startDate, endDate);
            return Result<IEnumerable<BillingDto>>.Failure($"Error: {ex.Message}");
        }
    }
}

