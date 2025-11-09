namespace NBT.Application.Bookings.DTOs;

/// <summary>
/// Data transfer object for booking/registration information
/// </summary>
public class BookingDto
{
    public Guid Id { get; set; }
    public string RegistrationNumber { get; set; } = string.Empty;
    public Guid StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string StudentNBTNumber { get; set; } = string.Empty;
    public Guid TestSessionId { get; set; }
    public string SessionName { get; set; } = string.Empty;
    public DateTime SessionDate { get; set; }
    public string VenueName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public List<string> TestTypesSelected { get; set; } = new();
    public bool IsRemoteWriter { get; set; }
    public string? RemoteLocation { get; set; }
    public string? SpecialSessionType { get; set; }
    public DateTime RegistrationDate { get; set; }
    public DateTime? ConfirmationDate { get; set; }
    public DateTime? CancellationDate { get; set; }
    public string? CancellationReason { get; set; }
    public PaymentInfoDto? Payment { get; set; }
}

/// <summary>
/// Payment information for a booking
/// </summary>
public class PaymentInfoDto
{
    public Guid Id { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public decimal AmountPaid { get; set; }
    public decimal Balance { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? EasyPayReference { get; set; }
    public DateTime? PaidDate { get; set; }
}
