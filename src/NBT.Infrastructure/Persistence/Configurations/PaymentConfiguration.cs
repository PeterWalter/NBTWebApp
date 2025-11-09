using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NBT.Domain.Entities;
using NBT.Domain.Enums;

namespace NBT.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework Core configuration for the Payment entity.
/// </summary>
public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("Payments");
        
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.RegistrationId)
            .IsRequired();
        
        builder.Property(p => p.InvoiceNumber)
            .IsRequired()
            .HasMaxLength(20);
        
        builder.Property(p => p.TotalAmount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");
        
        builder.Property(p => p.AmountPaid)
            .IsRequired()
            .HasColumnType("decimal(18,2)");
        
        builder.Property(p => p.IntakeYear)
            .IsRequired();
        
        builder.Property(p => p.PaymentMethod)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Property(p => p.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);
        
        builder.Property(p => p.EasyPayReference)
            .HasMaxLength(100);
        
        builder.Property(p => p.EasyPayTransactionId)
            .HasMaxLength(100);
        
        builder.Property(p => p.RefundReason)
            .HasMaxLength(500);
        
        builder.Property(p => p.Notes)
            .HasMaxLength(1000);
        
        builder.Property(p => p.CreatedBy)
            .HasMaxLength(255);
        
        builder.Property(p => p.CreatedDate)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");
        
        builder.Property(p => p.LastModifiedBy)
            .HasMaxLength(255);
        
        // Indexes
        builder.HasIndex(p => p.InvoiceNumber)
            .IsUnique()
            .HasDatabaseName("IX_Payments_InvoiceNumber");
        
        builder.HasIndex(p => p.RegistrationId)
            .IsUnique()
            .HasDatabaseName("IX_Payments_RegistrationId");
        
        builder.HasIndex(p => p.Status)
            .HasDatabaseName("IX_Payments_Status");
        
        builder.HasIndex(p => p.EasyPayReference)
            .HasDatabaseName("IX_Payments_EasyPayRef");
        
        builder.HasIndex(p => p.PaidDate)
            .HasDatabaseName("IX_Payments_PaidDate");
        
        // Relationships
        builder.HasOne(p => p.Registration)
            .WithOne(r => r.Payment)
            .HasForeignKey<Payment>(p => p.RegistrationId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasMany(p => p.Transactions)
            .WithOne(t => t.Payment)
            .HasForeignKey(t => t.PaymentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

