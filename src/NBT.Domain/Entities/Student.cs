using System.ComponentModel.DataAnnotations;
using NBT.Domain.Common;
using NBT.Domain.Enums;

namespace NBT.Domain.Entities;

/// <summary>
/// Represents a student registered for NBT tests.
/// Contains personal information, contact details, and academic background.
/// </summary>
public class Student : BaseEntity
{
    /// <summary>
    /// Gets or sets the student's NBT number (14 digits with Luhn checksum).
    /// This is auto-generated when the student is created.
    /// Format: YYYY + 9-digit sequence + check digit
    /// Example: 20240000000123
    /// </summary>
    [Required]
    [StringLength(14, MinimumLength = 14)]
    public string NBTNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the type of identification document (SA_ID, FOREIGN_ID, or PASSPORT).
    /// Determines validation rules for IDNumber.
    /// </summary>
    [Required]
    public IDType IDType { get; set; } = IDType.SA_ID;

    /// <summary>
    /// Gets or sets the student's identification number.
    /// - For SA_ID: 13-digit South African ID number with Luhn validation
    /// - For FOREIGN_ID: Foreign ID number (6-20 alphanumeric characters)
    /// - For PASSPORT: Passport number (6-20 alphanumeric characters)
    /// Example: 9001015009087 (SA_ID) or A1234567 (Foreign/Passport)
    /// </summary>
    [Required]
    [StringLength(20, MinimumLength = 6)]
    public string IDNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the student's nationality.
    /// Required for FOREIGN_ID and PASSPORT types.
    /// </summary>
    [StringLength(100)]
    public string? Nationality { get; set; }

    /// <summary>
    /// Gets or sets the student's country of origin.
    /// Required for FOREIGN_ID and PASSPORT types.
    /// </summary>
    [StringLength(100)]
    public string? CountryOfOrigin { get; set; }

    /// <summary>
    /// Gets or sets the student's first name.
    /// </summary>
    [Required]
    [StringLength(100)]
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the student's last name.
    /// </summary>
    [Required]
    [StringLength(100)]
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Gets the student's full name (computed property).
    /// </summary>
    public string FullName => $"{FirstName} {LastName}";

    /// <summary>
    /// Gets or sets the student's email address.
    /// Used for communication and login.
    /// </summary>
    [Required]
    [EmailAddress]
    [StringLength(255)]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the student's phone number.
    /// South African format expected.
    /// </summary>
    [Required]
    [Phone]
    [StringLength(20)]
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the student's date of birth.
    /// Extracted from ID number or provided manually.
    /// </summary>
    [Required]
    public DateTime DateOfBirth { get; set; }

    /// <summary>
    /// Gets or sets the student's gender.
    /// Values: "Male", "Female", "Other"
    /// Can be extracted from SA ID number.
    /// </summary>
    [Required]
    [StringLength(10)]
    public string Gender { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the student's age (calculated from date of birth).
    /// </summary>
    public int? Age { get; set; }

    /// <summary>
    /// Gets or sets the student's ethnicity.
    /// Values: "Black African", "Coloured", "Indian/Asian", "White", "Other", "Prefer not to say"
    /// </summary>
    [StringLength(50)]
    public string? Ethnicity { get; set; }

    /// <summary>
    /// Gets or sets the student's physical address.
    /// </summary>
    [StringLength(255)]
    public string? Address { get; set; }

    /// <summary>
    /// Gets or sets the student's city.
    /// </summary>
    [StringLength(100)]
    public string? City { get; set; }

    /// <summary>
    /// Gets or sets the student's province.
    /// </summary>
    [StringLength(100)]
    public string? Province { get; set; }

    /// <summary>
    /// Gets or sets the student's postal code.
    /// </summary>
    [StringLength(10)]
    public string? PostalCode { get; set; }

    /// <summary>
    /// Gets or sets the name of the student's school.
    /// </summary>
    [StringLength(255)]
    public string? SchoolName { get; set; }

    /// <summary>
    /// Gets or sets the student's current grade (10, 11, or 12).
    /// </summary>
    public int? Grade { get; set; }

    /// <summary>
    /// Gets or sets the student's home language.
    /// </summary>
    [StringLength(50)]
    public string? HomeLanguage { get; set; }

    /// <summary>
    /// Gets or sets any special accommodations required by the student.
    /// Examples: Extra time, wheelchair access, braille materials, etc.
    /// </summary>
    [StringLength(500)]
    public string? SpecialAccommodation { get; set; }

    // Survey fields
    /// <summary>
    /// Gets or sets the student's motivation for testing.
    /// </summary>
    [StringLength(1000)]
    public string? MotivationForTesting { get; set; }

    /// <summary>
    /// Gets or sets the student's career interests.
    /// </summary>
    [StringLength(500)]
    public string? CareerInterests { get; set; }

    /// <summary>
    /// Gets or sets the student's preferred study field.
    /// </summary>
    [StringLength(200)]
    public string? PreferredStudyField { get; set; }

    /// <summary>
    /// Gets or sets whether the student has access to a computer.
    /// </summary>
    public bool HasAccessToComputer { get; set; }

    /// <summary>
    /// Gets or sets whether the student has internet access.
    /// </summary>
    public bool HasInternetAccess { get; set; }

    /// <summary>
    /// Gets or sets any additional comments from the student.
    /// </summary>
    [StringLength(1000)]
    public string? AdditionalComments { get; set; }

    /// <summary>
    /// Gets or sets whether the student's account is active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    // Navigation properties

    /// <summary>
    /// Gets or sets the collection of registrations associated with this student.
    /// </summary>
    public virtual ICollection<Registration> Registrations { get; set; } = new List<Registration>();

    /// <summary>
    /// Gets or sets the collection of test results for this student.
    /// </summary>
    public virtual ICollection<TestResult> TestResults { get; set; } = new List<TestResult>();

    /// <summary>
    /// Gets or sets the collection of room allocations for this student.
    /// </summary>
    public virtual ICollection<RoomAllocation> RoomAllocations { get; set; } = new List<RoomAllocation>();
}
