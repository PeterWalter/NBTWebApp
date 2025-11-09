using NBT.Application.Venues.DTOs;

namespace NBT.WebUI.Client.Services;

public interface IRoomService
{
    Task<List<RoomDto>?> GetAllRoomsAsync();
    Task<List<RoomDto>?> GetRoomsByVenueIdAsync(Guid venueId);
    Task<List<RoomDto>?> GetAvailableRoomsAsync(Guid venueId);
    Task<RoomDto?> GetRoomByIdAsync(Guid id);
    Task<RoomDto?> CreateRoomAsync(CreateRoomDto dto);
    Task<bool> UpdateRoomAsync(Guid id, RoomDto dto);
    Task<bool> DeleteRoomAsync(Guid id);
}
