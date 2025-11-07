namespace NBT.Domain.Common;

/// <summary>
/// Interface for entities that support audit tracking.
/// </summary>
public interface IAuditableEntity
{
    /// <summary>
    /// Gets or sets the user who created the entity.
    /// </summary>
    string? CreatedBy { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the entity was created.
    /// </summary>
    DateTime CreatedDate { get; set; }

    /// <summary>
    /// Gets or sets the user who last modified the entity.
    /// </summary>
    string? LastModifiedBy { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the entity was last modified.
    /// </summary>
    DateTime? LastModifiedDate { get; set; }
}
