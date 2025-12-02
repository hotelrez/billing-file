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
    /// Get reservation by reservation number
    /// </summary>
    /// <param name="reservationNumber">Reservation number</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Reservation</returns>
    [HttpGet("number/{reservationNumber}")]
    [ProducesResponseType(typeof(ReservationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetReservationByNumber(string reservationNumber, CancellationToken cancellationToken)
    {
        var result = await _reservationService.GetReservationByNumberAsync(reservationNumber, cancellationToken);

        if (!result.IsSuccess)
        {
            return NotFound(new { error = result.ErrorMessage });
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
}

