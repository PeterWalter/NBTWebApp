using NBT.Application.Announcements.DTOs;
using NBT.Application.Announcements.Interfaces;
using NBT.Application.Common.Interfaces;
using NBT.Domain.Entities;

namespace NBT.Application.Announcements.Services;

public class AnnouncementService : IAnnouncementService
{
    private readonly IRepository<Announcement> _repository;

    public AnnouncementService(IRepository<Announcement> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<AnnouncementDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var announcements = await _repository.GetAllAsync(cancellationToken);
        return announcements.Select(MapToDto).OrderByDescending(a => a.PublicationDate);
    }

    public async Task<IEnumerable<AnnouncementDto>> GetFeaturedAsync(CancellationToken cancellationToken = default)
    {
        var announcements = await _repository.FindAsync(a => a.IsFeatured && a.Status == "Published", cancellationToken);
        return announcements.Select(MapToDto).OrderByDescending(a => a.PublicationDate);
    }

    public async Task<AnnouncementDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var announcement = await _repository.GetByIdAsync(id, cancellationToken);
        return announcement == null ? null : MapToDto(announcement);
    }

    public async Task<AnnouncementDto> CreateAsync(CreateAnnouncementDto dto, CancellationToken cancellationToken = default)
    {
        var announcement = new Announcement
        {
            Title = dto.Title,
            Summary = dto.Summary,
            FullContent = dto.FullContent,
            Category = dto.Category,
            PublicationDate = dto.PublicationDate ?? DateTime.UtcNow,
            Status = dto.Status,
            IsFeatured = dto.IsFeatured
        };

        var created = await _repository.AddAsync(announcement, cancellationToken);
        return MapToDto(created);
    }

    public async Task<AnnouncementDto> UpdateAsync(Guid id, UpdateAnnouncementDto dto, CancellationToken cancellationToken = default)
    {
        var announcement = await _repository.GetByIdAsync(id, cancellationToken);
        if (announcement == null)
            throw new KeyNotFoundException($"Announcement with id {id} not found");

        announcement.Title = dto.Title;
        announcement.Summary = dto.Summary;
        announcement.FullContent = dto.FullContent;
        announcement.Category = dto.Category;
        announcement.Status = dto.Status;
        announcement.IsFeatured = dto.IsFeatured;

        await _repository.UpdateAsync(announcement, cancellationToken);
        return MapToDto(announcement);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var announcement = await _repository.GetByIdAsync(id, cancellationToken);
        if (announcement == null)
            throw new KeyNotFoundException($"Announcement with id {id} not found");

        await _repository.DeleteAsync(announcement, cancellationToken);
    }

    private static AnnouncementDto MapToDto(Announcement announcement)
    {
        return new AnnouncementDto
        {
            Id = announcement.Id,
            Title = announcement.Title,
            Summary = announcement.Summary,
            FullContent = announcement.FullContent,
            Category = announcement.Category,
            PublicationDate = announcement.PublicationDate,
            Status = announcement.Status,
            IsFeatured = announcement.IsFeatured,
            CreatedDate = announcement.CreatedDate,
            LastModifiedDate = announcement.LastModifiedDate
        };
    }
}
