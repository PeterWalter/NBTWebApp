using System.ComponentModel.DataAnnotations;

namespace NBT.Application.Venues.DTOs;

public class VenueDto
{
    public Guid Id { get; set; }
    
    [Required(ErrorMessage = "Venue name is required")]
    [StringLength(255, ErrorMessage = "Venue name cannot exceed 255 characters")]
    public string VenueName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Venue code is required")]
    [StringLength(20, ErrorMessage = "Venue code cannot exceed 20 characters")]
    public string VenueCode { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Address is required")]
    [StringLength(255, ErrorMessage = "Address cannot exceed 255 characters")]
    public string Address { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "City is required")]
    [StringLength(100, ErrorMessage = "City cannot exceed 100 characters")]
    public string City { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Province is required")]
    [StringLength(100, ErrorMessage = "Province cannot exceed 100 characters")]
    public string Province { get; set; } = string.Empty;
    
    [StringLength(10, ErrorMessage = "Postal code cannot exceed 10 characters")]
    public string? PostalCode { get; set; }
    
    [StringLength(200, ErrorMessage = "Contact person cannot exceed 200 characters")]
    public string? ContactPerson { get; set; }
    
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [StringLength(255, ErrorMessage = "Email cannot exceed 255 characters")]
    public string? ContactEmail { get; set; }
    
    [Phone(ErrorMessage = "Invalid phone format")]
    [StringLength(20, ErrorMessage = "Phone cannot exceed 20 characters")]
    public string? ContactPhone { get; set; }
    
    public int TotalCapacity { get; set; }
    
    public bool IsAccessible { get; set; } = true;
    
    [Required]
    [StringLength(50)]
    public string Status { get; set; } = "Active";
    
    [StringLength(1000, ErrorMessage = "Notes cannot exceed 1000 characters")]
    public string? Notes { get; set; }
    
    public int RoomCount { get; set; }
    
    public DateTime CreatedDate { get; set; }
    public DateTime? LastModifiedDate { get; set; }
}
