using Microsoft.FluentUI.AspNetCore.Components;
using NBT.WebUI.Components;
using NBT.WebUI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add Fluent UI
builder.Services.AddFluentUIComponents();

// Add HTTP Client for API calls
builder.Services.AddHttpClient("NBT.WebAPI", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"] ?? "http://localhost:5046/");
});

// Register application services
builder.Services.AddScoped<IContentPageService, ContentPageService>();
builder.Services.AddScoped<IAnnouncementService, AnnouncementService>();
builder.Services.AddScoped<IResourceService, ResourceService>();
builder.Services.AddScoped<IContactInquiryService, ContactInquiryService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
