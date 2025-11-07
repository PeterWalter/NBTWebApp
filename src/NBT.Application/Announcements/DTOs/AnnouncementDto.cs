using NBT.Domain.Enums;

namespace NBT.Application.Announcements.DTOs;

/// <summary>
/// Data transfer object for Announcement entity.
/// </summary>
public class AnnouncementDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string FullContent { get; set; } = string.Empty;
    public AnnouncementCategory Category { get; set; }
    public DateTime PublicationDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public bool IsFeatured { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? LastModifiedDate { get; set; }
}
