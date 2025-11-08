namespace NBT.Domain.Enums;

/// <summary>
/// Represents the type of identification document used by a student.
/// Supports South African IDs, Foreign IDs, and Passports.
/// </summary>
public enum IDType
{
    /// <summary>
    /// South African ID number (13 digits with Luhn validation).
    /// </summary>
    SA_ID = 1,

    /// <summary>
    /// Foreign ID number for international students.
    /// </summary>
    FOREIGN_ID = 2,

    /// <summary>
    /// Passport number for international students.
    /// </summary>
    PASSPORT = 3
}
