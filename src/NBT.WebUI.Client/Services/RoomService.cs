using System.Net.Http.Json;
using NBT.Application.Venues.DTOs;

namespace NBT.WebUI.Client.Services;

public class RoomService : IRoomService
{
    private readonly HttpClient _httpClient;

    public RoomService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<RoomDto>?> GetAllRoomsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<RoomDto>>("api/rooms");
    }

    public async Task<List<RoomDto>?> GetRoomsByVenueIdAsync(Guid venueId)
    {
        return await _httpClient.GetFromJsonAsync<List<RoomDto>>($"api/rooms/venue/{venueId}");
    }

    public async Task<List<RoomDto>?> GetAvailableRoomsAsync(Guid venueId)
    {
        return await _httpClient.GetFromJsonAsync<List<RoomDto>>($"api/rooms/venue/{venueId}/available");
    }

    public async Task<RoomDto?> GetRoomByIdAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<RoomDto>($"api/rooms/{id}");
    }

    public async Task<RoomDto?> CreateRoomAsync(CreateRoomDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/rooms", dto);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<RoomDto>();
    }

    public async Task<bool> UpdateRoomAsync(Guid id, RoomDto dto)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/rooms/{id}", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteRoomAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"api/rooms/{id}");
        return response.IsSuccessStatusCode;
    }
}
