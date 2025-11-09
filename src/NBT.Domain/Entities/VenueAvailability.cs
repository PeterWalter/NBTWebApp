using System.ComponentModel.DataAnnotations;
using NBT.Domain.Common;

namespace NBT.Domain.Entities;

/// <summary>
/// Represents the availability of a venue for specific test dates.
/// Tracks which venues are available or unavailable for testing.
/// </summary>
public class VenueAvailability : BaseEntity
{
    /// <summary>
    /// Gets or sets the ID of the venue.
    /// </summary>
    [Required]
    public Guid VenueId { get; set; }

    /// <summary>
    /// Gets or sets the test date.
    /// </summary>
    [Required]
    public DateTime TestDate { get; set; }

    /// <summary>
    /// Gets or sets whether the venue is available on this date.
    /// </summary>
    [Required]
    public bool IsAvailable { get; set; } = true;

    /// <summary>
    /// Gets or sets the reason if the venue is unavailable.
    /// Examples: "Under renovation", "Booked for other event", "Holiday"
    /// </summary>
    [StringLength(500)]
    public string? UnavailableReason { get; set; }

    /// <summary>
    /// Gets or sets additional notes about the availability.
    /// </summary>
    [StringLength(1000)]
    public string? Notes { get; set; }

    // Navigation property

    /// <summary>
    /// Gets or sets the venue this availability record is for.
    /// </summary>
    public virtual Venue Venue { get; set; } = null!;
}
