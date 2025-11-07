using Microsoft.AspNetCore.Mvc;
using NBT.Application.Announcements.DTOs;
using NBT.Application.Announcements.Interfaces;

namespace NBT.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnnouncementsController : ControllerBase
{
    private readonly IAnnouncementService _service;
    private readonly ILogger<AnnouncementsController> _logger;

    public AnnouncementsController(IAnnouncementService service, ILogger<AnnouncementsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AnnouncementDto>>> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            var announcements = await _service.GetAllAsync(cancellationToken);
            return Ok(announcements);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving announcements");
            return StatusCode(500, "An error occurred while retrieving announcements");
        }
    }

    [HttpGet("featured")]
    public async Task<ActionResult<IEnumerable<AnnouncementDto>>> GetFeatured(CancellationToken cancellationToken)
    {
        try
        {
            var announcements = await _service.GetFeaturedAsync(cancellationToken);
            return Ok(announcements);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving featured announcements");
            return StatusCode(500, "An error occurred while retrieving featured announcements");
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<AnnouncementDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var announcement = await _service.GetByIdAsync(id, cancellationToken);
            if (announcement == null)
                return NotFound();

            return Ok(announcement);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving announcement {Id}", id);
            return StatusCode(500, "An error occurred while retrieving the announcement");
        }
    }

    [HttpPost]
    public async Task<ActionResult<AnnouncementDto>> Create([FromBody] CreateAnnouncementDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var created = await _service.CreateAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating announcement");
            return StatusCode(500, "An error occurred while creating the announcement");
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<AnnouncementDto>> Update(Guid id, [FromBody] UpdateAnnouncementDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var updated = await _service.UpdateAsync(id, dto, cancellationToken);
            return Ok(updated);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating announcement {Id}", id);
            return StatusCode(500, "An error occurred while updating the announcement");
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await _service.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting announcement {Id}", id);
            return StatusCode(500, "An error occurred while deleting the announcement");
        }
    }
}
