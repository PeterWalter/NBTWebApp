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
    /// Payment has been partially paid (installment).
    /// </summary>
    Partial = 1,

    /// <summary>
    /// Payment has been successfully processed (fully paid).
    /// </summary>
    Paid = 2,

    /// <summary>
    /// Payment processing failed.
    /// </summary>
    Failed = 3,

    /// <summary>
    /// Payment has been fully refunded.
    /// </summary>
    Refunded = 4,

    /// <summary>
    /// Payment has been partially refunded.
    /// </summary>
    PartialRefund = 5
}
