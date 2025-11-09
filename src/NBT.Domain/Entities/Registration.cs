using System.ComponentModel.DataAnnotations;
using NBT.Domain.Common;
using NBT.Domain.Enums;

namespace NBT.Domain.Entities;

/// <summary>
/// Represents a student's registration for a test session.
/// Links a student to a specific test session with selected test types.
/// </summary>
public class Registration : BaseEntity
{
    /// <summary>
    /// Gets or sets the registration number (format: REG-YYYY-NNNNNN).
    /// Example: REG-2024-000123
    /// </summary>
    [Required]
    [StringLength(20)]
    public string RegistrationNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the ID of the student for this registration.
    /// </summary>
    [Required]
    public Guid StudentId { get; set; }

    /// <summary>
    /// Gets or sets the ID of the test session for this registration.
    /// </summary>
    [Required]
    public Guid TestSessionId { get; set; }

    /// <summary>
    /// Gets or sets the current status of the registration.
    /// </summary>
    [Required]
    public RegistrationStatus Status { get; set; } = RegistrationStatus.Pending;

    /// <summary>
    /// Gets or sets the test types selected by the student (JSON array).
    /// Values can be: AcademicLiteracy, QuantitativeLiteracy, Mathematics
    /// Example: ["AcademicLiteracy", "Mathematics"]
    /// </summary>
    [Required]
    [StringLength(255)]
    public string TestTypesSelected { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the student requires a remote writer.
    /// </summary>
    public bool IsRemoteWriter { get; set; } = false;

    /// <summary>
    /// Gets or sets the remote location if applicable.
    /// </summary>
    [StringLength(255)]
    public string? RemoteLocation { get; set; }

    /// <summary>
    /// Gets or sets the special session type if applicable.
    /// Examples: "Weekend", "Evening", "Make-up", etc.
    /// </summary>
    [StringLength(100)]
    public string? SpecialSessionType { get; set; }

    /// <summary>
    /// Gets or sets the date when the registration was submitted.
    /// </summary>
    [Required]
    public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the date when the registration was confirmed.
    /// </summary>
    public DateTime? ConfirmationDate { get; set; }

    /// <summary>
    /// Gets or sets the date when the registration was cancelled.
    /// </summary>
    public DateTime? CancellationDate { get; set; }

    /// <summary>
    /// Gets or sets the reason for cancellation if applicable.
    /// </summary>
    [StringLength(500)]
    public string? CancellationReason { get; set; }

    // Navigation properties

    /// <summary>
    /// Gets or sets the student associated with this registration.
    /// </summary>
    public virtual Student Student { get; set; } = null!;

    /// <summary>
    /// Gets or sets the test session associated with this registration.
    /// </summary>
    public virtual TestSession TestSession { get; set; } = null!;

    /// <summary>
    /// Gets or sets the payment for this registration.
    /// </summary>
    public virtual Payment? Payment { get; set; }

    /// <summary>
    /// Gets or sets the test results for this registration.
    /// </summary>
    public virtual ICollection<TestResult> TestResults { get; set; } = new List<TestResult>();
}
