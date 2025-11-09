using NBT.Application.Common.Models;
using NBT.Application.Venues.DTOs;

namespace NBT.Application.Venues.Services;

public interface IRoomService
{
    Task<Result<List<RoomDto>>> GetAllRoomsAsync();
    Task<Result<List<RoomDto>>> GetRoomsByVenueIdAsync(Guid venueId);
    Task<Result<RoomDto>> GetRoomByIdAsync(Guid id);
    Task<Result<RoomDto>> CreateRoomAsync(CreateRoomDto dto);
    Task<Result<RoomDto>> UpdateRoomAsync(Guid id, CreateRoomDto dto);
    Task<Result> DeleteRoomAsync(Guid id);
    Task<Result<List<RoomDto>>> GetAvailableRoomsAsync(Guid venueId);
}
