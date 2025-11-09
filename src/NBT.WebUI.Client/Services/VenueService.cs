using System.Net.Http.Json;
using NBT.Application.Venues.DTOs;

namespace NBT.WebUI.Client.Services;

public class VenueService : IVenueService
{
    private readonly HttpClient _httpClient;

    public VenueService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<VenueDto>?> GetAllVenuesAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<VenueDto>>("api/venues");
    }

    public async Task<List<VenueDto>?> GetActiveVenuesAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<VenueDto>>("api/venues/active");
    }

    public async Task<VenueDto?> GetVenueByIdAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<VenueDto>($"api/venues/{id}");
    }

    public async Task<VenueDto?> CreateVenueAsync(CreateVenueDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/venues", dto);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<VenueDto>();
    }

    public async Task<bool> UpdateVenueAsync(Guid id, VenueDto dto)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/venues/{id}", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteVenueAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"api/venues/{id}");
        return response.IsSuccessStatusCode;
    }

    public async Task<List<VenueDto>?> SearchVenuesAsync(string searchTerm)
    {
        return await _httpClient.GetFromJsonAsync<List<VenueDto>>($"api/venues/search?searchTerm={searchTerm}");
    }
}
