using Microsoft.AspNetCore.Mvc;
using NBT.Application.ContactInquiries.DTOs;
using NBT.Application.ContactInquiries.Interfaces;

namespace NBT.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactInquiriesController : ControllerBase
{
    private readonly IContactInquiryService _service;
    private readonly ILogger<ContactInquiriesController> _logger;

    public ContactInquiriesController(IContactInquiryService service, ILogger<ContactInquiriesController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<ContactInquiryDto>> Submit([FromBody] CreateContactInquiryDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var inquiry = await _service.SubmitAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetByReferenceNumber), new { referenceNumber = inquiry.ReferenceNumber }, inquiry);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error submitting contact inquiry");
            return StatusCode(500, "An error occurred while submitting your inquiry");
        }
    }

    [HttpGet("reference/{referenceNumber}")]
    public async Task<ActionResult<ContactInquiryDto>> GetByReferenceNumber(string referenceNumber, CancellationToken cancellationToken)
    {
        try
        {
            var inquiry = await _service.GetByReferenceNumberAsync(referenceNumber, cancellationToken);
            if (inquiry == null)
                return NotFound();

            return Ok(inquiry);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving inquiry by reference {Reference}", referenceNumber);
            return StatusCode(500, "An error occurred while retrieving the inquiry");
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ContactInquiryDto>>> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            var inquiries = await _service.GetAllAsync(cancellationToken);
            return Ok(inquiries);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving contact inquiries");
            return StatusCode(500, "An error occurred while retrieving inquiries");
        }
    }

    [HttpPut("{id:guid}/status")]
    public async Task<ActionResult<ContactInquiryDto>> UpdateStatus(
        Guid id,
        [FromBody] UpdateStatusRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var updated = await _service.UpdateStatusAsync(id, request.Status, request.ResponseMessage, cancellationToken);
            return Ok(updated);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating inquiry status {Id}", id);
            return StatusCode(500, "An error occurred while updating the inquiry status");
        }
    }
}

public class UpdateStatusRequest
{
    public string Status { get; set; } = string.Empty;
    public string? ResponseMessage { get; set; }
}
