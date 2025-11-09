using NBT.Application.Common.Models;
using NBT.Application.Venues.DTOs;

namespace NBT.Application.Venues.Services;

public interface IVenueService
{
    Task<Result<List<VenueDto>>> GetAllVenuesAsync();
    Task<Result<VenueDto>> GetVenueByIdAsync(Guid id);
    Task<Result<VenueDto>> CreateVenueAsync(CreateVenueDto dto);
    Task<Result<VenueDto>> UpdateVenueAsync(Guid id, CreateVenueDto dto);
    Task<Result> DeleteVenueAsync(Guid id);
    Task<Result<List<VenueDto>>> GetActiveVenuesAsync();
    Task<Result<List<VenueDto>>> SearchVenuesAsync(string searchTerm);
}
