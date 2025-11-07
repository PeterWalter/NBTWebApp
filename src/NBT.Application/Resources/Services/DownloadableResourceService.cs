using NBT.Application.Common.Interfaces;
using NBT.Application.Resources.DTOs;
using NBT.Application.Resources.Interfaces;
using NBT.Domain.Entities;

namespace NBT.Application.Resources.Services;

public class DownloadableResourceService : IDownloadableResourceService
{
    private readonly IRepository<DownloadableResource> _repository;

    public DownloadableResourceService(IRepository<DownloadableResource> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<DownloadableResourceDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var resources = await _repository.GetAllAsync(cancellationToken);
        return resources.Select(MapToDto).OrderByDescending(r => r.UploadDate);
    }

    public async Task<IEnumerable<DownloadableResourceDto>> GetByCategoryAsync(string category, CancellationToken cancellationToken = default)
    {
        var resources = await _repository.FindAsync(r => r.Category == category && r.Status == "Active", cancellationToken);
        return resources.Select(MapToDto).OrderByDescending(r => r.UploadDate);
    }

    public async Task<DownloadableResourceDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var resource = await _repository.GetByIdAsync(id, cancellationToken);
        return resource == null ? null : MapToDto(resource);
    }

    public async Task<DownloadableResourceDto> CreateAsync(CreateDownloadableResourceDto dto, CancellationToken cancellationToken = default)
    {
        var resource = new DownloadableResource
        {
            Title = dto.Title,
            Description = dto.Description,
            FilePath = dto.FilePath,
            FileType = dto.FileType,
            FileSize = dto.FileSize,
            Category = dto.Category,
            UploadDate = DateTime.UtcNow,
            DownloadCount = 0,
            Status = dto.Status
        };

        var created = await _repository.AddAsync(resource, cancellationToken);
        return MapToDto(created);
    }

    public async Task<DownloadableResourceDto> UpdateAsync(Guid id, UpdateDownloadableResourceDto dto, CancellationToken cancellationToken = default)
    {
        var resource = await _repository.GetByIdAsync(id, cancellationToken);
        if (resource == null)
            throw new KeyNotFoundException($"DownloadableResource with id {id} not found");

        resource.Title = dto.Title;
        resource.Description = dto.Description;
        resource.Category = dto.Category;
        resource.Status = dto.Status;

        await _repository.UpdateAsync(resource, cancellationToken);
        return MapToDto(resource);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var resource = await _repository.GetByIdAsync(id, cancellationToken);
        if (resource == null)
            throw new KeyNotFoundException($"DownloadableResource with id {id} not found");

        await _repository.DeleteAsync(resource, cancellationToken);
    }

    public async Task IncrementDownloadCountAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var resource = await _repository.GetByIdAsync(id, cancellationToken);
        if (resource == null)
            throw new KeyNotFoundException($"DownloadableResource with id {id} not found");

        resource.DownloadCount++;
        await _repository.UpdateAsync(resource, cancellationToken);
    }

    private static DownloadableResourceDto MapToDto(DownloadableResource resource)
    {
        return new DownloadableResourceDto
        {
            Id = resource.Id,
            Title = resource.Title,
            Description = resource.Description,
            FilePath = resource.FilePath,
            FileType = resource.FileType,
            FileSize = resource.FileSize,
            Category = resource.Category,
            UploadDate = resource.UploadDate,
            DownloadCount = resource.DownloadCount,
            Status = resource.Status
        };
    }
}
