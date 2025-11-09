using Microsoft.EntityFrameworkCore;
using NBT.Application.Common.Models;
using NBT.Application.Venues.DTOs;
using NBT.Application.Venues.Services;
using NBT.Domain.Entities;
using NBT.Infrastructure.Persistence;

namespace NBT.Infrastructure.Services.Venues;

public class RoomService : IRoomService
{
    private readonly ApplicationDbContext _context;

    public RoomService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<RoomDto>>> GetAllRoomsAsync()
    {
        try
        {
            var rooms = await _context.Rooms
                .Include(r => r.Venue)
                .OrderBy(r => r.Venue.VenueName)
                .ThenBy(r => r.RoomName)
                .Select(r => MapToDto(r))
                .ToListAsync();

            return Result<List<RoomDto>>.Success(rooms);
        }
        catch (Exception ex)
        {
            return Result<List<RoomDto>>.Failure($"Error retrieving rooms: {ex.Message}");
        }
    }

    public async Task<Result<List<RoomDto>>> GetRoomsByVenueIdAsync(Guid venueId)
    {
        try
        {
            var rooms = await _context.Rooms
                .Include(r => r.Venue)
                .Where(r => r.VenueId == venueId)
                .OrderBy(r => r.RoomName)
                .Select(r => MapToDto(r))
                .ToListAsync();

            return Result<List<RoomDto>>.Success(rooms);
        }
        catch (Exception ex)
        {
            return Result<List<RoomDto>>.Failure($"Error retrieving rooms: {ex.Message}");
        }
    }

    public async Task<Result<RoomDto>> GetRoomByIdAsync(Guid id)
    {
        try
        {
            var room = await _context.Rooms
                .Include(r => r.Venue)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (room == null)
                return Result<RoomDto>.Failure("Room not found");

            return Result<RoomDto>.Success(MapToDto(room));
        }
        catch (Exception ex)
        {
            return Result<RoomDto>.Failure($"Error retrieving room: {ex.Message}");
        }
    }

    public async Task<Result<RoomDto>> CreateRoomAsync(CreateRoomDto dto)
    {
        try
        {
            // Verify venue exists
            var venue = await _context.Venues.FindAsync(dto.VenueId);
            if (venue == null)
                return Result<RoomDto>.Failure("Venue not found");

            var room = new Room
            {
                Id = Guid.NewGuid(),
                VenueId = dto.VenueId,
                RoomName = dto.RoomName,
                RoomNumber = dto.RoomNumber,
                Capacity = dto.Capacity,
                RoomType = dto.RoomType,
                HasComputers = dto.HasComputers,
                ComputerCount = dto.ComputerCount,
                IsAccessible = dto.IsAccessible,
                Status = "Available",
                Notes = dto.Notes,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = "System" // TODO: Get from current user
            };

            _context.Rooms.Add(room);

            // Update venue total capacity
            venue.TotalCapacity = await _context.Rooms
                .Where(r => r.VenueId == dto.VenueId)
                .SumAsync(r => r.Capacity) + dto.Capacity;

            venue.LastModifiedDate = DateTime.UtcNow;
            venue.LastModifiedBy = "System";

            await _context.SaveChangesAsync();

            room.Venue = venue;
            return Result<RoomDto>.Success(MapToDto(room));
        }
        catch (Exception ex)
        {
            return Result<RoomDto>.Failure($"Error creating room: {ex.Message}");
        }
    }

    public async Task<Result<RoomDto>> UpdateRoomAsync(Guid id, CreateRoomDto dto)
    {
        try
        {
            var room = await _context.Rooms
                .Include(r => r.Venue)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (room == null)
                return Result<RoomDto>.Failure("Room not found");

            var oldCapacity = room.Capacity;

            room.RoomName = dto.RoomName;
            room.RoomNumber = dto.RoomNumber;
            room.Capacity = dto.Capacity;
            room.RoomType = dto.RoomType;
            room.HasComputers = dto.HasComputers;
            room.ComputerCount = dto.ComputerCount;
            room.IsAccessible = dto.IsAccessible;
            room.Notes = dto.Notes;
            room.LastModifiedDate = DateTime.UtcNow;
            room.LastModifiedBy = "System"; // TODO: Get from current user

            // Update venue total capacity
            var venue = room.Venue;
            venue.TotalCapacity = venue.TotalCapacity - oldCapacity + dto.Capacity;
            venue.LastModifiedDate = DateTime.UtcNow;
            venue.LastModifiedBy = "System";

            await _context.SaveChangesAsync();

            return Result<RoomDto>.Success(MapToDto(room));
        }
        catch (Exception ex)
        {
            return Result<RoomDto>.Failure($"Error updating room: {ex.Message}");
        }
    }

    public async Task<Result> DeleteRoomAsync(Guid id)
    {
        try
        {
            var room = await _context.Rooms
                .Include(r => r.Venue)
                .Include(r => r.RoomAllocations)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (room == null)
                return Result.Failure("Room not found");

            // Check if room has allocations
            if (room.RoomAllocations.Any())
                return Result.Failure("Cannot delete room with existing allocations");

            var venue = room.Venue;
            venue.TotalCapacity -= room.Capacity;
            venue.LastModifiedDate = DateTime.UtcNow;
            venue.LastModifiedBy = "System";

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Error deleting room: {ex.Message}");
        }
    }

    public async Task<Result<List<RoomDto>>> GetAvailableRoomsAsync(Guid venueId)
    {
        try
        {
            var rooms = await _context.Rooms
                .Include(r => r.Venue)
                .Where(r => r.VenueId == venueId && r.Status == "Available")
                .OrderBy(r => r.RoomName)
                .Select(r => MapToDto(r))
                .ToListAsync();

            return Result<List<RoomDto>>.Success(rooms);
        }
        catch (Exception ex)
        {
            return Result<List<RoomDto>>.Failure($"Error retrieving available rooms: {ex.Message}");
        }
    }

    private static RoomDto MapToDto(Room room)
    {
        return new RoomDto
        {
            Id = room.Id,
            VenueId = room.VenueId,
            VenueName = room.Venue?.VenueName,
            RoomName = room.RoomName,
            RoomNumber = room.RoomNumber,
            Capacity = room.Capacity,
            RoomType = room.RoomType,
            HasComputers = room.HasComputers,
            ComputerCount = room.ComputerCount,
            IsAccessible = room.IsAccessible,
            Status = room.Status,
            Notes = room.Notes,
            CreatedDate = room.CreatedDate,
            LastModifiedDate = room.LastModifiedDate
        };
    }
}
