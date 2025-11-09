using NBT.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace NBT.Application.Staff.DTOs;

public class UpdateStaffDto
{
    [Required(ErrorMessage = "First name is required")]
    [StringLength(100, ErrorMessage = "First name cannot exceed 100 characters")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required")]
    [StringLength(100, ErrorMessage = "Last name cannot exceed 100 characters")]
    public string LastName { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Invalid phone number")]
    public string? PhoneNumber { get; set; }

    [Required(ErrorMessage = "Role is required")]
    public UserRole Role { get; set; }

    public string? InstitutionName { get; set; }
    public string? InstitutionId { get; set; }

    [Required(ErrorMessage = "Status is required")]
    public string Status { get; set; } = "Active";
}
