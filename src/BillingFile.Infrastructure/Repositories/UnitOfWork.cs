using BillingFile.Domain.Entities;
using BillingFile.Domain.Interfaces;
using BillingFile.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace BillingFile.Infrastructure.Repositories;

/// <summary>
/// Unit of Work implementation for transaction management across multiple databases
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly MemberPortalDbContext _memberPortalContext;
    private readonly PlayDbContext _playContext;
    private IDbContextTransaction? _memberPortalTransaction;
    private IDbContextTransaction? _playTransaction;
    private IRepository<Hotel>? _hotels;
    private IRepository<FullReservation>? _reservations;

    public UnitOfWork(MemberPortalDbContext memberPortalContext, PlayDbContext playContext)
    {
        _memberPortalContext = memberPortalContext ?? throw new ArgumentNullException(nameof(memberPortalContext));
        _playContext = playContext ?? throw new ArgumentNullException(nameof(playContext));
    }

    public IRepository<Hotel> Hotels
    {
        get
        {
            _hotels ??= new Repository<Hotel>(_memberPortalContext);
            return _hotels;
        }
    }

    public IRepository<FullReservation> Reservations
    {
        get
        {
            _reservations ??= new Repository<FullReservation>(_playContext);
            return _reservations;
        }
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Note: Since we're only reading, SaveChanges won't actually modify the databases
        var memberPortalChanges = await _memberPortalContext.SaveChangesAsync(cancellationToken);
        var playChanges = await _playContext.SaveChangesAsync(cancellationToken);
        return memberPortalChanges + playChanges;
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _memberPortalTransaction = await _memberPortalContext.Database.BeginTransactionAsync(cancellationToken);
        _playTransaction = await _playContext.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await SaveChangesAsync(cancellationToken);
            
            if (_memberPortalTransaction != null)
            {
                await _memberPortalTransaction.CommitAsync(cancellationToken);
            }
            
            if (_playTransaction != null)
            {
                await _playTransaction.CommitAsync(cancellationToken);
            }
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
        finally
        {
            if (_memberPortalTransaction != null)
            {
                await _memberPortalTransaction.DisposeAsync();
                _memberPortalTransaction = null;
            }
            
            if (_playTransaction != null)
            {
                await _playTransaction.DisposeAsync();
                _playTransaction = null;
            }
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_memberPortalTransaction != null)
        {
            await _memberPortalTransaction.RollbackAsync(cancellationToken);
            await _memberPortalTransaction.DisposeAsync();
            _memberPortalTransaction = null;
        }
        
        if (_playTransaction != null)
        {
            await _playTransaction.RollbackAsync(cancellationToken);
            await _playTransaction.DisposeAsync();
            _playTransaction = null;
        }
    }

    public void Dispose()
    {
        _memberPortalTransaction?.Dispose();
        _playTransaction?.Dispose();
        _memberPortalContext.Dispose();
        _playContext.Dispose();
    }
}

