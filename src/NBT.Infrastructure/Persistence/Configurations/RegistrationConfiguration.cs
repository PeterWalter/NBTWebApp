using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NBT.Domain.Entities;
using NBT.Domain.Enums;

namespace NBT.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework Core configuration for the Registration entity.
/// </summary>
public class RegistrationConfiguration : IEntityTypeConfiguration<Registration>
{
    public void Configure(EntityTypeBuilder<Registration> builder)
    {
        builder.ToTable("Registrations");
        
        builder.HasKey(r => r.Id);
        
        builder.Property(r => r.RegistrationNumber)
            .IsRequired()
            .HasMaxLength(20);
        
        builder.Property(r => r.StudentId)
            .IsRequired();
        
        builder.Property(r => r.TestSessionId)
            .IsRequired();
        
        builder.Property(r => r.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);
        
        builder.Property(r => r.TestTypesSelected)
            .IsRequired()
            .HasMaxLength(255);
        
        builder.Property(r => r.IsRemoteWriter)
            .IsRequired()
            .HasDefaultValue(false);
        
        builder.Property(r => r.RemoteLocation)
            .HasMaxLength(255);
        
        builder.Property(r => r.SpecialSessionType)
            .HasMaxLength(100);
        
        builder.Property(r => r.RegistrationDate)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");
        
        builder.Property(r => r.CancellationReason)
            .HasMaxLength(500);
        
        builder.Property(r => r.CreatedBy)
            .HasMaxLength(255);
        
        builder.Property(r => r.CreatedDate)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");
        
        builder.Property(r => r.LastModifiedBy)
            .HasMaxLength(255);
        
        // Indexes
        builder.HasIndex(r => r.RegistrationNumber)
            .IsUnique()
            .HasDatabaseName("IX_Registrations_Number");
        
        builder.HasIndex(r => r.StudentId)
            .HasDatabaseName("IX_Registrations_StudentId");
        
        builder.HasIndex(r => r.TestSessionId)
            .HasDatabaseName("IX_Registrations_SessionId");
        
        builder.HasIndex(r => r.Status)
            .HasDatabaseName("IX_Registrations_Status");
        
        builder.HasIndex(r => r.RegistrationDate)
            .HasDatabaseName("IX_Registrations_Date");
        
        // Relationships
        builder.HasOne(r => r.Student)
            .WithMany(s => s.Registrations)
            .HasForeignKey(r => r.StudentId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(r => r.TestSession)
            .WithMany(ts => ts.Registrations)
            .HasForeignKey(r => r.TestSessionId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(r => r.Payment)
            .WithOne(p => p.Registration)
            .HasForeignKey<Payment>(p => p.RegistrationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

