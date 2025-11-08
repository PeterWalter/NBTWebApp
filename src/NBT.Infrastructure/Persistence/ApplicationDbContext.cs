using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NBT.Application.Common.Interfaces;
using NBT.Domain.Common;
using NBT.Domain.Entities;

namespace NBT.Infrastructure.Persistence;

/// <summary>
/// Application database context implementing Identity and custom entities.
/// </summary>
public class ApplicationDbContext : IdentityDbContext<User, Microsoft.AspNetCore.Identity.IdentityRole<Guid>, Guid>, IApplicationDbContext
{
    private readonly ICurrentUserService? _currentUserService;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        ICurrentUserService? currentUserService = null)
        : base(options)
    {
        _currentUserService = currentUserService;
    }

    // Existing entities
    public DbSet<ContentPage> ContentPages { get; set; } = null!;
    public DbSet<Announcement> Announcements { get; set; } = null!;
    public DbSet<ContactInquiry> ContactInquiries { get; set; } = null!;
    public new DbSet<User> Users { get; set; } = null!;
    public DbSet<DownloadableResource> DownloadableResources { get; set; } = null!;
    public DbSet<SystemSetting> SystemSettings { get; set; } = null!;

    // NBT Core entities
    public DbSet<Student> Students { get; set; } = null!;
    public DbSet<Registration> Registrations { get; set; } = null!;
    public DbSet<Payment> Payments { get; set; } = null!;
    public DbSet<TestSession> TestSessions { get; set; } = null!;
    public DbSet<Venue> Venues { get; set; } = null!;
    public DbSet<Room> Rooms { get; set; } = null!;
    public DbSet<RoomAllocation> RoomAllocations { get; set; } = null!;
    public DbSet<TestResult> TestResults { get; set; } = null!;
    public DbSet<AuditLog> AuditLogs { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Apply all entity configurations from this assembly
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Update audit fields for entities implementing IAuditableEntity
        foreach (var entry in ChangeTracker.Entries<IAuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = _currentUserService?.UserName;
                    entry.Entity.CreatedDate = DateTime.UtcNow;
                    break;

                case EntityState.Modified:
                    entry.Entity.LastModifiedBy = _currentUserService?.UserName;
                    entry.Entity.LastModifiedDate = DateTime.UtcNow;
                    break;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
