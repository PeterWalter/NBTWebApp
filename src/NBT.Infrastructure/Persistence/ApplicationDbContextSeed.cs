using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NBT.Domain.Entities;
using NBT.Domain.Enums;

namespace NBT.Infrastructure.Persistence;

/// <summary>
/// Seeds the database with initial data for development and testing.
/// </summary>
public static class ApplicationDbContextSeed
{
    public static async Task SeedAsync(
        ApplicationDbContext context,
        UserManager<User> userManager,
        RoleManager<IdentityRole<Guid>> roleManager)
    {
        // Seed Roles
        await SeedRolesAsync(roleManager);

        // Seed Admin User
        await SeedAdminUserAsync(userManager);

        // Seed Content Pages
        await SeedContentPagesAsync(context);

        // Seed Announcements
        await SeedAnnouncementsAsync(context);

        // Seed Downloadable Resources
        await SeedDownloadableResourcesAsync(context);

        await context.SaveChangesAsync();
    }

    private static async Task SeedRolesAsync(RoleManager<IdentityRole<Guid>> roleManager)
    {
        var roles = new[] { "Administrator", "Staff", "Educator", "Institution", "Applicant" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>(role));
            }
        }
    }

    private static async Task SeedAdminUserAsync(UserManager<User> userManager)
    {
        var adminEmail = "admin@nbt.ac.za";

        if (await userManager.FindByEmailAsync(adminEmail) == null)
        {
            var adminUser = new User
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
                FirstName = "System",
                LastName = "Administrator",
                Role = UserRole.Admin,
                Status = "Active",
                CreatedDate = DateTime.UtcNow
            };

            var result = await userManager.CreateAsync(adminUser, "Admin@123!");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Administrator");
            }
        }
    }

    private static async Task SeedContentPagesAsync(ApplicationDbContext context)
    {
        if (!await context.ContentPages.AnyAsync())
        {
            var pages = new List<ContentPage>
            {
                new ContentPage
                {
                    Id = Guid.NewGuid(),
                    Title = "About NBT",
                    Slug = "about",
                    BodyContent = @"<h2>About the National Benchmark Tests</h2>
                    <p>The National Benchmark Tests (NBT) project is a national initiative to assess academic readiness for university education in South Africa.</p>
                    <p>The tests complement and supplement the information provided by the National Senior Certificate (NSC).</p>
                    <h3>Purpose</h3>
                    <p>The NBTs are designed to provide a more holistic picture of an applicant's academic potential and readiness for university-level study.</p>",
                    MetaDescription = "Learn about the National Benchmark Tests and their purpose in South African higher education.",
                    Keywords = "NBT, National Benchmark Tests, academic readiness, university admission",
                    Status = "Published",
                    PublicationDate = DateTime.UtcNow,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "System"
                },
                new ContentPage
                {
                    Id = Guid.NewGuid(),
                    Title = "Privacy Policy",
                    Slug = "privacy-policy",
                    BodyContent = @"<h2>Privacy Policy</h2>
                    <p>This privacy policy explains how NBT collects, uses, and protects your personal information.</p>
                    <h3>Information Collection</h3>
                    <p>We collect information that you provide when registering for the NBT tests or contacting us.</p>
                    <h3>Data Protection</h3>
                    <p>Your data is protected in accordance with POPIA (Protection of Personal Information Act) and GDPR standards.</p>",
                    MetaDescription = "NBT Privacy Policy - How we protect your personal information.",
                    Keywords = "privacy policy, data protection, POPIA, personal information",
                    Status = "Published",
                    PublicationDate = DateTime.UtcNow,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "System"
                },
                new ContentPage
                {
                    Id = Guid.NewGuid(),
                    Title = "Terms and Conditions",
                    Slug = "terms-and-conditions",
                    BodyContent = @"<h2>Terms and Conditions</h2>
                    <p>By using this website and registering for NBT tests, you agree to these terms and conditions.</p>
                    <h3>Test Registration</h3>
                    <p>All test registrations are subject to availability and payment of the required fees.</p>
                    <h3>Cancellation Policy</h3>
                    <p>Cancellations must be made at least 7 days before the scheduled test date for a refund.</p>",
                    MetaDescription = "NBT Terms and Conditions for test registration and website use.",
                    Keywords = "terms and conditions, test registration, NBT policies",
                    Status = "Published",
                    PublicationDate = DateTime.UtcNow,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "System"
                }
            };

            context.ContentPages.AddRange(pages);
        }
    }

    private static async Task SeedAnnouncementsAsync(ApplicationDbContext context)
    {
        if (!await context.Announcements.AnyAsync())
        {
            var announcements = new List<Announcement>
            {
                new Announcement
                {
                    Id = Guid.NewGuid(),
                    Title = "2025 Test Dates Announced",
                    Summary = "NBT test dates for 2025 have been announced. Register early to secure your preferred date and venue.",
                    FullContent = @"<p>We are pleased to announce the NBT test dates for 2025. Tests will be conducted at venues across South Africa.</p>
                    <p>Early registration is encouraged as seats are limited. Visit the registration portal to book your test.</p>
                    <ul>
                        <li>Term 1: February - March 2025</li>
                        <li>Term 2: May - June 2025</li>
                        <li>Term 3: August - September 2025</li>
                    </ul>",
                    Category = AnnouncementCategory.GeneralNews,
                    Status = "Published",
                    IsFeatured = true,
                    PublicationDate = DateTime.UtcNow.AddDays(-5),
                    CreatedDate = DateTime.UtcNow.AddDays(-5),
                    CreatedBy = "System"
                },
                new Announcement
                {
                    Id = Guid.NewGuid(),
                    Title = "New Venues Added in Eastern Cape",
                    Summary = "Additional test venues have been added in the Eastern Cape province to improve accessibility.",
                    FullContent = @"<p>In response to increased demand, NBT has added new test venues in the Eastern Cape:</p>
                    <ul>
                        <li>Mthatha</li>
                        <li>East London</li>
                        <li>Gqeberha (Port Elizabeth)</li>
                    </ul>
                    <p>These venues will be available for all 2025 test sessions.</p>",
                    Category = AnnouncementCategory.TestDates,
                    Status = "Published",
                    IsFeatured = false,
                    PublicationDate = DateTime.UtcNow.AddDays(-10),
                    CreatedDate = DateTime.UtcNow.AddDays(-10),
                    CreatedBy = "System"
                },
                new Announcement
                {
                    Id = Guid.NewGuid(),
                    Title = "Updated Registration Process",
                    Summary = "The NBT registration process has been streamlined for improved user experience.",
                    FullContent = @"<p>We have updated our registration process to make it faster and more user-friendly:</p>
                    <ul>
                        <li>Simplified online registration form</li>
                        <li>Multiple payment options including EFT and credit card</li>
                        <li>Instant confirmation emails</li>
                        <li>Digital admit cards</li>
                    </ul>",
                    Category = AnnouncementCategory.PolicyUpdate,
                    Status = "Published",
                    IsFeatured = true,
                    PublicationDate = DateTime.UtcNow.AddDays(-2),
                    CreatedDate = DateTime.UtcNow.AddDays(-2),
                    CreatedBy = "System"
                }
            };

            context.Announcements.AddRange(announcements);
        }
    }

    private static async Task SeedDownloadableResourcesAsync(ApplicationDbContext context)
    {
        if (!await context.DownloadableResources.AnyAsync())
        {
            var resources = new List<DownloadableResource>
            {
                new DownloadableResource
                {
                    Id = Guid.NewGuid(),
                    Title = "NBT Information Brochure",
                    Description = "Comprehensive information about the National Benchmark Tests, including test structure, content domains, and preparation tips.",
                    FilePath = "/resources/nbt-information-brochure.pdf",
                    FileType = "PDF",
                    FileSize = 2457600, // ~2.4 MB
                    Category = "General",
                    UploadDate = DateTime.UtcNow,
                    DownloadCount = 0,
                    Status = "Active",
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "System"
                },
                new DownloadableResource
                {
                    Id = Guid.NewGuid(),
                    Title = "Sample Questions - Academic Literacy",
                    Description = "Sample questions for the Academic Literacy test component with answers and explanations.",
                    FilePath = "/resources/sample-questions-academic-literacy.pdf",
                    FileType = "PDF",
                    FileSize = 1536000, // ~1.5 MB
                    Category = "Educator",
                    UploadDate = DateTime.UtcNow,
                    DownloadCount = 0,
                    Status = "Active",
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "System"
                },
                new DownloadableResource
                {
                    Id = Guid.NewGuid(),
                    Title = "Sample Questions - Quantitative Literacy",
                    Description = "Sample questions for the Quantitative Literacy test component with detailed solutions.",
                    FilePath = "/resources/sample-questions-quantitative-literacy.pdf",
                    FileType = "PDF",
                    FileSize = 1843200, // ~1.8 MB
                    Category = "Educator",
                    UploadDate = DateTime.UtcNow,
                    DownloadCount = 0,
                    Status = "Active",
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "System"
                },
                new DownloadableResource
                {
                    Id = Guid.NewGuid(),
                    Title = "Educator Guide to NBT Results",
                    Description = "Guide for educators on how to interpret and use NBT results for student support and placement.",
                    FilePath = "/resources/educator-guide-nbt-results.pdf",
                    FileType = "PDF",
                    FileSize = 3072000, // ~3 MB
                    Category = "Educator",
                    UploadDate = DateTime.UtcNow,
                    DownloadCount = 0,
                    Status = "Active",
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "System"
                },
                new DownloadableResource
                {
                    Id = Guid.NewGuid(),
                    Title = "Institution Data Integration Specification",
                    Description = "Technical specification for institutions integrating NBT results into their admission systems.",
                    FilePath = "/resources/institution-data-integration-spec.pdf",
                    FileType = "PDF",
                    FileSize = 2048000, // ~2 MB
                    Category = "Institution",
                    UploadDate = DateTime.UtcNow,
                    DownloadCount = 0,
                    Status = "Active",
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "System"
                }
            };

            context.DownloadableResources.AddRange(resources);
        }
    }
}
