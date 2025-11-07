namespace NBT.Application.ContentPages.DTOs;

public class CreateContentPageDto
{
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string BodyContent { get; set; } = string.Empty;
    public string? MetaDescription { get; set; }
    public string? Keywords { get; set; }
    public DateTime? PublicationDate { get; set; }
    public string Status { get; set; } = "Draft";
}

public class UpdateContentPageDto
{
    public string Title { get; set; } = string.Empty;
    public string BodyContent { get; set; } = string.Empty;
    public string? MetaDescription { get; set; }
    public string? Keywords { get; set; }
    public string Status { get; set; } = "Draft";
}
