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
    /// Gets or sets the SystemSettings DbSet.
    /// </summary>
    DbSet<SystemSetting> SystemSettings { get; set; }

    /// <summary>
    /// Gets or sets the Students DbSet.
    /// </summary>
    DbSet<Student> Students { get; set; }

    /// <summary>
    /// Gets or sets the Registrations DbSet.
    /// </summary>
    DbSet<Registration> Registrations { get; set; }

    /// <summary>
    /// Gets or sets the Payments DbSet.
    /// </summary>
    DbSet<Payment> Payments { get; set; }

    /// <summary>
    /// Gets or sets the TestSessions DbSet.
    /// </summary>
    DbSet<TestSession> TestSessions { get; set; }

    /// <summary>
    /// Gets or sets the Venues DbSet.
    /// </summary>
    DbSet<Venue> Venues { get; set; }

    /// <summary>
    /// Gets or sets the Rooms DbSet.
    /// </summary>
    DbSet<Room> Rooms { get; set; }

    /// <summary>
    /// Gets or sets the RoomAllocations DbSet.
    /// </summary>
    DbSet<RoomAllocation> RoomAllocations { get; set; }

    /// <summary>
    /// Gets or sets the TestResults DbSet.
    /// </summary>
    DbSet<TestResult> TestResults { get; set; }

    /// <summary>
    /// Gets or sets the AuditLogs DbSet.
    /// </summary>
    DbSet<AuditLog> AuditLogs { get; set; }

    /// <summary>
    /// Gets or sets the PaymentTransactions DbSet.
    /// </summary>
    DbSet<PaymentTransaction> PaymentTransactions { get; set; }

    /// <summary>
    /// Gets or sets the VenueAvailabilities DbSet.
    /// </summary>
    DbSet<VenueAvailability> VenueAvailabilities { get; set; }

    /// <summary>
    /// Gets or sets the TestDateCalendar DbSet.
    /// </summary>
    DbSet<TestDateCalendar> TestDateCalendar { get; set; }

    /// <summary>
    /// Gets or sets the TestPricings DbSet.
    /// </summary>
    DbSet<TestPricing> TestPricings { get; set; }

    /// <summary>
    /// Saves all changes made in this context to the database.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The number of state entries written to the database.</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
