using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NBT.Application.Staff.DTOs;
using NBT.Application.Staff.Interfaces;

namespace NBT.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,SuperUser")]
public class StaffController : ControllerBase
{
    private readonly IStaffService _staffService;
    private readonly ILogger<StaffController> _logger;

    public StaffController(IStaffService staffService, ILogger<StaffController> logger)
    {
        _staffService = staffService;
        _logger = logger;
    }

    [HttpPost("search")]
    public async Task<IActionResult> GetAllStaff([FromBody] StaffFilterDto filter)
    {
        var result = await _staffService.GetAllStaffAsync(filter);
        if (!result.Succeeded)
        {
            return BadRequest(new { message = result.Error });
        }

        return Ok(result.Data);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetStaffById(Guid id)
    {
        var result = await _staffService.GetStaffByIdAsync(id);
        if (!result.Succeeded)
        {
            return NotFound(new { message = result.Error });
        }

        return Ok(result.Data);
    }

    [HttpPost]
    public async Task<IActionResult> CreateStaff([FromBody] CreateStaffDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _staffService.CreateStaffAsync(dto);
        if (!result.Succeeded)
        {
            return BadRequest(new { message = result.Error });
        }

        return CreatedAtAction(nameof(GetStaffById), new { id = result.Data!.Id }, result.Data);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStaff(Guid id, [FromBody] UpdateStaffDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _staffService.UpdateStaffAsync(id, dto);
        if (!result.Succeeded)
        {
            return BadRequest(new { message = result.Error });
        }

        return Ok(result.Data);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStaff(Guid id)
    {
        var result = await _staffService.DeleteStaffAsync(id);
        if (!result.Succeeded)
        {
            return BadRequest(new { message = result.Error });
        }

        return Ok(new { message = "Staff member deleted successfully" });
    }

    [HttpPost("{id}/activate")]
    public async Task<IActionResult> ActivateStaff(Guid id)
    {
        var result = await _staffService.ActivateStaffAsync(id);
        if (!result.Succeeded)
        {
            return BadRequest(new { message = result.Error });
        }

        return Ok(new { message = "Staff member activated successfully" });
    }

    [HttpPost("{id}/deactivate")]
    public async Task<IActionResult> DeactivateStaff(Guid id)
    {
        var result = await _staffService.DeactivateStaffAsync(id);
        if (!result.Succeeded)
        {
            return BadRequest(new { message = result.Error });
        }

        return Ok(new { message = "Staff member deactivated successfully" });
    }

    [HttpPost("{id}/change-password")]
    public async Task<IActionResult> ChangePassword(Guid id, [FromBody] ChangePasswordDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _staffService.ChangePasswordAsync(id, dto);
        if (!result.Succeeded)
        {
            return BadRequest(new { message = result.Error });
        }

        return Ok(new { message = "Password changed successfully" });
    }

    [HttpPost("{id}/reset-password")]
    public async Task<IActionResult> ResetPassword(Guid id, [FromBody] string newPassword)
    {
        if (string.IsNullOrWhiteSpace(newPassword))
        {
            return BadRequest(new { message = "New password is required" });
        }

        var result = await _staffService.ResetPasswordAsync(id, newPassword);
        if (!result.Succeeded)
        {
            return BadRequest(new { message = result.Error });
        }

        return Ok(new { message = "Password reset successfully" });
    }

    [HttpGet("by-role/{role}")]
    public async Task<IActionResult> GetStaffByRole(string role)
    {
        var result = await _staffService.GetStaffByRoleAsync(role);
        if (!result.Succeeded)
        {
            return BadRequest(new { message = result.Error });
        }

        return Ok(result.Data);
    }

    [HttpGet("by-institution/{institutionId}")]
    public async Task<IActionResult> GetStaffByInstitution(string institutionId)
    {
        var result = await _staffService.GetStaffByInstitutionAsync(institutionId);
        if (!result.Succeeded)
        {
            return BadRequest(new { message = result.Error });
        }

        return Ok(result.Data);
    }
}
