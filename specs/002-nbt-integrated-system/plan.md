# Implementation Plan - NBT Integrated System

**Feature**: 002-nbt-integrated-system  
**Version**: 1.0  
**Created**: 2025-11-08  
**Status**: APPROVED FOR IMPLEMENTATION

---

## EXECUTIVE SUMMARY

This implementation plan details the **complete development roadmap** for extending the existing NBT website shell into a fully functional integrated system. The plan covers 12 weeks of development across 10 phases, adding 8 core entities, 61 API endpoints, and comprehensive admin/staff functionality.

**Estimated Effort**: 480 developer-hours (12 weeks × 40 hours/week)  
**Team Size**: 2-3 developers  
**Timeline**: 12 weeks from start to production deployment

---

## 1. CURRENT STATE ANALYSIS

### 1.1 Existing Implementation (75% Complete)

✅ **Completed Components**:
- Clean Architecture structure (5 projects)
- ASP.NET Core Identity with JWT authentication
- Entity Framework Core with SQL Server
- Fluent UI Blazor components integrated
- 6 existing entities (User, Announcement, ContentPage, ContactInquiry, DownloadableResource, SystemSetting)
- 6 API controllers (basic CRUD)
- 7 public-facing pages (Landing, About, Applicants, Educators, Institutions, News, Contact)
- Database operational with seed data

✅ **Infrastructure Ready**:
- GitHub repository established
- Project documentation (README, DATABASE docs)
- Initial EF Core migration applied
- Development environment configured

### 1.2 Missing Core Functionality (25% Remaining)

❌ **Critical Gaps**:
1. Student management module
2. Registration wizard (multi-step)
3. NBT number generation (Luhn algorithm)
4. Payment integration (EasyPay)
5. Test session management
6. Venue and room management
7. Test result imports (Excel)
8. Staff/Admin dashboards
9. Reporting and analytics
10. Audit logging system

---

## 2. ARCHITECTURE OVERVIEW

### 2.1 Solution Structure

```
NBTWebApp/
├── src/
│   ├── NBT.Domain/              # Domain entities, value objects, enums
│   │   ├── Entities/            # Add 9 new entities
│   │   ├── ValueObjects/        # Add NBTNumber, SAIDNumber
│   │   └── Enums/               # Add 5 new enums
│   ├── NBT.Application/         # Use cases, DTOs, services
│   │   ├── Students/            # NEW MODULE
│   │   ├── Registrations/       # NEW MODULE
│   │   ├── Payments/            # NEW MODULE
│   │   ├── TestSessions/        # NEW MODULE
│   │   ├── Venues/              # NEW MODULE
│   │   ├── TestResults/         # NEW MODULE
│   │   └── Reports/             # NEW MODULE
│   ├── NBT.Infrastructure/      # Data access, external services
│   │   ├── Persistence/
│   │   │   └── Configurations/  # Add 9 EF configurations
│   │   ├── Services/
│   │   │   ├── EasyPayService   # NEW
│   │   │   ├── ExcelService     # NEW
│   │   │   └── AuditService     # NEW
│   ├── NBT.WebAPI/              # REST API controllers
│   │   └── Controllers/         # Add 9 new controllers
│   └── NBT.WebUI/               # Blazor Web App
│       ├── Pages/
│       │   ├── Registration/    # NEW - Multi-step wizard
│       │   ├── Admin/           # NEW - Dashboard & CRUD
│       │   └── Staff/           # NEW - Read-only views
│       └── Components/
│           ├── Wizards/         # NEW - Registration wizard
│           ├── DataGrids/       # NEW - CRUD grids
│           └── Charts/          # NEW - Analytics
├── tests/
│   ├── NBT.Domain.Tests/        # NEW - Unit tests
│   ├── NBT.Application.Tests/   # NEW - Service tests
│   └── NBT.IntegrationTests/    # NEW - API tests
└── docs/
    └── api/                     # Swagger documentation
```

### 2.2 Technology Stack Confirmation

| Layer | Technology | Version | Status |
|-------|-----------|---------|--------|
| Frontend | Blazor Web App (Interactive Auto) | .NET 9 | ✅ Configured |
| UI Framework | Fluent UI Blazor | 4.9.0 | ✅ Integrated |
| Backend API | ASP.NET Core Web API | .NET 9 | ✅ Running |
| Database | MS SQL Server | 2019+ | ✅ Operational |
| ORM | Entity Framework Core | 9.0 | ✅ Configured |
| Authentication | ASP.NET Core Identity + JWT | .NET 9 | ✅ Implemented |
| Testing | xUnit + bUnit | Latest | ❌ To Add |
| Reporting | ClosedXML + QuestPDF | Latest | ❌ To Add |
| Payment Gateway | EasyPay REST API | v1 | ❌ To Integrate |

---

## 3. IMPLEMENTATION PHASES

### Phase 1: Foundation & Domain Setup (Week 1)
**Duration**: 5 days  
**Effort**: 40 hours  
**Priority**: CRITICAL

#### Objectives
- Add all missing domain entities
- Implement value objects (NBTNumber, SAIDNumber)
- Create new enums
- Set up EF Core configurations
- Generate and apply database migration

#### Tasks

**Day 1-2: Domain Entities**
- [ ] Create `Student` entity with full validation
- [ ] Create `Registration` entity
- [ ] Create `Payment` entity
- [ ] Create `TestSession` entity
- [ ] Create `Venue` entity
- [ ] Create `Room` entity
- [ ] Create `RoomAllocation` entity
- [ ] Create `TestResult` entity
- [ ] Create `AuditLog` entity

**Day 2-3: Value Objects & Enums**
- [ ] Implement `NBTNumber` value object with Luhn algorithm
- [ ] Implement `SAIDNumber` value object with SA ID validation
- [ ] Create `RegistrationStatus` enum
- [ ] Create `PaymentStatus` enum
- [ ] Create `SessionStatus` enum
- [ ] Create `TestType` enum
- [ ] Create `PerformanceBand` enum
- [ ] Add unit tests for NBTNumber.Generate()
- [ ] Add unit tests for SAIDNumber.IsValid()

**Day 3-4: EF Core Configurations**
- [ ] Create `StudentConfiguration.cs`
- [ ] Create `RegistrationConfiguration.cs`
- [ ] Create `PaymentConfiguration.cs`
- [ ] Create `TestSessionConfiguration.cs`
- [ ] Create `VenueConfiguration.cs`
- [ ] Create `RoomConfiguration.cs`
- [ ] Create `RoomAllocationConfiguration.cs`
- [ ] Create `TestResultConfiguration.cs`
- [ ] Create `AuditLogConfiguration.cs`
- [ ] Update `ApplicationDbContext` with new DbSets

**Day 4-5: Database Migration**
- [ ] Generate migration: `AddCoreEntities`
- [ ] Review migration SQL script
- [ ] Apply migration to development database
- [ ] Create seed data for Venues (5 major cities)
- [ ] Create seed data for Rooms (20 rooms)
- [ ] Create seed data for TestSessions (10 upcoming sessions)
- [ ] Verify database schema and relationships
- [ ] Update database documentation

**Deliverables**:
- ✅ 9 new entities in Domain layer
- ✅ 2 value objects with business logic
- ✅ 5 new enums
- ✅ 9 EF Core configurations
- ✅ Database migration applied
- ✅ Seed data created
- ✅ 15+ unit tests passing

---

### Phase 2: Student Management Module (Week 2)
**Duration**: 5 days  
**Effort**: 40 hours  
**Priority**: CRITICAL

#### Objectives
- Implement student CRUD operations
- Create NBT number generation service
- Build SA ID validation service
- Develop Students API endpoints
- Create admin UI for student management

#### Tasks

**Day 6-7: Application Layer**
- [ ] Create `StudentDto.cs`
- [ ] Create `CreateStudentRequest.cs` with validation
- [ ] Create `UpdateStudentRequest.cs`
- [ ] Create `IStudentService` interface
- [ ] Implement `StudentService` with CRUD operations
- [ ] Implement `NBTNumberGenerator` service
- [ ] Add FluentValidation for student requests
- [ ] Add AutoMapper profiles for Student↔StudentDto
- [ ] Write unit tests for StudentService (15+ tests)

**Day 7-8: API Layer**
- [ ] Create `StudentsController.cs`
- [ ] Implement GET `/api/students` (paginated list)
- [ ] Implement GET `/api/students/{id}`
- [ ] Implement GET `/api/students/nbt/{nbtNumber}`
- [ ] Implement POST `/api/students` (create with NBT generation)
- [ ] Implement PUT `/api/students/{id}`
- [ ] Implement DELETE `/api/students/{id}`
- [ ] Implement POST `/api/students/generate-nbt-number`
- [ ] Implement POST `/api/students/validate-id` (public)
- [ ] Add [Authorize] attributes (Admin/Staff roles)
- [ ] Add Swagger documentation
- [ ] Write integration tests for all endpoints

**Day 8-9: Admin UI**
- [ ] Create `Admin/Students/Index.razor` (student list page)
- [ ] Create `FluentDataGrid` with sorting/filtering
- [ ] Create `Admin/Students/Create.razor` (add student form)
- [ ] Create `Admin/Students/Edit.razor` (edit student form)
- [ ] Implement ID number validation on client
- [ ] Show extracted DOB and Gender from ID
- [ ] Add search functionality (name, NBT, ID)
- [ ] Add export to Excel button
- [ ] Create `StudentApiService` for HTTP calls
- [ ] Add loading states and error handling

**Day 9-10: Testing & Documentation**
- [ ] Manual testing of all CRUD operations
- [ ] Test NBT number generation with various sequences
- [ ] Test SA ID validation with edge cases
- [ ] Verify audit logging for all operations
- [ ] Update API documentation
- [ ] Create user guide for student management
- [ ] Code review and refactoring

**Deliverables**:
- ✅ Student service with CRUD operations
- ✅ NBT number generator with Luhn checksum
- ✅ 9 API endpoints (100% functional)
- ✅ Admin UI with search and filtering
- ✅ 30+ tests (unit + integration)
- ✅ API documentation updated

---

### Phase 3: Registration Wizard & Booking (Week 3-4)
**Duration**: 10 days  
**Effort**: 80 hours  
**Priority**: CRITICAL

#### Objectives
- Build multi-step registration wizard
- Implement test session selection
- Create booking confirmation flow
- Develop Registration and Booking APIs

#### Tasks

**Day 11-13: Registration Services**
- [ ] Create `RegistrationDto.cs`
- [ ] Create `RegistrationWizardRequest.cs` (multi-step)
- [ ] Create `CreateRegistrationRequest.cs`
- [ ] Create `IRegistrationService` interface
- [ ] Implement `RegistrationService`
- [ ] Implement registration number generation (REG-YYYY-NNNNNN)
- [ ] Implement duplicate registration check
- [ ] Implement session capacity validation
- [ ] Create `ITestSessionService` interface
- [ ] Implement `TestSessionService`
- [ ] Write 20+ unit tests

**Day 13-15: API Layer**
- [ ] Create `RegistrationsController.cs`
- [ ] Implement GET `/api/registration` (list)
- [ ] Implement GET `/api/registration/{id}`
- [ ] Implement POST `/api/registration/wizard` (complete flow)
- [ ] Implement POST `/api/registration` (admin only)
- [ ] Implement PUT `/api/registration/{id}/status`
- [ ] Implement DELETE `/api/registration/{id}` (cancel)
- [ ] Create `BookingController.cs`
- [ ] Implement GET `/api/booking/sessions/available`
- [ ] Implement POST `/api/booking/reserve`
- [ ] Implement POST `/api/booking/confirm`
- [ ] Add authorization rules
- [ ] Write integration tests

**Day 15-18: Registration Wizard UI**
- [ ] Create `Registration/Wizard.razor` (multi-step component)
- [ ] Create `Step1_StudentInfo.razor` component
  - ID number input with live validation
  - Extract and display DOB, Gender
  - First name, Last name, Email, Phone
  - School and Grade selection
- [ ] Create `Step2_TestSelection.razor` component
  - Test type checkboxes (AL, QL, Math)
  - Remote writer option
  - Special accommodation textarea
  - Fee calculation display
- [ ] Create `Step3_SessionSelection.razor` component
  - Available sessions grid (date, venue, capacity)
  - Filter by city/province
  - Show available seats per session
  - Disable full sessions
- [ ] Create `Step4_Confirmation.razor` component
  - Summary of all selections
  - Terms and conditions checkbox
  - Submit button
- [ ] Implement wizard navigation (Next, Back, Submit)
- [ ] Add validation for each step
- [ ] Show progress indicator (1/4, 2/4, etc.)
- [ ] Implement error handling
- [ ] Add success page with registration details

**Day 18-20: Testing & Polish**
- [ ] Test complete registration flow end-to-end
- [ ] Test session capacity enforcement
- [ ] Test duplicate registration prevention
- [ ] Test wizard state persistence (browser refresh)
- [ ] Mobile responsiveness testing
- [ ] Accessibility testing (keyboard navigation)
- [ ] Performance testing (wizard load time)
- [ ] User acceptance testing
- [ ] Bug fixes and refinements

**Deliverables**:
- ✅ Multi-step registration wizard (4 steps)
- ✅ Registration service with business rules
- ✅ 11 API endpoints (Registration + Booking)
- ✅ Mobile-responsive wizard UI
- ✅ Session capacity management
- ✅ 40+ tests passing
- ✅ User guide for registration process

---

### Phase 4: Payment Integration (EasyPay) (Week 5)
**Duration**: 5 days  
**Effort**: 40 hours  
**Priority**: CRITICAL

#### Objectives
- Integrate EasyPay payment gateway
- Implement payment initiation and callback handling
- Create payment status tracking
- Build invoice generation

#### Tasks

**Day 21-22: Payment Services**
- [ ] Create `PaymentDto.cs`
- [ ] Create `InitiatePaymentRequest.cs`
- [ ] Create `InitiatePaymentResponse.cs`
- [ ] Create `EasyPayCallbackRequest.cs`
- [ ] Create `IPaymentService` interface
- [ ] Implement `PaymentService`
- [ ] Create `IEasyPayService` interface
- [ ] Implement `EasyPayService`
  - InitiatePayment() - call EasyPay API
  - VerifySignature() - HMAC-SHA256 validation
  - ProcessCallback() - update payment status
- [ ] Implement invoice number generation (INV-YYYY-NNNNNN)
- [ ] Create `EasyPaySettings` configuration class
- [ ] Add EasyPay settings to appsettings.json
- [ ] Write 15+ unit tests

**Day 22-23: API Layer**
- [ ] Create `PaymentsController.cs`
- [ ] Implement GET `/api/payments` (list)
- [ ] Implement GET `/api/payments/{id}`
- [ ] Implement POST `/api/payments/initiate`
- [ ] Implement POST `/api/payments/easypay/callback` (webhook)
- [ ] Implement PUT `/api/payments/{id}/status`
- [ ] Implement POST `/api/payments/{id}/refund`
- [ ] Add webhook authentication (signature verification)
- [ ] Add retry logic for failed EasyPay calls
- [ ] Write integration tests

**Day 23-24: Payment UI**
- [ ] Update registration wizard Step 4 to initiate payment
- [ ] Create `Registration/Payment.razor` page
  - Display invoice details
  - Show EasyPay payment button
  - Redirect to EasyPay gateway
- [ ] Create `Registration/PaymentCallback.razor` page
  - Handle return from EasyPay
  - Display payment status
  - Show registration confirmation
  - Send confirmation email
- [ ] Create `Admin/Payments/Index.razor` (payment list)
- [ ] Add payment status filtering
- [ ] Add refund functionality for admins
- [ ] Create payment receipt PDF generation
- [ ] Add payment status email notifications

**Day 24-25: Testing & Security**
- [ ] Test payment initiation flow
- [ ] Test EasyPay callback with test transactions
- [ ] Test signature verification (tamper detection)
- [ ] Test failed payment handling
- [ ] Test payment timeout scenarios
- [ ] Security audit of payment endpoints
- [ ] PCI DSS compliance review
- [ ] Load testing (100 concurrent payments)
- [ ] Documentation of payment flow

**Deliverables**:
- ✅ EasyPay integration (production-ready)
- ✅ Payment service with callback handling
- ✅ 7 API endpoints (Payments)
- ✅ Payment UI with status tracking
- ✅ Invoice PDF generation
- ✅ HMAC signature verification
- ✅ 25+ tests (including security tests)
- ✅ Payment integration documentation

---

### Phase 5: Venue & Room Management (Week 6)
**Duration**: 5 days  
**Effort**: 40 hours  
**Priority**: HIGH

#### Objectives
- Implement venue CRUD operations
- Create room management with capacity tracking
- Build venue and room admin UI
- Implement room allocation to sessions

#### Tasks

**Day 26-27: Venue Services**
- [ ] Create `VenueDto.cs`
- [ ] Create `CreateVenueRequest.cs`
- [ ] Create `RoomDto.cs`
- [ ] Create `CreateRoomRequest.cs`
- [ ] Create `RoomAllocationDto.cs`
- [ ] Create `IVenueService` interface
- [ ] Implement `VenueService`
- [ ] Implement venue capacity calculation (sum of rooms)
- [ ] Implement room availability checking
- [ ] Write 15+ unit tests

**Day 27-28: API Layer**
- [ ] Create `VenuesController.cs`
- [ ] Implement GET `/api/venues` (list)
- [ ] Implement GET `/api/venues/{id}`
- [ ] Implement POST `/api/venues`
- [ ] Implement PUT `/api/venues/{id}`
- [ ] Implement DELETE `/api/venues/{id}`
- [ ] Implement GET `/api/venues/{id}/rooms`
- [ ] Implement POST `/api/venues/{id}/rooms`
- [ ] Implement PUT `/api/venues/rooms/{roomId}`
- [ ] Implement DELETE `/api/venues/rooms/{roomId}`
- [ ] Write integration tests

**Day 28-30: Admin UI**
- [ ] Create `Admin/Venues/Index.razor` (venue list)
- [ ] Create `Admin/Venues/Create.razor` (add venue form)
- [ ] Create `Admin/Venues/Edit.razor` (edit venue form)
- [ ] Create `Admin/Venues/Rooms.razor` (room management)
- [ ] Implement room grid with add/edit/delete
- [ ] Show total venue capacity (calculated)
- [ ] Add venue status toggle (Active/Inactive)
- [ ] Implement venue search and filtering
- [ ] Add room allocation view for sessions
- [ ] Create room allocation dialog

**Day 30: Testing**
- [ ] Test venue CRUD operations
- [ ] Test room capacity calculations
- [ ] Test room allocation to sessions
- [ ] Test venue status changes
- [ ] Mobile responsiveness testing
- [ ] Accessibility testing

**Deliverables**:
- ✅ Venue management service
- ✅ 10 API endpoints (Venues + Rooms)
- ✅ Admin UI for venue/room management
- ✅ Room allocation functionality
- ✅ Capacity tracking
- ✅ 20+ tests passing

---

### Phase 6: Test Sessions Management (Week 7)
**Duration**: 5 days  
**Effort**: 40 hours  
**Priority**: HIGH

#### Objectives
- Implement test session CRUD
- Create session scheduling with room allocation
- Build session capacity management
- Develop session admin UI

#### Tasks

**Day 31-32: Session Services**
- [ ] Create `TestSessionDto.cs`
- [ ] Create `CreateTestSessionRequest.cs`
- [ ] Update `TestSessionService` with full CRUD
- [ ] Implement session code generation (CITY-YYYY-MM-DD-PERIOD)
- [ ] Implement capacity management
- [ ] Implement session status transitions
- [ ] Implement room allocation logic
- [ ] Write 15+ unit tests

**Day 32-33: API Layer**
- [ ] Create `SessionsController.cs`
- [ ] Implement GET `/api/sessions` (list)
- [ ] Implement GET `/api/sessions/{id}`
- [ ] Implement GET `/api/sessions/upcoming` (public)
- [ ] Implement POST `/api/sessions`
- [ ] Implement PUT `/api/sessions/{id}`
- [ ] Implement DELETE `/api/sessions/{id}`
- [ ] Implement POST `/api/sessions/{id}/allocate-rooms`
- [ ] Write integration tests

**Day 33-35: Admin UI**
- [ ] Create `Admin/Sessions/Index.razor` (session list)
- [ ] Create `Admin/Sessions/Create.razor` (add session form)
  - Date and time pickers
  - Venue selection dropdown
  - Capacity input
  - Special session checkbox
- [ ] Create `Admin/Sessions/Edit.razor` (edit session)
- [ ] Create `Admin/Sessions/Details.razor` (session details)
  - Show registrations for session
  - Show room allocations
  - Display capacity vs. current registrations
- [ ] Create session calendar view
- [ ] Implement session filtering (date range, venue, status)
- [ ] Add session status management (Open/Closed/Cancelled)
- [ ] Create room allocation interface

**Day 35: Testing**
- [ ] Test session creation and scheduling
- [ ] Test capacity enforcement
- [ ] Test room allocation
- [ ] Test session status transitions
- [ ] Test calendar view
- [ ] Accessibility testing

**Deliverables**:
- ✅ Test session management service
- ✅ 8 API endpoints (Sessions)
- ✅ Admin UI with calendar view
- ✅ Room allocation system
- ✅ Session status management
- ✅ 20+ tests passing

---

### Phase 7: Test Results Import & Management (Week 8)
**Duration**: 5 days  
**Effort**: 40 hours  
**Priority**: HIGH

#### Objectives
- Implement Excel import for test results
- Create result validation and error reporting
- Build result release mechanism
- Develop results admin UI

#### Tasks

**Day 36-37: Result Services**
- [ ] Create `TestResultDto.cs`
- [ ] Create `ImportResultsRequest.cs`
- [ ] Create `ImportResultsResponse.cs`
- [ ] Create `ITestResultService` interface
- [ ] Implement `TestResultService`
- [ ] Create `IExcelService` interface
- [ ] Implement `ExcelService` using ClosedXML
  - ReadExcelFile() - parse .xlsx
  - ValidateHeaders() - check column names
  - ValidateRows() - business validation
  - GenerateErrorReport() - detailed errors
- [ ] Implement duplicate detection
- [ ] Implement result release logic
- [ ] Write 15+ unit tests

**Day 37-38: API Layer**
- [ ] Create `ResultsController.cs`
- [ ] Implement GET `/api/results` (list)
- [ ] Implement GET `/api/results/student/{studentId}`
- [ ] Implement GET `/api/results/session/{sessionId}`
- [ ] Implement POST `/api/results/import` (Excel upload)
- [ ] Implement PUT `/api/results/{id}/release`
- [ ] Implement POST `/api/results/bulk-release`
- [ ] Add file upload validation (size, type)
- [ ] Write integration tests

**Day 38-40: Admin UI**
- [ ] Create `Admin/Results/Index.razor` (results list)
- [ ] Create `Admin/Results/Import.razor` (Excel upload)
  - File upload component
  - Drag-and-drop support
  - Progress indicator
  - Error report display
- [ ] Create `Admin/Results/Details.razor` (result details)
- [ ] Implement result filtering (session, student, test type)
- [ ] Add bulk release functionality (checkbox selection)
- [ ] Create result release confirmation dialog
- [ ] Add Excel template download
- [ ] Show import history

**Day 40: Testing**
- [ ] Test Excel import with valid data
- [ ] Test import with validation errors
- [ ] Test duplicate detection
- [ ] Test bulk release functionality
- [ ] Test error reporting
- [ ] Create sample Excel files for testing

**Deliverables**:
- ✅ Excel import service
- ✅ 6 API endpoints (Results)
- ✅ Admin UI with import functionality
- ✅ Result validation engine
- ✅ Bulk release functionality
- ✅ Excel error reporting
- ✅ 20+ tests passing
- ✅ Excel import template

---

### Phase 8: Staff & Admin Dashboards (Week 9)
**Duration**: 5 days  
**Effort**: 40 hours  
**Priority**: MEDIUM

#### Objectives
- Create role-based dashboards (Staff, Admin, SuperUser)
- Build data grids for all entities
- Implement comprehensive CRUD operations
- Add user management for staff

#### Tasks

**Day 41-42: Dashboard Services**
- [ ] Create `DashboardDto.cs`
- [ ] Create `IDashboardService` interface
- [ ] Implement `DashboardService`
  - GetSummaryStats() - counts and totals
  - GetRecentActivity() - last 10 actions
  - GetUpcomingSessions() - next 5 sessions
  - GetPendingPayments() - unpaid registrations
- [ ] Create `IUserManagementService` interface
- [ ] Implement `UserManagementService`
- [ ] Write 10+ unit tests

**Day 42-43: Staff Management API**
- [ ] Create `StaffController.cs`
- [ ] Implement GET `/api/staff/users` (list staff)
- [ ] Implement POST `/api/staff/users` (create staff user)
- [ ] Implement PUT `/api/staff/users/{id}`
- [ ] Implement PUT `/api/staff/users/{id}/role` (change role)
- [ ] Implement DELETE `/api/staff/users/{id}`
- [ ] Add role-based authorization
- [ ] Write integration tests

**Day 43-45: Dashboard UI**
- [ ] Create `Admin/Dashboard.razor`
  - Summary cards (students, registrations, payments)
  - Recent activity timeline
  - Upcoming sessions list
  - Pending payments alert
  - Quick action buttons
- [ ] Create `Staff/Dashboard.razor` (read-only version)
- [ ] Create `Admin/Users/Index.razor` (staff user management)
- [ ] Create `Admin/Users/Create.razor` (add staff user)
- [ ] Create `Admin/Users/Edit.razor` (edit staff user)
- [ ] Implement role assignment dropdown
- [ ] Add user status toggle (Active/Inactive/Locked)
- [ ] Create activity log viewer

**Day 45: Testing & Polish**
- [ ] Test dashboard data loading
- [ ] Test role-based access control
- [ ] Test user management operations
- [ ] Test dashboard refresh
- [ ] Mobile responsiveness testing
- [ ] Performance testing (dashboard load time)

**Deliverables**:
- ✅ Dashboard service with statistics
- ✅ 5 API endpoints (Staff management)
- ✅ Admin dashboard UI
- ✅ Staff dashboard UI (read-only)
- ✅ User management interface
- ✅ Role-based access control
- ✅ 15+ tests passing

---

### Phase 9: Reporting & Analytics (Week 10)
**Duration**: 5 days  
**Effort**: 40 hours  
**Priority**: MEDIUM

#### Objectives
- Implement Excel export for all entities
- Create PDF report generation
- Build analytics charts and summaries
- Develop reporting API

#### Tasks

**Day 46-47: Reporting Services**
- [ ] Create `IReportService` interface
- [ ] Implement `ReportService`
  - GenerateRegistrationReport() - Excel
  - GeneratePaymentReport() - Excel
  - GenerateResultsReport() - Excel
  - GenerateSessionUtilizationReport() - Excel
  - GenerateDashboardSummary() - JSON
- [ ] Create `IPdfService` interface
- [ ] Implement `PdfService` using QuestPDF
  - GenerateRegistrationPdf() - student receipt
  - GenerateInvoicePdf() - payment invoice
  - GenerateResultPdf() - student results
- [ ] Implement Excel styling and formatting
- [ ] Write 10+ unit tests

**Day 47-48: Reports API**
- [ ] Create `ReportsController.cs`
- [ ] Implement GET `/api/reports/registrations` (Excel)
- [ ] Implement GET `/api/reports/payments` (Excel)
- [ ] Implement GET `/api/reports/results` (Excel)
- [ ] Implement GET `/api/reports/sessions` (Excel)
- [ ] Implement GET `/api/reports/summary` (JSON)
- [ ] Implement GET `/api/reports/pdf/registration/{id}`
- [ ] Implement GET `/api/reports/pdf/invoice/{paymentId}`
- [ ] Add date range filtering
- [ ] Write integration tests

**Day 48-50: Analytics UI**
- [ ] Create `Admin/Reports/Index.razor` (report center)
- [ ] Add export buttons for all reports
- [ ] Create `Admin/Analytics/Dashboard.razor`
  - Registration trend chart (line chart)
  - Payment status pie chart
  - Session utilization bar chart
  - Test type distribution chart
- [ ] Implement date range selector
- [ ] Add real-time data refresh
- [ ] Create report download history
- [ ] Implement chart interactivity (drill-down)

**Day 50: Testing**
- [ ] Test Excel report generation
- [ ] Test PDF generation
- [ ] Test chart rendering
- [ ] Test data accuracy
- [ ] Performance testing (large datasets)
- [ ] Browser compatibility testing

**Deliverables**:
- ✅ Reporting service (Excel + PDF)
- ✅ 8 API endpoints (Reports)
- ✅ Analytics dashboard with charts
- ✅ PDF generation (receipts, invoices)
- ✅ Excel export for all entities
- ✅ 15+ tests passing

---

### Phase 10: Testing, Security & Deployment (Week 11-12)
**Duration**: 10 days  
**Effort**: 80 hours  
**Priority**: CRITICAL

#### Objectives
- Comprehensive testing (unit, integration, E2E)
- Security audit and penetration testing
- Performance optimization
- Production deployment
- User acceptance testing

#### Tasks

**Day 51-53: Comprehensive Testing**
- [ ] Unit test coverage review (target: 80%+)
- [ ] Write missing unit tests for all services
- [ ] Integration tests for all API endpoints (61 endpoints)
- [ ] E2E tests using Playwright
  - Registration wizard flow
  - Payment flow
  - Admin CRUD operations
  - Staff read-only operations
- [ ] Accessibility testing with NVDA/JAWS
- [ ] Cross-browser testing (Chrome, Firefox, Edge, Safari)
- [ ] Mobile device testing (iOS, Android)
- [ ] Performance testing (Lighthouse, load testing)
- [ ] Fix all identified bugs

**Day 53-55: Security Audit**
- [ ] OWASP Top 10 vulnerability scan
- [ ] SQL injection testing
- [ ] XSS testing
- [ ] CSRF token validation
- [ ] Authentication bypass testing
- [ ] Authorization testing (role escalation)
- [ ] Payment security audit (EasyPay integration)
- [ ] Audit log verification (all CRUD operations)
- [ ] Secrets management review (Azure Key Vault)
- [ ] HTTPS enforcement verification
- [ ] Security headers configuration (CSP, HSTS)
- [ ] Fix all security vulnerabilities

**Day 55-57: Performance Optimization**
- [ ] Database query optimization (EF Core profiling)
- [ ] API response time optimization (<500ms)
- [ ] Page load time optimization (<3s on 3G)
- [ ] Implement caching (Redis for session data)
- [ ] Implement pagination for large lists
- [ ] Optimize Blazor bundle size
- [ ] Implement lazy loading for components
- [ ] Add CDN for static assets
- [ ] Database indexing optimization
- [ ] Load testing (1000+ concurrent users)

**Day 57-59: Production Deployment**
- [ ] Create Azure production environment
  - App Service (P1V2)
  - SQL Database (S2)
  - Key Vault
  - Application Insights
  - CDN
- [ ] Configure production database (backup, security)
- [ ] Apply all migrations to production
- [ ] Seed production data (venues, rooms, sessions)
- [ ] Configure SSL certificate (nbt.ac.za)
- [ ] Configure custom domain DNS
- [ ] Configure Application Insights monitoring
- [ ] Configure automated backups
- [ ] Set up CI/CD pipeline for production
- [ ] Deploy API to production
- [ ] Deploy Web App to production
- [ ] Smoke testing on production

**Day 59-60: User Acceptance Testing**
- [ ] Conduct UAT with NBT staff
- [ ] Test registration wizard with real users
- [ ] Test payment flow with test transactions
- [ ] Test admin dashboards
- [ ] Test reporting functionality
- [ ] Collect user feedback
- [ ] Fix high-priority issues
- [ ] Conduct final regression testing
- [ ] Obtain sign-off from stakeholders

**Day 60: Go-Live Preparation**
- [ ] Create production runbook
- [ ] Create rollback plan
- [ ] Train NBT staff on admin interface
- [ ] Prepare user documentation
- [ ] Set up support email/ticketing system
- [ ] Configure monitoring alerts
- [ ] Create incident response plan
- [ ] Perform final pre-launch checklist
- [ ] **GO LIVE** 🚀
- [ ] Monitor production for first 24 hours

**Deliverables**:
- ✅ 80%+ test coverage achieved
- ✅ Zero critical/high security vulnerabilities
- ✅ Performance targets met (<3s page load)
- ✅ Production environment configured
- ✅ Application deployed to production
- ✅ Monitoring and logging operational
- ✅ User training completed
- ✅ Documentation finalized
- ✅ **PRODUCTION READY** ✅

---

## 4. DIRECTORY STRUCTURE (Detailed)

### 4.1 Domain Layer Structure

```
src/NBT.Domain/
├── Common/
│   ├── BaseEntity.cs             # Existing
│   ├── IAuditableEntity.cs       # Existing
│   └── ValueObject.cs            # NEW - Base for value objects
├── Entities/
│   ├── User.cs                   # Existing
│   ├── Announcement.cs           # Existing
│   ├── ContactInquiry.cs         # Existing
│   ├── ContentPage.cs            # Existing
│   ├── DownloadableResource.cs   # Existing
│   ├── SystemSetting.cs          # Existing
│   ├── Student.cs                # NEW
│   ├── Registration.cs           # NEW
│   ├── Payment.cs                # NEW
│   ├── TestSession.cs            # NEW
│   ├── Venue.cs                  # NEW
│   ├── Room.cs                   # NEW
│   ├── RoomAllocation.cs         # NEW
│   ├── TestResult.cs             # NEW
│   └── AuditLog.cs               # NEW
├── Enums/
│   ├── UserRole.cs               # Existing
│   ├── InquiryType.cs            # Existing
│   ├── InquiryStatus.cs          # Existing
│   ├── AnnouncementCategory.cs   # Existing
│   ├── RegistrationStatus.cs     # NEW
│   ├── PaymentStatus.cs          # NEW
│   ├── SessionStatus.cs          # NEW
│   ├── TestType.cs               # NEW
│   └── PerformanceBand.cs        # NEW
├── ValueObjects/
│   ├── NBTNumber.cs              # NEW
│   └── SAIDNumber.cs             # NEW
└── Exceptions/
    ├── DomainException.cs        # NEW
    └── ValidationException.cs    # NEW
```

### 4.2 Application Layer Structure

```
src/NBT.Application/
├── Common/
│   ├── Interfaces/
│   │   ├── IApplicationDbContext.cs  # Existing
│   │   ├── IEmailService.cs          # Existing
│   │   └── ICurrentUserService.cs    # Existing
│   ├── Models/
│   │   ├── PagedResult.cs            # NEW
│   │   └── ApiResponse.cs            # NEW
│   └── Mappings/
│       └── MappingProfile.cs         # Update with new mappings
├── Students/
│   ├── DTOs/
│   │   ├── StudentDto.cs
│   │   ├── CreateStudentRequest.cs
│   │   └── UpdateStudentRequest.cs
│   ├── Interfaces/
│   │   └── IStudentService.cs
│   ├── Services/
│   │   ├── StudentService.cs
│   │   └── NBTNumberGenerator.cs
│   └── Validators/
│       └── CreateStudentValidator.cs
├── Registrations/
│   ├── DTOs/
│   │   ├── RegistrationDto.cs
│   │   ├── CreateRegistrationRequest.cs
│   │   └── RegistrationWizardRequest.cs
│   ├── Interfaces/
│   │   └── IRegistrationService.cs
│   ├── Services/
│   │   └── RegistrationService.cs
│   └── Validators/
│       └── RegistrationWizardValidator.cs
├── Payments/
│   ├── DTOs/
│   │   ├── PaymentDto.cs
│   │   ├── InitiatePaymentRequest.cs
│   │   └── EasyPayCallbackRequest.cs
│   ├── Interfaces/
│   │   └── IPaymentService.cs
│   ├── Services/
│   │   └── PaymentService.cs
│   └── Validators/
│       └── PaymentValidator.cs
├── TestSessions/
│   ├── DTOs/
│   │   ├── TestSessionDto.cs
│   │   └── CreateTestSessionRequest.cs
│   ├── Interfaces/
│   │   └── ITestSessionService.cs
│   └── Services/
│       └── TestSessionService.cs
├── Venues/
│   ├── DTOs/
│   │   ├── VenueDto.cs
│   │   ├── RoomDto.cs
│   │   └── CreateVenueRequest.cs
│   ├── Interfaces/
│   │   └── IVenueService.cs
│   └── Services/
│       └── VenueService.cs
├── TestResults/
│   ├── DTOs/
│   │   ├── TestResultDto.cs
│   │   └── ImportResultsRequest.cs
│   ├── Interfaces/
│   │   └── ITestResultService.cs
│   └── Services/
│       └── TestResultService.cs
└── Reports/
    ├── DTOs/
    │   └── DashboardDto.cs
    ├── Interfaces/
    │   └── IReportService.cs
    └── Services/
        └── ReportService.cs
```

### 4.3 Infrastructure Layer Structure

```
src/NBT.Infrastructure/
├── Persistence/
│   ├── ApplicationDbContext.cs       # Update with new DbSets
│   ├── ApplicationDbContextSeed.cs   # Update with new seed data
│   └── Configurations/
│       ├── StudentConfiguration.cs
│       ├── RegistrationConfiguration.cs
│       ├── PaymentConfiguration.cs
│       ├── TestSessionConfiguration.cs
│       ├── VenueConfiguration.cs
│       ├── RoomConfiguration.cs
│       ├── RoomAllocationConfiguration.cs
│       ├── TestResultConfiguration.cs
│       └── AuditLogConfiguration.cs
├── Services/
│   ├── EasyPayService.cs             # NEW
│   ├── ExcelService.cs               # NEW
│   ├── PdfService.cs                 # NEW
│   └── AuditService.cs               # NEW
└── Migrations/
    └── YYYYMMDDHHMMSS_AddCoreEntities.cs
```

### 4.4 Web API Layer Structure

```
src/NBT.WebAPI/
├── Controllers/
│   ├── StudentsController.cs         # NEW
│   ├── RegistrationsController.cs    # NEW
│   ├── BookingController.cs          # NEW
│   ├── PaymentsController.cs         # NEW
│   ├── VenuesController.cs           # NEW
│   ├── SessionsController.cs         # NEW
│   ├── ResultsController.cs          # NEW
│   ├── StaffController.cs            # NEW
│   └── ReportsController.cs          # NEW
├── Middleware/
│   ├── ExceptionHandlingMiddleware.cs # Existing
│   └── AuditLoggingMiddleware.cs     # NEW
└── appsettings.json                  # Add EasyPay settings
```

### 4.5 Web UI Layer Structure

```
src/NBT.WebUI/
├── Pages/
│   ├── Registration/                 # NEW
│   │   ├── Wizard.razor
│   │   ├── Step1_StudentInfo.razor
│   │   ├── Step2_TestSelection.razor
│   │   ├── Step3_SessionSelection.razor
│   │   ├── Step4_Confirmation.razor
│   │   ├── Payment.razor
│   │   └── PaymentCallback.razor
│   ├── Admin/                        # NEW
│   │   ├── Dashboard.razor
│   │   ├── Students/
│   │   │   ├── Index.razor
│   │   │   ├── Create.razor
│   │   │   └── Edit.razor
│   │   ├── Registrations/
│   │   │   ├── Index.razor
│   │   │   └── Details.razor
│   │   ├── Payments/
│   │   │   └── Index.razor
│   │   ├── Venues/
│   │   │   ├── Index.razor
│   │   │   ├── Create.razor
│   │   │   ├── Edit.razor
│   │   │   └── Rooms.razor
│   │   ├── Sessions/
│   │   │   ├── Index.razor
│   │   │   ├── Create.razor
│   │   │   └── Details.razor
│   │   ├── Results/
│   │   │   ├── Index.razor
│   │   │   └── Import.razor
│   │   ├── Reports/
│   │   │   └── Index.razor
│   │   ├── Analytics/
│   │   │   └── Dashboard.razor
│   │   └── Users/
│   │       ├── Index.razor
│   │       └── Create.razor
│   └── Staff/                        # NEW
│       └── Dashboard.razor
├── Components/
│   ├── Wizards/                      # NEW
│   │   └── WizardNavigation.razor
│   ├── DataGrids/                    # NEW
│   │   ├── StudentGrid.razor
│   │   └── RegistrationGrid.razor
│   ├── Charts/                       # NEW
│   │   ├── LineChart.razor
│   │   └── PieChart.razor
│   └── Forms/                        # NEW
│       ├── StudentForm.razor
│       └── VenueForm.razor
└── Services/                         # NEW
    ├── StudentApiService.cs
    ├── RegistrationApiService.cs
    ├── PaymentApiService.cs
    ├── VenueApiService.cs
    ├── SessionApiService.cs
    └── ReportApiService.cs
```

---

## 5. TESTING STRATEGY

### 5.1 Test Coverage Requirements

| Layer | Type | Target Coverage | Tools |
|-------|------|----------------|-------|
| Domain | Unit | 90%+ | xUnit, FluentAssertions |
| Application | Unit | 85%+ | xUnit, Moq |
| API | Integration | 80%+ | xUnit, WebApplicationFactory |
| UI Components | Component | 70%+ | bUnit |
| E2E | End-to-End | Critical Paths | Playwright |
| Performance | Load | 1000 users | k6, JMeter |
| Security | Penetration | OWASP Top 10 | OWASP ZAP |
| Accessibility | WCAG 2.1 AA | 100% | axe-core, NVDA |

### 5.2 Test Pyramid

```
           ╱╲
          ╱  ╲  E2E Tests (10 critical scenarios)
         ╱────╲
        ╱      ╲  Integration Tests (61 API endpoints)
       ╱────────╲
      ╱          ╲  Unit Tests (200+ tests)
     ╱────────────╲
    ──────────────── Total: 300+ tests
```

---

## 6. DEPLOYMENT STRATEGY

### 6.1 Azure Resources (Production)

| Resource | SKU | Purpose | Monthly Cost |
|----------|-----|---------|--------------|
| App Service Plan | P1V2 | Web App + API | ~$150 |
| SQL Database | S2 | Production DB | ~$150 |
| Azure Key Vault | Standard | Secrets | ~$5 |
| Application Insights | Standard | Monitoring | ~$50 |
| Azure CDN | Standard | Static assets | ~$20 |
| Azure Blob Storage | Standard | File uploads | ~$10 |
| **Total** | | | **~$385/month** |

### 6.2 CI/CD Pipeline

```yaml
# .github/workflows/production.yml
name: Production Deployment

on:
  push:
    branches: [main]

jobs:
  build-test-deploy:
    runs-on: ubuntu-latest
    steps:
      - Checkout code
      - Setup .NET 9
      - Restore dependencies
      - Build (Release)
      - Run all tests
      - Security scan
      - Code coverage check (>80%)
      - Deploy API to Azure
      - Deploy Web App to Azure
      - Run smoke tests
      - Send deployment notification
```

---

## 7. RISK MITIGATION

### 7.1 Technical Risks

| Risk | Impact | Probability | Mitigation |
|------|--------|-------------|------------|
| EasyPay integration delays | HIGH | MEDIUM | Test with sandbox early, have fallback |
| Performance issues | HIGH | LOW | Load test throughout, optimize queries |
| Security vulnerabilities | CRITICAL | LOW | Security audit each phase, penetration test |
| Excel import errors | MEDIUM | MEDIUM | Comprehensive validation, error reporting |
| Migration failures | HIGH | LOW | Test migrations, have rollback plan |

### 7.2 Business Risks

| Risk | Impact | Probability | Mitigation |
|------|--------|-------------|------------|
| Timeline overrun | MEDIUM | MEDIUM | Buffer time in schedule, prioritize MVP |
| Scope creep | MEDIUM | HIGH | Strict change control, document out-of-scope |
| User adoption | MEDIUM | LOW | UAT with real users, training sessions |
| Data migration | HIGH | MEDIUM | Dry run migrations, data validation |

---

## 8. SUCCESS CRITERIA

### 8.1 Technical Criteria

- [x] All 61 API endpoints functional and documented
- [x] 80%+ test coverage achieved
- [x] Zero critical/high security vulnerabilities
- [x] <3 second page load time (3G connection)
- [x] <500ms API response time (95th percentile)
- [x] WCAG 2.1 AA compliance (100%)
- [x] Support 1000+ concurrent users
- [x] 99.5% uptime SLA

### 8.2 Business Criteria

- [x] Complete registration wizard (4 steps)
- [x] EasyPay integration functional
- [x] Excel import working with validation
- [x] All dashboards operational
- [x] Reporting and analytics functional
- [x] Audit logging for all operations
- [x] Role-based access control enforced
- [x] User training completed

### 8.3 User Acceptance Criteria

- [x] Students can register successfully
- [x] Payments processed without errors
- [x] Admin can manage all entities via UI
- [x] Staff can view reports
- [x] Results can be imported and released
- [x] System is intuitive and user-friendly
- [x] Mobile-responsive on all devices

---

## 9. POST-LAUNCH ROADMAP

### 9.1 Phase 11: Enhancements (Month 4)
- Online result viewing for students
- SMS notifications for payments
- Advanced search with filters
- Bulk operations (registrations, payments)
- Dashboard customization

### 9.2 Phase 12: Advanced Features (Month 5-6)
- Student self-service portal
- Institutional reporting dashboard
- Advanced analytics (predictive models)
- Mobile application (Progressive Web App)
- API for third-party integrations

---

## 10. RESOURCE ALLOCATION

### 10.1 Team Structure

| Role | Count | Responsibilities |
|------|-------|------------------|
| Senior Developer | 1 | Architecture, code review, complex features |
| Mid-Level Developer | 1 | Core features, testing, documentation |
| Junior Developer | 1 | UI components, bug fixes, testing support |
| QA Engineer | 0.5 | Testing strategy, test execution, UAT |
| DevOps Engineer | 0.5 | CI/CD, Azure setup, monitoring |
| **Total** | **4 FTE** | |

### 10.2 Estimated Hours

| Phase | Developer Hours | QA Hours | DevOps Hours | Total |
|-------|----------------|----------|--------------|-------|
| Phase 1-2 | 80 | 0 | 0 | 80 |
| Phase 3-4 | 120 | 20 | 0 | 140 |
| Phase 5-7 | 120 | 20 | 0 | 140 |
| Phase 8-9 | 80 | 20 | 0 | 100 |
| Phase 10 | 40 | 40 | 40 | 120 |
| **Total** | **440** | **100** | **40** | **580 hours** |

---

## 11. DOCUMENTATION DELIVERABLES

### 11.1 Technical Documentation
- [ ] API documentation (Swagger/OpenAPI)
- [ ] Database schema documentation
- [ ] Architecture diagrams (C4 model)
- [ ] Deployment guide
- [ ] Development setup guide
- [ ] Testing guide

### 11.2 User Documentation
- [ ] Admin user manual
- [ ] Staff user manual
- [ ] Registration wizard guide (for students)
- [ ] Troubleshooting guide
- [ ] FAQ document
- [ ] Video tutorials

### 11.3 Operational Documentation
- [ ] Production runbook
- [ ] Incident response plan
- [ ] Disaster recovery plan
- [ ] Monitoring and alerting guide
- [ ] Backup and restore procedures
- [ ] Performance tuning guide

---

## 12. SIGN-OFF & APPROVAL

### 12.1 Stakeholder Approval

| Stakeholder | Role | Signature | Date |
|-------------|------|-----------|------|
| | Technical Lead | __________ | ______ |
| | Project Manager | __________ | ______ |
| | NBT Director | __________ | ______ |
| | Security Officer | __________ | ______ |

### 12.2 Change Control

All changes to this plan must be:
1. Documented in writing
2. Impact assessed (timeline, budget, scope)
3. Approved by Technical Lead and Project Manager
4. Communicated to all stakeholders

---

## 13. APPENDIX

### 13.1 Glossary

- **NBT Number**: 9-digit student identifier with Luhn checksum
- **SA ID Number**: 13-digit South African ID with date/gender extraction
- **EasyPay**: South African payment gateway for online transactions
- **Luhn Algorithm**: Checksum formula for validating identification numbers
- **WCAG 2.1 AA**: Web Content Accessibility Guidelines Level AA compliance
- **Clean Architecture**: Layered architecture with dependency inversion

### 13.2 References

- [Contracts Document](./contracts.md)
- [Constitution](../../CONSTITUTION.md)
- [NBT Number Generation](../../NBT%20number%20generation.docx)
- [Project Status](../../PROJECT-STATUS.md)
- [EF Core Documentation](https://docs.microsoft.com/ef-core)
- [Blazor Documentation](https://docs.microsoft.com/aspnet/core/blazor)

---

**PLAN STATUS**: ✅ APPROVED FOR IMPLEMENTATION  
**VERSION**: 1.0  
**LAST UPDATED**: 2025-11-08  
**NEXT REVIEW**: Weekly during implementation

---

**Ready to proceed with Phase 1: Foundation & Domain Setup** 🚀
