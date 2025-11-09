# NBT Integrated Web Application - Complete Specification

## Document Control
- **Version**: 2.0
- **Last Updated**: 2025-11-09
- **Status**: ACTIVE - Extended with all requirements
- **Related**: CONSTITUTION.md, IMPLEMENTATION-PLAN.md

---

## 1. System Overview

### 1.1 Purpose
The NBT Integrated Web Application is a complete digital platform for managing the National Benchmark Tests (NBT) in South Africa. It provides end-to-end functionality from student registration through test booking, payment processing, test administration, and results delivery.

### 1.2 Technology Stack
- **Frontend**: Blazor Web App with Interactive Auto render mode
- **UI Framework**: Microsoft Fluent UI Blazor Components (NO MudBlazor)
- **Backend**: ASP.NET Core 9.0 Web API
- **Database**: Microsoft SQL Server
- **Architecture**: Clean Architecture with DDD principles
- **Authentication**: JWT with refresh tokens
- **Payment**: EasyPay integration
- **Reporting**: Excel/PDF exports

### 1.3 User Roles
1. **Applicant/Student**: Register, book tests, make payments, view results
2. **Staff**: CRUD operations, payment processing, result imports
3. **Admin**: Full system access, user management, configuration
4. **SuperUser**: System administration, audit logs, advanced configuration

---

## 2. Student/Applicant Activities

### 2.1 Account Creation & Login
- **Registration**: New account creation with duplicate prevention
- **OTP Verification**: Secure onboarding via email/SMS
- **SA ID Support**: Automatic DOB and Gender extraction from valid SA ID
- **Foreign ID Support**: Registration with Foreign ID or Passport for non-SA applicants
- **Login**: Secure authentication with JWT tokens
- **Password Reset**: Self-service password recovery

### 2.2 NBT Number Generation
- **Algorithm**: Luhn (modulus-10) checksum validation
- **Format**: 14-digit unique identifier (YYYYNNNNNNNNC)
- **Trigger**: Automatic generation upon successful registration
- **Uniqueness**: Globally unique across all students
- **Linking**: Connects all bookings, payments, and results

### 2.3 Registration Wizard
**Multi-Step Process**:

**Step 1: Account & Personal Information (Combined)**
- Account credentials (email, password)
- Personal details (Name, Surname, DOB)
- SA ID (with auto-extraction of DOB and Gender) OR Foreign ID/Passport
- Age (calculated from DOB, not manually entered)
- Gender (auto-extracted from SA ID or manually selected)
- Ethnicity
- Contact information (phone, email)

**Step 2: Academic & Test Preferences (Combined)**
- Current grade/level
- School/Institution details
- Test type selection (AQL or AQL + MAT)
- Preferred test language
- Special accommodations required

**Step 3: Venue & Booking Selection**
- Venue selection from available options
- Preferred test date (from calendar)
- Venue type (National, Special Session, Online)
- Room capacity awareness

**Step 4: Background Survey Questionnaire**
- Pre-test questionnaire
- Background information
- Research and equity reporting data
- Optional demographic information

**Features**:
- **Progress Saving**: Automatic save at each step
- **Resume Capability**: Continue from where left off if interrupted
- **Validation**: Real-time input validation
- **Navigation**: Next/Previous/Save & Exit
- **Duplicate Check**: Prevent multiple registrations with same ID

### 2.4 Booking & Payment

#### 2.4.1 Booking Rules
- **One Active Booking**: Student can only book one test at a time
- **Next Booking**: Allowed only after closing date of previous test has passed
- **Annual Limit**: Maximum 2 tests per year
- **Test Validity**: 3 years from booking date
- **Booking Changes**: Allowed before closing date
- **Booking Start**: Anytime after Year Intake starts (usually April 1st)

#### 2.4.2 Payment Processing
- **EasyPay Integration**: Payment reference generation
- **Installments**: Payments can be made in installments until complete
- **Payment Order**: Oldest unpaid test is paid first
- **Variable Costs**: Test costs vary by intake year
- **Bank Uploads**: Support for bank payment file uploads (specific format)
- **Status Tracking**: Real-time payment status updates
- **Confirmation**: Automatic status update upon payment confirmation

#### 2.4.3 Payment Rules
- **Full Payment Required**: Only completely paid tests can be viewed/downloaded by students
- **Staff/Admin Access**: Can view all tests regardless of payment status
- **PDF Certificate**: Available for download only for fully paid tests

### 2.5 Special & Remote Sessions
- **Off-Site Testing**: Request form for remote testing
- **Invigilator Details**: Capture invigilator information
- **Venue Details**: Remote venue specifications
- **Automatic Routing**: Forms routed to NBT remote administration team
- **Approval Workflow**: Admin approval required

### 2.6 Test Calendar & Scheduling
- **Test Dates Table**: Available test dates with closing dates
- **Sunday Tests**: Highlighted with specific color
- **Online Tests**: Highlighted separately
- **Online Requirements**: Computer with video, sound, and internet
- **Online Dates**: Specific calendar dates for online tests
- **Global Access**: Online tests can be written from anywhere in the world

### 2.7 Results Access
- **Secure Login**: JWT-authenticated access
- **Result Types**:
  - **AQL Test**: Academic Literacy (AL) + Quantitative Literacy (QL)
  - **MAT Test**: AL + QL + Mathematics (MAT)
- **Performance Levels**: For each domain (e.g., Basic Lower, Basic Upper, Intermediate Lower, Proficient Lower, Proficient Upper)
- **Barcodes**: Unique barcode per test distinguishing the actual answer sheet
- **Multiple Tests**: Different barcodes differentiate tests
- **Visibility Rules**:
  - Students: Only fully paid tests
  - Staff/Admin: All tests regardless of payment status
- **Download**: PDF certificate for fully paid tests
- **Result Release**: Automated notification when results available

### 2.8 Profile Management
- **Update Details**: Personal and academic information
- **Document Upload**: Supporting documents
- **Password Change**: Self-service password management
- **Audit Trail**: All edits logged for tracking

### 2.9 Notifications
- **Email Alerts**: Registration confirmation, payment confirmation, test reminders, results availability
- **SMS Alerts**: Critical notifications
- **In-App Notifications**: Dashboard notifications
- **Preferences**: Notification preferences management

### 2.10 Account Retention
- **Persistent Accounts**: Remain active for future access
- **History Preservation**: Academic history and results continuity
- **Multi-Year Access**: Support for multiple test cycles

---

## 3. Staff/Admin Dashboards

### 3.1 Dashboard Layout
- **Landing Page Redirect**: Authenticated users redirected to role-appropriate dashboard
- **Left-Side Menu**: Navigation structure
- **Dashboard Widgets**: Quick access to key functions
- **Role-Based Views**: Content based on user role

### 3.2 Student Management
- **Search & Filter**: Find students by various criteria
- **CRUD Operations**: Create, Read, Update, Delete student records
- **Registration Review**: View and edit registration details
- **NBT Number Lookup**: Search by NBT number
- **Account Status**: Enable/disable accounts

### 3.3 Payment Management
- **Payment Dashboard**: Overview of all payments
- **Payment Status**: Track payment progress
- **Installment Tracking**: Monitor installment payments
- **Bank Upload**: Process bank payment files
- **Manual Adjustments**: Admin-only payment corrections
- **Payment Reports**: Generate payment summaries

### 3.4 Results Management
- **Result Import**: Bulk import from test centers
- **Result Entry**: Manual result entry if needed
- **Result Validation**: Verify result data integrity
- **Barcode Management**: Link barcodes to specific tests
- **Result Release**: Control when results are visible
- **Result Reports**: Generate result analytics

### 3.5 Venue Management
- **Venue Types**: National, Special Session, Research, Other
- **CRUD Operations**: Create, Read, Update, Delete venues
- **Date Availability**: Manage venue availability by date
- **Capacity Management**: Track venue capacity
- **Room Management**: Manage rooms within venues (for information only)
- **Test Session Linkage**: Link test sessions to venues (not individual rooms)

### 3.6 Test Session Management
- **Session Creation**: Create new test sessions
- **Date & Time**: Specify session date and time
- **Venue Assignment**: Link session to venue
- **Capacity Tracking**: Monitor registrations vs. capacity
- **Session Status**: Active, Full, Completed, Cancelled

### 3.7 Special Session Management
- **Request Review**: Review remote test requests
- **Approval Workflow**: Approve/reject special sessions
- **Invigilator Verification**: Verify invigilator credentials
- **Remote Venue Setup**: Configure remote venues

### 3.8 Reporting & Analytics
- **Registration Reports**: Track registration trends
- **Payment Reports**: Financial summaries
- **Test Attendance**: Track attendance rates
- **Result Statistics**: Performance analytics
- **Venue Utilization**: Venue usage reports
- **Export Options**: Excel and PDF formats

---

## 4. Landing Page & Public Content

### 4.1 Landing Page Structure
**Main Menus**:
1. **Applicants**
   - Submenus matching current NBT website
   - Registration portal
   - Test information
   - How to prepare
   - FAQs

2. **Institutions**
   - Submenus matching current NBT website
   - Institutional registration
   - Bulk booking information
   - Result access for institutions
   - Reports for institutions

3. **Educators**
   - Submenus matching current NBT website
   - Resources for educators
   - Test specifications
   - Teaching materials
   - Professional development

### 4.2 Content Pages
- **About NBT**: Mission, vision, history
- **Test Information**: Detailed test descriptions
- **Preparation Resources**: Study guides, sample questions
- **FAQs**: Comprehensive question/answer section
- **Contact**: Contact forms, office locations, hours
- **News & Updates**: Latest announcements
- **Videos**: Embedded videos from current NBT website where available

### 4.3 Video Integration
- **Placement**: Videos on relevant pages
- **Sources**: Current NBT website video library
- **Topics**: Test preparation, how-to guides, testimonials
- **Accessibility**: Captions and transcripts

---

## 5. Data Model

### 5.1 Core Entities

#### Student
```csharp
public class Student
{
    public int Id { get; set; }
    public string NBTNumber { get; set; } // 14-digit Luhn validated
    public string SAIdNumber { get; set; } // Luhn validated (if SA citizen)
    public string ForeignId { get; set; } // For non-SA applicants
    public string PassportNumber { get; set; } // Alternative for non-SA
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; } // Extracted from SA ID or manually entered
    public string Ethnicity { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string SchoolName { get; set; }
    public string CurrentGrade { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public string CreatedBy { get; set; }
    public string ModifiedBy { get; set; }
}
```

#### Registration
```csharp
public class Registration
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public Student Student { get; set; }
    public string TestType { get; set; } // "AQL" or "AQL+MAT"
    public string PreferredLanguage { get; set; }
    public bool SpecialAccommodationRequired { get; set; }
    public string AccommodationDetails { get; set; }
    public int CurrentStep { get; set; } // For resume capability
    public bool IsCompleted { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? CompletedDate { get; set; }
}
```

#### Booking
```csharp
public class Booking
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public Student Student { get; set; }
    public int TestSessionId { get; set; }
    public TestSession TestSession { get; set; }
    public DateTime BookingDate { get; set; }
    public DateTime TestDate { get; set; }
    public DateTime ClosingDate { get; set; }
    public string BookingStatus { get; set; } // Pending, Confirmed, Cancelled
    public bool IsActive { get; set; }
    public decimal TestCost { get; set; } // Cost at time of booking (varies by intake year)
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
}
```

#### Payment
```csharp
public class Payment
{
    public int Id { get; set; }
    public int BookingId { get; set; }
    public Booking Booking { get; set; }
    public string EasyPayReference { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string PaymentStatus { get; set; } // Pending, Completed, Failed
    public string PaymentMethod { get; set; } // EasyPay, BankTransfer, etc.
    public bool IsInstallment { get; set; }
    public decimal TotalAmountPaid { get; set; }
    public decimal RemainingAmount { get; set; }
    public DateTime CreatedDate { get; set; }
    public string CreatedBy { get; set; }
}
```

#### TestResult
```csharp
public class TestResult
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public Student Student { get; set; }
    public int BookingId { get; set; }
    public Booking Booking { get; set; }
    public string Barcode { get; set; } // Unique barcode identifying the answer sheet
    public string TestType { get; set; } // "AQL" or "MAT"
    
    // AQL Results
    public int? ALScore { get; set; }
    public string ALPerformanceLevel { get; set; }
    public int? QLScore { get; set; }
    public string QLPerformanceLevel { get; set; }
    
    // MAT Results (if applicable)
    public int? MATScore { get; set; }
    public string MATPerformanceLevel { get; set; }
    
    public DateTime TestDate { get; set; }
    public DateTime ResultReleaseDate { get; set; }
    public bool IsVisible { get; set; } // Based on payment status
    public DateTime CreatedDate { get; set; }
    public string CreatedBy { get; set; }
}
```

#### Venue
```csharp
public class Venue
{
    public int Id { get; set; }
    public string VenueName { get; set; }
    public string VenueCode { get; set; }
    public string VenueType { get; set; } // National, Special Session, Research, Other
    public string Address { get; set; }
    public string City { get; set; }
    public string Province { get; set; }
    public string PostalCode { get; set; }
    public int TotalCapacity { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    
    // Navigation
    public ICollection<VenueDateAvailability> DateAvailabilities { get; set; }
    public ICollection<Room> Rooms { get; set; } // For information only
}
```

#### VenueDateAvailability
```csharp
public class VenueDateAvailability
{
    public int Id { get; set; }
    public int VenueId { get; set; }
    public Venue Venue { get; set; }
    public DateTime Date { get; set; }
    public bool IsAvailable { get; set; }
    public string Reason { get; set; } // If unavailable
}
```

#### Room
```csharp
public class Room
{
    public int Id { get; set; }
    public int VenueId { get; set; }
    public Venue Venue { get; set; }
    public string RoomNumber { get; set; }
    public string RoomName { get; set; }
    public int Capacity { get; set; }
    public bool IsActive { get; set; }
    // Note: Test sessions link to Venue, not Room
}
```

#### TestSession
```csharp
public class TestSession
{
    public int Id { get; set; }
    public int VenueId { get; set; } // Links to Venue, not Room
    public Venue Venue { get; set; }
    public DateTime TestDate { get; set; }
    public DateTime ClosingDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public string TestType { get; set; } // "AQL", "MAT", "Both"
    public int Capacity { get; set; }
    public int RegisteredCount { get; set; }
    public bool IsSundayTest { get; set; } // For highlighting
    public bool IsOnlineTest { get; set; } // For highlighting
    public string SessionStatus { get; set; } // Active, Full, Completed, Cancelled
    public DateTime CreatedDate { get; set; }
}
```

#### BackgroundQuestionnaire
```csharp
public class BackgroundQuestionnaire
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public Student Student { get; set; }
    public DateTime CompletedDate { get; set; }
    // Dynamic fields based on questionnaire version
    public string QuestionnaireData { get; set; } // JSON storage
}
```

#### SpecialSession
```csharp
public class SpecialSession
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public Student Student { get; set; }
    public string InvigilatorName { get; set; }
    public string InvigilatorContact { get; set; }
    public string InvigilatorEmail { get; set; }
    public string RemoteVenueName { get; set; }
    public string RemoteVenueAddress { get; set; }
    public DateTime RequestedDate { get; set; }
    public string Status { get; set; } // Pending, Approved, Rejected
    public string ReviewedBy { get; set; }
    public DateTime? ReviewedDate { get; set; }
    public string Comments { get; set; }
}
```

---

## 6. API Specifications

### 6.1 Authentication API
```
POST /api/v1/auth/register
POST /api/v1/auth/login
POST /api/v1/auth/refresh
POST /api/v1/auth/logout
POST /api/v1/auth/forgot-password
POST /api/v1/auth/reset-password
POST /api/v1/auth/verify-otp
```

### 6.2 Registration API
```
POST   /api/v1/registration/start
GET    /api/v1/registration/{id}
PUT    /api/v1/registration/{id}/step
POST   /api/v1/registration/{id}/complete
GET    /api/v1/registration/resume/{studentId}
POST   /api/v1/registration/validate-id
POST   /api/v1/registration/generate-nbt-number
```

### 6.3 Booking API
```
GET    /api/v1/booking/available-sessions
POST   /api/v1/booking/create
GET    /api/v1/booking/{id}
PUT    /api/v1/booking/{id}
DELETE /api/v1/booking/{id}
GET    /api/v1/booking/student/{studentId}
GET    /api/v1/booking/check-eligibility/{studentId}
```

### 6.4 Payment API
```
POST   /api/v1/payments/initiate
GET    /api/v1/payments/{id}
PUT    /api/v1/payments/{id}/confirm
GET    /api/v1/payments/booking/{bookingId}
POST   /api/v1/payments/bank-upload
GET    /api/v1/payments/student/{studentId}
GET    /api/v1/payments/status/{easyPayReference}
```

### 6.5 Results API
```
GET    /api/v1/results/student/{studentId}
GET    /api/v1/results/{id}
POST   /api/v1/results/import
GET    /api/v1/results/{id}/pdf
GET    /api/v1/results/barcode/{barcode}
PUT    /api/v1/results/{id}/visibility
```

### 6.6 Venue API
```
GET    /api/v1/venues
GET    /api/v1/venues/{id}
POST   /api/v1/venues
PUT    /api/v1/venues/{id}
DELETE /api/v1/venues/{id}
GET    /api/v1/venues/available/{date}
POST   /api/v1/venues/{id}/availability
```

### 6.7 Test Session API
```
GET    /api/v1/test-sessions
GET    /api/v1/test-sessions/{id}
POST   /api/v1/test-sessions
PUT    /api/v1/test-sessions/{id}
DELETE /api/v1/test-sessions/{id}
GET    /api/v1/test-sessions/calendar
GET    /api/v1/test-sessions/venue/{venueId}
```

### 6.8 Reports API
```
GET    /api/v1/reports/registrations
GET    /api/v1/reports/payments
GET    /api/v1/reports/results
GET    /api/v1/reports/venues
POST   /api/v1/reports/export/excel
POST   /api/v1/reports/export/pdf
```

### 6.9 Staff API
```
GET    /api/v1/staff/students
GET    /api/v1/staff/students/{id}
PUT    /api/v1/staff/students/{id}
GET    /api/v1/staff/bookings
GET    /api/v1/staff/payments
POST   /api/v1/staff/payments/manual-adjustment
GET    /api/v1/staff/special-sessions
PUT    /api/v1/staff/special-sessions/{id}/approve
PUT    /api/v1/staff/special-sessions/{id}/reject
```

---

## 7. User Interface Specifications

### 7.1 Blazor Components (Fluent UI)
All UI components MUST use Microsoft Fluent UI Blazor Components:
- `<FluentButton>`
- `<FluentTextField>`
- `<FluentSelect>`
- `<FluentDataGrid>`
- `<FluentDialog>`
- `<FluentCard>`
- `<FluentWizard>` or custom stepper for registration wizard
- `<FluentProgressRing>` for loading states
- `<FluentMessageBar>` for notifications

### 7.2 Registration Wizard Layout
```
┌─────────────────────────────────────────────────────┐
│  NBT Registration                    [ Step 1 of 4 ] │
├─────────────────────────────────────────────────────┤
│  ●─────●─────○─────○                                │
│  Account  Academic  Venue  Survey                   │
│                                                      │
│  [Wizard Step Content]                              │
│                                                      │
│  [ Previous ]              [ Save & Exit ] [ Next ] │
└─────────────────────────────────────────────────────┘
```

### 7.3 Student Dashboard Layout
```
┌─────┬─────────────────────────────────────────────┐
│ NBT │  Welcome, [Student Name]          [Profile] │
├─────┼─────────────────────────────────────────────┤
│     │  ┌───────────────┐  ┌───────────────┐      │
│ M   │  │ Active        │  │ Payment       │      │
│ E   │  │ Bookings: 1   │  │ Due: R150     │      │
│ N   │  └───────────────┘  └───────────────┘      │
│ U   │                                             │
│     │  Recent Activity                            │
│ >My │  • Booking confirmed for 2025-12-15        │
│ Pro │  • Payment received: R350                   │
│ fil │  • Result available: AQL Test              │
│ e   │                                             │
│     │  Quick Actions                              │
│ >Bo │  [Book New Test] [Make Payment] [View...]  │
│ ok  │                                             │
│ ing │                                             │
│ s   │                                             │
│     │                                             │
│ >Pa │                                             │
│ yme │                                             │
│ nts │                                             │
│     │                                             │
│ >Re │                                             │
│ sul │                                             │
│ ts  │                                             │
└─────┴─────────────────────────────────────────────┘
```

### 7.4 Staff Dashboard Layout
```
┌─────┬─────────────────────────────────────────────┐
│ NBT │  Staff Portal                  [Admin Name] │
├─────┼─────────────────────────────────────────────┤
│     │  ┌──────┐ ┌──────┐ ┌──────┐ ┌──────┐      │
│ M   │  │ 156  │ │ 23   │ │ R45k │ │ 12   │      │
│ E   │  │ Regis│ │ Pend │ │ Pay  │ │ Spec │      │
│ N   │  └──────┘ └──────┘ └──────┘ └──────┘      │
│ U   │                                             │
│     │  Recent Registrations                       │
│ >Da │  [Data Grid with Search/Filter]            │
│ shb │                                             │
│ oard│  Pending Actions                            │
│     │  • 5 special sessions awaiting approval     │
│ >St │  • 12 bank payments to process             │
│ ude │  • 3 result imports pending                │
│ nts │                                             │
│     │  Quick Actions                              │
│ >Bo │  [Process Payments] [Import Results]...    │
│ oki │                                             │
│ ngs │                                             │
│     │                                             │
│ >Pa │                                             │
│ yme │                                             │
│ nts │                                             │
│     │                                             │
│ >Re │                                             │
│ sul │                                             │
│ ts  │                                             │
│     │                                             │
│ >Ve │                                             │
│ nue │                                             │
│ s   │                                             │
│     │                                             │
│ >Re │                                             │
│ por │                                             │
│ ts  │                                             │
└─────┴─────────────────────────────────────────────┘
```

---

## 8. Workflows

### 8.1 Student Registration Flow
```
1. Landing Page
2. Click "Register" → Registration Wizard
3. Step 1: Account & Personal Info
   - Enter email, password
   - Enter SA ID (auto-extract DOB/Gender) OR Foreign ID/Passport
   - Enter name, ethnicity
   - Validate & Save
4. Step 2: Academic & Test Preferences
   - Enter school, grade
   - Select test type (AQL or AQL+MAT)
   - Select language
   - Validate & Save
5. Step 3: Venue & Booking
   - Select venue type
   - Choose test date
   - Select venue
   - Validate & Save
6. Step 4: Background Survey
   - Complete questionnaire
   - Submit
7. Generate NBT Number
8. Redirect to Dashboard
```

### 8.2 Test Booking Flow
```
1. Student Dashboard
2. Click "Book Test"
3. Check Eligibility:
   - No active booking?
   - Previous test closing date passed?
   - Less than 2 tests this year?
4. Select Test Type (AQL or AQL+MAT)
5. View Test Calendar
   - See available dates
   - Sunday tests highlighted
   - Online tests highlighted
6. Select Test Date
7. View Available Venues
8. Select Venue
9. Review Booking Details
10. Confirm Booking
11. Generate EasyPay Reference
12. Redirect to Payment
```

### 8.3 Payment Flow
```
1. Booking Confirmed
2. View Payment Details
   - Test cost (varies by intake year)
   - EasyPay reference
   - Payment instructions
3. Choose Payment Method:
   - EasyPay online
   - Bank transfer
   - Installment plan
4. Make Payment
5. Payment Confirmation
   - EasyPay webhook updates status
   - Or admin processes bank upload
6. Booking Status Updated
7. Email/SMS Confirmation Sent
```

### 8.4 Result Access Flow
```
1. Results Released (staff import)
2. Check Payment Status
   - Fully paid? → Visible to student
   - Not paid? → Not visible to student
3. Student Login
4. Navigate to Results
5. View Results:
   - AL score & performance level
   - QL score & performance level
   - MAT score & performance level (if applicable)
   - Barcode displayed
6. Download PDF Certificate (if fully paid)
```

### 8.5 Staff Result Import Flow
```
1. Staff Login
2. Navigate to Results Management
3. Click "Import Results"
4. Upload CSV/Excel File
5. System Validates:
   - Barcode unique
   - Student exists
   - Booking exists
6. Preview Import
7. Confirm Import
8. Results Saved
9. Visibility Set Based on Payment Status
10. Notifications Sent to Students with Paid Tests
```

---

## 9. Validation Rules

### 9.1 NBT Number Validation
- **Format**: 14 digits
- **Algorithm**: Luhn (modulus-10)
- **Uniqueness**: Globally unique
- **Generation**: Server-side only

### 9.2 SA ID Validation
- **Format**: 13 digits
- **Algorithm**: Luhn (modulus-10)
- **DOB Extraction**: Positions 1-6 (YYMMDD)
- **Gender Extraction**: Position 7 (0-4 female, 5-9 male)
- **Validation**: Full Luhn validation + DOB validity

### 9.3 Foreign ID Validation
- **Format**: Alphanumeric, 6-20 characters
- **Required**: If not SA citizen
- **Alternative**: Passport number accepted

### 9.4 Email Validation
- **Format**: RFC 5322 compliant
- **Uniqueness**: Per account
- **Verification**: OTP required

### 9.5 Phone Validation
- **Format**: E.164 international format
- **South African**: +27 followed by 9 digits
- **International**: Supported

### 9.6 Booking Validation
- **One Active**: Only one active booking per student
- **Eligibility**: Previous test closing date must have passed
- **Annual Limit**: Maximum 2 tests per year
- **Validity**: 3 years from booking date
- **Capacity**: Venue capacity not exceeded

### 9.7 Payment Validation
- **Amount**: Positive value
- **Order**: Oldest unpaid test first
- **Reference**: EasyPay reference unique
- **Status**: Valid status transitions only

---

## 10. Security Requirements

### 10.1 Authentication
- **JWT Tokens**: Access token (15 min) + Refresh token (7 days)
- **Password**: Minimum 8 characters, uppercase, lowercase, digit, special
- **OTP**: 6-digit numeric, 10-minute expiry
- **Session**: Secure cookie with HttpOnly and Secure flags

### 10.2 Authorization
- **Role-Based**: Admin, Staff, SuperUser, Student
- **Claims-Based**: Fine-grained permissions
- **Resource-Based**: User can only access their own data
- **Admin Override**: Admin/SuperUser can access all data

### 10.3 Data Protection
- **HTTPS**: All communication encrypted
- **Database**: Encrypted at rest
- **Passwords**: Bcrypt hashed with salt
- **PII**: Encrypted in database
- **Audit**: All access logged

### 10.4 Input Validation
- **Server-Side**: All inputs validated on server
- **Client-Side**: User experience validation
- **Sanitization**: Prevent SQL injection, XSS
- **File Upload**: Virus scanning, type validation

---

## 11. Performance Requirements

### 11.1 Load Times
- **Initial Load**: < 3 seconds
- **Navigation**: < 1 second
- **API Response**: < 500ms (95th percentile)
- **Database Query**: < 100ms (average)

### 11.2 Scalability
- **Concurrent Users**: 10,000+
- **Database**: Connection pooling
- **Caching**: Redis for session and reference data
- **CDN**: Static assets served from CDN

### 11.3 Optimization
- **Lazy Loading**: Images, components
- **Pagination**: Large datasets
- **Indexing**: Database indexes on frequently queried columns
- **Compression**: Gzip/Brotli for HTTP responses

---

## 12. Testing Requirements

### 12.1 Unit Tests
- **Coverage**: > 80%
- **Focus**: Business logic, validation, calculations
- **Framework**: xUnit

### 12.2 Integration Tests
- **Coverage**: All API endpoints
- **Focus**: Database interactions, external services
- **Framework**: xUnit with WebApplicationFactory

### 12.3 End-to-End Tests
- **Coverage**: Critical user workflows
- **Focus**: Registration, booking, payment, results
- **Framework**: Playwright or Selenium

### 12.4 Performance Tests
- **Load Testing**: JMeter or k6
- **Stress Testing**: Identify breaking points
- **Benchmarks**: Response time targets

---

## 13. Deployment & CI/CD

### 13.1 Branching Strategy
- **main**: Production-ready code
- **develop**: Integration branch
- **feature/***: Feature development
- **hotfix/***: Production fixes

### 13.2 CI/CD Pipeline
1. **Build**: Compile solution
2. **Test**: Run all unit and integration tests
3. **Scan**: Security vulnerability scanning
4. **Deploy to Staging**: Automatic on merge to main
5. **Smoke Test**: Automated smoke tests
6. **Deploy to Production**: Manual approval required

### 13.3 Environments
- **Development**: Local developer machines
- **Staging**: Pre-production testing
- **Production**: Live system

---

## 14. Monitoring & Logging

### 14.1 Application Monitoring
- **Application Insights**: Azure monitoring
- **Uptime**: 99.9% SLA
- **Alerts**: Automated alerts for errors, performance

### 14.2 Logging
- **Structured Logging**: Serilog
- **Log Levels**: Debug, Info, Warning, Error, Critical
- **Correlation IDs**: Track requests across services
- **Retention**: 90 days for application logs, 7 years for audit logs

### 14.3 Audit Trail
- **User Actions**: All CRUD operations logged
- **Changes**: Before/after values
- **Access**: Who accessed what data when
- **Compliance**: POPIA compliance

---

## 15. Documentation

### 15.1 Technical Documentation
- **Architecture Diagram**: System components and interactions
- **Database Schema**: ER diagram and table definitions
- **API Documentation**: Swagger/OpenAPI
- **Deployment Guide**: Infrastructure and deployment steps

### 15.2 User Documentation
- **Student Guide**: Step-by-step registration and booking
- **Staff Manual**: Dashboard usage and operations
- **Admin Guide**: System configuration and management
- **FAQs**: Common questions and answers

### 15.3 Video Tutorials
- **Registration**: How to register and create account
- **Booking**: How to book a test
- **Payment**: Payment process
- **Results**: Accessing and downloading results

---

## 16. Success Criteria

### 16.1 Functional
- ✅ Students can register and receive NBT number
- ✅ Students can book tests with eligibility checks
- ✅ Payments can be made via EasyPay and bank transfer
- ✅ Installment payments supported
- ✅ Results are imported and visible based on payment status
- ✅ Staff can manage all operations via dashboards
- ✅ Admin can configure system and access all data
- ✅ Audit trail captures all changes
- ✅ Reports can be generated and exported

### 16.2 Non-Functional
- ✅ Page load < 3 seconds
- ✅ API response < 500ms
- ✅ WCAG 2.1 AA compliance
- ✅ 99.9% uptime
- ✅ Support 10,000+ concurrent users
- ✅ Zero data breaches
- ✅ POPIA compliance

### 16.3 Business
- ✅ Reduce manual registration processing by 80%
- ✅ Reduce payment processing errors by 90%
- ✅ Improve student satisfaction scores
- ✅ Reduce administrative overhead
- ✅ Enable data-driven decision making

---

## Appendices

### Appendix A: Glossary
- **NBT**: National Benchmark Tests
- **AQL**: Academic and Quantitative Literacy
- **MAT**: Mathematics
- **AL**: Academic Literacy
- **QL**: Quantitative Literacy
- **SA ID**: South African Identity Document
- **Luhn Algorithm**: Checksum formula for validation
- **EasyPay**: Payment gateway provider
- **JWT**: JSON Web Token
- **OTP**: One-Time Password
- **POPIA**: Protection of Personal Information Act
- **WCAG**: Web Content Accessibility Guidelines

### Appendix B: References
- Current NBT Website: www.nbt.ac.za
- Fluent UI Blazor: https://www.fluentui-blazor.net/
- EasyPay API Documentation: [Provider docs]
- WCAG 2.1 Guidelines: https://www.w3.org/WAI/WCAG21/quickref/

---

**Document Status**: APPROVED  
**Next Review**: 2025-02-09  
**Owner**: NBT Development Team
