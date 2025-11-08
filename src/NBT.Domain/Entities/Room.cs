using System.ComponentModel.DataAnnotations;
using NBT.Domain.Common;

namespace NBT.Domain.Entities;

/// <summary>
/// Represents a room within a venue where tests can be conducted.
/// Contains room specifications and capacity information.
/// </summary>
public class Room : BaseEntity
{
    /// <summary>
    /// Gets or sets the ID of the venue this room belongs to.
    /// </summary>
    [Required]
    public Guid VenueId { get; set; }

    /// <summary>
    /// Gets or sets the name of the room.
    /// Example: "Computer Lab A", "Exam Hall 1"
    /// </summary>
    [Required]
    [StringLength(255)]
    public string RoomName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the room number or identifier.
    /// </summary>
    [StringLength(50)]
    public string? RoomNumber { get; set; }

    /// <summary>
    /// Gets or sets the maximum capacity of the room.
    /// </summary>
    [Required]
    public int Capacity { get; set; }

    /// <summary>
    /// Gets or sets the type of room.
    /// Values: "ComputerLab", "Classroom", "Hall", "ExamRoom"
    /// </summary>
    [Required]
    [StringLength(50)]
    public string RoomType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the room has computers.
    /// </summary>
    public bool HasComputers { get; set; } = false;

    /// <summary>
    /// Gets or sets the number of computers in the room.
    /// </summary>
    public int? ComputerCount { get; set; }

    /// <summary>
    /// Gets or sets whether the room is wheelchair accessible.
    /// </summary>
    public bool IsAccessible { get; set; } = true;

    /// <summary>
    /// Gets or sets the current status of the room.
    /// Values: "Available", "Unavailable", "UnderMaintenance"
    /// </summary>
    [Required]
    [StringLength(50)]
    public string Status { get; set; } = "Available";

    /// <summary>
    /// Gets or sets additional notes about the room.
    /// </summary>
    [StringLength(500)]
    public string? Notes { get; set; }

    // Navigation properties

    /// <summary>
    /// Gets or sets the venue this room belongs to.
    /// </summary>
    public virtual Venue Venue { get; set; } = null!;

    /// <summary>
    /// Gets or sets the room allocations for test sessions.
    /// </summary>
    public virtual ICollection<RoomAllocation> RoomAllocations { get; set; } = new List<RoomAllocation>();
}
