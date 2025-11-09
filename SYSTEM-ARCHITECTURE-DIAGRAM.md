# NBT System Architecture Diagram

## 📐 Complete System Overview

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                         NBT INTEGRATED WEB APPLICATION                       │
│                              (.NET 9.0 Blazor)                              │
└─────────────────────────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────────────────────────┐
│                            PRESENTATION LAYER                                │
│                        (Blazor Web App Interactive)                         │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐  │
│  │   Student    │  │    Staff     │  │    Admin     │  │  SuperUser   │  │
│  │   Portal     │  │  Dashboard   │  │  Dashboard   │  │  Dashboard   │  │
│  └──────────────┘  └──────────────┘  └──────────────┘  └──────────────┘  │
│         │                 │                 │                 │            │
│         ├─────────────────┴─────────────────┴─────────────────┤            │
│         │                                                       │            │
│  ┌──────▼────────────────────────────────────────────────────▼──────┐     │
│  │              FluentUI Components (NO MudBlazor)                   │     │
│  │  • FluentWizard  • FluentDataGrid  • FluentDialog                │     │
│  │  • FluentCard    • FluentSelect    • FluentButton                │     │
│  └───────────────────────────────────────────────────────────────────┘     │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
                                      │
                                      │ HTTPS/JWT
                                      ▼
┌─────────────────────────────────────────────────────────────────────────────┐
│                              API LAYER                                       │
│                    (ASP.NET Core Web API - .NET 9.0)                        │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐  │
│  │ Registration │  │   Booking    │  │   Payment    │  │   Results    │  │
│  │     API      │  │     API      │  │     API      │  │     API      │  │
│  └──────────────┘  └──────────────┘  └──────────────┘  └──────────────┘  │
│                                                                             │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐  │
│  │    Venue     │  │   Session    │  │   Reports    │  │     Auth     │  │
│  │     API      │  │     API      │  │     API      │  │     API      │  │
│  └──────────────┘  └──────────────┘  └──────────────┘  └──────────────┘  │
│                                                                             │
│  Middleware: JWT Auth | Exception Handler | CORS | Rate Limiting           │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
                                      │
                                      │ DI Container
                                      ▼
┌─────────────────────────────────────────────────────────────────────────────┐
│                           APPLICATION LAYER                                  │
│                         (Business Logic Services)                            │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  ┌────────────────────────────────────────────────────────────────┐        │
│  │                      Service Interfaces                         │        │
│  │  IStudentService | IRegistrationService | IBookingService       │        │
│  │  IPaymentService | IResultService | IVenueService               │        │
│  └────────────────────────────────────────────────────────────────┘        │
│                             │                                               │
│                             ▼                                               │
│  ┌────────────────────────────────────────────────────────────────┐        │
│  │                   Business Logic Services                       │        │
│  │                                                                 │        │
│  │  • NBT Number Generator (Luhn Algorithm)                        │        │
│  │  • SA ID Validator (Luhn + Date/Gender extraction)              │        │
│  │  • Booking Rules Validator (1 test, 2/year, 3-year validity)    │        │
│  │  • Payment Integration (EasyPay)                                │        │
│  │  • Result Import & Domain Mapper                                │        │
│  │  • Barcode Generator (Unique per test write)                    │        │
│  │  • Room Allocation Engine                                       │        │
│  │  • Report Generator (Excel/PDF)                                 │        │
│  └────────────────────────────────────────────────────────────────┘        │
│                             │                                               │
│                             ▼                                               │
│  ┌────────────────────────────────────────────────────────────────┐        │
│  │                     DTOs & ViewModels                           │        │
│  │  StudentDto | TestResultDto | TestResultDomainDto               │        │
│  │  PreTestQuestionnaireDto | BookingDto | PaymentDto              │        │
│  └────────────────────────────────────────────────────────────────┘        │
│                             │                                               │
│                             ▼                                               │
│  ┌────────────────────────────────────────────────────────────────┐        │
│  │                      AutoMapper                                 │        │
│  │  Entity ←→ DTO conversions                                      │        │
│  └────────────────────────────────────────────────────────────────┘        │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
                                      │
                                      │ Repository Pattern
                                      ▼
┌─────────────────────────────────────────────────────────────────────────────┐
│                          INFRASTRUCTURE LAYER                                │
│                    (Data Access & External Services)                        │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  ┌────────────────────────────────────────────────────────────────┐        │
│  │                    Repository Pattern                           │        │
│  │  IStudentRepository | IRegistrationRepository                   │        │
│  │  ITestResultRepository | IVenueRepository                       │        │
│  └────────────────────────────────────────────────────────────────┘        │
│                             │                                               │
│                             ▼                                               │
│  ┌────────────────────────────────────────────────────────────────┐        │
│  │              Entity Framework Core (ORM)                        │        │
│  │  • DbContext  • Migrations  • Change Tracking                   │        │
│  └────────────────────────────────────────────────────────────────┘        │
│                             │                                               │
│  ┌────────────────────────────────────────────────────────────────┐        │
│  │                 External Service Integrations                   │        │
│  │  • EasyPay Payment Gateway                                      │        │
│  │  • Email Service (SendGrid/SMTP)                                │        │
│  │  • SMS Service                                                  │        │
│  │  • Azure Key Vault (Secrets)                                    │        │
│  │  • Azure Blob Storage (Documents)                               │        │
│  └────────────────────────────────────────────────────────────────┘        │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
                                      │
                                      │ ADO.NET / SQL
                                      ▼
┌─────────────────────────────────────────────────────────────────────────────┐
│                              DOMAIN LAYER                                    │
│                          (Core Business Entities)                            │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  ┌──────────────┐         ┌──────────────┐         ┌──────────────┐       │
│  │   Student    │◄───────►│ Registration │◄───────►│   Payment    │       │
│  │              │  1:N    │              │  1:1    │              │       │
│  │ • NBTNumber  │         │ • Status     │         │ • EasyPayRef │       │
│  │ • IDType     │         │ • TestType   │         │ • Status     │       │
│  │ • DOB/Gender │         │              │         │              │       │
│  │ • Ethnicity  │         └──────────────┘         └──────────────┘       │
│  └──────┬───────┘                                                          │
│         │                                                                   │
│         │ 1:N                                                               │
│         ▼                                                                   │
│  ┌──────────────┐         ┌──────────────────┐                            │
│  │ TestResult   │◄───────►│TestResultDomain  │                            │
│  │              │  1:N    │                  │                            │
│  │ • Barcode    │         │ • DomainType     │ ◄─┐                        │
│  │   (UNIQUE)   │         │   (AL|QL|MAT)    │   │                        │
│  │ • TestType   │         │ • Score          │   │  AQL: 2 domains        │
│  │   (AQL|MAT)  │         │ • Performance    │   │  MAT: 3 domains        │
│  └──────────────┘         │   Level          │   │                        │
│                           └──────────────────┘ ◄─┘                        │
│                                                                             │
│  ┌──────────────┐         ┌──────────────┐         ┌──────────────┐       │
│  │    Venue     │◄───────►│ TestSession  │◄───────►│RoomAllocation│       │
│  │              │  1:N    │              │  1:N    │              │       │
│  │ • Name       │         │ • SessionDate│         │ • StudentId  │       │
│  │ • Capacity   │         │ • VenueId    │         │ • RoomId     │       │
│  └──────┬───────┘         │   (NOT Room!)│         └──────────────┘       │
│         │                 └──────────────┘                │                │
│         │ 1:N                                             │                │
│         ▼                                                 │                │
│  ┌──────────────┐                                         │                │
│  │     Room     │◄────────────────────────────────────────┘                │
│  │              │                                                           │
│  │ • RoomNumber │                                                           │
│  │ • Capacity   │                                                           │
│  └──────────────┘                                                           │
│                                                                             │
│  ┌──────────────┐         ┌──────────────────┐                            │
│  │   Student    │◄───────►│ PreTestQuestion- │                            │
│  │              │  1:N    │     naire        │                            │
│  │              │         │                  │                            │
│  │              │         │ • Responses(JSON)│                            │
│  │              │         │ • CompletedDate  │                            │
│  └──────────────┘         └──────────────────┘                            │
│                                                                             │
│  ┌────────────────────────────────────────────────────────────────┐        │
│  │                     Enums & Value Objects                       │        │
│  │  • IDType: SA_ID | FOREIGN_ID | PASSPORT                        │        │
│  │  • Gender: Male | Female | Other                                │        │
│  │  • TestType: AQL | MAT                                          │        │
│  │  • DomainType: AL | QL | MAT                                    │        │
│  │  • PerformanceLevel: BasicLower → ProficientUpper (6 levels)    │        │
│  └────────────────────────────────────────────────────────────────┘        │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
                                      │
                                      │
                                      ▼
┌─────────────────────────────────────────────────────────────────────────────┐
│                             DATA LAYER                                       │
│                      (MS SQL Server Database)                                │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  Tables:                                                                    │
│  ├─ Students (with NBTNumber UNIQUE, IDNumber UNIQUE)                       │
│  ├─ Registrations                                                           │
│  ├─ Payments                                                                │
│  ├─ TestResults (with Barcode UNIQUE)                                       │
│  ├─ TestResultDomains (NEW - AL/QL/MAT scores)                              │
│  ├─ PreTestQuestionnaires (NEW - survey responses)                          │
│  ├─ Venues                                                                  │
│  ├─ Rooms                                                                   │
│  ├─ TestSessions (VenueId FK, NOT RoomId)                                   │
│  ├─ RoomAllocations                                                         │
│  ├─ SpecialSessions                                                         │
│  ├─ AuditLogs (comprehensive activity tracking)                             │
│  └─ AspNetUsers/Roles (Identity framework)                                  │
│                                                                             │
│  Indexes:                                                                   │
│  ├─ NBTNumber (UNIQUE)                                                      │
│  ├─ IDNumber (UNIQUE per IDType)                                            │
│  ├─ Barcode (UNIQUE)                                                        │
│  └─ Foreign Keys (all relationships)                                        │
│                                                                             │
│  Constraints:                                                               │
│  ├─ NBTNumber: 14 digits, Luhn-valid                                        │
│  ├─ Barcode: System-generated, unique per test write                        │
│  ├─ TestResultDomain: AQL=2 domains, MAT=3 domains                          │
│  └─ Scores: 0-100 range validation                                          │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

---

## 🔄 Registration Wizard Flow (4 Steps)

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                        REGISTRATION WIZARD FLOW                              │
└─────────────────────────────────────────────────────────────────────────────┘

    ┌──────────────────────────────────────────────────┐
    │  STEP 1: Account & Personal Information          │
    ├──────────────────────────────────────────────────┤
    │  • Select ID Type (SA_ID | FOREIGN_ID | PASSPORT)│
    │  • Enter ID Number (with Luhn validation)        │
    │  • Duplicate check                               │
    │  • Personal details: Name, Email, Phone          │
    │                                                  │
    │  IF SA_ID:                                       │
    │    ✓ Auto-extract DOB from ID (pos 0-5)         │
    │    ✓ Auto-extract Gender from ID (pos 6)        │
    │  ELSE:                                           │
    │    ✓ Manual DOB entry                           │
    │    ✓ Manual Gender selection                    │
    │                                                  │
    │  • Ethnicity (manual entry)                      │
    │  • Nationality (if Foreign/Passport)             │
    │  • Country of Origin (if Foreign/Passport)       │
    └──────────────────┬───────────────────────────────┘
                       │
                       ▼ [Next] (when valid)
    ┌──────────────────────────────────────────────────┐
    │  STEP 2: Academic & Test Selection               │
    ├──────────────────────────────────────────────────┤
    │  • School Name                                   │
    │  • Grade                                         │
    │  • Academic Year                                 │
    │                                                  │
    │  • Test Type Selection:                          │
    │    ○ AQL (Academic + Quantitative Literacy)      │
    │    ○ MAT (AQL + Mathematics)                     │
    │                                                  │
    │  • Venue Selection (dropdown with capacity)      │
    │  • Session Selection (dates/times)               │
    │                                                  │
    │  • Special Accommodations:                       │
    │    □ Requires special support                    │
    │    [Text area for details if checked]            │
    └──────────────────┬───────────────────────────────┘
                       │
                       ▼ [Next] (when valid)
    ┌──────────────────────────────────────────────────┐
    │  STEP 3: Pre-Test Questionnaire (NEW)            │
    ├──────────────────────────────────────────────────┤
    │  Research & Equity Reporting Survey              │
    │                                                  │
    │  Questions:                                      │
    │  • Home Language                                 │
    │  • Socioeconomic Background                      │
    │  • School Type                                   │
    │  • Parent Education Level                        │
    │  • Technology Access                             │
    │  • Study Resources                               │
    │  • [Additional survey questions]                 │
    │                                                  │
    │  Responses stored as JSON                        │
    └──────────────────┬───────────────────────────────┘
                       │
                       ▼ [Next] (when complete)
    ┌──────────────────────────────────────────────────┐
    │  STEP 4: Review & Confirmation                   │
    ├──────────────────────────────────────────────────┤
    │  ┌────────────────────────────────────────────┐  │
    │  │  SUMMARY                                   │  │
    │  │  ────────────────────────────────────────  │  │
    │  │  Personal Information:                     │  │
    │  │    Name: John Doe                          │  │
    │  │    ID: 9001015009087 (SA_ID)               │  │
    │  │    DOB: 1990-01-01 (Age: 34)               │  │
    │  │    Gender: Male                            │  │
    │  │    Ethnicity: African                      │  │
    │  │                                            │  │
    │  │  Test Information:                         │  │
    │  │    Test Type: MAT (AL + QL + Math)         │  │
    │  │    Venue: Cape Town Main Campus            │  │
    │  │    Session: 2025-04-15 09:00               │  │
    │  └────────────────────────────────────────────┘  │
    │                                                  │
    │  □ I accept Terms and Conditions                 │
    │                                                  │
    │  [Submit Registration]                           │
    └──────────────────┬───────────────────────────────┘
                       │
                       ▼ [Submit]
    ┌──────────────────────────────────────────────────┐
    │  SERVER-SIDE PROCESSING                          │
    ├──────────────────────────────────────────────────┤
    │  1. Validate all data (server-side)              │
    │  2. Generate NBT Number (14-digit Luhn)          │
    │  3. Create Student record                        │
    │  4. Create Registration record                   │
    │  5. Save PreTestQuestionnaire                    │
    │  6. Send confirmation email/SMS                  │
    │  7. Log audit entry                              │
    └──────────────────┬───────────────────────────────┘
                       │
                       ▼
    ┌──────────────────────────────────────────────────┐
    │  SUCCESS!                                        │
    ├──────────────────────────────────────────────────┤
    │  Your NBT Number: 20250000000014                 │
    │                                                  │
    │  Please save this number for future reference.   │
    │                                                  │
    │  [Go to Login]                                   │
    └──────────────────────────────────────────────────┘
```

---

## 📊 Test Result Structure

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                          TEST RESULT STRUCTURE                               │
└─────────────────────────────────────────────────────────────────────────────┘

AQL TEST (Academic & Quantitative Literacy):
┌────────────────────────────────────────┐
│ TestResult                             │
│ ─────────────────────────────────────  │
│ Barcode: BC-2025-000001 (UNIQUE)       │
│ TestType: AQL                          │
│ TestDate: 2025-04-15                   │
│ StudentId: {guid}                      │
└────────────┬───────────────────────────┘
             │
             ├─► TestResultDomain #1
             │   ├─ DomainType: AL (Academic Literacy)
             │   ├─ Score: 65.5
             │   ├─ Percentile: 72
             │   └─ PerformanceLevel: IntermediateLower
             │
             └─► TestResultDomain #2
                 ├─ DomainType: QL (Quantitative Literacy)
                 ├─ Score: 58.0
                 ├─ Percentile: 65
                 └─ PerformanceLevel: BasicUpper

MAT TEST (includes AQL + Mathematics):
┌────────────────────────────────────────┐
│ TestResult                             │
│ ─────────────────────────────────────  │
│ Barcode: BC-2025-000002 (UNIQUE)       │
│ TestType: MAT                          │
│ TestDate: 2025-06-20                   │
│ StudentId: {guid}                      │
└────────────┬───────────────────────────┘
             │
             ├─► TestResultDomain #1
             │   ├─ DomainType: AL (Academic Literacy)
             │   ├─ Score: 72.0
             │   ├─ Percentile: 78
             │   └─ PerformanceLevel: IntermediateUpper
             │
             ├─► TestResultDomain #2
             │   ├─ DomainType: QL (Quantitative Literacy)
             │   ├─ Score: 68.5
             │   ├─ Percentile: 71
             │   └─ PerformanceLevel: IntermediateLower
             │
             └─► TestResultDomain #3
                 ├─ DomainType: MAT (Mathematics)
                 ├─ Score: 81.0
                 ├─ Percentile: 85
                 └─ PerformanceLevel: ProficientLower

BARCODE UNIQUENESS:
┌────────────────────────────────────────────────────────────────┐
│ If a student writes 2 tests:                                   │
│                                                                │
│ Test Write #1: Barcode BC-2025-000001                          │
│   └─ Identifies Answer Sheet #1                               │
│                                                                │
│ Test Write #2: Barcode BC-2025-000002                          │
│   └─ Identifies Answer Sheet #2 (DIFFERENT from #1)           │
│                                                                │
│ Purpose: Distinguish multiple test attempts by same student    │
└────────────────────────────────────────────────────────────────┘
```

---

## 🔐 Security & Access Control

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                       ROLE-BASED ACCESS CONTROL                              │
└─────────────────────────────────────────────────────────────────────────────┘

┌──────────────┐
│   STUDENT    │ (Authenticated User)
└──────┬───────┘
       │
       ├─► View: Own registration, bookings, results
       ├─► Create: New registration, booking, questionnaire
       ├─► Update: Own profile, change booking (before close date)
       └─► Cannot: Access admin areas, view other students

┌──────────────┐
│    STAFF     │ (Read-Only Role)
└──────┬───────┘
       │
       ├─► View: All students, bookings, payments, results
       ├─► Generate: Reports and analytics
       └─► Cannot: Modify data, access system settings

┌──────────────┐
│    ADMIN     │ (Full CRUD Role)
└──────┬───────┘
       │
       ├─► View: All data
       ├─► Create: Students, bookings, sessions, venues
       ├─► Update: Registrations, payments, results
       ├─► Delete: Most entities (with audit)
       └─► Cannot: System configuration, user roles

┌──────────────┐
│  SUPERUSER   │ (System Administrator)
└──────┬───────┘
       │
       ├─► All Admin privileges
       ├─► Import: Test results (bulk)
       ├─► Manage: Users, roles, permissions
       ├─► Configure: System settings, integrations
       ├─► Access: Full audit logs
       └─► Manage: Venues, rooms, sessions

JWT TOKEN STRUCTURE:
{
  "sub": "{user-id}",
  "email": "user@example.com",
  "role": "Admin",
  "nbr": true,
  "exp": 1704067200
}
```

---

## 🎓 Student Booking Rules Engine

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                       BOOKING VALIDATION ENGINE                              │
└─────────────────────────────────────────────────────────────────────────────┘

RULE #1: Intake Start Date
┌────────────────────────────────────────┐
│ IF Current Date >= April 1             │
│ THEN Allow booking                     │
│ ELSE Block with message:               │
│   "Bookings open on April 1"           │
└────────────────────────────────────────┘

RULE #2: One Active Booking
┌────────────────────────────────────────┐
│ Query: SELECT COUNT(*) FROM Bookings  │
│        WHERE StudentId = ?             │
│        AND Status = 'Active'           │
│                                        │
│ IF Count > 0                           │
│ THEN Block with message:               │
│   "You have an active booking"         │
│ ELSE Allow booking                     │
└────────────────────────────────────────┘

RULE #3: Annual Limit (2 tests/year)
┌────────────────────────────────────────┐
│ Query: SELECT COUNT(*) FROM Bookings  │
│        WHERE StudentId = ?             │
│        AND YEAR(BookingDate) = ?       │
│                                        │
│ IF Count >= 2                          │
│ THEN Block with message:               │
│   "Maximum 2 tests per year reached"   │
│ ELSE Allow booking                     │
└────────────────────────────────────────┘

RULE #4: Rebooking After Close Date
┌────────────────────────────────────────┐
│ IF Active booking exists               │
│ AND Current Date > Booking Close Date  │
│ THEN Allow new booking                 │
│ ELSE Block                             │
└────────────────────────────────────────┘

RULE #5: Booking Modification Before Close
┌────────────────────────────────────────┐
│ IF Current Date < Booking Close Date   │
│ THEN Allow modification                │
│ ELSE Block with message:               │
│   "Booking closed, cannot modify"      │
└────────────────────────────────────────┘

RULE #6: Test Validity (3 years)
┌────────────────────────────────────────┐
│ Validity Period:                       │
│   Start: Booking Date                  │
│   End: Booking Date + 3 years          │
│                                        │
│ Valid until: 2028-04-15 (if booked     │
│              2025-04-15)               │
└────────────────────────────────────────┘
```

---

**Last Updated**: 2025-11-09  
**Version**: 1.0  
**Status**: ✅ Complete Architecture Documentation
