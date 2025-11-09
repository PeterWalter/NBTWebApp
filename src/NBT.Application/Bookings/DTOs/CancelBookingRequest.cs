using System.ComponentModel.DataAnnotations;

namespace NBT.Application.Bookings.DTOs;

/// <summary>
/// Request to cancel a booking
/// </summary>
public class CancelBookingRequest
{
    [Required]
    public Guid BookingId { get; set; }

    [Required(ErrorMessage = "Cancellation reason is required")]
    [StringLength(500, MinimumLength = 10, ErrorMessage = "Please provide a reason between 10 and 500 characters")]
    public string Reason { get; set; } = string.Empty;
}
