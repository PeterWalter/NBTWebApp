using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NBT.Domain.Entities;

namespace NBT.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework Core configuration for the TestResult entity.
/// </summary>
public class TestResultConfiguration : IEntityTypeConfiguration<TestResult>
{
    public void Configure(EntityTypeBuilder<TestResult> builder)
    {
        builder.ToTable("TestResults");
        
        builder.HasKey(tr => tr.Id);
        
        builder.Property(tr => tr.StudentId)
            .IsRequired();
        
        builder.Property(tr => tr.TestSessionId)
            .IsRequired();
        
        builder.Property(tr => tr.RegistrationId)
            .IsRequired();
        
        builder.Property(tr => tr.Barcode)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Property(tr => tr.TestType)
            .IsRequired()
            .HasMaxLength(20);
        
        builder.Property(tr => tr.ALScore)
            .HasColumnType("decimal(5,2)");
        
        builder.Property(tr => tr.ALPerformanceLevel)
            .HasMaxLength(50);
        
        builder.Property(tr => tr.QLScore)
            .HasColumnType("decimal(5,2)");
        
        builder.Property(tr => tr.QLPerformanceLevel)
            .HasMaxLength(50);
        
        builder.Property(tr => tr.MATScore)
            .HasColumnType("decimal(5,2)");
        
        builder.Property(tr => tr.MATPerformanceLevel)
            .HasMaxLength(50);
        
        builder.Property(tr => tr.OverallPerformanceBand)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Property(tr => tr.Percentile)
            .IsRequired();
        
        builder.Property(tr => tr.IsReleased)
            .IsRequired()
            .HasDefaultValue(false);
        
        builder.Property(tr => tr.TestDate)
            .IsRequired()
            .HasColumnType("date");
        
        builder.Property(tr => tr.CreatedBy)
            .HasMaxLength(255);
        
        builder.Property(tr => tr.CreatedDate)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");
        
        builder.Property(tr => tr.LastModifiedBy)
            .HasMaxLength(255);
        
        // Indexes
        builder.HasIndex(tr => tr.StudentId)
            .HasDatabaseName("IX_TestResults_StudentId");
        
        builder.HasIndex(tr => tr.TestSessionId)
            .HasDatabaseName("IX_TestResults_SessionId");
        
        builder.HasIndex(tr => tr.Barcode)
            .IsUnique()
            .HasDatabaseName("IX_TestResults_Barcode");
        
        builder.HasIndex(tr => tr.RegistrationId)
            .HasDatabaseName("IX_TestResults_RegistrationId");
        
        builder.HasIndex(tr => new { tr.StudentId, tr.TestType })
            .HasDatabaseName("IX_TestResults_StudentTest");
        
        builder.HasIndex(tr => tr.IsReleased)
            .HasDatabaseName("IX_TestResults_Released");
        
        builder.HasIndex(tr => tr.TestDate)
            .HasDatabaseName("IX_TestResults_Date");
        
        // Relationships
        builder.HasOne(tr => tr.Student)
            .WithMany(s => s.TestResults)
            .HasForeignKey(tr => tr.StudentId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(tr => tr.TestSession)
            .WithMany()
            .HasForeignKey(tr => tr.TestSessionId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(tr => tr.Registration)
            .WithMany(r => r.TestResults)
            .HasForeignKey(tr => tr.RegistrationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

