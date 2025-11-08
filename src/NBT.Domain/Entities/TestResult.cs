using System.ComponentModel.DataAnnotations;
using NBT.Domain.Common;

namespace NBT.Domain.Entities;

/// <summary>
/// Represents the results of a student's NBT test.
/// Contains scores, percentiles, and performance bands.
/// </summary>
public class TestResult : BaseEntity
{
    /// <summary>
    /// Gets or sets the ID of the student.
    /// </summary>
    [Required]
    public Guid StudentId { get; set; }

    /// <summary>
    /// Gets or sets the ID of the test session.
    /// </summary>
    [Required]
    public Guid TestSessionId { get; set; }

    /// <summary>
    /// Gets or sets the type of test.
    /// Values: "AcademicLiteracy", "QuantitativeLiteracy", "Mathematics"
    /// </summary>
    [Required]
    [StringLength(50)]
    public string TestType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the raw score (0-100).
    /// </summary>
    [Required]
    public decimal RawScore { get; set; }

    /// <summary>
    /// Gets or sets the percentile rank (1-99).
    /// Indicates how the student performed relative to others.
    /// </summary>
    [Required]
    public int Percentile { get; set; }

    /// <summary>
    /// Gets or sets the performance band achieved.
    /// Values: "Elementary", "Basic", "Intermediate", "Proficient"
    /// </summary>
    [Required]
    [StringLength(50)]
    public string PerformanceBand { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the results have been released to the student.
    /// </summary>
    public bool IsReleased { get; set; } = false;

    /// <summary>
    /// Gets or sets the date when the test was taken.
    /// </summary>
    [Required]
    public DateTime TestDate { get; set; }

    /// <summary>
    /// Gets or sets the date when the results were processed.
    /// </summary>
    public DateTime? ResultDate { get; set; }

    /// <summary>
    /// Gets or sets the date when the results were released to the student.
    /// </summary>
    public DateTime? ReleasedDate { get; set; }

    // Navigation properties

    /// <summary>
    /// Gets or sets the student these results belong to.
    /// </summary>
    public virtual Student Student { get; set; } = null!;

    /// <summary>
    /// Gets or sets the test session these results are from.
    /// </summary>
    public virtual TestSession TestSession { get; set; } = null!;
}
