using System.ComponentModel.DataAnnotations;
using NBT.Domain.Common;
using NBT.Domain.Enums;

namespace NBT.Domain.Entities;

/// <summary>
/// Represents a scheduled test session at a specific venue.
/// Manages capacity, registrations, and session lifecycle.
/// </summary>
public class TestSession : BaseEntity
{
    /// <summary>
    /// Gets or sets the session code (format: CITY-YYYY-MM-DD-PERIOD).
    /// Example: JHB-2024-06-15-AM
    /// </summary>
    [Required]
    [StringLength(50)]
    public string SessionCode { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the descriptive name of the session.
    /// Example: "Johannesburg Morning Session - June 2024"
    /// </summary>
    [Required]
    [StringLength(255)]
    public string SessionName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date of the test session.
    /// </summary>
    [Required]
    public DateTime SessionDate { get; set; }

    /// <summary>
    /// Gets or sets the start time of the session.
    /// </summary>
    [Required]
    public TimeSpan StartTime { get; set; }

    /// <summary>
    /// Gets or sets the end time of the session.
    /// </summary>
    [Required]
    public TimeSpan EndTime { get; set; }

    /// <summary>
    /// Gets or sets the ID of the venue where the session will be held.
    /// </summary>
    [Required]
    public Guid VenueId { get; set; }

    /// <summary>
    /// Gets or sets the maximum capacity for this session.
    /// </summary>
    [Required]
    public int Capacity { get; set; }

    /// <summary>
    /// Gets or sets the current number of confirmed registrations.
    /// </summary>
    public int CurrentRegistrations { get; set; } = 0;

    /// <summary>
    /// Gets the number of available seats (computed property).
    /// </summary>
    public int AvailableSeats => Capacity - CurrentRegistrations;

    /// <summary>
    /// Gets or sets the current status of the session.
    /// </summary>
    [Required]
    public SessionStatus Status { get; set; } = SessionStatus.Open;

    /// <summary>
    /// Gets or sets whether this is a special session.
    /// Special sessions may have different rules or requirements.
    /// </summary>
    public bool IsSpecialSession { get; set; } = false;

    /// <summary>
    /// Gets or sets notes about the special session.
    /// </summary>
    [StringLength(1000)]
    public string? SpecialSessionNotes { get; set; }

    /// <summary>
    /// Gets or sets additional notes about the session.
    /// </summary>
    [StringLength(1000)]
    public string? Notes { get; set; }

    // Navigation properties

    /// <summary>
    /// Gets or sets the venue for this test session.
    /// </summary>
    public virtual Venue Venue { get; set; } = null!;

    /// <summary>
    /// Gets or sets the registrations for this session.
    /// </summary>
    public virtual ICollection<Registration> Registrations { get; set; } = new List<Registration>();

    /// <summary>
    /// Gets or sets the room allocations for this session.
    /// </summary>
    public virtual ICollection<RoomAllocation> RoomAllocations { get; set; } = new List<RoomAllocation>();
}
