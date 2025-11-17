using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.FluentUI.AspNetCore.Components;
using NBT.WebUI.Components;
using NBT.WebUI.Services;
using NBT.WebUI.Services.Bookings;
using System.Net.Http;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddRazorComponents()
 .AddInteractiveServerComponents();

// Configure Blazor Hub connection with extended timeouts
builder.Services.AddServerSideBlazor(options =>
{
 options.DetailedErrors = builder.Environment.IsDevelopment();
 options.DisconnectedCircuitMaxRetained =100;
 options.DisconnectedCircuitRetentionPeriod = TimeSpan.FromMinutes(5);
 options.JSInteropDefaultCallTimeout = TimeSpan.FromMinutes(2);
 options.MaxBufferedUnacknowledgedRenderBatches =20;
})
.AddHubOptions(options =>
{
 options.ClientTimeoutInterval = TimeSpan.FromMinutes(1);
 options.HandshakeTimeout = TimeSpan.FromSeconds(30);
 options.KeepAliveInterval = TimeSpan.FromSeconds(15);
 options.MaximumReceiveMessageSize =128 *1024;
});

// Add Fluent UI
builder.Services.AddFluentUIComponents();

// Add Authentication and Authorization
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();

// Read API base url from configuration
var apiUrl = builder.Configuration["ApiBaseUrl"];
var logger = LoggerFactory.Create(b => b.AddConsole()).CreateLogger("Program");
if (string.IsNullOrWhiteSpace(apiUrl))
{
 logger.LogWarning("ApiBaseUrl not configured. HTTP clients will throw a helpful error if used. Set 'ApiBaseUrl' in appsettings or environment variables (e.g. https://localhost:7001/).");
}

// Expose API options to the app
var apiOptions = new ApiOptions
{
 BaseUrl = apiUrl,
 IsConfigured = !string.IsNullOrWhiteSpace(apiUrl)
};
builder.Services.AddSingleton(apiOptions);

// Register a handler that throws a helpful error when backend is unavailable or not configured
builder.Services.AddTransient<BackendUnavailableHandler>(sp => new BackendUnavailableHandler(
 apiOptions.IsConfigured
 ? $"Unable to contact backend at {apiOptions.BaseUrl}. Ensure the API is running and the address is correct."
 : "Backend API base URL is not configured. Set 'ApiBaseUrl' in appsettings or environment variables (e.g. https://localhost:7001/)."));

// Register HttpClient factory for non-service usage
builder.Services.AddHttpClient();

// Add Authentication Service
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

// Configure each API client to either use the configured BaseAddress or the BackendUnavailableHandler
void AddApiHttpClient<TService, TImplementation>(string? name = null)
 where TService : class
 where TImplementation : class, TService
{
 var clientBuilder = builder.Services.AddHttpClient<TService, TImplementation>();
 if (apiOptions.IsConfigured)
 {
 clientBuilder.ConfigureHttpClient(c =>
 {
 c.BaseAddress = new Uri(apiOptions.BaseUrl!);
 c.Timeout = TimeSpan.FromSeconds(30);
 });
 }
 else
 {
 // Any attempt to send a request will go through the handler which throws a helpful exception
 clientBuilder.AddHttpMessageHandler<BackendUnavailableHandler>();
 }
}

// Register specific services
AddApiHttpClient<IAuthenticationService, AuthenticationService>();
AddApiHttpClient<IContentPageService, ContentPageService>();
AddApiHttpClient<IAnnouncementService, AnnouncementService>();
AddApiHttpClient<IResourceService, ResourceService>();
AddApiHttpClient<IContactInquiryService, ContactInquiryService>();
AddApiHttpClient<IRegistrationService, RegistrationService>();

// For typed non-interface services
{
 var b = builder.Services.AddHttpClient<BookingApiService>();
 if (apiOptions.IsConfigured)
 {
 b.ConfigureHttpClient(c => { c.BaseAddress = new Uri(apiOptions.BaseUrl!); c.Timeout = TimeSpan.FromSeconds(30); });
 }
 else
 {
 b.AddHttpMessageHandler<BackendUnavailableHandler>();
 }
}

{
 var b = builder.Services.AddHttpClient<PaymentApiService>();
 if (apiOptions.IsConfigured)
 {
 b.ConfigureHttpClient(c => { c.BaseAddress = new Uri(apiOptions.BaseUrl!); c.Timeout = TimeSpan.FromSeconds(30); });
 }
 else
 {
 b.AddHttpMessageHandler<BackendUnavailableHandler>();
 }
}

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
 app.UseExceptionHandler("/Error");
 app.UseHsts();
}
else
{
 // Show detailed errors when developing
 app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();

// NOTE: removed app.UseAntiforgery() — that middleware does not exist.
// If you need antiforgery, call services.AddAntiforgery() and
// use the antiforgery token where required from components/handlers.

app.MapRazorComponents<App>()
 .AddInteractiveServerRenderMode();

app.Run();

// Helper types
public class ApiOptions
{
 public string? BaseUrl { get; set; }
 public bool IsConfigured { get; set; }
}

public class BackendUnavailableHandler : DelegatingHandler
{
 private readonly string _message;
 public BackendUnavailableHandler(string message)
 {
 _message = message;
 }

 protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
 {
 throw new HttpRequestException(_message);
 }
}
