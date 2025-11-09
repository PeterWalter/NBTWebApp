using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NBT.Domain.Entities;

namespace NBT.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework Core configuration for the TestPricing entity.
/// </summary>
public class TestPricingConfiguration : IEntityTypeConfiguration<TestPricing>
{
    public void Configure(EntityTypeBuilder<TestPricing> builder)
    {
        builder.ToTable("TestPricing");
        
        builder.HasKey(tp => tp.Id);
        
        builder.Property(tp => tp.IntakeYear)
            .IsRequired();
        
        builder.Property(tp => tp.TestType)
            .IsRequired()
            .HasMaxLength(20);
        
        builder.Property(tp => tp.Price)
            .IsRequired()
            .HasColumnType("decimal(18,2)");
        
        builder.Property(tp => tp.EffectiveFrom)
            .IsRequired()
            .HasColumnType("date");
        
        builder.Property(tp => tp.EffectiveTo)
            .HasColumnType("date");
        
        builder.Property(tp => tp.IsActive)
            .IsRequired()
            .HasDefaultValue(true);
        
        builder.Property(tp => tp.Notes)
            .HasMaxLength(500);
        
        builder.Property(tp => tp.CreatedBy)
            .HasMaxLength(255);
        
        builder.Property(tp => tp.CreatedDate)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");
        
        builder.Property(tp => tp.LastModifiedBy)
            .HasMaxLength(255);
        
        // Indexes
        builder.HasIndex(tp => tp.IntakeYear)
            .HasDatabaseName("IX_TestPricing_IntakeYear");
        
        builder.HasIndex(tp => tp.TestType)
            .HasDatabaseName("IX_TestPricing_TestType");
        
        builder.HasIndex(tp => new { tp.IntakeYear, tp.TestType, tp.IsActive })
            .HasDatabaseName("IX_TestPricing_YearTypeActive");
        
        builder.HasIndex(tp => tp.IsActive)
            .HasDatabaseName("IX_TestPricing_IsActive");
    }
}
