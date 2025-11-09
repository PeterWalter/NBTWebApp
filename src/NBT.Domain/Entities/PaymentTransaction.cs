using System.ComponentModel.DataAnnotations;
using NBT.Domain.Common;
using NBT.Domain.Enums;

namespace NBT.Domain.Entities;

/// <summary>
/// Represents an individual payment transaction for installment tracking.
/// Tracks each payment made towards a registration.
/// </summary>
public class PaymentTransaction : BaseEntity
{
    /// <summary>
    /// Gets or sets the ID of the payment this transaction belongs to.
    /// </summary>
    [Required]
    public Guid PaymentId { get; set; }

    /// <summary>
    /// Gets or sets the transaction reference number.
    /// Format: TXN-YYYY-NNNNNN
    /// Example: TXN-2024-000123
    /// </summary>
    [Required]
    [StringLength(20)]
    public string TransactionReference { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date and time of the transaction.
    /// </summary>
    [Required]
    public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the amount of this transaction.
    /// </summary>
    [Required]
    public decimal Amount { get; set; }

    /// <summary>
    /// Gets or sets the payment method used for this transaction.
    /// Values: "EasyPay", "Cash", "EFT", "Card"
    /// </summary>
    [Required]
    [StringLength(50)]
    public string PaymentMethod { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the status of this transaction.
    /// </summary>
    [Required]
    public TransactionStatus Status { get; set; } = TransactionStatus.Pending;

    /// <summary>
    /// Gets or sets the external transaction ID (from payment gateway).
    /// </summary>
    [StringLength(100)]
    public string? ExternalTransactionId { get; set; }

    /// <summary>
    /// Gets or sets additional notes about the transaction.
    /// </summary>
    [StringLength(1000)]
    public string? Notes { get; set; }

    /// <summary>
    /// Gets or sets the name of the user who recorded this transaction.
    /// Applicable for manual payments recorded by staff.
    /// </summary>
    [StringLength(100)]
    public string? RecordedBy { get; set; }

    // Navigation property

    /// <summary>
    /// Gets or sets the payment associated with this transaction.
    /// </summary>
    public virtual Payment Payment { get; set; } = null!;
}
