using NBT.Application.Bookings.DTOs;
using NBT.Application.Common.Models;

namespace NBT.Application.Bookings.Services;

/// <summary>
/// Service for managing student bookings/registrations
/// </summary>
public interface IBookingService
{
    /// <summary>
    /// Get all bookings with optional filtering and pagination
    /// </summary>
    Task<PaginatedList<BookingDto>> GetAllBookingsAsync(
        int pageNumber = 1,
        int pageSize = 10,
        string? studentName = null,
        string? nbtNumber = null,
        string? status = null,
        DateTime? sessionDateFrom = null,
        DateTime? sessionDateTo = null);

    /// <summary>
    /// Get a booking by ID
    /// </summary>
    Task<BookingDto?> GetBookingByIdAsync(Guid id);

    /// <summary>
    /// Get bookings for a specific student
    /// </summary>
    Task<List<BookingDto>> GetBookingsByStudentIdAsync(Guid studentId);

    /// <summary>
    /// Get bookings for a specific test session
    /// </summary>
    Task<List<BookingDto>> GetBookingsBySessionIdAsync(Guid sessionId);

    /// <summary>
    /// Create a new booking
    /// </summary>
    Task<(bool Success, string Message, BookingDto? Booking)> CreateBookingAsync(CreateBookingRequest request);

    /// <summary>
    /// Update an existing booking
    /// </summary>
    Task<(bool Success, string Message, BookingDto? Booking)> UpdateBookingAsync(UpdateBookingRequest request);

    /// <summary>
    /// Cancel a booking
    /// </summary>
    Task<(bool Success, string Message)> CancelBookingAsync(CancelBookingRequest request);

    /// <summary>
    /// Confirm a booking (after payment)
    /// </summary>
    Task<(bool Success, string Message)> ConfirmBookingAsync(Guid bookingId);

    /// <summary>
    /// Check if student can book (validation rules)
    /// </summary>
    Task<BookingValidationResult> ValidateBookingAsync(Guid studentId, Guid sessionId);
}
