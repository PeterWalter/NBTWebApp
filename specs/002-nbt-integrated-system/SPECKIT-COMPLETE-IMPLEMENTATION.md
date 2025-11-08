# SpecKit Complete Implementation Guide
## NBT Integrated Web Application

**Version:** 1.0  
**Date:** 2025-11-08  
**Status:** Implementation Ready

---

## Executive Summary

This document consolidates all SpecKit commands and requirements for the National Benchmark Tests (NBT) Integrated Web Application. It provides a complete specification framework including:

1. **Constitution** - Non-negotiable architectural and quality standards
2. **Specification** - Detailed functional requirements and workflows
3. **Plan** - Implementation roadmap and task breakdown
4. **Contracts** - API schemas and data models
5. **Tasks** - Granular task list with dependencies
6. **Review** - Audit checklist and validation criteria
7. **Quickstart** - Developer onboarding guide
8. **Implementation** - Execution strategy

---

## 1. Constitution (/speckit.constitution)

### Purpose
Define the immutable architectural, security, and quality standards that govern the NBT Integrated Web Application.

### Key Principles

#### Technology Stack (Non-Negotiable)
- **Frontend**: Blazor Web App Interactive Auto (.NET 8+)
- **UI Library**: Microsoft Fluent UI Blazor Components
- **Backend**: ASP.NET Core Web API (.NET 8+)
- **Database**: MS SQL Server 2019+
- **ORM**: Entity Framework Core 8+
- **Architecture**: Clean Architecture (4 layers)

#### Critical Business Rules
1. **NBT Number**: 14-digit Luhn-validated unique identifier
2. **ID Support**: SA ID, Foreign ID, and Passport ID registration
3. **Booking Rules**:
   - One active booking per student
   - Maximum 2 tests per year
   - 3-year test validity from booking date
   - Bookings open April 1 annually
   - Can modify before close of booking date
4. **Test Session**: Linked to Venue (NOT Room)
5. **Room Allocation**: Separate entity linking Student to Room for a Session

#### Security Requirements (Mandatory)
- HTTPS only (TLS 1.2 minimum)
- JWT authentication with refresh tokens
- Role-based access: Staff, Admin, SuperUser
- Luhn validation for NBT numbers
- SA ID validation with Luhn checksum
- Foreign ID/Passport validation
- Comprehensive audit logging
- POPIA compliance

#### Accessibility Standards
- WCAG 2.1 Level AA compliance
- Keyboard navigation for all interactive elements
- ARIA labels on all form controls
- 4.5:1 color contrast ratio

#### Performance Standards
- Initial page load < 3 seconds
- API response time < 500ms (95th percentile)
- Database queries < 100ms (95th percentile)
- All list operations paginated

#### Quality Standards
- Unit test coverage minimum 80%
- Integration tests for all API endpoints
- No lazy loading (explicit loading only)
- All queries use AsNoTracking() for read-only operations

**Reference**: See `constitution.md` for full details

---

## 2. Specification (/speckit.specify)

### Functional Areas

#### 2.1 Student Registration Wizard
**Multi-step workflow with automatic progress saving**

**Steps:**
1. **Personal Information**
   - ID Type selection (SA_ID | FOREIGN_ID | PASSPORT)
   - ID Number with validation
   - Full name, date of birth, contact details
   - Duplicate prevention

2. **NBT Number Generation**
   - Automatic 14-digit number generation
   - Luhn algorithm validation
   - Unique identifier assignment

3. **Academic Background**
   - School details
   - Grade/Year of study
   - Previous test history (if applicable)

4. **Test Preferences**
   - Test type selection (AQL only or AQL + MAT)
   - Preferred test date range
   - Venue selection from available options

5. **Special Accommodations**
   - Disability accommodations
   - Remote writer requests
   - Special session requirements

6. **Confirmation & Payment**
   - Summary review
   - EasyPay payment initiation
   - Email/SMS confirmation

**Business Rules:**
- Save draft after each step
- Allow backward navigation with data preservation
- Validate at each step (client + server)
- Prevent duplicate registrations
- Generate NBT number only once per student

#### 2.2 Test Booking System

**Features:**
- View available test sessions with venue and capacity
- Real-time capacity checking
- Booking validation (one active booking, 2 per year limit)
- Booking modification before close date
- Payment integration with EasyPay
- Automated booking confirmations

**Business Rules:**
- Cannot book if active booking exists
- Cannot book if 2 tests already in current year
- Cannot book if session is full
- Cannot modify after close date
- Test remains valid for 3 years from booking date

#### 2.3 Payment Integration (EasyPay)

**Workflow:**
1. System generates unique payment reference
2. Payment request sent to EasyPay API
3. User redirected to EasyPay portal
4. EasyPay webhook notifies system of payment status
5. System updates booking and sends confirmation

**Payment States:**
- Pending
- Processing
- Paid
- Failed
- Cancelled
- Refunded

**Technical Requirements:**
- Idempotent webhook processing
- Signature verification
- 3 retry attempts with exponential backoff
- 30-minute payment timeout

#### 2.4 Special Sessions & Remote Writers

**Features:**
- Special session request form
- Invigilator details capture
- Remote venue information
- Automatic routing to NBT remote administration team
- Approval workflow
- Document upload for supporting materials

**Business Rules:**
- Requires additional verification
- Separate approval process
- Invigilator credentials mandatory
- Remote venue must meet NBT standards

#### 2.5 Pre-Test Questionnaire

**Purpose:**
- Collect background information for research
- Support equity reporting
- Inform NBT operational insights

**Requirements:**
- Mandatory before test participation
- Anonymous data aggregation
- Secure data storage
- POPIA compliance

#### 2.6 Staff/Admin Dashboards

**Staff Role (Read-Only):**
- View student registrations
- View payment status
- View test results
- Generate reports

**Admin Role (Full CRUD):**
- Manage students (create, update, delete)
- Manage registrations
- Manage payments and refunds
- Manage test sessions
- Manage venues and rooms
- Import test results
- Generate and export reports

**SuperUser Role (System Configuration):**
- All Admin privileges
- User management
- Role assignments
- System settings
- Bulk data imports
- Full audit log access
- Venue and room configuration

#### 2.7 Venue & Room Management

**Venue Management:**
- Create/update/delete venues
- Set total venue capacity
- Assign address and contact details
- Track venue status (Active/Inactive)

**Room Management:**
- Add rooms to venues
- Set room capacity
- Room numbering/naming
- Track room status

**Test Session Management:**
- Create sessions linked to venues
- Set session date/time
- Define test type (AQL, MAT, Both)
- Session capacity = sum of room capacities
- Track available seats in real-time

**Room Allocation:**
- Assign students to specific rooms
- Manage seating arrangements
- Generate seating charts
- Track room utilization

#### 2.8 Test Results Management

**Excel Import:**
- Upload .xlsx or .xls files
- Validate NBT numbers (Luhn check)
- Validate scores (0-100 range)
- Duplicate detection
- Transactional import (all-or-nothing)
- Detailed error reporting

**Results Access:**
- Students view their results securely
- Performance band display
- Percentile ranking
- Downloadable result certificates
- 3-year result retention

**Admin Functions:**
- View all results
- Update results (with audit trail)
- Delete results (SuperUser only)
- Generate result reports

#### 2.9 Reporting & Analytics

**Report Types:**
1. **Registration Reports**
   - Total registrations by period
   - Registration status breakdown
   - Payment status tracking
   - Venue distribution

2. **Payment Reports**
   - Revenue by period
   - Payment method breakdown
   - Outstanding payments
   - Refund tracking

3. **Test Results Reports**
   - Performance statistics
   - Pass/fail rates
   - Score distributions
   - Comparative analytics

4. **Venue Utilization Reports**
   - Capacity usage by venue
   - Room allocation efficiency
   - Session attendance rates
   - Venue performance metrics

**Export Formats:**
- Excel (.xlsx) for data analysis
- PDF for formal reports
- CSV for data portability

**Performance Requirements:**
- Report generation < 10 seconds
- Excel export < 7 seconds (1000 records)
- PDF export < 5 seconds (1000 records)

#### 2.10 Notifications System

**Email Notifications:**
- Registration confirmation
- Payment confirmation
- Booking confirmation
- Test reminder (7 days before)
- Test reminder (1 day before)
- Results availability
- Account security events

**SMS Notifications (Optional):**
- Critical reminders
- Payment confirmations
- Test day reminders

**Technical Requirements:**
- Asynchronous sending
- Retry mechanism
- Template-based content
- Personalization tokens
- Delivery tracking

#### 2.11 Profile Management

**Student Features:**
- Update personal information
- Change password
- Upload documents
- View booking history
- Access test results
- Download certificates

**Audit Trail:**
- All profile changes logged
- Timestamp and user tracking
- Before/after value capture
- IP address logging

---

## 3. Plan (/speckit.plan)

### Implementation Phases

#### Phase 1: Shell Audit & Foundation (Week 1)
**Objective**: Validate existing project structure and identify gaps

**Tasks:**
1. Audit all 5 projects (Domain, Application, Infrastructure, WebAPI, WebUI)
2. Verify entity relationships and configurations
3. Check service registrations and DI setup
4. Review existing migrations
5. Identify missing components
6. Document gaps and create task backlog

**Deliverables:**
- Gap analysis report
- Updated task list
- Architecture validation

#### Phase 2: Domain Layer Completion (Week 2)
**Objective**: Complete all domain entities, value objects, and validators

**Tasks:**
1. Verify all 15 entities exist:
   - ✅ Student (with IDType enum support)
   - ✅ Registration
   - ✅ Payment
   - ✅ TestSession
   - ✅ Venue
   - ✅ Room
   - ✅ RoomAllocation
   - ✅ TestResult
   - SpecialSession
   - ✅ AuditLog

2. Implement validators:
   - ✅ LuhnValidator (NBT number)
   - ✅ South African ID Validator
   - ✅ Foreign ID Validator
   - Booking business rules validator
   - Capacity validator

3. Create value objects:
   - NBTNumber
   - IDNumber
   - EmailAddress
   - PhoneNumber

**Deliverables:**
- All domain entities complete
- All validators implemented and tested
- Value objects created
- Domain layer tests passing

#### Phase 3: Application Layer Implementation (Week 3-4)
**Objective**: Implement all business logic services and DTOs

**Tasks:**
1. Create service interfaces:
   - IStudentService
   - IRegistrationService
   - IBookingService
   - IPaymentService
   - IVenueService
   - IRoomService
   - IResultsImportService
   - IReportService
   - INotificationService

2. Create DTOs for all entities
3. Implement AutoMapper profiles
4. Create FluentValidation validators for DTOs
5. Implement CQRS commands and queries (if using MediatR)

**Deliverables:**
- All service interfaces defined
- All DTOs created
- AutoMapper profiles configured
- FluentValidation validators complete
- Unit tests for all services

#### Phase 4: Infrastructure Layer Implementation (Week 5-6)
**Objective**: Complete data access, external integrations, and repositories

**Tasks:**
1. Entity Framework configurations:
   - ✅ Student configuration
   - ✅ Registration configuration
   - ✅ Payment configuration
   - ✅ TestSession configuration
   - ✅ Venue configuration
   - ✅ Room configuration
   - ✅ RoomAllocation configuration
   - ✅ TestResult configuration
   - SpecialSession configuration
   - ✅ AuditLog configuration

2. Repository implementations:
   - Generic repository pattern
   - Specialized repositories for complex queries

3. External service implementations:
   - ✅ NBT Number Generator
   - EasyPay API integration (or mock)
   - Email service (SendGrid/SMTP)
   - SMS service (optional)
   - Excel import/export service
   - PDF generation service

4. Database migrations:
   - ✅ Core entities migration
   - ✅ Student ID type support migration
   - SpecialSession migration
   - Indexes and constraints migration

**Deliverables:**
- All EF configurations complete
- All repositories implemented
- External integrations ready
- Migrations applied successfully
- Integration tests passing

#### Phase 5: Web API Implementation (Week 7-8)
**Objective**: Implement all API endpoints with proper authorization

**API Controllers:**
1. RegistrationsController
   - POST /api/registration/start
   - POST /api/registration/generate-nbt-number
   - POST /api/registration/submit
   - GET /api/registration/{id}
   - GET /api/registration (paginated)
   - PUT /api/registration/{id}
   - DELETE /api/registration/{id}

2. BookingsController
   - GET /api/booking/available-sessions
   - POST /api/booking/book
   - PUT /api/booking/{id}
   - POST /api/booking/{id}/cancel
   - GET /api/booking/my-bookings

3. PaymentsController
   - POST /api/payment/initiate
   - POST /api/payment/webhook
   - GET /api/payment/{id}/status
   - PUT /api/payment/{id}/status

4. VenuesController
   - GET /api/venue
   - POST /api/venue
   - GET /api/venue/{id}
   - PUT /api/venue/{id}
   - DELETE /api/venue/{id}
   - GET /api/venue/{id}/rooms
   - POST /api/venue/{id}/rooms
   - GET /api/venue/{id}/capacity

5. ResultsController
   - POST /api/results/import
   - GET /api/results/{nbtNumber}
   - GET /api/results (paginated)
   - PUT /api/results/{id}
   - DELETE /api/results/{id}

6. ReportsController
   - GET /api/reports/registrations
   - GET /api/reports/registrations/export
   - GET /api/reports/payments
   - GET /api/reports/results
   - GET /api/reports/venue-utilization

**Deliverables:**
- All API endpoints implemented
- Authorization attributes applied
- Swagger documentation complete
- API integration tests passing

#### Phase 6: Blazor Web UI Implementation (Week 9-11)
**Objective**: Build all user-facing pages and admin dashboards

**Components:**

1. **Public Pages:**
   - /register (Registration Wizard)
   - /booking (Test Booking)
   - /payment/confirm (Payment Confirmation)
   - /my-bookings (User Bookings)
   - /my-results (User Results)
   - /profile (Profile Management)

2. **Admin Pages:**
   - /admin/dashboard (Overview)
   - /admin/registrations (CRUD)
   - /admin/students (CRUD)
   - /admin/payments (Management)
   - /admin/venues (CRUD)
   - /admin/rooms (CRUD)
   - /admin/sessions (CRUD)
   - /admin/results-import (Import UI)
   - /admin/reports (Report Generation)

3. **Shared Components:**
   - RegistrationWizard
   - PagedDataGrid
   - ExportButton
   - ImportDialog
   - ConfirmationDialog
   - ValidationSummary
   - StatusBadge

**Deliverables:**
- All pages implemented
- All components created
- Fluent UI theming applied
- Navigation working
- UI tests passing

#### Phase 7: Security & Authentication (Week 12)
**Objective**: Complete authentication, authorization, and audit logging

**Tasks:**
1. JWT authentication setup
2. Refresh token mechanism
3. Role-based authorization
4. Audit logging middleware
5. HTTPS enforcement
6. HSTS configuration
7. Security headers
8. CORS configuration

**Deliverables:**
- Authentication working
- Authorization enforced
- Audit logging active
- Security tests passing

#### Phase 8: Testing & Quality Assurance (Week 13-14)
**Objective**: Achieve comprehensive test coverage and quality validation

**Test Types:**
1. Unit Tests (80%+ coverage)
2. Integration Tests (100% endpoint coverage)
3. UI Tests (critical workflows)
4. Performance Tests
5. Accessibility Tests (WCAG 2.1 AA)
6. Security Tests

**Deliverables:**
- All tests passing
- Coverage reports
- Performance benchmarks
- Accessibility audit
- Security scan results

#### Phase 9: CI/CD & Deployment (Week 15)
**Objective**: Setup automated build, test, and deployment pipelines

**Tasks:**
1. GitHub Actions workflow
2. Automated testing
3. Code quality gates
4. Docker containerization
5. Azure App Service deployment
6. Database migration automation
7. Secrets management (Azure Key Vault)
8. Monitoring and logging setup

**Deliverables:**
- CI/CD pipeline operational
- Staging environment deployed
- Production deployment ready
- Monitoring dashboards active

#### Phase 10: Documentation & Handover (Week 16)
**Objective**: Complete all documentation and developer handover

**Tasks:**
1. Update README
2. API documentation (Swagger)
3. Architecture diagrams
4. Database schema documentation
5. Deployment guide
6. User manual
7. Admin manual
8. Developer onboarding guide

**Deliverables:**
- All documentation complete
- Knowledge transfer sessions
- Production deployment
- Project handover

---

## 4. Contracts (/speckit.contracts)

### Data Models

#### Student Entity
```csharp
public class Student : BaseEntity
{
    public Guid Id { get; set; }
    public string NBTNumber { get; set; } // 14-digit Luhn validated
    public IDType IDType { get; set; } // SA_ID | FOREIGN_ID | PASSPORT
    public string IDNumber { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string? Nationality { get; set; } // Required for FOREIGN_ID/PASSPORT
    public string? CountryOfOrigin { get; set; } // Required for FOREIGN_ID/PASSPORT
    
    // Navigation properties
    public ICollection<Registration> Registrations { get; set; }
    public ICollection<RoomAllocation> RoomAllocations { get; set; }
}
```

#### Registration Entity
```csharp
public class Registration : BaseEntity
{
    public Guid Id { get; set; }
    public Guid StudentId { get; set; }
    public Guid TestSessionId { get; set; }
    public RegistrationStatus Status { get; set; }
    public DateTime RegistrationDate { get; set; }
    public DateTime? CloseDate { get; set; }
    public TestType TestType { get; set; } // AQL | MAT | Both
    public bool RequiresSpecialAccommodation { get; set; }
    public string? SpecialAccommodationDetails { get; set; }
    
    // Navigation properties
    public Student Student { get; set; }
    public TestSession TestSession { get; set; }
    public Payment Payment { get; set; }
    public TestResult? TestResult { get; set; }
}
```

#### Payment Entity
```csharp
public class Payment : BaseEntity
{
    public Guid Id { get; set; }
    public Guid RegistrationId { get; set; }
    public string EasyPayReference { get; set; }
    public decimal Amount { get; set; }
    public PaymentStatus Status { get; set; }
    public DateTime PaymentDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    public string? TransactionId { get; set; }
    
    // Navigation property
    public Registration Registration { get; set; }
}
```

#### TestSession Entity
```csharp
public class TestSession : BaseEntity
{
    public Guid Id { get; set; }
    public Guid VenueId { get; set; } // CRITICAL: Linked to Venue, NOT Room
    public DateTime TestDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public TestType TestType { get; set; }
    public SessionStatus Status { get; set; }
    public int Capacity { get; set; } // Sum of room capacities
    public DateTime CloseDate { get; set; }
    
    // Navigation properties
    public Venue Venue { get; set; }
    public ICollection<Registration> Registrations { get; set; }
    public ICollection<RoomAllocation> RoomAllocations { get; set; }
}
```

#### Venue Entity
```csharp
public class Venue : BaseEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string Province { get; set; }
    public string PostalCode { get; set; }
    public int TotalCapacity { get; set; }
    public bool IsActive { get; set; }
    public string ContactPerson { get; set; }
    public string ContactEmail { get; set; }
    public string ContactPhone { get; set; }
    
    // Navigation properties
    public ICollection<Room> Rooms { get; set; }
    public ICollection<TestSession> TestSessions { get; set; }
}
```

#### Room Entity
```csharp
public class Room : BaseEntity
{
    public Guid Id { get; set; }
    public Guid VenueId { get; set; }
    public string RoomNumber { get; set; }
    public int Capacity { get; set; }
    public bool IsActive { get; set; }
    
    // Navigation properties
    public Venue Venue { get; set; }
    public ICollection<RoomAllocation> RoomAllocations { get; set; }
}
```

#### RoomAllocation Entity
```csharp
public class RoomAllocation : BaseEntity
{
    public Guid Id { get; set; }
    public Guid TestSessionId { get; set; }
    public Guid StudentId { get; set; }
    public Guid? RoomId { get; set; } // Nullable - assigned by admin after booking
    public int? SeatNumber { get; set; }
    public DateTime AllocationDate { get; set; }
    
    // Navigation properties
    public TestSession TestSession { get; set; }
    public Student Student { get; set; }
    public Room? Room { get; set; }
}
```

#### TestResult Entity
```csharp
public class TestResult : BaseEntity
{
    public Guid Id { get; set; }
    public Guid RegistrationId { get; set; }
    public int? AQLScore { get; set; }
    public int? MATScore { get; set; }
    public PerformanceBand? AQLBand { get; set; }
    public PerformanceBand? MATBand { get; set; }
    public DateTime ResultDate { get; set; }
    public DateTime ExpiryDate { get; set; } // 3 years from booking date
    public bool IsReleased { get; set; }
    
    // Navigation property
    public Registration Registration { get; set; }
}
```

#### AuditLog Entity
```csharp
public class AuditLog
{
    public Guid Id { get; set; }
    public DateTime Timestamp { get; set; }
    public string UserId { get; set; }
    public string UserEmail { get; set; }
    public string Action { get; set; }
    public string Module { get; set; }
    public string EntityType { get; set; }
    public string EntityId { get; set; }
    public string? BeforeValue { get; set; } // JSON
    public string? AfterValue { get; set; } // JSON
    public string IpAddress { get; set; }
    public string UserAgent { get; set; }
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
}
```

### DTOs

#### RegistrationDto
```csharp
public class RegistrationDto
{
    public Guid Id { get; set; }
    public string NBTNumber { get; set; }
    public string StudentName { get; set; }
    public string Email { get; set; }
    public DateTime RegistrationDate { get; set; }
    public string Status { get; set; }
    public DateTime TestDate { get; set; }
    public string VenueName { get; set; }
    public string TestType { get; set; }
    public string PaymentStatus { get; set; }
}
```

#### BookingDto
```csharp
public class BookingDto
{
    public Guid Id { get; set; }
    public Guid SessionId { get; set; }
    public DateTime TestDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public string VenueName { get; set; }
    public string TestType { get; set; }
    public int AvailableSeats { get; set; }
    public bool CanBook { get; set; }
}
```

### API Endpoints

See Section 9 of constitution.md for complete API contract definitions.

---

## 5. Tasks (/speckit.tasks)

### Task Breakdown

#### Epic 1: Shell Audit & Foundation
- [x] **TASK-001**: Audit Domain layer entities
- [x] **TASK-002**: Audit Application layer services
- [ ] **TASK-003**: Audit Infrastructure layer configurations
- [ ] **TASK-004**: Audit Web API controllers
- [ ] **TASK-005**: Audit Web UI components
- [ ] **TASK-006**: Create gap analysis report

#### Epic 2: Domain Layer Completion
- [x] **TASK-010**: Create Student entity with IDType support
- [x] **TASK-011**: Create Registration entity
- [x] **TASK-012**: Create Payment entity
- [x] **TASK-013**: Create TestSession entity
- [x] **TASK-014**: Create Venue entity
- [x] **TASK-015**: Create Room entity
- [x] **TASK-016**: Create RoomAllocation entity
- [x] **TASK-017**: Create TestResult entity
- [ ] **TASK-018**: Create SpecialSession entity
- [x] **TASK-019**: Create AuditLog entity
- [x] **TASK-020**: Implement LuhnValidator
- [x] **TASK-021**: Implement SA ID Validator
- [x] **TASK-022**: Implement Foreign ID Validator
- [ ] **TASK-023**: Implement Booking Rules Validator
- [ ] **TASK-024**: Create NBTNumber value object
- [ ] **TASK-025**: Write unit tests for all validators

#### Epic 3: Application Layer Implementation
- [ ] **TASK-030**: Create IStudentService interface
- [ ] **TASK-031**: Create IRegistrationService interface
- [ ] **TASK-032**: Create IBookingService interface
- [ ] **TASK-033**: Create IPaymentService interface
- [ ] **TASK-034**: Create IVenueService interface
- [ ] **TASK-035**: Create IRoomService interface
- [ ] **TASK-036**: Create IResultsImportService interface
- [ ] **TASK-037**: Create IReportService interface
- [ ] **TASK-038**: Create INotificationService interface
- [ ] **TASK-039**: Create all DTOs
- [ ] **TASK-040**: Configure AutoMapper profiles
- [ ] **TASK-041**: Create FluentValidation validators
- [ ] **TASK-042**: Write unit tests for all services

#### Epic 4: Infrastructure Layer Implementation
- [x] **TASK-050**: Configure Student entity (EF Core)
- [x] **TASK-051**: Configure Registration entity
- [x] **TASK-052**: Configure Payment entity
- [x] **TASK-053**: Configure TestSession entity
- [x] **TASK-054**: Configure Venue entity
- [x] **TASK-055**: Configure Room entity
- [x] **TASK-056**: Configure RoomAllocation entity
- [x] **TASK-057**: Configure TestResult entity
- [ ] **TASK-058**: Configure SpecialSession entity
- [x] **TASK-059**: Configure AuditLog entity
- [x] **TASK-060**: Create migration for core entities
- [ ] **TASK-061**: Implement generic repository
- [ ] **TASK-062**: Implement specialized repositories
- [x] **TASK-063**: Implement NBT Number Generator service
- [ ] **TASK-064**: Implement EasyPay integration service
- [ ] **TASK-065**: Implement Email service
- [ ] **TASK-066**: Implement Excel import/export service
- [ ] **TASK-067**: Implement PDF generation service
- [ ] **TASK-068**: Write integration tests

#### Epic 5: Web API Implementation
- [x] **TASK-070**: Create RegistrationsController (partial)
- [x] **TASK-071**: Create StudentsController (partial)
- [ ] **TASK-072**: Create BookingsController
- [ ] **TASK-073**: Create PaymentsController
- [ ] **TASK-074**: Create VenuesController
- [ ] **TASK-075**: Create RoomsController
- [ ] **TASK-076**: Create ResultsController
- [ ] **TASK-077**: Create ReportsController
- [ ] **TASK-078**: Configure Swagger documentation
- [ ] **TASK-079**: Add authorization attributes
- [ ] **TASK-080**: Implement error handling middleware
- [ ] **TASK-081**: Write API integration tests

#### Epic 6: Blazor Web UI Implementation
- [ ] **TASK-090**: Create RegistrationWizard component
- [ ] **TASK-091**: Create PersonalInformationStep component
- [ ] **TASK-092**: Create NBTNumberGenerationStep component
- [ ] **TASK-093**: Create TestSessionSelectionStep component
- [ ] **TASK-094**: Create PaymentStep component
- [ ] **TASK-095**: Create ConfirmationStep component
- [ ] **TASK-096**: Create SessionCalendar component
- [ ] **TASK-097**: Create BookingConfirmation component
- [ ] **TASK-098**: Create admin dashboard pages
- [ ] **TASK-099**: Create PagedDataGrid component
- [ ] **TASK-100**: Create ExportButton component
- [ ] **TASK-101**: Create ImportDialog component
- [ ] **TASK-102**: Apply Fluent UI theming
- [ ] **TASK-103**: Configure navigation
- [ ] **TASK-104**: Write UI tests (bUnit)

#### Epic 7: Security & Authentication
- [ ] **TASK-110**: Configure JWT authentication
- [ ] **TASK-111**: Implement refresh token mechanism
- [ ] **TASK-112**: Configure role-based authorization
- [ ] **TASK-113**: Implement audit logging middleware
- [ ] **TASK-114**: Configure HTTPS enforcement
- [ ] **TASK-115**: Configure HSTS
- [ ] **TASK-116**: Add security headers
- [ ] **TASK-117**: Configure CORS
- [ ] **TASK-118**: Write security tests

#### Epic 8: Testing & Quality Assurance
- [ ] **TASK-120**: Achieve 80% unit test coverage
- [ ] **TASK-121**: Complete integration tests
- [ ] **TASK-122**: Complete UI tests
- [ ] **TASK-123**: Run performance tests
- [ ] **TASK-124**: Run accessibility audit
- [ ] **TASK-125**: Run security scan
- [ ] **TASK-126**: Fix all identified issues

#### Epic 9: CI/CD & Deployment
- [ ] **TASK-130**: Create GitHub Actions workflow
- [ ] **TASK-131**: Configure automated testing
- [ ] **TASK-132**: Configure code quality gates
- [ ] **TASK-133**: Create Docker containers
- [ ] **TASK-134**: Configure Azure App Service
- [ ] **TASK-135**: Setup Azure Key Vault
- [ ] **TASK-136**: Configure Application Insights
- [ ] **TASK-137**: Deploy to staging environment
- [ ] **TASK-138**: Deploy to production

#### Epic 10: Documentation & Handover
- [ ] **TASK-140**: Update README
- [ ] **TASK-141**: Complete API documentation
- [ ] **TASK-142**: Create architecture diagrams
- [ ] **TASK-143**: Document database schema
- [ ] **TASK-144**: Write deployment guide
- [ ] **TASK-145**: Write user manual
- [ ] **TASK-146**: Write admin manual
- [ ] **TASK-147**: Write developer guide
- [ ] **TASK-148**: Conduct knowledge transfer

---

## 6. Review (/speckit.review)

### Audit Checklist

#### Architecture Compliance
- [ ] Clean Architecture principles followed
- [ ] Dependency injection configured
- [ ] Repository pattern implemented
- [ ] No circular dependencies
- [ ] Separation of concerns maintained

#### Entity Completeness
- [x] Student entity with IDType enum
- [x] Registration entity
- [x] Payment entity
- [x] TestSession entity
- [x] Venue entity
- [x] Room entity
- [x] RoomAllocation entity
- [x] TestResult entity
- [ ] SpecialSession entity
- [x] AuditLog entity

#### Relationship Validation
- [x] Student 1:N Registration
- [x] Registration 1:1 Payment
- [x] TestSession N:1 Venue (CRITICAL)
- [x] Venue 1:N Room
- [x] TestSession 1:N RoomAllocation
- [x] Room 1:N RoomAllocation
- [x] Student 1:N RoomAllocation

#### Business Logic Validation
- [x] Luhn validator implemented
- [x] SA ID validator implemented
- [x] Foreign ID validator implemented
- [ ] Booking rules enforced
- [ ] Capacity checking working
- [ ] Duplicate prevention active

#### Security Validation
- [ ] JWT authentication configured
- [ ] Authorization attributes applied
- [ ] Audit logging active
- [ ] HTTPS enforced
- [ ] Secrets in Key Vault

#### Performance Validation
- [ ] All queries paginated
- [ ] Lazy loading disabled
- [ ] AsNoTracking used for reads
- [ ] Projections used
- [ ] Load times < 3 seconds

#### Testing Validation
- [ ] Unit test coverage >= 80%
- [ ] Integration tests for all endpoints
- [ ] UI tests for critical workflows
- [ ] Performance tests executed
- [ ] Accessibility tests passed

---

## 7. Quickstart (/speckit.quickstart)

### Developer Setup Guide

#### Prerequisites
- .NET 8 SDK or later
- Visual Studio 2022 or VS Code
- SQL Server 2019+ or LocalDB
- Git

#### Clone Repository
```powershell
git clone https://github.com/yourusername/NBTWebApp.git
cd NBTWebApp
```

#### Restore Packages
```powershell
dotnet restore
```

#### Configure Database
1. Update connection string in `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=NBTWebApp;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

2. Apply migrations:
```powershell
cd src\NBT.Infrastructure
dotnet ef database update --startup-project ..\NBT.WebAPI
```

#### Run Application
Terminal 1 (Web API):
```powershell
cd src\NBT.WebAPI
dotnet run
```

Terminal 2 (Blazor Web UI):
```powershell
cd src\NBT.WebUI
dotnet run
```

#### Verify Setup
- Web UI: https://localhost:7001
- Web API: https://localhost:7000
- Swagger: https://localhost:7000/swagger

#### Run Tests
```powershell
dotnet test
```

#### Seed Test Data
```powershell
cd src\NBT.Infrastructure
dotnet run --project ..\NBT.WebAPI --seed
```

---

## 8. Implementation Strategy

### Execution Approach

#### 1. Incremental Development
- Complete one feature at a time
- Test thoroughly before moving to next
- Maintain working application at all times

#### 2. Test-Driven Development (TDD)
- Write tests first for business logic
- Implement feature to pass tests
- Refactor for quality

#### 3. Continuous Integration
- Commit frequently to feature branches
- Run automated tests on every commit
- Merge to develop only when tests pass

#### 4. Code Review Process
- All changes require pull request
- Minimum one approval required
- Automated checks must pass

#### 5. Documentation as Code
- Update documentation with code changes
- Keep README current
- Maintain API documentation

### Success Criteria

#### Must Have (P0)
- [x] Core domain entities created
- [x] Database schema established
- [ ] Registration wizard functional
- [ ] Booking system operational
- [ ] Payment integration working
- [ ] Admin CRUD operations complete
- [ ] Results import functional
- [ ] Basic reporting working

#### Should Have (P1)
- [ ] Advanced reporting
- [ ] Email notifications
- [ ] Special sessions management
- [ ] Room allocation UI
- [ ] Excel/PDF exports

#### Nice to Have (P2)
- [ ] SMS notifications
- [ ] Advanced analytics
- [ ] Mobile responsiveness
- [ ] Dark mode theme

### Risk Mitigation

#### Technical Risks
1. **EasyPay Integration Complexity**
   - Mitigation: Create mock service first
   - Mitigation: Thorough webhook testing

2. **Performance Issues**
   - Mitigation: Implement caching
   - Mitigation: Optimize database queries
   - Mitigation: Use pagination everywhere

3. **Security Vulnerabilities**
   - Mitigation: Regular security scans
   - Mitigation: Follow OWASP guidelines
   - Mitigation: Comprehensive audit logging

#### Project Risks
1. **Scope Creep**
   - Mitigation: Strict adherence to constitution
   - Mitigation: Change control process

2. **Timeline Delays**
   - Mitigation: Prioritized task list
   - Mitigation: MVP first approach

---

## 9. Next Steps

### Immediate Actions (Week 1)

1. **Complete Shell Audit**
   - Review all existing code
   - Identify all gaps
   - Create detailed task backlog

2. **Setup Development Environment**
   - All developers have working setup
   - Database migrations applied
   - Test data seeded

3. **Begin Phase 2 Implementation**
   - Complete remaining domain entities
   - Implement all validators
   - Achieve 100% domain layer tests

### Weekly Milestones

- **Week 1**: Shell audit complete, environment setup
- **Week 2**: Domain layer 100% complete
- **Week 3-4**: Application layer complete
- **Week 5-6**: Infrastructure layer complete
- **Week 7-8**: Web API complete
- **Week 9-11**: Web UI complete
- **Week 12**: Security complete
- **Week 13-14**: Testing complete
- **Week 15**: Deployment complete
- **Week 16**: Documentation and handover

### Definition of Done

A feature is considered "done" when:
- [ ] Code implemented and reviewed
- [ ] Unit tests written and passing
- [ ] Integration tests written and passing
- [ ] Documentation updated
- [ ] Code merged to develop branch
- [ ] Feature deployed to staging
- [ ] Acceptance criteria met

---

## 10. Appendix

### Glossary

**AQL**: Academic and Quantitative Literacy Test  
**MAT**: Mathematics Test  
**NBT**: National Benchmark Tests  
**CEA**: Centre for Educational Assessment  
**POPIA**: Protection of Personal Information Act  
**WCAG**: Web Content Accessibility Guidelines  
**TDD**: Test-Driven Development  
**CRUD**: Create, Read, Update, Delete  
**JWT**: JSON Web Token  
**RBAC**: Role-Based Access Control

### References

- [Constitution](constitution.md)
- [Contracts](contracts.md)
- [Plan](plan.md)
- [Tasks](tasks.md)
- [Review](review.md)
- [Quickstart](quickstart.md)

### Contact

For questions or clarifications, contact the technical lead or architecture review board.

---

**Document Version**: 1.0  
**Last Updated**: 2025-11-08  
**Status**: Active  
**Next Review**: As needed

