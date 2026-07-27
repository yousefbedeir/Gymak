using System.Security.Claims;
using Gymak.Application.DTOs;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace Gymak.Web.Identity;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ProtectedLocalStorage _protectedLocalStorage;
    private LoggedInUserDto? _currentUser;
    private const string USER_SESSION_KEY = "Gymak_User_Session";

    public CustomAuthenticationStateProvider(ProtectedLocalStorage protectedLocalStorage)
    {
        _protectedLocalStorage = protectedLocalStorage;
    }

    public LoggedInUserDto? CurrentUser => _currentUser;

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            if (_currentUser == null)
            {
                var userSessionResult = await _protectedLocalStorage.GetAsync<LoggedInUserDto>(USER_SESSION_KEY);
                if (userSessionResult.Success && userSessionResult.Value != null)
                {
                    _currentUser = userSessionResult.Value;
                }
            }

            if (_currentUser != null)
            {
                var identity = CreateClaimsIdentity(_currentUser);
                var claimsPrincipal = new ClaimsPrincipal(identity);
                return new AuthenticationState(claimsPrincipal);
            }
        }
        catch
        {
            // Prerendering environment or storage error
        }

        var anonymousIdentity = new ClaimsIdentity();
        return new AuthenticationState(new ClaimsPrincipal(anonymousIdentity));
    }

    public async Task MarkUserAsAuthenticated(LoggedInUserDto user)
    {
        _currentUser = user;
        try
        {
            await _protectedLocalStorage.SetAsync(USER_SESSION_KEY, user);
        }
        catch
        {
            // Fallback for environments without localStorage support
        }

        var claimsPrincipal = new ClaimsPrincipal(CreateClaimsIdentity(user));
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
    }

    public async Task MarkUserAsLoggedOut()
    {
        _currentUser = null;
        try
        {
            await _protectedLocalStorage.DeleteAsync(USER_SESSION_KEY);
        }
        catch
        {
        }

        var anonymousIdentity = new ClaimsIdentity();
        var claimsPrincipal = new ClaimsPrincipal(anonymousIdentity);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
    }

    private static ClaimsIdentity CreateClaimsIdentity(LoggedInUserDto user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new(ClaimTypes.Name, user.FullName),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, user.Role.ToString()),
            new("IsPremium", user.IsPremium.ToString())
        };

        return new ClaimsIdentity(claims, "GymakAuth");
    }
}
