using BillingFile.Application.DTOs;
using BillingFile.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BillingFile.API.Controllers;

/// <summary>
/// API controller for billing operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class BillingController : ControllerBase
{
    private readonly IReservationService _reservationService;
    private readonly ILogger<BillingController> _logger;

    public BillingController(
        IReservationService reservationService,
        ILogger<BillingController> logger)
    {
        _reservationService = reservationService ?? throw new ArgumentNullException(nameof(reservationService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Get billing records (reservations) by arrival date range
    /// </summary>
    /// <param name="startDate">Start date (inclusive)</param>
    /// <param name="endDate">End date (inclusive)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of reservations with arrival date in the specified range</returns>
    /// <remarks>
    /// Sample request:
    ///     GET /api/billing?startDate=2025-12-01&amp;endDate=2025-12-31
    /// </remarks>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ReservationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetBillingByDateRange(
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

