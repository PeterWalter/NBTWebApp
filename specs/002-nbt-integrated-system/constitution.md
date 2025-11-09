# NBT Integrated System - Constitution

**Version:** 3.0  
**Last Updated:** 2025-11-09  
**Status:** BINDING - Non-Negotiable Principles  
**Architecture:** Blazor Web App Interactive Auto Mode (.NET 9.0)  
**Scope:** Full NBT Integrated System with Registration, Booking, Payments, Results, and Reporting

---

## IMPORTANT: Critical Relationships & Business Rules

**CRITICAL RELATIONSHIP:** `TestSession` entity is linked to `TestVenue`, NOT to `Room`. 
- A `TestSession` has a many-to-one relationship with `Venue`
- `Room` entities are managed separately and linked to `Venue`
- `RoomAllocation` links students to specific rooms within a venue for a session

**CRITICAL FIX:** All JSON property serialization MUST use proper casing to prevent "property value in JSON" errors
- Use `[JsonPropertyName("propertyName")]` attributes consistently
- Configure JSON serialization options globally
- Test all API endpoints for proper JSON formatting

**CRITICAL CI/CD WORKFLOW:**
- **Successful builds** MUST be followed by a push to GitHub
- **New phases** MUST start with a GitHub branch creation
- **Phase completion** requires full testing before merge to main
- **GitHub Actions** must pass all build, test, and deployment steps
- **Feature branches** follow naming convention: `feature/{phase-name}`
- **Merge strategy**: Squash and merge for clean history

**CRITICAL PAYMENT RULES:**
- **Installment Payments**: Payments can be made in installments until complete
- **Payment Order**: Payments processed in order of tests being written
- **Year-Based Costs**: Test costs vary by intake year
- **Payment Calculation**: System must account for intake year when calculating complete payment
- **View Restrictions**: Only completely paid tests can be viewed/downloaded by students
- **Admin Override**: Staff and Admin can view all tests regardless of payment status

**CRITICAL VENUE RULES:**
- **Venue Types**: National, Special Session, Research, Other (extensible)
- **Availability**: Venues may not be available for certain test dates during the year
- **Test Calendar**: System MUST maintain a table of test dates with:
  - Test date and closing booking date
  - Sunday test highlighting (color-coded)
  - Online test highlighting (written anywhere with computer, video, sound, internet)
  - Specific online test session dates

**CRITICAL RESULT RULES:**
- **Test Types**:
  - AQL Test: AL (Academic Literacy) + QL (Quantitative Literacy) results
  - MAT Test: AL + QL + MAT (Mathematics) results
- **Performance Levels**: Basic Lower, Basic Upper, Intermediate Lower, Intermediate Upper, Proficient Lower, Proficient Upper, etc.
- **Barcode**: Each test has unique barcode distinguishing the actual answer sheet used
  - Multiple test attempts have different barcodes
  - Essential for differentiating test instances
- **Payment Restriction**: Students can only view/download fully paid test results
- **Admin Access**: Staff/Admin can view all results regardless of payment

---

## 1. FOUNDATIONAL PRINCIPLES

### 1.1 Core Mission
This constitution extends the base NBT Web Application constitution to define the immutable architectural, security, and quality standards specifically for the **NBT Integrated System**. This system encompasses:
- Student registration wizard with NBT number generation (14-digit Luhn-validated)
- Support for SA ID, Foreign ID, and Passport ID registration
- Test booking and scheduling system (one active booking per student after intake start date [April 1], max 2 tests/year, 3-year validity)
- Booking change capability before close of booking date
- EasyPay payment integration with automated status updates
- Special sessions and remote writer management with automatic routing to NBT remote administration team
- Pre-test questionnaire completion for research and equity reporting
- Results access (AQL and MAT) and profile management with audit logging
- Staff/Admin dashboards for comprehensive CRUD operations
- Test result imports and management
- Venue and room scheduling with capacity management
- Advanced reporting and analytics (Excel/PDF exports)
- Account retention and automated notifications (email/SMS for registration, payment, test reminders, results)
- Complete end-to-end digital journey compliance with CEA/NBT operational standards

### 1.2 Scope Extension
These principles apply to ALL new modules beyond the base website:
- Registration Wizard module
- Booking Module
- Payment Integration module
- Venue Management module
- Results Management module
- Reporting Module
- Special Sessions module
- All associated APIs, services, and database entities

### 1.3 Shell Project Completion Mandate
**PRINCIPLE:** The existing Blazor WebAssembly + ASP.NET Core Web API shell project MUST be reviewed, audited, and completed. All missing components, incomplete features, and placeholders MUST be identified and fully implemented.

---

## 2. TECHNOLOGY STACK (EXTENDED)

### 2.1 Frontend Stack (Extended)
```yaml
Base Framework: Blazor Web App Interactive Auto (.NET 9.0)
  NOTE: .NET 9 passed Release Candidate (RC) over a year ago and is now stable
UI Library: Microsoft Fluent UI Blazor Components (NO MudBlazor)
Additional Components:
  - FluentDataGrid (paginated tables)
  - FluentWizard (multi-step registration)
  - FluentDialog (modals, confirmations)
  - FluentDatePicker, FluentTimePicker (scheduling)
  - FluentChart (reporting visualizations)
State Management: Cascading parameters + service injection
Form Validation: FluentValidation + DataAnnotations
```

### 2.2 Backend Stack (Extended)
```yaml
Additional Integrations:
  - EasyPay Payment Gateway API
  - Excel import/export (EPPlus or ClosedXML)
  - PDF generation (QuestPDF or iText)
  - Email service (SendGrid or SMTP)
Business Logic:
  - NBT Number Generator (Luhn algorithm)
  - ID Number Validator (South African)
  - Payment Status Tracker
  - Room Capacity Calculator
  - Duplicate Detection Engine
```

### 2.3 Database (Extended Entities)
```yaml
Core Entities (5 existing): User, ContentPage, Announcement, ContactInquiry, DownloadableResource
New Entities (15 required):
  1. Student (personal information, NBT number, ID type [SA_ID|FOREIGN_ID|PASSPORT])
  2. Registration (application data, status tracking)
  3. TestSession (test date, venue, capacity)
  4. Venue (name, location, total capacity, venue type, online flag)
  5. Room (room number, capacity, venue FK)
  6. RoomAllocation (student-room assignment for session)
  7. Payment (amount, status, EasyPay reference, installment tracking, intake year)
  8. PaymentInstallment (payment FK, installment number, amount, status, date paid)
  9. TestResult (test barcode, student FK, test type [AQL|MAT], payment status check)
  10. TestResultDomain (domain type [AL|QL|MAT], performance level, score, testResult FK)
  11. SpecialSession (remote writer, special accommodations)
  12. PreTestQuestionnaire (survey responses, student FK)
  13. TestCalendar (test date, closing date, Sunday flag, online flag)
  14. VenueAvailability (venue FK, test date FK, available flag)
  15. AuditLog (comprehensive activity logging)

Student Entity Extended Fields:
  - IDType: enum (SA_ID, FOREIGN_ID, PASSPORT)
  - IDNumber: string (validated based on IDType)
  - NBTNumber: string (14-digit Luhn-validated unique identifier)
  - Nationality: string (if FOREIGN_ID or PASSPORT)
  - CountryOfOrigin: string (if FOREIGN_ID or PASSPORT)
  - DateOfBirth: DateTime (extracted from SA_ID if applicable)
  - Gender: enum (Male, Female, Other) (extracted from SA_ID if applicable)
  - Age: int (calculated from DateOfBirth, not stored if DOB is present)
  - Ethnicity: string (collected during registration)

Payment Entity Structure:
  - PaymentId: Guid (primary key)
  - StudentId: Guid (foreign key to Student)
  - BookingId: Guid (foreign key to TestBooking)
  - TotalAmount: decimal
  - AmountPaid: decimal (sum of installments)
  - IntakeYear: int (for year-based cost calculation)
  - Status: enum (Pending, Partial, Complete, Failed)
  - EasyPayReference: string (unique)
  - CreatedDate: DateTime
  - UpdatedDate: DateTime

PaymentInstallment Entity Structure:
  - InstallmentId: Guid (primary key)
  - PaymentId: Guid (foreign key to Payment)
  - InstallmentNumber: int
  - Amount: decimal
  - Status: enum (Pending, Paid, Failed)
  - DatePaid: DateTime (nullable)
  - EasyPayTransactionId: string
  - CreatedDate: DateTime

TestResult Entity Structure:
  - TestResultId: Guid (primary key)
  - StudentId: Guid (foreign key to Student)
  - TestSessionId: Guid (foreign key to TestSession)
  - Barcode: string (unique identifier for the actual answer sheet - MANDATORY)
  - TestType: enum (AQL, MAT)
  - TestDate: DateTime
  - IsFullyPaid: bool (computed from Payment status)
  - PaymentId: Guid (foreign key to Payment)
  - CreatedDate: DateTime
  - UpdatedDate: DateTime

Venue Entity Structure:
  - VenueId: Guid (primary key)
  - Name: string
  - Type: enum (National, SpecialSession, Research, Other)
  - Address: string
  - IsOnline: bool
  - TotalCapacity: int
  - CreatedDate: DateTime
  - UpdatedDate: DateTime

TestCalendar Entity Structure:
  - TestCalendarId: Guid (primary key)
  - TestDate: DateTime
  - ClosingDate: DateTime
  - IsSunday: bool (computed)
  - IsOnline: bool
  - TestType: enum (AQL, MAT, Both)
  - IntakeYear: int
  - CreatedDate: DateTime
  - UpdatedDate: DateTime

VenueAvailability Entity Structure:
  - VenueAvailabilityId: Guid (primary key)
  - VenueId: Guid (foreign key to Venue)
  - TestCalendarId: Guid (foreign key to TestCalendar)
  - IsAvailable: bool
  - Notes: string (nullable)
  - CreatedDate: DateTime
  - UpdatedDate: DateTime

TestResultDomain Entity Structure:
  - TestResultDomainId: Guid (primary key)
  - TestResultId: Guid (foreign key to TestResult)
  - DomainType: enum (AL, QL, MAT)
  - Score: decimal (0-100)
  - PerformanceLevel: enum (BasicLower, BasicUpper, IntermediateLower, IntermediateUpper, ProficientLower, ProficientUpper)
  - Percentile: decimal (0-100)
  - CreatedDate: DateTime

PreTestQuestionnaire Entity Structure:
  - QuestionnaireId: Guid (primary key)
  - StudentId: Guid (foreign key to Student)
  - Responses: JSON (survey questions and answers)
  - CompletedDate: DateTime
  - RegistrationId: Guid (foreign key to Registration)

Relationships:
  - Student 1:N Registration
  - Registration 1:1 Payment
  - Payment 1:N PaymentInstallment (installment tracking)
  - TestSession N:1 Venue (CRITICAL: Session belongs to Venue, NOT Room)
  - Venue 1:N Room
  - TestSession 1:N RoomAllocation
  - Room 1:N RoomAllocation
  - Student 1:N RoomAllocation
  - Student 1:N TestResult
  - TestResult N:1 Payment (for payment status validation)
  - TestResult 1:N TestResultDomain (2 for AQL domains [AL, QL], 3 for MAT [AL, QL, MAT])
  - Student 1:N PreTestQuestionnaire
  - Registration 1:1 PreTestQuestionnaire
  - Venue 1:N VenueAvailability
  - TestCalendar 1:N VenueAvailability
  - TestSession N:1 TestCalendar (links session to calendar entry)
```

---

## 3. STUDENT WORKFLOW & BUSINESS RULES

### 3.1 Student Activities Overview
Students (applicants/writers) interact with the NBT Web Application through a structured, self-service workflow. Their activities include:

**Account Creation & Login**
- Register new account with OTP verification
- Duplicate prevention via SA ID, Foreign ID, or Passport number validation
- **FOREIGN ID SUPPORT**: Applicants without a South African ID (SA_ID) can register with a Foreign ID or Passport ID
- Three ID types supported: SA_ID, FOREIGN_ID, PASSPORT
- Secure authentication with password requirements
- Account remains active for future test cycles and preserves history

**NBT Number Generation**
- Automatic generation upon successful registration
- 14-digit format: YYYYSSSSSSSSSC (Year + Sequence + Luhn check digit)
- Unique identifier linking all bookings, payments, and results
- MANDATORY: Must pass Luhn (modulus-10) validation
- Generated once per student and persists across all test cycles
- This identifier links all bookings, payments, and results for the student

**Registration Wizard**
- Multi-step process collecting:
  * Personal details (SA ID, Foreign ID, or Passport ID; contact info)
  * Academic background (school, grade)
  * Test preferences (AQL, MAT, or both)
  * Venue selection
  * Special accommodation requests
- Each step validates inputs and saves progress automatically
- Registration resumable if interrupted

**Booking & Payment Rules** (CRITICAL BUSINESS RULES)
- **Intake Start Date**: Bookings open annually on April 1 (Year Intake start date). Bookings can be done anytime after start of Year Intake.
- **One Active Booking**: A student can only book one test at a time
- **Rebooking Allowed**: Student can book another test ONLY IF the closing date of the currently booked test has passed
- **Annual Limit**: Maximum of 2 tests per year per student
- **Test Validity**: Tests remain valid for 3 years from date of booking (NOT from write date)
- **Booking Changes**: Students can change their booking BEFORE the close of booking date
- **Test Types**: Choose AQL only OR AQL + MAT (both AQL and MAT)
- **Venue Selection**: Choose from available venues with capacity
  * Venues filtered by type (National, Special Session, Research, Other)
  * Venues filtered by availability for selected test date
  * Online venues available for online test dates
- **Test Calendar**: All bookings reference Test Calendar entries showing:
  * Test date and closing booking date
  * Sunday tests (color highlighted)
  * Online tests (special indicator)
- **EasyPay Integration**: System generates EasyPay payment reference upon booking
- **INSTALLMENT PAYMENTS** (NEW):
  * Test payments can be made in installments until complete
  * Payments processed in order of tests being written
  * Test costs vary by intake year (stored per payment record)
  * System calculates complete payment considering intake year
  * Payment status: Pending → Partial (installments) → Complete
  * Each installment tracked separately with amount, date, transaction ID
- **PAYMENT ENFORCEMENT**:
  * Only completely paid tests can be viewed/downloaded by students
  * Partial payments allow booking but restrict result access
  * Staff and Admin can view all tests regardless of payment status
  * Result viewing checks Payment.Status == Complete OR User.Role IN (Staff, Admin, SuperUser)
- **Payment Tracking**: Automated transaction recording and status updates from EasyPay confirmations
- **Status Updates**: Payment confirmations trigger booking status changes
- **ENFORCEMENT**: These rules MUST be validated at both UI and API layers before booking confirmation
- **VALIDATION RULES**: 
  * Cannot book if active booking exists
  * Cannot book if 2 tests already booked in current calendar year
  * Cannot book if session is full (capacity reached)
  * Cannot modify booking after close date
  * Cannot book if venue not available for selected test date
  * Online tests require online test calendar entry


**Special or Remote Sessions**
- Applicants needing off-site testing complete a special form with invigilator and venue details
- Remote session requests routed automatically to the NBT remote administration team
- Additional verification and approval workflow required
- Invigilator details and remote venue information mandatory

**Pre-Test Questionnaire**
- After registration, applicants complete an online background questionnaire
- Information informs research and equity reporting
- Mandatory before test participation
- Data supports NBT research initiatives and operational insights

**Results Access & Structure** (UPDATED WITH PAYMENT RESTRICTIONS)
- Students securely log in to view or download their AQL and MAT results once released
- Results available for 3 years from test date
- **PAYMENT RESTRICTIONS** (CRITICAL):
  * **Student Access**: Can ONLY view/download results for fully paid tests (Payment.Status == Complete)
  * **Staff/Admin Access**: Can view ALL tests regardless of payment status
  * Result viewing API enforces: `WHERE (Payment.Status = 'Complete') OR (User.Role IN ('Staff', 'Admin', 'SuperUser'))`
  * Unpaid result message: "Payment required to view this result. Please complete your payment."
- **Test Result Domains**:
  * **AQL Test**: Generates results for two domains:
    - Academic Literacy (AL) with performance level
    - Quantitative Literacy (QL) with performance level
  * **MAT Test** (AQL + Math): Generates results for three domains:
    - Academic Literacy (AL) with performance level
    - Quantitative Literacy (QL) with performance level
    - Mathematics (MAT) with performance level
- **Performance Levels** (per domain): Basic Lower, Basic Upper, Intermediate Lower, Intermediate Upper, Proficient Lower, Proficient Upper
- **Barcode Uniqueness** (MANDATORY): Each result has a unique barcode that distinguishes the test written (identifies the actual answer sheet used)
  * If a student writes 2 tests, each test has a different barcode
  * Barcode format: System-generated unique alphanumeric identifier (e.g., "NBT20250315-AQL-001234")
  * Essential for differentiating multiple test attempts
  * Barcode printed on PDF certificate for verification
  * Barcode MUST be unique across all results (database constraint)
- Performance band and percentile displayed per domain
- Downloadable PDF certificates with barcode (only for fully paid tests)

**Profile Management**
- Applicants may update personal or academic details
- Upload supporting documents (if required)
- Password reset functionality
- All edits logged for audit tracking and compliance

**Notifications**
- Automated email/SMS alerts confirm:
  * Registration confirmation
  * Payment confirmation
  * Test reminders (7 days before, 1 day before)
  * Result availability
  * Booking modifications
  * Account security events

**Account Retention**
- Accounts remain active for future access or new test cycles
- Preserves academic history and results continuity
- Historical data preserved for reporting
- Access to past results and bookings maintained
- Continuity for multiple test cycles

**SUMMARY**: These activities collectively enable a complete end-to-end digital journey — from registration to test completion — while ensuring data integrity, accessibility, and compliance with CEA/NBT operational standards.

### 3.2 Booking Business Rules (MANDATORY ENFORCEMENT)

```csharp
// MANDATORY: Enforce booking rules at service layer
public interface IBookingValidationService
{
    /// <summary>
    /// Validates if student can book a new test
    /// </summary>
    Task<BookingValidationResult> ValidateNewBooking(Guid studentId, DateTime sessionDate);
    
    /// <summary>
    /// Checks if student has reached annual booking limit (2 tests/year)
    /// </summary>
    Task<bool> HasReachedAnnualLimit(Guid studentId, int year);
    
    /// <summary>
    /// Checks if student has an active booking
    /// </summary>
    Task<bool> HasActiveBooking(Guid studentId);
    
    /// <summary>
    /// Validates booking modification eligibility (before close date)
    /// </summary>
    Task<bool> CanModifyBooking(Guid bookingId, DateTime currentDate);
    
    /// <summary>
    /// Checks test validity (3 years from booking date)
    /// </summary>
    Task<bool> IsTestStillValid(Guid testResultId);
}
```

**PRINCIPLE:** Booking rules MUST be enforced at both client (UI validation) and server (API validation) layers. Database constraints MUST prevent rule violations.

---

## 4. ARCHITECTURAL STANDARDS (EXTENDED)

### 4.1 Multi-Step Registration Wizard
```yaml
Pattern: Wizard Component with Step Navigation
Steps:
  1. Account & Personal Information (Combined Step):
     - ID Type selection (SA_ID, FOREIGN_ID, PASSPORT)
     - ID Number entry with validation
     - Duplicate prevention check
     - Personal details: First Name, Last Name, Email, Phone
     - If SA_ID: Auto-extract DOB and Gender from ID
     - Manual entry: DOB (if not SA_ID), Gender, Ethnicity
     - Age calculated from DOB (not required as separate input)
  
  2. Academic & Test Selection (Combined Step):
     - Academic background (school, grade, year)
     - Test type selection (AQL or MAT)
     - Test preferences and special accommodations
     - Venue and session selection (capacity validation)
  
  3. Pre-Test Questionnaire:
     - Background survey questions for research and equity reporting
     - Multiple choice and short answer questions
     - Save responses to PreTestQuestionnaire entity
  
  4. Review & Confirmation:
     - Summary of all entered information
     - Terms and conditions acceptance
     - Submit registration
     - NBT Number generation (automatic upon submission)
     - Navigate to login page after successful registration

Persistence Strategy: Save draft on each step
Validation: Client + server-side per step
Navigation: Forward (enabled when step is valid), backward, cancel with state preservation
NBT Number Generation: Triggered automatically on final submission, not as separate step
```

**PRINCIPLE:** Registration MUST be resumable. Partial data MUST be persisted to prevent data loss. NBT number is generated server-side upon successful submission, not during wizard navigation.

### 4.2 NBT Number Generation (Luhn Algorithm)
```csharp
// MANDATORY IMPLEMENTATION
public class LuhnValidator : ILuhnValidator
{
    /// <summary>
    /// Generates a valid 14-digit NBT number using Luhn checksum
    /// Format: YYYYSSSSSSSSSC (Year + Sequence + Check digit)
    /// </summary>
    /// <param name="year">Registration year (4 digits)</param>
    /// <param name="sequence">Sequential number (9 digits)</param>
    /// <returns>14-digit NBT number with Luhn check digit</returns>
    public string GenerateNBTNumber(int year, int sequence);
    
    /// <summary>
    /// Validates NBT number using Luhn algorithm
    /// </summary>
    /// <param name="nbtNumber">14-digit NBT number to validate</param>
    /// <returns>True if valid, false otherwise</returns>
    public bool ValidateNBTNumber(string nbtNumber);
    
    /// <summary>
    /// Calculates Luhn check digit for given number
    /// </summary>
    private int CalculateCheckDigit(string number);
}
```

**PRINCIPLE:** ALL NBT numbers MUST be 14 digits and pass Luhn validation. Generation and validation logic MUST be in Domain layer. NBT number uniquely links all bookings, payments, and results for a student.

### 4.3 EasyPay Integration Standards
```yaml
Integration Type: RESTful API
Payment Flow:
  1. Registration submission triggers payment initiation
  2. System generates unique payment reference
  3. Payment request sent to EasyPay API
  4. EasyPay returns payment URL
  5. User redirected to EasyPay portal
  6. EasyPay sends webhook notification on completion
  7. System updates payment status
  8. Registration confirmed/rejected based on payment

Payment States: Pending → Processing → Paid | Failed | Cancelled
Webhook Handling: Idempotent processing, signature verification
Retry Logic: 3 attempts with exponential backoff
Timeout: 30 minutes for payment completion
```

**PRINCIPLE:** Payment integration MUST be asynchronous. Payment status MUST be tracked separately from registration status.

### 4.4 Venue and Room Management
```yaml
Venue Structure:
  - Venue: Physical test center (name, address, total capacity)
  - Room: Specific room within venue (room number, capacity)
  - TestSession: Test event (date, time, venue, type)
  - RoomAllocation: Assignment of student to room for a session

Capacity Management:
  - Venue capacity = sum of all room capacities
  - Session capacity = venue capacity (rooms allocated dynamically)
  - Room allocation happens after booking
  - Overbooking prevention via real-time capacity checks

CRITICAL: TestSession entity has VenueId FK, NOT RoomId
```

**PRINCIPLE:** Capacity validation MUST be enforced at database level with constraints. Overbooking MUST be prevented via pessimistic locking.

### 4.5 Results Import Process
```yaml
Import Type: Excel (.xlsx, .xls)
Validation Steps:
  1. File format validation (headers, columns)
  2. Data type validation (numeric scores, dates)
  3. NBT number validation (Luhn check)
  4. Duplicate detection (NBT number + session)
  5. Student existence verification
  6. Score range validation (0-100)
  7. Business rule validation (result not already exists)

Transaction Handling:
  - All-or-nothing import (rollback on any error)
  - Detailed error reporting with row numbers
  - Success count, error count, skipped count
  - Audit log entry for every import attempt

Error Reporting:
  {
    "TotalRows": 150,
    "SuccessCount": 145,
    "ErrorCount": 5,
    "Errors": [
      { "Row": 12, "Field": "NBTNumber", "Error": "Invalid Luhn checksum" },
      { "Row": 45, "Field": "Score", "Error": "Score must be between 0-100" }
    ]
  }
```

**PRINCIPLE:** All data imports MUST be transactional. Partial imports are FORBIDDEN. Every import MUST be audited.

---

## 4. SECURITY REQUIREMENTS (EXTENDED)

### 4.1 Role-Based Access Control (Extended)
```yaml
Roles (Extended):
  - Staff: 
    * Read-only access to reports and student data
    * View bookings, payments, results
    * Cannot modify data
  
  - Admin:
    * Full CRUD on students, registrations, bookings
    * Full CRUD on payments, results, sessions
    * Full CRUD on venues, rooms
    * Cannot access system settings
  
  - SuperUser:
    * All Admin privileges
    * System configuration and settings
    * User management and role assignments
    * Data imports (students, results)
    * Full audit log access
    * Venue and room management

Authorization Enforcement:
  - [Authorize(Roles = "Staff,Admin,SuperUser")] - Read operations
  - [Authorize(Roles = "Admin,SuperUser")] - CRUD operations
  - [Authorize(Roles = "SuperUser")] - System configuration, imports
```

### 4.2 Data Validation (Extended)
```csharp
// NBT Number Validation (MANDATORY)
public class NBTNumberValidator : AbstractValidator<string>
{
    public NBTNumberValidator()
    {
        RuleFor(x => x)
            .NotEmpty().WithMessage("NBT number is required")
            .Length(9).WithMessage("NBT number must be exactly 9 digits")
            .Matches(@"^\d{9}$").WithMessage("NBT number must contain only digits")
            .Must(ValidateLuhnChecksum).WithMessage("Invalid NBT number (failed Luhn validation)");
    }
}

// South African ID Validation (MANDATORY)
public class SouthAfricanIDValidator : AbstractValidator<string>
{
    public SouthAfricanIDValidator()
    {
        RuleFor(x => x)
            .NotEmpty()
            .Length(13)
            .Matches(@"^\d{13}$")
            .Must(ValidateDatePortion)
            .Must(ValidateGenderDigit)
            .Must(ValidateCitizenshipDigit)
            .Must(ValidateLuhnChecksum);
    }
}

// Foreign ID / Passport Validation (MANDATORY)
public class ForeignIDValidator : AbstractValidator<string>
{
    public ForeignIDValidator()
    {
        RuleFor(x => x)
            .NotEmpty().WithMessage("Foreign ID or Passport number is required")
            .MinimumLength(6).WithMessage("Foreign ID must be at least 6 characters")
            .MaximumLength(20).WithMessage("Foreign ID must not exceed 20 characters")
            .Matches(@"^[A-Z0-9]+$").WithMessage("Foreign ID must contain only uppercase letters and numbers");
    }
}

// Applicant ID Type Validator (supports SA ID, Foreign ID, or Passport)
public class ApplicantIDValidator : AbstractValidator<ApplicantRegistrationDto>
{
    public ApplicantIDValidator()
    {
        When(x => x.IDType == "SA_ID", () =>
        {
            RuleFor(x => x.IDNumber).SetValidator(new SouthAfricanIDValidator());
        });
        
        When(x => x.IDType == "FOREIGN_ID" || x.IDType == "PASSPORT", () =>
        {
            RuleFor(x => x.IDNumber).SetValidator(new ForeignIDValidator());
        });
        
        RuleFor(x => x.IDType)
            .NotEmpty()
            .Must(x => x == "SA_ID" || x == "FOREIGN_ID" || x == "PASSPORT")
            .WithMessage("ID Type must be SA_ID, FOREIGN_ID, or PASSPORT");
    }
}
```

### 4.3 Audit Logging (Extended)
```yaml
Mandatory Logging Events (Extended):
  Registration Module:
    - Registration created, updated, submitted
    - NBT number generated
    - Payment initiated, completed, failed
  
  Booking Module:
    - Session booked, cancelled
    - Room allocated, deallocated
    - Capacity checks (success/failure)
  
  Venue Management:
    - Venue created, updated, deleted
    - Room created, updated, deleted
    - Capacity changes
  
  Results Module:
    - Results imported (file name, row count)
    - Import errors logged with details
    - Individual result created/updated
  
  Reports:
    - Report generated (type, parameters, user)
    - Export completed (format, record count)

Audit Log Schema (Extended):
  {
    "Id": "int",
    "Timestamp": "datetime",
    "UserId": "string",
    "UserEmail": "string",
    "Action": "string", // Create, Update, Delete, Login, Import, Export, Generate
    "Module": "string", // Registration, Booking, Venue, Results, Reports
    "EntityType": "string",
    "EntityId": "string",
    "BeforeValue": "json", // For updates
    "AfterValue": "json",  // For updates/creates
    "IpAddress": "string",
    "UserAgent": "string",
    "Success": "bool",
    "ErrorMessage": "string" // If failed
  }
```

**PRINCIPLE:** EVERY data modification and system event MUST be logged. Audit logs MUST be immutable. Retention: 7 years minimum.

---

## 5. ACCESSIBILITY STANDARDS (EXTENDED)

### 5.1 Wizard Accessibility
```html
<!-- REQUIRED: Wizard steps must be keyboard navigable -->
<FluentWizard @ref="wizardRef" aria-label="Student registration wizard">
    <FluentWizardStep Label="Personal Information" 
                      aria-label="Step 1 of 5: Personal Information"
                      aria-current="@(currentStep == 1 ? "step" : "false")">
    </FluentWizardStep>
</FluentWizard>

<!-- REQUIRED: Step indicators must be accessible -->
<nav aria-label="Registration progress">
    <ol role="list" aria-label="Progress steps">
        <li role="listitem" aria-current="@(currentStep == 1 ? "step" : "false")">
            <span class="step-number">1</span>
            <span class="step-label">Personal Information</span>
        </li>
    </ol>
</nav>
```

### 5.2 Data Grid Accessibility
```html
<!-- REQUIRED: All tables must have proper ARIA attributes -->
<FluentDataGrid Items="@registrations" 
                aria-label="Student registrations"
                role="grid"
                aria-rowcount="@totalRecords"
                aria-colcount="7">
    <PropertyColumn Property="@(r => r.NBTNumber)" 
                    Title="NBT Number"
                    aria-sort="@GetSortDirection("NBTNumber")" />
</FluentDataGrid>
```

**PRINCIPLE:** ALL interactive components (wizards, modals, grids) MUST be fully keyboard-navigable and screen-reader accessible.

---

## 6. PERFORMANCE STANDARDS (EXTENDED)

### 6.1 Load Time Requirements (Extended)
```yaml
Registration Wizard:
  - Step load time: < 300ms
  - NBT number generation: < 100ms
  - Duplicate check: < 200ms
  - Form submission: < 1 second

Admin Dashboards:
  - Initial dashboard load: < 2 seconds
  - Data grid refresh: < 500ms
  - Filter application: < 300ms
  - Export generation: < 5 seconds (for 1000 records)

Import Operations:
  - File upload: < 2 seconds (10MB max)
  - Parsing and validation: < 10 seconds (1000 rows)
  - Database insertion: < 15 seconds (1000 rows)
  - Total import time: < 30 seconds (1000 rows)

Report Generation:
  - Simple reports: < 3 seconds
  - Complex reports: < 10 seconds
  - PDF export: < 5 seconds
  - Excel export: < 7 seconds
```

### 6.2 Database Query Optimization (MANDATORY)
```csharp
// REQUIRED: All list queries must be paginated
public async Task<PagedResult<Registration>> GetRegistrationsAsync(
    int page = 1, 
    int pageSize = 50,
    string sortBy = "CreatedDate",
    bool ascending = false,
    RegistrationFilter filter = null)
{
    var query = _context.Registrations
        .Include(r => r.Student)
        .Include(r => r.Payment)
        .Include(r => r.TestSession)
        .AsNoTracking(); // Read-only queries
    
    if (filter != null)
        query = ApplyFilter(query, filter);
    
    var totalCount = await query.CountAsync();
    
    var items = await query
        .OrderBy(sortBy, ascending)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .Select(r => new RegistrationDto // Projection
        {
            Id = r.Id,
            NBTNumber = r.Student.NBTNumber,
            StudentName = r.Student.FirstName + " " + r.Student.LastName,
            SessionDate = r.TestSession.TestDate,
            PaymentStatus = r.Payment.Status
        })
        .ToListAsync();
    
    return new PagedResult<Registration>(items, totalCount, page, pageSize);
}
```

**PRINCIPLE:** NO queries without pagination. NO lazy loading. Use explicit loading and projections. All queries MUST use AsNoTracking() for read-only operations.

---

## 7. CODE QUALITY STANDARDS (EXTENDED)

### 7.1 Testing Requirements (Extended)
```yaml
Unit Tests (MANDATORY):
  - All validators (NBT number, ID number, capacity)
  - All business logic (NBT generation, payment workflows)
  - All services (RegistrationService, VenueService, etc.)
  - Coverage: 85% minimum

Integration Tests (MANDATORY):
  - All API endpoints (30+ endpoints)
  - Database migrations (up and down)
  - External API integrations (EasyPay mock)
  - Coverage: 100% of API endpoints

UI Tests (MANDATORY):
  - Registration wizard (full flow)
  - Booking workflow
  - Admin CRUD operations
  - Report generation
  - Framework: bUnit

E2E Tests (RECOMMENDED):
  - Critical user journeys (registration → payment → confirmation)
  - Admin workflows (import, export, CRUD)
  - Framework: Playwright
```

### 7.2 Component Structure (MANDATORY)
```
src/NBT.WebUI/Components/
├── Registration/
│   ├── RegistrationWizard.razor
│   ├── PersonalInformationStep.razor
│   ├── NBTNumberGenerationStep.razor
│   ├── TestSessionSelectionStep.razor
│   ├── PaymentStep.razor
│   └── ConfirmationStep.razor
├── Booking/
│   ├── SessionCalendar.razor
│   ├── SessionDetails.razor
│   └── BookingConfirmation.razor
├── Admin/
│   ├── RegistrationManagement.razor
│   ├── VenueManagement.razor
│   ├── RoomManagement.razor
│   ├── ResultsImport.razor
│   └── ReportsExport.razor
├── Common/
│   ├── PagedDataGrid.razor
│   ├── ExportButton.razor
│   ├── ImportDialog.razor
│   └── ConfirmationDialog.razor
└── Shared/
    ├── ValidationSummary.razor
    └── StatusBadge.razor
```

---

## 8. DATA IMPORT/EXPORT STANDARDS

### 8.1 Excel Import (Results)
```yaml
File Requirements:
  - Format: .xlsx or .xls
  - Max size: 10 MB
  - Max rows: 10,000
  - Required columns: NBTNumber, SessionDate, TestType, Score

Validation Rules:
  1. Header validation (exact match)
  2. NBTNumber: Luhn validation, exists in Student table
  3. SessionDate: Valid date, session exists
  4. TestType: Enum validation
  5. Score: Numeric, 0-100 range
  6. Duplicate: NBTNumber + SessionDate + TestType must be unique

Error Handling:
  - Collect all errors before failing
  - Return detailed error report with row numbers
  - Rollback entire transaction on any error
  - Log import attempt (success or failure)

Success Response:
  {
    "Success": true,
    "TotalRows": 1000,
    "ImportedCount": 1000,
    "Errors": []
  }

Error Response:
  {
    "Success": false,
    "TotalRows": 1000,
    "ImportedCount": 0,
    "Errors": [
      { "Row": 12, "Column": "NBTNumber", "Error": "Invalid Luhn checksum" }
    ]
  }
```

### 8.2 Report Export (Excel/PDF)
```yaml
Excel Export:
  - Library: EPPlus or ClosedXML
  - Format: .xlsx
  - Features: Headers, filters, formatting
  - Max records: 50,000

PDF Export:
  - Library: QuestPDF or iText
  - Format: .pdf
  - Features: Headers, footers, page numbers, tables
  - Max records: 10,000

Export Process:
  1. Query data with filters
  2. Generate file in memory
  3. Return as file download
  4. Log export event (user, timestamp, record count)

Performance:
  - Excel: < 7 seconds for 1000 records
  - PDF: < 5 seconds for 1000 records
```

---

## 9. API CONTRACT STANDARDS

### 9.1 Registration API
```csharp
// POST /api/registration/start
[HttpPost("start")]
[AllowAnonymous]
public async Task<ActionResult<RegistrationDto>> StartRegistration([FromBody] StartRegistrationRequest request)
{
    // Validate ID number, check for duplicates
    // Create draft registration with Pending status
    // Return registration ID for continuation
}

// POST /api/registration/generate-nbt-number
[HttpPost("generate-nbt-number")]
[AllowAnonymous]
public async Task<ActionResult<string>> GenerateNBTNumber([FromBody] string idNumber)
{
    // Generate NBT number using Luhn algorithm
    // Ensure uniqueness
    // Associate with registration
}

// POST /api/registration/submit
[HttpPost("submit")]
[AllowAnonymous]
public async Task<ActionResult<RegistrationDto>> SubmitRegistration([FromBody] SubmitRegistrationRequest request)
{
    // Validate all fields
    // Check session capacity
    // Create payment record
    // Initiate EasyPay payment
    // Return payment URL
}

// GET /api/registration/{id}
[HttpGet("{id}")]
[Authorize(Roles = "Staff,Admin,SuperUser")]
public async Task<ActionResult<RegistrationDto>> GetRegistration(int id)

// GET /api/registration
[HttpGet]
[Authorize(Roles = "Staff,Admin,SuperUser")]
public async Task<ActionResult<PagedResult<RegistrationDto>>> GetRegistrations([FromQuery] RegistrationQueryParams query)

// PUT /api/registration/{id}
[HttpPut("{id}")]
[Authorize(Roles = "Admin,SuperUser")]
public async Task<ActionResult<RegistrationDto>> UpdateRegistration(int id, [FromBody] UpdateRegistrationRequest request)

// DELETE /api/registration/{id}
[HttpDelete("{id}")]
[Authorize(Roles = "SuperUser")]
public async Task<ActionResult> DeleteRegistration(int id)
```

### 9.2 Booking API
```csharp
// GET /api/booking/available-sessions
[HttpGet("available-sessions")]
[AllowAnonymous]
public async Task<ActionResult<List<TestSessionDto>>> GetAvailableSessions([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)

// POST /api/booking/book
[HttpPost("book")]
[AllowAnonymous]
public async Task<ActionResult<BookingDto>> BookSession([FromBody] BookSessionRequest request)

// POST /api/booking/cancel
[HttpPost("cancel")]
[Authorize]
public async Task<ActionResult> CancelBooking([FromBody] int bookingId)

// GET /api/booking/my-bookings
[HttpGet("my-bookings")]
[Authorize]
public async Task<ActionResult<List<BookingDto>>> GetMyBookings()
```

### 9.3 Payment API
```csharp
// POST /api/payment/initiate
[HttpPost("initiate")]
[AllowAnonymous]
public async Task<ActionResult<PaymentInitiationDto>> InitiatePayment([FromBody] InitiatePaymentRequest request)

// POST /api/payment/webhook
[HttpPost("webhook")]
[AllowAnonymous]
public async Task<ActionResult> ProcessPaymentWebhook([FromBody] EasyPayWebhookDto webhook)

// GET /api/payment/{id}/status
[HttpGet("{id}/status")]
[Authorize]
public async Task<ActionResult<PaymentStatusDto>> GetPaymentStatus(int id)

// PUT /api/payment/{id}/status
[HttpPut("{id}/status")]
[Authorize(Roles = "Admin,SuperUser")]
public async Task<ActionResult> UpdatePaymentStatus(int id, [FromBody] UpdatePaymentStatusRequest request)
```

### 9.4 Venue API
```csharp
// GET /api/venue
[HttpGet]
[AllowAnonymous]
public async Task<ActionResult<List<VenueDto>>> GetVenues()

// POST /api/venue
[HttpPost]
[Authorize(Roles = "SuperUser")]
public async Task<ActionResult<VenueDto>> CreateVenue([FromBody] CreateVenueRequest request)

// GET /api/venue/{id}/rooms
[HttpGet("{id}/rooms")]
[AllowAnonymous]
public async Task<ActionResult<List<RoomDto>>> GetVenueRooms(int id)

// POST /api/venue/{id}/rooms
[HttpPost("{id}/rooms")]
[Authorize(Roles = "SuperUser")]
public async Task<ActionResult<RoomDto>> CreateRoom(int id, [FromBody] CreateRoomRequest request)

// GET /api/venue/{id}/capacity
[HttpGet("{id}/capacity")]
[AllowAnonymous]
public async Task<ActionResult<VenueCapacityDto>> GetVenueCapacity(int id, [FromQuery] DateTime testDate)
```

### 9.5 Results API
```csharp
// POST /api/results/import
[HttpPost("import")]
[Authorize(Roles = "SuperUser")]
public async Task<ActionResult<ImportResultDto>> ImportResults([FromForm] IFormFile file)

// GET /api/results/{nbtNumber}
[HttpGet("{nbtNumber}")]
[Authorize]
public async Task<ActionResult<TestResultDto>> GetResultByNBTNumber(string nbtNumber)

// GET /api/results
[HttpGet]
[Authorize(Roles = "Staff,Admin,SuperUser")]
public async Task<ActionResult<PagedResult<TestResultDto>>> GetResults([FromQuery] ResultQueryParams query)

// PUT /api/results/{id}
[HttpPut("{id}")]
[Authorize(Roles = "Admin,SuperUser")]
public async Task<ActionResult<TestResultDto>> UpdateResult(int id, [FromBody] UpdateResultRequest request)

// DELETE /api/results/{id}
[HttpDelete("{id}")]
[Authorize(Roles = "SuperUser")]
public async Task<ActionResult> DeleteResult(int id)
```

### 9.6 Reports API
```csharp
// GET /api/reports/registrations
[HttpGet("registrations")]
[Authorize(Roles = "Staff,Admin,SuperUser")]
public async Task<ActionResult<RegistrationReportDto>> GetRegistrationReport([FromQuery] ReportQueryParams query)

// GET /api/reports/registrations/export
[HttpGet("registrations/export")]
[Authorize(Roles = "Staff,Admin,SuperUser")]
public async Task<FileResult> ExportRegistrations([FromQuery] ExportQueryParams query)
// Returns Excel or PDF based on query.Format

// GET /api/reports/payments
[HttpGet("payments")]
[Authorize(Roles = "Staff,Admin,SuperUser")]
public async Task<ActionResult<PaymentReportDto>> GetPaymentReport([FromQuery] ReportQueryParams query)

// GET /api/reports/results
[HttpGet("results")]
[Authorize(Roles = "Staff,Admin,SuperUser")]
public async Task<ActionResult<ResultsReportDto>> GetResultsReport([FromQuery] ReportQueryParams query)

// GET /api/reports/venue-utilization
[HttpGet("venue-utilization")]
[Authorize(Roles = "Admin,SuperUser")]
public async Task<ActionResult<VenueUtilizationReportDto>> GetVenueUtilizationReport([FromQuery] VenueReportQueryParams query)
```

---

## 10. SHELL PROJECT AUDIT REQUIREMENTS

### 10.1 Mandatory Audit Tasks
```yaml
Phase 1: Project Structure Audit
  - Verify all 5 projects exist (Domain, Application, Infrastructure, WebAPI, WebUI)
  - Check Directory.Build.props configuration
  - Verify NuGet package references
  - Confirm .NET 8 target framework

Phase 2: Domain Layer Audit
  - Verify all 15 entities exist (5 existing + 10 new)
  - Check entity relationships and foreign keys
  - Validate Luhn validator implementation
  - Verify ID number validator
  - Confirm enums (PaymentStatus, RegistrationStatus, TestType, etc.)

Phase 3: Application Layer Audit
  - Verify DTOs for all entities
  - Check service interfaces (IRegistrationService, IVenueService, etc.)
  - Validate command/query handlers (if using CQRS)
  - Confirm FluentValidation validators
  - Verify AutoMapper profiles

Phase 4: Infrastructure Layer Audit
  - Check DbContext configuration
  - Verify entity configurations (Fluent API)
  - Validate migrations (up and down)
  - Confirm repository implementations
  - Check EasyPay integration service
  - Verify email service implementation

Phase 5: WebAPI Audit
  - Verify all 6 controller groups (Registration, Booking, Payment, Venue, Results, Reports)
  - Check authorization attributes
  - Validate request/response models
  - Confirm Swagger documentation
  - Verify error handling middleware

Phase 6: WebUI Audit
  - Check registration wizard implementation
  - Verify booking components
  - Validate admin dashboard pages
  - Confirm Fluent UI component usage
  - Check service registrations in Program.cs
  - Verify appsettings.json configuration
```

### 10.2 Gap Identification Checklist
```yaml
Missing Components:
  - [ ] NBT number generator service
  - [ ] Luhn validator implementation
  - [ ] ID number validator implementation
  - [ ] Registration wizard component (5 steps)
  - [ ] Booking calendar component
  - [ ] Payment integration service
  - [ ] Excel import service
  - [ ] Report generation service
  - [ ] Email notification service
  - [ ] Capacity validation service

Missing API Endpoints:
  - [ ] POST /api/registration/start
  - [ ] POST /api/registration/generate-nbt-number
  - [ ] POST /api/registration/submit
  - [ ] GET /api/booking/available-sessions
  - [ ] POST /api/booking/book
  - [ ] POST /api/payment/initiate
  - [ ] POST /api/payment/webhook
  - [ ] POST /api/results/import
  - [ ] GET /api/reports/registrations/export
  - [ ] GET /api/venue/{id}/capacity

Missing Pages:
  - [ ] /register (registration wizard)
  - [ ] /booking (session booking)
  - [ ] /payment (payment confirmation)
  - [ ] /my-bookings (user bookings)
  - [ ] /admin/registrations (admin CRUD)
  - [ ] /admin/venues (venue management)
  - [ ] /admin/rooms (room management)
  - [ ] /admin/results-import (import UI)
  - [ ] /admin/reports (report generation)

Missing Database Entities:
  - [ ] Student (if not exists)
  - [ ] Registration
  - [ ] Payment
  - [ ] TestSession
  - [ ] Venue
  - [ ] Room
  - [ ] RoomAllocation
  - [ ] TestResult
  - [ ] SpecialSession
  - [ ] AuditLog (extended)

Missing Configurations:
  - [ ] EasyPay API settings in appsettings.json
  - [ ] SMTP settings for email
  - [ ] File upload limits and paths
  - [ ] Report generation settings
  - [ ] Luhn algorithm seed values
```

---

## 11. WORKFLOW TRACEABILITY (MANDATORY)

### 11.1 Registration Workflow
```yaml
Step 1: User visits /register
  - Component: RegistrationWizard.razor
  - Service: IRegistrationService.StartRegistration()
  - API: POST /api/registration/start
  - Database: INSERT INTO Registrations (Status = Draft)

Step 2: User enters personal information
  - Component: PersonalInformationStep.razor
  - Validation: IDNumberValidator, duplicate check
  - Service: IStudentService.ValidateStudent()
  - Database: SELECT Students WHERE IDNumber = ?

Step 3: System generates NBT number
  - Component: NBTNumberGenerationStep.razor
  - Service: ILuhnValidator.GenerateNBTNumber()
  - API: POST /api/registration/generate-nbt-number
  - Database: UPDATE Registrations SET NBTNumber = ?

Step 4: User selects test session
  - Component: TestSessionSelectionStep.razor
  - Service: ITestSessionService.GetAvailableSessions()
  - API: GET /api/booking/available-sessions
  - Validation: Capacity check via IVenueService.CheckCapacity()

Step 5: User initiates payment
  - Component: PaymentStep.razor
  - Service: IPaymentService.InitiatePayment()
  - API: POST /api/payment/initiate
  - External: Call EasyPay API
  - Database: INSERT INTO Payments (Status = Pending)

Step 6: User confirms registration
  - Component: ConfirmationStep.razor
  - Service: IRegistrationService.SubmitRegistration()
  - API: POST /api/registration/submit
  - Database: UPDATE Registrations SET Status = Submitted

Step 7: EasyPay webhook received
  - API: POST /api/payment/webhook
  - Service: IPaymentService.ProcessWebhook()
  - Database: UPDATE Payments SET Status = Paid
  - Database: UPDATE Registrations SET Status = Confirmed
  - Service: IEmailService.SendConfirmationEmail()
```

### 11.2 Booking Workflow
```yaml
Step 1: User views available sessions
  - Page: /booking
  - Component: SessionCalendar.razor
  - Service: ITestSessionService.GetAvailableSessions()
  - API: GET /api/booking/available-sessions
  - Database: SELECT TestSessions WHERE TestDate >= ? AND (SELECT COUNT(*) FROM RoomAllocations WHERE SessionId = TestSessions.Id) < VenueCapacity

Step 2: User selects session
  - Component: SessionDetails.razor
  - Displays: Date, venue, room availability, price

Step 3: User books session
  - Component: BookingConfirmation.razor
  - Service: IBookingService.BookSession()
  - API: POST /api/booking/book
  - Validation: Capacity check, duplicate booking check
  - Database: INSERT INTO RoomAllocations (StudentId, SessionId, RoomId = NULL) // Room assigned later by admin

Step 4: Payment flow (same as registration)
```

### 11.3 Results Import Workflow
```yaml
Step 1: Admin uploads Excel file
  - Page: /admin/results-import
  - Component: ResultsImport.razor
  - API: POST /api/results/import (multipart/form-data)

Step 2: System validates file
  - Service: IResultsImportService.ValidateFile()
  - Checks: File format, size, headers

Step 3: System parses and validates data
  - Service: IResultsImportService.ParseAndValidate()
  - Validation: NBT numbers, scores, duplicates
  - Collects all errors

Step 4: System imports or rejects
  - If errors: Return error report
  - If valid: Begin transaction
  - Database: INSERT INTO TestResults (bulk insert)
  - Database: INSERT INTO AuditLog (import event)
  - Commit or rollback

Step 5: Admin views import summary
  - Component: ImportResultSummary.razor
  - Displays: Success count, error count, detailed error list
```

### 11.4 Venue Management Workflow
```yaml
Step 1: Admin creates venue
  - Page: /admin/venues
  - Component: VenueManagement.razor
  - API: POST /api/venue
  - Database: INSERT INTO Venues

Step 2: Admin adds rooms to venue
  - Component: RoomManagement.razor
  - API: POST /api/venue/{id}/rooms
  - Database: INSERT INTO Rooms (VenueId = ?)
  - Validation: Room capacity <= venue total capacity

Step 3: Admin creates test session
  - Page: /admin/sessions
  - API: POST /api/session
  - Database: INSERT INTO TestSessions (VenueId = ?) // NOT RoomId
  - Validation: Session date must be future date

Step 4: System calculates session capacity
  - Service: IVenueService.GetVenueCapacity(venueId, sessionDate)
  - Query: SELECT SUM(Capacity) FROM Rooms WHERE VenueId = ?
  - Returns: Available capacity for session

Step 5: Admin allocates students to rooms
  - Component: RoomAllocationDialog.razor
  - API: PUT /api/room-allocation
  - Database: UPDATE RoomAllocations SET RoomId = ? WHERE SessionId = ? AND StudentId = ?
```

### 11.5 Reporting Workflow
```yaml
Step 1: Staff/Admin accesses reports
  - Page: /admin/reports
  - Component: ReportsExport.razor

Step 2: User selects report type and filters
  - Options: Registrations, Payments, Results, Venue Utilization
  - Filters: Date range, status, venue, etc.

Step 3: User generates report
  - API: GET /api/reports/{reportType}
  - Service: IReportService.GenerateReport()
  - Database: Complex queries with joins and aggregations
  - Returns: Report data in JSON

Step 4: User exports report (optional)
  - API: GET /api/reports/{reportType}/export?format=excel|pdf
  - Service: IExportService.ExportToExcel() or ExportToPDF()
  - Returns: File download

Step 5: System logs report generation
  - Database: INSERT INTO AuditLog (Action = "GenerateReport", EntityType = "Report")
```

---

## 12. COMPLIANCE CHECKLIST (EXTENDED)

Before any deployment or phase completion, verify:

**Architecture:**
- [ ] All 5 projects follow Clean Architecture
- [ ] All dependencies flow inward (no circular dependencies)
- [ ] All services registered via DI
- [ ] Repository pattern used for all data access

**Security:**
- [ ] All API endpoints have authorization attributes
- [ ] NBT numbers validated with Luhn algorithm
- [ ] ID numbers validated per SA ID rules
- [ ] HTTPS enforced, HSTS enabled
- [ ] JWT authentication configured correctly
- [ ] Refresh token mechanism working
- [ ] Role-based access control enforced (Staff, Admin, SuperUser)
- [ ] Audit logs implemented for all CRUD operations

**Validation:**
- [ ] Luhn validator implemented and tested
- [ ] ID number validator implemented and tested
- [ ] Capacity validation working (prevents overbooking)
- [ ] Duplicate detection working (registration, results)
- [ ] All form inputs validated (client + server)

**Data Integrity:**
- [ ] All database relationships defined with foreign keys
- [ ] TestSession linked to Venue (NOT Room)
- [ ] RoomAllocation links Student to Room within a Session
- [ ] Cascade delete rules configured
- [ ] Unique constraints on NBT numbers
- [ ] Check constraints on scores (0-100)

**Functionality:**
- [ ] Registration wizard works end-to-end (5 steps)
- [ ] NBT number generation produces valid numbers
- [ ] Booking workflow prevents overbooking
- [ ] Payment integration connects to EasyPay (or mock)
- [ ] Results import validates and imports correctly
- [ ] Room allocation assigns students to rooms
- [ ] Reports generate and export correctly

**Performance:**
- [ ] All list queries paginated (page size 50)
- [ ] Lazy loading disabled, explicit loading used
- [ ] AsNoTracking() used for read-only queries
- [ ] Projections used to avoid loading full entities
- [ ] Registration wizard loads in < 2 seconds
- [ ] Admin dashboards load in < 2 seconds
- [ ] Import processes 1000 rows in < 30 seconds

**Testing:**
- [ ] Unit tests written for all validators
- [ ] Unit tests written for all business logic
- [ ] Integration tests written for all API endpoints
- [ ] UI tests written for critical workflows
- [ ] Code coverage >= 85%

**Accessibility:**
- [ ] WCAG 2.1 AA compliance verified
- [ ] All forms keyboard-navigable
- [ ] All components have ARIA labels
- [ ] Color contrast ratio >= 4.5:1
- [ ] Screen reader tested

**Documentation:**
- [ ] All API endpoints documented in Swagger
- [ ] All public methods have XML comments
- [ ] README updated with setup instructions
- [ ] Architecture diagrams updated
- [ ] Database schema documented

**Deployment:**
- [ ] CI/CD pipeline passing all stages
- [ ] Health check endpoint working
- [ ] Environment-specific configurations
- [ ] Secrets in Key Vault (not config files)
- [ ] Database migrations applied successfully

---

## 13. AMENDMENT PROCESS

This constitution extends the base NBT Web Application constitution. Amendments follow the same process:

1. **Proposal:** Documented RFC
2. **Review:** Architecture review board
3. **Testing:** Impact analysis
4. **Approval:** Technical lead sign-off
5. **Implementation:** Update CI/CD rules
6. **Communication:** Team notification

**CRITICAL CHANGES IN THIS VERSION:**
- TestSession now linked to Venue (not Room)
- RoomAllocation entity added to manage student-room assignments
- Comprehensive workflow traceability added
- Extended audit logging requirements
- Shell project audit requirements added

---

## SIGNATURES

**Effective Date:** 2025-11-08  
**Constitution Version:** 2.0  
**Architecture:** Blazor Web App Interactive Auto + ASP.NET Core Web API  
**Scope:** Full NBT Integrated System  
**Next Review Date:** 2026-11-08

---

*This constitution is binding for all contributors to the NBT Integrated System. All code, configurations, and deployments MUST comply with these principles without exception.*
