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
    private readonly ILogger<ContentPageService> _logger;

    public ContentPageService(IHttpClientFactory httpClientFactory, ILogger<ContentPageService> logger)
    {
        _httpClient = httpClientFactory.CreateClient("NBT.WebAPI");
        _logger = logger;
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
            _logger.LogError(ex, "Error fetching content pages");
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
            _logger.LogError(ex, "Error fetching content page {Id}", id);
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
            _logger.LogError(ex, "Error fetching content page by slug {Slug}", slug);
            return null;
        }
    }
}
