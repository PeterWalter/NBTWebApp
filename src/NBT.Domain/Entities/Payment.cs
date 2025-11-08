using System.ComponentModel.DataAnnotations;
using NBT.Domain.Common;
using NBT.Domain.Enums;

namespace NBT.Domain.Entities;

/// <summary>
/// Represents a payment transaction for a registration.
/// Tracks payment method, status, and integration with EasyPay.
/// </summary>
public class Payment : BaseEntity
{
    /// <summary>
    /// Gets or sets the ID of the registration this payment is for.
    /// </summary>
    [Required]
    public Guid RegistrationId { get; set; }

    /// <summary>
    /// Gets or sets the invoice number (format: INV-YYYY-NNNNNN).
    /// Example: INV-2024-000123
    /// </summary>
    [Required]
    [StringLength(20)]
    public string InvoiceNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the payment amount in ZAR.
    /// </summary>
    [Required]
    public decimal Amount { get; set; }

    /// <summary>
    /// Gets or sets the payment method.
    /// Values: "EasyPay", "Cash", "EFT", "Card"
    /// </summary>
    [Required]
    [StringLength(50)]
    public string PaymentMethod { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the current status of the payment.
    /// </summary>
    [Required]
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

    /// <summary>
    /// Gets or sets the EasyPay reference number.
    /// Used for tracking payment with EasyPay gateway.
    /// </summary>
    [StringLength(100)]
    public string? EasyPayReference { get; set; }

    /// <summary>
    /// Gets or sets the EasyPay transaction ID.
    /// Received from EasyPay callback.
    /// </summary>
    [StringLength(100)]
    public string? EasyPayTransactionId { get; set; }

    /// <summary>
    /// Gets or sets the date when payment was received.
    /// </summary>
    public DateTime? PaidDate { get; set; }

    /// <summary>
    /// Gets or sets the date when refund was processed.
    /// </summary>
    public DateTime? RefundedDate { get; set; }

    /// <summary>
    /// Gets or sets the reason for refund if applicable.
    /// </summary>
    [StringLength(500)]
    public string? RefundReason { get; set; }

    /// <summary>
    /// Gets or sets additional notes about the payment.
    /// </summary>
    [StringLength(1000)]
    public string? Notes { get; set; }

    // Navigation property

    /// <summary>
    /// Gets or sets the registration associated with this payment.
    /// </summary>
    public virtual Registration Registration { get; set; } = null!;
}
