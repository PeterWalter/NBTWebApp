using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NBT.Application.Bookings.DTOs;
using NBT.Application.Bookings.Services;

namespace NBT.WebAPI.Controllers;

/// <summary>
/// Controller for managing bookings/registrations
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;
    private readonly ILogger<BookingsController> _logger;

    public BookingsController(
        IBookingService bookingService,
        ILogger<BookingsController> logger)
    {
        _bookingService = bookingService;
        _logger = logger;
    }

    /// <summary>
    /// Get all bookings with filtering and pagination
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Admin,Staff")]
    public async Task<IActionResult> GetAllBookings(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? studentName = null,
        [FromQuery] string? nbtNumber = null,
        [FromQuery] string? status = null,
        [FromQuery] DateTime? sessionDateFrom = null,
        [FromQuery] DateTime? sessionDateTo = null)
    {
        try
        {
            var result = await _bookingService.GetAllBookingsAsync(
                pageNumber, pageSize, studentName, nbtNumber, status, sessionDateFrom, sessionDateTo);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting bookings");
            return StatusCode(500, "An error occurred while retrieving bookings");
        }
    }

    /// <summary>
    /// Get a booking by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBookingById(Guid id)
    {
        try
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);
            if (booking == null)
            {
                return NotFound("Booking not found");
            }

            // Students can only view their own bookings
            if (!User.IsInRole("Admin") && !User.IsInRole("Staff"))
            {
                var userEmail = User.Identity?.Name;
                // TODO: Add check to ensure student owns this booking
            }

            return Ok(booking);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting booking {BookingId}", id);
            return StatusCode(500, "An error occurred while retrieving the booking");
        }
    }

    /// <summary>
    /// Get bookings for a specific student
    /// </summary>
    [HttpGet("student/{studentId}")]
    public async Task<IActionResult> GetBookingsByStudentId(Guid studentId)
    {
        try
        {
            // Students can only view their own bookings
            if (!User.IsInRole("Admin") && !User.IsInRole("Staff"))
            {
                // TODO: Verify the student ID matches the current user
            }

            var bookings = await _bookingService.GetBookingsByStudentIdAsync(studentId);
            return Ok(bookings);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting bookings for student {StudentId}", studentId);
            return StatusCode(500, "An error occurred while retrieving bookings");
        }
    }

    /// <summary>
    /// Get bookings for a specific test session
    /// </summary>
    [HttpGet("session/{sessionId}")]
    [Authorize(Roles = "Admin,Staff")]
    public async Task<IActionResult> GetBookingsBySessionId(Guid sessionId)
    {
        try
        {
            var bookings = await _bookingService.GetBookingsBySessionIdAsync(sessionId);
            return Ok(bookings);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting bookings for session {SessionId}", sessionId);
            return StatusCode(500, "An error occurred while retrieving bookings");
        }
    }

    /// <summary>
    /// Create a new booking
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateBooking([FromBody] CreateBookingRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _bookingService.CreateBookingAsync(request);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return CreatedAtAction(nameof(GetBookingById), new { id = result.Booking!.Id }, result.Booking);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating booking");
            return StatusCode(500, "An error occurred while creating the booking");
        }
    }

    /// <summary>
    /// Update an existing booking
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBooking(Guid id, [FromBody] UpdateBookingRequest request)
    {
        try
        {
            if (id != request.Id)
            {
                return BadRequest("ID mismatch");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _bookingService.UpdateBookingAsync(request);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Booking);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating booking {BookingId}", id);
            return StatusCode(500, "An error occurred while updating the booking");
        }
    }

    /// <summary>
    /// Cancel a booking
    /// </summary>
    [HttpPost("{id}/cancel")]
    public async Task<IActionResult> CancelBooking(Guid id, [FromBody] CancelBookingRequest request)
    {
        try
        {
            if (id != request.BookingId)
            {
                return BadRequest("ID mismatch");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _bookingService.CancelBookingAsync(request);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(new { message = result.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cancelling booking {BookingId}", id);
            return StatusCode(500, "An error occurred while cancelling the booking");
        }
    }

    /// <summary>
    /// Validate if a booking can be created
    /// </summary>
    [HttpPost("validate")]
    public async Task<IActionResult> ValidateBooking([FromBody] CreateBookingRequest request)
    {
        try
        {
            var result = await _bookingService.ValidateBookingAsync(request.StudentId, request.TestSessionId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating booking");
            return StatusCode(500, "An error occurred while validating the booking");
        }
    }
}
