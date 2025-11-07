namespace NBT.Application.Authentication.DTOs;

/// <summary>
/// Data transfer object for authentication result.
/// </summary>
public class AuthenticationResult
{
    public bool Succeeded { get; set; }
    public string? Token { get; set; }
    public DateTime? Expiration { get; set; }
    public string? Error { get; set; }
    public List<string> Errors { get; set; } = new();
    
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}
