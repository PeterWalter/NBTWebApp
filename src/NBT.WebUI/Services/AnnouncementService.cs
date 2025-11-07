using NBT.WebUI.Models;
using System.Net.Http.Json;

namespace NBT.WebUI.Services;

public interface IAnnouncementService
{
    Task<List<AnnouncementDto>> GetAllAsync();
    Task<List<AnnouncementDto>> GetFeaturedAsync();
    Task<AnnouncementDto?> GetByIdAsync(Guid id);
    Task<AnnouncementDto?> CreateAnnouncementAsync(CreateAnnouncementDto announcement);
    Task<AnnouncementDto?> UpdateAnnouncementAsync(Guid id, UpdateAnnouncementDto announcement);
    Task<bool> DeleteAnnouncementAsync(Guid id);
    Task<List<AnnouncementDto>> GetAllAnnouncementsAsync();
}

public class AnnouncementService : IAnnouncementService
{
    private readonly HttpClient _httpClient;

    public AnnouncementService(HttpClient httpClient)
    {
        _httpClient = httpClient;
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
            Console.WriteLine($"Error fetching announcements: {ex.Message}");
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
            Console.WriteLine($"Error fetching featured announcements: {ex.Message}");
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
            Console.WriteLine($"Error fetching announcement {id}: {ex.Message}");
            return null;
        }
    }

    public async Task<AnnouncementDto?> CreateAnnouncementAsync(CreateAnnouncementDto announcement)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/announcements", announcement);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<AnnouncementDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating announcement: {ex.Message}");
            throw;
        }
    }

    public async Task<AnnouncementDto?> UpdateAnnouncementAsync(Guid id, UpdateAnnouncementDto announcement)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"api/announcements/{id}", announcement);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<AnnouncementDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating announcement {id}: {ex.Message}");
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
            Console.WriteLine($"Error deleting announcement {id}: {ex.Message}");
            return false;
        }
    }
}
