namespace NBT.Domain.Enums;

/// <summary>
/// Represents user roles in the NBT system.
/// </summary>
public enum UserRole
{
    /// <summary>
    /// Applicant/Student applying for NBT tests.
    /// </summary>
    Applicant = 0,

    /// <summary>
    /// System administrator with full access.
    /// </summary>
    Admin = 1,

    /// <summary>
    /// NBT staff member with operational access.
    /// </summary>
    Staff = 2,

    /// <summary>
    /// User from a higher education institution with limited access.
    /// </summary>
    InstitutionalUser = 3,

    /// <summary>
    /// Super user with highest level access.
    /// </summary>
    SuperUser = 4
}
