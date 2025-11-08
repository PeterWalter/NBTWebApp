using System.Text.Json.Serialization;

namespace NBT.Application.Students.DTOs;

public class StudentDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    
    [JsonPropertyName("nbtNumber")]
    public string NBTNumber { get; set; } = string.Empty;
    
    [JsonPropertyName("firstName")]
    public string FirstName { get; set; } = string.Empty;
    
    [JsonPropertyName("lastName")]
    public string LastName { get; set; } = string.Empty;
    
    [JsonPropertyName("idType")]
    public string IDType { get; set; } = "SA_ID"; // SA_ID, FOREIGN_ID, or PASSPORT
    
    [JsonPropertyName("idNumber")]
    public string IDNumber { get; set; } = string.Empty;
    
    [JsonPropertyName("nationality")]
    public string? Nationality { get; set; }
    
    [JsonPropertyName("countryOfOrigin")]
    public string? CountryOfOrigin { get; set; }
    
    [JsonPropertyName("dateOfBirth")]
    public DateTime DateOfBirth { get; set; }
    
    [JsonPropertyName("gender")]
    public string Gender { get; set; } = string.Empty;
    
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;
    
    [JsonPropertyName("phoneNumber")]
    public string PhoneNumber { get; set; } = string.Empty;
    
    [JsonPropertyName("alternativePhoneNumber")]
    public string? AlternativePhoneNumber { get; set; }
    
    [JsonPropertyName("addressLine1")]
    public string? AddressLine1 { get; set; }
    
    [JsonPropertyName("addressLine2")]
    public string? AddressLine2 { get; set; }
    
    [JsonPropertyName("city")]
    public string? City { get; set; }
    
    [JsonPropertyName("province")]
    public string? Province { get; set; }
    
    [JsonPropertyName("postalCode")]
    public string? PostalCode { get; set; }
    
    [JsonPropertyName("country")]
    public string? Country { get; set; }
    
    [JsonPropertyName("schoolName")]
    public string SchoolName { get; set; } = string.Empty;
    
    [JsonPropertyName("schoolProvince")]
    public string? SchoolProvince { get; set; }
    
    [JsonPropertyName("gradeYear")]
    public int? GradeYear { get; set; }
    
    [JsonPropertyName("requiresAccommodation")]
    public bool RequiresAccommodation { get; set; }
    
    [JsonPropertyName("accommodationDetails")]
    public string? AccommodationDetails { get; set; }
    
    [JsonPropertyName("isActive")]
    public bool IsActive { get; set; }
    
    [JsonPropertyName("createdDate")]
    public DateTime CreatedDate { get; set; }
    
    [JsonPropertyName("createdBy")]
    public string? CreatedBy { get; set; }
    
    [JsonPropertyName("lastModifiedDate")]
    public DateTime? LastModifiedDate { get; set; }
    
    [JsonPropertyName("lastModifiedBy")]
    public string? LastModifiedBy { get; set; }
}

public class CreateStudentDto
{
    [JsonPropertyName("firstName")]
    public string FirstName { get; set; } = string.Empty;
    
    [JsonPropertyName("lastName")]
    public string LastName { get; set; } = string.Empty;
    
    [JsonPropertyName("idType")]
    public string IDType { get; set; } = "SA_ID"; // SA_ID, FOREIGN_ID, or PASSPORT
    
    [JsonPropertyName("idNumber")]
    public string IDNumber { get; set; } = string.Empty;
    
    [JsonPropertyName("nationality")]
    public string? Nationality { get; set; }
    
    [JsonPropertyName("countryOfOrigin")]
    public string? CountryOfOrigin { get; set; }
    
    [JsonPropertyName("dateOfBirth")]
    public DateTime DateOfBirth { get; set; }
    
    [JsonPropertyName("gender")]
    public string Gender { get; set; } = string.Empty;
    
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;
    
    [JsonPropertyName("phoneNumber")]
    public string PhoneNumber { get; set; } = string.Empty;
    
    [JsonPropertyName("alternativePhoneNumber")]
    public string? AlternativePhoneNumber { get; set; }
    
    [JsonPropertyName("addressLine1")]
    public string? AddressLine1 { get; set; }
    
    [JsonPropertyName("addressLine2")]
    public string? AddressLine2 { get; set; }
    
    [JsonPropertyName("city")]
    public string? City { get; set; }
    
    [JsonPropertyName("province")]
    public string? Province { get; set; }
    
    [JsonPropertyName("postalCode")]
    public string? PostalCode { get; set; }
    
    [JsonPropertyName("country")]
    public string? Country { get; set; }
    
    [JsonPropertyName("schoolName")]
    public string SchoolName { get; set; } = string.Empty;
    
    [JsonPropertyName("schoolProvince")]
    public string? SchoolProvince { get; set; }
    
    [JsonPropertyName("gradeYear")]
    public int? GradeYear { get; set; }
    
    [JsonPropertyName("requiresAccommodation")]
    public bool RequiresAccommodation { get; set; }
    
    [JsonPropertyName("accommodationDetails")]
    public string? AccommodationDetails { get; set; }
}

public class UpdateStudentDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    
    [JsonPropertyName("firstName")]
    public string FirstName { get; set; } = string.Empty;
    
    [JsonPropertyName("lastName")]
    public string LastName { get; set; } = string.Empty;
    
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;
    
    [JsonPropertyName("phoneNumber")]
    public string PhoneNumber { get; set; } = string.Empty;
    
    [JsonPropertyName("alternativePhoneNumber")]
    public string? AlternativePhoneNumber { get; set; }
    
    [JsonPropertyName("addressLine1")]
    public string? AddressLine1 { get; set; }
    
    [JsonPropertyName("addressLine2")]
    public string? AddressLine2 { get; set; }
    
    [JsonPropertyName("city")]
    public string? City { get; set; }
    
    [JsonPropertyName("province")]
    public string? Province { get; set; }
    
    [JsonPropertyName("postalCode")]
    public string? PostalCode { get; set; }
    
    [JsonPropertyName("country")]
    public string? Country { get; set; }
    
    [JsonPropertyName("schoolName")]
    public string SchoolName { get; set; } = string.Empty;
    
    [JsonPropertyName("schoolProvince")]
    public string? SchoolProvince { get; set; }
    
    [JsonPropertyName("gradeYear")]
    public int? GradeYear { get; set; }
    
    [JsonPropertyName("requiresAccommodation")]
    public bool RequiresAccommodation { get; set; }
    
    [JsonPropertyName("accommodationDetails")]
    public string? AccommodationDetails { get; set; }
    
    [JsonPropertyName("isActive")]
    public bool IsActive { get; set; }
}
