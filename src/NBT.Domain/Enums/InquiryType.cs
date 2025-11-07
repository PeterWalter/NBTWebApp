namespace NBT.Domain.Enums;

/// <summary>
/// Represents the type of inquiry submitted through the contact form.
/// </summary>
public enum InquiryType
{
    /// <summary>
    /// Inquiry from a prospective test applicant.
    /// </summary>
    Applicant = 1,

    /// <summary>
    /// Inquiry from an educator (teacher, counselor, school administrator).
    /// </summary>
    Educator = 2,

    /// <summary>
    /// Inquiry from a higher education institution.
    /// </summary>
    Institution = 3,

    /// <summary>
    /// Inquiry from media representatives.
    /// </summary>
    Media = 4,

    /// <summary>
    /// General inquiry that doesn't fit other categories.
    /// </summary>
    Other = 5
}
