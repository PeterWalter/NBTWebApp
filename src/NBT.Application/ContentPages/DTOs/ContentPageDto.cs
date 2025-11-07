namespace NBT.Application.ContentPages.DTOs;

/// <summary>
/// Data transfer object for ContentPage entity.
/// </summary>
public class ContentPageDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string BodyContent { get; set; } = string.Empty;
    public string? MetaDescription { get; set; }
    public string? Keywords { get; set; }
    public DateTime PublicationDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public DateTime? LastModifiedDate { get; set; }
}
