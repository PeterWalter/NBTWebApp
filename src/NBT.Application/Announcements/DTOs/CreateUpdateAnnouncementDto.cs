using NBT.Domain.Enums;

namespace NBT.Application.Announcements.DTOs;

public class CreateAnnouncementDto
{
    public string Title { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string FullContent { get; set; } = string.Empty;
    public AnnouncementCategory Category { get; set; }
    public DateTime? PublicationDate { get; set; }
    public string Status { get; set; } = "Draft";
    public bool IsFeatured { get; set; }
}

public class UpdateAnnouncementDto
{
    public string Title { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string FullContent { get; set; } = string.Empty;
    public AnnouncementCategory Category { get; set; }
    public string Status { get; set; } = "Draft";
    public bool IsFeatured { get; set; }
}
