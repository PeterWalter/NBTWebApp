using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NBT.Application.Venues.DTOs;
using NBT.Application.Venues.Services;

namespace NBT.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class VenuesController : ControllerBase
{
    private readonly IVenueService _venueService;
    private readonly ILogger<VenuesController> _logger;

    public VenuesController(IVenueService venueService, ILogger<VenuesController> logger)
    {
        _venueService = venueService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _venueService.GetAllVenuesAsync();
        if (!result.Succeeded)
            return BadRequest(result.Error);

        return Ok(result.Data);
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActive()
    {
        var result = await _venueService.GetActiveVenuesAsync();
        if (!result.Succeeded)
            return BadRequest(result.Error);

        return Ok(result.Data);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _venueService.GetVenueByIdAsync(id);
        if (!result.Succeeded)
            return NotFound(result.Error);

        return Ok(result.Data);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string searchTerm)
    {
        var result = await _venueService.SearchVenuesAsync(searchTerm);
        if (!result.Succeeded)
            return BadRequest(result.Error);

        return Ok(result.Data);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Staff")]
    public async Task<IActionResult> Create([FromBody] CreateVenueDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _venueService.CreateVenueAsync(dto);
        if (!result.Succeeded)
            return BadRequest(result.Error);

        return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin,Staff")]
    public async Task<IActionResult> Update(Guid id, [FromBody] CreateVenueDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _venueService.UpdateVenueAsync(id, dto);
        if (!result.Succeeded)
            return BadRequest(result.Error);

        return Ok(result.Data);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _venueService.DeleteVenueAsync(id);
        if (!result.Succeeded)
            return BadRequest(result.Error);

        return NoContent();
    }
}
