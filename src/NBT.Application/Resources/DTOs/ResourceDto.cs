namespace NBT.Application.Resources.DTOs;

/// <summary>
/// Data transfer object for DownloadableResource entity.
/// </summary>
public class ResourceDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string FileType { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string Category { get; set; } = string.Empty;
    public DateTime UploadDate { get; set; }
    public int DownloadCount { get; set; }
    public string Status { get; set; } = string.Empty;
}
