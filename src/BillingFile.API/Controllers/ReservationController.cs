using BillingFile.Application.DTOs;
using BillingFile.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BillingFile.API.Controllers;

/// <summary>
/// API controller for reservation operations (read-only)
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ReservationController : ControllerBase
{
    private readonly IReservationService _reservationService;
    private readonly ILogger<ReservationController> _logger;

    public ReservationController(
        IReservationService reservationService,
        ILogger<ReservationController> logger)
    {
        _reservationService = reservationService ?? throw new ArgumentNullException(nameof(reservationService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Get all reservations
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of reservations</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ReservationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllReservations(CancellationToken cancellationToken)
    {
        var result = await _reservationService.GetAllReservationsAsync(cancellationToken);

        if (!result.IsSuccess)
        {
            return StatusCode(500, new { error = result.ErrorMessage });
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Get reservation by ID
    /// </summary>
    /// <param name="id">Reservation ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Reservation</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ReservationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetReservationById(int id, CancellationToken cancellationToken)
    {
        var result = await _reservationService.GetReservationByIdAsync(id, cancellationToken);

        if (!result.IsSuccess)
        {
            return NotFound(new { error = result.ErrorMessage });
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Get reservation by confirmation number
    /// </summary>
    /// <param name="confirmNumber">Confirmation number</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Reservation</returns>
    [HttpGet("confirm/{confirmNumber}")]
    [ProducesResponseType(typeof(ReservationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetReservationByConfirmNumber(string confirmNumber, CancellationToken cancellationToken)
    {
        var result = await _reservationService.GetReservationByConfirmNumberAsync(confirmNumber, cancellationToken);

        if (!result.IsSuccess)
        {
            return NotFound(new { error = result.ErrorMessage });
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Get reservations by hotel ID
    /// </summary>
    /// <param name="hotelId">Hotel ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of reservations for the hotel</returns>
    [HttpGet("hotelid/{hotelId}")]
    [ProducesResponseType(typeof(IEnumerable<ReservationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetReservationsByHotelId(int hotelId, CancellationToken cancellationToken)
    {
        var result = await _reservationService.GetReservationsByHotelIdAsync(hotelId, cancellationToken);

        if (!result.IsSuccess)
        {
            return StatusCode(500, new { error = result.ErrorMessage });
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Get reservations by hotel code
    /// </summary>
    /// <param name="hotelCode">Hotel code</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of reservations for the hotel</returns>
    [HttpGet("hotel/{hotelCode}")]
    [ProducesResponseType(typeof(IEnumerable<ReservationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetReservationsByHotelCode(string hotelCode, CancellationToken cancellationToken)
    {
        var result = await _reservationService.GetReservationsByHotelCodeAsync(hotelCode, cancellationToken);

        if (!result.IsSuccess)
        {
            return StatusCode(500, new { error = result.ErrorMessage });
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Get reservations by status
    /// </summary>
    /// <param name="status">Reservation status</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of reservations with the specified status</returns>
    [HttpGet("status/{status}")]
    [ProducesResponseType(typeof(IEnumerable<ReservationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetReservationsByStatus(string status, CancellationToken cancellationToken)
    {
        var result = await _reservationService.GetReservationsByStatusAsync(status, cancellationToken);

        if (!result.IsSuccess)
        {
            return StatusCode(500, new { error = result.ErrorMessage });
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Get reservations by arrival date range
    /// </summary>
    /// <param name="startDate">Start date (inclusive)</param>
    /// <param name="endDate">End date (inclusive)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of reservations with arrival date in the specified range</returns>
    /// <remarks>
    /// Sample request:
    ///     GET /api/reservation/arrivals?startDate=2024-01-01&amp;endDate=2024-12-31
    /// </remarks>
    [HttpGet("arrivals")]
    [ProducesResponseType(typeof(IEnumerable<ReservationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetReservationsByArrivalDateRange(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate,
        CancellationToken cancellationToken)
    {
        if (startDate == default || endDate == default)
        {
            return BadRequest(new { error = "Both startDate and endDate are required" });
        }

        var result = await _reservationService.GetReservationsByArrivalDateRangeAsync(startDate, endDate, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(new { error = result.ErrorMessage });
        }

        return Ok(result.Data);
    }
}

