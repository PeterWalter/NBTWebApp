using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NBT.Domain.Entities;

namespace NBT.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework Core configuration for the RoomAllocation entity.
/// </summary>
public class RoomAllocationConfiguration : IEntityTypeConfiguration<RoomAllocation>
{
    public void Configure(EntityTypeBuilder<RoomAllocation> builder)
    {
        builder.ToTable("RoomAllocations");
        
        builder.HasKey(ra => ra.Id);
        
        builder.Property(ra => ra.TestSessionId)
            .IsRequired();
        
        builder.Property(ra => ra.RoomId)
            .IsRequired();
        
        builder.Property(ra => ra.StudentId)
            .IsRequired();
        
        builder.Property(ra => ra.SeatNumber)
            .HasMaxLength(20);
        
        builder.Property(ra => ra.AllocationDate)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");
        
        builder.Property(ra => ra.Notes)
            .HasMaxLength(500);
        
        builder.Property(ra => ra.CreatedBy)
            .HasMaxLength(255);
        
        builder.Property(ra => ra.CreatedDate)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");
        
        builder.Property(ra => ra.LastModifiedBy)
            .HasMaxLength(255);
        
        // Indexes
        builder.HasIndex(ra => ra.TestSessionId)
            .HasDatabaseName("IX_RoomAllocations_SessionId");
        
        builder.HasIndex(ra => ra.RoomId)
            .HasDatabaseName("IX_RoomAllocations_RoomId");
        
        builder.HasIndex(ra => ra.StudentId)
            .HasDatabaseName("IX_RoomAllocations_StudentId");
        
        // Unique constraint: A student can only be allocated to one room per session
        builder.HasIndex(ra => new { ra.TestSessionId, ra.StudentId })
            .IsUnique()
            .HasDatabaseName("IX_RoomAllocations_SessionStudent");
        
        // Relationships
        builder.HasOne(ra => ra.TestSession)
            .WithMany(ts => ts.RoomAllocations)
            .HasForeignKey(ra => ra.TestSessionId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(ra => ra.Room)
            .WithMany(r => r.RoomAllocations)
            .HasForeignKey(ra => ra.RoomId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(ra => ra.Student)
            .WithMany(s => s.RoomAllocations)
            .HasForeignKey(ra => ra.StudentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

