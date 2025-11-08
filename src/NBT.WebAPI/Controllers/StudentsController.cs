using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NBT.Application.Students.DTOs;
using NBT.Application.Students.Services;

namespace NBT.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,Staff,SuperUser")]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;
    private readonly ILogger<StudentsController> _logger;

    public StudentsController(
        IStudentService studentService,
        ILogger<StudentsController> logger)
    {
        _studentService = studentService;
        _logger = logger;
    }

    /// <summary>
    /// Gets all students with pagination.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<StudentDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 50, CancellationToken cancellationToken = default)
    {
        try
        {
            var students = await _studentService.GetAllAsync(page, pageSize, cancellationToken);
            return Ok(students);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving students");
            return StatusCode(500, new { message = "An error occurred while retrieving students." });
        }
    }

    /// <summary>
    /// Gets a student by ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(StudentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var student = await _studentService.GetByIdAsync(id, cancellationToken);
            if (student == null)
            {
                return NotFound(new { message = $"Student with ID {id} not found." });
            }
            return Ok(student);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving student {StudentId}", id);
            return StatusCode(500, new { message = "An error occurred while retrieving the student." });
        }
    }

    /// <summary>
    /// Gets a student by NBT number.
    /// </summary>
    [HttpGet("nbt/{nbtNumber}")]
    [ProducesResponseType(typeof(StudentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByNBTNumber(string nbtNumber, CancellationToken cancellationToken = default)
    {
        try
        {
            var student = await _studentService.GetByNBTNumberAsync(nbtNumber, cancellationToken);
            if (student == null)
            {
                return NotFound(new { message = $"Student with NBT number {nbtNumber} not found." });
            }
            return Ok(student);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving student by NBT number {NBTNumber}", nbtNumber);
            return StatusCode(500, new { message = "An error occurred while retrieving the student." });
        }
    }

    /// <summary>
    /// Gets a student by SA ID number.
    /// </summary>
    [HttpGet("said/{saIdNumber}")]
    [ProducesResponseType(typeof(StudentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBySAIDNumber(string saIdNumber, CancellationToken cancellationToken = default)
    {
        try
        {
            var student = await _studentService.GetBySAIDNumberAsync(saIdNumber, cancellationToken);
            if (student == null)
            {
                return NotFound(new { message = $"Student with SA ID number {saIdNumber} not found." });
            }
            return Ok(student);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving student by SA ID number {SAIDNumber}", saIdNumber);
            return StatusCode(500, new { message = "An error occurred while retrieving the student." });
        }
    }

    /// <summary>
    /// Searches students by name, email, NBT number, or SA ID number.
    /// </summary>
    [HttpGet("search")]
    [ProducesResponseType(typeof(IEnumerable<StudentDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Search([FromQuery] string searchTerm, [FromQuery] int page = 1, [FromQuery] int pageSize = 50, CancellationToken cancellationToken = default)
    {
        try
        {
            var students = await _studentService.SearchAsync(searchTerm, page, pageSize, cancellationToken);
            return Ok(students);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching students with term: {SearchTerm}", searchTerm);
            return StatusCode(500, new { message = "An error occurred while searching students." });
        }
    }

    /// <summary>
    /// Creates a new student and generates an NBT number.
    /// </summary>
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(StudentDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateStudentDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var student = await _studentService.CreateAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = student.Id }, student);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating student");
            return StatusCode(500, new { message = "An error occurred while creating the student." });
        }
    }

    /// <summary>
    /// Checks if an ID number is already registered (for duplicate prevention during registration).
    /// </summary>
    [HttpGet("check-duplicate")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> CheckDuplicate([FromQuery] string idNumber, [FromQuery] string idType, CancellationToken cancellationToken = default)
    {
        try
        {
            var exists = await _studentService.CheckDuplicateAsync(idNumber, idType, cancellationToken);
            return Ok(new { exists });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking duplicate for ID {IDNumber}", idNumber);
            return StatusCode(500, new { message = "An error occurred while checking for duplicates." });
        }
    }

    /// <summary>
    /// Updates an existing student.
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(StudentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateStudentDto dto, CancellationToken cancellationToken = default)
    {
        if (id != dto.Id)
        {
            return BadRequest(new { message = "ID in URL does not match ID in request body." });
        }

        try
        {
            var student = await _studentService.UpdateAsync(dto, cancellationToken);
            return Ok(student);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating student {StudentId}", id);
            return StatusCode(500, new { message = "An error occurred while updating the student." });
        }
    }

    /// <summary>
    /// Soft deletes a student (sets IsActive to false).
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _studentService.DeleteAsync(id, cancellationToken);
            if (!result)
            {
                return NotFound(new { message = $"Student with ID {id} not found." });
            }
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting student {StudentId}", id);
            return StatusCode(500, new { message = "An error occurred while deleting the student." });
        }
    }

    /// <summary>
    /// Validates an NBT number format and uniqueness.
    /// </summary>
    [HttpGet("validate/nbt/{nbtNumber}")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> ValidateNBTNumber(string nbtNumber, CancellationToken cancellationToken = default)
    {
        try
        {
            var isValid = await _studentService.ValidateNBTNumberAsync(nbtNumber, cancellationToken);
            return Ok(new { nbtNumber, isValid });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating NBT number {NBTNumber}", nbtNumber);
            return StatusCode(500, new { message = "An error occurred while validating the NBT number." });
        }
    }

    /// <summary>
    /// Validates a South African ID number.
    /// </summary>
    [HttpGet("validate/said/{saIdNumber}")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> ValidateSAIDNumber(string saIdNumber, CancellationToken cancellationToken = default)
    {
        try
        {
            var isValid = await _studentService.ValidateSAIDNumberAsync(saIdNumber, cancellationToken);
            return Ok(new { saIdNumber, isValid });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating SA ID number {SAIDNumber}", saIdNumber);
            return StatusCode(500, new { message = "An error occurred while validating the SA ID number." });
        }
    }

    /// <summary>
    /// Generates a new NBT number (for preview purposes).
    /// </summary>
    [HttpPost("generate-nbt")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> GenerateNBTNumber(CancellationToken cancellationToken = default)
    {
        try
        {
            var nbtNumber = await _studentService.GenerateNBTNumberAsync(cancellationToken);
            return Ok(new { nbtNumber });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating NBT number");
            return StatusCode(500, new { message = "An error occurred while generating the NBT number." });
        }
    }
}
