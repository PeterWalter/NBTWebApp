using System.ComponentModel.DataAnnotations;

namespace NBT.Application.Bookings.DTOs;

/// <summary>
/// Request to create a new booking/registration
/// </summary>
public class CreateBookingRequest
{
    [Required(ErrorMessage = "Student ID is required")]
    public Guid StudentId { get; set; }

    [Required(ErrorMessage = "Test Session ID is required")]
    public Guid TestSessionId { get; set; }

    [Required(ErrorMessage = "At least one test type must be selected")]
    [MinLength(1, ErrorMessage = "At least one test type must be selected")]
    public List<string> TestTypesSelected { get; set; } = new();

    public bool IsRemoteWriter { get; set; }

    [StringLength(255)]
    public string? RemoteLocation { get; set; }

    [StringLength(100)]
    public string? SpecialSessionType { get; set; }
}
