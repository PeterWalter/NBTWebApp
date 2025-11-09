using NBT.Domain.Enums;

namespace NBT.Application.Staff.DTOs;

public class StaffDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public string Status { get; set; } = "Active";
    public string? InstitutionName { get; set; }
    public string? InstitutionId { get; set; }
    public DateTime? LastLoginDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool EmailConfirmed { get; set; }
    public bool PhoneNumberConfirmed { get; set; }
    public string FullName => $"{FirstName} {LastName}";
}
