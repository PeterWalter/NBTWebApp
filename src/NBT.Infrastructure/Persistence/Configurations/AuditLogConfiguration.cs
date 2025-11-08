using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NBT.Domain.Entities;

namespace NBT.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework Core configuration for the AuditLog entity.
/// Constitution Section 8: Comprehensive audit trail required.
/// </summary>
public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.ToTable("AuditLogs");
        
        builder.HasKey(al => al.Id);
        
        builder.Property(al => al.Timestamp)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");
        
        builder.Property(al => al.UserId)
            .IsRequired();
        
        builder.Property(al => al.UserEmail)
            .IsRequired()
            .HasMaxLength(255);
        
        builder.Property(al => al.Action)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Property(al => al.EntityType)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(al => al.BeforeValue)
            .HasColumnType("nvarchar(max)");
        
        builder.Property(al => al.AfterValue)
            .HasColumnType("nvarchar(max)");
        
        builder.Property(al => al.IpAddress)
            .HasMaxLength(45);
        
        builder.Property(al => al.UserAgent)
            .HasMaxLength(500);
        
        builder.Property(al => al.Notes)
            .HasMaxLength(1000);
        
        builder.Property(al => al.CreatedBy)
            .HasMaxLength(255);
        
        builder.Property(al => al.CreatedDate)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");
        
        builder.Property(al => al.LastModifiedBy)
            .HasMaxLength(255);
        
        // Indexes for audit querying
        builder.HasIndex(al => al.Timestamp)
            .HasDatabaseName("IX_AuditLogs_Timestamp");
        
        builder.HasIndex(al => al.UserId)
            .HasDatabaseName("IX_AuditLogs_UserId");
        
        builder.HasIndex(al => al.UserEmail)
            .HasDatabaseName("IX_AuditLogs_UserEmail");
        
        builder.HasIndex(al => al.Action)
            .HasDatabaseName("IX_AuditLogs_Action");
        
        builder.HasIndex(al => al.EntityType)
            .HasDatabaseName("IX_AuditLogs_EntityType");
        
        builder.HasIndex(al => al.EntityId)
            .HasDatabaseName("IX_AuditLogs_EntityId");
        
        builder.HasIndex(al => new { al.EntityType, al.EntityId })
            .HasDatabaseName("IX_AuditLogs_Entity");
        
        builder.HasIndex(al => new { al.Timestamp, al.Action })
            .HasDatabaseName("IX_AuditLogs_TimeAction");
    }
}

