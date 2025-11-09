using System.Net.Http.Json;
using NBT.Application.Bookings.DTOs;

namespace NBT.WebUI.Services.Bookings;

/// <summary>
/// API service for payment operations
/// </summary>
public class PaymentApiService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<PaymentApiService> _logger;

    public PaymentApiService(HttpClient httpClient, ILogger<PaymentApiService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<InitiatePaymentResponse?> InitiatePaymentAsync(InitiatePaymentRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/payments/initiate", request);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<InitiatePaymentResponse>();
            }

            _logger.LogWarning("Failed to initiate payment: {StatusCode}", response.StatusCode);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error initiating payment");
            return null;
        }
    }

    public async Task<PaymentInfoDto?> GetPaymentByRegistrationIdAsync(Guid registrationId)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<PaymentInfoDto>($"api/payments/registration/{registrationId}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting payment for registration {RegistrationId}", registrationId);
            return null;
        }
    }

    public async Task<string?> CheckPaymentStatusAsync(Guid paymentId)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<Dictionary<string, string>>($"api/payments/{paymentId}/status");
            return response?["status"];
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking payment status {PaymentId}", paymentId);
            return null;
        }
    }
}
