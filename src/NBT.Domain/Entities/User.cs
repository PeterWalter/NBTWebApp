using Microsoft.AspNetCore.Identity;
using NBT.Domain.Enums;

namespace NBT.Domain.Entities;

/// <summary>
/// Represents an application user (extends ASP.NET Core Identity).
/// </summary>
public class User : IdentityUser<Guid>
{
    /// <summary>
    /// Gets or sets the user's first name.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user's last name.
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user's role.
    /// </summary>
    public UserRole Role { get; set; }

    /// <summary>
    /// Gets or sets the institution name (for institutional users).
    /// </summary>
    public string? InstitutionName { get; set; }

    /// <summary>
    /// Gets or sets the institution ID (for institutional users).
    /// </summary>
    public string? InstitutionId { get; set; }

    /// <summary>
    /// Gets or sets the user account status (Active, Inactive, Locked).
    /// </summary>
    public string Status { get; set; } = "Active";

    /// <summary>
    /// Gets or sets the date of the last successful login.
    /// </summary>
    public DateTime? LastLoginDate { get; set; }

    /// <summary>
    /// Gets or sets the date when the user account was created.
    /// </summary>
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the password reset token (for forgot password functionality).
    /// </summary>
    public string? PasswordResetToken { get; set; }

    /// <summary>
    /// Gets or sets the expiry date for the password reset token.
    /// </summary>
    public DateTime? TokenExpiryDate { get; set; }

    /// <summary>
    /// Gets or sets the refresh token for JWT authentication.
    /// </summary>
    public string? RefreshToken { get; set; }

    /// <summary>
    /// Gets or sets the expiry date for the refresh token.
    /// </summary>
    public DateTime? RefreshTokenExpiry { get; set; }
}
