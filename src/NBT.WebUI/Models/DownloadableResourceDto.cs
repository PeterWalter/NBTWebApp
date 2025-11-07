namespace NBT.WebUI.Models;

public class DownloadableResourceDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public string? FileName { get; set; }
    public long? FileSizeBytes { get; set; }
    public string? MimeType { get; set; }
    public int DownloadCount { get; set; }
    public DateTime UploadedAt { get; set; }
}
