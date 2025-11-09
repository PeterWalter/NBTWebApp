using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NBT.Domain.Entities;
using NBT.Domain.Enums;

namespace NBT.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework Core configuration for the PaymentTransaction entity.
/// </summary>
public class PaymentTransactionConfiguration : IEntityTypeConfiguration<PaymentTransaction>
{
    public void Configure(EntityTypeBuilder<PaymentTransaction> builder)
    {
        builder.ToTable("PaymentTransactions");
        
        builder.HasKey(pt => pt.Id);
        
        builder.Property(pt => pt.PaymentId)
            .IsRequired();
        
        builder.Property(pt => pt.TransactionReference)
            .IsRequired()
            .HasMaxLength(20);
        
        builder.Property(pt => pt.TransactionDate)
            .IsRequired();
        
        builder.Property(pt => pt.Amount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");
        
        builder.Property(pt => pt.PaymentMethod)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Property(pt => pt.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);
        
        builder.Property(pt => pt.ExternalTransactionId)
            .HasMaxLength(100);
        
        builder.Property(pt => pt.Notes)
            .HasMaxLength(1000);
        
        builder.Property(pt => pt.RecordedBy)
            .HasMaxLength(100);
        
        builder.Property(pt => pt.CreatedBy)
            .HasMaxLength(255);
        
        builder.Property(pt => pt.CreatedDate)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");
        
        builder.Property(pt => pt.LastModifiedBy)
            .HasMaxLength(255);
        
        // Indexes
        builder.HasIndex(pt => pt.PaymentId)
            .HasDatabaseName("IX_PaymentTransactions_PaymentId");
        
        builder.HasIndex(pt => pt.TransactionReference)
            .IsUnique()
            .HasDatabaseName("IX_PaymentTransactions_Reference");
        
        builder.HasIndex(pt => pt.TransactionDate)
            .HasDatabaseName("IX_PaymentTransactions_Date");
        
        builder.HasIndex(pt => pt.Status)
            .HasDatabaseName("IX_PaymentTransactions_Status");
        
        // Relationships
        builder.HasOne(pt => pt.Payment)
            .WithMany(p => p.Transactions)
            .HasForeignKey(pt => pt.PaymentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
