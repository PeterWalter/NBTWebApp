using NBT.Domain.Entities;

namespace NBT.WebUI.Client.Services;

public interface IPublicContentService
{
    Task<List<TestPricing>> GetCurrentTestPricingAsync();
    Task<List<Announcement>> GetFeaturedAnnouncementsAsync();
}
