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
    /// Gets or sets the ID of the registration.
    /// Links result to specific booking and payment.
    /// </summary>
    [Required]
    public Guid RegistrationId { get; set; }

    /// <summary>
    /// Gets or sets the unique barcode that identifies this specific test instance.
    /// Format: BC-{NBTNumber}-{TestDate}-{Sequence}
    /// Example: BC-20240000000123-20240615-001
    /// Distinguishes multiple tests written by the same student.
    /// </summary>
    [Required]
    [StringLength(50)]
    public string Barcode { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the type of test.
    /// Values: "AQL" (Academic and Quantitative Literacy) or "AQL_MAT" (includes Mathematics)
    /// </summary>
    [Required]
    [StringLength(20)]
    public string TestType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the Academic Literacy (AL) score.
    /// </summary>
    public decimal? ALScore { get; set; }

    /// <summary>
    /// Gets or sets the Academic Literacy performance level.
    /// Values: "Basic Lower", "Basic Upper", "Intermediate Lower", "Intermediate Upper", 
    /// "Proficient Lower", "Proficient Upper", "Outstanding"
    /// </summary>
    [StringLength(50)]
    public string? ALPerformanceLevel { get; set; }

    /// <summary>
    /// Gets or sets the Quantitative Literacy (QL) score.
    /// </summary>
    public decimal? QLScore { get; set; }

    /// <summary>
    /// Gets or sets the Quantitative Literacy performance level.
    /// Values: "Basic Lower", "Basic Upper", "Intermediate Lower", "Intermediate Upper", 
    /// "Proficient Lower", "Proficient Upper", "Outstanding"
    /// </summary>
    [StringLength(50)]
    public string? QLPerformanceLevel { get; set; }

    /// <summary>
    /// Gets or sets the Mathematics (MAT) score.
    /// Only applicable for AQL_MAT tests.
    /// </summary>
    public decimal? MATScore { get; set; }

    /// <summary>
    /// Gets or sets the Mathematics performance level.
    /// Values: "Basic Lower", "Basic Upper", "Intermediate Lower", "Intermediate Upper", 
    /// "Proficient Lower", "Proficient Upper", "Outstanding"
    /// Only applicable for AQL_MAT tests.
    /// </summary>
    [StringLength(50)]
    public string? MATPerformanceLevel { get; set; }

    /// <summary>
    /// Gets or sets the overall performance band across all domains.
    /// </summary>
    [Required]
    [StringLength(50)]
    public string OverallPerformanceBand { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the percentile rank (1-99).
    /// Indicates how the student performed relative to others.
    /// </summary>
    [Required]
    public int Percentile { get; set; }

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

    /// <summary>
    /// Gets or sets the registration these results are for.
    /// </summary>
    public virtual Registration Registration { get; set; } = null!;
}
