using System.ComponentModel.DataAnnotations;
using NBT.Domain.Common;

namespace NBT.Domain.Entities;

/// <summary>
/// Represents the pricing for test types by intake year.
/// Test costs can vary from year to year.
/// </summary>
public class TestPricing : BaseEntity
{
    /// <summary>
    /// Gets or sets the intake year this pricing applies to.
    /// Example: 2024, 2025
    /// </summary>
    [Required]
    public int IntakeYear { get; set; }

    /// <summary>
    /// Gets or sets the test type.
    /// Values: "AQL" (Academic and Quantitative Literacy) or "AQL_MAT" (includes Mathematics)
    /// </summary>
    [Required]
    [StringLength(20)]
    public string TestType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the price in ZAR.
    /// </summary>
    [Required]
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets the date this pricing becomes effective.
    /// </summary>
    [Required]
    public DateTime EffectiveFrom { get; set; }

    /// <summary>
    /// Gets or sets the date this pricing stops being effective.
    /// Null means it's currently active.
    /// </summary>
    public DateTime? EffectiveTo { get; set; }

    /// <summary>
    /// Gets or sets whether this pricing is currently active.
    /// </summary>
    [Required]
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Gets or sets additional notes about this pricing.
    /// </summary>
    [StringLength(500)]
    public string? Notes { get; set; }
}
