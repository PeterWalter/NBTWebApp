using NBT.Application.Common.Interfaces;
using NBT.Application.ContactInquiries.DTOs;
using NBT.Application.ContactInquiries.Interfaces;
using NBT.Domain.Entities;
using NBT.Domain.Enums;

namespace NBT.Application.ContactInquiries.Services;

public class ContactInquiryService : IContactInquiryService
{
    private readonly IRepository<ContactInquiry> _repository;

    public ContactInquiryService(IRepository<ContactInquiry> repository)
    {
        _repository = repository;
    }

    public async Task<ContactInquiryDto> SubmitAsync(CreateContactInquiryDto dto, CancellationToken cancellationToken = default)
    {
        var referenceNumber = GenerateReferenceNumber();
        
        var inquiry = new ContactInquiry
        {
            SubmissionDateTime = DateTime.UtcNow,
            Name = dto.Name,
            Email = dto.Email,
            Phone = dto.Phone,
            InquiryType = dto.InquiryType,
            Subject = dto.Subject,
            Message = dto.Message,
            Status = InquiryStatus.New,
            ReferenceNumber = referenceNumber,
            PrivacyConsent = true
        };

        var created = await _repository.AddAsync(inquiry, cancellationToken);
        return MapToDto(created);
    }

    public async Task<ContactInquiryDto?> GetByReferenceNumberAsync(string referenceNumber, CancellationToken cancellationToken = default)
    {
        var inquiries = await _repository.FindAsync(i => i.ReferenceNumber == referenceNumber, cancellationToken);
        var inquiry = inquiries.FirstOrDefault();
        return inquiry == null ? null : MapToDto(inquiry);
    }

    public async Task<IEnumerable<ContactInquiryDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var inquiries = await _repository.GetAllAsync(cancellationToken);
        return inquiries.Select(MapToDto).OrderByDescending(i => i.SubmissionDateTime);
    }

    public async Task<ContactInquiryDto> UpdateStatusAsync(Guid id, string status, string? responseMessage, CancellationToken cancellationToken = default)
    {
        var inquiry = await _repository.GetByIdAsync(id, cancellationToken);
        if (inquiry == null)
            throw new KeyNotFoundException($"ContactInquiry with id {id} not found");

        if (Enum.TryParse<InquiryStatus>(status, out var inquiryStatus))
        {
            inquiry.Status = inquiryStatus;
        }
        
        if (!string.IsNullOrWhiteSpace(responseMessage))
        {
            inquiry.Response = responseMessage;
        }

        await _repository.UpdateAsync(inquiry, cancellationToken);
        return MapToDto(inquiry);
    }

    private static string GenerateReferenceNumber()
    {
        return $"NBT{DateTime.UtcNow:yyyyMMddHHmmss}{Random.Shared.Next(1000, 9999)}";
    }

    private static ContactInquiryDto MapToDto(ContactInquiry inquiry)
    {
        return new ContactInquiryDto
        {
            Id = inquiry.Id,
            SubmissionDateTime = inquiry.SubmissionDateTime,
            Name = inquiry.Name,
            Email = inquiry.Email,
            Phone = inquiry.Phone,
            InquiryType = inquiry.InquiryType,
            Subject = inquiry.Subject,
            Message = inquiry.Message,
            Status = inquiry.Status,
            AssignedToId = inquiry.AssignedToId,
            Response = inquiry.Response,
            ReferenceNumber = inquiry.ReferenceNumber
        };
    }
}
