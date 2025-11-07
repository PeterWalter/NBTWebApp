using Microsoft.AspNetCore.Mvc;
using NBT.Application.ContentPages.DTOs;
using NBT.Application.ContentPages.Interfaces;

namespace NBT.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContentPagesController : ControllerBase
{
    private readonly IContentPageService _service;
    private readonly ILogger<ContentPagesController> _logger;

    public ContentPagesController(IContentPageService service, ILogger<ContentPagesController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ContentPageDto>>> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            var pages = await _service.GetAllAsync(cancellationToken);
            return Ok(pages);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving content pages");
            return StatusCode(500, "An error occurred while retrieving content pages");
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ContentPageDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var page = await _service.GetByIdAsync(id, cancellationToken);
            if (page == null)
                return NotFound();

            return Ok(page);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving content page {Id}", id);
            return StatusCode(500, "An error occurred while retrieving the content page");
        }
    }

    [HttpGet("slug/{slug}")]
    public async Task<ActionResult<ContentPageDto>> GetBySlug(string slug, CancellationToken cancellationToken)
    {
        try
        {
            var page = await _service.GetBySlugAsync(slug, cancellationToken);
            if (page == null)
                return NotFound();

            return Ok(page);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving content page by slug {Slug}", slug);
            return StatusCode(500, "An error occurred while retrieving the content page");
        }
    }

    [HttpPost]
    public async Task<ActionResult<ContentPageDto>> Create([FromBody] CreateContentPageDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var created = await _service.CreateAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating content page");
            return StatusCode(500, "An error occurred while creating the content page");
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ContentPageDto>> Update(Guid id, [FromBody] UpdateContentPageDto dto, CancellationToken cancellationToken)
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
            _logger.LogError(ex, "Error updating content page {Id}", id);
            return StatusCode(500, "An error occurred while updating the content page");
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
            _logger.LogError(ex, "Error deleting content page {Id}", id);
            return StatusCode(500, "An error occurred while deleting the content page");
        }
    }
}
