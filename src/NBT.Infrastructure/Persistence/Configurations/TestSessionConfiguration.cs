using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NBT.Domain.Entities;
using NBT.Domain.Enums;

namespace NBT.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework Core configuration for the TestSession entity.
/// </summary>
public class TestSessionConfiguration : IEntityTypeConfiguration<TestSession>
{
    public void Configure(EntityTypeBuilder<TestSession> builder)
    {
        builder.ToTable("TestSessions");
        
        builder.HasKey(ts => ts.Id);
        
        builder.Property(ts => ts.SessionCode)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Property(ts => ts.SessionName)
            .IsRequired()
            .HasMaxLength(255);
        
        builder.Property(ts => ts.SessionDate)
            .IsRequired()
            .HasColumnType("date");
        
        builder.Property(ts => ts.StartTime)
            .IsRequired();
        
        builder.Property(ts => ts.EndTime)
            .IsRequired();
        
        builder.Property(ts => ts.VenueId)
            .IsRequired();
        
        builder.Property(ts => ts.Capacity)
            .IsRequired();
        
        builder.Property(ts => ts.CurrentRegistrations)
            .IsRequired()
            .HasDefaultValue(0);
        
        builder.Property(ts => ts.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);
        
        builder.Property(ts => ts.IsSpecialSession)
            .IsRequired()
            .HasDefaultValue(false);
        
        builder.Property(ts => ts.SpecialSessionNotes)
            .HasMaxLength(1000);
        
        builder.Property(ts => ts.Notes)
            .HasMaxLength(1000);
        
        builder.Property(ts => ts.CreatedBy)
            .HasMaxLength(255);
        
        builder.Property(ts => ts.CreatedDate)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");
        
        builder.Property(ts => ts.LastModifiedBy)
            .HasMaxLength(255);
        
        // Indexes
        builder.HasIndex(ts => ts.SessionCode)
            .IsUnique()
            .HasDatabaseName("IX_TestSessions_Code");
        
        builder.HasIndex(ts => ts.SessionDate)
            .HasDatabaseName("IX_TestSessions_Date");
        
        builder.HasIndex(ts => ts.VenueId)
            .HasDatabaseName("IX_TestSessions_VenueId");
        
        builder.HasIndex(ts => ts.Status)
            .HasDatabaseName("IX_TestSessions_Status");
        
        builder.HasIndex(ts => new { ts.SessionDate, ts.VenueId })
            .HasDatabaseName("IX_TestSessions_DateVenue");
        
        // Relationships
        builder.HasOne(ts => ts.Venue)
            .WithMany(v => v.TestSessions)
            .HasForeignKey(ts => ts.VenueId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasMany(ts => ts.Registrations)
            .WithOne(r => r.TestSession)
            .HasForeignKey(r => r.TestSessionId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasMany(ts => ts.RoomAllocations)
            .WithOne(ra => ra.TestSession)
            .HasForeignKey(ra => ra.TestSessionId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Computed property
        builder.Ignore(ts => ts.AvailableSeats);
    }
}

