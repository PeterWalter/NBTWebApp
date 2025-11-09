using System.ComponentModel.DataAnnotations;

namespace NBT.Application.Bookings.DTOs;

/// <summary>
/// Request to update an existing booking
/// </summary>
public class UpdateBookingRequest
{
    [Required]
    public Guid Id { get; set; }

    public Guid? TestSessionId { get; set; }

    public List<string>? TestTypesSelected { get; set; }

    public bool? IsRemoteWriter { get; set; }

    [StringLength(255)]
    public string? RemoteLocation { get; set; }

    [StringLength(100)]
    public string? SpecialSessionType { get; set; }
}
