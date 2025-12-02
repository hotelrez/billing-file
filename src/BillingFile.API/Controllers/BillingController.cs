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
    private readonly IBillingService _billingService;
    private readonly ILogger<BillingController> _logger;

    public BillingController(
        IBillingService billingService,
        ILogger<BillingController> logger)
    {
        _billingService = billingService ?? throw new ArgumentNullException(nameof(billingService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Get all billing records
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of billing records</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<BillingRecordDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllBillingRecords(CancellationToken cancellationToken)
    {
        var result = await _billingService.GetAllBillingRecordsAsync(cancellationToken);

        if (!result.IsSuccess)
        {
            return StatusCode(500, new { error = result.ErrorMessage });
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Get billing record by ID
    /// </summary>
    /// <param name="id">Billing record ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Billing record</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(BillingRecordDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetBillingRecordById(int id, CancellationToken cancellationToken)
    {
        var result = await _billingService.GetBillingRecordByIdAsync(id, cancellationToken);

        if (!result.IsSuccess)
        {
            return NotFound(new { error = result.ErrorMessage });
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Get billing records by status
    /// </summary>
    /// <param name="status">Billing status (Pending, Paid, Overdue, Cancelled, Refunded)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of billing records with the specified status</returns>
    [HttpGet("status/{status}")]
    [ProducesResponseType(typeof(IEnumerable<BillingRecordDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetBillingRecordsByStatus(string status, CancellationToken cancellationToken)
    {
        var result = await _billingService.GetBillingRecordsByStatusAsync(status, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(new { error = result.ErrorMessage });
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Create a new billing record
    /// </summary>
    /// <param name="dto">Billing record data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created billing record</returns>
    [HttpPost]
    [ProducesResponseType(typeof(BillingRecordDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateBillingRecord(
        [FromBody] CreateBillingRecordDto dto,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _billingService.CreateBillingRecordAsync(dto, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(new { error = result.ErrorMessage });
        }

        return CreatedAtAction(
            nameof(GetBillingRecordById),
            new { id = result.Data!.Id },
            result.Data);
    }

    /// <summary>
    /// Update an existing billing record
    /// </summary>
    /// <param name="id">Billing record ID</param>
    /// <param name="dto">Updated billing record data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated billing record</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(BillingRecordDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateBillingRecord(
        int id,
        [FromBody] CreateBillingRecordDto dto,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _billingService.UpdateBillingRecordAsync(id, dto, cancellationToken);

        if (!result.IsSuccess)
        {
            return NotFound(new { error = result.ErrorMessage });
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Delete a billing record (soft delete)
    /// </summary>
    /// <param name="id">Billing record ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success status</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteBillingRecord(int id, CancellationToken cancellationToken)
    {
        var result = await _billingService.DeleteBillingRecordAsync(id, cancellationToken);

        if (!result.IsSuccess)
        {
            return NotFound(new { error = result.ErrorMessage });
        }

        return NoContent();
    }
}

