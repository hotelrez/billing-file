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
    
    // Keyless entity for stored procedure results
    public DbSet<BillingSpResult> BillingSpResults => Set<BillingSpResult>();
    
    // HotelBillingCurrency table (existing table in Play database)
    public DbSet<HotelBillingCurrency> HotelBillingCurrencies => Set<HotelBillingCurrency>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Map to existing table in Play database - EXACT schema match (106 columns)
        modelBuilder.Entity<FullReservation>(entity =>
        {
            entity.ToTable("FullReservation", "dbo");
            entity.HasKey(e => e.ID);
            
            // Primary key
            entity.Property(e => e.ID).HasColumnName("ID");
            
            // All string columns are nvarchar(255) unless specified
            // All nullable int and decimal columns
            // Read-only entity - no modification tracking needed
        });
        
        // Keyless entity for stored procedure results
        modelBuilder.Entity<BillingSpResult>(entity =>
        {
            entity.HasNoKey();
            entity.ToView(null); // Not mapped to a table/view
        });
        
        // Map to EXISTING HotelBillingCurrency table in Play database (3 columns only)
        modelBuilder.Entity<HotelBillingCurrency>(entity =>
        {
            entity.ToTable("HotelBillingCurrency", "dbo");
            entity.HasKey(e => e.HotelID);
            
            entity.Property(e => e.HotelID).HasColumnName("HotelID").IsRequired();
            entity.Property(e => e.Enabled).HasColumnName("Enabled").IsRequired();
            entity.Property(e => e.Currency).HasColumnName("Currency").IsRequired().HasMaxLength(10);
        });
    }
}

