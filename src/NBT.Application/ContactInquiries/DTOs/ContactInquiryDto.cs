using NBT.Domain.Enums;

namespace NBT.Application.ContactInquiries.DTOs;

/// <summary>
/// Data transfer object for ContactInquiry entity.
/// </summary>
public class ContactInquiryDto
{
    public Guid Id { get; set; }
    public DateTime SubmissionDateTime { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public InquiryType InquiryType { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public InquiryStatus Status { get; set; }
    public Guid? AssignedToId { get; set; }
    public string? Response { get; set; }
    public string ReferenceNumber { get; set; } = string.Empty;
}
