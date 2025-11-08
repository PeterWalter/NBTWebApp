using System.ComponentModel.DataAnnotations;
using NBT.Domain.Common;

namespace NBT.Domain.Entities;

/// <summary>
/// Represents the allocation of a student to a specific room for a test session.
/// Links students to rooms within a venue for a particular session.
/// </summary>
public class RoomAllocation : BaseEntity
{
    /// <summary>
    /// Gets or sets the ID of the test session.
    /// </summary>
    [Required]
    public Guid TestSessionId { get; set; }

    /// <summary>
    /// Gets or sets the ID of the room.
    /// </summary>
    [Required]
    public Guid RoomId { get; set; }

    /// <summary>
    /// Gets or sets the ID of the student allocated to this room.
    /// </summary>
    [Required]
    public Guid StudentId { get; set; }

    /// <summary>
    /// Gets or sets the seat number assigned to the student in this room.
    /// </summary>
    [StringLength(20)]
    public string? SeatNumber { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the allocation was made.
    /// </summary>
    public DateTime AllocationDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets additional notes about this allocation.
    /// </summary>
    [StringLength(500)]
    public string? Notes { get; set; }

    // Navigation properties

    /// <summary>
    /// Gets or sets the test session for this allocation.
    /// </summary>
    public virtual TestSession TestSession { get; set; } = null!;

    /// <summary>
    /// Gets or sets the room for this allocation.
    /// </summary>
    public virtual Room Room { get; set; } = null!;

    /// <summary>
    /// Gets or sets the student for this allocation.
    /// </summary>
    public virtual Student Student { get; set; } = null!;
}
