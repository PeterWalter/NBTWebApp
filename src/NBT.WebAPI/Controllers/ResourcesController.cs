using Microsoft.AspNetCore.Mvc;
using NBT.Application.Resources.DTOs;
using NBT.Application.Resources.Interfaces;

namespace NBT.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ResourcesController : ControllerBase
{
    private readonly IDownloadableResourceService _service;
    private readonly ILogger<ResourcesController> _logger;

    public ResourcesController(IDownloadableResourceService service, ILogger<ResourcesController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DownloadableResourceDto>>> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            var resources = await _service.GetAllAsync(cancellationToken);
            return Ok(resources);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving resources");
            return StatusCode(500, "An error occurred while retrieving resources");
        }
    }

    [HttpGet("category/{category}")]
    public async Task<ActionResult<IEnumerable<DownloadableResourceDto>>> GetByCategory(string category, CancellationToken cancellationToken)
    {
        try
        {
            var resources = await _service.GetByCategoryAsync(category, cancellationToken);
            return Ok(resources);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving resources by category {Category}", category);
            return StatusCode(500, "An error occurred while retrieving resources");
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<DownloadableResourceDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var resource = await _service.GetByIdAsync(id, cancellationToken);
            if (resource == null)
                return NotFound();

            return Ok(resource);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving resource {Id}", id);
            return StatusCode(500, "An error occurred while retrieving the resource");
        }
    }

    [HttpPost]
    public async Task<ActionResult<DownloadableResourceDto>> Create([FromBody] CreateDownloadableResourceDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var created = await _service.CreateAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating resource");
            return StatusCode(500, "An error occurred while creating the resource");
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<DownloadableResourceDto>> Update(Guid id, [FromBody] UpdateDownloadableResourceDto dto, CancellationToken cancellationToken)
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
            _logger.LogError(ex, "Error updating resource {Id}", id);
            return StatusCode(500, "An error occurred while updating the resource");
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
            _logger.LogError(ex, "Error deleting resource {Id}", id);
            return StatusCode(500, "An error occurred while deleting the resource");
        }
    }

    [HttpPost("{id:guid}/download")]
    public async Task<ActionResult> IncrementDownloadCount(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await _service.IncrementDownloadCountAsync(id, cancellationToken);
            return Ok();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error incrementing download count for resource {Id}", id);
            return StatusCode(500, "An error occurred");
        }
    }
}
