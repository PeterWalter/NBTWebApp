using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NBT.Application.Bookings.DTOs;
using NBT.Application.Bookings.Services;
using NBT.Domain.Entities;
using NBT.Domain.Enums;
using NBT.Infrastructure.Persistence;

namespace NBT.Infrastructure.Services.Payments;

/// <summary>
/// Implementation of payment service
/// </summary>
public class PaymentService : IPaymentService
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly ILogger<PaymentService> _logger;
    private readonly decimal _testFee;

    public PaymentService(
        ApplicationDbContext context,
        IConfiguration configuration,
        ILogger<PaymentService> logger)
    {
        _context = context;
        _configuration = configuration;
        _logger = logger;
        _testFee = decimal.Parse(configuration["Payment:TestFee"] ?? "280.00");
    }

    public async Task<InitiatePaymentResponse> InitiatePaymentAsync(InitiatePaymentRequest request)
    {
        try
        {
            // Check if registration exists
            var registration = await _context.Registrations
                .Include(r => r.Student)
                .Include(r => r.Payment)
                .FirstOrDefaultAsync(r => r.Id == request.RegistrationId);

            if (registration == null)
            {
                return new InitiatePaymentResponse
                {
                    Success = false,
                    Message = "Registration not found"
                };
            }

            // Check if payment already exists
            if (registration.Payment != null)
            {
                if (registration.Payment.Status == PaymentStatus.Paid)
                {
                    return new InitiatePaymentResponse
                    {
                        Success = false,
                        Message = "Payment has already been made for this registration"
                    };
                }

                // Return existing payment info if still pending
                return new InitiatePaymentResponse
                {
                    Success = true,
                    Message = "Payment already initiated",
                    PaymentId = registration.Payment.Id,
                    InvoiceNumber = registration.Payment.InvoiceNumber,
                    Amount = registration.Payment.TotalAmount,
                    AmountPaid = registration.Payment.AmountPaid,
                    Balance = registration.Payment.Balance,
                    EasyPayReference = registration.Payment.EasyPayReference,
                    PaymentUrl = GenerateEasyPayUrl(registration.Payment.EasyPayReference!)
                };
            }

            // Generate invoice number
            var year = DateTime.UtcNow.Year;
            var lastPayment = await _context.Payments
                .Where(p => p.InvoiceNumber.StartsWith($"INV-{year}-"))
                .OrderByDescending(p => p.InvoiceNumber)
                .FirstOrDefaultAsync();

            int nextSequence = 1;
            if (lastPayment != null)
            {
                var parts = lastPayment.InvoiceNumber.Split('-');
                if (parts.Length == 3 && int.TryParse(parts[2], out var seq))
                {
                    nextSequence = seq + 1;
                }
            }

            var invoiceNumber = $"INV-{year}-{nextSequence:D6}";
            var easyPayReference = $"NBT{year}{nextSequence:D6}";

            // Create payment record
            // TODO: Get pricing from TestPricing table based on intake year and test type
            var currentYear = DateTime.UtcNow.Year;
            var payment = new Payment
            {
                Id = Guid.NewGuid(),
                RegistrationId = request.RegistrationId,
                InvoiceNumber = invoiceNumber,
                TotalAmount = _testFee,
                AmountPaid = 0,
                IntakeYear = currentYear,
                PaymentMethod = request.PaymentMethod,
                Status = PaymentStatus.Pending,
                EasyPayReference = easyPayReference
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Payment initiated: {InvoiceNumber} for Registration {RegistrationId}", 
                invoiceNumber, request.RegistrationId);

            return new InitiatePaymentResponse
            {
                Success = true,
                Message = "Payment initiated successfully",
                PaymentId = payment.Id,
                InvoiceNumber = invoiceNumber,
                Amount = _testFee,
                EasyPayReference = easyPayReference,
                PaymentUrl = GenerateEasyPayUrl(easyPayReference)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error initiating payment for Registration {RegistrationId}", request.RegistrationId);
            return new InitiatePaymentResponse
            {
                Success = false,
                Message = "An error occurred while initiating payment"
            };
        }
    }

    public async Task<(bool Success, string Message)> ProcessPaymentCallbackAsync(
        string reference, string transactionId, string status)
    {
        try
        {
            var payment = await _context.Payments
                .Include(p => p.Registration)
                .FirstOrDefaultAsync(p => p.EasyPayReference == reference);

            if (payment == null)
            {
                _logger.LogWarning("Payment not found for reference: {Reference}", reference);
                return (false, "Payment not found");
            }

            payment.EasyPayTransactionId = transactionId;

            if (status.Equals("Success", StringComparison.OrdinalIgnoreCase) ||
                status.Equals("Paid", StringComparison.OrdinalIgnoreCase))
            {
                payment.Status = PaymentStatus.Paid;
                payment.PaidDate = DateTime.UtcNow;

                // Update registration status
                payment.Registration.Status = RegistrationStatus.Confirmed;
                payment.Registration.ConfirmationDate = DateTime.UtcNow;

                _logger.LogInformation("Payment successful: {InvoiceNumber}", payment.InvoiceNumber);
            }
            else
            {
                payment.Status = PaymentStatus.Failed;
                _logger.LogWarning("Payment failed: {InvoiceNumber} - Status: {Status}", 
                    payment.InvoiceNumber, status);
            }

            await _context.SaveChangesAsync();

            return (true, "Payment callback processed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing payment callback for reference {Reference}", reference);
            return (false, "An error occurred while processing payment callback");
        }
    }

    public async Task<PaymentInfoDto?> GetPaymentByRegistrationIdAsync(Guid registrationId)
    {
        var payment = await _context.Payments
            .FirstOrDefaultAsync(p => p.RegistrationId == registrationId);

        return payment == null ? null : MapToDto(payment);
    }

    public async Task<PaymentInfoDto?> GetPaymentByInvoiceNumberAsync(string invoiceNumber)
    {
        var payment = await _context.Payments
            .FirstOrDefaultAsync(p => p.InvoiceNumber == invoiceNumber);

        return payment == null ? null : MapToDto(payment);
    }

    public async Task<string> CheckPaymentStatusAsync(Guid paymentId)
    {
        var payment = await _context.Payments.FindAsync(paymentId);
        return payment?.Status.ToString() ?? "NotFound";
    }

    private string GenerateEasyPayUrl(string reference)
    {
        var easyPayBaseUrl = _configuration["Payment:EasyPayUrl"] ?? "https://www.easypay.co.za/pay";
        var merchantId = _configuration["Payment:MerchantId"] ?? "NBTTEST";
        
        // In production, this would generate the actual EasyPay payment URL with all required parameters
        return $"{easyPayBaseUrl}?merchant={merchantId}&reference={reference}&amount={_testFee:F2}";
    }

    private static PaymentInfoDto MapToDto(Payment payment)
    {
        return new PaymentInfoDto
        {
            Id = payment.Id,
            InvoiceNumber = payment.InvoiceNumber,
            Amount = payment.TotalAmount,
            AmountPaid = payment.AmountPaid,
            Balance = payment.Balance,
            PaymentMethod = payment.PaymentMethod,
            Status = payment.Status.ToString(),
            EasyPayReference = payment.EasyPayReference,
            PaidDate = payment.PaidDate
        };
    }
}
