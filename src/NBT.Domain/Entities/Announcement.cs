using NBT.Domain.Common;
using NBT.Domain.Enums;

namespace NBT.Domain.Entities;

/// <summary>
/// Represents an announcement or news item.
/// </summary>
public class Announcement : BaseEntity
{
    /// <summary>
    /// Gets or sets the announcement title.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a brief summary of the announcement.
    /// </summary>
    public string Summary { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the full HTML content of the announcement.
    /// </summary>
    public string FullContent { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the announcement category.
    /// </summary>
    public AnnouncementCategory Category { get; set; }

    /// <summary>
    /// Gets or sets the publication date.
    /// </summary>
    public DateTime PublicationDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the status (Published, Draft, Archived).
    /// </summary>
    public string Status { get; set; } = "Draft";

    /// <summary>
    /// Gets or sets whether the announcement is featured on the homepage.
    /// </summary>
    public bool IsFeatured { get; set; }
}
