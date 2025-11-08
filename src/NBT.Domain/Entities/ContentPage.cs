using NBT.Domain.Common;

namespace NBT.Domain.Entities;

/// <summary>
/// Represents a content page on the website (e.g., About, Policies).
/// </summary>
public class ContentPage : BaseEntity
{
    /// <summary>
    /// Gets or sets the page title.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the URL-friendly slug for the page.
    /// </summary>
    public string Slug { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the HTML body content of the page.
    /// </summary>
    public string BodyContent { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the meta description for SEO.
    /// </summary>
    public string? MetaDescription { get; set; }

    /// <summary>
    /// Gets or sets the meta keywords for SEO.
    /// </summary>
    public string? Keywords { get; set; }

    /// <summary>
    /// Gets or sets the publication date of the page.
    /// </summary>
    public DateTime PublicationDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the page status (Draft, Published, Archived).
    /// </summary>
    public string Status { get; set; } = "Draft";
}
