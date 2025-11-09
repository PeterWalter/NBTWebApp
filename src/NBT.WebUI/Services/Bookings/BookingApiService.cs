using System.Net.Http.Json;
using NBT.Application.Bookings.DTOs;
using NBT.Application.Common.Models;

namespace NBT.WebUI.Services.Bookings;

/// <summary>
/// API service for booking operations
/// </summary>
public class BookingApiService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<BookingApiService> _logger;

    public BookingApiService(HttpClient httpClient, ILogger<BookingApiService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<PaginatedList<BookingDto>?> GetAllBookingsAsync(
        int pageNumber = 1,
        int pageSize = 10,
        string? studentName = null,
        string? nbtNumber = null,
        string? status = null,
        DateTime? sessionDateFrom = null,
        DateTime? sessionDateTo = null)
    {
        try
        {
            var queryParams = new List<string>
            {
                $"pageNumber={pageNumber}",
                $"pageSize={pageSize}"
            };

            if (!string.IsNullOrWhiteSpace(studentName))
                queryParams.Add($"studentName={Uri.EscapeDataString(studentName)}");
            if (!string.IsNullOrWhiteSpace(nbtNumber))
                queryParams.Add($"nbtNumber={Uri.EscapeDataString(nbtNumber)}");
            if (!string.IsNullOrWhiteSpace(status))
                queryParams.Add($"status={Uri.EscapeDataString(status)}");
            if (sessionDateFrom.HasValue)
                queryParams.Add($"sessionDateFrom={sessionDateFrom.Value:yyyy-MM-dd}");
            if (sessionDateTo.HasValue)
                queryParams.Add($"sessionDateTo={sessionDateTo.Value:yyyy-MM-dd}");

            var query = string.Join("&", queryParams);
            return await _httpClient.GetFromJsonAsync<PaginatedList<BookingDto>>($"api/bookings?{query}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting bookings");
            return null;
        }
    }

    public async Task<BookingDto?> GetBookingByIdAsync(Guid id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<BookingDto>($"api/bookings/{id}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting booking {BookingId}", id);
            return null;
        }
    }

    public async Task<List<BookingDto>?> GetBookingsByStudentIdAsync(Guid studentId)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<List<BookingDto>>($"api/bookings/student/{studentId}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting bookings for student {StudentId}", studentId);
            return null;
        }
    }

    public async Task<(bool Success, string Message, BookingDto? Booking)> CreateBookingAsync(CreateBookingRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/bookings", request);
            if (response.IsSuccessStatusCode)
            {
                var booking = await response.Content.ReadFromJsonAsync<BookingDto>();
                return (true, "Booking created successfully", booking);
            }

            var error = await response.Content.ReadAsStringAsync();
            return (false, error, null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating booking");
            return (false, "An error occurred while creating the booking", null);
        }
    }

    public async Task<(bool Success, string Message, BookingDto? Booking)> UpdateBookingAsync(UpdateBookingRequest request)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"api/bookings/{request.Id}", request);
            if (response.IsSuccessStatusCode)
            {
                var booking = await response.Content.ReadFromJsonAsync<BookingDto>();
                return (true, "Booking updated successfully", booking);
            }

            var error = await response.Content.ReadAsStringAsync();
            return (false, error, null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating booking");
            return (false, "An error occurred while updating the booking", null);
        }
    }

    public async Task<(bool Success, string Message)> CancelBookingAsync(CancelBookingRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"api/bookings/{request.BookingId}/cancel", request);
            if (response.IsSuccessStatusCode)
            {
                return (true, "Booking cancelled successfully");
            }

            var error = await response.Content.ReadAsStringAsync();
            return (false, error);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cancelling booking");
            return (false, "An error occurred while cancelling the booking");
        }
    }

    public async Task<BookingValidationResult?> ValidateBookingAsync(Guid studentId, Guid sessionId)
    {
        try
        {
            var request = new CreateBookingRequest
            {
                StudentId = studentId,
                TestSessionId = sessionId,
                TestTypesSelected = new List<string>()
            };

            var response = await _httpClient.PostAsJsonAsync("api/bookings/validate", request);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<BookingValidationResult>();
            }

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating booking");
            return null;
        }
    }
}
