using NBT.WebUI.Models;
using System.Net.Http.Json;

namespace NBT.WebUI.Services;

public interface IAnnouncementService
{
    Task<List<AnnouncementDto>> GetAllAsync();
    Task<List<AnnouncementDto>> GetFeaturedAsync();
    Task<AnnouncementDto?> GetByIdAsync(Guid id);
}

public class AnnouncementService : IAnnouncementService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<AnnouncementService> _logger;

    public AnnouncementService(IHttpClientFactory httpClientFactory, ILogger<AnnouncementService> logger)
    {
        _httpClient = httpClientFactory.CreateClient("NBT.WebAPI");
        _logger = logger;
    }

    public async Task<List<AnnouncementDto>> GetAllAsync()
    {
        try
        {
            var result = await _httpClient.GetFromJsonAsync<List<AnnouncementDto>>("api/announcements");
            return result ?? new List<AnnouncementDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching announcements");
            return new List<AnnouncementDto>();
        }
    }

    public async Task<List<AnnouncementDto>> GetFeaturedAsync()
    {
        try
        {
            var result = await _httpClient.GetFromJsonAsync<List<AnnouncementDto>>("api/announcements/featured");
            return result ?? new List<AnnouncementDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching featured announcements");
            return new List<AnnouncementDto>();
        }
    }

    public async Task<AnnouncementDto?> GetByIdAsync(Guid id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<AnnouncementDto>($"api/announcements/{id}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching announcement {Id}", id);
            return null;
        }
    }
}
