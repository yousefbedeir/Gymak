using Gymak.Application;
using Gymak.Application.Common.Interfaces;
using Gymak.Infrastructure;
using Gymak.Infrastructure.Persistence;
using Gymak.Web.Components;
using Gymak.Web.Identity;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

// =========================================================================
// 1. Clean Architecture Service Registrations
// =========================================================================
builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration);

// =========================================================================
// 2. Authentication & Session Services
// =========================================================================
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider =>
    provider.GetRequiredService<CustomAuthenticationStateProvider>());
builder.Services.AddAuthenticationCore();
builder.Services.AddAuthorizationCore();

// =========================================================================
// 3. UI & Web Framework Registrations
// =========================================================================
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Seed Database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
    await DbInitializer.SeedAsync(context, passwordHasher);
}

// =========================================================================
// 4. HTTP Request Pipeline Configuration
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
