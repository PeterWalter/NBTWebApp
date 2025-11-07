namespace NBT.Application.Common.Interfaces;

/// <summary>
/// Interface for accessing current user information.
/// </summary>
public interface ICurrentUserService
{
    /// <summary>
    /// Gets the current user's ID.
    /// </summary>
    Guid? UserId { get; }

    /// <summary>
    /// Gets the current user's username.
    /// </summary>
    string? UserName { get; }

    /// <summary>
    /// Gets whether the user is authenticated.
    /// </summary>
    bool IsAuthenticated { get; }

    /// <summary>
    /// Checks if the current user has a specific role.
    /// </summary>
    /// <param name="role">Role name.</param>
    /// <returns>True if user has the role.</returns>
    bool IsInRole(string role);
}
