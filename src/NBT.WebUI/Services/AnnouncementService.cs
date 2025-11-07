using NBT.WebUI.Models;
using System.Net.Http.Json;

namespace NBT.WebUI.Services;

public interface IAnnouncementService
{
    Task<List<AnnouncementDto>> GetAllAsync();
    Task<List<AnnouncementDto>> GetFeaturedAsync();
    Task<AnnouncementDto?> GetByIdAsync(Guid id);
    Task<AnnouncementDto?> CreateAnnouncementAsync(AnnouncementDto announcement);
    Task<AnnouncementDto?> UpdateAnnouncementAsync(Guid id, AnnouncementDto announcement);
    Task<bool> DeleteAnnouncementAsync(Guid id);
    Task<List<AnnouncementDto>> GetAllAnnouncementsAsync(); // Alias for consistency
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

    public Task<List<AnnouncementDto>> GetAllAnnouncementsAsync() => GetAllAsync();

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

    public async Task<AnnouncementDto?> CreateAnnouncementAsync(AnnouncementDto announcement)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/announcements", announcement);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<AnnouncementDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating announcement");
            throw;
        }
    }

    public async Task<AnnouncementDto?> UpdateAnnouncementAsync(Guid id, AnnouncementDto announcement)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"api/announcements/{id}", announcement);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<AnnouncementDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating announcement {Id}", id);
            throw;
        }
    }

    public async Task<bool> DeleteAnnouncementAsync(Guid id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/announcements/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting announcement {Id}", id);
            return false;
        }
    }
}
