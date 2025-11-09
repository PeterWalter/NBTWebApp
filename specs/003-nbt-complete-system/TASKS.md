# NBT Integrated Web Application - Task Breakdown

This document provides a complete task breakdown organized by implementation phases. Each task includes priority, estimated effort, dependencies, and acceptance criteria.

## Legend
- **Priority**: P0 (Critical), P1 (High), P2 (Medium), P3 (Low)
- **Effort**: S (Small: < 4 hours), M (Medium: 4-8 hours), L (Large: 1-2 days), XL (Extra Large: 2-5 days)
- **Status**: Not Started, In Progress, Complete, Blocked

---

## Phase 0: Shell Audit & Gap Analysis

### TASK-001: Project Structure Review
**Priority**: P0 | **Effort**: M | **Status**: Not Started
**Dependencies**: None

**Description**: Review current solution structure and identify missing components.

**Acceptance Criteria**:
- [ ] All projects audited (Domain, Application, Infrastructure, API, Client)
- [ ] Missing folders/projects documented
- [ ] Gap analysis report created
- [ ] Component inventory completed

---

### TASK-002: Database Schema Review
**Priority**: P0 | **Effort**: M | **Status**: Not Started
**Dependencies**: TASK-001

**Description**: Validate EF Core models and database schema against specification.

**Acceptance Criteria**:
- [ ] All entities reviewed
- [ ] Missing entities identified
- [ ] ERD diagram created
- [ ] Migration scripts reviewed

---

### TASK-003: API Endpoint Audit
**Priority**: P0 | **Effort**: M | **Status**: Not Started
**Dependencies**: TASK-001

**Description**: Review existing API controllers and identify missing endpoints.

**Acceptance Criteria**:
- [ ] All controllers audited
- [ ] Endpoint inventory created
- [ ] Missing endpoints documented
- [ ] Swagger documentation reviewed

---

### TASK-004: Frontend Component Audit
**Priority**: P0 | **Effort**: L | **Status**: Not Started
**Dependencies**: TASK-001

**Description**: Review Blazor components and identify MudBlazor dependencies for migration.

**Acceptance Criteria**:
- [ ] All components audited
- [ ] MudBlazor usage documented
- [ ] Missing components listed
- [ ] Navigation map created

---

### TASK-005: Configuration Review
**Priority**: P0 | **Effort**: S | **Status**: Not Started
**Dependencies**: TASK-001

**Description**: Verify configurations and dependency injection setup.

**Acceptance Criteria**:
- [ ] appsettings.json reviewed
- [ ] Service registrations verified
- [ ] Missing configurations documented

---

## Phase 1: Foundation & Infrastructure

### TASK-101: Upgrade to .NET 9.0
**Priority**: P0 | **Effort**: M | **Status**: Not Started
**Dependencies**: TASK-005

**Description**: Update all projects to target .NET 9.0 and update NuGet packages.

**Acceptance Criteria**:
- [ ] All project files updated to .NET 9.0
- [ ] NuGet packages updated to compatible versions
- [ ] EF Core 9.0 installed
- [ ] Solution builds successfully
- [ ] All tests pass

---

### TASK-102: Complete Domain Model - Student Entity
**Priority**: P0 | **Effort**: M | **Status**: Not Started
**Dependencies**: TASK-101

**Description**: Implement/update Student entity with NBT number, SA ID, and Foreign ID support.

**Acceptance Criteria**:
- [ ] Student entity complete with all properties
- [ ] Support for SA ID and Foreign ID/Passport
- [ ] Proper EF Core configurations
- [ ] Navigation properties defined
- [ ] Migration generated

---

### TASK-103: Complete Domain Model - Registration Entity
**Priority**: P0 | **Effort**: M | **Status**: Not Started
**Dependencies**: TASK-102

**Description**: Implement Registration entity with wizard progress tracking.

**Acceptance Criteria**:
- [ ] Registration entity with step tracking
- [ ] Relationships to Student configured
- [ ] Auto-save support included
- [ ] Migration generated

---

### TASK-104: Complete Domain Model - Booking Entity
**Priority**: P0 | **Effort**: M | **Status**: Not Started
**Dependencies**: TASK-102

**Description**: Implement Booking entity with business rules support.

**Acceptance Criteria**:
- [ ] Booking entity complete
- [ ] Relationships configured
- [ ] Business rule fields included
- [ ] Migration generated

---

### TASK-105: Complete Domain Model - Payment Entities
**Priority**: P0 | **Effort**: M | **Status**: Not Started
**Dependencies**: TASK-104

**Description**: Implement Payment and PaymentInstallment entities.

**Acceptance Criteria**:
- [ ] Payment entity complete
- [ ] PaymentInstallment entity created
- [ ] Installment relationship configured
- [ ] Migration generated

---

### TASK-106: Complete Domain Model - Result Entity
**Priority**: P0 | **Effort**: M | **Status**: Not Started
**Dependencies**: TASK-104

**Description**: Implement Result entity with barcode and performance levels.

**Acceptance Criteria**:
- [ ] Result entity with all score fields
- [ ] Performance level enums defined
- [ ] Barcode field included
- [ ] Migration generated

---

### TASK-107: Complete Domain Model - Venue & TestSession Entities
**Priority**: P0 | **Effort**: M | **Status**: Not Started
**Dependencies**: TASK-102

**Description**: Implement Venue, TestSession, TestDate, and related entities.

**Acceptance Criteria**:
- [ ] Venue entity with type enum
- [ ] TestSession linked to Venue (not Room)
- [ ] TestDate entity with closing dates
- [ ] VenueAvailability entity
- [ ] Migrations generated

---

### TASK-108: Complete Domain Model - Supporting Entities
**Priority**: P1 | **Effort**: M | **Status**: Not Started
**Dependencies**: TASK-102

**Description**: Implement IntakeYear, SpecialSession, Document, Notification, User, AuditLog.

**Acceptance Criteria**:
- [ ] All supporting entities created
- [ ] Relationships configured
- [ ] Migrations generated
- [ ] All enums defined

---

### TASK-109: Implement Repository Pattern
**Priority**: P0 | **Effort**: L | **Status**: Not Started
**Dependencies**: TASK-108

**Description**: Complete generic repository pattern and unit of work.

**Acceptance Criteria**:
- [ ] IRepository<T> interface defined
- [ ] Generic repository implemented
- [ ] Unit of work pattern implemented
- [ ] All entity repositories created
- [ ] Service registrations added

---

### TASK-110: Implement JWT Authentication
**Priority**: P0 | **Effort**: L | **Status**: Not Started
**Dependencies**: TASK-109

**Description**: Complete JWT-based authentication system with refresh tokens.

**Acceptance Criteria**:
- [ ] JWT token generation implemented
- [ ] Token validation configured
- [ ] Refresh token support added
- [ ] Authentication middleware configured
- [ ] Tests passing

---

### TASK-111: Implement Role-Based Authorization
**Priority**: P0 | **Effort**: M | **Status**: Not Started
**Dependencies**: TASK-110

**Description**: Configure role-based authorization for all user roles.

**Acceptance Criteria**:
- [ ] Roles defined (Student, Staff, Admin, SuperUser)
- [ ] Authorization policies configured
- [ ] Controller attributes applied
- [ ] Tests passing

---

### TASK-112: Implement Audit Logging
**Priority**: P1 | **Effort**: M | **Status**: Not Started
**Dependencies**: TASK-109

**Description**: Add audit logging interceptor for all data modifications.

**Acceptance Criteria**:
- [ ] EF Core interceptor implemented
- [ ] AuditLog entity populated automatically
- [ ] User context captured
- [ ] Tests passing

---

## Phase 2: Registration Wizard

### TASK-201: NBT Number Generator Service
**Priority**: P0 | **Effort**: M | **Status**: Not Started
**Dependencies**: TASK-109

**Description**: Implement NBT number generation using Luhn algorithm.

**Acceptance Criteria**:
- [ ] 14-digit number generation
- [ ] Luhn algorithm validation
- [ ] Uniqueness check
- [ ] Unit tests passing (100% coverage)

---

### TASK-202: SA ID Validator Service
**Priority**: P0 | **Effort**: M | **Status**: Not Started
**Dependencies**: TASK-109

**Description**: Implement SA ID validation with DOB and gender extraction.

**Acceptance Criteria**:
- [ ] 13-digit validation
- [ ] Luhn algorithm check
- [ ] DOB extraction
- [ ] Gender extraction
- [ ] Unit tests passing

---

### TASK-203: Registration Service - Create/Resume
**Priority**: P0 | **Effort**: L | **Status**: Not Started
**Dependencies**: TASK-202

**Description**: Implement registration service with create and resume functionality.

**Acceptance Criteria**:
- [ ] Create registration API
- [ ] Resume registration API
- [ ] Step progress tracking
- [ ] Auto-save functionality
- [ ] Duplicate prevention
- [ ] Unit tests passing

---

### TASK-204: Registration API - Step 1
**Priority**: P0 | **Effort**: M | **Status**: Not Started
**Dependencies**: TASK-203

**Description**: Create API endpoint for registration step 1 (Account & Personal).

**Acceptance Criteria**:
- [ ] POST /api/registration/create endpoint
- [ ] DTO validation
- [ ] SA ID auto-extraction working
- [ ] Foreign ID support
- [ ] Tests passing

---

### TASK-205: Registration API - Step 2
**Priority**: P0 | **Effort**: M | **Status**: Not Started
**Dependencies**: TASK-204

**Description**: Create API endpoint for registration step 2 (Academic & Test).

**Acceptance Criteria**:
- [ ] PUT /api/registration/{id}/step2 endpoint
- [ ] Academic info saved
- [ ] Test preferences captured
- [ ] Tests passing

---

### TASK-206: Registration API - Step 3
**Priority**: P0 | **Effort**: M | **Status**: Not Started
**Dependencies**: TASK-205

**Description**: Create API endpoint for registration step 3 (Survey).

**Acceptance Criteria**:
- [ ] PUT /api/registration/{id}/step3 endpoint
- [ ] Survey responses saved as JSON
- [ ] Registration marked complete
- [ ] NBT number generated
- [ ] Tests passing

---

### TASK-207: Registration Wizard - Frontend Components
**Priority**: P0 | **Effort**: XL | **Status**: Not Started
**Dependencies**: TASK-206

**Description**: Build complete 3-step registration wizard frontend using Fluent UI.

**Components to Create**:
- RegistrationWizard.razor
- Step1AccountPersonal.razor
- Step2AcademicTest.razor
- Step3Survey.razor
- WizardNavigation.razor
- ProgressIndicator.razor
- SaIdInput.razor (with auto-extraction)
- ForeignIdInput.razor
- EmailValidation.razor
- PasswordStrength.razor

**Acceptance Criteria**:
- [ ] All 3 steps implemented
- [ ] SA ID auto-fills DOB and Gender
- [ ] Age calculated from DOB (not manual)
- [ ] Foreign ID flow working
- [ ] Client-side validation functional
- [ ] Auto-save working
- [ ] Resume from interruption working
- [ ] NBT number generated on completion
- [ ] Redirect to login after completion
- [ ] E2E tests passing

---

### TASK-208: Registration Wizard - ViewModels
**Priority**: P0 | **Effort**: M | **Status**: Not Started
**Dependencies**: TASK-207

**Description**: Create ViewModels for wizard state management.

**Acceptance Criteria**:
- [ ] RegistrationWizardViewModel
- [ ] AccountPersonalViewModel
- [ ] AcademicTestViewModel
- [ ] SurveyViewModel
- [ ] State management working
- [ ] Auto-save integrated

---

## Phase 3: Booking & Payment Module

### TASK-301: Booking Service - Business Rules
**Priority**: P0 | **Effort**: L | **Status**: Not Started
**Dependencies**: TASK-109

**Description**: Implement booking service with all business rules enforcement.

**Business Rules to Implement**:
- One active booking at a time
- Can book next test only after closing date
- Maximum 2 tests per year
- Tests valid for 3 years
- Booking changes before closing date

**Acceptance Criteria**:
- [ ] All business rules enforced
- [ ] Eligibility check method
- [ ] Booking validation
- [ ] Unit tests for all rules (100% coverage)

---

### TASK-302: Booking API Endpoints
**Priority**: P0 | **Effort**: M | **Status**: Not Started
**Dependencies**: TASK-301

**Description**: Create all booking API endpoints.

**Acceptance Criteria**:
- [ ] POST /api/booking/create
- [ ] PUT /api/booking/{id}
- [ ] DELETE /api/booking/{id}
- [ ] GET /api/booking/{id}
- [ ] GET /api/booking/student/{studentId}
- [ ] GET /api/booking/check-eligibility/{studentId}
- [ ] Integration tests passing

---

### TASK-303: Venue Availability Service
**Priority**: P0 | **Effort**: M | **Status**: Not Started
**Dependencies**: TASK-107

**Description**: Implement venue availability checking and calendar service.

**Acceptance Criteria**:
- [ ] Availability check by date
- [ ] Capacity tracking
- [ ] Test date calendar
- [ ] Sunday/Online test identification
- [ ] Unit tests passing

---

### TASK-304: Payment Service - Core
**Priority**: P0 | **Effort**: L | **Status**: Not Started
**Dependencies**: TASK-109

**Description**: Implement payment service with installment support.

**Acceptance Criteria**:
- [ ] Payment recording
- [ ] Installment support
- [ ] Payment to booking association
- [ ] Cost calculation by intake year
- [ ] Status tracking
- [ ] Unit tests passing

---

### TASK-305: EasyPay Integration Service
**Priority**: P1 | **Effort**: L | **Status**: Not Started
**Dependencies**: TASK-304

**Description**: Integrate with EasyPay payment gateway.

**Acceptance Criteria**:
- [ ] Payment reference generation
- [ ] Webhook implementation
- [ ] Status update handling
- [ ] Error handling
- [ ] Tests with mock service

---

### TASK-306: Payment File Upload Processor
**Priority**: P1 | **Effort**: M | **Status**: Not Started
**Dependencies**: TASK-304

**Description**: Implement bank payment file upload and processing.

**Acceptance Criteria**:
- [ ] File format validation
- [ ] File parsing
- [ ] Payment matching
- [ ] Error reporting
- [ ] Unit tests passing

---

### TASK-307: Payment API Endpoints
**Priority**: P0 | **Effort**: M | **Status**: Not Started
**Dependencies**: TASK-306

**Description**: Create all payment API endpoints.

**Acceptance Criteria**:
- [ ] POST /api/payment/create
- [ ] POST /api/payment/installment
- [ ] POST /api/payment/upload-file
- [ ] POST /api/payment/easypay-callback
- [ ] GET /api/payment/status/{bookingId}
- [ ] Integration tests passing

---

### TASK-308: Booking Wizard - Frontend
**Priority**: P0 | **Effort**: L | **Status**: Not Started
**Dependencies**: TASK-302, TASK-307

**Description**: Build booking wizard frontend with Fluent UI.

**Components**:
- BookingWizard.razor
- TestTypeSelection.razor
- VenueDateSelection.razor
- BookingReview.razor
- PaymentIntegration.razor
- PaymentStatus.razor
- InstallmentTracker.razor

**Acceptance Criteria**:
- [ ] Booking flow smooth
- [ ] Venue calendar shows availability
- [ ] Sunday/Online tests highlighted
- [ ] Business rules enforced client-side
- [ ] EasyPay integration working
- [ ] Payment status visible
- [ ] E2E tests passing

---

## Phase 4: Staff/Admin Dashboards

### TASK-401: Student Management Service
**Priority**: P0 | **Effort**: M | **Status**: Not Started
**Dependencies**: TASK-109

**Description**: Implement student management service for staff/admin.

**Acceptance Criteria**:
- [ ] List all students
- [ ] Search and filter
- [ ] CRUD operations
- [ ] View registration status
- [ ] Authorization checks
- [ ] Unit tests passing

---

### TASK-402: Student Management API
**Priority**: P0 | **Effort**: M | **Status**: Not Started
**Dependencies**: TASK-401

**Description**: Create student management API endpoints.

**Acceptance Criteria**:
- [ ] GET /api/students/list
- [ ] GET /api/students/search
- [ ] GET /api/students/{id}
- [ ] PUT /api/students/{id}
- [ ] DELETE /api/students/{id}
- [ ] Integration tests passing

---

### TASK-403: Booking Management Service
**Priority**: P0 | **Effort**: M | **Status**: Not Started
**Dependencies**: TASK-301

**Description**: Implement booking management service for staff.

**Acceptance Criteria**:
- [ ] List all bookings
- [ ] Filter by date/venue/status
- [ ] Modify bookings
- [ ] Cancel bookings
- [ ] Unit tests passing

---

### TASK-404: Payment Management Service
**Priority**: P0 | **Effort**: M | **Status**: Not Started
**Dependencies**: TASK-304

**Description**: Implement payment management service for staff.

**Acceptance Criteria**:
- [ ] Process manual payments
- [ ] Upload bank files
- [ ] Reconcile payments
- [ ] View payment history
- [ ] Generate payment reports
- [ ] Unit tests passing

---

### TASK-405: User Management Service
**Priority**: P1 | **Effort**: M | **Status**: Not Started
**Dependencies**: TASK-110

**Description**: Implement user and role management for admins.

**Acceptance Criteria**:
- [ ] Create/edit/delete users
- [ ] Role assignment
- [ ] Activation/deactivation
- [ ] Unit tests passing

---

### TASK-406: Staff Dashboard Frontend
**Priority**: P0 | **Effort**: XL | **Status**: Not Started
**Dependencies**: TASK-402, TASK-403, TASK-404

**Description**: Build staff dashboard with left-side menu.

**Components**:
- StaffDashboard.razor
- SideMenu.razor
- StudentManagement.razor
- BookingManagement.razor
- PaymentManagement.razor
- ResultsManagement.razor
- DashboardStats.razor

**Acceptance Criteria**:
- [ ] Dashboard responsive
- [ ] Left-side navigation functional
- [ ] All CRUD operations working
- [ ] Search and filters responsive
- [ ] Data grids performant
- [ ] E2E tests passing

---

### TASK-407: Admin Dashboard Frontend
**Priority**: P0 | **Effort**: L | **Status**: Not Started
**Dependencies**: TASK-405, TASK-406

**Description**: Build admin dashboard with additional features.

**Components**:
- AdminDashboard.razor
- UserManagement.razor
- VenueManagement.razor
- AuditLogViewer.razor
- SystemConfiguration.razor

**Acceptance Criteria**:
- [ ] All staff features accessible
- [ ] User management working
- [ ] Venue management functional
- [ ] Audit logs viewable
- [ ] E2E tests passing

---

## Phase 5: Venue & Room Management

### TASK-501: Venue Service - CRUD
**Priority**: P0 | **Effort**: M | **Status**: Not Started
**Dependencies**: TASK-107

**Description**: Implement venue management service.

**Acceptance Criteria**:
- [ ] Create/update/delete venues
- [ ] Venue types supported
- [ ] Capacity tracking
- [ ] Unit tests passing

---

### TASK-502: Test Date Service
**Priority**: P0 | **Effort**: M | **Status**: Not Started
**Dependencies**: TASK-107

**Description**: Implement test date and calendar management.

**Acceptance Criteria**:
- [ ] Create/update test dates
- [ ] Closing date management
- [ ] Sunday test marking
- [ ] Online test support
- [ ] Calendar view data
- [ ] Unit tests passing

---

### TASK-503: Venue API Endpoints
**Priority**: P0 | **Effort**: M | **Status**: Not Started
**Dependencies**: TASK-501, TASK-502

**Description**: Create venue and test date API endpoints.

**Acceptance Criteria**:
- [ ] Venue CRUD endpoints
- [ ] Test date CRUD endpoints
- [ ] Availability endpoints
- [ ] Calendar endpoint
- [ ] Integration tests passing

---

### TASK-504: Venue Management Frontend
**Priority**: P0 | **Effort**: L | **Status**: Not Started
**Dependencies**: TASK-503

**Description**: Build venue management UI with Fluent UI.

**Components**:
- VenueList.razor
- VenueEditor.razor
- VenueTypeSelector.razor
- TestDateCalendar.razor
- OnlineTestManager.razor
- VenueAvailability.razor

**Acceptance Criteria**:
- [ ] Venue listing with filters
- [ ] Create/edit forms working
- [ ] Calendar functional
- [ ] Sunday/Online tests highlighted
- [ ] Closing dates visible
- [ ] E2E tests passing

---

## Phase 6: Results Management

### TASK-601: Results Service - Core
**Priority**: P0 | **Effort**: M | **Status**: Not Started
**Dependencies**: TASK-109

**Description**: Implement results management service.

**Acceptance Criteria**:
- [ ] Create/update results
- [ ] Barcode generation
- [ ] Performance level assignment
- [ ] Payment status check for download
- [ ] Unit tests passing

---

### TASK-602: Results Import Service
**Priority**: P0 | **Effort**: L | **Status**: Not Started
**Dependencies**: TASK-601

**Description**: Implement bulk results import service.

**Acceptance Criteria**:
- [ ] CSV/Excel file parsing
- [ ] Validation
- [ ] Bulk insert
- [ ] Error reporting
- [ ] Unit tests passing

---

### TASK-603: Certificate Generation Service
**Priority**: P0 | **Effort**: L | **Status**: Not Started
**Dependencies**: TASK-601

**Description**: Implement PDF certificate generation.

**Acceptance Criteria**:
- [ ] PDF template
- [ ] Barcode on certificate
- [ ] Official NBT branding
- [ ] Data population
- [ ] Payment check enforced
- [ ] Unit tests passing

---

### TASK-604: Results API Endpoints
**Priority**: P0 | **Effort**: M | **Status**: Not Started
**Dependencies**: TASK-603

**Description**: Create results API endpoints.

**Acceptance Criteria**:
- [ ] POST /api/results/import
- [ ] GET /api/results/student/{studentId}
- [ ] GET /api/results/certificate/{id}
- [ ] GET /api/results/verify/{barcode}
- [ ] Integration tests passing

---

### TASK-605: Results Frontend - Student View
**Priority**: P0 | **Effort**: M | **Status**: Not Started
**Dependencies**: TASK-604

**Description**: Build results viewing for students.

**Components**:
- ResultsViewer.razor
- ResultsCertificate.razor
- PerformanceLevelDisplay.razor

**Acceptance Criteria**:
- [ ] Results displayed clearly
- [ ] Performance levels shown
- [ ] Certificate download (paid only)
- [ ] Multiple test history
- [ ] Barcodes visible
- [ ] E2E tests passing

---

### TASK-606: Results Frontend - Staff/Admin
**Priority**: P0 | **Effort**: M | **Status**: Not Started
**Dependencies**: TASK-604

**Description**: Build results management for staff/admin.

**Components**:
- ResultsImport.razor
- ResultsManagement.razor
- BulkImportProgress.razor

**Acceptance Criteria**:
- [ ] Bulk import UI
- [ ] Progress tracking
- [ ] Error display
- [ ] View all results
- [ ] Generate any certificate
- [ ] E2E tests passing

---

## Phase 7: Reporting & Analytics

### TASK-701: Reporting Service
**Priority**: P1 | **Effort**: L | **Status**: Not Started
**Dependencies**: TASK-109

**Description**: Implement reporting service with standard reports.

**Standard Reports**:
- Registration summary
- Booking statistics
- Payment reconciliation
- Test results analysis
- Venue utilization
- User activity

**Acceptance Criteria**:
- [ ] All standard reports implemented
- [ ] Data aggregation correct
- [ ] Filters working
- [ ] Unit tests passing

---

### TASK-702: Excel Export Service
**Priority**: P1 | **Effort**: M | **Status**: Not Started
**Dependencies**: TASK-701

**Description**: Implement Excel export functionality.

**Acceptance Criteria**:
- [ ] XLSX generation
- [ ] Multiple sheets support
- [ ] Formatting applied
- [ ] Large dataset handling
- [ ] Unit tests passing

---

### TASK-703: Custom Report Builder
**Priority**: P2 | **Effort**: L | **Status**: Not Started
**Dependencies**: TASK-701

**Description**: Implement custom ad-hoc report builder.

**Acceptance Criteria**:
- [ ] Query builder UI
- [ ] Field selection
- [ ] Filter configuration
- [ ] Sort/group options
- [ ] Save custom reports
- [ ] Unit tests passing

---

### TASK-704: Report API Endpoints
**Priority**: P1 | **Effort**: M | **Status**: Not Started
**Dependencies**: TASK-702

**Description**: Create reporting API endpoints.

**Acceptance Criteria**:
- [ ] All standard report endpoints
- [ ] Custom report endpoint
- [ ] Export endpoint
- [ ] Dashboard stats endpoint
- [ ] Integration tests passing

---

### TASK-705: Reporting Dashboard Frontend
**Priority**: P1 | **Effort**: L | **Status**: Not Started
**Dependencies**: TASK-704

**Description**: Build reporting dashboard with charts.

**Components**:
- ReportDashboard.razor
- ReportSelector.razor
- CustomReportBuilder.razor
- ChartDisplay.razor
- ExportOptions.razor

**Acceptance Criteria**:
- [ ] Report selection intuitive
- [ ] Charts clear
- [ ] Export options working
- [ ] Custom builder functional
- [ ] E2E tests passing

---

## Phase 8: Landing Page & Public Content

### TASK-801: Landing Page Layout
**Priority**: P1 | **Effort**: M | **Status**: Not Started
**Dependencies**: None

**Description**: Design and implement landing page layout.

**Acceptance Criteria**:
- [ ] Responsive layout
- [ ] Main navigation menus
- [ ] Hero section
- [ ] Call-to-action buttons
- [ ] News section
- [ ] Footer

---

### TASK-802: Main Navigation Menus
**Priority**: P1 | **Effort**: L | **Status**: Not Started
**Dependencies**: TASK-801

**Description**: Implement all main menus with submenus matching current NBT website.

**Menus**:
- Applicants (with submenus)
- Institutions (with submenus)
- Educators (with submenus)

**Acceptance Criteria**:
- [ ] All menus implemented
- [ ] Submenus match current site
- [ ] Responsive on mobile
- [ ] Accessible

---

### TASK-803: Video Integration
**Priority**: P2 | **Effort**: M | **Status**: Not Started
**Dependencies**: TASK-801

**Description**: Add video player capability for educational content.

**Acceptance Criteria**:
- [ ] Video player component
- [ ] YouTube/Vimeo embed support
- [ ] Responsive sizing
- [ ] Accessibility features

---

### TASK-804: Public Content Pages
**Priority**: P1 | **Effort**: L | **Status**: Not Started
**Dependencies**: TASK-802

**Description**: Create all public informational pages.

**Pages**:
- About NBT
- Test Information
- Registration Guide
- Payment Information
- Special Sessions
- Contact Information
- Privacy Policy
- Terms of Service
- FAQ

**Acceptance Criteria**:
- [ ] All pages created
- [ ] Content structured
- [ ] Breadcrumbs working
- [ ] Mobile responsive
- [ ] WCAG 2.1 AA compliant

---

## Phase 9: Fluent UI Migration

### TASK-901: MudBlazor Dependency Audit
**Priority**: P0 | **Effort**: M | **Status**: Not Started
**Dependencies**: TASK-004

**Description**: Complete audit of all MudBlazor component usage.

**Acceptance Criteria**:
- [ ] All MudBlazor usages documented
- [ ] Component mapping created
- [ ] Migration strategy defined
- [ ] Effort estimated

---

### TASK-902: Replace MudBlazor Components
**Priority**: P0 | **Effort**: XL | **Status**: Not Started
**Dependencies**: TASK-901

**Description**: Systematically replace all MudBlazor components with Fluent UI.

**Component Mappings**:
- MudTextField → FluentTextField
- MudButton → FluentButton
- MudDataGrid → FluentDataGrid
- MudDialog → FluentDialog
- MudSelect → FluentSelect
- MudMenu → FluentMenu
- MudCard → FluentCard
- etc.

**Acceptance Criteria**:
- [ ] All MudBlazor components replaced
- [ ] Functionality preserved
- [ ] Styling consistent
- [ ] All pages tested
- [ ] E2E tests passing

---

### TASK-903: Remove MudBlazor Package
**Priority**: P0 | **Effort**: S | **Status**: Not Started
**Dependencies**: TASK-902

**Description**: Remove MudBlazor NuGet package and references.

**Acceptance Criteria**:
- [ ] MudBlazor package removed
- [ ] No MudBlazor imports remaining
- [ ] Solution builds successfully
- [ ] All tests pass

---

### TASK-904: Fluent UI Theming
**Priority**: P1 | **Effort**: M | **Status**: Not Started
**Dependencies**: TASK-903

**Description**: Configure Fluent UI theme with NBT branding.

**Acceptance Criteria**:
- [ ] Theme configured
- [ ] Brand colors applied
- [ ] Typography set
- [ ] Component defaults configured
- [ ] Consistent across app

---

## Phase 10: Special Features & Polish

### TASK-1001: Email Service Implementation
**Priority**: P1 | **Effort**: M | **Status**: Not Started
**Dependencies**: TASK-109

**Description**: Implement email notification service.

**Acceptance Criteria**:
- [ ] SMTP configuration
- [ ] Template system
- [ ] Queue implementation
- [ ] Delivery tracking
- [ ] Unit tests passing

---

### TASK-1002: SMS Service Implementation
**Priority**: P2 | **Effort**: M | **Status**: Not Started
**Dependencies**: TASK-109

**Description**: Implement SMS notification service.

**Acceptance Criteria**:
- [ ] SMS provider integration
- [ ] Template system
- [ ] Delivery tracking
- [ ] Unit tests passing

---

### TASK-1003: Notification Templates
**Priority**: P1 | **Effort**: M | **Status**: Not Started
**Dependencies**: TASK-1001

**Description**: Create all notification templates.

**Templates Needed**:
- Registration confirmation
- NBT number assignment
- Booking confirmation
- Payment received
- Test reminders (7 days, 1 day)
- Results available
- Profile changes
- Password reset

**Acceptance Criteria**:
- [ ] All templates created
- [ ] Placeholders working
- [ ] HTML and text versions
- [ ] Tested with real data

---

### TASK-1004: Special Session Workflow
**Priority**: P2 | **Effort**: L | **Status**: Not Started
**Dependencies**: TASK-109

**Description**: Implement special/remote session request workflow.

**Acceptance Criteria**:
- [ ] Request form created
- [ ] Routing to admin team
- [ ] Approval workflow
- [ ] Notifications sent
- [ ] E2E tests passing

---

### TASK-1005: Document Upload System
**Priority**: P1 | **Effort**: M | **Status**: Not Started
**Dependencies**: TASK-109

**Description**: Complete document upload and management system.

**Acceptance Criteria**:
- [ ] File validation
- [ ] Secure storage
- [ ] Virus scanning
- [ ] Document viewer
- [ ] Staff verification
- [ ] Unit tests passing

---

### TASK-1006: Profile Management Frontend
**Priority**: P1 | **Effort**: M | **Status**: Not Started
**Dependencies**: TASK-1005

**Description**: Build student profile management UI.

**Components**:
- StudentProfile.razor
- ProfileEditor.razor
- DocumentUpload.razor
- ChangePassword.razor
- ProfileAuditLog.razor

**Acceptance Criteria**:
- [ ] Profile viewing functional
- [ ] Editing works
- [ ] Document upload working
- [ ] Password change functional
- [ ] Audit trail visible
- [ ] E2E tests passing

---

## Phase 11: Testing & Quality Assurance

### TASK-1101: Unit Test - Domain Layer
**Priority**: P0 | **Effort**: L | **Status**: Not Started
**Dependencies**: TASK-108

**Description**: Write unit tests for domain layer.

**Acceptance Criteria**:
- [ ] All entities tested
- [ ] Value objects tested
- [ ] Business logic tested
- [ ] 80%+ code coverage

---

### TASK-1102: Unit Test - Application Layer
**Priority**: P0 | **Effort**: XL | **Status**: Not Started
**Dependencies**: All service tasks

**Description**: Write unit tests for all services.

**Acceptance Criteria**:
- [ ] All services tested
- [ ] Edge cases covered
- [ ] Mocking implemented
- [ ] 80%+ code coverage

---

### TASK-1103: Integration Tests - API
**Priority**: P0 | **Effort**: XL | **Status**: Not Started
**Dependencies**: All API tasks

**Description**: Write integration tests for all API endpoints.

**Acceptance Criteria**:
- [ ] All endpoints tested
- [ ] Authentication tested
- [ ] Authorization tested
- [ ] Error scenarios covered
- [ ] 100% endpoint coverage

---

### TASK-1104: End-to-End Tests
**Priority**: P1 | **Effort**: XL | **Status**: Not Started
**Dependencies**: All frontend tasks

**Description**: Write E2E tests for critical workflows.

**Workflows to Test**:
- Complete registration wizard
- Booking and payment flow
- Results viewing
- Staff CRUD operations
- Admin operations

**Acceptance Criteria**:
- [ ] All workflows automated
- [ ] Tests run in CI/CD
- [ ] Test reports generated
- [ ] Screenshots on failure

---

### TASK-1105: Performance Testing
**Priority**: P1 | **Effort**: L | **Status**: Not Started
**Dependencies**: TASK-1103

**Description**: Conduct performance and load testing.

**Acceptance Criteria**:
- [ ] 1000+ concurrent users tested
- [ ] Page load < 3 seconds validated
- [ ] API response < 500ms validated
- [ ] Bottlenecks identified and fixed
- [ ] Performance report created

---

### TASK-1106: Security Testing
**Priority**: P0 | **Effort**: L | **Status**: Not Started
**Dependencies**: TASK-1103

**Description**: Conduct security testing and vulnerability scanning.

**Acceptance Criteria**:
- [ ] OWASP ZAP scan completed
- [ ] Authentication vulnerabilities tested
- [ ] Authorization bypass tested
- [ ] SQL injection tested
- [ ] XSS tested
- [ ] All critical issues fixed
- [ ] Security report created

---

### TASK-1107: Accessibility Testing
**Priority**: P0 | **Effort**: M | **Status**: Not Started
**Dependencies**: All frontend tasks

**Description**: Validate WCAG 2.1 AA compliance.

**Acceptance Criteria**:
- [ ] Automated accessibility tests run
- [ ] Screen reader testing done
- [ ] Keyboard navigation tested
- [ ] Color contrast validated
- [ ] WCAG 2.1 AA compliant
- [ ] Accessibility report created

---

## Phase 12: CI/CD & Deployment

### TASK-1201: GitHub Actions - Build Pipeline
**Priority**: P0 | **Effort**: M | **Status**: Not Started
**Dependencies**: TASK-1103

**Description**: Configure automated build pipeline.

**Acceptance Criteria**:
- [ ] Build on every commit
- [ ] Tests run automatically
- [ ] Build artifacts created
- [ ] Notifications on failure

---

### TASK-1202: GitHub Actions - Deployment Pipeline
**Priority**: P0 | **Effort**: M | **Status**: Not Started
**Dependencies**: TASK-1201

**Description**: Configure automated deployment to staging and production.

**Acceptance Criteria**:
- [ ] Deploy to staging on merge to develop
- [ ] Deploy to production on merge to main
- [ ] Automated database migrations
- [ ] Rollback capability
- [ ] Approval gates for production

---

### TASK-1203: Branch Strategy Implementation
**Priority**: P0 | **Effort**: S | **Status**: Not Started
**Dependencies**: None

**Description**: Implement Git branching strategy.

**Branches**:
- main: Production
- develop: Integration
- feature/*: Features
- hotfix/*: Urgent fixes

**Acceptance Criteria**:
- [ ] Branch protection rules configured
- [ ] PR requirements set
- [ ] Code review required
- [ ] Tests must pass before merge

---

### TASK-1204: Environment Configuration
**Priority**: P0 | **Effort**: M | **Status**: Not Started
**Dependencies**: None

**Description**: Configure all environments.

**Environments**:
- Development (local)
- Staging (Azure)
- Production (Azure)

**Acceptance Criteria**:
- [ ] Connection strings configured
- [ ] Environment variables set
- [ ] Secrets secured (Azure Key Vault)
- [ ] Logging configured
- [ ] Monitoring configured

---

### TASK-1205: Database Migration Strategy
**Priority**: P0 | **Effort**: M | **Status**: Not Started
**Dependencies**: TASK-108

**Description**: Define and test database migration strategy.

**Acceptance Criteria**:
- [ ] All migrations reviewed
- [ ] Rollback scripts created
- [ ] Migration process documented
- [ ] Automated in CI/CD
- [ ] Tested on staging

---

### TASK-1206: Monitoring & Logging Setup
**Priority**: P0 | **Effort**: M | **Status**: Not Started
**Dependencies**: TASK-1204

**Description**: Configure Application Insights and logging.

**Acceptance Criteria**:
- [ ] Application Insights configured
- [ ] Log aggregation working
- [ ] Alerts configured
- [ ] Dashboards created
- [ ] Error tracking active

---

## Phase 13: User Acceptance Testing

### TASK-1301: UAT Planning
**Priority**: P0 | **Effort**: M | **Status**: Not Started
**Dependencies**: TASK-1107

**Description**: Plan and prepare for UAT.

**Acceptance Criteria**:
- [ ] Test scenarios documented
- [ ] Test data prepared
- [ ] Test users recruited
- [ ] UAT environment ready
- [ ] Schedule confirmed

---

### TASK-1302: UAT Execution
**Priority**: P0 | **Effort**: L | **Status**: Not Started
**Dependencies**: TASK-1301

**Description**: Conduct user acceptance testing.

**Acceptance Criteria**:
- [ ] All scenarios tested
- [ ] Feedback collected
- [ ] Issues documented
- [ ] Critical issues fixed
- [ ] Re-testing completed
- [ ] UAT sign-off obtained

---

### TASK-1303: Training Materials
**Priority**: P1 | **Effort**: L | **Status**: Not Started
**Dependencies**: TASK-1107

**Description**: Create user training materials.

**Materials**:
- User guides (Student, Staff, Admin)
- Video tutorials
- FAQ documents
- Quick reference guides

**Acceptance Criteria**:
- [ ] All materials created
- [ ] Videos recorded
- [ ] FAQs comprehensive
- [ ] Materials reviewed and approved

---

## Phase 14: Go-Live Preparation

### TASK-1401: Pre-Launch Checklist
**Priority**: P0 | **Effort**: M | **Status**: Not Started
**Dependencies**: TASK-1302

**Description**: Complete pre-launch checklist.

**Acceptance Criteria**:
- [ ] All features tested and approved
- [ ] Performance requirements met
- [ ] Security testing passed
- [ ] Accessibility compliance achieved
- [ ] UAT approved
- [ ] Training materials ready
- [ ] CI/CD operational
- [ ] Monitoring configured
- [ ] Backup strategy tested
- [ ] Support processes defined
- [ ] Go-live communication prepared

---

### TASK-1402: Data Migration (If Applicable)
**Priority**: P0 | **Effort**: L | **Status**: Not Started
**Dependencies**: TASK-1401

**Description**: Migrate existing data to new system.

**Acceptance Criteria**:
- [ ] Data extraction completed
- [ ] Data transformation done
- [ ] Data loaded successfully
- [ ] Data integrity verified
- [ ] No data loss
- [ ] Historical data accessible

---

### TASK-1403: Go-Live Execution
**Priority**: P0 | **Effort**: M | **Status**: Not Started
**Dependencies**: TASK-1402

**Description**: Execute production deployment.

**Acceptance Criteria**:
- [ ] Deployment successful
- [ ] All functions verified
- [ ] Users notified
- [ ] Support team ready
- [ ] Monitoring active

---

### TASK-1404: Post-Launch Support
**Priority**: P0 | **Effort**: L | **Status**: Not Started
**Dependencies**: TASK-1403

**Description**: Provide intensive post-launch support.

**Duration**: 2-4 weeks

**Acceptance Criteria**:
- [ ] System monitored closely
- [ ] Issues resolved promptly
- [ ] User feedback collected
- [ ] Minor adjustments made
- [ ] System stable
- [ ] Future enhancements planned

---

## Summary Statistics

**Total Tasks**: 140+
**Critical (P0) Tasks**: 85+
**High (P1) Tasks**: 35+
**Medium (P2) Tasks**: 15+
**Low (P3) Tasks**: 5+

**Estimated Timeline**: 12-16 weeks (realistic with buffer)

**Key Milestones**:
1. Week 2: Shell audit complete, .NET 9 upgraded
2. Week 4: Registration wizard operational
3. Week 6: Booking and payment functional
4. Week 8: All dashboards complete
5. Week 10: Results and reporting operational
6. Week 12: Testing complete
7. Week 14: UAT approved
8. Week 16: Production launch

---

## Notes
- Tasks are organized by phase for clarity
- Dependencies must be completed before starting dependent tasks
- Some tasks can be parallelized (e.g., different modules)
- Regular progress reviews recommended
- Buffer time built into estimates for unknowns
