using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NBT.Application.Bookings.DTOs;
using NBT.Application.Bookings.Services;
using NBT.Application.Common.Models;
using NBT.Domain.Entities;
using NBT.Domain.Enums;
using NBT.Infrastructure.Persistence;
using System.Text.Json;

namespace NBT.Infrastructure.Services.Bookings;

/// <summary>
/// Implementation of booking/registration service
/// </summary>
public class BookingService : IBookingService
{
    private readonly ApplicationDbContext _context;
    private readonly IBookingValidationService _validationService;
    private readonly ILogger<BookingService> _logger;

    public BookingService(
        ApplicationDbContext context,
        IBookingValidationService validationService,
        ILogger<BookingService> logger)
    {
        _context = context;
        _validationService = validationService;
        _logger = logger;
    }

    public async Task<PaginatedList<BookingDto>> GetAllBookingsAsync(
        int pageNumber = 1,
        int pageSize = 10,
        string? studentName = null,
        string? nbtNumber = null,
        string? status = null,
        DateTime? sessionDateFrom = null,
        DateTime? sessionDateTo = null)
    {
        var query = _context.Registrations
            .Include(r => r.Student)
            .Include(r => r.TestSession)
                .ThenInclude(ts => ts.Venue)
            .Include(r => r.Payment)
            .AsQueryable();

        // Apply filters
        if (!string.IsNullOrWhiteSpace(studentName))
        {
            query = query.Where(r => 
                r.Student.FirstName.Contains(studentName) || 
                r.Student.LastName.Contains(studentName));
        }

        if (!string.IsNullOrWhiteSpace(nbtNumber))
        {
            query = query.Where(r => r.Student.NBTNumber == nbtNumber);
        }

        if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<RegistrationStatus>(status, out var statusEnum))
        {
            query = query.Where(r => r.Status == statusEnum);
        }

        if (sessionDateFrom.HasValue)
        {
            query = query.Where(r => r.TestSession.SessionDate >= sessionDateFrom.Value);
        }

        if (sessionDateTo.HasValue)
        {
            query = query.Where(r => r.TestSession.SessionDate <= sessionDateTo.Value);
        }

        var totalCount = await query.CountAsync();
        var items = await query
            .OrderByDescending(r => r.RegistrationDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(r => MapToDto(r))
            .ToListAsync();

        return new PaginatedList<BookingDto>(items, totalCount, pageNumber, pageSize);
    }

    public async Task<BookingDto?> GetBookingByIdAsync(Guid id)
    {
        var registration = await _context.Registrations
            .Include(r => r.Student)
            .Include(r => r.TestSession)
                .ThenInclude(ts => ts.Venue)
            .Include(r => r.Payment)
            .FirstOrDefaultAsync(r => r.Id == id);

        return registration == null ? null : MapToDto(registration);
    }

    public async Task<List<BookingDto>> GetBookingsByStudentIdAsync(Guid studentId)
    {
        var registrations = await _context.Registrations
            .Include(r => r.Student)
            .Include(r => r.TestSession)
                .ThenInclude(ts => ts.Venue)
            .Include(r => r.Payment)
            .Where(r => r.StudentId == studentId)
            .OrderByDescending(r => r.RegistrationDate)
            .ToListAsync();

        return registrations.Select(MapToDto).ToList();
    }

    public async Task<List<BookingDto>> GetBookingsBySessionIdAsync(Guid sessionId)
    {
        var registrations = await _context.Registrations
            .Include(r => r.Student)
            .Include(r => r.TestSession)
                .ThenInclude(ts => ts.Venue)
            .Include(r => r.Payment)
            .Where(r => r.TestSessionId == sessionId)
            .OrderBy(r => r.Student.LastName)
            .ThenBy(r => r.Student.FirstName)
            .ToListAsync();

        return registrations.Select(MapToDto).ToList();
    }

    public async Task<(bool Success, string Message, BookingDto? Booking)> CreateBookingAsync(CreateBookingRequest request)
    {
        try
        {
            // Validate the booking
            var validation = await ValidateBookingAsync(request.StudentId, request.TestSessionId);
            if (!validation.IsValid)
            {
                return (false, validation.ErrorMessage ?? "Validation failed", null);
            }

            // Generate registration number
            var year = DateTime.UtcNow.Year;
            var lastReg = await _context.Registrations
                .Where(r => r.RegistrationNumber.StartsWith($"REG-{year}-"))
                .OrderByDescending(r => r.RegistrationNumber)
                .FirstOrDefaultAsync();

            int nextSequence = 1;
            if (lastReg != null)
            {
                var parts = lastReg.RegistrationNumber.Split('-');
                if (parts.Length == 3 && int.TryParse(parts[2], out var seq))
                {
                    nextSequence = seq + 1;
                }
            }

            var registrationNumber = $"REG-{year}-{nextSequence:D6}";

            // Create registration
            var registration = new Registration
            {
                Id = Guid.NewGuid(),
                RegistrationNumber = registrationNumber,
                StudentId = request.StudentId,
                TestSessionId = request.TestSessionId,
                Status = RegistrationStatus.Pending,
                TestTypesSelected = JsonSerializer.Serialize(request.TestTypesSelected),
                IsRemoteWriter = request.IsRemoteWriter,
                RemoteLocation = request.RemoteLocation,
                SpecialSessionType = request.SpecialSessionType,
                RegistrationDate = DateTime.UtcNow
            };

            _context.Registrations.Add(registration);

            // Update session capacity
            var session = await _context.TestSessions.FindAsync(request.TestSessionId);
            if (session != null)
            {
                session.CurrentRegistrations++;
            }

            await _context.SaveChangesAsync();

            _logger.LogInformation("Booking created: {RegistrationNumber} for Student {StudentId}", 
                registrationNumber, request.StudentId);

            var booking = await GetBookingByIdAsync(registration.Id);
            return (true, "Booking created successfully", booking);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating booking for Student {StudentId}", request.StudentId);
            return (false, "An error occurred while creating the booking", null);
        }
    }

    public async Task<(bool Success, string Message, BookingDto? Booking)> UpdateBookingAsync(UpdateBookingRequest request)
    {
        try
        {
            var registration = await _context.Registrations.FindAsync(request.Id);
            if (registration == null)
            {
                return (false, "Booking not found", null);
            }

            if (registration.Status == RegistrationStatus.Confirmed || 
                registration.Status == RegistrationStatus.Completed)
            {
                return (false, "Cannot update confirmed or completed bookings", null);
            }

            // Check if session date has passed
            var session = await _context.TestSessions.FindAsync(registration.TestSessionId);
            if (session != null && session.SessionDate < DateTime.Today)
            {
                return (false, "Cannot update bookings for past sessions", null);
            }

            // Update fields if provided
            if (request.TestSessionId.HasValue && request.TestSessionId.Value != registration.TestSessionId)
            {
                // Validate new session
                var validation = await ValidateBookingAsync(registration.StudentId, request.TestSessionId.Value);
                if (!validation.IsValid)
                {
                    return (false, validation.ErrorMessage ?? "Validation failed", null);
                }

                // Update session capacities
                var oldSession = await _context.TestSessions.FindAsync(registration.TestSessionId);
                if (oldSession != null)
                {
                    oldSession.CurrentRegistrations--;
                }

                var newSession = await _context.TestSessions.FindAsync(request.TestSessionId.Value);
                if (newSession != null)
                {
                    newSession.CurrentRegistrations++;
                }

                registration.TestSessionId = request.TestSessionId.Value;
            }

            if (request.TestTypesSelected != null)
            {
                registration.TestTypesSelected = JsonSerializer.Serialize(request.TestTypesSelected);
            }

            if (request.IsRemoteWriter.HasValue)
            {
                registration.IsRemoteWriter = request.IsRemoteWriter.Value;
            }

            if (request.RemoteLocation != null)
            {
                registration.RemoteLocation = request.RemoteLocation;
            }

            if (request.SpecialSessionType != null)
            {
                registration.SpecialSessionType = request.SpecialSessionType;
            }

            await _context.SaveChangesAsync();

            _logger.LogInformation("Booking updated: {RegistrationNumber}", registration.RegistrationNumber);

            var booking = await GetBookingByIdAsync(registration.Id);
            return (true, "Booking updated successfully", booking);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating booking {BookingId}", request.Id);
            return (false, "An error occurred while updating the booking", null);
        }
    }

    public async Task<(bool Success, string Message)> CancelBookingAsync(CancelBookingRequest request)
    {
        try
        {
            var registration = await _context.Registrations
                .Include(r => r.TestSession)
                .Include(r => r.Payment)
                .FirstOrDefaultAsync(r => r.Id == request.BookingId);

            if (registration == null)
            {
                return (false, "Booking not found");
            }

            if (registration.Status == RegistrationStatus.Cancelled)
            {
                return (false, "Booking is already cancelled");
            }

            if (registration.Status == RegistrationStatus.Completed)
            {
                return (false, "Cannot cancel completed bookings");
            }

            // Check if session date is within 7 days
            if (registration.TestSession.SessionDate <= DateTime.Today.AddDays(7))
            {
                return (false, "Cannot cancel bookings less than 7 days before the test date");
            }

            registration.Status = RegistrationStatus.Cancelled;
            registration.CancellationDate = DateTime.UtcNow;
            registration.CancellationReason = request.Reason;

            // Update session capacity
            registration.TestSession.CurrentRegistrations--;

            // If payment was made, mark for refund
            if (registration.Payment != null && registration.Payment.Status == PaymentStatus.Paid)
            {
                registration.Payment.Status = PaymentStatus.Refunded;
                registration.Payment.RefundedDate = DateTime.UtcNow;
                registration.Payment.RefundReason = request.Reason;
            }

            await _context.SaveChangesAsync();

            _logger.LogInformation("Booking cancelled: {RegistrationNumber}", registration.RegistrationNumber);

            return (true, "Booking cancelled successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cancelling booking {BookingId}", request.BookingId);
            return (false, "An error occurred while cancelling the booking");
        }
    }

    public async Task<(bool Success, string Message)> ConfirmBookingAsync(Guid bookingId)
    {
        try
        {
            var registration = await _context.Registrations.FindAsync(bookingId);
            if (registration == null)
            {
                return (false, "Booking not found");
            }

            if (registration.Status == RegistrationStatus.Confirmed)
            {
                return (false, "Booking is already confirmed");
            }

            registration.Status = RegistrationStatus.Confirmed;
            registration.ConfirmationDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Booking confirmed: {RegistrationNumber}", registration.RegistrationNumber);

            return (true, "Booking confirmed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error confirming booking {BookingId}", bookingId);
            return (false, "An error occurred while confirming the booking");
        }
    }

    public async Task<BookingValidationResult> ValidateBookingAsync(Guid studentId, Guid sessionId)
    {
        var session = await _context.TestSessions.FindAsync(sessionId);
        if (session == null)
        {
            return new BookingValidationResult
            {
                IsValid = false,
                ErrorMessage = "Test session not found"
            };
        }

        return await _validationService.ValidateNewBookingAsync(studentId, session.SessionDate);
    }

    private static BookingDto MapToDto(Registration registration)
    {
        return new BookingDto
        {
            Id = registration.Id,
            RegistrationNumber = registration.RegistrationNumber,
            StudentId = registration.StudentId,
            StudentName = $"{registration.Student.FirstName} {registration.Student.LastName}",
            StudentNBTNumber = registration.Student.NBTNumber,
            TestSessionId = registration.TestSessionId,
            SessionName = registration.TestSession.SessionName,
            SessionDate = registration.TestSession.SessionDate,
            VenueName = registration.TestSession.Venue?.VenueName ?? "TBD",
            Status = registration.Status.ToString(),
            TestTypesSelected = string.IsNullOrEmpty(registration.TestTypesSelected) 
                ? new List<string>() 
                : JsonSerializer.Deserialize<List<string>>(registration.TestTypesSelected) ?? new List<string>(),
            IsRemoteWriter = registration.IsRemoteWriter,
            RemoteLocation = registration.RemoteLocation,
            SpecialSessionType = registration.SpecialSessionType,
            RegistrationDate = registration.RegistrationDate,
            ConfirmationDate = registration.ConfirmationDate,
            CancellationDate = registration.CancellationDate,
            CancellationReason = registration.CancellationReason,
            Payment = registration.Payment == null ? null : new PaymentInfoDto
            {
                Id = registration.Payment.Id,
                InvoiceNumber = registration.Payment.InvoiceNumber,
                Amount = registration.Payment.TotalAmount,
                AmountPaid = registration.Payment.AmountPaid,
                Balance = registration.Payment.Balance,
                PaymentMethod = registration.Payment.PaymentMethod,
                Status = registration.Payment.Status.ToString(),
                EasyPayReference = registration.Payment.EasyPayReference,
                PaidDate = registration.Payment.PaidDate
            }
        };
    }
}
