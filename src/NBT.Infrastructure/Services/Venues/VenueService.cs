using Microsoft.EntityFrameworkCore;
using NBT.Application.Common.Models;
using NBT.Application.Venues.DTOs;
using NBT.Application.Venues.Services;
using NBT.Domain.Entities;
using NBT.Infrastructure.Persistence;

namespace NBT.Infrastructure.Services.Venues;

public class VenueService : IVenueService
{
    private readonly ApplicationDbContext _context;

    public VenueService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<VenueDto>>> GetAllVenuesAsync()
    {
        try
        {
            var venues = await _context.Venues
                .Include(v => v.Rooms)
                .OrderBy(v => v.VenueName)
                .Select(v => MapToDto(v))
                .ToListAsync();

            return Result<List<VenueDto>>.Success(venues);
        }
        catch (Exception ex)
        {
            return Result<List<VenueDto>>.Failure($"Error retrieving venues: {ex.Message}");
        }
    }

    public async Task<Result<VenueDto>> GetVenueByIdAsync(Guid id)
    {
        try
        {
            var venue = await _context.Venues
                .Include(v => v.Rooms)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (venue == null)
                return Result<VenueDto>.Failure("Venue not found");

            return Result<VenueDto>.Success(MapToDto(venue));
        }
        catch (Exception ex)
        {
            return Result<VenueDto>.Failure($"Error retrieving venue: {ex.Message}");
        }
    }

    public async Task<Result<VenueDto>> CreateVenueAsync(CreateVenueDto dto)
    {
        try
        {
            // Check if venue code already exists
            if (await _context.Venues.AnyAsync(v => v.VenueCode == dto.VenueCode))
                return Result<VenueDto>.Failure("Venue code already exists");

            var venue = new Venue
            {
                Id = Guid.NewGuid(),
                VenueName = dto.VenueName,
                VenueCode = dto.VenueCode,
                Address = dto.Address,
                City = dto.City,
                Province = dto.Province,
                PostalCode = dto.PostalCode,
                ContactPerson = dto.ContactPerson,
                ContactEmail = dto.ContactEmail,
                ContactPhone = dto.ContactPhone,
                IsAccessible = dto.IsAccessible,
                Status = "Active",
                Notes = dto.Notes,
                TotalCapacity = 0,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = "System" // TODO: Get from current user
            };

            _context.Venues.Add(venue);
            await _context.SaveChangesAsync();

            return Result<VenueDto>.Success(MapToDto(venue));
        }
        catch (Exception ex)
        {
            return Result<VenueDto>.Failure($"Error creating venue: {ex.Message}");
        }
    }

    public async Task<Result<VenueDto>> UpdateVenueAsync(Guid id, CreateVenueDto dto)
    {
        try
        {
            var venue = await _context.Venues.FindAsync(id);
            if (venue == null)
                return Result<VenueDto>.Failure("Venue not found");

            // Check if venue code already exists (excluding current venue)
            if (await _context.Venues.AnyAsync(v => v.VenueCode == dto.VenueCode && v.Id != id))
                return Result<VenueDto>.Failure("Venue code already exists");

            venue.VenueName = dto.VenueName;
            venue.VenueCode = dto.VenueCode;
            venue.Address = dto.Address;
            venue.City = dto.City;
            venue.Province = dto.Province;
            venue.PostalCode = dto.PostalCode;
            venue.ContactPerson = dto.ContactPerson;
            venue.ContactEmail = dto.ContactEmail;
            venue.ContactPhone = dto.ContactPhone;
            venue.IsAccessible = dto.IsAccessible;
            venue.Notes = dto.Notes;
            venue.LastModifiedDate = DateTime.UtcNow;
            venue.LastModifiedBy = "System"; // TODO: Get from current user

            await _context.SaveChangesAsync();

            return Result<VenueDto>.Success(MapToDto(venue));
        }
        catch (Exception ex)
        {
            return Result<VenueDto>.Failure($"Error updating venue: {ex.Message}");
        }
    }

    public async Task<Result> DeleteVenueAsync(Guid id)
    {
        try
        {
            var venue = await _context.Venues
                .Include(v => v.Rooms)
                .Include(v => v.TestSessions)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (venue == null)
                return Result.Failure("Venue not found");

            // Check if venue has associated rooms or test sessions
            if (venue.Rooms.Any())
                return Result.Failure("Cannot delete venue with associated rooms");

            if (venue.TestSessions.Any())
                return Result.Failure("Cannot delete venue with associated test sessions");

            _context.Venues.Remove(venue);
            await _context.SaveChangesAsync();

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Error deleting venue: {ex.Message}");
        }
    }

    public async Task<Result<List<VenueDto>>> GetActiveVenuesAsync()
    {
        try
        {
            var venues = await _context.Venues
                .Include(v => v.Rooms)
                .Where(v => v.Status == "Active")
                .OrderBy(v => v.VenueName)
                .Select(v => MapToDto(v))
                .ToListAsync();

            return Result<List<VenueDto>>.Success(venues);
        }
        catch (Exception ex)
        {
            return Result<List<VenueDto>>.Failure($"Error retrieving active venues: {ex.Message}");
        }
    }

    public async Task<Result<List<VenueDto>>> SearchVenuesAsync(string searchTerm)
    {
        try
        {
            var query = _context.Venues.Include(v => v.Rooms).AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                query = query.Where(v =>
                    v.VenueName.ToLower().Contains(searchTerm) ||
                    v.VenueCode.ToLower().Contains(searchTerm) ||
                    v.City.ToLower().Contains(searchTerm) ||
                    v.Province.ToLower().Contains(searchTerm));
            }

            var venues = await query
                .OrderBy(v => v.VenueName)
                .Select(v => MapToDto(v))
                .ToListAsync();

            return Result<List<VenueDto>>.Success(venues);
        }
        catch (Exception ex)
        {
            return Result<List<VenueDto>>.Failure($"Error searching venues: {ex.Message}");
        }
    }

    private static VenueDto MapToDto(Venue venue)
    {
        return new VenueDto
        {
            Id = venue.Id,
            VenueName = venue.VenueName,
            VenueCode = venue.VenueCode,
            Address = venue.Address,
            City = venue.City,
            Province = venue.Province,
            PostalCode = venue.PostalCode,
            ContactPerson = venue.ContactPerson,
            ContactEmail = venue.ContactEmail,
            ContactPhone = venue.ContactPhone,
            TotalCapacity = venue.TotalCapacity,
            IsAccessible = venue.IsAccessible,
            Status = venue.Status,
            Notes = venue.Notes,
            RoomCount = venue.Rooms?.Count ?? 0,
            CreatedDate = venue.CreatedDate,
            LastModifiedDate = venue.LastModifiedDate
        };
    }
}
