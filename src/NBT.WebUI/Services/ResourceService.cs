using NBT.WebUI.Models;
using System.Net.Http.Json;

namespace NBT.WebUI.Services;

public interface IResourceService
{
    Task<List<DownloadableResourceDto>> GetAllAsync();
    Task<List<DownloadableResourceDto>> GetByCategoryAsync(string category);
    Task<DownloadableResourceDto?> GetByIdAsync(Guid id);
    Task IncrementDownloadCountAsync(Guid id);
}

public class ResourceService : IResourceService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ResourceService> _logger;

    public ResourceService(IHttpClientFactory httpClientFactory, ILogger<ResourceService> logger)
    {
        _httpClient = httpClientFactory.CreateClient("NBT.WebAPI");
        _logger = logger;
    }

    public async Task<List<DownloadableResourceDto>> GetAllAsync()
    {
        try
        {
            var result = await _httpClient.GetFromJsonAsync<List<DownloadableResourceDto>>("api/resources");
            return result ?? new List<DownloadableResourceDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching resources");
            return new List<DownloadableResourceDto>();
        }
    }

    public async Task<List<DownloadableResourceDto>> GetByCategoryAsync(string category)
    {
        try
        {
            var result = await _httpClient.GetFromJsonAsync<List<DownloadableResourceDto>>($"api/resources/category/{category}");
            return result ?? new List<DownloadableResourceDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching resources for category {Category}", category);
            return new List<DownloadableResourceDto>();
        }
    }

    public async Task<DownloadableResourceDto?> GetByIdAsync(Guid id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<DownloadableResourceDto>($"api/resources/{id}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching resource {Id}", id);
            return null;
        }
    }

    public async Task IncrementDownloadCountAsync(Guid id)
    {
        try
        {
            await _httpClient.PostAsync($"api/resources/{id}/download", null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error incrementing download count for resource {Id}", id);
        }
    }
}
