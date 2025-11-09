using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NBT.Domain.Entities;

namespace NBT.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework Core configuration for the TestDateCalendar entity.
/// </summary>
public class TestDateCalendarConfiguration : IEntityTypeConfiguration<TestDateCalendar>
{
    public void Configure(EntityTypeBuilder<TestDateCalendar> builder)
    {
        builder.ToTable("TestDateCalendar");
        
        builder.HasKey(td => td.Id);
        
        builder.Property(td => td.TestDate)
            .IsRequired()
            .HasColumnType("date");
        
        builder.Property(td => td.ClosingBookingDate)
            .IsRequired()
            .HasColumnType("date");
        
        builder.Property(td => td.IsSunday)
            .IsRequired()
            .HasDefaultValue(false);
        
        builder.Property(td => td.IsOnline)
            .IsRequired()
            .HasDefaultValue(false);
        
        builder.Property(td => td.IsActive)
            .IsRequired()
            .HasDefaultValue(true);
        
        builder.Property(td => td.IntakeYear)
            .IsRequired();
        
        builder.Property(td => td.Notes)
            .HasMaxLength(1000);
        
        builder.Property(td => td.CreatedBy)
            .HasMaxLength(255);
        
        builder.Property(td => td.CreatedDate)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");
        
        builder.Property(td => td.LastModifiedBy)
            .HasMaxLength(255);
        
        // Indexes
        builder.HasIndex(td => td.TestDate)
            .IsUnique()
            .HasDatabaseName("IX_TestDateCalendar_TestDate");
        
        builder.HasIndex(td => td.IntakeYear)
            .HasDatabaseName("IX_TestDateCalendar_IntakeYear");
        
        builder.HasIndex(td => td.IsActive)
            .HasDatabaseName("IX_TestDateCalendar_IsActive");
        
        builder.HasIndex(td => td.IsSunday)
            .HasDatabaseName("IX_TestDateCalendar_IsSunday");
        
        builder.HasIndex(td => td.IsOnline)
            .HasDatabaseName("IX_TestDateCalendar_IsOnline");
    }
}
