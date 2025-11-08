using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NBT.Domain.Entities;

namespace NBT.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework Core configuration for the Room entity.
/// </summary>
public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.ToTable("Rooms");
        
        builder.HasKey(r => r.Id);
        
        builder.Property(r => r.VenueId)
            .IsRequired();
        
        builder.Property(r => r.RoomName)
            .IsRequired()
            .HasMaxLength(255);
        
        builder.Property(r => r.RoomNumber)
            .HasMaxLength(50);
        
        builder.Property(r => r.Capacity)
            .IsRequired();
        
        builder.Property(r => r.RoomType)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Property(r => r.HasComputers)
            .IsRequired()
            .HasDefaultValue(false);
        
        builder.Property(r => r.IsAccessible)
            .IsRequired()
            .HasDefaultValue(true);
        
        builder.Property(r => r.Status)
            .IsRequired()
            .HasMaxLength(50)
            .HasDefaultValue("Available");
        
        builder.Property(r => r.Notes)
            .HasMaxLength(500);
        
        builder.Property(r => r.CreatedBy)
            .HasMaxLength(255);
        
        builder.Property(r => r.CreatedDate)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");
        
        builder.Property(r => r.LastModifiedBy)
            .HasMaxLength(255);
        
        // Indexes
        builder.HasIndex(r => r.VenueId)
            .HasDatabaseName("IX_Rooms_VenueId");
        
        builder.HasIndex(r => new { r.VenueId, r.RoomNumber })
            .IsUnique()
            .HasDatabaseName("IX_Rooms_VenueRoom");
        
        builder.HasIndex(r => r.Status)
            .HasDatabaseName("IX_Rooms_Status");
        
        // Relationships
        builder.HasOne(r => r.Venue)
            .WithMany(v => v.Rooms)
            .HasForeignKey(r => r.VenueId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(r => r.RoomAllocations)
            .WithOne(ra => ra.Room)
            .HasForeignKey(ra => ra.RoomId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

