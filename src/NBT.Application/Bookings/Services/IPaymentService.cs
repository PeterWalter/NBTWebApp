using NBT.Application.Bookings.DTOs;

namespace NBT.Application.Bookings.Services;

/// <summary>
/// Service for managing payments
/// </summary>
public interface IPaymentService
{
    /// <summary>
    /// Initiate a payment for a registration
    /// </summary>
    Task<InitiatePaymentResponse> InitiatePaymentAsync(InitiatePaymentRequest request);

    /// <summary>
    /// Process payment callback from EasyPay
    /// </summary>
    Task<(bool Success, string Message)> ProcessPaymentCallbackAsync(string reference, string transactionId, string status);

    /// <summary>
    /// Get payment by registration ID
    /// </summary>
    Task<PaymentInfoDto?> GetPaymentByRegistrationIdAsync(Guid registrationId);

    /// <summary>
    /// Get payment by invoice number
    /// </summary>
    Task<PaymentInfoDto?> GetPaymentByInvoiceNumberAsync(string invoiceNumber);

    /// <summary>
    /// Check payment status
    /// </summary>
    Task<string> CheckPaymentStatusAsync(Guid paymentId);
}
