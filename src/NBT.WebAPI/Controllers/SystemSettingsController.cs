using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NBT.Application.Common.Interfaces;

namespace NBT.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SystemSettingsController : ControllerBase
{
    private readonly IApplicationDbContext _context;

    public SystemSettingsController(IApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("{key}")]
    public async Task<ActionResult<string>> GetSetting(string key)
    {
        var setting = await _context.SystemSettings
            .Where(s => s.Key == key && s.IsActive)
            .FirstOrDefaultAsync();

        if (setting == null)
        {
            return NotFound();
        }

        return Ok(setting.Value);
    }

    [HttpGet("category/{category}")]
    public async Task<ActionResult<Dictionary<string, string>>> GetSettingsByCategory(string category)
    {
        var settingsList = await _context.SystemSettings
            .Where(s => s.Category == category && s.IsActive)
            .ToListAsync();
        
        var settings = settingsList.ToDictionary(s => s.Key, s => s.Value);

        return Ok(settings);
    }
}
