using BillingFile.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BillingFile.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework configuration for BillingRecord
/// </summary>
public class BillingRecordConfiguration : IEntityTypeConfiguration<BillingRecord>
{
    public void Configure(EntityTypeBuilder<BillingRecord> builder)
    {
        builder.ToTable("BillingRecords");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.CustomerName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(b => b.CustomerEmail)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(b => b.InvoiceNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(b => b.InvoiceNumber)
            .IsUnique();

        builder.Property(b => b.Amount)
            .HasPrecision(18, 2);

        builder.Property(b => b.Currency)
            .IsRequired()
            .HasMaxLength(3);

        builder.Property(b => b.Description)
            .HasMaxLength(1000);

        builder.Property(b => b.Status)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.HasIndex(b => b.Status);
        builder.HasIndex(b => b.InvoiceDate);
        builder.HasIndex(b => b.CustomerEmail);
    }
}

