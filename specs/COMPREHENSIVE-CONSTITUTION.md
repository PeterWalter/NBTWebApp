# NBT Web Application - Comprehensive Constitution

**Document Version:** 2.0  
**Date:** 2025-11-09  
**Status:** AUTHORITATIVE

---

## 1. EXECUTIVE OVERVIEW

This constitution defines the non-negotiable principles, coding standards, and architectural rules for the **National Benchmark Tests (NBT) Integrated Web Application**. The system is built on an existing Blazor WebAssembly + ASP.NET Core Web API shell that must be audited, completed, and enhanced with all missing components, modules, and configurations.

### Core Technology Stack
- **Frontend:** Blazor WebAssembly with **Fluent UI** components (.NET 9)
- **Backend:** ASP.NET Core Web API (.NET 9)
- **Database:** Microsoft SQL Server with Entity Framework Core
- **Authentication:** JWT-based with role-based access control
- **Architecture:** Clean Architecture with CQRS patterns

---

## 2. NON-NEGOTIABLE ARCHITECTURAL PRINCIPLES

### 2.1 Clean Architecture
```
┌─────────────────────────────────────────┐
│         Presentation Layer              │
│  (Blazor WebAssembly + Fluent UI)      │
└─────────────────────────────────────────┘
              ↓ (DTOs)
┌─────────────────────────────────────────┐
│         Application Layer               │
│  (Business Logic, Services, CQRS)      │
└─────────────────────────────────────────┘
              ↓ (Interfaces)
┌─────────────────────────────────────────┐
│         Domain Layer                    │
│  (Entities, Value Objects, Enums)      │
└─────────────────────────────────────────┘
              ↓ (Repository Pattern)
┌─────────────────────────────────────────┐
│      Infrastructure Layer               │
│  (EF Core, External Services)          │
└─────────────────────────────────────────┘
```

**Mandatory Rules:**
- Domain layer has NO external dependencies
- Application layer depends ONLY on Domain
- Infrastructure implements interfaces from Application
- Presentation depends on Application abstractions only
- NO circular dependencies allowed

### 2.2 Dependency Injection
- ALL services MUST be registered via DI container
- Constructor injection ONLY (no property/method injection)
- Explicit lifetime management: Singleton, Scoped, Transient
- NO service locator pattern

### 2.3 Entity Framework Core
- Code-First approach with Fluent API configuration
- ALL database access through EF Core DbContext
- NO raw SQL except for complex reporting queries
- Migration-based schema management
- Explicit transaction boundaries for multi-step operations

---

## 3. CODING STANDARDS

### 3.1 C# Standards (.NET 9)
```csharp
// ✅ CORRECT - Nullable reference types enabled
public class Student
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty; // Required
    public string? MiddleName { get; set; }              // Optional
    public DateOnly DateOfBirth { get; set; }
}

// ✅ CORRECT - Async all the way
public async Task<Result<StudentDto>> GetStudentAsync(Guid id, CancellationToken ct)
{
    var student = await _context.Students.FindAsync(new object[] { id }, ct);
    return student == null 
        ? Result<StudentDto>.Failure("Student not found")
        : Result<StudentDto>.Success(_mapper.Map<StudentDto>(student));
}

// ✅ CORRECT - Guard clauses
public void ValidateNBTNumber(string nbtNumber)
{
    if (string.IsNullOrWhiteSpace(nbtNumber))
        throw new ArgumentException("NBT number cannot be empty", nameof(nbtNumber));
        
    if (nbtNumber.Length != 14)
        throw new ValidationException("NBT number must be 14 digits");
}
```

### 3.2 JSON Serialization Standards
**ALL DTOs MUST use JsonPropertyName attributes:**
```csharp
using System.Text.Json.Serialization;

public class StudentDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    
    [JsonPropertyName("firstName")]
    public string FirstName { get; set; } = string.Empty;
    
    [JsonPropertyName("nbtNumber")]
    public string? NbtNumber { get; set; }
    
    [JsonPropertyName("dateOfBirth")]
    public DateOnly DateOfBirth { get; set; }
}
```

**WebAPI Program.cs configuration:**
```csharp
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = builder.Environment.IsDevelopment();
    });
```

### 3.3 Naming Conventions
- **Entities:** PascalCase singular (Student, TestBooking, Payment)
- **DTOs:** PascalCase + "Dto" suffix (StudentDto, BookingDto)
- **Interfaces:** "I" prefix (IStudentRepository, INBTNumberGenerator)
- **Private fields:** _camelCase (\_context, \_logger)
- **Async methods:** "Async" suffix (GetStudentAsync, SaveBookingAsync)
- **Boolean properties:** Affirmative (IsActive, HasPaid, CanWrite)

---

## 4. BUSINESS RULE ENFORCEMENT

### 4.1 NBT Number Generation
```csharp
/// <summary>
/// Generates a unique 14-digit NBT number using Luhn algorithm
/// Format: YYYYMMDDXXXXXX where:
///   YYYY = Year of registration
///   MM = Month of registration
///   DD = Day of registration
///   XXXXXX = Sequential number + Luhn check digit
/// </summary>
public interface INBTNumberGenerator
{
    Task<string> GenerateNBTNumberAsync(CancellationToken ct = default);
    bool ValidateNBTNumber(string nbtNumber);
}
```

**Mandatory Rules:**
- MUST use Luhn (modulus-10) algorithm
- MUST be unique across all students
- MUST be validated on all inputs
- MUST be generated immediately after successful registration

### 4.2 South African ID Number Handling
```csharp
public class SAIDValidator
{
    // Format: YYMMDDGGGGSAZ
    // YY = Year, MM = Month, DD = Day
    // GGGG = Gender (0000-4999 female, 5000-9999 male)
    // S = SA Citizen (0) or Permanent Resident (1)
    // A = Race (no longer used, can be any digit)
    // Z = Luhn check digit
    
    public ValidationResult Validate(string idNumber);
    public DateOnly ExtractDateOfBirth(string idNumber);
    public Gender ExtractGender(string idNumber);
}
```

**Mandatory Rules:**
- MUST validate using Luhn algorithm
- MUST extract DOB and Gender automatically
- MUST support Foreign ID/Passport for non-SA citizens
- Foreign IDs stored in separate field with country code

### 4.3 Test Booking Rules
**CRITICAL BUSINESS RULES:**
1. **Booking Window:** Tests can be booked anytime after Intake Start (April 1st each year)
2. **Single Active Booking:** Student can only have ONE active booking at a time
3. **Next Booking Condition:** Can only book next test AFTER current booking's closing date has passed
4. **Annual Limit:** Maximum 2 tests per calendar year
5. **Result Validity:** Test results valid for 3 years from booking date
6. **Booking Modification:** Students can change booking BEFORE booking closing date
7. **Sunday Tests:** Tests on Sunday must be highlighted in UI
8. **Online Tests:** Can be written from anywhere with internet, video, and audio setup

### 4.4 Payment Rules
**CRITICAL PAYMENT RULES:**
1. **Installment Payments:** Test fees can be paid in installments until fully paid
2. **Payment Order:** Payments applied to tests in chronological order of booking
3. **Variable Pricing:** Test costs vary by Intake Year - must calculate based on year
4. **Result Access:**
   - Students can ONLY view/download fully paid test results
   - Staff and Admin can view all results regardless of payment status
5. **Payment Methods:**
   - EasyPay online payment (primary)
   - Bank deposit uploads (batch processed from standardized file format)
6. **Payment Tracking:** ALL transactions logged with timestamp, amount, method, reference

### 4.5 Test Session Rules
**SESSION TYPES:**
- **National:** Standard venues across South Africa
- **Special Session:** Off-site, requires invigilator details and approval
- **Research:** Academic research sessions
- **Online:** Remote supervised via video proctoring

**SESSION LINKING:**
- TestSession linked to TestVenue (NOT individual rooms)
- Room allocation separate process based on venue capacity
- Venue availability tracked per test date

### 4.6 Result Management
**RESULT STRUCTURE:**
```csharp
public class TestResult
{
    public Guid Id { get; set; }
    public Guid StudentId { get; set; }
    public string Barcode { get; set; } = string.Empty; // Unique per answer sheet
    public TestType TestType { get; set; } // AQL, AQL+MAT
    
    // Domain Scores
    public DomainScore? AcademicLiteracy { get; set; }
    public DomainScore? QuantitativeLiteracy { get; set; }
    public DomainScore? Mathematics { get; set; }
    
    public DateTime DateWritten { get; set; }
    public bool IsPaid { get; set; }
}

public class DomainScore
{
    public string Domain { get; set; } = string.Empty; // AL, QL, MAT
    public int RawScore { get; set; }
    public string PerformanceLevel { get; set; } = string.Empty; 
    // e.g., "Basic Lower", "Basic Upper", "Intermediate Lower", 
    //      "Intermediate Upper", "Proficient Lower", "Proficient Upper"
}
```

**RESULT RULES:**
- Barcode distinguishes physical answer sheet (allows same student multiple attempts)
- AQL test returns AL + QL scores
- AQL+MAT test returns AL + QL + MAT scores
- Only fully paid results downloadable as PDF certificate by students
- Staff/Admin can view all results

---

## 5. SECURITY & COMPLIANCE STANDARDS

### 5.1 Authentication & Authorization
```csharp
public enum UserRole
{
    Applicant = 0,    // Students/Test takers
    Staff = 1,        // NBT staff members
    Admin = 2,        // System administrators
    SuperUser = 3     // Full system access
}
```

**Mandatory Security Rules:**
- JWT-based authentication with refresh tokens
- HTTPS ONLY - NO exceptions for HTTP traffic
- Password complexity: min 8 chars, uppercase, lowercase, digit, special char
- Account lockout after 5 failed login attempts (15-minute cooldown)
- Session timeout after 30 minutes of inactivity
- OTP verification for new registrations (email/SMS)
- Duplicate account prevention by email and ID number

### 5.2 Audit Logging
**ALL changes MUST be logged:**
```csharp
public class AuditLog
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string Entity { get; set; } = string.Empty;
    public Guid EntityId { get; set; }
    public string? OldValue { get; set; }
    public string? NewValue { get; set; }
    public DateTime Timestamp { get; set; }
    public string IPAddress { get; set; } = string.Empty;
}
```

### 5.3 WCAG 2.1 AA Compliance
- ALL interactive elements keyboard accessible
- ARIA labels for screen readers
- Minimum contrast ratio 4.5:1
- Form validation with clear error messages
- Skip navigation links
- Alt text for all images

---

## 6. PERFORMANCE STANDARDS

### 6.1 Load Performance
- **Initial Page Load:** < 3 seconds on 3G connection
- **API Response Time:** < 500ms for 95th percentile
- **Database Queries:** < 100ms for single entity fetch
- **Report Generation:** < 10 seconds for Excel/PDF exports

### 6.2 Optimization Requirements
- Lazy loading for Blazor components
- Pagination for all list views (max 50 items per page)
- Response caching for static data (venues, test dates)
- Database indexes on all foreign keys and frequently queried fields
- Async/await for ALL I/O operations

---

## 7. TESTING REQUIREMENTS

### 7.1 Unit Tests
- Minimum 80% code coverage for business logic
- All domain entities and value objects tested
- All services and validators tested
- Mock external dependencies

### 7.2 Integration Tests
- API endpoint tests with in-memory database
- Authentication flow tests
- Payment integration tests (mocked EasyPay)
- Database migration tests

### 7.3 End-to-End Tests
- Registration wizard complete flow
- Booking and payment flow
- Result viewing flow
- Admin CRUD operations

---

## 8. CI/CD COMPLIANCE

### 8.1 GitHub Workflow
```yaml
# Mandatory checks before merge:
- Build succeeds on all projects
- All unit tests pass
- Integration tests pass
- Code coverage >= 80%
- No security vulnerabilities (CodeQL)
- Successful deployment to staging
```

### 8.2 Branch Strategy
- **main:** Production-ready code only
- **develop:** Integration branch for features
- **feature/*:** New feature development
- **bugfix/*:** Bug fixes
- **hotfix/*:** Emergency production fixes

### 8.3 Release Process
1. Feature branch → Pull Request → Code Review
2. All checks pass + 2 approvals required
3. Merge to develop → Deploy to staging
4. QA testing on staging
5. Merge develop to main → Deploy to production

---

## 9. FLUENT UI STANDARDS

### 9.1 Component Usage
**MANDATORY:** Use Microsoft Fluent UI Blazor components ONLY
- NO MudBlazor components
- NO Bootstrap components beyond base CSS reset
- Consistent theming across all pages

### 9.2 Standard Components
```razor
<!-- Forms -->
<FluentTextField @bind-Value="model.FirstName" Label="First Name" Required />
<FluentDatePicker @bind-Value="model.DateOfBirth" Label="Date of Birth" />
<FluentSelect @bind-Value="model.Gender" Label="Gender">
    <FluentOption Value="Male">Male</FluentOption>
    <FluentOption Value="Female">Female</FluentOption>
</FluentSelect>

<!-- Buttons -->
<FluentButton Appearance="Appearance.Accent" OnClick="Submit">Submit</FluentButton>
<FluentButton Appearance="Appearance.Neutral" OnClick="Cancel">Cancel</FluentButton>

<!-- Navigation -->
<FluentNavMenu>
    <FluentNavLink Href="/" Icon="@(new Icons.Regular.Size20.Home())">Home</FluentNavLink>
    <FluentNavLink Href="/registration" Icon="@(new Icons.Regular.Size20.PersonAdd())">Register</FluentNavLink>
</FluentNavMenu>

<!-- Data Display -->
<FluentDataGrid Items="@students">
    <PropertyColumn Property="@(s => s.NbtNumber)" Title="NBT Number" />
    <PropertyColumn Property="@(s => s.FullName)" Title="Full Name" />
</FluentDataGrid>
```

---

## 10. DATA RETENTION & COMPLIANCE

### 10.1 Account Retention
- Student accounts remain active indefinitely for future test cycles
- Academic history and test results preserved
- Inactive accounts archived after 5 years (not deleted)

### 10.2 POPIA Compliance (South African Privacy Law)
- Explicit consent for data collection
- Right to access personal information
- Right to correction of inaccurate data
- Data minimization - collect only necessary information
- Secure deletion upon request (with audit trail)

---

## 11. REGISTRATION WIZARD REQUIREMENTS

### 11.1 Wizard Structure
**Step 1 + 2 Combined: Account & Personal Information**
- Email, Password, Confirm Password
- First Name, Middle Name, Last Name
- SA ID / Foreign ID / Passport
- Date of Birth (auto-filled from SA ID)
- Gender (auto-filled from SA ID)
- Age (calculated from DOB)
- Ethnicity
- Contact Number

**Step 3 + 4 Combined: Academic & Accommodation**
- School Name
- Grade/Year
- Test Type Selection (AQL or AQL+MAT)
- Special Accommodation Needs (Y/N)
- Accommodation Details (if applicable)

**Step 5: Survey Questionnaire**
- Background questionnaire for research and equity reporting
- Optional socio-economic questions
- Academic background questions

**Step 6: Confirmation & NBT Number Generation**
- Review all entered information
- Generate NBT number using Luhn algorithm
- Display NBT number prominently
- Send confirmation email with NBT number

### 11.2 Wizard Persistence
**CRITICAL REQUIREMENT:**
- If registration interrupted, student MUST be able to resume from last completed step
- Auto-save progress after each step completion
- Load saved progress on return (by email match)
- DO NOT require re-entry of completed information

---

## 12. DASHBOARD REQUIREMENTS

### 12.1 Landing Page Structure
**Main Menus:**
1. **Applicants** (with submenus matching current NBT website)
   - About the Tests
   - How to Register
   - Test Dates and Venues
   - Frequently Asked Questions
   - Contact Us

2. **Institutions** (with submenus)
   - Using NBT Results
   - Request Results
   - Research and Reports
   - Partner with NBT

3. **Educators** (with submenus)
   - Teaching Resources
   - Professional Development
   - NBT in the Classroom
   - Assessment Literacy

### 12.2 User Dashboards
**After Login, users directed to role-appropriate dashboard:**

**Applicant Dashboard (Left Sidebar Menu):**
- My Profile
- My Bookings
- My Payments
- My Results
- Book a Test
- Special Session Request
- Download Certificate
- Help & Support

**Staff Dashboard (Left Sidebar Menu):**
- Dashboard Overview
- Manage Applicants
- Manage Bookings
- Manage Payments
- Upload Results
- Manage Venues
- Reports
- Settings

**Admin Dashboard (Left Sidebar Menu):**
- System Overview
- User Management
- Role Management
- Venue Management
- Test Date Configuration
- Payment Configuration
- System Settings
- Audit Logs
- Reports & Analytics

### 12.3 Video Integration
- Embed relevant videos on information pages
- Source videos from current NBT website
- Lazy load videos for performance
- Provide video transcripts for accessibility

---

## 13. DEPLOYMENT CONFIGURATION

### 13.1 Environment Variables
```json
// appsettings.Production.json
{
  "ConnectionStrings": {
    "DefaultConnection": "[Azure SQL Connection]"
  },
  "JwtSettings": {
    "SecretKey": "[Secure Key from Azure Key Vault]",
    "Issuer": "https://nbt.ac.za",
    "Audience": "https://nbt.ac.za",
    "ExpiryMinutes": 30
  },
  "EasyPaySettings": {
    "MerchantId": "[EasyPay Merchant ID]",
    "ApiKey": "[From Azure Key Vault]",
    "WebhookUrl": "https://api.nbt.ac.za/webhooks/easypay"
  },
  "EmailSettings": {
    "SmtpServer": "smtp.office365.com",
    "SmtpPort": 587,
    "FromEmail": "noreply@nbt.ac.za",
    "FromName": "NBT System"
  }
}
```

### 13.2 Azure Resources
- **App Service:** Linux-based, Premium tier
- **SQL Database:** Standard S2 tier minimum
- **Key Vault:** For secrets management
- **Application Insights:** For monitoring
- **CDN:** For static assets
- **Storage Account:** For file uploads (results, certificates)

---

## 14. CONSTITUTION ENFORCEMENT

### 14.1 Code Review Checklist
Before ANY code merge to develop or main:
- [ ] Follows Clean Architecture layers
- [ ] All DTOs have JsonPropertyName attributes
- [ ] All services registered via DI
- [ ] Entity configurations use Fluent API
- [ ] Async/await used for I/O operations
- [ ] Business rules validated in Application layer
- [ ] Audit logging implemented
- [ ] Unit tests added/updated
- [ ] Security rules enforced
- [ ] WCAG compliance verified
- [ ] Performance standards met
- [ ] No MudBlazor components used

### 14.2 Technical Debt Policy
- NO "TODO" comments without associated GitHub issue
- NO commented-out code blocks in commits
- NO hardcoded configuration values
- NO bypass of validation rules
- ALL warnings treated as errors in CI/CD

---

## 15. SUPPORT & MAINTENANCE

### 15.1 Documentation Requirements
- XML comments on ALL public APIs
- README.md for each major module
- Architecture decision records (ADR) for significant changes
- API documentation via Swagger/OpenAPI
- User guides for each role type

### 15.2 Monitoring
- Application Insights telemetry
- Error tracking and alerting
- Performance monitoring
- Usage analytics
- Scheduled health checks

---

## COMPLIANCE STATEMENT

This constitution is **BINDING** for all development work on the NBT Web Application. Deviations require:
1. Written justification
2. Technical lead approval
3. Documentation in ADR
4. Update to this constitution if pattern changes

**Last Updated:** 2025-11-09  
**Review Cycle:** Quarterly  
**Authority:** NBT Technical Lead

---

**END OF CONSTITUTION**
