using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NBT.Domain.Entities;

namespace NBT.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for ContactInquiry.
/// </summary>
public class ContactInquiryConfiguration : IEntityTypeConfiguration<ContactInquiry>
{
    public void Configure(EntityTypeBuilder<ContactInquiry> builder)
    {
        builder.ToTable("ContactInquiries");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.Email)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(c => c.Phone)
            .HasMaxLength(20);

        builder.Property(c => c.InquiryType)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(c => c.Subject)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(c => c.Message)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(c => c.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(c => c.Response)
            .HasMaxLength(2000);

        builder.Property(c => c.ReferenceNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(c => c.ReferenceNumber)
            .IsUnique();

        builder.Property(c => c.SubmissionDateTime)
            .IsRequired();

        builder.Property(c => c.PrivacyConsent)
            .IsRequired();

        builder.HasIndex(c => c.Status);
        builder.HasIndex(c => c.SubmissionDateTime);
    }
}
