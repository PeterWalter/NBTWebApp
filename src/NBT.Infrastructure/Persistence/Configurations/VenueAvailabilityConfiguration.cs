using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NBT.Domain.Entities;

namespace NBT.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework Core configuration for the VenueAvailability entity.
/// </summary>
public class VenueAvailabilityConfiguration : IEntityTypeConfiguration<VenueAvailability>
{
    public void Configure(EntityTypeBuilder<VenueAvailability> builder)
    {
        builder.ToTable("VenueAvailability");
        
        builder.HasKey(va => va.Id);
        
        builder.Property(va => va.VenueId)
            .IsRequired();
        
        builder.Property(va => va.TestDate)
            .IsRequired()
            .HasColumnType("date");
        
        builder.Property(va => va.IsAvailable)
            .IsRequired()
            .HasDefaultValue(true);
        
        builder.Property(va => va.UnavailableReason)
            .HasMaxLength(500);
        
        builder.Property(va => va.Notes)
            .HasMaxLength(1000);
        
        builder.Property(va => va.CreatedBy)
            .HasMaxLength(255);
        
        builder.Property(va => va.CreatedDate)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");
        
        builder.Property(va => va.LastModifiedBy)
            .HasMaxLength(255);
        
        // Indexes
        builder.HasIndex(va => va.VenueId)
            .HasDatabaseName("IX_VenueAvailability_VenueId");
        
        builder.HasIndex(va => va.TestDate)
            .HasDatabaseName("IX_VenueAvailability_TestDate");
        
        builder.HasIndex(va => new { va.VenueId, va.TestDate })
            .IsUnique()
            .HasDatabaseName("IX_VenueAvailability_VenueDate");
        
        builder.HasIndex(va => va.IsAvailable)
            .HasDatabaseName("IX_VenueAvailability_IsAvailable");
        
        // Relationships
        builder.HasOne(va => va.Venue)
            .WithMany(v => v.VenueAvailabilities)
            .HasForeignKey(va => va.VenueId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
