using NBT.WebUI.Models;
using System.Net.Http.Json;

namespace NBT.WebUI.Services;

public interface IContentPageService
{
    Task<List<ContentPageDto>> GetAllAsync();
    Task<ContentPageDto?> GetByIdAsync(Guid id);
    Task<ContentPageDto?> GetBySlugAsync(string slug);
}

public class ContentPageService : IContentPageService
{
    private readonly HttpClient _httpClient;

    public ContentPageService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<ContentPageDto>> GetAllAsync()
    {
        try
        {
            var result = await _httpClient.GetFromJsonAsync<List<ContentPageDto>>("api/contentpages");
            return result ?? new List<ContentPageDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching content pages: {ex.Message}");
            return new List<ContentPageDto>();
        }
    }

    public async Task<ContentPageDto?> GetByIdAsync(Guid id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<ContentPageDto>($"api/contentpages/{id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching content page {id}: {ex.Message}");
            return null;
        }
    }

    public async Task<ContentPageDto?> GetBySlugAsync(string slug)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<ContentPageDto>($"api/contentpages/slug/{slug}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching content page by slug {slug}: {ex.Message}");
            return null;
        }
    }
}
