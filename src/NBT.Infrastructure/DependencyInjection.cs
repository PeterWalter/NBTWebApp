using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NBT.Application.Announcements.Interfaces;
using NBT.Application.Announcements.Services;
using NBT.Application.Common.Interfaces;
using NBT.Application.ContactInquiries.Interfaces;
using NBT.Application.ContactInquiries.Services;
using NBT.Application.ContentPages.Interfaces;
using NBT.Application.ContentPages.Services;
using NBT.Application.Resources.Interfaces;
using NBT.Application.Resources.Services;
using NBT.Domain.Entities;
using NBT.Infrastructure.Persistence;
using NBT.Infrastructure.Repositories;
using NBT.Infrastructure.Services;

namespace NBT.Infrastructure;

/// <summary>
/// Infrastructure layer dependency injection configuration.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds Infrastructure layer services to the service collection.
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <param name="configuration">Application configuration.</param>
    /// <returns>Service collection for chaining.</returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Database Context
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        // Identity
        services.AddIdentity<User, IdentityRole<Guid>>(options =>
        {
            // Password settings
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 8;

            // Lockout settings
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;

            // User settings
            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

        // Infrastructure Services
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IFileStorageService, FileStorageService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        // Repositories
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        // Application Services
        services.AddScoped<IContentPageService, ContentPageService>();
        services.AddScoped<IAnnouncementService, AnnouncementService>();
        services.AddScoped<IContactInquiryService, ContactInquiryService>();
        services.AddScoped<IDownloadableResourceService, DownloadableResourceService>();

        return services;
    }
}
