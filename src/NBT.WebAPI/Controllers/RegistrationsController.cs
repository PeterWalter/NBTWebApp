using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NBT.Application.Bookings.Services;
using NBT.Application.Students.DTOs;
using NBT.Application.Students.Services;
using NBT.Domain.Common;

namespace NBT.WebAPI.Controllers;

/// <summary>
/// Controller for managing student registrations and NBT number generation.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class RegistrationsController : ControllerBase
{
    private readonly IStudentService _studentService;
    private readonly IBookingValidationService _bookingValidationService;
    private readonly ILuhnValidator _luhnValidator;
    private readonly ILogger<RegistrationsController> _logger;

    public RegistrationsController(
        IStudentService studentService,
        IBookingValidationService bookingValidationService,
        ILuhnValidator luhnValidator,
        ILogger<RegistrationsController> logger)
    {
        _studentService = studentService;
        _bookingValidationService = bookingValidationService;
        _luhnValidator = luhnValidator;
        _logger = logger;
    }

    /// <summary>
    /// Start a new registration (draft mode).
    /// </summary>
    [HttpPost("start")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(StudentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<StudentDto>> StartRegistration(
        [FromBody] CreateStudentDto request,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Starting registration for ID: {IDNumber}", request.IDNumber);
            
            var result = await _studentService.CreateAsync(request, cancellationToken);
            
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error starting registration");
            return BadRequest(new { Message = "Failed to start registration", Error = ex.Message });
        }
    }

    /// <summary>
    /// Generate NBT number for a student.
    /// </summary>
    [HttpPost("generate-nbt-number")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<string>> GenerateNBTNumber(
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Generating NBT number");
            
            var nbtNumber = await _studentService.GenerateNBTNumberAsync(cancellationToken);
            
            return Ok(new { NBTNumber = nbtNumber });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating NBT number");
            return BadRequest(new { Message = "Failed to generate NBT number", Error = ex.Message });
        }
    }

    /// <summary>
    /// Validate if student can book a new test.
    /// </summary>
    [HttpPost("validate-booking")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> ValidateBooking(
        [FromBody] ValidateBookingRequest request,
        CancellationToken cancellationToken)
    {
        var validation = await _bookingValidationService.ValidateNewBookingAsync(
            request.StudentId,
            request.SessionDate,
            cancellationToken);

        return Ok(validation);
    }

    /// <summary>
    /// Get all registrations (Staff/Admin/SuperUser only).
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Staff,Admin,SuperUser")]
    [ProducesResponseType(typeof(IEnumerable<StudentDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<StudentDto>>> GetRegistrations(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50,
        CancellationToken cancellationToken = default)
    {
        var result = await _studentService.GetAllAsync(page, pageSize, cancellationToken);
        
        return Ok(result);
    }

    /// <summary>
    /// Get registration by ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    [Authorize(Roles = "Staff,Admin,SuperUser")]
    [ProducesResponseType(typeof(StudentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<StudentDto>> GetRegistration(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _studentService.GetByIdAsync(id, cancellationToken);
        
        if (result == null)
        {
            return NotFound(new { Message = "Student not found" });
        }
        
        return Ok(result);
    }

    /// <summary>
    /// Update registration.
    /// </summary>
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin,SuperUser")]
    [ProducesResponseType(typeof(StudentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<StudentDto>> UpdateRegistration(
        Guid id,
        [FromBody] UpdateStudentDto request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await _studentService.UpdateAsync(request, cancellationToken);
            
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating student");
            return NotFound(new { Message = ex.Message });
        }
    }

    /// <summary>
    /// Delete registration (SuperUser only).
    /// </summary>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "SuperUser")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRegistration(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _studentService.DeleteAsync(id, cancellationToken);
        
        if (!result)
        {
            return NotFound(new { Message = "Student not found" });
        }
        
        return NoContent();
    }
}

/// <summary>
/// Request model for validating booking eligibility.
/// </summary>
public record ValidateBookingRequest(Guid StudentId, DateTime SessionDate);
