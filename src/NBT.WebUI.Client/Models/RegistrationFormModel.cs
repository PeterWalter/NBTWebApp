using System.ComponentModel.DataAnnotations;

namespace NBT.WebUI.Client.Models;

public class RegistrationFormModel
{
    // Step 1: ID Type and Number
    [Required(ErrorMessage = "ID Type is required")]
    public string IDType { get; set; } = "SA_ID";
    
    [Required(ErrorMessage = "ID Number is required")]
    [StringLength(20, MinimumLength = 6, ErrorMessage = "ID Number must be between 6 and 20 characters")]
    public string IDNumber { get; set; } = string.Empty;
    
    public string? Nationality { get; set; }
    
    public string? CountryOfOrigin { get; set; }
    
    // Step 2: Personal Information
    [Required(ErrorMessage = "First Name is required")]
    [StringLength(100, ErrorMessage = "First Name cannot exceed 100 characters")]
    public string FirstName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Last Name is required")]
    [StringLength(100, ErrorMessage = "Last Name cannot exceed 100 characters")]
    public string LastName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Date of Birth is required")]
    public DateTime? DateOfBirth { get; set; }
    
    [Required(ErrorMessage = "Gender is required")]
    public string Gender { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Ethnicity is required")]
    public string Ethnicity { get; set; } = string.Empty;
    
    // Step 3: Contact Information
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Phone Number is required")]
    [Phone(ErrorMessage = "Invalid phone number")]
    public string PhoneNumber { get; set; } = string.Empty;
    
    public string? AlternativePhoneNumber { get; set; }
    
    // Step 4: Address Information
    public string? AddressLine1 { get; set; }
    
    public string? AddressLine2 { get; set; }
    
    public string? City { get; set; }
    
    public string? Province { get; set; }
    
    public string? PostalCode { get; set; }
    
    public string? Country { get; set; } = "South Africa";
    
    // Step 5: Academic Information
    [Required(ErrorMessage = "School Name is required")]
    public string SchoolName { get; set; } = string.Empty;
    
    public string? SchoolProvince { get; set; }
    
    [Range(10, 12, ErrorMessage = "Grade must be between 10 and 12")]
    public int? GradeYear { get; set; }
    
    public string? HomeLanguage { get; set; }
    
    // Step 6: Survey Questions
    public string? MotivationForTesting { get; set; }
    public string? CareerInterests { get; set; }
    public string? PreferredStudyField { get; set; }
    public bool HasAccessToComputer { get; set; }
    public bool HasInternetAccess { get; set; }
    public string? AdditionalComments { get; set; }
    
    // Step 7: Special Accommodations
    public bool RequiresAccommodation { get; set; }
    
    public string? AccommodationDetails { get; set; }
    
    // Auto-generated on submission
    public string NBTNumber { get; set; } = string.Empty;
}
