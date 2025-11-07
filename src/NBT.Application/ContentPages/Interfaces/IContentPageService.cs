using NBT.Application.ContentPages.DTOs;

namespace NBT.Application.ContentPages.Interfaces;

public interface IContentPageService
{
    Task<IEnumerable<ContentPageDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ContentPageDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ContentPageDto?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<ContentPageDto> CreateAsync(CreateContentPageDto dto, CancellationToken cancellationToken = default);
    Task<ContentPageDto> UpdateAsync(Guid id, UpdateContentPageDto dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
