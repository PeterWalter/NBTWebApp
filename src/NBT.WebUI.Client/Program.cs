using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.FluentUI.AspNetCore.Components;
using NBT.WebUI.Client;
using NBT.WebUI.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Configure HttpClient with API base address
var apiBaseAddress = builder.Configuration["ApiBaseUrl"] ?? "https://localhost:7001";
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiBaseAddress) });

// Register Fluent UI services
builder.Services.AddFluentUIComponents();

// Register application services
builder.Services.AddScoped<IRegistrationService, RegistrationService>();

await builder.Build().RunAsync();
