using System.ComponentModel.DataAnnotations;
using NBT.Domain.Common;

namespace NBT.Domain.Entities;

/// <summary>
/// Represents an immutable audit log entry for tracking system changes.
/// All create, update, delete, and critical operations are logged here.
/// Constitution Section 8 compliance: Full audit trail required.
/// </summary>
public class AuditLog : BaseEntity
{
    /// <summary>
    /// Gets or sets the timestamp of the action (UTC).
    /// </summary>
    [Required]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the ID of the user who performed the action.
    /// </summary>
    [Required]
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the email of the user who performed the action.
    /// </summary>
    [Required]
    [StringLength(255)]
    public string UserEmail { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the action performed.
    /// Values: "Create", "Update", "Delete", "Login", "Logout", "Import", "Export", "View"
    /// </summary>
    [Required]
    [StringLength(50)]
    public string Action { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the type of entity affected.
    /// Example: "Student", "Registration", "Payment", "TestResult"
    /// </summary>
    [Required]
    [StringLength(100)]
    public string EntityType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the ID of the entity affected.
    /// </summary>
    public Guid? EntityId { get; set; }

    /// <summary>
    /// Gets or sets the state of the entity before the change (JSON).
    /// </summary>
    [StringLength(int.MaxValue)]
    public string? BeforeValue { get; set; }

    /// <summary>
    /// Gets or sets the state of the entity after the change (JSON).
    /// </summary>
    [StringLength(int.MaxValue)]
    public string? AfterValue { get; set; }

    /// <summary>
    /// Gets or sets the IP address of the client.
    /// </summary>
    [StringLength(45)]
    public string? IpAddress { get; set; }

    /// <summary>
    /// Gets or sets the user agent (browser/client information).
    /// </summary>
    [StringLength(500)]
    public string? UserAgent { get; set; }

    /// <summary>
    /// Gets or sets additional context or notes about the action.
    /// </summary>
    [StringLength(1000)]
    public string? Notes { get; set; }
}
