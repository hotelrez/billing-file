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

        // Map to existing table in MemberPortal database - EXACT schema match
        modelBuilder.Entity<Hotel>(entity =>
        {
            entity.ToTable("Hotel", "dbo");
            entity.HasKey(e => e.ID); // Primary key is ID (uppercase)
            
            // Map property names to column names
            entity.Property(e => e.ID).HasColumnName("ID");
            entity.Property(e => e.TrustCode).HasColumnName("TrustCode").HasMaxLength(10);
            entity.Property(e => e.Name).HasColumnName("Name").IsRequired().HasMaxLength(255);
            entity.Property(e => e.CountryCode).HasColumnName("CountryCode").IsRequired().HasMaxLength(10);
            entity.Property(e => e.CityCode).HasColumnName("CityCode").HasMaxLength(10);
            entity.Property(e => e.fkHotelChainID).HasColumnName("fkHotelChainID").IsRequired();
            entity.Property(e => e.BookingEngineBaseURL).HasColumnName("BookingEngineBaseURL").HasMaxLength(255);
            entity.Property(e => e.CanGeneratePromotionalUrl).HasColumnName("CanGeneratePromotionalUrl");
            entity.Property(e => e.IsEnterpriseMember).HasColumnName("IsEnterpriseMember");
            entity.Property(e => e.HasGoogleAnalytics).HasColumnName("HasGoogleAnalytics").IsRequired();
            entity.Property(e => e.REZMedia).HasColumnName("REZMedia").HasMaxLength(1);
            entity.Property(e => e.REZbooker).HasColumnName("REZbooker").HasMaxLength(1);
            entity.Property(e => e.TrustYouId).HasColumnName("TrustYouId").HasMaxLength(40);
            entity.Property(e => e.Active).HasColumnName("Active").IsRequired();
            
            // Read-only - disable change tracking for better performance
            entity.HasQueryFilter(e => e.Active != 0); // Only get active hotels by default
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

        // Map to existing table in Play database - EXACT schema match
        modelBuilder.Entity<FullReservation>(entity =>
        {
            entity.ToTable("FullReservation", "dbo");
            entity.HasKey(e => e.Id);
            
            // Explicitly map all columns
            entity.Property(e => e.Id).HasColumnName("Id");
            entity.Property(e => e.ReservationNumber).HasColumnName("ReservationNumber").IsRequired().HasMaxLength(50);
            entity.Property(e => e.GuestName).HasColumnName("GuestName").IsRequired().HasMaxLength(200);
            entity.Property(e => e.GuestEmail).HasColumnName("GuestEmail").HasMaxLength(200);
            entity.Property(e => e.GuestPhone).HasColumnName("GuestPhone").HasMaxLength(50);
            entity.Property(e => e.CheckInDate).HasColumnName("CheckInDate").IsRequired();
            entity.Property(e => e.CheckOutDate).HasColumnName("CheckOutDate").IsRequired();
            entity.Property(e => e.NumberOfGuests).HasColumnName("NumberOfGuests").IsRequired();
            entity.Property(e => e.NumberOfNights).HasColumnName("NumberOfNights").IsRequired();
            entity.Property(e => e.RoomType).HasColumnName("RoomType").HasMaxLength(100);
            entity.Property(e => e.TotalAmount).HasColumnName("TotalAmount").IsRequired().HasPrecision(18, 2);
            entity.Property(e => e.Status).HasColumnName("Status").IsRequired().HasMaxLength(50);
            entity.Property(e => e.HotelCode).HasColumnName("HotelCode").HasMaxLength(50);
            entity.Property(e => e.Notes).HasColumnName("Notes");
            entity.Property(e => e.CreatedAt).HasColumnName("CreatedAt").IsRequired();
            entity.Property(e => e.UpdatedAt).HasColumnName("UpdatedAt");
            entity.Property(e => e.IsDeleted).HasColumnName("IsDeleted").IsRequired();
            
            // Soft delete filter
            entity.HasQueryFilter(e => !e.IsDeleted);
        });
    }
}

