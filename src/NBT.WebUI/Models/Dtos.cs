namespace NBT.WebUI.Models;

public enum AnnouncementCategory
{
    PolicyUpdate = 1,
    TestDates = 2,
    FeeChanges = 3,
    GeneralNews = 4
}

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

public class ContentPageDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? MetaKeywords { get; set; }
    public bool IsPublished { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? LastModifiedDate { get; set; }
}

public class DownloadableResourceDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public string FileExtension { get; set; } = string.Empty;
    public long FileSizeBytes { get; set; }
    public string DownloadUrl { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public int DownloadCount { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? LastModifiedDate { get; set; }
}

public class ContactInquiryDto
{
    public Guid Id { get; set; }
    public string ReferenceNumber { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime SubmissionDate { get; set; }
}

public class CreateContactInquiryDto
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}
