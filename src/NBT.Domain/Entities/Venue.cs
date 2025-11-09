using System.ComponentModel.DataAnnotations;
using NBT.Domain.Common;

namespace NBT.Domain.Entities;

/// <summary>
/// Represents a physical venue where NBT tests are conducted.
/// Contains venue details, location, and capacity information.
/// </summary>
public class Venue : BaseEntity
{
    /// <summary>
    /// Gets or sets the name of the venue.
    /// Example: "University of Johannesburg - Auckland Park Campus"
    /// </summary>
    [Required]
    [StringLength(255)]
    public string VenueName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the unique venue code.
    /// Example: "UJ-AP", "UCT-MAIN"
    /// </summary>
    [Required]
    [StringLength(20)]
    public string VenueCode { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the type of venue.
    /// Values: National, SpecialSession, Research, Online, Other
    /// </summary>
    [Required]
    [StringLength(50)]
    public string VenueType { get; set; } = "National";

    /// <summary>
    /// Gets or sets the physical address of the venue.
    /// </summary>
    [Required]
    [StringLength(255)]
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the city where the venue is located.
    /// </summary>
    [Required]
    [StringLength(100)]
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the province where the venue is located.
    /// </summary>
    [Required]
    [StringLength(100)]
    public string Province { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the postal code of the venue.
    /// </summary>
    [StringLength(10)]
    public string? PostalCode { get; set; }

    /// <summary>
    /// Gets or sets the name of the contact person at the venue.
    /// </summary>
    [StringLength(200)]
    public string? ContactPerson { get; set; }

    /// <summary>
    /// Gets or sets the contact email for the venue.
    /// </summary>
    [EmailAddress]
    [StringLength(255)]
    public string? ContactEmail { get; set; }

    /// <summary>
    /// Gets or sets the contact phone number for the venue.
    /// </summary>
    [Phone]
    [StringLength(20)]
    public string? ContactPhone { get; set; }

    /// <summary>
    /// Gets or sets the total capacity of the venue (sum of all rooms).
    /// </summary>
    public int TotalCapacity { get; set; }

    /// <summary>
    /// Gets or sets whether the venue is wheelchair accessible.
    /// </summary>
    public bool IsAccessible { get; set; } = true;

    /// <summary>
    /// Gets or sets the current status of the venue.
    /// Values: "Active", "Inactive", "UnderMaintenance"
    /// </summary>
    [Required]
    [StringLength(50)]
    public string Status { get; set; } = "Active";

    /// <summary>
    /// Gets or sets additional notes about the venue.
    /// </summary>
    [StringLength(1000)]
    public string? Notes { get; set; }

    // Navigation properties

    /// <summary>
    /// Gets or sets the rooms in this venue.
    /// </summary>
    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();

    /// <summary>
    /// Gets or sets the test sessions scheduled at this venue.
    /// </summary>
    public virtual ICollection<TestSession> TestSessions { get; set; } = new List<TestSession>();

    /// <summary>
    /// Gets or sets the venue availability records.
    /// </summary>
    public virtual ICollection<VenueAvailability> VenueAvailabilities { get; set; } = new List<VenueAvailability>();
}
