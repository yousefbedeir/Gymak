using Gymak.Application;
using Gymak.Infrastructure;
using Gymak.Web.Components;

var builder = WebApplication.CreateBuilder(args);

// =========================================================================
// 1. Clean Architecture Service Registrations
// =========================================================================
builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration);

// =========================================================================
// 2. UI & Web Framework Registrations
// =========================================================================
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// =========================================================================
// 3. HTTP Request Pipeline Configuration
// =========================================================================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
