using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NBT.Domain.Entities;

namespace NBT.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework Core configuration for the Student entity.
/// Defines table structure, relationships, indexes, and constraints.
/// </summary>
public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        // Table configuration
        builder.ToTable("Students");
        
        // Primary key
        builder.HasKey(s => s.Id);
        
        // Properties configuration
        builder.Property(s => s.NBTNumber)
            .IsRequired()
            .HasMaxLength(9)
            .IsFixedLength();
        
        builder.Property(s => s.IDNumber)
            .IsRequired()
            .HasMaxLength(13)
            .IsFixedLength();
        
        builder.Property(s => s.FirstName)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(s => s.LastName)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(s => s.Email)
            .IsRequired()
            .HasMaxLength(255);
        
        builder.Property(s => s.Phone)
            .IsRequired()
            .HasMaxLength(20);
        
        builder.Property(s => s.DateOfBirth)
            .IsRequired()
            .HasColumnType("date");
        
        builder.Property(s => s.Gender)
            .IsRequired()
            .HasMaxLength(10);
        
        builder.Property(s => s.Address)
            .HasMaxLength(255);
        
        builder.Property(s => s.City)
            .HasMaxLength(100);
        
        builder.Property(s => s.Province)
            .HasMaxLength(100);
        
        builder.Property(s => s.PostalCode)
            .HasMaxLength(10);
        
        builder.Property(s => s.SchoolName)
            .HasMaxLength(255);
        
        builder.Property(s => s.Grade)
            .IsRequired(false);
        
        builder.Property(s => s.HomeLanguage)
            .HasMaxLength(50);
        
        builder.Property(s => s.SpecialAccommodation)
            .HasMaxLength(500);
        
        builder.Property(s => s.IsActive)
            .IsRequired()
            .HasDefaultValue(true);
        
        // Audit fields
        builder.Property(s => s.CreatedBy)
            .HasMaxLength(255);
        
        builder.Property(s => s.CreatedDate)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");
        
        builder.Property(s => s.LastModifiedBy)
            .HasMaxLength(255);
        
        builder.Property(s => s.LastModifiedDate)
            .IsRequired(false);
        
        // Indexes for performance
        builder.HasIndex(s => s.NBTNumber)
            .IsUnique()
            .HasDatabaseName("IX_Students_NBTNumber");
        
        builder.HasIndex(s => s.IDNumber)
            .IsUnique()
            .HasDatabaseName("IX_Students_IDNumber");
        
        builder.HasIndex(s => s.Email)
            .IsUnique()
            .HasDatabaseName("IX_Students_Email");
        
        builder.HasIndex(s => s.IsActive)
            .HasDatabaseName("IX_Students_IsActive");
        
        builder.HasIndex(s => new { s.FirstName, s.LastName })
            .HasDatabaseName("IX_Students_FullName");
        
        // Relationships
        builder.HasMany(s => s.Registrations)
            .WithOne(r => r.Student)
            .HasForeignKey(r => r.StudentId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasMany(s => s.TestResults)
            .WithOne(tr => tr.Student)
            .HasForeignKey(tr => tr.StudentId)
            .OnDelete(DeleteBehavior.Restrict);
        
        // Ignored computed properties
        builder.Ignore(s => s.FullName);
    }
}

