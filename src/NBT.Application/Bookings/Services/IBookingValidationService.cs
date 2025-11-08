using NBT.Application.Bookings.DTOs;

namespace NBT.Application.Bookings.Services;

/// <summary>
/// Service for validating booking business rules.
/// Enforces: one active booking, 2 tests per year, 3-year validity, capacity checks.
/// </summary>
public interface IBookingValidationService
{
    /// <summary>
    /// Validates if student can book a new test.
    /// Checks: active booking, annual limit, intake period.
    /// </summary>
    /// <param name="studentId">Student ID</param>
    /// <param name="sessionDate">Requested session date</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Validation result with error details if invalid</returns>
    Task<BookingValidationResult> ValidateNewBookingAsync(
        Guid studentId, 
        DateTime sessionDate,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Checks if student has reached annual booking limit (2 tests per year).
    /// </summary>
    /// <param name="studentId">Student ID</param>
    /// <param name="year">Year to check</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if limit reached, false otherwise</returns>
    Task<bool> HasReachedAnnualLimitAsync(
        Guid studentId, 
        int year,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Checks if student has an active booking.
    /// Active booking = registration with status Pending or Confirmed and closing date not passed.
    /// </summary>
    /// <param name="studentId">Student ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if active booking exists, false otherwise</returns>
    Task<bool> HasActiveBookingAsync(
        Guid studentId,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Validates if booking can be modified (before closing date).
    /// </summary>
    /// <param name="registrationId">Registration ID</param>
    /// <param name="currentDate">Current date</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if can modify, false otherwise</returns>
    Task<bool> CanModifyBookingAsync(
        Guid registrationId, 
        DateTime currentDate,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Checks if test result is still valid (3 years from booking date).
    /// </summary>
    /// <param name="bookingDate">Original booking date</param>
    /// <param name="currentDate">Current date</param>
    /// <returns>True if valid, false if expired</returns>
    bool IsTestStillValid(DateTime bookingDate, DateTime currentDate);
    
    /// <summary>
    /// Checks if booking period is open (after April 1 intake start date).
    /// </summary>
    /// <param name="currentDate">Current date</param>
    /// <returns>True if within booking period, false otherwise</returns>
    bool IsWithinBookingPeriod(DateTime currentDate);
}
