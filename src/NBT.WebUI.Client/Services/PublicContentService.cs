using System.Net.Http.Json;
using NBT.Domain.Entities;

namespace NBT.WebUI.Client.Services;

public class PublicContentService : IPublicContentService
{
    private readonly HttpClient _httpClient;

    public PublicContentService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<TestPricing>> GetCurrentTestPricingAsync()
    {
        try
        {
            var result = await _httpClient.GetFromJsonAsync<List<TestPricing>>("api/public/test-pricing");
            return result ?? new List<TestPricing>();
        }
        catch
        {
            return new List<TestPricing>();
        }
    }

    public async Task<List<Announcement>> GetFeaturedAnnouncementsAsync()
    {
        try
        {
            var result = await _httpClient.GetFromJsonAsync<List<Announcement>>("api/public/announcements/featured");
            return result ?? new List<Announcement>();
        }
        catch
        {
            return new List<Announcement>();
        }
    }
}
