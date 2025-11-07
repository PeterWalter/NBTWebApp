using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NBT.Domain.Entities;

namespace NBT.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for Announcement.
/// </summary>
public class AnnouncementConfiguration : IEntityTypeConfiguration<Announcement>
{
    public void Configure(EntityTypeBuilder<Announcement> builder)
    {
        builder.ToTable("Announcements");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Title)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(a => a.Summary)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(a => a.FullContent)
            .IsRequired();

        builder.Property(a => a.Category)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(a => a.Status)
            .IsRequired()
            .HasMaxLength(50)
            .HasDefaultValue("Draft");

        builder.Property(a => a.IsFeatured)
            .HasDefaultValue(false);

        builder.Property(a => a.PublicationDate)
            .IsRequired();

        builder.Property(a => a.CreatedDate)
            .IsRequired();

        builder.Property(a => a.CreatedBy)
            .HasMaxLength(256);

        builder.Property(a => a.LastModifiedBy)
            .HasMaxLength(256);

        builder.HasIndex(a => a.PublicationDate);
        builder.HasIndex(a => a.Category);
        builder.HasIndex(a => a.IsFeatured);
    }
}
