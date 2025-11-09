using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.FluentUI.AspNetCore.Components;
using NBT.WebUI.Components;
using NBT.WebUI.Services;
using NBT.WebUI.Services.Bookings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Configure Blazor Hub connection with extended timeouts
builder.Services.AddServerSideBlazor(options =>
{
    options.DetailedErrors = builder.Environment.IsDevelopment();
    options.DisconnectedCircuitMaxRetained = 100;
    options.DisconnectedCircuitRetentionPeriod = TimeSpan.FromMinutes(5);
    options.JSInteropDefaultCallTimeout = TimeSpan.FromMinutes(2);
    options.MaxBufferedUnacknowledgedRenderBatches = 20;
})
.AddHubOptions(options =>
{
    options.ClientTimeoutInterval = TimeSpan.FromMinutes(1);
    options.HandshakeTimeout = TimeSpan.FromSeconds(30);
    options.KeepAliveInterval = TimeSpan.FromSeconds(15);
    options.MaximumReceiveMessageSize = 128 * 1024;
});

// Add Fluent UI
builder.Services.AddFluentUIComponents();

// Add Authentication and Authorization
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();

// Add HTTP Client for API calls
var apiUrl = builder.Configuration["ApiBaseUrl"] ?? "http://localhost:5000/";

// Register HttpClient for non-service usage
builder.Services.AddHttpClient();

// Add Authentication Service
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddHttpClient<IAuthenticationService, AuthenticationService>(client =>
{
    client.BaseAddress = new Uri(apiUrl);
    client.Timeout = TimeSpan.FromSeconds(30);
});

builder.Services.AddHttpClient<IContentPageService, ContentPageService>(client =>
{
    client.BaseAddress = new Uri(apiUrl);
    client.Timeout = TimeSpan.FromSeconds(30);
});
builder.Services.AddHttpClient<IAnnouncementService, AnnouncementService>(client =>
{
    client.BaseAddress = new Uri(apiUrl);
    client.Timeout = TimeSpan.FromSeconds(30);
});
builder.Services.AddHttpClient<IResourceService, ResourceService>(client =>
{
    client.BaseAddress = new Uri(apiUrl);
    client.Timeout = TimeSpan.FromSeconds(30);
});
builder.Services.AddHttpClient<IContactInquiryService, ContactInquiryService>(client =>
{
    client.BaseAddress = new Uri(apiUrl);
    client.Timeout = TimeSpan.FromSeconds(30);
});
builder.Services.AddHttpClient<IRegistrationService, RegistrationService>(client =>
{
    client.BaseAddress = new Uri(apiUrl);
    client.Timeout = TimeSpan.FromSeconds(30);
});

builder.Services.AddHttpClient<BookingApiService>(client =>
{
    client.BaseAddress = new Uri(apiUrl);
    client.Timeout = TimeSpan.FromSeconds(30);
});

builder.Services.AddHttpClient<PaymentApiService>(client =>
{
    client.BaseAddress = new Uri(apiUrl);
    client.Timeout = TimeSpan.FromSeconds(30);
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
