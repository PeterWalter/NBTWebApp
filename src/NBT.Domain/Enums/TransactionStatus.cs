namespace NBT.Domain.Enums;

/// <summary>
/// Represents the status of a payment transaction.
/// </summary>
public enum TransactionStatus
{
    /// <summary>
    /// Transaction is pending processing.
    /// </summary>
    Pending = 0,

    /// <summary>
    /// Transaction completed successfully.
    /// </summary>
    Success = 1,

    /// <summary>
    /// Transaction failed.
    /// </summary>
    Failed = 2,

    /// <summary>
    /// Transaction was cancelled.
    /// </summary>
    Cancelled = 3,

    /// <summary>
    /// Transaction was refunded.
    /// </summary>
    Refunded = 4
}
