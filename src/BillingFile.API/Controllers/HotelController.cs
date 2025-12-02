using BillingFile.Application.DTOs;
using BillingFile.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BillingFile.API.Controllers;

/// <summary>
/// API controller for hotel operations (read-only)
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class HotelController : ControllerBase
{
    private readonly IHotelService _hotelService;
    private readonly ILogger<HotelController> _logger;

    public HotelController(
        IHotelService hotelService,
        ILogger<HotelController> logger)
    {
        _hotelService = hotelService ?? throw new ArgumentNullException(nameof(hotelService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Get all hotels
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of hotels</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<HotelDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllHotels(CancellationToken cancellationToken)
    {
        var result = await _hotelService.GetAllHotelsAsync(cancellationToken);

        if (!result.IsSuccess)
        {
            return StatusCode(500, new { error = result.ErrorMessage });
        }

        return Ok(result.Data);
    }
}
