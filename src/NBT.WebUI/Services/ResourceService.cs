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

    public ResourceService(HttpClient httpClient)
    {
        _httpClient = httpClient;
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
            Console.WriteLine($"Error fetching resources: {ex.Message}");
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
            Console.WriteLine($"Error fetching resources for category {category}: {ex.Message}");
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
            Console.WriteLine($"Error fetching resource {id}: {ex.Message}");
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
            Console.WriteLine($"Error incrementing download count for resource {id}: {ex.Message}");
        }
    }
}
