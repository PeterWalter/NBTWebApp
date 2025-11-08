namespace NBT.Domain.Enums;

/// <summary>
/// Represents the status of a payment transaction.
/// </summary>
public enum PaymentStatus
{
    /// <summary>
    /// Payment is pending and awaiting processing.
    /// </summary>
    Pending = 0,

    /// <summary>
    /// Payment has been successfully processed.
    /// </summary>
    Paid = 1,

    /// <summary>
    /// Payment processing failed.
    /// </summary>
    Failed = 2,

    /// <summary>
    /// Payment has been fully refunded.
    /// </summary>
    Refunded = 3,

    /// <summary>
    /// Payment has been partially refunded.
    /// </summary>
    PartialRefund = 4
}
