using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NBT.Application.Venues.DTOs;
using NBT.Application.Venues.Services;

namespace NBT.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RoomsController : ControllerBase
{
    private readonly IRoomService _roomService;
    private readonly ILogger<RoomsController> _logger;

    public RoomsController(IRoomService roomService, ILogger<RoomsController> logger)
    {
        _roomService = roomService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _roomService.GetAllRoomsAsync();
        if (!result.Succeeded)
            return BadRequest(result.Error);

        return Ok(result.Data);
    }

    [HttpGet("venue/{venueId:guid}")]
    public async Task<IActionResult> GetByVenueId(Guid venueId)
    {
        var result = await _roomService.GetRoomsByVenueIdAsync(venueId);
        if (!result.Succeeded)
            return BadRequest(result.Error);

        return Ok(result.Data);
    }

    [HttpGet("venue/{venueId:guid}/available")]
    public async Task<IActionResult> GetAvailable(Guid venueId)
    {
        var result = await _roomService.GetAvailableRoomsAsync(venueId);
        if (!result.Succeeded)
            return BadRequest(result.Error);

        return Ok(result.Data);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _roomService.GetRoomByIdAsync(id);
        if (!result.Succeeded)
            return NotFound(result.Error);

        return Ok(result.Data);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Staff")]
    public async Task<IActionResult> Create([FromBody] CreateRoomDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _roomService.CreateRoomAsync(dto);
        if (!result.Succeeded)
            return BadRequest(result.Error);

        return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin,Staff")]
    public async Task<IActionResult> Update(Guid id, [FromBody] CreateRoomDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _roomService.UpdateRoomAsync(id, dto);
        if (!result.Succeeded)
            return BadRequest(result.Error);

        return Ok(result.Data);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _roomService.DeleteRoomAsync(id);
        if (!result.Succeeded)
            return BadRequest(result.Error);

        return NoContent();
    }
}
