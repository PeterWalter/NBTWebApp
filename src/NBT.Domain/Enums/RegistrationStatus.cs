namespace NBT.Domain.Enums;

/// <summary>
/// Represents the status of a student registration.
/// </summary>
public enum RegistrationStatus
{
    /// <summary>
    /// Registration has been submitted but not yet confirmed.
    /// </summary>
    Pending = 0,

    /// <summary>
    /// Registration has been confirmed and is active.
    /// </summary>
    Confirmed = 1,

    /// <summary>
    /// Registration has been cancelled by the student or administrator.
    /// </summary>
    Cancelled = 2,

    /// <summary>
    /// Student did not attend the test session.
    /// </summary>
    NoShow = 3,

    /// <summary>
    /// Student has completed the test.
    /// </summary>
    Completed = 4
}
