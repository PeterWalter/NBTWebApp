# Critical Updates & Requirements - NBT Integrated System

**Date**: 2025-11-08  
**Status**: MANDATORY IMPLEMENTATION  
**Priority**: CRITICAL

---

## 🚨 CRITICAL FIX: JSON Property Serialization

### Problem
The application experiences "property value in JSON" errors due to inconsistent JSON property naming and serialization configuration.

### Solution (MANDATORY)

#### 1. Global JSON Configuration

**Location**: `src/NBT.WebAPI/Program.cs`

```csharp
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Use camelCase for JSON properties
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        
        // Handle null values consistently
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        
        // Handle enums as strings (not integers)
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        
        // Handle circular references
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        
        // Pretty print in development
        options.JsonSerializerOptions.WriteIndented = builder.Environment.IsDevelopment();
    });
```

#### 2. DTO Property Decoration

**ALL DTOs MUST use `[JsonPropertyName]` attributes:**

```csharp
public class StudentDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    
    [JsonPropertyName("nbtNumber")]
    public string NBTNumber { get; set; }
    
    [JsonPropertyName("idNumber")]
    public string IDNumber { get; set; }
    
    [JsonPropertyName("firstName")]
    public string FirstName { get; set; }
    
    [JsonPropertyName("lastName")]
    public string LastName { get; set; }
    
    [JsonPropertyName("email")]
    public string Email { get; set; }
    
    // ... all other properties
}
```

#### 3. Blazor WebAssembly Configuration

**Location**: `src/NBT.WebUI/Program.cs`

```csharp
builder.Services.AddHttpClient("NBT.WebAPI", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiUrl"] ?? "https://localhost:5001");
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});
```

#### 4. Testing

**MANDATORY test for every API endpoint:**

```csharp
[Fact]
public async Task POST_Students_Returns_ValidJson()
{
    // Arrange
    var request = new CreateStudentRequest
    {
        IDNumber = "0001015800089",
        FirstName = "John",
        LastName = "Doe",
        Email = "john@test.com",
        Phone = "+27821234567"
    };
    
    // Act
    var response = await _client.PostAsJsonAsync("/api/students", request);
    
    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.Created);
    
    var json = await response.Content.ReadAsStringAsync();
    var student = JsonSerializer.Deserialize<StudentDto>(json, _jsonOptions);
    
    student.Should().NotBeNull();
    student.NBTNumber.Should().NotBeNullOrEmpty();
    student.FirstName.Should().Be("John");
}
```

---

## 📋 COMPLETE STUDENT WORKFLOW (UPDATED)

### Student (Applicant/Writer) Activities

Students interact with the NBT Web Application through a structured, self-service workflow:

#### 1. Account Creation & Login
- Register a new account or sign in
- **Supports THREE ID types:**
  - **SA_ID**: South African ID number (13 digits with Luhn validation)
  - **FOREIGN_ID**: Foreign national ID (6-20 alphanumeric characters)
  - **PASSPORT**: International passport number (6-20 alphanumeric characters)
- OTP verification for security
- Duplicate prevention via ID number validation
- Secure authentication with password requirements
- Account remains active for future test cycles

#### 2. NBT Number Generation
- Automatic generation upon successful registration
- **Format**: YYYYSSSSSSSSSC (14 digits)
  - YYYY = Registration year (4 digits)
  - SSSSSSSSS = Sequential number (9 digits)
  - C = Luhn checksum (1 digit)
- Unique identifier linking ALL:
  - Bookings
  - Payments
  - Results
- **MANDATORY**: Must pass Luhn (modulus-10) validation

#### 3. Registration Wizard (Multi-Step Process)

**Step 1: Personal Details**
- ID Type selection: SA_ID | FOREIGN_ID | PASSPORT
- ID number with real-time validation
- Auto-extraction (SA ID only):
  - Date of birth
  - Gender (M/F)
  - Citizenship status
- Manual entry for Foreign ID/Passport:
  - Date of birth
  - Gender
  - Nationality
  - Country of origin
- Contact info: Email, phone, address

**Step 2: Academic Background**
- School name
- Current grade (10-12)
- Home language

**Step 3: Test Preferences**
- Test type selection: AQL only OR AQL + MAT
- Venue selection (from available venues)
- Test date selection (from available sessions)
- Special accommodation requests (optional)

**Step 4: Confirmation & Payment**
- Summary review
- EasyPay payment initiation
- Terms and conditions acceptance

**Features:**
- Each step validates inputs and saves progress automatically
- Registration resumable if interrupted
- Progress indicator visible throughout

#### 4. Booking & Payment Rules (CRITICAL BUSINESS RULES)

**Intake Start Date**
- Bookings open annually on **April 1** (Year Intake start date)
- Students cannot book before this date

**One Active Booking Rule**
- A student can only have **ONE active booking** at a time
- "Active" means: booked and closing date has not passed

**Rebooking Rules**
- Student can book another test ONLY AFTER the **closing date** of their current booking has passed
- Example:
  - Current booking closes: March 15
  - Student can book new test: March 16 onwards

**Annual Limit**
- Maximum of **2 tests per year** per student
- Year = Calendar year (January 1 - December 31)

**Test Validity**
- Tests remain valid for **3 years from booking date**
- Students can access results during this period

**Booking Changes**
- Students can **change their booking** BEFORE the close of booking date
- Changes include:
  - Test session date
  - Venue selection
  - Test type (AQL only ↔ AQL + MAT)
- After closing date: NO changes allowed

**Test Types**
- Choose **AQL only** (Academic & Quantitative Literacy)
- OR **AQL + MAT** (includes Mathematics)
- Cannot book MAT alone

**Venue Selection**
- Choose from active venues with capacity
- System shows available seats per session
- Overbooking prevented via capacity checks

**EasyPay Integration**
- System generates unique EasyPay payment reference upon booking
- Automated transaction recording
- Payment status updates trigger booking status changes:
  - Payment Pending → Booking Pending
  - Payment Paid → Booking Confirmed
  - Payment Failed → Booking Cancelled

#### 5. Special or Remote Sessions
- Applicants needing off-site testing complete a special form
- Required information:
  - Invigilator name and contact details
  - Remote venue information
  - Reason for remote testing
- Requests routed **automatically** to NBT remote administration team
- Additional verification and approval workflow required

#### 6. Pre-Test Questionnaire
- **Mandatory** after registration
- Online background questionnaire covering:
  - Educational background
  - Socioeconomic information
  - Language proficiency
  - Study plans
- Information informs:
  - Research initiatives
  - Equity reporting
  - Operational insights
- Must be completed before test participation

#### 7. Results Access
- Students securely log in to view/download results once released
- Results display:
  - AQL score (raw score, percentile, performance band)
  - MAT score (if applicable)
  - National norms comparison
  - Performance band interpretation
- Results available for **3 years from test date**
- Downloadable result certificates (PDF format)
- Historical results accessible from account

#### 8. Profile Management
- Update personal details (address, contact info)
- **Cannot change**: ID number, date of birth, NBT number
- Upload supporting documents (if required by institutions)
- Password reset functionality
- Two-factor authentication management
- **ALL edits logged** for audit tracking and compliance

#### 9. Notifications (Email/SMS)
Automated alerts sent for:
- **Registration**: Confirmation with NBT number
- **Payment**: 
  - Payment initiated
  - Payment confirmed
  - Payment failed
- **Test Reminders**:
  - 7 days before test date
  - 1 day before test date
- **Results**: Result availability notification
- **Booking Modifications**: Change confirmations
- **Account Security**: 
  - Login from new device
  - Password reset
  - Profile changes

#### 10. Account Retention
- Accounts remain active indefinitely
- Access to:
  - Future test cycles (new bookings)
  - Historical booking data
  - Past results (within 3-year validity)
- Academic history preserved for reporting
- Continuity across multiple test cycles
- Re-registration not required for subsequent tests

---

## 🔒 MANDATORY ID TYPE VALIDATION

### ID Type Enum

```csharp
public enum IDType
{
    SA_ID = 1,
    FOREIGN_ID = 2,
    PASSPORT = 3
}
```

### Student Entity Updates

```csharp
public class Student : BaseEntity, IAuditableEntity
{
    /// <summary>
    /// Type of identification document
    /// </summary>
    [Required]
    public IDType IDType { get; set; } = IDType.SA_ID;
    
    /// <summary>
    /// ID/Passport number (validation depends on IDType)
    /// </summary>
    [Required, StringLength(20)]
    public string IDNumber { get; set; } = string.Empty;
    
    /// <summary>
    /// Nationality (required for FOREIGN_ID and PASSPORT)
    /// </summary>
    [StringLength(100)]
    public string? Nationality { get; set; }
    
    /// <summary>
    /// Country of origin (required for FOREIGN_ID and PASSPORT)
    /// </summary>
    [StringLength(100)]
    public string? CountryOfOrigin { get; set; }
    
    // ... other properties
}
```

### Validation Logic

```csharp
public class StudentValidator : AbstractValidator<CreateStudentRequest>
{
    public StudentValidator()
    {
        RuleFor(x => x.IDType)
            .IsInEnum()
            .WithMessage("ID Type must be SA_ID, FOREIGN_ID, or PASSPORT");
        
        // SA ID validation
        When(x => x.IDType == IDType.SA_ID, () =>
        {
            RuleFor(x => x.IDNumber)
                .NotEmpty()
                .Length(13)
                .Matches(@"^\d{13}$")
                .Must(BeValidSAID)
                .WithMessage("Invalid South African ID number");
            
            RuleFor(x => x.Nationality)
                .Empty()
                .WithMessage("Nationality not required for SA ID");
            
            RuleFor(x => x.CountryOfOrigin)
                .Empty()
                .WithMessage("Country not required for SA ID");
        });
        
        // Foreign ID validation
        When(x => x.IDType == IDType.FOREIGN_ID, () =>
        {
            RuleFor(x => x.IDNumber)
                .NotEmpty()
                .Length(6, 20)
                .Matches(@"^[A-Z0-9]+$")
                .WithMessage("Foreign ID must be 6-20 alphanumeric characters");
            
            RuleFor(x => x.Nationality)
                .NotEmpty()
                .WithMessage("Nationality is required for Foreign ID");
            
            RuleFor(x => x.CountryOfOrigin)
                .NotEmpty()
                .WithMessage("Country of origin is required for Foreign ID");
            
            RuleFor(x => x.DateOfBirth)
                .NotEmpty()
                .WithMessage("Date of birth must be provided for Foreign ID");
        });
        
        // Passport validation
        When(x => x.IDType == IDType.PASSPORT, () =>
        {
            RuleFor(x => x.IDNumber)
                .NotEmpty()
                .Length(6, 20)
                .Matches(@"^[A-Z0-9]+$")
                .WithMessage("Passport number must be 6-20 alphanumeric characters");
            
            RuleFor(x => x.Nationality)
                .NotEmpty()
                .WithMessage("Nationality is required for Passport");
            
            RuleFor(x => x.CountryOfOrigin)
                .NotEmpty()
                .WithMessage("Country of origin is required for Passport");
            
            RuleFor(x => x.DateOfBirth)
                .NotEmpty()
                .WithMessage("Date of birth must be provided for Passport");
        });
    }
    
    private bool BeValidSAID(string idNumber)
    {
        return SAIDNumber.IsValid(idNumber);
    }
}
```

---

## 🔗 CRITICAL RELATIONSHIPS

### TestSession → Venue (NOT Room)

**CORRECT Relationship:**
```csharp
public class TestSession : BaseEntity
{
    public Guid VenueId { get; set; }
    public Venue Venue { get; set; } = null!;
    
    // NO direct RoomId reference
}
```

### RoomAllocation: Links Students to Rooms

**Purpose**: Assign specific students to specific rooms within a venue for a session

```csharp
public class RoomAllocation : BaseEntity
{
    public Guid TestSessionId { get; set; }
    public TestSession TestSession { get; set; } = null!;
    
    public Guid RoomId { get; set; }
    public Room Room { get; set; } = null!;
    
    public Guid StudentId { get; set; }
    public Student Student { get; set; } = null!;
    
    public DateTime AllocatedDate { get; set; }
    public string AllocatedBy { get; set; } = string.Empty;
}
```

**Flow:**
1. Student books a test session (TestSession linked to Venue)
2. Admin allocates students to specific rooms
3. RoomAllocation table links: Student + TestSession + Room
4. System tracks room capacity per session

---

## ✅ BOOKING VALIDATION SERVICE (MANDATORY)

```csharp
public interface IBookingValidationService
{
    /// <summary>
    /// Validates if student can book a new test (checks all business rules)
    /// </summary>
    Task<BookingValidationResult> ValidateNewBookingAsync(
        Guid studentId, 
        DateTime sessionDate, 
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Checks if student has reached annual booking limit (2 tests/year)
    /// </summary>
    Task<bool> HasReachedAnnualLimitAsync(
        Guid studentId, 
        int year, 
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Checks if student has an active booking (booking exists and not yet closed)
    /// </summary>
    Task<bool> HasActiveBookingAsync(
        Guid studentId, 
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Validates if student can modify existing booking (before close date)
    /// </summary>
    Task<bool> CanModifyBookingAsync(
        Guid bookingId, 
        DateTime currentDate, 
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Checks if test result is still valid (within 3 years from booking date)
    /// </summary>
    Task<bool> IsTestStillValidAsync(
        Guid testResultId, 
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Checks if booking period is open (after April 1 intake start date)
    /// </summary>
    Task<bool> IsBookingPeriodOpenAsync(
        DateTime bookingDate, 
        CancellationToken cancellationToken = default);
}

public class BookingValidationResult
{
    public bool IsValid { get; set; }
    public List<string> Errors { get; set; } = new();
    
    public static BookingValidationResult Success()
        => new() { IsValid = true };
    
    public static BookingValidationResult Failure(params string[] errors)
        => new() { IsValid = false, Errors = errors.ToList() };
}
```

### Implementation Example

```csharp
public class BookingValidationService : IBookingValidationService
{
    private readonly IApplicationDbContext _context;
    
    public async Task<BookingValidationResult> ValidateNewBookingAsync(
        Guid studentId, 
        DateTime sessionDate, 
        CancellationToken cancellationToken = default)
    {
        var errors = new List<string>();
        
        // Rule 1: Check if booking period is open (after April 1)
        var currentYear = DateTime.Now.Year;
        var intakeStartDate = new DateTime(currentYear, 4, 1); // April 1
        
        if (DateTime.Now < intakeStartDate)
        {
            errors.Add($"Bookings open on {intakeStartDate:yyyy-MM-dd}. Current booking period not yet open.");
        }
        
        // Rule 2: Check for active bookings
        var hasActiveBooking = await HasActiveBookingAsync(studentId, cancellationToken);
        if (hasActiveBooking)
        {
            errors.Add("You already have an active booking. You can only book one test at a time.");
        }
        
        // Rule 3: Check annual limit (2 tests per year)
        var sessionYear = sessionDate.Year;
        var hasReachedLimit = await HasReachedAnnualLimitAsync(studentId, sessionYear, cancellationToken);
        if (hasReachedLimit)
        {
            errors.Add($"You have reached the maximum of 2 tests for {sessionYear}.");
        }
        
        // Rule 4: Check session capacity
        var session = await _context.TestSessions
            .FirstOrDefaultAsync(s => s.SessionDate.Date == sessionDate.Date, cancellationToken);
        
        if (session == null)
        {
            errors.Add("Selected test session not found.");
        }
        else if (session.AvailableSeats <= 0)
        {
            errors.Add("Selected test session is fully booked.");
        }
        
        return errors.Any() 
            ? BookingValidationResult.Failure(errors.ToArray())
            : BookingValidationResult.Success();
    }
    
    public async Task<bool> HasReachedAnnualLimitAsync(
        Guid studentId, 
        int year, 
        CancellationToken cancellationToken = default)
    {
        var startDate = new DateTime(year, 1, 1);
        var endDate = new DateTime(year, 12, 31);
        
        var bookingCount = await _context.Registrations
            .Where(r => r.StudentId == studentId)
            .Where(r => r.RegistrationDate >= startDate && r.RegistrationDate <= endDate)
            .Where(r => r.Status != RegistrationStatus.Cancelled)
            .CountAsync(cancellationToken);
        
        return bookingCount >= 2;
    }
    
    public async Task<bool> HasActiveBookingAsync(
        Guid studentId, 
        CancellationToken cancellationToken = default)
    {
        var today = DateTime.Now.Date;
        
        return await _context.Registrations
            .AnyAsync(r => 
                r.StudentId == studentId &&
                r.Status == RegistrationStatus.Confirmed &&
                r.BookingCloseDate >= today,
                cancellationToken);
    }
    
    public async Task<bool> CanModifyBookingAsync(
        Guid bookingId, 
        DateTime currentDate, 
        CancellationToken cancellationToken = default)
    {
        var registration = await _context.Registrations
            .FirstOrDefaultAsync(r => r.Id == bookingId, cancellationToken);
        
        if (registration == null)
            return false;
        
        // Can modify only if before closing date
        return registration.BookingCloseDate.Date >= currentDate.Date;
    }
    
    public async Task<bool> IsTestStillValidAsync(
        Guid testResultId, 
        CancellationToken cancellationToken = default)
    {
        var result = await _context.TestResults
            .Include(r => r.Student)
            .ThenInclude(s => s.Registrations)
            .FirstOrDefaultAsync(r => r.Id == testResultId, cancellationToken);
        
        if (result == null)
            return false;
        
        // Find the registration for this test
        var registration = result.Student.Registrations
            .FirstOrDefault(r => r.TestSessionId == result.TestSessionId);
        
        if (registration == null)
            return false;
        
        // Valid for 3 years from booking date
        var expiryDate = registration.RegistrationDate.AddYears(3);
        
        return DateTime.Now <= expiryDate;
    }
    
    public Task<bool> IsBookingPeriodOpenAsync(
        DateTime bookingDate, 
        CancellationToken cancellationToken = default)
    {
        var currentYear = bookingDate.Year;
        var intakeStartDate = new DateTime(currentYear, 4, 1); // April 1
        
        return Task.FromResult(bookingDate >= intakeStartDate);
    }
}
```

---

## 📝 IMPLEMENTATION CHECKLIST

### Phase 1: JSON Serialization Fix (IMMEDIATE)
- [ ] Configure JSON options in WebAPI Program.cs
- [ ] Configure JSON options in WebUI Program.cs
- [ ] Add [JsonPropertyName] to ALL DTOs
- [ ] Test ALL API endpoints for proper JSON formatting
- [ ] Add JSON serialization tests to integration test suite

### Phase 2: ID Type Support (CRITICAL)
- [ ] Add IDType enum to Domain
- [ ] Update Student entity with IDType, Nationality, CountryOfOrigin
- [ ] Create migration for new fields
- [ ] Update validation logic for all three ID types
- [ ] Update Registration Wizard Step 1 UI
- [ ] Add ID type selector dropdown
- [ ] Conditional field display based on ID type
- [ ] Update API documentation

### Phase 3: Booking Validation Service (CRITICAL)
- [ ] Create IBookingValidationService interface
- [ ] Implement BookingValidationService
- [ ] Add all business rule checks
- [ ] Register service in DI container
- [ ] Integrate into Registration workflow
- [ ] Add comprehensive unit tests (20+ scenarios)
- [ ] Document business rules in user guide

### Phase 4: TestSession Relationship (VERIFY)
- [ ] Verify TestSession → Venue relationship (NOT Room)
- [ ] Verify RoomAllocation entity exists
- [ ] Test room allocation workflow
- [ ] Update admin UI if needed

### Phase 5: Student Workflow Documentation
- [ ] Update user guide with complete workflow
- [ ] Document all 10 activity stages
- [ ] Create flowcharts for booking rules
- [ ] Add FAQ for common scenarios
- [ ] Create video tutorials

---

## 🚀 IMMEDIATE NEXT STEPS

1. **Fix JSON serialization** (TODAY)
   - Configure JSON options globally
   - Decorate all DTOs with [JsonPropertyName]
   - Test endpoints

2. **Add ID type support** (Week 1)
   - Update domain model
   - Create migration
   - Update validation

3. **Implement booking validation** (Week 2)
   - Create service
   - Add business rules
   - Test thoroughly

4. **Update documentation** (Ongoing)
   - Keep specs updated
   - Document all decisions
   - Maintain API docs

---

**STATUS**: READY FOR IMPLEMENTATION  
**PRIORITY**: CRITICAL  
**ESTIMATED EFFORT**: 3-5 days for all critical updates  

**Next Action**: Begin JSON serialization fix immediately
