using NBT.Application.ContactInquiries.DTOs;

namespace NBT.Application.ContactInquiries.Interfaces;

public interface IContactInquiryService
{
    Task<ContactInquiryDto> SubmitAsync(CreateContactInquiryDto dto, CancellationToken cancellationToken = default);
    Task<ContactInquiryDto?> GetByReferenceNumberAsync(string referenceNumber, CancellationToken cancellationToken = default);
    Task<IEnumerable<ContactInquiryDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ContactInquiryDto> UpdateStatusAsync(Guid id, string status, string? responseMessage, CancellationToken cancellationToken = default);
}
