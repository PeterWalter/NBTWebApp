using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NBT.Domain.Entities;

namespace NBT.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for DownloadableResource.
/// </summary>
public class DownloadableResourceConfiguration : IEntityTypeConfiguration<DownloadableResource>
{
    public void Configure(EntityTypeBuilder<DownloadableResource> builder)
    {
        builder.ToTable("DownloadableResources");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Title)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(r => r.Description)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(r => r.FilePath)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(r => r.FileType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(r => r.FileSize)
            .IsRequired();

        builder.Property(r => r.Category)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(r => r.Status)
            .IsRequired()
            .HasMaxLength(50)
            .HasDefaultValue("Active");

        builder.Property(r => r.DownloadCount)
            .HasDefaultValue(0);

        builder.Property(r => r.UploadDate)
            .IsRequired();

        builder.Property(r => r.CreatedDate)
            .IsRequired();

        builder.Property(r => r.CreatedBy)
            .HasMaxLength(256);

        builder.Property(r => r.LastModifiedBy)
            .HasMaxLength(256);

        builder.HasIndex(r => r.Category);
        builder.HasIndex(r => r.Status);
    }
}
