using System.ComponentModel.DataAnnotations;

namespace NBT.Application.Bookings.DTOs;

/// <summary>
/// Request to initiate payment for a booking
/// </summary>
public class InitiatePaymentRequest
{
    [Required]
    public Guid RegistrationId { get; set; }

    [Required]
    [StringLength(50)]
    public string PaymentMethod { get; set; } = "EasyPay";

    public string? ReturnUrl { get; set; }
    public string? CancelUrl { get; set; }
}

/// <summary>
/// Response after initiating payment
/// </summary>
public class InitiatePaymentResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public Guid PaymentId { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public decimal AmountPaid { get; set; }
    public decimal Balance { get; set; }
    public string? PaymentUrl { get; set; }
    public string? EasyPayReference { get; set; }
}
