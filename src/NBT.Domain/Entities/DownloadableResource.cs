using NBT.Domain.Common;

namespace NBT.Domain.Entities;

/// <summary>
/// Represents a downloadable resource (e.g., PDF guides for educators).
/// </summary>
public class DownloadableResource : BaseEntity
{
    /// <summary>
    /// Gets or sets the resource title.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the resource description.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the file path or blob storage URL.
    /// </summary>
    public string FilePath { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the file type (e.g., PDF, DOCX).
    /// </summary>
    public string FileType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the file size in bytes.
    /// </summary>
    public long FileSize { get; set; }

    /// <summary>
    /// Gets or sets the resource category (Educator, Institution, General).
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the upload date.
    /// </summary>
    public DateTime UploadDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the download count for analytics.
    /// </summary>
    public int DownloadCount { get; set; }

    /// <summary>
    /// Gets or sets the resource status (Active, Archived).
    /// </summary>
    public string Status { get; set; } = "Active";
}
