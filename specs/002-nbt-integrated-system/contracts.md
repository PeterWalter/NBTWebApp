# Data Contracts & API Schemas - NBT Integrated System

**Feature**: 002-nbt-integrated-system  
**Version**: 1.0  
**Created**: 2025-11-08  
**Status**: APPROVED

---

## EXECUTIVE SUMMARY

This document defines **all data contracts, API endpoints, domain entities, DTOs, and validation rules** for the NBT Integrated Web Application. It extends the existing shell project with 8 new core entities and 40+ API endpoints to support:

✅ **Student registration workflow** with NBT number generation (Luhn algorithm)  
✅ **Multi-step booking wizard** with test session selection  
✅ **Payment integration** with EasyPay gateway  
✅ **Test result imports** via Excel with validation  
✅ **Venue and room management** with capacity tracking  
✅ **Staff/Admin dashboards** with full CRUD operations  
✅ **Reporting and analytics** (Excel/PDF exports)  
✅ **Role-based security** (Staff, Admin, SuperUser)

---

## 1. MISSING CORE ENTITIES

### Current State (Existing)
- ✅ User (authentication)
- ✅ Announcement  
- ✅ ContactInquiry  
- ✅ ContentPage  
- ✅ DownloadableResource  
- ✅ SystemSetting

### Required New Entities
- ❌ **Student** - Student applicants with NBT numbers
- ❌ **Registration** - Test session registrations
- ❌ **Payment** - Payment transactions (EasyPay)
- ❌ **TestSession** - Scheduled test sessions
- ❌ **Venue** - Physical test venues
- ❌ **Room** - Rooms within venues
- ❌ **RoomAllocation** - Room assignments to sessions
- ❌ **TestResult** - Student test results
- ❌ **AuditLog** - Full audit trail

---

## 2. DOMAIN ENTITIES (Full Definitions)

### 2.1 Student Entity

**Location**: `src/NBT.Domain/Entities/Student.cs`

```csharp
namespace NBT.Domain.Entities;

/// <summary>
/// Represents a student applicant in the NBT system.
/// </summary>
public class Student : BaseEntity, IAuditableEntity
{
    /// <summary>
    /// NBT number (14 digits with Luhn checksum). Format: YYYYSSSSSSSSSC
    /// YYYY = Year, SSSSSSSSS = 9-digit Sequence, C = Checksum
    /// </summary>
    [Required, StringLength(14, MinimumLength = 14)]
    public string NBTNumber { get; set; } = string.Empty;
    
    /// <summary>
    /// South African ID number (13 digits with Luhn validation).
    /// </summary>
    [Required, StringLength(13, MinimumLength = 13)]
    public string IDNumber { get; set; } = string.Empty;
    
    [Required, StringLength(100)]
    public string FirstName { get; set; } = string.Empty;
    
    [Required, StringLength(100)]
    public string LastName { get; set; } = string.Empty;
    
    /// <summary>
    /// Computed from ID number (YYMMDD portion).
    /// </summary>
    public DateTime DateOfBirth { get; set; }
    
    /// <summary>
    /// Gender derived from ID number: M/F
    /// </summary>
    [StringLength(1)]
    public string Gender { get; set; } = string.Empty;
    
    [Required, EmailAddress, StringLength(255)]
    public string Email { get; set; } = string.Empty;
    
    [Required, Phone, StringLength(20)]
    public string Phone { get; set; } = string.Empty;
    
    [StringLength(500)]
    public string Address { get; set; } = string.Empty;
    
    [StringLength(100)]
    public string City { get; set; } = string.Empty;
    
    [StringLength(100)]
    public string Province { get; set; } = string.Empty;
    
    [StringLength(10)]
    public string PostalCode { get; set; } = string.Empty;
    
    [Required, StringLength(200)]
    public string SchoolName { get; set; } = string.Empty;
    
    [Range(10, 12)]
    public int? Grade { get; set; }
    
    [StringLength(50)]
    public string HomeLanguage { get; set; } = string.Empty;
    
    /// <summary>
    /// Special accommodations (e.g., extra time, scribe).
    /// </summary>
    [StringLength(500)]
    public string? SpecialAccommodation { get; set; }
    
    // IAuditableEntity implementation
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime? UpdatedDate { get; set; }
    public string? UpdatedBy { get; set; }
    
    // Navigation properties
    public ICollection<Registration> Registrations { get; set; } = new List<Registration>();
    public ICollection<TestResult> TestResults { get; set; } = new List<TestResult>();
}
```

**EF Core Configuration**: `src/NBT.Infrastructure/Persistence/Configurations/StudentConfiguration.cs`

```csharp
public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.ToTable("Students");
        
        builder.HasKey(s => s.Id);
        
        // Unique constraints
        builder.HasIndex(s => s.NBTNumber).IsUnique();
        builder.HasIndex(s => s.IDNumber).IsUnique();
        builder.HasIndex(s => s.Email);
        
        // Relationships
        builder.HasMany(s => s.Registrations)
            .WithOne(r => r.Student)
            .HasForeignKey(r => r.StudentId)
            .OnDelete(DeleteBehavior.Restrict);
            
        builder.HasMany(s => s.TestResults)
            .WithOne(tr => tr.Student)
            .HasForeignKey(tr => tr.StudentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
```

### 2.2 Registration Entity

```csharp
namespace NBT.Domain.Entities;

public class Registration : BaseEntity, IAuditableEntity
{
    public Guid StudentId { get; set; }
    public Student Student { get; set; } = null!;
    
    public Guid TestSessionId { get; set; }
    public TestSession TestSession { get; set; } = null!;
    
    /// <summary>
    /// Format: REG-YYYY-NNNNNN (e.g., REG-2025-000001)
    /// </summary>
    [Required, StringLength(20)]
    public string RegistrationNumber { get; set; } = string.Empty;
    
    public RegistrationStatus Status { get; set; } = RegistrationStatus.Pending;
    
    /// <summary>
    /// JSON array: ["AQL", "MAT"] (AQL = Academic & Quantitative Literacy, MAT = Mathematics)
    /// </summary>
    [Required]
    public string TestTypesSelected { get; set; } = string.Empty;
    
    /// <summary>
    /// Booking close date (student can modify until this date)
    /// </summary>
    [Required]
    public DateTime BookingCloseDate { get; set; }
    
    /// <summary>
    /// Test validity expiration (3 years from booking date)
    /// </summary>
    [Required]
    public DateTime ValidUntilDate { get; set; }
    
    public bool IsRemoteWriter { get; set; }
    
    [StringLength(200)]
    public string? RemoteLocation { get; set; }
    
    [StringLength(100)]
    public string? SpecialSessionType { get; set; }
    
    public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
    public DateTime? ConfirmationDate { get; set; }
    public DateTime? CancellationDate { get; set; }
    
    [StringLength(500)]
    public string? CancellationReason { get; set; }
    
    // Audit fields
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime? UpdatedDate { get; set; }
    public string? UpdatedBy { get; set; }
    
    // Navigation
    public Payment? Payment { get; set; }
}
```

### 2.3 Payment Entity

```csharp
namespace NBT.Domain.Entities;

public class Payment : BaseEntity, IAuditableEntity
{
    public Guid RegistrationId { get; set; }
    public Registration Registration { get; set; } = null!;
    
    /// <summary>
    /// Format: INV-YYYY-NNNNNN
    /// </summary>
    [Required, StringLength(20)]
    public string InvoiceNumber { get; set; } = string.Empty;
    
    [Required, Range(0, 100000)]
    public decimal Amount { get; set; }
    
    /// <summary>
    /// EasyPay, Cash, EFT, Card
    /// </summary>
    [Required, StringLength(50)]
    public string PaymentMethod { get; set; } = "EasyPay";
    
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
    
    /// <summary>
    /// EasyPay merchant reference (unique per payment).
    /// </summary>
    [StringLength(100)]
    public string? EasyPayReference { get; set; }
    
    /// <summary>
    /// EasyPay transaction ID (returned after successful payment).
    /// </summary>
    [StringLength(100)]
    public string? EasyPayTransactionId { get; set; }
    
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? PaidDate { get; set; }
    public DateTime? RefundedDate { get; set; }
    
    [StringLength(500)]
    public string? RefundReason { get; set; }
    
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime? UpdatedDate { get; set; }
    public string? UpdatedBy { get; set; }
}
```

### 2.4 TestSession Entity

```csharp
namespace NBT.Domain.Entities;

public class TestSession : BaseEntity, IAuditableEntity
{
    /// <summary>
    /// Format: CITY-YYYY-MM-DD-PERIOD (e.g., JHB-2025-03-15-AM)
    /// </summary>
    [Required, StringLength(50)]
    public string SessionCode { get; set; } = string.Empty;
    
    [Required, StringLength(200)]
    public string SessionName { get; set; } = string.Empty;
    
    [Required]
    public DateTime SessionDate { get; set; }
    
    [Required]
    public TimeSpan StartTime { get; set; }
    
    [Required]
    public TimeSpan EndTime { get; set; }
    
    public Guid VenueId { get; set; }
    public Venue Venue { get; set; } = null!;
    
    [Required, Range(1, 10000)]
    public int Capacity { get; set; }
    
    [Required, Range(0, 10000)]
    public int CurrentRegistrations { get; set; }
    
    /// <summary>
    /// Computed: Capacity - CurrentRegistrations
    /// </summary>
    public int AvailableSeats => Capacity - CurrentRegistrations;
    
    public SessionStatus Status { get; set; } = SessionStatus.Open;
    
    public bool IsSpecialSession { get; set; }
    
    [StringLength(1000)]
    public string? SpecialSessionNotes { get; set; }
    
    // Audit
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime? UpdatedDate { get; set; }
    public string? UpdatedBy { get; set; }
    
    // Navigation
    public ICollection<Registration> Registrations { get; set; } = new List<Registration>();
    public ICollection<RoomAllocation> RoomAllocations { get; set; } = new List<RoomAllocation>();
}
```

### 2.5 Venue Entity

```csharp
namespace NBT.Domain.Entities;

public class Venue : BaseEntity, IAuditableEntity
{
    [Required, StringLength(200)]
    public string VenueName { get; set; } = string.Empty;
    
    /// <summary>
    /// Short code (e.g., JHB-WITS, CPT-UCT)
    /// </summary>
    [Required, StringLength(20)]
    public string VenueCode { get; set; } = string.Empty;
    
    [Required, StringLength(500)]
    public string Address { get; set; } = string.Empty;
    
    [Required, StringLength(100)]
    public string City { get; set; } = string.Empty;
    
    [Required, StringLength(100)]
    public string Province { get; set; } = string.Empty;
    
    [Required, StringLength(10)]
    public string PostalCode { get; set; } = string.Empty;
    
    [Required, StringLength(100)]
    public string ContactPerson { get; set; } = string.Empty;
    
    [Required, EmailAddress, StringLength(255)]
    public string ContactEmail { get; set; } = string.Empty;
    
    [Required, Phone, StringLength(20)]
    public string ContactPhone { get; set; } = string.Empty;
    
    [Required, Range(0, 50000)]
    public int TotalCapacity { get; set; }
    
    public bool IsAccessible { get; set; }
    
    [Required, StringLength(20)]
    public string Status { get; set; } = "Active"; // Active, Inactive, UnderMaintenance
    
    [StringLength(1000)]
    public string? Notes { get; set; }
    
    // Audit
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime? UpdatedDate { get; set; }
    public string? UpdatedBy { get; set; }
    
    // Navigation
    public ICollection<Room> Rooms { get; set; } = new List<Room>();
    public ICollection<TestSession> TestSessions { get; set; } = new List<TestSession>();
}
```

### 2.6 Room Entity

```csharp
namespace NBT.Domain.Entities;

public class Room : BaseEntity
{
    public Guid VenueId { get; set; }
    public Venue Venue { get; set; } = null!;
    
    [Required, StringLength(100)]
    public string RoomName { get; set; } = string.Empty;
    
    [Required, StringLength(20)]
    public string RoomNumber { get; set; } = string.Empty;
    
    [Required, Range(1, 1000)]
    public int Capacity { get; set; }
    
    [Required, StringLength(50)]
    public string RoomType { get; set; } = string.Empty; // ComputerLab, Classroom, Hall, ExamRoom
    
    public bool HasComputers { get; set; }
    
    [Range(0, 1000)]
    public int? ComputerCount { get; set; }
    
    public bool IsAccessible { get; set; }
    
    [Required, StringLength(20)]
    public string Status { get; set; } = "Available"; // Available, Unavailable, UnderMaintenance
    
    [StringLength(500)]
    public string? Notes { get; set; }
    
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedDate { get; set; }
    
    // Navigation
    public ICollection<RoomAllocation> RoomAllocations { get; set; } = new List<RoomAllocation>();
}
```

### 2.7 RoomAllocation Entity

```csharp
namespace NBT.Domain.Entities;

public class RoomAllocation : BaseEntity
{
    public Guid TestSessionId { get; set; }
    public TestSession TestSession { get; set; } = null!;
    
    public Guid RoomId { get; set; }
    public Room Room { get; set; } = null!;
    
    [Required, Range(0, 1000)]
    public int AllocatedStudents { get; set; }
    
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public string CreatedBy { get; set; } = string.Empty;
}
```

### 2.8 TestResult Entity

```csharp
namespace NBT.Domain.Entities;

public class TestResult : BaseEntity, IAuditableEntity
{
    public Guid StudentId { get; set; }
    public Student Student { get; set; } = null!;
    
    public Guid TestSessionId { get; set; }
    public TestSession TestSession { get; set; } = null!;
    
    [Required, StringLength(50)]
    public string TestType { get; set; } = string.Empty; // AcademicLiteracy, QuantitativeLiteracy, Mathematics
    
    [Required, Range(0, 100)]
    public decimal RawScore { get; set; }
    
    [Required, Range(1, 99)]
    public int Percentile { get; set; }
    
    [Required, StringLength(50)]
    public string PerformanceBand { get; set; } = string.Empty; // Elementary, Basic, Intermediate, Proficient
    
    public bool IsReleased { get; set; }
    
    [Required]
    public DateTime TestDate { get; set; }
    
    [Required]
    public DateTime ResultDate { get; set; }
    
    public DateTime? ReleasedDate { get; set; }
    
    // Audit
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime? UpdatedDate { get; set; }
    public string? UpdatedBy { get; set; }
}
```

### 2.9 AuditLog Entity

```csharp
namespace NBT.Domain.Entities;

/// <summary>
/// Immutable audit log for all CRUD operations.
/// </summary>
public class AuditLog : BaseEntity
{
    [Required]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    
    [Required, StringLength(100)]
    public string UserId { get; set; } = string.Empty;
    
    [Required, StringLength(255)]
    public string UserEmail { get; set; } = string.Empty;
    
    [Required, StringLength(50)]
    public string Action { get; set; } = string.Empty; // Create, Update, Delete, Login, Import
    
    [Required, StringLength(100)]
    public string EntityType { get; set; } = string.Empty;
    
    [StringLength(100)]
    public string? EntityId { get; set; }
    
    public string? BeforeValue { get; set; } // JSON
    
    public string? AfterValue { get; set; } // JSON
    
    [Required, StringLength(45)]
    public string IpAddress { get; set; } = string.Empty;
    
    [StringLength(500)]
    public string? UserAgent { get; set; }
}
```

---

## 3. ENUMS (New Enums Required)

**Location**: `src/NBT.Domain/Enums/`

```csharp
public enum RegistrationStatus
{
    Pending = 0,
    Confirmed = 1,
    Cancelled = 2,
    NoShow = 3,
    Completed = 4
}

public enum PaymentStatus
{
    Pending = 0,
    Paid = 1,
    Failed = 2,
    Refunded = 3,
    PartialRefund = 4
}

public enum SessionStatus
{
    Open = 0,
    Full = 1,
    Closed = 2,
    Completed = 3,
    Cancelled = 4
}

public enum TestType
{
    AcademicLiteracy = 1,
    QuantitativeLiteracy = 2,
    Mathematics = 3
}

public enum PerformanceBand
{
    Elementary = 1,
    Basic = 2,
    Intermediate = 3,
    Proficient = 4
}
```

---

## 4. VALUE OBJECTS (Domain Logic)

### 4.1 NBTNumber Value Object

**Location**: `src/NBT.Domain/ValueObjects/NBTNumber.cs`

```csharp
namespace NBT.Domain.ValueObjects;

/// <summary>
/// NBT Number with Luhn algorithm validation.
/// Format: YYYYSSSSSSSSSC (14 digits)
/// YYYY = Full Year (4 digits), SSSSSSSSS = Sequence (9 digits), C = Checksum (1 digit)
/// Example: 20250000012345 (Year 2025, Sequence 000001234, Checksum 5)
/// </summary>
public class NBTNumber : ValueObject
{
    public string Value { get; private set; }
    
    private NBTNumber(string value)
    {
        Value = value;
    }
    
    /// <summary>
    /// Generates a new NBT number with Luhn checksum.
    /// </summary>
    public static NBTNumber Generate(int year, int sequence)
    {
        if (year < 2000 || year > 2099)
            throw new ArgumentException("Year must be between 2000 and 2099.");
            
        if (sequence < 1 || sequence > 999999999)
            throw new ArgumentException("Sequence must be between 1 and 999999999.");
            
        string yearPart = year.ToString("D4"); // 4 digits
        string sequencePart = sequence.ToString("D9"); // 9 digits
        string baseNumber = yearPart + sequencePart;
        
        int checksum = CalculateLuhnChecksum(baseNumber);
        string nbtNumber = baseNumber + checksum;
        
        return new NBTNumber(nbtNumber);
    }
    
    /// <summary>
    /// Creates from existing NBT number (validates checksum).
    /// </summary>
    public static NBTNumber Create(string value)
    {
        if (!IsValid(value))
            throw new DomainException($"Invalid NBT number: {value}");
            
        return new NBTNumber(value);
    }
    
    public static bool IsValid(string nbtNumber)
    {
        if (string.IsNullOrWhiteSpace(nbtNumber) || nbtNumber.Length != 14)
            return false;
            
        if (!long.TryParse(nbtNumber, out _))
            return false;
            
        return ValidateLuhnChecksum(nbtNumber);
    }
    
    private static int CalculateLuhnChecksum(string number)
    {
        int sum = 0;
        bool alternate = false;
        
        // Process from right to left
        for (int i = number.Length - 1; i >= 0; i--)
        {
            int digit = number[i] - '0';
            
            if (alternate)
            {
                digit *= 2;
                if (digit > 9)
                    digit -= 9; // Sum digits: 12 → 1+2 = 3
            }
            
            sum += digit;
            alternate = !alternate;
        }
        
        int checksum = (10 - (sum % 10)) % 10;
        return checksum;
    }
    
    private static bool ValidateLuhnChecksum(string nbtNumber)
    {
        string baseNumber = nbtNumber.Substring(0, 13);
        int providedChecksum = int.Parse(nbtNumber.Substring(13, 1));
        int calculatedChecksum = CalculateLuhnChecksum(baseNumber);
        
        return providedChecksum == calculatedChecksum;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    
    public override string ToString() => Value;
}
```

### 4.2 SAIDNumber Value Object

**Location**: `src/NBT.Domain/ValueObjects/SAIDNumber.cs`

```csharp
namespace NBT.Domain.ValueObjects;

/// <summary>
/// South African ID Number with full validation.
/// Format: YYMMDDGSSSCAZ (13 digits)
/// YYMMDD = Date of Birth
/// G = Gender (0-4 female, 5-9 male)
/// SSS = Sequence
/// C = Citizenship (0=SA, 1=Permanent Resident)
/// A = Race (deprecated, always 8)
/// Z = Luhn checksum
/// </summary>
public class SAIDNumber : ValueObject
{
    public string Value { get; private set; }
    public DateTime DateOfBirth { get; private set; }
    public string Gender { get; private set; }
    public bool IsSACitizen { get; private set; }
    
    private SAIDNumber(string value)
    {
        Value = value;
        DateOfBirth = ExtractDateOfBirth(value);
        Gender = ExtractGender(value);
        IsSACitizen = ExtractCitizenship(value);
    }
    
    public static SAIDNumber Create(string value)
    {
        if (!IsValid(value))
            throw new DomainException($"Invalid South African ID number: {value}");
            
        return new SAIDNumber(value);
    }
    
    public static bool IsValid(string idNumber)
    {
        if (string.IsNullOrWhiteSpace(idNumber) || idNumber.Length != 13)
            return false;
            
        if (!long.TryParse(idNumber, out _))
            return false;
            
        // Validate date portion
        if (!IsValidDate(idNumber))
            return false;
            
        // Validate Luhn checksum
        if (!ValidateLuhnChecksum(idNumber))
            return false;
            
        return true;
    }
    
    private static bool IsValidDate(string idNumber)
    {
        int year = int.Parse(idNumber.Substring(0, 2));
        int month = int.Parse(idNumber.Substring(2, 2));
        int day = int.Parse(idNumber.Substring(4, 2));
        
        // Determine century (assume 20xx for years <= current year, else 19xx)
        int currentYear = DateTime.Now.Year % 100;
        int century = year <= currentYear ? 2000 : 1900;
        int fullYear = century + year;
        
        try
        {
            var date = new DateTime(fullYear, month, day);
            return date <= DateTime.Now; // Cannot be in the future
        }
        catch
        {
            return false;
        }
    }
    
    private static bool ValidateLuhnChecksum(string idNumber)
    {
        int sum = 0;
        bool alternate = false;
        
        for (int i = idNumber.Length - 1; i >= 0; i--)
        {
            int digit = idNumber[i] - '0';
            
            if (alternate)
            {
                digit *= 2;
                if (digit > 9)
                    digit -= 9;
            }
            
            sum += digit;
            alternate = !alternate;
        }
        
        return sum % 10 == 0;
    }
    
    private static DateTime ExtractDateOfBirth(string idNumber)
    {
        int year = int.Parse(idNumber.Substring(0, 2));
        int month = int.Parse(idNumber.Substring(2, 2));
        int day = int.Parse(idNumber.Substring(4, 2));
        
        int currentYear = DateTime.Now.Year % 100;
        int century = year <= currentYear ? 2000 : 1900;
        int fullYear = century + year;
        
        return new DateTime(fullYear, month, day);
    }
    
    private static string ExtractGender(string idNumber)
    {
        int genderDigit = int.Parse(idNumber.Substring(6, 1));
        return genderDigit >= 5 ? "M" : "F";
    }
    
    private static bool ExtractCitizenship(string idNumber)
    {
        int citizenshipDigit = int.Parse(idNumber.Substring(10, 1));
        return citizenshipDigit == 0; // 0 = SA citizen
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    
    public override string ToString() => Value;
}
```

---

## 5. API ENDPOINT SUMMARY

### Complete Endpoint Matrix

| Module | Count | Base URL | Auth |
|--------|-------|----------|------|
| Students | 9 | `/api/students` | Admin, Staff |
| Registration | 7 | `/api/registration` | Public, Admin |
| Booking | 4 | `/api/booking` | Public |
| Payments | 7 | `/api/payments` | Public, Admin |
| Venues | 9 | `/api/venues` | Admin |
| Test Sessions | 7 | `/api/sessions` | Admin, Staff |
| Test Results | 6 | `/api/results` | Admin, Staff |
| Staff/Admin | 5 | `/api/staff` | Admin, SuperUser |
| Reports | 7 | `/api/reports` | Admin, Staff |
| **TOTAL** | **61** | | |

---

## 6. DETAILED API SPECIFICATIONS

### 6.1 Students API

**Base URL**: `/api/students`

#### GET `/api/students`
**Description**: List all students (paginated)  
**Auth**: Admin, Staff  
**Query Parameters**:
- `page` (int, default: 1)
- `pageSize` (int, default: 50, max: 100)
- `search` (string, optional) - Search by name, NBT number, ID number
- `grade` (int, optional) - Filter by grade
- `sortBy` (string, optional) - Sort field
- `sortDesc` (bool, default: false)

**Response 200**:
```json
{
  "success": true,
  "data": [
    {
      "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "nbtNumber": "250012345",
      "idNumber": "0001015800089",
      "firstName": "John",
      "lastName": "Doe",
      "dateOfBirth": "2000-01-01T00:00:00Z",
      "gender": "M",
      "email": "john@example.com",
      "phone": "+27821234567",
      "schoolName": "ABC High School",
      "grade": 12,
      "createdDate": "2025-01-01T00:00:00Z"
    }
  ],
  "pagination": {
    "page": 1,
    "pageSize": 50,
    "totalRecords": 150,
    "totalPages": 3
  }
}
```

#### POST `/api/students`
**Description**: Create new student (generates NBT number automatically)  
**Auth**: Admin  
**Request Body**:
```json
{
  "idNumber": "0001015800089",
  "firstName": "John",
  "lastName": "Doe",
  "email": "john@example.com",
  "phone": "+27821234567",
  "address": "123 Main Street",
  "city": "Johannesburg",
  "province": "Gauteng",
  "postalCode": "2000",
  "schoolName": "ABC High School",
  "grade": 12,
  "homeLanguage": "English",
  "specialAccommodation": null
}
```

**Validation Rules**:
- ✅ `idNumber`: Must be 13 digits, pass Luhn validation
- ✅ `email`: Valid email format, unique
- ✅ `phone`: SA format (+27 or 0)
- ✅ `grade`: 10-12 (if provided)
- ✅ NBT number generated automatically

**Response 201**:
```json
{
  "success": true,
  "data": {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "nbtNumber": "250012346", // Auto-generated
    "idNumber": "0001015800089",
    "firstName": "John",
    "lastName": "Doe",
    "dateOfBirth": "2000-01-01T00:00:00Z", // Extracted from ID
    "gender": "M", // Extracted from ID
    "email": "john@example.com",
    "createdDate": "2025-11-08T16:00:00Z"
  },
  "message": "Student created successfully"
}
```

#### GET `/api/students/{id}`
**Description**: Get student by ID  
**Auth**: Admin, Staff  
**Response 200**: Single student object

#### GET `/api/students/nbt/{nbtNumber}`
**Description**: Get student by NBT number  
**Auth**: Admin, Staff  
**Response 200**: Single student object

#### POST `/api/students/generate-nbt-number`
**Description**: Generate NBT number without creating student  
**Auth**: Admin  
**Request Body**:
```json
{
  "idNumber": "0001015800089"
}
```

**Response 200**:
```json
{
  "success": true,
  "data": {
    "nbtNumber": "250012347",
    "message": "NBT number generated successfully"
  }
}
```

#### POST `/api/students/validate-id`
**Description**: Validate SA ID number (public endpoint for registration wizard)  
**Auth**: Public  
**Request Body**:
```json
{
  "idNumber": "0001015800089"
}
```

**Response 200**:
```json
{
  "success": true,
  "data": {
    "isValid": true,
    "dateOfBirth": "2000-01-01T00:00:00Z",
    "gender": "M",
    "isSACitizen": true
  }
}
```

---

## 7. EASYPAY INTEGRATION

### EasyPay Configuration

**Location**: `src/NBT.WebAPI/appsettings.json`

```json
{
  "EasyPay": {
    "MerchantId": "NBT_MERCHANT_ID",
    "ApiKey": "STORED_IN_KEYVAULT",
    "ApiSecret": "STORED_IN_KEYVAULT",
    "PaymentUrl": "https://api.easypay.co.za/v1/payment",
    "CallbackUrl": "https://nbt.ac.za/api/payments/easypay/callback",
    "ReturnUrl": "https://nbt.ac.za/registration/payment-complete",
    "IsTestMode": false,
    "TestFee": 150.00
  }
}
```

### EasyPay Payment Flow

1. **Student completes registration wizard**
2. **System creates Payment record** with status `Pending`
3. **API calls EasyPay** with `InitiatePaymentRequest`
4. **EasyPay returns** payment URL
5. **User redirected** to EasyPay payment page
6. **User completes payment** on EasyPay
7. **EasyPay sends callback** to `/api/payments/easypay/callback`
8. **System updates Payment** status to `Paid`
9. **System updates Registration** status to `Confirmed`
10. **Email confirmation** sent to student

### EasyPay API Request

```csharp
public class EasyPayPaymentRequest
{
    public string MerchantId { get; set; }
    public string Reference { get; set; } // Our InvoiceNumber
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "ZAR";
    public string Description { get; set; }
    public string CallbackUrl { get; set; }
    public string ReturnUrl { get; set; }
    public Dictionary<string, string> CustomFields { get; set; }
}
```

### EasyPay Callback

```csharp
public class EasyPayCallbackRequest
{
    public string Reference { get; set; } // Our InvoiceNumber
    public string TransactionId { get; set; }
    public string Status { get; set; } // Success, Failed, Cancelled
    public decimal Amount { get; set; }
    public DateTime TransactionDate { get; set; }
    public string Signature { get; set; } // HMAC-SHA256 for verification
}
```

---

## 8. VALIDATION SUMMARY

### Student Creation
- ✅ SA ID number: 13 digits, Luhn valid, valid date, cannot be duplicate
- ✅ Email: Valid format, unique
- ✅ Phone: SA format (+27 or 0 followed by 9 digits)
- ✅ Grade: 10-12 (if provided)
- ✅ NBT number: Auto-generated with Luhn checksum

### Registration Creation
- ✅ Student must exist
- ✅ Test session must exist and be Open
- ✅ At least one test type must be selected
- ✅ Session must have available capacity
- ✅ No duplicate registrations (same student + session)

### Payment Processing
- ✅ Registration must exist
- ✅ Amount must match fee structure
- ✅ EasyPay reference must be unique
- ✅ Callback signature must be valid (HMAC verification)

### Test Result Import
- ✅ Excel file: .xlsx format, max 10MB
- ✅ NBT number must exist in system
- ✅ Test session must exist
- ✅ Raw score: 0-100
- ✅ Percentile: 1-99
- ✅ No duplicate results (same student + session + test type)

---

## 9. DATABASE MIGRATION

**Create Migration**:
```bash
dotnet ef migrations add AddCoreEntities --project src/NBT.Infrastructure --startup-project src/NBT.WebAPI
```

**Apply Migration**:
```bash
dotnet ef database update --project src/NBT.Infrastructure --startup-project src/NBT.WebAPI
```

---

## 10. STATUS & NEXT STEPS

✅ **Contract Definition**: COMPLETE  
⏳ **Implementation Plan**: NEXT  
⏳ **Task Breakdown**: PENDING

**Next Document**: `plan.md` - Full implementation plan with timeline

---

**APPROVED FOR IMPLEMENTATION**  
**Author**: NBT Development Team  
**Date**: 2025-11-08
