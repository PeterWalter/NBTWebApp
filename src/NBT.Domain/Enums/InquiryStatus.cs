namespace NBT.Domain.Enums;

/// <summary>
/// Represents the current status of a contact inquiry.
/// </summary>
public enum InquiryStatus
{
    /// <summary>
    /// Inquiry has been submitted but not yet reviewed.
    /// </summary>
    New = 1,

    /// <summary>
    /// Inquiry is being processed by staff.
    /// </summary>
    InProgress = 2,

    /// <summary>
    /// Inquiry has been resolved with a response.
    /// </summary>
    Resolved = 3,

    /// <summary>
    /// Inquiry has been closed (no further action required).
    /// </summary>
    Closed = 4
}
