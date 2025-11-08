namespace NBT.Domain.Enums;

/// <summary>
/// Represents the status of a test session.
/// </summary>
public enum SessionStatus
{
    /// <summary>
    /// Session is open for registrations.
    /// </summary>
    Open = 0,

    /// <summary>
    /// Session has reached capacity.
    /// </summary>
    Full = 1,

    /// <summary>
    /// Session registration is closed.
    /// </summary>
    Closed = 2,

    /// <summary>
    /// Session has been completed.
    /// </summary>
    Completed = 3,

    /// <summary>
    /// Session has been cancelled.
    /// </summary>
    Cancelled = 4
}
