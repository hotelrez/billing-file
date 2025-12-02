using BillingFile.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BillingFile.Infrastructure.Data;

/// <summary>
/// MemberPortal database context
/// </summary>
public class MemberPortalDbContext : DbContext
{
    public MemberPortalDbContext(DbContextOptions<MemberPortalDbContext> options)
        : base(options)
    {
    }

    public DbSet<Hotel> Hotels => Set<Hotel>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Map to existing table in MemberPortal database
        modelBuilder.Entity<Hotel>(entity =>
        {
            entity.ToTable("Hotel", "dbo");
            entity.HasKey(e => e.Id);
            
            // Configure to NOT track changes for read-only access
            entity.HasQueryFilter(e => !e.IsDeleted);
        });
    }
}

/// <summary>
/// Play database context
/// </summary>
public class PlayDbContext : DbContext
{
    public PlayDbContext(DbContextOptions<PlayDbContext> options)
        : base(options)
    {
    }

    public DbSet<FullReservation> FullReservations => Set<FullReservation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Map to existing table in Play database
        modelBuilder.Entity<FullReservation>(entity =>
        {
            entity.ToTable("FullReservation", "dbo");
            entity.HasKey(e => e.Id);
            
            // Configure to NOT track changes for read-only access
            entity.HasQueryFilter(e => !e.IsDeleted);
        });
    }
}

