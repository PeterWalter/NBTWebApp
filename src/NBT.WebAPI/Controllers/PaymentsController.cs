using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NBT.Application.Bookings.DTOs;
using NBT.Application.Bookings.Services;

namespace NBT.WebAPI.Controllers;

/// <summary>
/// Controller for managing payments
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentService;
    private readonly ILogger<PaymentsController> _logger;

    public PaymentsController(
        IPaymentService paymentService,
        ILogger<PaymentsController> logger)
    {
        _paymentService = paymentService;
        _logger = logger;
    }

    /// <summary>
    /// Initiate a payment for a registration
    /// </summary>
    [HttpPost("initiate")]
    public async Task<IActionResult> InitiatePayment([FromBody] InitiatePaymentRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _paymentService.InitiatePaymentAsync(request);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error initiating payment");
            return StatusCode(500, "An error occurred while initiating payment");
        }
    }

    /// <summary>
    /// Process payment callback from EasyPay (webhook endpoint)
    /// </summary>
    [HttpPost("callback")]
    [AllowAnonymous] // EasyPay needs to call this without authentication
    public async Task<IActionResult> PaymentCallback(
        [FromForm] string reference,
        [FromForm] string transactionId,
        [FromForm] string status)
    {
        try
        {
            // TODO: Add signature verification for security

            var result = await _paymentService.ProcessPaymentCallbackAsync(reference, transactionId, status);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(new { message = result.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing payment callback");
            return StatusCode(500, "An error occurred while processing payment callback");
        }
    }

    /// <summary>
    /// Get payment by registration ID
    /// </summary>
    [HttpGet("registration/{registrationId}")]
    public async Task<IActionResult> GetPaymentByRegistrationId(Guid registrationId)
    {
        try
        {
            var payment = await _paymentService.GetPaymentByRegistrationIdAsync(registrationId);
            if (payment == null)
            {
                return NotFound("Payment not found");
            }

            return Ok(payment);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting payment for registration {RegistrationId}", registrationId);
            return StatusCode(500, "An error occurred while retrieving payment");
        }
    }

    /// <summary>
    /// Get payment by invoice number
    /// </summary>
    [HttpGet("invoice/{invoiceNumber}")]
    [Authorize(Roles = "Admin,Staff")]
    public async Task<IActionResult> GetPaymentByInvoiceNumber(string invoiceNumber)
    {
        try
        {
            var payment = await _paymentService.GetPaymentByInvoiceNumberAsync(invoiceNumber);
            if (payment == null)
            {
                return NotFound("Payment not found");
            }

            return Ok(payment);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting payment by invoice {InvoiceNumber}", invoiceNumber);
            return StatusCode(500, "An error occurred while retrieving payment");
        }
    }

    /// <summary>
    /// Check payment status
    /// </summary>
    [HttpGet("{paymentId}/status")]
    public async Task<IActionResult> CheckPaymentStatus(Guid paymentId)
    {
        try
        {
            var status = await _paymentService.CheckPaymentStatusAsync(paymentId);
            return Ok(new { status });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking payment status {PaymentId}", paymentId);
            return StatusCode(500, "An error occurred while checking payment status");
        }
    }
}
