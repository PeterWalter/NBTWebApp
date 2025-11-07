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

    public ContactInquiryService(HttpClient httpClient)
    {
        _httpClient = httpClient;
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
            Console.WriteLine($"Error submitting contact inquiry: {ex.Message}");
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
            Console.WriteLine($"Error fetching inquiry by reference number {referenceNumber}: {ex.Message}");
            return null;
        }
    }
}
