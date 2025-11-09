# NBT Web Application - Complete Implementation Plan

## Document Control
- **Version**: 2.0
- **Last Updated**: 2025-11-09
- **Status**: ACTIVE
- **Related**: CONSTITUTION.md, COMPLETE-SPECIFICATION.md

---

## Executive Summary

This document outlines the complete implementation plan for the NBT Integrated Web Application, building upon the existing Blazor WebAssembly + ASP.NET Core Web API shell. The plan identifies missing components, defines the implementation strategy, and provides a roadmap for delivering a fully functional, production-ready system.

---

## 1. Current State Assessment

### 1.1 Existing Shell Components
The project already has:
- ✅ Blazor Web App project structure
- ✅ ASP.NET Core Web API backend
- ✅ Entity Framework Core setup
- ✅ Basic authentication infrastructure
- ✅ Database context and migrations
- ✅ Some domain models
- ✅ Basic navigation structure
- ✅ Some FluentUI components (partially migrated from MudBlazor)

### 1.2 Identified Gaps
Missing or incomplete:
- ❌ Complete FluentUI migration (MudBlazor references still exist)
- ❌ Registration wizard with multi-step flow
- ❌ NBT number generation with Luhn validation
- ❌ Booking eligibility checks and workflow
- ❌ EasyPay payment integration
- ❌ Installment payment tracking
- ❌ Bank payment file upload processing
- ❌ Result import functionality
- ❌ Barcode management for results
- ❌ Payment-based result visibility
- ❌ Venue type management (National, Special Session, etc.)
- ❌ Venue date availability tracking
- ❌ Test calendar with Sunday/Online highlighting
- ❌ Special/remote session management
- ❌ Background questionnaire component
- ❌ Resume registration capability
- ❌ Foreign ID support
- ❌ SA ID auto-extraction of DOB/Gender
- ❌ Staff dashboards with full CRUD
- ❌ Admin dashboards
- ❌ Report generation (Excel/PDF)
- ❌ Role-based dashboards
- ❌ Landing page with proper menus
- ❌ Video integration
- ❌ Complete audit logging
- ❌ Full test coverage

---

## 2. Implementation Strategy

### 2.1 Approach
- **Incremental Development**: Build and test features incrementally
- **Feature Branches**: Each major feature on separate branch
- **Continuous Integration**: Automated builds and tests
- **Review & Merge**: Code review before merging to main
- **Automated Deployment**: Staging deployment on merge

### 2.2 Technology Stack Confirmation
- **Frontend**: Blazor Web App (Interactive Auto)
- **UI Library**: Microsoft Fluent UI Blazor Components ONLY
- **Backend**: ASP.NET Core 9.0 Web API
- **Database**: MS SQL Server
- **ORM**: Entity Framework Core
- **Auth**: JWT with refresh tokens
- **Payments**: EasyPay API integration
- **Reports**: EPPlus (Excel), iText7 (PDF)
- **Testing**: xUnit, Moq, Playwright
- **CI/CD**: GitHub Actions

### 2.3 Development Phases

**Phase 0**: Foundation & Cleanup (1 week)
**Phase 1**: Registration & NBT Number (2 weeks)
**Phase 2**: Booking System (2 weeks)
**Phase 3**: Payment Integration (2 weeks)
**Phase 4**: Results Management (1 week)
**Phase 5**: Venue & Calendar (1 week)
**Phase 6**: Dashboards & Reports (2 weeks)
**Phase 7**: Landing Page & Content (1 week)
**Phase 8**: Testing & Deployment (2 weeks)

**Total Duration**: 14 weeks (3.5 months)

---

## 3. Phase Breakdown

### PHASE 0: Foundation & Cleanup

#### Objectives
- Complete MudBlazor to FluentUI migration
- Verify architecture integrity
- Set up complete CI/CD pipeline
- Establish coding standards

#### Tasks

**0.1 FluentUI Migration**
- [ ] Identify all remaining MudBlazor references
- [ ] Replace with FluentUI equivalents
- [ ] Update theme configuration
- [ ] Test all pages for UI consistency
- [ ] Remove MudBlazor NuGet packages

**0.2 Architecture Review**
- [ ] Review Clean Architecture implementation
- [ ] Verify dependency injection setup
- [ ] Validate entity relationships
- [ ] Check repository pattern implementation
- [ ] Ensure service layer separation

**0.3 CI/CD Setup**
- [ ] Create GitHub Actions workflow
- [ ] Configure build pipeline
- [ ] Set up automated testing
- [ ] Configure staging deployment
- [ ] Set up branch protection rules

**0.4 Code Standards**
- [ ] Configure EditorConfig
- [ ] Set up code analysis rules
- [ ] Create coding guidelines document
- [ ] Set up pre-commit hooks

**0.5 Database Review**
- [ ] Review existing migrations
- [ ] Verify entity configurations
- [ ] Add missing indexes
- [ ] Create seed data
- [ ] Document schema

**Deliverables**:
- Fully FluentUI-based UI
- CI/CD pipeline operational
- Clean architecture verified
- Standards documented

---

### PHASE 1: Registration & NBT Number

#### Objectives
- Implement complete registration wizard
- Add NBT number generation
- Support SA ID and Foreign ID
- Enable resume capability

#### Tasks

**1.1 Domain Models**
- [ ] Update Student entity with all fields
- [ ] Add Foreign ID and Passport fields
- [ ] Create Registration entity
- [ ] Create BackgroundQuestionnaire entity
- [ ] Add EF Core configurations

**1.2 NBT Number Generation**
- [ ] Implement Luhn algorithm service
- [ ] Create NBT number generator
- [ ] Add uniqueness validation
- [ ] Create unit tests
- [ ] Document algorithm

**1.3 SA ID Validation**
- [ ] Implement SA ID Luhn validation
- [ ] Add DOB extraction logic
- [ ] Add Gender extraction logic
- [ ] Create validation service
- [ ] Add unit tests

**1.4 Registration Wizard - Step 1 (Account & Personal)**
- [ ] Create FluentUI wizard component
- [ ] Build Step 1 form (combined account & personal)
- [ ] Add SA ID field with auto-extraction
- [ ] Add Foreign ID / Passport alternative
- [ ] Calculate Age from DOB
- [ ] Add Gender and Ethnicity fields
- [ ] Implement validation
- [ ] Add progress saving
- [ ] Test step navigation

**1.5 Registration Wizard - Step 2 (Academic & Test)**
- [ ] Create Step 2 form (combined academic & test)
- [ ] Add school/institution fields
- [ ] Add test type selection (AQL / AQL+MAT)
- [ ] Add language preference
- [ ] Add special accommodation fields
- [ ] Implement validation
- [ ] Add progress saving

**1.6 Registration Wizard - Step 3 (Venue & Booking)**
- [ ] Create Step 3 form
- [ ] Add venue type selector
- [ ] Add test date picker
- [ ] Add venue selector
- [ ] Implement validation
- [ ] Add progress saving

**1.7 Registration Wizard - Step 4 (Survey)**
- [ ] Create Step 4 form
- [ ] Build dynamic questionnaire component
- [ ] Add survey questions
- [ ] Implement JSON storage
- [ ] Add completion handling

**1.8 Wizard Completion**
- [ ] Generate NBT number on completion
- [ ] Send OTP for verification
- [ ] Create student account
- [ ] Redirect to dashboard
- [ ] Send welcome email

**1.9 Resume Capability**
- [ ] Store current step in database
- [ ] Detect incomplete registrations
- [ ] Redirect to correct step
- [ ] Load saved data
- [ ] Test resume flow

**1.10 API Endpoints**
- [ ] POST /api/v1/registration/start
- [ ] GET /api/v1/registration/{id}
- [ ] PUT /api/v1/registration/{id}/step
- [ ] POST /api/v1/registration/{id}/complete
- [ ] GET /api/v1/registration/resume/{studentId}
- [ ] POST /api/v1/registration/validate-id
- [ ] POST /api/v1/registration/generate-nbt-number

**1.11 Testing**
- [ ] Unit tests for services
- [ ] Integration tests for API
- [ ] E2E test for complete registration
- [ ] Test SA ID scenarios
- [ ] Test Foreign ID scenarios
- [ ] Test resume capability

**Deliverables**:
- Functional 4-step registration wizard
- NBT number generation working
- SA ID and Foreign ID support
- Resume capability implemented
- Full test coverage

---

### PHASE 2: Booking System

#### Objectives
- Implement test booking workflow
- Add eligibility checks
- Create test calendar
- Support booking modifications

#### Tasks

**2.1 Domain Models**
- [ ] Create/update Booking entity
- [ ] Create TestSession entity
- [ ] Create TestDate entity
- [ ] Add venue linkage
- [ ] Add EF Core configurations

**2.2 Booking Eligibility Service**
- [ ] Check for active bookings
- [ ] Check previous test closing dates
- [ ] Check annual limit (max 2 per year)
- [ ] Calculate test validity (3 years)
- [ ] Create unit tests

**2.3 Test Calendar Component**
- [ ] Create FluentUI calendar component
- [ ] Highlight Sunday tests (color-coded)
- [ ] Highlight Online tests (color-coded)
- [ ] Show available dates
- [ ] Show closing dates
- [ ] Filter by test type

**2.4 Booking Workflow**
- [ ] Create booking initiation page
- [ ] Add eligibility check UI
- [ ] Add test type selection
- [ ] Add test date selection from calendar
- [ ] Add venue selection
- [ ] Create booking confirmation page
- [ ] Implement booking logic
- [ ] Send confirmation email

**2.5 Booking Modification**
- [ ] Allow changes before closing date
- [ ] Create modification UI
- [ ] Validate modification eligibility
- [ ] Update booking records
- [ ] Send update notification

**2.6 Online Test Support**
- [ ] Add online test indicator
- [ ] Create online test requirements page
- [ ] Validate technical requirements
- [ ] Store online test preferences

**2.7 API Endpoints**
- [ ] GET /api/v1/booking/available-sessions
- [ ] POST /api/v1/booking/create
- [ ] GET /api/v1/booking/{id}
- [ ] PUT /api/v1/booking/{id}
- [ ] DELETE /api/v1/booking/{id}
- [ ] GET /api/v1/booking/student/{studentId}
- [ ] GET /api/v1/booking/check-eligibility/{studentId}

**2.8 Testing**
- [ ] Unit tests for eligibility logic
- [ ] Integration tests for booking API
- [ ] E2E test for booking flow
- [ ] Test eligibility scenarios
- [ ] Test modification scenarios

**Deliverables**:
- Functional booking system
- Eligibility checks working
- Test calendar with highlighting
- Booking modifications supported
- Full test coverage

---

### PHASE 3: Payment Integration

#### Objectives
- Integrate EasyPay payment gateway
- Support installment payments
- Handle bank payment uploads
- Track payment status

#### Tasks

**3.1 Domain Models**
- [ ] Create/update Payment entity
- [ ] Add installment tracking fields
- [ ] Add intake year cost tracking
- [ ] Add payment method types
- [ ] Add EF Core configurations

**3.2 Payment Calculation Service**
- [ ] Get test cost by intake year
- [ ] Calculate remaining balance
- [ ] Determine payment order (oldest first)
- [ ] Track installments
- [ ] Create unit tests

**3.3 EasyPay Integration**
- [ ] Create EasyPay service
- [ ] Generate payment references
- [ ] Build payment initiation endpoint
- [ ] Create webhook handler for confirmations
- [ ] Handle payment status updates
- [ ] Test with EasyPay sandbox

**3.4 Payment UI**
- [ ] Create payment initiation page
- [ ] Display test costs
- [ ] Show payment breakdown
- [ ] Display EasyPay reference
- [ ] Show payment instructions
- [ ] Create payment status page

**3.5 Installment Payment**
- [ ] Create installment plan UI
- [ ] Track partial payments
- [ ] Calculate remaining balance
- [ ] Update booking status
- [ ] Send payment reminders

**3.6 Bank Payment Upload**
- [ ] Define bank file format
- [ ] Create file upload component
- [ ] Build file parser
- [ ] Validate payment data
- [ ] Match payments to bookings
- [ ] Update payment records
- [ ] Generate processing report

**3.7 Payment History**
- [ ] Create payment history page
- [ ] Display all payments for student
- [ ] Show payment status
- [ ] Show remaining balance
- [ ] Download payment receipts

**3.8 API Endpoints**
- [ ] POST /api/v1/payments/initiate
- [ ] GET /api/v1/payments/{id}
- [ ] PUT /api/v1/payments/{id}/confirm
- [ ] GET /api/v1/payments/booking/{bookingId}
- [ ] POST /api/v1/payments/bank-upload
- [ ] GET /api/v1/payments/student/{studentId}
- [ ] GET /api/v1/payments/status/{easyPayReference}
- [ ] POST /api/v1/payments/webhook (EasyPay callback)

**3.9 Testing**
- [ ] Unit tests for payment calculations
- [ ] Integration tests for EasyPay
- [ ] Test installment scenarios
- [ ] Test bank upload processing
- [ ] E2E test for payment flow

**Deliverables**:
- EasyPay integration working
- Installment payments supported
- Bank upload processing functional
- Payment tracking complete
- Full test coverage

---

### PHASE 4: Results Management

#### Objectives
- Implement result import functionality
- Add barcode management
- Support payment-based visibility
- Enable PDF certificate downloads

#### Tasks

**4.1 Domain Models**
- [ ] Create/update TestResult entity
- [ ] Add barcode field
- [ ] Add performance level fields (AL, QL, MAT)
- [ ] Add visibility flag
- [ ] Add EF Core configurations

**4.2 Result Import Service**
- [ ] Define import file format (CSV/Excel)
- [ ] Create file parser
- [ ] Validate result data
- [ ] Match to students and bookings
- [ ] Assign unique barcodes
- [ ] Create unit tests

**4.3 Result Visibility Logic**
- [ ] Check payment status
- [ ] Set visibility flag
- [ ] Different rules for students vs staff/admin
- [ ] Create service with unit tests

**4.4 Result Import UI (Staff)**
- [ ] Create import page
- [ ] Add file upload component
- [ ] Show import preview
- [ ] Display validation errors
- [ ] Confirm import
- [ ] Show import results

**4.5 Result Display UI (Student)**
- [ ] Create results page
- [ ] Display AL, QL, MAT scores
- [ ] Show performance levels
- [ ] Display barcode
- [ ] Show test date
- [ ] Handle multiple tests (different barcodes)

**4.6 PDF Certificate Generation**
- [ ] Design certificate template
- [ ] Implement PDF generation (iText7)
- [ ] Include all result details
- [ ] Add barcode to certificate
- [ ] Create download endpoint
- [ ] Test PDF generation

**4.7 Result Notifications**
- [ ] Send email when results available
- [ ] Only notify for fully paid tests
- [ ] Include result summary
- [ ] Link to view results

**4.8 Staff Result Management**
- [ ] View all results (regardless of payment)
- [ ] Search by student, barcode
- [ ] Manual result entry (if needed)
- [ ] Edit results (with audit log)
- [ ] Reprocess visibility

**4.9 API Endpoints**
- [ ] GET /api/v1/results/student/{studentId}
- [ ] GET /api/v1/results/{id}
- [ ] POST /api/v1/results/import
- [ ] GET /api/v1/results/{id}/pdf
- [ ] GET /api/v1/results/barcode/{barcode}
- [ ] PUT /api/v1/results/{id}/visibility
- [ ] POST /api/v1/results (manual entry)
- [ ] PUT /api/v1/results/{id} (edit)

**4.10 Testing**
- [ ] Unit tests for import logic
- [ ] Unit tests for visibility logic
- [ ] Integration tests for result API
- [ ] Test PDF generation
- [ ] E2E test for result flow

**Deliverables**:
- Result import working
- Barcode management functional
- Payment-based visibility enforced
- PDF certificates generated
- Full test coverage

---

### PHASE 5: Venue & Calendar Management

#### Objectives
- Implement venue management
- Add venue type support
- Track venue date availability
- Link test sessions to venues

#### Tasks

**5.1 Domain Models**
- [ ] Update Venue entity with all fields
- [ ] Add VenueType enum (National, Special Session, Research, Other)
- [ ] Create VenueDateAvailability entity
- [ ] Update Room entity (information only)
- [ ] Update TestSession to link to Venue (not Room)
- [ ] Add EF Core configurations

**5.2 Venue Management (Admin/Staff)**
- [ ] Create venue list page
- [ ] Add venue create/edit form
- [ ] Include venue type selector
- [ ] Add address fields
- [ ] Set capacity
- [ ] Enable/disable venues
- [ ] Implement CRUD operations

**5.3 Date Availability Management**
- [ ] Create availability calendar UI
- [ ] Mark dates as available/unavailable
- [ ] Add reason for unavailability
- [ ] Bulk date updates
- [ ] Visual availability indicator

**5.4 Room Management**
- [ ] Create room list page (per venue)
- [ ] Add room create/edit form
- [ ] Set room capacity
- [ ] Note: Rooms for information only (sessions link to venues)

**5.5 Test Session Management**
- [ ] Create test session list
- [ ] Add session create/edit form
- [ ] Link to venue (not room)
- [ ] Set date, time, capacity
- [ ] Mark Sunday tests
- [ ] Mark online tests
- [ ] Track registered count

**5.6 Venue Selection (Student)**
- [ ] Display available venues for selected date
- [ ] Filter by venue type
- [ ] Show capacity status
- [ ] Show venue details
- [ ] Handle unavailable dates

**5.7 API Endpoints**
- [ ] GET /api/v1/venues
- [ ] GET /api/v1/venues/{id}
- [ ] POST /api/v1/venues
- [ ] PUT /api/v1/venues/{id}
- [ ] DELETE /api/v1/venues/{id}
- [ ] GET /api/v1/venues/available/{date}
- [ ] POST /api/v1/venues/{id}/availability
- [ ] GET /api/v1/test-sessions
- [ ] POST /api/v1/test-sessions
- [ ] PUT /api/v1/test-sessions/{id}

**5.8 Testing**
- [ ] Unit tests for venue services
- [ ] Integration tests for venue API
- [ ] Test availability logic
- [ ] Test session linkage

**Deliverables**:
- Venue management functional
- Venue types supported
- Date availability tracked
- Test sessions linked to venues
- Full test coverage

---

### PHASE 6: Dashboards & Reports

#### Objectives
- Build role-based dashboards
- Implement CRUD operations for staff/admin
- Create report generation
- Support Excel and PDF exports

#### Tasks

**6.1 Student Dashboard**
- [ ] Create dashboard layout with left menu
- [ ] Add summary widgets (bookings, payments, results)
- [ ] Show recent activity
- [ ] Add quick action buttons
- [ ] Create profile page
- [ ] Create bookings page
- [ ] Create payments page
- [ ] Create results page

**6.2 Staff Dashboard**
- [ ] Create dashboard layout with left menu
- [ ] Add summary widgets (registrations, payments, etc.)
- [ ] Show pending actions
- [ ] Create student management page with search/filter
- [ ] Create booking management page
- [ ] Create payment management page
- [ ] Create result management page
- [ ] Add quick actions

**6.3 Admin Dashboard**
- [ ] Create admin dashboard layout
- [ ] Add system overview widgets
- [ ] Create user management page
- [ ] Create configuration page
- [ ] Create audit log viewer
- [ ] Add system health indicators

**6.4 Special Session Management (Staff)**
- [ ] Create special session requests page
- [ ] Show pending approvals
- [ ] Add approval/rejection workflow
- [ ] Capture reviewer comments
- [ ] Send notifications

**6.5 Report Generation**
- [ ] Create report selection page
- [ ] Implement registration report
- [ ] Implement payment report
- [ ] Implement result report
- [ ] Implement venue utilization report
- [ ] Add date range filters
- [ ] Add export options

**6.6 Excel Export**
- [ ] Implement Excel generation (EPPlus)
- [ ] Format worksheets
- [ ] Add charts/summaries
- [ ] Test with large datasets

**6.7 PDF Export**
- [ ] Implement PDF generation (iText7)
- [ ] Design report templates
- [ ] Add charts/tables
- [ ] Test with large datasets

**6.8 CRUD Operations (Staff/Admin)**
- [ ] Students: View, Edit, Disable
- [ ] Bookings: View, Edit, Cancel
- [ ] Payments: View, Manual Entry, Adjust
- [ ] Results: View All, Edit, Import
- [ ] Venues: Full CRUD
- [ ] Test Sessions: Full CRUD
- [ ] Users: Full CRUD (Admin only)

**6.9 API Endpoints**
- [ ] GET /api/v1/staff/students
- [ ] PUT /api/v1/staff/students/{id}
- [ ] GET /api/v1/staff/bookings
- [ ] GET /api/v1/staff/payments
- [ ] POST /api/v1/staff/payments/manual-adjustment
- [ ] GET /api/v1/staff/special-sessions
- [ ] PUT /api/v1/staff/special-sessions/{id}/approve
- [ ] GET /api/v1/reports/registrations
- [ ] GET /api/v1/reports/payments
- [ ] POST /api/v1/reports/export/excel
- [ ] POST /api/v1/reports/export/pdf

**6.10 Testing**
- [ ] Test all dashboard pages
- [ ] Test CRUD operations
- [ ] Test report generation
- [ ] Test export functionality
- [ ] E2E tests for workflows

**Deliverables**:
- Role-based dashboards functional
- CRUD operations complete
- Reports generated successfully
- Excel and PDF exports working
- Full test coverage

---

### PHASE 7: Landing Page & Content

#### Objectives
- Create public landing page
- Implement menu structure matching current NBT website
- Add content pages
- Integrate videos

#### Tasks

**7.1 Landing Page Design**
- [ ] Design landing page layout
- [ ] Create hero section
- [ ] Add main navigation (Applicants, Institutions, Educators)
- [ ] Create footer
- [ ] Implement responsive design

**7.2 Applicants Menu**
- [ ] Research current NBT website structure
- [ ] Create submenus matching current site
- [ ] Create content pages:
  - [ ] About the NBT
  - [ ] Test information
  - [ ] How to register
  - [ ] How to prepare
  - [ ] Test centers
  - [ ] FAQs for applicants
- [ ] Add "Register" call-to-action
- [ ] Add "Login" link

**7.3 Institutions Menu**
- [ ] Research current NBT website structure
- [ ] Create submenus matching current site
- [ ] Create content pages:
  - [ ] About institutional use
  - [ ] How to register institution
  - [ ] Bulk booking information
  - [ ] Result access for institutions
  - [ ] Reports for institutions
  - [ ] FAQs for institutions
- [ ] Add institutional login

**7.4 Educators Menu**
- [ ] Research current NBT website structure
- [ ] Create submenus matching current site
- [ ] Create content pages:
  - [ ] Resources for educators
  - [ ] Test specifications
  - [ ] Teaching materials
  - [ ] Professional development
  - [ ] FAQs for educators
- [ ] Add resource downloads

**7.5 Static Pages**
- [ ] About Us
- [ ] Contact Us (with form)
- [ ] Privacy Policy
- [ ] Terms of Service
- [ ] Accessibility Statement
- [ ] Sitemap

**7.6 Video Integration**
- [ ] Identify videos from current NBT website
- [ ] Embed videos on relevant pages
- [ ] Add video player controls
- [ ] Ensure accessibility (captions)
- [ ] Test video loading and playback

**7.7 Content Management**
- [ ] Create CMS for content editing (optional)
- [ ] Or use markdown files for content
- [ ] Version control for content
- [ ] Content review workflow

**7.8 SEO & Analytics**
- [ ] Add meta tags
- [ ] Create robots.txt and sitemap.xml
- [ ] Integrate Google Analytics
- [ ] Test SEO optimization

**7.9 Testing**
- [ ] Test all navigation
- [ ] Test responsive design
- [ ] Test video playback
- [ ] Test forms
- [ ] Accessibility testing

**Deliverables**:
- Professional landing page
- Complete menu structure
- All content pages created
- Videos integrated
- SEO optimized

---

### PHASE 8: Testing & Deployment

#### Objectives
- Comprehensive testing
- Performance optimization
- Security hardening
- Production deployment

#### Tasks

**8.1 Unit Testing**
- [ ] Ensure >80% code coverage
- [ ] Test all services
- [ ] Test all validation logic
- [ ] Test all calculations
- [ ] Mock external dependencies

**8.2 Integration Testing**
- [ ] Test all API endpoints
- [ ] Test database interactions
- [ ] Test EasyPay integration (sandbox)
- [ ] Test file uploads
- [ ] Test PDF generation

**8.3 End-to-End Testing**
- [ ] Test registration flow
- [ ] Test booking flow
- [ ] Test payment flow
- [ ] Test result access flow
- [ ] Test admin workflows
- [ ] Test on multiple browsers
- [ ] Test on mobile devices

**8.4 Performance Testing**
- [ ] Load testing with JMeter/k6
- [ ] Stress testing
- [ ] Identify bottlenecks
- [ ] Optimize queries
- [ ] Add caching where needed
- [ ] Achieve <3s page load

**8.5 Security Testing**
- [ ] Penetration testing
- [ ] Vulnerability scanning
- [ ] Test authentication/authorization
- [ ] Test input validation
- [ ] Test SQL injection prevention
- [ ] Test XSS prevention
- [ ] Review HTTPS configuration

**8.6 Accessibility Testing**
- [ ] Automated accessibility testing
- [ ] Manual keyboard navigation
- [ ] Screen reader testing
- [ ] Color contrast verification
- [ ] WCAG 2.1 AA compliance check

**8.7 User Acceptance Testing**
- [ ] Create test scenarios
- [ ] Recruit test users
- [ ] Conduct UAT sessions
- [ ] Collect feedback
- [ ] Fix identified issues
- [ ] Re-test

**8.8 Documentation**
- [ ] Complete API documentation
- [ ] Write deployment guide
- [ ] Create user manuals
- [ ] Record video tutorials
- [ ] Update README files

**8.9 Production Preparation**
- [ ] Set up production environment (Azure)
- [ ] Configure Azure SQL Database
- [ ] Set up Azure App Service
- [ ] Configure Azure Key Vault for secrets
- [ ] Set up CDN for static assets
- [ ] Configure Application Insights
- [ ] Set up backup and disaster recovery

**8.10 Deployment**
- [ ] Deploy to production
- [ ] Run smoke tests
- [ ] Monitor for errors
- [ ] Verify all functionality
- [ ] Update DNS (if needed)
- [ ] Announce go-live

**8.11 Post-Deployment**
- [ ] Monitor application health
- [ ] Monitor performance
- [ ] Collect user feedback
- [ ] Fix any production issues
- [ ] Plan for future enhancements

**Deliverables**:
- Fully tested application
- Performance optimized
- Security hardened
- Deployed to production
- Complete documentation

---

## 4. Project Structure

### 4.1 Solution Organization
```
NBTWebApp/
├── src/
│   ├── NBTWebApp.Client/              # Blazor WebAssembly client
│   │   ├── Pages/
│   │   │   ├── Registration/
│   │   │   │   ├── RegistrationWizard.razor
│   │   │   │   ├── Step1AccountPersonal.razor
│   │   │   │   ├── Step2AcademicTest.razor
│   │   │   │   ├── Step3VenueBooking.razor
│   │   │   │   └── Step4Survey.razor
│   │   │   ├── Booking/
│   │   │   ├── Payment/
│   │   │   ├── Results/
│   │   │   └── Dashboard/
│   │   ├── Shared/
│   │   │   ├── FluentComponents/
│   │   │   └── Layouts/
│   │   └── Services/
│   ├── NBTWebApp/                      # Blazor Server host & API
│   │   ├── Controllers/
│   │   │   ├── RegistrationController.cs
│   │   │   ├── BookingController.cs
│   │   │   ├── PaymentController.cs
│   │   │   ├── ResultsController.cs
│   │   │   ├── VenueController.cs
│   │   │   └── ReportsController.cs
│   │   ├── Components/
│   │   └── Program.cs
│   ├── NBTWebApp.Core/                 # Domain models & interfaces
│   │   ├── Entities/
│   │   │   ├── Student.cs
│   │   │   ├── Registration.cs
│   │   │   ├── Booking.cs
│   │   │   ├── Payment.cs
│   │   │   ├── TestResult.cs
│   │   │   ├── Venue.cs
│   │   │   ├── Room.cs
│   │   │   └── TestSession.cs
│   │   ├── Interfaces/
│   │   │   ├── IStudentRepository.cs
│   │   │   ├── IRegistrationService.cs
│   │   │   ├── IBookingService.cs
│   │   │   ├── IPaymentService.cs
│   │   │   └── IResultService.cs
│   │   ├── DTOs/
│   │   └── Enums/
│   ├── NBTWebApp.Infrastructure/       # Data access & external services
│   │   ├── Data/
│   │   │   ├── ApplicationDbContext.cs
│   │   │   └── Migrations/
│   │   ├── Repositories/
│   │   ├── Services/
│   │   │   ├── NBTNumberGenerator.cs
│   │   │   ├── LuhnValidator.cs
│   │   │   ├── SAIdValidator.cs
│   │   │   ├── EasyPayService.cs
│   │   │   ├── ResultImportService.cs
│   │   │   └── PdfGenerationService.cs
│   │   └── ExternalServices/
│   └── NBTWebApp.Tests/                # Test projects
│       ├── UnitTests/
│       ├── IntegrationTests/
│       └── E2ETests/
├── database-scripts/
├── specs/
│   ├── CONSTITUTION.md
│   ├── COMPLETE-SPECIFICATION.md
│   └── IMPLEMENTATION-PLAN-COMPLETE.md
└── .github/
    └── workflows/
        ├── build-test.yml
        └── deploy.yml
```

---

## 5. Technology Decisions

### 5.1 Frontend
- **Blazor Web App** with Interactive Auto render mode
- **Fluent UI Blazor** for all UI components
- **No MudBlazor** - complete migration required

### 5.2 Backend
- **ASP.NET Core 9.0** Web API
- **Clean Architecture** pattern
- **Repository Pattern** for data access
- **Service Layer** for business logic

### 5.3 Database
- **SQL Server** (Azure SQL in production)
- **Entity Framework Core** for ORM
- **Code-First** migrations

### 5.4 Authentication
- **JWT tokens** (access + refresh)
- **ASP.NET Core Identity** for user management
- **Role-based** authorization

### 5.5 External Services
- **EasyPay** for payment processing
- **SendGrid** or **SMTP** for emails
- **Twilio** for SMS (optional)

### 5.6 Reporting
- **EPPlus** for Excel generation
- **iText7** for PDF generation

### 5.7 Testing
- **xUnit** for unit and integration tests
- **Moq** for mocking
- **Playwright** for E2E tests

### 5.8 CI/CD
- **GitHub Actions** for automation
- **Azure App Service** for hosting
- **Azure SQL Database** for data

---

## 6. Quality Assurance

### 6.1 Code Quality
- **Code reviews** required for all PRs
- **Unit test coverage** >80%
- **Static code analysis** with SonarQube or similar
- **Consistent coding style** enforced via EditorConfig

### 6.2 Testing Strategy
- **Test-Driven Development** where appropriate
- **Integration tests** for all API endpoints
- **E2E tests** for critical workflows
- **Performance tests** before production

### 6.3 Documentation
- **XML documentation** for all public APIs
- **README** files in each project
- **Architecture Decision Records** (ADRs)
- **User documentation** and tutorials

---

## 7. Risk Management

### 7.1 Identified Risks
1. **EasyPay Integration Complexity**: Mitigate with early prototype and sandbox testing
2. **Performance Issues**: Mitigate with load testing and optimization sprints
3. **Security Vulnerabilities**: Mitigate with security reviews and penetration testing
4. **Scope Creep**: Mitigate with strict change control and phased approach
5. **Third-Party Dependencies**: Mitigate with fallback strategies and monitoring

### 7.2 Mitigation Strategies
- **Early prototyping** of complex features
- **Regular security reviews**
- **Performance benchmarks** in each phase
- **Automated testing** to catch regressions
- **Continuous integration** to detect issues early

---

## 8. Success Metrics

### 8.1 Technical Metrics
- ✅ >80% unit test coverage
- ✅ <3 seconds page load time
- ✅ <500ms API response time (95th percentile)
- ✅ Zero critical security vulnerabilities
- ✅ WCAG 2.1 AA compliance

### 8.2 Business Metrics
- ✅ 100% feature completion per specification
- ✅ All critical workflows operational
- ✅ UAT sign-off achieved
- ✅ Production deployment successful
- ✅ Zero data loss or corruption

### 8.3 User Satisfaction
- ✅ Positive UAT feedback
- ✅ Intuitive user interface
- ✅ Responsive and fast
- ✅ Accessible to all users

---

## 9. Timeline Summary

| Phase | Duration | Deliverables |
|-------|----------|--------------|
| Phase 0 | 1 week | FluentUI migration, CI/CD setup |
| Phase 1 | 2 weeks | Registration wizard, NBT number |
| Phase 2 | 2 weeks | Booking system, test calendar |
| Phase 3 | 2 weeks | Payment integration, installments |
| Phase 4 | 1 week | Result management, PDF certificates |
| Phase 5 | 1 week | Venue management, date availability |
| Phase 6 | 2 weeks | Dashboards, reports, CRUD operations |
| Phase 7 | 1 week | Landing page, content pages |
| Phase 8 | 2 weeks | Testing, optimization, deployment |
| **Total** | **14 weeks** | **Fully functional production system** |

---

## 10. Next Steps

### 10.1 Immediate Actions
1. ✅ Review and approve this implementation plan
2. ⏳ Set up development environment for all team members
3. ⏳ Configure GitHub repository with branch protection
4. ⏳ Start Phase 0: FluentUI migration and CI/CD setup

### 10.2 Team Setup
- Assign developers to specific phases
- Set up communication channels
- Schedule daily standups
- Plan sprint retrospectives

### 10.3 Stakeholder Communication
- Share plan with stakeholders
- Schedule regular demo sessions
- Establish feedback channels
- Plan UAT sessions

---

## Document Approval

**Prepared By**: Development Team  
**Reviewed By**: Technical Lead  
**Approved By**: Project Manager  
**Date**: 2025-11-09  
**Status**: APPROVED - Ready for Implementation

---

**END OF DOCUMENT**
