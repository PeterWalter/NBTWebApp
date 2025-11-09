using NBT.Application.Venues.DTOs;

namespace NBT.WebUI.Client.Services;

public interface IVenueService
{
    Task<List<VenueDto>?> GetAllVenuesAsync();
    Task<List<VenueDto>?> GetActiveVenuesAsync();
    Task<VenueDto?> GetVenueByIdAsync(Guid id);
    Task<VenueDto?> CreateVenueAsync(CreateVenueDto dto);
    Task<bool> UpdateVenueAsync(Guid id, VenueDto dto);
    Task<bool> DeleteVenueAsync(Guid id);
    Task<List<VenueDto>?> SearchVenuesAsync(string searchTerm);
}
