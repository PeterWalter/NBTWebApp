using Microsoft.EntityFrameworkCore;
using NBT.Domain.Entities;

namespace NBT.Application.Common.Interfaces;

/// <summary>
/// Interface for the application database context.
/// </summary>
public interface IApplicationDbContext
{
    /// <summary>
    /// Gets or sets the ContentPages DbSet.
    /// </summary>
    DbSet<ContentPage> ContentPages { get; set; }

    /// <summary>
    /// Gets or sets the Announcements DbSet.
    /// </summary>
    DbSet<Announcement> Announcements { get; set; }

    /// <summary>
    /// Gets or sets the ContactInquiries DbSet.
    /// </summary>
    DbSet<ContactInquiry> ContactInquiries { get; set; }

    /// <summary>
    /// Gets or sets the Users DbSet.
    /// </summary>
    DbSet<User> Users { get; set; }

    /// <summary>
    /// Gets or sets the DownloadableResources DbSet.
    /// </summary>
    DbSet<DownloadableResource> DownloadableResources { get; set; }

    /// <summary>
    /// Saves all changes made in this context to the database.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The number of state entries written to the database.</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
