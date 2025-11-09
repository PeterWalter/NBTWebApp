using System.ComponentModel.DataAnnotations;
using NBT.Domain.Common;

namespace NBT.Domain.Entities;

/// <summary>
/// Represents a test date in the annual calendar.
/// Tracks test dates, closing dates, and special characteristics.
/// </summary>
public class TestDateCalendar : BaseEntity
{
    /// <summary>
    /// Gets or sets the test date.
    /// </summary>
    [Required]
    public DateTime TestDate { get; set; }

    /// <summary>
    /// Gets or sets the closing date for bookings.
    /// Students must book before this date.
    /// </summary>
    [Required]
    public DateTime ClosingBookingDate { get; set; }

    /// <summary>
    /// Gets or sets whether this test is on a Sunday.
    /// Sunday tests are highlighted differently in the calendar.
    /// </summary>
    [Required]
    public bool IsSunday { get; set; }

    /// <summary>
    /// Gets or sets whether this is an online test.
    /// Online tests can be taken from anywhere with proper setup.
    /// </summary>
    [Required]
    public bool IsOnline { get; set; }

    /// <summary>
    /// Gets or sets whether this test date is active.
    /// Inactive dates are not shown to students.
    /// </summary>
    [Required]
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Gets or sets the intake year this test date belongs to.
    /// Example: 2024, 2025
    /// </summary>
    [Required]
    public int IntakeYear { get; set; }

    /// <summary>
    /// Gets or sets additional notes about this test date.
    /// Examples: "Special session", "Research project", etc.
    /// </summary>
    [StringLength(1000)]
    public string? Notes { get; set; }
}
