namespace NBT.WebUI.Services;

public interface IAuthenticationService
{
    Task<AuthenticationResult> LoginAsync(string email, string password, bool rememberMe);
    Task LogoutAsync();
    Task<bool> IsAuthenticatedAsync();
    Task<UserInfo?> GetCurrentUserAsync();
    Task<string?> GetTokenAsync();
}

public class AuthenticationResult
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public string? Token { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? Expiration { get; set; }
    public UserInfo? User { get; set; }
}

public class UserInfo
{
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new();
}
