using NBT.Domain.Enums;

namespace NBT.Application.ContactInquiries.DTOs;

public class CreateContactInquiryDto
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public InquiryType InquiryType { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}
