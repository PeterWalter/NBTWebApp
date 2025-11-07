using NBT.Application.Resources.DTOs;

namespace NBT.Application.Resources.Interfaces;

public interface IDownloadableResourceService
{
    Task<IEnumerable<DownloadableResourceDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<DownloadableResourceDto>> GetByCategoryAsync(string category, CancellationToken cancellationToken = default);
    Task<DownloadableResourceDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<DownloadableResourceDto> CreateAsync(CreateDownloadableResourceDto dto, CancellationToken cancellationToken = default);
    Task<DownloadableResourceDto> UpdateAsync(Guid id, UpdateDownloadableResourceDto dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task IncrementDownloadCountAsync(Guid id, CancellationToken cancellationToken = default);
}
