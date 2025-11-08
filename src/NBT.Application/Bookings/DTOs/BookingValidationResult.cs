namespace NBT.Application.Bookings.DTOs;

/// <summary>
/// Result of booking validation check.
/// </summary>
public class BookingValidationResult
{
    /// <summary>
    /// Gets or sets whether the booking is valid.
    /// </summary>
    public bool IsValid { get; set; }
    
    /// <summary>
    /// Gets or sets the error message if validation failed.
    /// </summary>
    public string? ErrorMessage { get; set; }
    
    /// <summary>
    /// Gets or sets the error code for client-side handling.
    /// </summary>
    public string? ErrorCode { get; set; }
    
    /// <summary>
    /// Creates a successful validation result.
    /// </summary>
    public static BookingValidationResult Success() => new() { IsValid = true };
    
    /// <summary>
    /// Creates a failed validation result with error message.
    /// </summary>
    public static BookingValidationResult Failure(string errorCode, string errorMessage)
        => new() { IsValid = false, ErrorCode = errorCode, ErrorMessage = errorMessage };
}
