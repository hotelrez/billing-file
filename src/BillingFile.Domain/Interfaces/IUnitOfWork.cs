using BillingFile.Domain.Entities;

namespace BillingFile.Domain.Interfaces;

/// <summary>
/// Unit of Work pattern interface for transaction management
/// </summary>
public interface IUnitOfWork : IDisposable
{
    IRepository<Hotel> Hotels { get; }
    IRepository<FullReservation> Reservations { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}

