using Microsoft.EntityFrameworkCore;
using NBT.Application.Bookings.DTOs;
using NBT.Application.Bookings.Services;
using NBT.Domain.Enums;
using NBT.Infrastructure.Persistence;

namespace NBT.Infrastructure.Services.Bookings;

/// <summary>
/// Implementation of booking validation service.
/// Enforces all NBT booking business rules.
/// </summary>
public class BookingValidationService : IBookingValidationService
{
    private readonly ApplicationDbContext _context;
    private const int AnnualBookingLimit = 2;
    private const int TestValidityYears = 3;
    private const int IntakeStartMonth = 4; // April
    private const int IntakeStartDay = 1;

    public BookingValidationService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<BookingValidationResult> ValidateNewBookingAsync(
        Guid studentId, 
        DateTime sessionDate,
        CancellationToken cancellationToken = default)
    {
        // Check if booking period is open
        if (!IsWithinBookingPeriod(DateTime.UtcNow))
        {
            return BookingValidationResult.Failure(
                "BOOKING_PERIOD_CLOSED",
                "Booking period has not started yet. Bookings open on April 1 each year.");
        }

        // Check if student has an active booking
        if (await HasActiveBookingAsync(studentId, cancellationToken))
        {
            return BookingValidationResult.Failure(
                "ACTIVE_BOOKING_EXISTS",
                "You already have an active booking. You can only book one test at a time. " +
                "You can book another test after the closing date of your current booking has passed.");
        }

        // Check annual limit
        var sessionYear = sessionDate.Year;
        if (await HasReachedAnnualLimitAsync(studentId, sessionYear, cancellationToken))
        {
            return BookingValidationResult.Failure(
                "ANNUAL_LIMIT_REACHED",
                $"You have reached the maximum limit of {AnnualBookingLimit} tests per year.");
        }

        // Session must be in the future
        if (sessionDate.Date <= DateTime.UtcNow.Date)
        {
            return BookingValidationResult.Failure(
                "INVALID_SESSION_DATE",
                "Test session must be in the future.");
        }

        return BookingValidationResult.Success();
    }

    public async Task<bool> HasReachedAnnualLimitAsync(
        Guid studentId, 
        int year,
        CancellationToken cancellationToken = default)
    {
        var startDate = new DateTime(year, 1, 1);
        var endDate = new DateTime(year, 12, 31);

        var bookingCount = await _context.Registrations
            .Where(r => r.StudentId == studentId)
            .Where(r => r.TestSession.SessionDate >= startDate && r.TestSession.SessionDate <= endDate)
            .Where(r => r.Status == RegistrationStatus.Confirmed || r.Status == RegistrationStatus.Pending)
            .CountAsync(cancellationToken);

        return bookingCount >= AnnualBookingLimit;
    }

    public async Task<bool> HasActiveBookingAsync(
        Guid studentId,
        CancellationToken cancellationToken = default)
    {
        var currentDate = DateTime.UtcNow.Date;

        var activeBooking = await _context.Registrations
            .Include(r => r.TestSession)
            .Where(r => r.StudentId == studentId)
            .Where(r => r.Status == RegistrationStatus.Pending || r.Status == RegistrationStatus.Confirmed)
            .Where(r => r.TestSession.SessionDate >= currentDate)
            .AnyAsync(cancellationToken);

        return activeBooking;
    }

    public async Task<bool> CanModifyBookingAsync(
        Guid registrationId, 
        DateTime currentDate,
        CancellationToken cancellationToken = default)
    {
        var registration = await _context.Registrations
            .Include(r => r.TestSession)
            .FirstOrDefaultAsync(r => r.Id == registrationId, cancellationToken);

        if (registration == null)
            return false;

        // Can modify only if session date hasn't passed and status is Pending/Confirmed
        if (registration.Status != RegistrationStatus.Pending && 
            registration.Status != RegistrationStatus.Confirmed)
            return false;

        // Check if closing date has passed (typically a few days before session date)
        // For now, we'll use 7 days before session as closing date
        var closingDate = registration.TestSession.SessionDate.AddDays(-7);
        
        return currentDate.Date < closingDate.Date;
    }

    public bool IsTestStillValid(DateTime bookingDate, DateTime currentDate)
    {
        var expiryDate = bookingDate.AddYears(TestValidityYears);
        return currentDate.Date <= expiryDate.Date;
    }

    public bool IsWithinBookingPeriod(DateTime currentDate)
    {
        var intakeStartDate = new DateTime(currentDate.Year, IntakeStartMonth, IntakeStartDay);
        
        // If current date is before April 1 of current year, return false
        if (currentDate.Date < intakeStartDate.Date)
            return false;

        return true;
    }
}
