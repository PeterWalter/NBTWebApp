using NBT.Application.Announcements.DTOs;

namespace NBT.Application.Announcements.Interfaces;

public interface IAnnouncementService
{
    Task<IEnumerable<AnnouncementDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<AnnouncementDto>> GetFeaturedAsync(CancellationToken cancellationToken = default);
    Task<AnnouncementDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<AnnouncementDto> CreateAsync(CreateAnnouncementDto dto, CancellationToken cancellationToken = default);
    Task<AnnouncementDto> UpdateAsync(Guid id, UpdateAnnouncementDto dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
