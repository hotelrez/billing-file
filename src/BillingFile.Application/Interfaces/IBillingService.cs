using BillingFile.Application.Common;
using BillingFile.Application.DTOs;

namespace BillingFile.Application.Interfaces;

/// <summary>
/// Service interface for billing operations
/// </summary>
public interface IBillingService
{
    Task<Result<IEnumerable<BillingRecordDto>>> GetAllBillingRecordsAsync(CancellationToken cancellationToken = default);
    Task<Result<BillingRecordDto>> GetBillingRecordByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Result<BillingRecordDto>> CreateBillingRecordAsync(CreateBillingRecordDto dto, CancellationToken cancellationToken = default);
    Task<Result<BillingRecordDto>> UpdateBillingRecordAsync(int id, CreateBillingRecordDto dto, CancellationToken cancellationToken = default);
    Task<Result<bool>> DeleteBillingRecordAsync(int id, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<BillingRecordDto>>> GetBillingRecordsByStatusAsync(string status, CancellationToken cancellationToken = default);
}

