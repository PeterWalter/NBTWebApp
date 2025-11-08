using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NBT.Domain.Entities;

namespace NBT.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework Core configuration for the Venue entity.
/// </summary>
public class VenueConfiguration : IEntityTypeConfiguration<Venue>
{
    public void Configure(EntityTypeBuilder<Venue> builder)
    {
        builder.ToTable("Venues");
        
        builder.HasKey(v => v.Id);
        
        builder.Property(v => v.VenueName)
            .IsRequired()
            .HasMaxLength(255);
        
        builder.Property(v => v.VenueCode)
            .IsRequired()
            .HasMaxLength(20);
        
        builder.Property(v => v.Address)
            .IsRequired()
            .HasMaxLength(255);
        
        builder.Property(v => v.City)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(v => v.Province)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(v => v.PostalCode)
            .HasMaxLength(10);
        
        builder.Property(v => v.ContactPerson)
            .HasMaxLength(200);
        
        builder.Property(v => v.ContactEmail)
            .HasMaxLength(255);
        
        builder.Property(v => v.ContactPhone)
            .HasMaxLength(20);
        
        builder.Property(v => v.TotalCapacity)
            .IsRequired()
            .HasDefaultValue(0);
        
        builder.Property(v => v.IsAccessible)
            .IsRequired()
            .HasDefaultValue(true);
        
        builder.Property(v => v.Status)
            .IsRequired()
            .HasMaxLength(50)
            .HasDefaultValue("Active");
        
        builder.Property(v => v.Notes)
            .HasMaxLength(1000);
        
        builder.Property(v => v.CreatedBy)
            .HasMaxLength(255);
        
        builder.Property(v => v.CreatedDate)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");
        
        builder.Property(v => v.LastModifiedBy)
            .HasMaxLength(255);
        
        // Indexes
        builder.HasIndex(v => v.VenueCode)
            .IsUnique()
            .HasDatabaseName("IX_Venues_Code");
        
        builder.HasIndex(v => v.City)
            .HasDatabaseName("IX_Venues_City");
        
        builder.HasIndex(v => v.Province)
            .HasDatabaseName("IX_Venues_Province");
        
        builder.HasIndex(v => v.Status)
            .HasDatabaseName("IX_Venues_Status");
        
        // Relationships
        builder.HasMany(v => v.Rooms)
            .WithOne(r => r.Venue)
            .HasForeignKey(r => r.VenueId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(v => v.TestSessions)
            .WithOne(ts => ts.Venue)
            .HasForeignKey(ts => ts.VenueId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

