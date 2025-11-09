# NBT Web Application - Complete SpecKit Implementation
**Version:** 3.0  
**Date:** 2025-11-09  
**Status:** Active - Ready for Implementation

---

## Table of Contents
1. [Constitution - Non-Negotiable Principles](#1-constitution)
2. [Specification - Complete System Requirements](#2-specification)
3. [Implementation Plan - Phase-by-Phase Execution](#3-implementation-plan)
4. [Data Contracts & API Schemas](#4-data-contracts)
5. [Task Breakdown](#5-task-breakdown)
6. [Code Review Checklist](#6-code-review)
7. [Quick Start Guide](#7-quick-start)

---

## 1. CONSTITUTION - Non-Negotiable Principles

### 1.1 Core Technology Stack
✅ **Frontend:** Blazor WebAssembly with **Fluent UI** components (NO MudBlazor)
✅ **Backend:** ASP.NET Core 9.0 Web API
✅ **Database:** MS SQL Server with Entity Framework Core 9.0
✅ **Authentication:** JWT-based with role-based authorization
✅ **Architecture:** Clean Architecture with clear separation of concerns

### 1.2 Architectural Mandates
```
NBT.Domain          → Entities, Enums, ValueObjects, Interfaces
NBT.Application     → Business Logic, DTOs, Services, Validators  
NBT.Infrastructure  → Data Access, External Services, Email, Payment
NBT.WebAPI          → Controllers, Middleware, API Configuration
NBT.WebUI.Client    → Blazor Components, Pages, ViewModels
```

### 1.3 Critical Business Rules

#### Student Registration & NBT Number
- ✅ Students can register with **SA ID**, **Foreign ID**, or **Passport ID**
- ✅ **NBT Number** (14 digits) auto-generated using **Luhn algorithm** upon registration completion
- ✅ For SA ID: **DOB and Gender** automatically extracted
- ✅ **Duplicate prevention** on ID numbers and NBT numbers
- ✅ **Resumable Registration**: If interrupted, student returns to last completed step

#### Test Booking Rules
- ✅ Booking opens **1 April** each year (Intake Year)
- ✅ Student can book **only ONE test at a time**
- ✅ Next booking allowed only after **closing date** of current booking
- ✅ **Maximum 2 tests per year**
- ✅ Tests valid for **3 years** from booking date
- ✅ Can **change booking before closing date**
- ✅ Test Types: **AQL** or **AQL+MAT**

#### Payment Rules (CRITICAL)
- ✅ **Installment payments allowed** until complete
- ✅ Payments applied **in order** of tests being written
- ✅ **Test costs vary by Intake Year**
- ✅ **Payment records uploaded** from bank files (specific format)
- ✅ Only **fully paid** tests visible/downloadable by students
- ✅ **Staff/Admin** can view all tests regardless of payment status
- ✅ EasyPay integration for online payments

#### Test Results Structure
- ✅ **AQL Test**: AL (Academic Literacy) + QL (Quantitative Literacy) scores
- ✅ **MAT Test**: AL + QL + MAT (Mathematics) scores
- ✅ **Performance Levels**: Basic Lower/Upper, Intermediate Lower/Upper, Proficient Lower/Upper
- ✅ **Unique Barcode** per test (distinguishes answer sheets)
- ✅ Students writing 2 tests have **different barcodes**
- ✅ Results include **barcode on certificate**

#### Venue Management
- ✅ **Venue Types**: National, Special Session, Research, Online, Other
- ✅ Venues may be **unavailable** for certain dates
- ✅ **Test Date Calendar** with closing booking dates
- ✅ **Sunday tests highlighted** (different color)
- ✅ **Online tests** written globally with video/sound/internet
- ✅ Online tests have **specific calendar dates**

#### Test Session Linking
- ✅ **TestSession linked to Venue** (NOT Room)
- ✅ Rooms used for allocation within venue

### 1.4 User Activities & Workflows

#### Student/Applicant/Writer Activities
1. **Account Creation & Login**
   - Register new account or sign in
   - OTP verification for secure onboarding
   - Duplicate prevention

2. **NBT Number Generation**
   - 14-digit unique number using Luhn algorithm
   - Links all bookings, payments, and results

3. **Registration Wizard** (Multi-Step)
   - Step 1: Personal & ID Information (SA ID, Foreign ID, Passport)
   - Step 2: Contact & Academic Information (includes Age, Gender, Ethnicity)
   - Step 3: Survey Questionnaire
   - **Auto-save progress** at each step
   - **Resume capability** if interrupted

4. **Booking & Payment**
   - Choose test type (AQL or AQL+MAT)
   - Select venue and date
   - Can book anytime after **1 April** (Intake start)
   - **One test at a time**
   - Can book another only after **closing date** of current booking
   - **Maximum 2 tests per year**
   - **Tests valid 3 years** from booking date
   - Can **change booking before closing date**
   - EasyPay payment reference generated
   - **Installment payments** tracked

5. **Special or Remote Sessions**
   - Off-site testing request
   - Invigilator and venue details
   - Routed to NBT remote administration team

6. **Pre-Test Questionnaire**
   - Online background survey
   - Informs research and equity reporting

7. **Results Access**
   - Secure login to view/download results
   - **Only fully paid tests visible**
   - Download PDF certificate with barcode

8. **Profile Management**
   - Update personal/academic details
   - Upload supporting documents
   - Reset password
   - All edits logged for audit

9. **Notifications**
   - Email/SMS alerts for:
     - Registration confirmation
     - Payment confirmation
     - Test reminders
     - Result availability

10. **Account Retention**
    - Accounts remain active for future access
    - Preserves academic history and results

#### Staff Activities
- CRUD operations for students, payments, results, venues
- View **all tests** regardless of payment status
- Process manual payments
- Import bank payment files
- Generate reports
- Assist students

#### Admin Activities
- All Staff capabilities
- System configuration
- Venue and session management
- User role management
- Full audit access

### 1.5 Security Standards (Non-Negotiable)
- ✅ **HTTPS Only** - All data transfer encrypted
- ✅ **JWT Authentication** with secure storage
- ✅ **Role-Based Access** (Admin, Staff, SuperUser, Student)
- ✅ **Full Audit Logging** - All CRUD operations tracked
- ✅ **Password Policy**: Min 8 chars, uppercase, lowercase, number, special char
- ✅ **Luhn Validation** for NBT and SA ID numbers

### 1.6 Performance Standards
- ✅ Initial page load: **< 3 seconds**
- ✅ API response time: **< 500ms**
- ✅ Database queries: **< 200ms** average

### 1.7 Testing Requirements
- ✅ Unit tests: **80%+ coverage**
- ✅ Integration tests for all API endpoints
- ✅ E2E tests for critical workflows
- ✅ Load testing for concurrent users

### 1.8 CI/CD & GitHub Workflow
- ✅ **Successful builds → Push to GitHub**
- ✅ **New phases → Create branch**
- ✅ **Phase complete → Merge to main** after full testing
- ✅ All tests must pass before merge

### 1.9 Accessibility
- ✅ **WCAG 2.1 AA compliance**
- ✅ Keyboard navigation
- ✅ Screen reader compatible
- ✅ Color contrast ratios (4.5:1 minimum)

---

## 2. SPECIFICATION - Complete System Requirements

### 2.1 Landing Page Structure

#### Public Navigation Menus
Based on current NBT website structure:

**1. APPLICANTS Menu**
- About NBT Tests
- Test Types (AQL, MAT)
- Test Dates & Venues
- How to Register
- How to Book
- Special Accommodations
- FAQs for Applicants
- Contact Us

**2. INSTITUTIONS Menu**
- For Universities
- For TVET Colleges
- For Private Colleges
- Institutional Research
- Bulk Ordering
- Reports & Analytics
- FAQs for Institutions
- Contact Us

**3. EDUCATORS Menu**
- Educator Resources
- Teaching with NBT
- Workshops & Training
- Research Publications
- Assessment Guidelines
- FAQs for Educators
- Contact Us

#### Landing Page Features
- Hero section with NBT overview
- Quick actions: Register, Login, Book Test
- Test calendar preview
- News and announcements
- Video tutorials (embedded where available)
- Testimonials
- Statistics dashboard

### 2.2 Dashboard Structure (After Login)

#### Left-Side Menu Navigation

**Student Dashboard:**
```
📊 Dashboard (Home)
📝 Register / Complete Registration
📅 Book a Test
💳 Payments
   ├─ Make Payment
   ├─ Payment History
   └─ Upload Payment Proof
📄 My Results
   ├─ View Results
   └─ Download Certificates
👤 My Profile
   ├─ Personal Information
   ├─ Contact Details
   ├─ Academic Information
   └─ Change Password
🔔 Notifications
❓ Help & Support
```

**Staff Dashboard:**
```
📊 Dashboard (Overview)
👥 Students
   ├─ Search Students
   ├─ View All
   └─ Registration Requests
📅 Registrations
   ├─ All Registrations
   ├─ By Status
   ├─ By Venue
   └─ By Date
💳 Payments
   ├─ All Payments
   ├─ Process Manual Payment
   ├─ Upload Bank File
   └─ Payment Reports
📄 Results
   ├─ View All Results
   ├─ Import Results
   └─ Release Results
🏢 Venues
   ├─ View Venues
   └─ Venue Availability
📊 Reports
   ├─ Registration Reports
   ├─ Payment Reports
   ├─ Results Reports
   └─ Venue Utilization
📝 Audit Logs
❓ Help & Support
```

**Admin Dashboard:**
```
📊 Dashboard (Overview)
👥 Students (All Staff capabilities)
📅 Registrations (All Staff capabilities)
💳 Payments (All Staff capabilities)
📄 Results (All Staff capabilities)
🏢 Venue Management
   ├─ Create Venue
   ├─ Edit Venues
   ├─ Manage Rooms
   └─ Set Availability
📅 Test Sessions
   ├─ Create Session
   ├─ Session Calendar
   ├─ Manage Capacity
   └─ Session Reports
💰 Pricing Management
   ├─ Set Test Prices
   ├─ Intake Year Pricing
   └─ Special Pricing
👥 User Management
   ├─ Staff Accounts
   ├─ Admin Accounts
   └─ Role Assignment
⚙️ System Configuration
   ├─ Email Templates
   ├─ SMS Settings
   ├─ Payment Gateway
   └─ System Settings
📊 Advanced Reports
   ├─ Financial Reports
   ├─ Demographics
   ├─ Performance Analytics
   └─ Export Data
📝 Full Audit Logs
❓ Help & Support
```

### 2.3 Registration Wizard (Resumable)

#### Step 1: Personal & ID Information
**Fields:**
- ID Type: Dropdown (SA ID, Foreign ID, Passport)
- ID Number: Text input with validation
- **For SA ID**: Auto-extract DOB and Gender
- **For Foreign/Passport**: 
  - Date of Birth (date picker)
  - Gender (dropdown)
  - Nationality (dropdown)
  - Country of Origin (dropdown)
- First Name
- Last Name
- Email (unique, will receive OTP)
- Phone Number

**Validations:**
- SA ID: 13 digits, Luhn check, extract DOB/Gender
- Foreign ID/Passport: 6-20 alphanumeric
- Email: Valid format, unique
- Duplicate check on ID Number

**Progress Saving:**
- Auto-save on "Next" button
- Store progress in database with StudentId
- Flag: `RegistrationStep = 1, IsComplete = false`

#### Step 2: Contact & Academic Information
**Fields:**
- Address Line 1
- Address Line 2 (optional)
- City
- Province/State
- Postal Code
- School Name
- Grade
- Home Language
- **Age** (calculated from DOB, display only)
- **Gender** (auto-filled for SA ID, editable for Foreign/Passport)
- **Ethnicity** (dropdown, optional)
- Special Accommodation Required? (Yes/No)
- If Yes: Describe accommodation needs

**Progress Saving:**
- Auto-save on "Next" button
- Update: `RegistrationStep = 2`

#### Step 3: Survey Questionnaire
**Fields:**
- Why are you taking the NBT? (multi-select)
  - University application
  - Career guidance
  - Self-assessment
  - Other (specify)
- What field do you plan to study? (dropdown)
- Career interests (text area)
- Do you have access to a computer? (Yes/No)
- Do you have internet access? (Yes/No, If yes: Speed)
- Additional comments (text area, optional)

**Completion:**
- Save all survey responses
- Set: `RegistrationStep = 3, IsComplete = true`
- **Generate NBT Number** using Luhn algorithm
- Send **email verification OTP**
- Display success page with NBT number
- **Redirect to Login Page**

#### Resumable Logic
```csharp
// On student login or return to registration:
var student = await _studentRepository.GetByIdOrEmailAsync(idOrEmail);

if (student != null && !student.IsRegistrationComplete)
{
    // Resume from last completed step
    var lastStep = student.RegistrationStep; // 1, 2, or 3
    
    // Redirect to appropriate wizard step
    if (lastStep == 0 || lastStep == 1)
        NavigateTo("/register/step1");
    else if (lastStep == 2)
        NavigateTo("/register/step2");
    else if (lastStep == 3)
        NavigateTo("/register/step3");
}
else if (student != null && student.IsRegistrationComplete)
{
    // Registration complete, go to dashboard
    NavigateTo("/dashboard");
}
```

### 2.4 Payment Upload Feature

#### Bank Payment File Upload
**Purpose:** Staff/Admin can upload bank payment records in bulk

**File Format:**
```csv
StudentNBTNumber,RegistrationNumber,PaymentDate,Amount,TransactionReference,BankName
20240000000123,REG-2024-000001,2024-06-15,500.00,TXN123456,FNB
20240000000456,REG-2024-000002,2024-06-15,250.00,TXN123457,Standard Bank
```

**Process:**
1. Admin navigates to: **Payments → Upload Bank File**
2. Select CSV file
3. System validates:
   - NBT Number exists
   - Registration exists
   - Amount is valid
   - Date format correct
4. Display preview of records to import
5. Confirm import
6. System creates PaymentTransaction records
7. Updates Payment status and balance
8. Sends email confirmations to students
9. Generates import report

**API Endpoint:**
```
POST /api/payments/upload-bank-file
Content-Type: multipart/form-data

Response:
{
  "success": true,
  "recordsProcessed": 150,
  "recordsFailed": 2,
  "errors": [
    {
      "line": 45,
      "error": "NBT Number not found"
    }
  ]
}
```

### 2.5 Video Integration

**Video Locations:**
- Landing page: Hero section video (overview)
- Registration page: "How to Register" tutorial
- Booking page: "How to Book a Test" tutorial
- Payment page: "How to Pay" tutorial
- Results page: "Understanding Your Results" tutorial
- Each dashboard section: Context-sensitive help videos

**Video Sources:**
- Embedded from YouTube/Vimeo
- Hosted on Azure Media Services
- Reference current NBT website for video URLs

**Implementation:**
```razor
<FluentMediaPlayer 
    Src="@VideoUrl"
    Poster="@PosterUrl"
    Controls="true"
    Width="100%"
    MaxWidth="800px" />
```

### 2.6 Core Data Models (Complete)

#### Student Entity
```csharp
public class Student : BaseEntity
{
    // Identity
    public string NBTNumber { get; set; } // 14 digits, Luhn validated
    public IDType IDType { get; set; } // SA_ID, FOREIGN_ID, PASSPORT
    public string IDNumber { get; set; } // Unique
    public string? Nationality { get; set; } // Required for Foreign/Passport
    public string? CountryOfOrigin { get; set; } // Required for Foreign/Passport
    
    // Personal
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; } // Unique
    public string Phone { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; } // Auto from SA ID or manual
    public int Age => CalculateAge(DateOfBirth);
    public string? Ethnicity { get; set; } // Optional
    
    // Address
    public string AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string City { get; set; }
    public string Province { get; set; }
    public string PostalCode { get; set; }
    
    // Academic
    public string SchoolName { get; set; }
    public string Grade { get; set; }
    public string HomeLanguage { get; set; }
    
    // Special Needs
    public bool RequiresSpecialAccommodation { get; set; }
    public string? SpecialAccommodationDetails { get; set; }
    
    // Survey
    public string? TestMotivation { get; set; } // JSON array
    public string? PlannedStudyField { get; set; }
    public string? CareerInterests { get; set; }
    public bool HasComputerAccess { get; set; }
    public bool HasInternetAccess { get; set; }
    public string? InternetSpeed { get; set; }
    public string? SurveyComments { get; set; }
    
    // Registration Progress
    public int RegistrationStep { get; set; } // 0, 1, 2, 3
    public bool IsRegistrationComplete { get; set; }
    public DateTime? RegistrationCompletedDate { get; set; }
    
    // Account
    public bool IsActive { get; set; }
    public bool IsEmailVerified { get; set; }
    public string? EmailVerificationOTP { get; set; }
    public DateTime? OTPExpiry { get; set; }
    
    // Navigation
    public virtual ICollection<Registration> Registrations { get; set; }
    public virtual ICollection<TestResult> TestResults { get; set; }
    public virtual ICollection<RoomAllocation> RoomAllocations { get; set; }
}
```

#### Registration Entity
```csharp
public class Registration : BaseEntity
{
    public string RegistrationNumber { get; set; } // REG-YYYY-NNNNNN
    public Guid StudentId { get; set; }
    public Guid TestSessionId { get; set; }
    
    public RegistrationStatus Status { get; set; } // Pending, Confirmed, Cancelled, Completed
    public string TestTypesSelected { get; set; } // JSON: ["AQL"] or ["AQL","MAT"]
    
    public bool IsRemoteWriter { get; set; }
    public string? RemoteLocation { get; set; }
    public string? SpecialSessionType { get; set; }
    
    public DateTime RegistrationDate { get; set; }
    public DateTime? ConfirmationDate { get; set; }
    public DateTime? CancellationDate { get; set; }
    public string? CancellationReason { get; set; }
    
    // Booking rules tracking
    public DateTime BookingClosingDate { get; set; }
    public bool CanBeModified => DateTime.UtcNow < BookingClosingDate;
    
    // Navigation
    public virtual Student Student { get; set; }
    public virtual TestSession TestSession { get; set; }
    public virtual Payment Payment { get; set; }
    public virtual TestResult? TestResult { get; set; }
}
```

#### Payment Entity (Enhanced)
```csharp
public class Payment : BaseEntity
{
    public Guid RegistrationId { get; set; }
    public string InvoiceNumber { get; set; } // INV-YYYY-NNNNNN
    
    public decimal TotalAmount { get; set; } // Based on test type + intake year
    public decimal AmountPaid { get; set; } // Sum of all transactions
    public decimal Balance => TotalAmount - AmountPaid;
    public bool IsFullyPaid => Balance <= 0;
    
    public PaymentStatus Status { get; set; } // Pending, Partial, Completed, Refunded, Failed
    
    // EasyPay
    public string? EasyPayReference { get; set; } // EP-{RegistrationId}-{Timestamp}
    public string? EasyPayTransactionId { get; set; }
    
    public DateTime? PaidDate { get; set; } // When fully paid
    public DateTime? RefundedDate { get; set; }
    public string? RefundReason { get; set; }
    
    public int IntakeYear { get; set; } // Price calculation
    
    public string? Notes { get; set; }
    
    // Navigation
    public virtual Registration Registration { get; set; }
    public virtual ICollection<PaymentTransaction> Transactions { get; set; }
}
```

#### PaymentTransaction Entity (NEW)
```csharp
public class PaymentTransaction : BaseEntity
{
    public Guid PaymentId { get; set; }
    
    public DateTime TransactionDate { get; set; }
    public decimal Amount { get; set; }
    public PaymentMethod PaymentMethod { get; set; } // EasyPay, EFT, Cash, Card, BankUpload
    public string TransactionReference { get; set; }
    
    public TransactionStatus Status { get; set; } // Success, Failed, Pending
    
    public string? BankName { get; set; } // For bank uploads
    public string? Notes { get; set; }
    
    public string CreatedBy { get; set; } // User who recorded transaction
    
    // Navigation
    public virtual Payment Payment { get; set; }
}
```

#### TestResult Entity (Enhanced with Barcode)
```csharp
public class TestResult : BaseEntity
{
    public Guid StudentId { get; set; }
    public Guid TestSessionId { get; set; }
    public Guid RegistrationId { get; set; }
    
    public string Barcode { get; set; } // BC-{NBTNumber}-{TestDate}-{Sequence}
    public string TestType { get; set; } // AQL, AQL_MAT
    
    // Academic Literacy
    public decimal? ALScore { get; set; }
    public string? ALPerformanceLevel { get; set; }
    
    // Quantitative Literacy
    public decimal? QLScore { get; set; }
    public string? QLPerformanceLevel { get; set; }
    
    // Mathematics (only for AQL_MAT)
    public decimal? MATScore { get; set; }
    public string? MATPerformanceLevel { get; set; }
    
    public string OverallPerformanceBand { get; set; }
    public int? Percentile { get; set; }
    
    public bool IsReleased { get; set; }
    public DateTime TestDate { get; set; }
    public DateTime? ResultDate { get; set; }
    public DateTime? ReleasedDate { get; set; }
    
    // Payment check
    public bool IsAccessibleToStudent => IsReleased && Registration.Payment.IsFullyPaid;
    
    // Navigation
    public virtual Student Student { get; set; }
    public virtual TestSession TestSession { get; set; }
    public virtual Registration Registration { get; set; }
}
```

#### TestSession Entity (Linked to Venue)
```csharp
public class TestSession : BaseEntity
{
    public string SessionCode { get; set; } // CITY-YYYY-MM-DD-PERIOD
    public string SessionName { get; set; }
    
    public DateTime SessionDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    
    // IMPORTANT: Linked to Venue (NOT Room)
    public Guid VenueId { get; set; }
    
    public int Capacity { get; set; }
    public int CurrentRegistrations { get; set; }
    public int AvailableSeats => Capacity - CurrentRegistrations;
    
    public SessionStatus Status { get; set; } // Open, Closed, Completed, Cancelled
    
    public bool IsSpecialSession { get; set; }
    public bool IsOnline { get; set; }
    public bool IsSunday { get; set; }
    
    public string? SpecialSessionNotes { get; set; }
    public string? Notes { get; set; }
    
    // Navigation
    public virtual Venue Venue { get; set; }
    public virtual ICollection<Registration> Registrations { get; set; }
    public virtual ICollection<RoomAllocation> RoomAllocations { get; set; }
    public virtual ICollection<TestResult> TestResults { get; set; }
}
```

#### Venue Entity
```csharp
public class Venue : BaseEntity
{
    public string VenueName { get; set; }
    public string VenueCode { get; set; } // Unique
    public VenueType VenueType { get; set; } // National, SpecialSession, Research, Online, Other
    
    public string Address { get; set; }
    public string City { get; set; }
    public string Province { get; set; }
    public string PostalCode { get; set; }
    
    public string ContactPerson { get; set; }
    public string ContactEmail { get; set; }
    public string ContactPhone { get; set; }
    
    public int TotalCapacity { get; set; }
    public bool IsAccessible { get; set; }
    
    public VenueStatus Status { get; set; } // Active, Inactive, UnderMaintenance
    
    public string? Notes { get; set; }
    
    // Navigation
    public virtual ICollection<Room> Rooms { get; set; }
    public virtual ICollection<TestSession> TestSessions { get; set; }
    public virtual ICollection<VenueAvailability> Availability { get; set; }
}
```

#### VenueAvailability Entity
```csharp
public class VenueAvailability : BaseEntity
{
    public Guid VenueId { get; set; }
    public DateTime TestDate { get; set; }
    public bool IsAvailable { get; set; }
    public string? Reason { get; set; } // e.g., "Under renovation", "Fully booked"
    
    // Navigation
    public virtual Venue Venue { get; set; }
}
```

#### TestDateCalendar Entity
```csharp
public class TestDateCalendar : BaseEntity
{
    public DateTime TestDate { get; set; }
    public DateTime ClosingBookingDate { get; set; }
    
    public bool IsSunday { get; set; }
    public bool IsOnline { get; set; }
    public bool IsActive { get; set; }
    
    public int IntakeYear { get; set; }
    public string? Notes { get; set; }
}
```

#### TestPricing Entity
```csharp
public class TestPricing : BaseEntity
{
    public int IntakeYear { get; set; }
    public string TestType { get; set; } // AQL, AQL_MAT
    public decimal Price { get; set; }
    
    public DateTime EffectiveFrom { get; set; }
    public DateTime? EffectiveTo { get; set; }
    public bool IsActive { get; set; }
}
```

### 2.7 API Endpoints (Complete)

#### Student Endpoints
```
POST   /api/students/register              - Register new student (Step 1)
POST   /api/students/register/step2        - Save Step 2 data
POST   /api/students/register/step3        - Complete registration (generates NBT number)
POST   /api/students/login                 - Student login
GET    /api/students/registration-progress - Check if registration incomplete
GET    /api/students/{id}                  - Get student profile
PUT    /api/students/{id}                  - Update student profile
GET    /api/students/{id}/registrations    - Get student registrations
GET    /api/students/{id}/results          - Get student results (paid only)
GET    /api/students/check-duplicate       - Check duplicate ID
POST   /api/students/verify-otp            - Verify OTP
POST   /api/students/resend-otp            - Resend OTP
```

#### Registration Endpoints
```
POST   /api/registrations                  - Create registration (booking)
GET    /api/registrations/{id}             - Get registration
PUT    /api/registrations/{id}             - Update registration (before closing date)
DELETE /api/registrations/{id}             - Cancel registration
GET    /api/registrations                  - List registrations (staff/admin)
GET    /api/registrations/student/{studentId} - Get student's registrations
GET    /api/registrations/validate-booking - Check if student can book
```

#### Payment Endpoints
```
POST   /api/payments                       - Create payment record
GET    /api/payments/{id}                  - Get payment
PUT    /api/payments/{id}/status           - Update payment status
POST   /api/payments/record-transaction    - Record manual payment
POST   /api/payments/upload-bank-file      - Upload bank payment CSV
POST   /api/payments/easypay-callback      - EasyPay callback
GET    /api/payments/registration/{regId}  - Get registration payment
GET    /api/payments/invoice/{invoiceNumber} - Get payment by invoice
GET    /api/payments/transactions/{paymentId} - Get payment transaction history
```

#### Result Endpoints
```
POST   /api/results/import                 - Import results (admin)
GET    /api/results/student/{studentId}    - Get student results
GET    /api/results/{id}/certificate       - Download certificate PDF
PUT    /api/results/{id}/release           - Release result
GET    /api/results                        - List all results (staff/admin)
GET    /api/results/barcode/{barcode}      - Get result by barcode
```

#### Venue Endpoints
```
POST   /api/venues                         - Create venue
GET    /api/venues/{id}                    - Get venue
PUT    /api/venues/{id}                    - Update venue
DELETE /api/venues/{id}                    - Delete venue
GET    /api/venues                         - List venues
GET    /api/venues/available               - Get available venues
POST   /api/venues/{id}/availability       - Set venue availability
GET    /api/venues/{id}/test-dates         - Get available test dates for venue
```

#### Test Session Endpoints
```
POST   /api/test-sessions                  - Create session
GET    /api/test-sessions/{id}             - Get session
PUT    /api/test-sessions/{id}             - Update session
DELETE /api/test-sessions/{id}             - Delete session
GET    /api/test-sessions                  - List sessions
GET    /api/test-sessions/available        - Get available sessions
GET    /api/test-sessions/date/{date}      - Get sessions by date
```

#### Calendar Endpoints
```
GET    /api/calendar/test-dates            - Get test dates
POST   /api/calendar/test-dates            - Create test date
PUT    /api/calendar/test-dates/{id}       - Update test date
DELETE /api/calendar/test-dates/{id}       - Delete test date
GET    /api/calendar/test-dates/{year}     - Get test dates by year
```

#### Pricing Endpoints
```
GET    /api/pricing/test/{testType}/{year} - Get test price
POST   /api/pricing                        - Create pricing
PUT    /api/pricing/{id}                   - Update pricing
GET    /api/pricing                        - List all pricing
GET    /api/pricing/active/{year}          - Get active pricing for year
```

#### Report Endpoints
```
GET    /api/reports/registrations          - Registration report
GET    /api/reports/payments               - Payment report
GET    /api/reports/results                - Results report
GET    /api/reports/venues                 - Venue utilization report
GET    /api/reports/demographics           - Demographics report
GET    /api/reports/financial              - Financial report
POST   /api/reports/export                 - Export report (Excel/PDF)
```

---

## 3. IMPLEMENTATION PLAN - Phase-by-Phase Execution

### Phase 0: Shell Audit & Gap Analysis
**Objective:** Review existing project and identify missing components

**Tasks:**
1. ✅ Audit all existing entities in NBT.Domain
2. ✅ Check EF Core DbContext configuration
3. ✅ Review existing API controllers
4. ✅ Verify Blazor pages and components
5. ✅ Check service registrations in Program.cs
6. ✅ Verify database migrations
7. ✅ Test existing functionality
8. ✅ Document gaps and incomplete features

**Deliverables:**
- Gap analysis document
- List of missing entities/services
- Migration plan

### Phase 1: Foundation - Complete Domain Model
**Objective:** Ensure all entities and EF Core configuration are complete

**Tasks:**
1. ✅ Add/Update Student entity with resumable registration fields
2. ✅ Add/Update Registration entity with booking rules
3. ✅ Create PaymentTransaction entity
4. ✅ Add/Update Payment entity with installment tracking
5. ✅ Add/Update TestResult entity with barcode
6. ✅ Verify TestSession → Venue relationship (NOT Room)
7. ✅ Add VenueAvailability entity
8. ✅ Add TestDateCalendar entity
9. ✅ Add TestPricing entity
10. ✅ Configure all EF Core relationships
11. ✅ Add indexes for performance
12. ✅ Create/update migrations
13. ✅ Apply migrations to database

**Commands:**
```bash
cd src/NBT.Infrastructure
dotnet ef migrations add CompleteDataModel --startup-project ../NBT.WebAPI
dotnet ef database update --startup-project ../NBT.WebAPI
```

**GitHub:**
```bash
git checkout -b feature/complete-domain-model
# Make changes
dotnet build
dotnet test
git add .
git commit -m "Phase 1: Complete domain model with all entities"
git push origin feature/complete-domain-model
# Create PR and merge to main after review
```

### Phase 2: Registration Wizard (Resumable)
**Objective:** Implement multi-step registration with resume capability

**Tasks:**
1. ✅ Create RegistrationWizard.razor component
2. ✅ Create Step1PersonalInfo.razor
3. ✅ Create Step2ContactAcademic.razor
4. ✅ Create Step3Survey.razor
5. ✅ Implement auto-save on each step
6. ✅ Implement resume logic in StudentService
7. ✅ Create NBT number generation service (Luhn)
8. ✅ Create SA ID validation and extraction
9. ✅ Create OTP generation and verification
10. ✅ Create registration API endpoints
11. ✅ Test wizard flow end-to-end
12. ✅ Test resume functionality

**GitHub:**
```bash
git checkout -b feature/registration-wizard-resumable
# Build and test
dotnet build
dotnet test
# Run app and test manually
dotnet run --project src/NBT.WebUI
# Push
git add .
git commit -m "Phase 2: Resumable registration wizard"
git push origin feature/registration-wizard-resumable
# Merge to main after full testing
```

### Phase 3: Booking & Payment Module
**Objective:** Complete test booking and payment system with installments

**Tasks:**
1. ✅ Create booking validation service
2. ✅ Create test pricing service
3. ✅ Create booking API endpoints
4. ✅ Create payment transaction service
5. ✅ Create EasyPay integration service
6. ✅ Create bank file upload service
7. ✅ Create payment upload page (CSV)
8. ✅ Create booking pages (Blazor)
9. ✅ Create payment pages (Blazor)
10. ✅ Test one-test-at-a-time rule
11. ✅ Test installment payments
12. ✅ Test payment upload

**GitHub:**
```bash
git checkout -b feature/booking-payment-installments
# Build, test, push
dotnet build && dotnet test
git add .
git commit -m "Phase 3: Booking and payment with installments"
git push origin feature/booking-payment-installments
# Merge after testing
```

### Phase 4: Staff/Admin Dashboards
**Objective:** Complete staff and admin functionality

**Tasks:**
1. ✅ Create Staff dashboard layout
2. ✅ Create Admin dashboard layout
3. ✅ Create Student management pages
4. ✅ Create Registration management pages
5. ✅ Create Payment management pages
6. ✅ Create payment upload page
7. ✅ Create Result management pages
8. ✅ Implement left-side navigation
9. ✅ Implement role-based routing
10. ✅ Test all CRUD operations

**GitHub:**
```bash
git checkout -b feature/staff-admin-dashboards
# Build, test, push
dotnet build && dotnet test
git add .
git commit -m "Phase 4: Staff and admin dashboards"
git push origin feature/staff-admin-dashboards
# Merge after testing
```

### Phase 5: Venue & Test Session Management
**Objective:** Complete venue and session management

**Tasks:**
1. ✅ Create Venue management pages
2. ✅ Create Room management pages
3. ✅ Create Test session pages
4. ✅ Create Test date calendar
5. ✅ Implement venue availability
6. ✅ Implement test date highlighting (Sunday, Online)
7. ✅ Create venue/session API endpoints
8. ✅ Test venue-session relationship

**GitHub:**
```bash
git checkout -b feature/venue-session-management
# Build, test, push
dotnet build && dotnet test
git add .
git commit -m "Phase 5: Venue and session management"
git push origin feature/venue-session-management
# Merge after testing
```

### Phase 6: Results & Barcode System
**Objective:** Complete result management with barcode

**Tasks:**
1. ✅ Create result import service
2. ✅ Create barcode generation
3. ✅ Create result visibility logic (payment check)
4. ✅ Create PDF certificate generator (with barcode)
5. ✅ Create result pages (student)
6. ✅ Create result management pages (staff/admin)
7. ✅ Test result import
8. ✅ Test certificate generation

**GitHub:**
```bash
git checkout -b feature/results-barcode-system
# Build, test, push
dotnet build && dotnet test
git add .
git commit -m "Phase 6: Results and barcode system"
git push origin feature/results-barcode-system
# Merge after testing
```

### Phase 7: Landing Page & Navigation
**Objective:** Complete public landing page with menus

**Tasks:**
1. ✅ Create landing page layout
2. ✅ Create Applicants menu with subpages
3. ✅ Create Institutions menu with subpages
4. ✅ Create Educators menu with subpages
5. ✅ Embed videos on relevant pages
6. ✅ Create FAQ pages
7. ✅ Create contact pages
8. ✅ Test navigation

**GitHub:**
```bash
git checkout -b feature/landing-page-navigation
# Build, test, push
dotnet build && dotnet test
git add .
git commit -m "Phase 7: Landing page and navigation"
git push origin feature/landing-page-navigation
# Merge after testing
```

### Phase 8: Reporting & Analytics
**Objective:** Complete reporting module

**Tasks:**
1. ✅ Create report services
2. ✅ Create Excel export service
3. ✅ Create PDF export service
4. ✅ Create report pages
5. ✅ Create data visualization
6. ✅ Test exports

**GitHub:**
```bash
git checkout -b feature/reporting-analytics
# Build, test, push
dotnet build && dotnet test
git add .
git commit -m "Phase 8: Reporting and analytics"
git push origin feature/reporting-analytics
# Merge after testing
```

### Phase 9: Security & Audit
**Objective:** Complete authentication, authorization, and audit logging

**Tasks:**
1. ✅ Implement JWT authentication
2. ✅ Implement role-based authorization
3. ✅ Create audit logging service
4. ✅ Add audit logging to all CRUD operations
5. ✅ Create audit log viewing page
6. ✅ Test security

**GitHub:**
```bash
git checkout -b feature/security-audit
# Build, test, push
dotnet build && dotnet test
git add .
git commit -m "Phase 9: Security and audit logging"
git push origin feature/security-audit
# Merge after testing
```

### Phase 10: Testing & QA
**Objective:** Comprehensive testing

**Tasks:**
1. ✅ Unit tests (80%+ coverage)
2. ✅ Integration tests
3. ✅ E2E tests for all workflows
4. ✅ Performance testing
5. ✅ Security testing
6. ✅ Accessibility testing
7. ✅ Browser compatibility testing

**GitHub:**
```bash
git checkout -b feature/comprehensive-testing
# Run all tests
dotnet test --collect:"XPlat Code Coverage"
# Push test results
git add .
git commit -m "Phase 10: Comprehensive testing"
git push origin feature/comprehensive-testing
# Merge after all tests pass
```

### Phase 11: Deployment & CI/CD
**Objective:** Production deployment

**Tasks:**
1. ✅ Configure GitHub Actions
2. ✅ Setup Azure App Service
3. ✅ Setup Azure SQL Database
4. ✅ Configure connection strings
5. ✅ Setup automated deployments
6. ✅ Configure monitoring
7. ✅ Deploy to production
8. ✅ Smoke tests

**GitHub:**
```bash
# Final production deployment
git checkout main
git pull origin main
# Tag release
git tag -a v1.0.0 -m "Release v1.0.0 - Production ready"
git push origin v1.0.0
# GitHub Actions will deploy automatically
```

---

## 4. DATA CONTRACTS & API SCHEMAS

### 4.1 Request DTOs

#### RegisterStudentStep1Request
```csharp
public class RegisterStudentStep1Request
{
    [Required]
    public IDType IDType { get; set; }
    
    [Required]
    [StringLength(20)]
    public string IDNumber { get; set; }
    
    // For Foreign ID / Passport
    public string? Nationality { get; set; }
    public string? CountryOfOrigin { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    
    [Required]
    [StringLength(100)]
    public string FirstName { get; set; }
    
    [Required]
    [StringLength(100)]
    public string LastName { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    [Phone]
    public string Phone { get; set; }
}
```

#### RegisterStudentStep2Request
```csharp
public class RegisterStudentStep2Request
{
    [Required]
    public Guid StudentId { get; set; }
    
    [Required]
    public string AddressLine1 { get; set; }
    
    public string? AddressLine2 { get; set; }
    
    [Required]
    public string City { get; set; }
    
    [Required]
    public string Province { get; set; }
    
    [Required]
    public string PostalCode { get; set; }
    
    [Required]
    public string SchoolName { get; set; }
    
    [Required]
    public string Grade { get; set; }
    
    [Required]
    public string HomeLanguage { get; set; }
    
    public string? Ethnicity { get; set; }
    
    public bool RequiresSpecialAccommodation { get; set; }
    public string? SpecialAccommodationDetails { get; set; }
}
```

#### RegisterStudentStep3Request
```csharp
public class RegisterStudentStep3Request
{
    [Required]
    public Guid StudentId { get; set; }
    
    public string? TestMotivation { get; set; } // JSON array
    public string? PlannedStudyField { get; set; }
    public string? CareerInterests { get; set; }
    public bool HasComputerAccess { get; set; }
    public bool HasInternetAccess { get; set; }
    public string? InternetSpeed { get; set; }
    public string? SurveyComments { get; set; }
}
```

#### CreateBookingRequest
```csharp
public class CreateBookingRequest
{
    [Required]
    public Guid StudentId { get; set; }
    
    [Required]
    public Guid TestSessionId { get; set; }
    
    [Required]
    public List<string> TestTypes { get; set; } // ["AQL"] or ["AQL", "MAT"]
    
    public bool IsRemoteWriter { get; set; }
    public string? RemoteLocation { get; set; }
    public string? SpecialSessionType { get; set; }
}
```

#### RecordPaymentTransactionRequest
```csharp
public class RecordPaymentTransactionRequest
{
    [Required]
    public Guid PaymentId { get; set; }
    
    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Amount { get; set; }
    
    [Required]
    public PaymentMethod PaymentMethod { get; set; }
    
    [Required]
    public string TransactionReference { get; set; }
    
    public string? BankName { get; set; }
    public string? Notes { get; set; }
}
```

#### UploadBankFileRequest
```csharp
public class UploadBankFileRequest
{
    [Required]
    public IFormFile File { get; set; }
    
    [Required]
    public int IntakeYear { get; set; }
}
```

#### ImportResultsRequest
```csharp
public class ImportResultsRequest
{
    [Required]
    public IFormFile File { get; set; }
    
    [Required]
    public Guid TestSessionId { get; set; }
    
    [Required]
    public DateTime TestDate { get; set; }
}
```

### 4.2 Response DTOs

#### StudentProfileResponse
```csharp
public class StudentProfileResponse
{
    public Guid Id { get; set; }
    public string NBTNumber { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public DateTime DateOfBirth { get; set; }
    public int Age { get; set; }
    public string Gender { get; set; }
    public string? Ethnicity { get; set; }
    public bool IsRegistrationComplete { get; set; }
    public int RegistrationStep { get; set; }
}
```

#### RegistrationResponse
```csharp
public class RegistrationResponse
{
    public Guid Id { get; set; }
    public string RegistrationNumber { get; set; }
    public string StudentName { get; set; }
    public string NBTNumber { get; set; }
    public string VenueName { get; set; }
    public DateTime TestDate { get; set; }
    public List<string> TestTypes { get; set; }
    public string Status { get; set; }
    public bool CanBeModified { get; set; }
    public DateTime BookingClosingDate { get; set; }
    public PaymentSummary Payment { get; set; }
}

public class PaymentSummary
{
    public decimal TotalAmount { get; set; }
    public decimal AmountPaid { get; set; }
    public decimal Balance { get; set; }
    public bool IsFullyPaid { get; set; }
    public string Status { get; set; }
}
```

#### TestResultResponse
```csharp
public class TestResultResponse
{
    public Guid Id { get; set; }
    public string Barcode { get; set; }
    public string TestType { get; set; }
    public DateTime TestDate { get; set; }
    
    public decimal? ALScore { get; set; }
    public string? ALPerformanceLevel { get; set; }
    
    public decimal? QLScore { get; set; }
    public string? QLPerformanceLevel { get; set; }
    
    public decimal? MATScore { get; set; }
    public string? MATPerformanceLevel { get; set; }
    
    public string OverallPerformanceBand { get; set; }
    public int? Percentile { get; set; }
    
    public bool IsAccessible { get; set; }
    public DateTime? ReleasedDate { get; set; }
}
```

---

## 5. TASK BREAKDOWN

### Shell Audit Tasks
- [ ] Review NBT.Domain entities
- [ ] Review NBT.Application services
- [ ] Review NBT.Infrastructure repositories
- [ ] Review NBT.WebAPI controllers
- [ ] Review NBT.WebUI.Client pages
- [ ] Check service registrations
- [ ] Verify database migrations
- [ ] Document gaps

### Phase 1: Domain Model
- [ ] Add PaymentTransaction entity
- [ ] Update Payment entity
- [ ] Update Student entity (registration progress)
- [ ] Add VenueAvailability entity
- [ ] Add TestDateCalendar entity
- [ ] Add TestPricing entity
- [ ] Update TestResult (barcode)
- [ ] Verify TestSession-Venue relationship
- [ ] Configure EF relationships
- [ ] Create migrations
- [ ] Apply migrations

### Phase 2: Registration Wizard
- [ ] Create RegistrationWizard component
- [ ] Create Step1 component
- [ ] Create Step2 component
- [ ] Create Step3 component
- [ ] Implement auto-save
- [ ] Implement resume logic
- [ ] Create NBT number service
- [ ] Create SA ID validation
- [ ] Create OTP service
- [ ] Create registration endpoints
- [ ] Test end-to-end
- [ ] Test resume functionality

### Phase 3: Booking & Payment
- [ ] Create booking validation service
- [ ] Create pricing service
- [ ] Create booking endpoints
- [ ] Create payment transaction service
- [ ] Create EasyPay service
- [ ] Create bank upload service
- [ ] Create booking pages
- [ ] Create payment pages
- [ ] Create upload page
- [ ] Test booking rules
- [ ] Test installments
- [ ] Test uploads

### Phase 4: Dashboards
- [ ] Create Staff layout
- [ ] Create Admin layout
- [ ] Create Student management
- [ ] Create Registration management
- [ ] Create Payment management
- [ ] Create Result management
- [ ] Create navigation menus
- [ ] Implement role-based routing
- [ ] Test CRUD operations

### Phase 5: Venues & Sessions
- [ ] Create Venue pages
- [ ] Create Room pages
- [ ] Create Session pages
- [ ] Create Calendar component
- [ ] Implement availability
- [ ] Implement date highlighting
- [ ] Create endpoints
- [ ] Test relationships

### Phase 6: Results & Barcodes
- [ ] Create import service
- [ ] Create barcode generator
- [ ] Create visibility logic
- [ ] Create PDF generator
- [ ] Create student result pages
- [ ] Create admin result pages
- [ ] Test import
- [ ] Test certificates

### Phase 7: Landing Page
- [ ] Create landing layout
- [ ] Create Applicants menu
- [ ] Create Institutions menu
- [ ] Create Educators menu
- [ ] Embed videos
- [ ] Create FAQ pages
- [ ] Create contact pages
- [ ] Test navigation

### Phase 8: Reporting
- [ ] Create report services
- [ ] Create Excel export
- [ ] Create PDF export
- [ ] Create report pages
- [ ] Create visualizations
- [ ] Test exports

### Phase 9: Security
- [ ] Implement JWT auth
- [ ] Implement authorization
- [ ] Create audit service
- [ ] Add audit logging
- [ ] Create audit pages
- [ ] Test security

### Phase 10: Testing
- [ ] Write unit tests
- [ ] Write integration tests
- [ ] Write E2E tests
- [ ] Performance testing
- [ ] Security testing
- [ ] Accessibility testing

### Phase 11: Deployment
- [ ] Configure GitHub Actions
- [ ] Setup Azure resources
- [ ] Configure connection strings
- [ ] Setup monitoring
- [ ] Deploy
- [ ] Smoke tests

---

## 6. CODE REVIEW CHECKLIST

### Architecture Review
- [ ] Clean Architecture layers respected
- [ ] No circular dependencies
- [ ] Proper dependency injection
- [ ] Repository pattern used
- [ ] Unit of Work implemented

### Entity Review
- [ ] All entities have BaseEntity
- [ ] Navigation properties configured
- [ ] Indexes on frequently queried fields
- [ ] Required fields marked
- [ ] String lengths defined
- [ ] Soft delete implemented

### API Review
- [ ] RESTful conventions followed
- [ ] DTOs used for requests/responses
- [ ] Proper HTTP status codes
- [ ] Error handling implemented
- [ ] Validation on all inputs
- [ ] Authorization on endpoints

### Blazor Review
- [ ] Fluent UI components used (NO MudBlazor)
- [ ] Code-behind pattern used
- [ ] Dependency injection used
- [ ] State management proper
- [ ] Loading states handled
- [ ] Error messages displayed

### Security Review
- [ ] HTTPS enforced
- [ ] JWT tokens used
- [ ] Role-based authorization
- [ ] Input sanitization
- [ ] SQL injection prevented
- [ ] XSS protection
- [ ] Audit logging present

### Performance Review
- [ ] Pagination implemented
- [ ] Lazy loading used
- [ ] Caching where appropriate
- [ ] Async/await used
- [ ] Queries optimized

### Testing Review
- [ ] Unit tests written
- [ ] Integration tests written
- [ ] E2E tests for critical flows
- [ ] Code coverage > 80%
- [ ] All tests passing

---

## 7. QUICK START GUIDE

### Prerequisites
- .NET 9.0 SDK
- Visual Studio 2022 or VS Code
- SQL Server 2019+ or LocalDB
- Node.js 18 LTS (for build tools)
- Git

### Clone Repository
```bash
git clone https://github.com/yourusername/NBTWebApp.git
cd NBTWebApp
```

### Restore Packages
```bash
dotnet restore
```

### Configure Connection String
Edit `src/NBT.WebAPI/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=NBTDatabase;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

### Apply Migrations
```bash
cd src/NBT.Infrastructure
dotnet ef database update --startup-project ../NBT.WebAPI
```

### Run Application
```bash
# Terminal 1: API
cd src/NBT.WebAPI
dotnet run

# Terminal 2: Blazor WebAssembly
cd src/NBT.WebUI
dotnet run
```

### Verify
- API: https://localhost:7001/swagger
- Web: https://localhost:7002

### Run Tests
```bash
dotnet test
```

### Seed Test Data (Optional)
```bash
cd src/NBT.WebAPI
dotnet run --seed
```

### Common Commands
```bash
# Create migration
dotnet ef migrations add MigrationName --startup-project ../NBT.WebAPI

# Update database
dotnet ef database update --startup-project ../NBT.WebAPI

# Rollback migration
dotnet ef database update PreviousMigration --startup-project ../NBT.WebAPI

# Build solution
dotnet build

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"

# Clean solution
dotnet clean
```

### Development Workflow
1. Create feature branch: `git checkout -b feature/feature-name`
2. Make changes
3. Build: `dotnet build`
4. Test: `dotnet test`
5. Run app: `dotnet run`
6. Test manually
7. Commit: `git add . && git commit -m "message"`
8. Push: `git push origin feature/feature-name`
9. Create PR
10. Merge to main after review

### Troubleshooting
- **Migration issues**: Delete database and re-apply migrations
- **Connection issues**: Check connection string
- **Build errors**: Clean solution and restore packages
- **Blazor not loading**: Check API is running first

---

## IMPLEMENTATION NOTES

### Critical Success Factors
1. ✅ **NO MudBlazor** - Use Fluent UI only
2. ✅ **TestSession linked to Venue** - NOT Room
3. ✅ **Resumable registration** - Save progress at each step
4. ✅ **Installment payments** - Track with PaymentTransaction
5. ✅ **Barcode on results** - Unique per test
6. ✅ **Payment-gated results** - Students see only paid tests
7. ✅ **One test at a time** - Enforce booking rules
8. ✅ **GitHub workflow** - Branch → Test → Merge

### Next Steps
1. Run shell audit
2. Start Phase 1: Domain Model
3. Follow phases sequentially
4. Test thoroughly at each phase
5. Push to GitHub after successful build
6. Merge to main after full testing

---

**Document Status:** ✅ READY FOR IMPLEMENTATION  
**Last Updated:** 2025-11-09  
**Version:** 3.0
