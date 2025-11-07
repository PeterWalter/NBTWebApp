using NBT.Domain.Common;
using NBT.Domain.Enums;

namespace NBT.Domain.Entities;

/// <summary>
/// Represents a contact inquiry submitted through the website.
/// </summary>
public class ContactInquiry : BaseEntity
{
    /// <summary>
    /// Gets or sets the date and time when the inquiry was submitted.
    /// </summary>
    public DateTime SubmissionDateTime { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the name of the person submitting the inquiry.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the email address of the inquirer.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the phone number (optional).
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Gets or sets the type of inquiry.
    /// </summary>
    public InquiryType InquiryType { get; set; }

    /// <summary>
    /// Gets or sets the inquiry subject.
    /// </summary>
    public string Subject { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the inquiry message (max 1000 characters).
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the current status of the inquiry.
    /// </summary>
    public InquiryStatus Status { get; set; } = InquiryStatus.New;

    /// <summary>
    /// Gets or sets the ID of the staff member assigned to handle the inquiry.
    /// </summary>
    public Guid? AssignedToId { get; set; }

    /// <summary>
    /// Gets or sets the response provided by staff.
    /// </summary>
    public string? Response { get; set; }

    /// <summary>
    /// Gets or sets whether the user consented to the privacy policy.
    /// </summary>
    public bool PrivacyConsent { get; set; }

    /// <summary>
    /// Gets or sets the reference number for tracking purposes.
    /// </summary>
    public string ReferenceNumber { get; set; } = string.Empty;
}
