using Microsoft.EntityFrameworkCore;
using NBT.Application.Common.Interfaces;
using NBT.Application.ContentPages.DTOs;
using NBT.Application.ContentPages.Interfaces;
using NBT.Domain.Entities;

namespace NBT.Application.ContentPages.Services;

public class ContentPageService : IContentPageService
{
    private readonly IRepository<ContentPage> _repository;

    public ContentPageService(IRepository<ContentPage> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ContentPageDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var pages = await _repository.GetAllAsync(cancellationToken);
        return pages.Select(MapToDto);
    }

    public async Task<ContentPageDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var page = await _repository.GetByIdAsync(id, cancellationToken);
        return page == null ? null : MapToDto(page);
    }

    public async Task<ContentPageDto?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        var pages = await _repository.FindAsync(p => p.Slug == slug, cancellationToken);
        var page = pages.FirstOrDefault();
        return page == null ? null : MapToDto(page);
    }

    public async Task<ContentPageDto> CreateAsync(CreateContentPageDto dto, CancellationToken cancellationToken = default)
    {
        var page = new ContentPage
        {
            Title = dto.Title,
            Slug = dto.Slug,
            BodyContent = dto.BodyContent,
            MetaDescription = dto.MetaDescription,
            Keywords = dto.Keywords,
            PublicationDate = dto.PublicationDate ?? DateTime.UtcNow,
            Status = dto.Status
        };

        var created = await _repository.AddAsync(page, cancellationToken);
        return MapToDto(created);
    }

    public async Task<ContentPageDto> UpdateAsync(Guid id, UpdateContentPageDto dto, CancellationToken cancellationToken = default)
    {
        var page = await _repository.GetByIdAsync(id, cancellationToken);
        if (page == null)
            throw new KeyNotFoundException($"ContentPage with id {id} not found");

        page.Title = dto.Title;
        page.BodyContent = dto.BodyContent;
        page.MetaDescription = dto.MetaDescription;
        page.Keywords = dto.Keywords;
        page.Status = dto.Status;

        await _repository.UpdateAsync(page, cancellationToken);
        return MapToDto(page);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var page = await _repository.GetByIdAsync(id, cancellationToken);
        if (page == null)
            throw new KeyNotFoundException($"ContentPage with id {id} not found");

        await _repository.DeleteAsync(page, cancellationToken);
    }

    private static ContentPageDto MapToDto(ContentPage page)
    {
        return new ContentPageDto
        {
            Id = page.Id,
            Title = page.Title,
            Slug = page.Slug,
            BodyContent = page.BodyContent,
            MetaDescription = page.MetaDescription,
            Keywords = page.Keywords,
            PublicationDate = page.PublicationDate,
            Status = page.Status,
            CreatedDate = page.CreatedDate,
            LastModifiedDate = page.LastModifiedDate
        };
    }
}
