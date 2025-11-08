using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace NBT.WebUI.Services;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ClaimsPrincipal _anonymous = new(new ClaimsIdentity());
    private readonly IAuthenticationService _authService;

    public CustomAuthenticationStateProvider(IAuthenticationService authService)
    {
        _authService = authService;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var isAuthenticated = await _authService.IsAuthenticatedAsync();
            
            if (isAuthenticated)
            {
                var user = await _authService.GetCurrentUserAsync();
                
                if (user != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.GivenName, user.FirstName),
                        new Claim(ClaimTypes.Surname, user.LastName),
                        new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}")
                    };

                    foreach (var role in user.Roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }

                    var identity = new ClaimsIdentity(claims, "jwt");
                    var claimsPrincipal = new ClaimsPrincipal(identity);
                    
                    return new AuthenticationState(claimsPrincipal);
                }
            }
        }
        catch
        {
            // If anything goes wrong, return anonymous
        }

        return new AuthenticationState(_anonymous);
    }

    public void NotifyAuthenticationStateChanged()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}
