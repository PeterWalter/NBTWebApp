using System.ComponentModel.DataAnnotations;

namespace NBT.Application.Venues.DTOs;

public class RoomDto
{
    public Guid Id { get; set; }
    
    [Required(ErrorMessage = "Venue is required")]
    public Guid VenueId { get; set; }
    
    public string? VenueName { get; set; }
    
    [Required(ErrorMessage = "Room name is required")]
    [StringLength(255, ErrorMessage = "Room name cannot exceed 255 characters")]
    public string RoomName { get; set; } = string.Empty;
    
    [StringLength(50, ErrorMessage = "Room number cannot exceed 50 characters")]
    public string? RoomNumber { get; set; }
    
    [Required(ErrorMessage = "Capacity is required")]
    [Range(1, 1000, ErrorMessage = "Capacity must be between 1 and 1000")]
    public int Capacity { get; set; }
    
    [Required(ErrorMessage = "Room type is required")]
    [StringLength(50)]
    public string RoomType { get; set; } = "ComputerLab";
    
    public bool HasComputers { get; set; } = false;
    
    [Range(0, 1000, ErrorMessage = "Computer count must be between 0 and 1000")]
    public int? ComputerCount { get; set; }
    
    public bool IsAccessible { get; set; } = true;
    
    [Required]
    [StringLength(50)]
    public string Status { get; set; } = "Available";
    
    [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters")]
    public string? Notes { get; set; }
    
    public DateTime CreatedDate { get; set; }
    public DateTime? LastModifiedDate { get; set; }
}
