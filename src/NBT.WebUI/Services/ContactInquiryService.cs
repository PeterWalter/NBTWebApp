using NBT.WebUI.Models;
using System.Net.Http.Json;

namespace NBT.WebUI.Services;

public interface IContactInquiryService
{
    Task<ContactInquiryDto?> SubmitInquiryAsync(CreateContactInquiryDto inquiry);
    Task<ContactInquiryDto?> GetByReferenceNumberAsync(string referenceNumber);
}

public class ContactInquiryService : IContactInquiryService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ContactInquiryService> _logger;

    public ContactInquiryService(IHttpClientFactory httpClientFactory, ILogger<ContactInquiryService> logger)
    {
        _httpClient = httpClientFactory.CreateClient("NBT.WebAPI");
        _logger = logger;
    }

    public async Task<ContactInquiryDto?> SubmitInquiryAsync(CreateContactInquiryDto inquiry)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/contactinquiries", inquiry);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ContactInquiryDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error submitting contact inquiry");
            return null;
        }
    }

    public async Task<ContactInquiryDto?> GetByReferenceNumberAsync(string referenceNumber)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<ContactInquiryDto>($"api/contactinquiries/reference/{referenceNumber}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching inquiry by reference number {ReferenceNumber}", referenceNumber);
            return null;
        }
    }
}
