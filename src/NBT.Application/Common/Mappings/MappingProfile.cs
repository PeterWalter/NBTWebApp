using AutoMapper;
using NBT.Domain.Entities;
using NBT.Application.ContentPages.DTOs;
using NBT.Application.Announcements.DTOs;
using NBT.Application.ContactInquiries.DTOs;
using NBT.Application.Resources.DTOs;

namespace NBT.Application.Common.Mappings;

/// <summary>
/// AutoMapper profile for mapping between domain entities and DTOs.
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // ContentPage mappings
        CreateMap<ContentPage, ContentPageDto>().ReverseMap();

        // Announcement mappings
        CreateMap<Announcement, AnnouncementDto>().ReverseMap();

        // ContactInquiry mappings
        CreateMap<ContactInquiry, ContactInquiryDto>().ReverseMap();

        // DownloadableResource mappings
        CreateMap<DownloadableResource, ResourceDto>().ReverseMap();
    }
}
