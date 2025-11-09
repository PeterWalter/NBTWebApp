# NBT Integrated Web Application - Constitution

## Document Control
- **Version**: 1.0
- **Date**: 2025-11-09
- **Status**: ACTIVE
- **Authority**: Non-Negotiable

---

## 1. FOUNDATIONAL PRINCIPLES

### 1.1 Architecture Mandates
- **REQUIRED**: Clean Architecture pattern with strict layer separation
- **REQUIRED**: Blazor WebAssembly with Interactive Auto mode for frontend
- **REQUIRED**: ASP.NET Core Web API (.NET 9) for backend services
- **REQUIRED**: MS SQL Server for production database
- **REQUIRED**: Fluent UI components (NO MudBlazor or other UI frameworks)
- **REQUIRED**: Entity Framework Core for all data access
- **REQUIRED**: Dependency Injection for all service registrations
- **PROHIBITED**: Direct database access from presentation layer
- **PROHIBITED**: Business logic in controllers or Razor components

### 1.2 Project Structure
```
src/
├── NBT.Domain/          # Entities, enums, domain logic
├── NBT.Application/     # Services, DTOs, interfaces, business logic
├── NBT.Infrastructure/  # EF Core, repositories, external services
├── NBT.WebAPI/         # Controllers, middleware, API configuration
├── NBT.Client/         # Blazor WASM client (if separated)
└── NBT.WebUI/          # Blazor pages, components, ViewModels
```

---

## 2. SECURITY REQUIREMENTS

### 2.1 Authentication & Authorization
- **REQUIRED**: JWT token-based authentication
- **REQUIRED**: Role-based access control (Admin, Staff, SuperUser, Student)
- **REQUIRED**: HTTPS-only for all communications
- **REQUIRED**: Secure password hashing (bcrypt or PBKDF2)
- **REQUIRED**: OTP verification for account creation
- **REQUIRED**: Full audit logging for all CRUD operations
- **PROHIBITED**: Plain-text password storage
- **PROHIBITED**: HTTP endpoints in production

### 2.2 Data Protection
- **REQUIRED**: WCAG 2.1 AA compliance for accessibility
- **REQUIRED**: Input validation on both client and server
- **REQUIRED**: SQL injection prevention (parameterized queries only)
- **REQUIRED**: XSS protection (output encoding)
- **REQUIRED**: CORS configuration for production domains only

---

## 3. BUSINESS LOGIC RULES

### 3.1 NBT Number Generation
- **REQUIRED**: 14-digit unique identifier using Luhn (modulus-10) algorithm
- **REQUIRED**: Generated immediately after successful registration
- **REQUIRED**: Immutable once assigned
- **REQUIRED**: Linked to all bookings, payments, and results

### 3.2 Student Registration
- **REQUIRED**: Multi-step wizard with progress saving
- **REQUIRED**: Support for SA ID, Foreign ID, and Passport ID
- **REQUIRED**: Automatic DOB and Gender extraction from SA ID
- **REQUIRED**: Age, Gender, Ethnicity collection
- **REQUIRED**: Duplicate prevention checks
- **REQUIRED**: Resume capability for interrupted registrations
- **REQUIRED**: Pre-test questionnaire completion
- **REQUIRED**: Special accommodation requests workflow

### 3.3 Test Booking Rules
- **REQUIRED**: Students can book only ONE test at a time
- **REQUIRED**: New booking allowed only after previous test closing date passes
- **REQUIRED**: Maximum 2 tests per year per student
- **REQUIRED**: Test validity period: 3 years from booking date
- **REQUIRED**: Booking modification allowed before closing date
- **REQUIRED**: Test types: AQL, MAT, or AQL+MAT
- **REQUIRED**: Venue selection with availability validation
- **REQUIRED**: Intake year starts April 1st annually

### 3.4 Payment Rules
- **REQUIRED**: EasyPay integration for payment processing
- **REQUIRED**: Installment payment support until full payment
- **REQUIRED**: Payment order: chronological by test booking date
- **REQUIRED**: Cost variation by intake year tracking
- **REQUIRED**: Bank payment file upload support
- **REQUIRED**: Payment status tracking and automatic updates
- **REQUIRED**: Students view/download ONLY fully paid results
- **REQUIRED**: Staff/Admin view all results regardless of payment status

### 3.5 Test Results Rules
- **REQUIRED**: AQL test → AL and QL scores
- **REQUIRED**: MAT test → AL, QL, and MAT scores
- **REQUIRED**: Performance levels: Basic Lower, Basic Upper, Intermediate Lower, Proficient Lower, etc.
- **REQUIRED**: Barcode on results to distinguish test instances
- **REQUIRED**: Unique barcode per answer sheet
- **REQUIRED**: PDF certificate generation for completed tests
- **REQUIRED**: Secure result access post-release

### 3.6 Venue Management Rules
- **REQUIRED**: Venue types: National, Special Session, Research, Other
- **REQUIRED**: Venue availability by date tracking
- **REQUIRED**: Room capacity management
- **REQUIRED**: Online test support (any location with video/audio/internet)
- **REQUIRED**: Test date calendar with closing dates
- **REQUIRED**: Sunday test highlighting
- **REQUIRED**: Online test date highlighting
- **REQUIRED**: TestSession linked to TestVenue (not individual rooms)

### 3.7 Special Sessions
- **REQUIRED**: Remote writer management
- **REQUIRED**: Invigilator details collection
- **REQUIRED**: Off-site venue information capture
- **REQUIRED**: Automatic routing to NBT remote administration team

---

## 4. TECHNICAL STANDARDS

### 4.1 Validation Standards
- **REQUIRED**: Luhn algorithm validation for NBT and ID numbers
- **REQUIRED**: Email format validation (RFC 5322)
- **REQUIRED**: Phone number format validation (international formats)
- **REQUIRED**: Date range validation (realistic DOB, future test dates)
- **REQUIRED**: File upload validation (type, size limits)

### 4.2 Performance Requirements
- **REQUIRED**: Page load time < 3 seconds
- **REQUIRED**: API response time < 500ms (95th percentile)
- **REQUIRED**: Efficient database indexing on frequently queried fields
- **REQUIRED**: Lazy loading for large datasets
- **REQUIRED**: Pagination for list views (max 50 items per page)

### 4.3 Testing Requirements
- **REQUIRED**: Unit tests for business logic (minimum 80% coverage)
- **REQUIRED**: Integration tests for API endpoints
- **REQUIRED**: End-to-end tests for critical workflows
- **REQUIRED**: Automated CI/CD pipeline with test gates
- **REQUIRED**: Manual UAT sign-off before production deployment

### 4.4 Code Quality Standards
- **REQUIRED**: Consistent naming conventions (PascalCase for public, camelCase for private)
- **REQUIRED**: XML documentation comments for public APIs
- **REQUIRED**: SOLID principles adherence
- **REQUIRED**: DRY principle (no code duplication)
- **REQUIRED**: Async/await for I/O operations
- **REQUIRED**: Proper exception handling with logging
- **PROHIBITED**: Magic numbers (use constants/enums)
- **PROHIBITED**: Commented-out code in production

### 4.5 Database Standards
- **REQUIRED**: EF Core Code-First migrations
- **REQUIRED**: Proper foreign key constraints
- **REQUIRED**: Audit fields (CreatedBy, CreatedDate, ModifiedBy, ModifiedDate)
- **REQUIRED**: Soft delete implementation (IsDeleted flag)
- **REQUIRED**: Database transaction management for multi-table operations
- **REQUIRED**: Connection string encryption in production

---

## 5. USER INTERFACE STANDARDS

### 5.1 Component Standards
- **REQUIRED**: Fluent UI components exclusively
- **REQUIRED**: Responsive design (mobile, tablet, desktop)
- **REQUIRED**: Consistent color scheme and branding
- **REQUIRED**: Loading indicators for async operations
- **REQUIRED**: Error message display with user-friendly text
- **REQUIRED**: Form validation feedback (inline and summary)
- **REQUIRED**: Accessible navigation with keyboard support

### 5.2 User Experience
- **REQUIRED**: Dashboard on login (role-based)
- **REQUIRED**: Left-side navigation menu for authenticated users
- **REQUIRED**: Landing page menus: Applicants, Institutions, Educators
- **REQUIRED**: Contextual help and tooltips
- **REQUIRED**: Progress indicators for multi-step processes
- **REQUIRED**: Automated email/SMS notifications
- **REQUIRED**: Video embedding support for instructional content
- **REQUIRED**: Breadcrumb navigation for deep pages

---

## 6. DEPLOYMENT & OPERATIONS

### 6.1 Environment Standards
- **REQUIRED**: Development, Staging, Production environments
- **REQUIRED**: Automated deployment via GitHub Actions
- **REQUIRED**: Environment-specific configuration (appsettings.json)
- **REQUIRED**: Database migration automation
- **REQUIRED**: Health check endpoints
- **REQUIRED**: Application logging (Serilog or similar)
- **REQUIRED**: Performance monitoring and alerting

### 6.2 Version Control
- **REQUIRED**: Git branching strategy (feature → dev → staging → main)
- **REQUIRED**: Pull request reviews before merge
- **REQUIRED**: Semantic versioning (MAJOR.MINOR.PATCH)
- **REQUIRED**: Commit message standards (Conventional Commits)
- **REQUIRED**: Tag releases in main branch

### 6.3 Documentation
- **REQUIRED**: API documentation (Swagger/OpenAPI)
- **REQUIRED**: Developer quickstart guide
- **REQUIRED**: User manuals for each role
- **REQUIRED**: Architecture decision records (ADRs)
- **REQUIRED**: Database schema diagrams
- **REQUIRED**: Deployment runbooks

---

## 7. DATA CONTRACTS

### 7.1 Core Entities
- Student (with ForeignID/PassportID support)
- Registration (multi-step wizard state)
- NBTNumber (Luhn-validated unique identifier)
- Booking (test selection, venue, dates)
- Payment (EasyPay integration, installments)
- Result (AL, QL, MAT scores with barcode)
- Venue (type, capacity, availability)
- Room (capacity tracking)
- TestSession (linked to Venue)
- TestDate (calendar with closing dates, Sunday/Online flags)
- Staff (roles and permissions)
- AuditLog (comprehensive change tracking)

### 7.2 API Endpoints
- `/api/auth` - Authentication and authorization
- `/api/registration` - Student registration wizard
- `/api/nbt-number` - NBT number generation
- `/api/booking` - Test booking management
- `/api/payments` - Payment processing and status
- `/api/results` - Test result access
- `/api/venues` - Venue management
- `/api/rooms` - Room allocation
- `/api/staff` - Staff CRUD operations
- `/api/reports` - Excel/PDF report generation
- `/api/admin` - Administrative functions

---

## 8. COMPLIANCE & AUDIT

### 8.1 Audit Requirements
- **REQUIRED**: Log all user actions (login, CRUD, downloads)
- **REQUIRED**: Timestamp all database records
- **REQUIRED**: Track user identity for all changes
- **REQUIRED**: Immutable audit log (append-only)
- **REQUIRED**: Audit log retention: minimum 7 years

### 8.2 Reporting Requirements
- **REQUIRED**: Excel export for all list views
- **REQUIRED**: PDF certificate generation for results
- **REQUIRED**: Summary charts and analytics dashboards
- **REQUIRED**: Financial reconciliation reports
- **REQUIRED**: Student demographic reports for equity analysis

---

## 9. CHANGE MANAGEMENT

### 9.1 Constitution Updates
- **PROCESS**: Constitution changes require stakeholder approval
- **PROCESS**: Version increment and changelog required
- **PROCESS**: Communication to all development team members
- **PROCESS**: Grace period for migration (minimum 2 sprints)

### 9.2 Deprecation Policy
- **PROCESS**: Feature deprecation notice: minimum 6 months
- **PROCESS**: Migration path documentation required
- **PROCESS**: Backward compatibility during transition period

---

## 10. ENFORCEMENT

### 10.1 Code Review Checklist
- [ ] Clean Architecture layers respected
- [ ] Fluent UI components used (no MudBlazor)
- [ ] EF Core used for data access
- [ ] JWT authentication implemented
- [ ] Input validation on client and server
- [ ] Audit logging in place
- [ ] Unit tests written (80%+ coverage)
- [ ] Performance requirements met
- [ ] Accessibility compliance verified
- [ ] Documentation updated

### 10.2 Non-Compliance Response
- **MINOR**: Code review comment → fix before merge
- **MAJOR**: Pull request rejection → rework required
- **CRITICAL**: Architecture violation → escalate to lead architect

---

## SIGNATURES

**Approved By**: NBT Project Steering Committee  
**Effective Date**: 2025-11-09  
**Review Cycle**: Quarterly  
**Next Review**: 2025-02-09

---

*This constitution is the authoritative source for all architectural, security, and operational decisions in the NBT Integrated Web Application project. All team members must adhere to these principles without exception.*
