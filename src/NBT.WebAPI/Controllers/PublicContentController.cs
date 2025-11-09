using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NBT.Infrastructure.Persistence;

namespace NBT.WebAPI.Controllers;

[ApiController]
[Route("api/public")]
public class PublicContentController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public PublicContentController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("test-pricing")]
    public async Task<IActionResult> GetCurrentTestPricing()
    {
        try
        {
            var currentYear = DateTime.UtcNow.Year;
            var pricing = await _context.TestPricings
                .Where(p => p.IsActive && p.IntakeYear == currentYear)
                .OrderBy(p => p.TestType)
                .ToListAsync();

            return Ok(pricing);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error retrieving test pricing", error = ex.Message });
        }
    }

    [HttpGet("announcements/featured")]
    public async Task<IActionResult> GetFeaturedAnnouncements()
    {
        try
        {
            var announcements = await _context.Announcements
                .Where(a => a.IsFeatured && a.Status == "Published")
                .OrderByDescending(a => a.PublicationDate)
                .Take(3)
                .ToListAsync();

            return Ok(announcements);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error retrieving announcements", error = ex.Message });
        }
    }
}
