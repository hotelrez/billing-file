using AutoMapper;
using BillingFile.Application.Common;
using BillingFile.Application.DTOs;
using BillingFile.Application.Interfaces;
using BillingFile.Domain.Entities;
using BillingFile.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace BillingFile.Application.Services;

/// <summary>
/// Service implementation for billing operations
/// </summary>
public class BillingService : IBillingService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<BillingService> _logger;

    public BillingService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<BillingService> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Result<IEnumerable<BillingRecordDto>>> GetAllBillingRecordsAsync(
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all billing records");

            var records = await _unitOfWork.BillingRecords.GetAllAsync(cancellationToken);
            var dtos = _mapper.Map<IEnumerable<BillingRecordDto>>(records);

            _logger.LogInformation("Successfully retrieved {Count} billing records", dtos.Count());
            return Result<IEnumerable<BillingRecordDto>>.Success(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving billing records");
            return Result<IEnumerable<BillingRecordDto>>.Failure("An error occurred while retrieving billing records");
        }
    }

    public async Task<Result<BillingRecordDto>> GetBillingRecordByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving billing record with ID: {Id}", id);

            var record = await _unitOfWork.BillingRecords.GetByIdAsync(id, cancellationToken);
            
            if (record == null)
            {
                _logger.LogWarning("Billing record with ID {Id} not found", id);
                return Result<BillingRecordDto>.Failure($"Billing record with ID {id} not found");
            }

            var dto = _mapper.Map<BillingRecordDto>(record);
            return Result<BillingRecordDto>.Success(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving billing record with ID: {Id}", id);
            return Result<BillingRecordDto>.Failure("An error occurred while retrieving the billing record");
        }
    }

    public async Task<Result<BillingRecordDto>> CreateBillingRecordAsync(
        CreateBillingRecordDto dto,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new billing record for invoice: {InvoiceNumber}", dto.InvoiceNumber);

            // Check if invoice number already exists
            var existingRecord = await _unitOfWork.BillingRecords.FindAsync(
                r => r.InvoiceNumber == dto.InvoiceNumber,
                cancellationToken);

            if (existingRecord.Any())
            {
                _logger.LogWarning("Invoice number {InvoiceNumber} already exists", dto.InvoiceNumber);
                return Result<BillingRecordDto>.Failure($"Invoice number {dto.InvoiceNumber} already exists");
            }

            var record = _mapper.Map<BillingRecord>(dto);
            await _unitOfWork.BillingRecords.AddAsync(record, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var resultDto = _mapper.Map<BillingRecordDto>(record);
            _logger.LogInformation("Successfully created billing record with ID: {Id}", record.Id);

            return Result<BillingRecordDto>.Success(resultDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating billing record");
            return Result<BillingRecordDto>.Failure("An error occurred while creating the billing record");
        }
    }

    public async Task<Result<BillingRecordDto>> UpdateBillingRecordAsync(
        int id,
        CreateBillingRecordDto dto,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating billing record with ID: {Id}", id);

            var record = await _unitOfWork.BillingRecords.GetByIdAsync(id, cancellationToken);
            
            if (record == null)
            {
                _logger.LogWarning("Billing record with ID {Id} not found", id);
                return Result<BillingRecordDto>.Failure($"Billing record with ID {id} not found");
            }

            _mapper.Map(dto, record);
            await _unitOfWork.BillingRecords.UpdateAsync(record, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var resultDto = _mapper.Map<BillingRecordDto>(record);
            _logger.LogInformation("Successfully updated billing record with ID: {Id}", id);

            return Result<BillingRecordDto>.Success(resultDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating billing record with ID: {Id}", id);
            return Result<BillingRecordDto>.Failure("An error occurred while updating the billing record");
        }
    }

    public async Task<Result<bool>> DeleteBillingRecordAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting billing record with ID: {Id}", id);

            var record = await _unitOfWork.BillingRecords.GetByIdAsync(id, cancellationToken);
            
            if (record == null)
            {
                _logger.LogWarning("Billing record with ID {Id} not found", id);
                return Result<bool>.Failure($"Billing record with ID {id} not found");
            }

            await _unitOfWork.BillingRecords.DeleteAsync(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully deleted billing record with ID: {Id}", id);
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting billing record with ID: {Id}", id);
            return Result<bool>.Failure("An error occurred while deleting the billing record");
        }
    }

    public async Task<Result<IEnumerable<BillingRecordDto>>> GetBillingRecordsByStatusAsync(
        string status,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving billing records with status: {Status}", status);

            if (!Enum.TryParse<BillingStatus>(status, true, out var billingStatus))
            {
                return Result<IEnumerable<BillingRecordDto>>.Failure($"Invalid status: {status}");
            }

            var records = await _unitOfWork.BillingRecords.FindAsync(
                r => r.Status == billingStatus,
                cancellationToken);

            var dtos = _mapper.Map<IEnumerable<BillingRecordDto>>(records);

            _logger.LogInformation("Successfully retrieved {Count} billing records with status: {Status}", 
                dtos.Count(), status);

            return Result<IEnumerable<BillingRecordDto>>.Success(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving billing records by status: {Status}", status);
            return Result<IEnumerable<BillingRecordDto>>.Failure(
                "An error occurred while retrieving billing records");
        }
    }
}

