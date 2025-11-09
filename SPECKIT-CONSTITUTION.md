# NBT Web Application - Constitution

## Non-Negotiable Principles, Coding Standards, and Architectural Rules

**Version:** 2.0  
**Date:** 2025-11-09  
**Status:** Active

---

## 1. Core Principles

### 1.1 Technology Stack
- **Frontend:** Blazor WebAssembly with **Fluent UI** components (NOT MudBlazor)
- **Backend:** ASP.NET Core 9.0 Web API
- **Database:** MS SQL Server with Entity Framework Core
- **Authentication:** JWT-based with role-based authorization
- **Architecture:** Clean Architecture with clear separation of concerns

### 1.2 Architectural Standards
- **Clean Architecture Layers:**
  - `NBT.Domain`: Entities, Enums, ValueObjects, Interfaces
  - `NBT.Application`: Business Logic, DTOs, Services, Validators
  - `NBT.Infrastructure`: Data Access, External Services, Email, Payment Gateway
  - `NBT.WebAPI`: Controllers, Middleware, API Configuration
  - `NBT.WebUI.Client`: Blazor Components, Pages, ViewModels

- **Dependency Injection:** All services must be registered and injected
- **Entity Framework Core:** Code-First approach with migrations
- **Repository Pattern:** All data access through repositories
- **Unit of Work:** Transaction management through UnitOfWork pattern

### 1.3 Security Standards
- **HTTPS Only:** All data transfer must use HTTPS
- **Authentication:** JWT tokens with secure storage
- **Authorization:** Role-based access control (Admin, Staff, SuperUser, Student)
- **Audit Logging:** All CRUD operations must be logged
- **Data Validation:** Server-side and client-side validation
- **Password Policy:** Minimum 8 characters, uppercase, lowercase, number, special character

### 1.4 Data Integrity
- **NBT Number Generation:** 14-digit number with Luhn algorithm validation
- **SA ID Validation:** 13-digit with Luhn algorithm and DOB/Gender extraction
- **Foreign ID Support:** Passport and Foreign ID support for international students
- **Duplicate Prevention:** No duplicate ID numbers or NBT numbers
- **Data Consistency:** Foreign key constraints and cascade rules

---

## 2. Business Rules

### 2.1 Student Registration
- Students can register with SA ID, Foreign ID, or Passport
- NBT number is auto-generated upon successful registration (14 digits)
- Age and Gender extracted from SA ID for South African students
- Duplicate ID numbers are not allowed
- Account activation via email verification (OTP)

### 2.2 Test Booking Rules
- **Booking Window:** Opens on 1 April each year (Intake Year)
- **One Test at a Time:** Student can only book one test at a time
- **Subsequent Bookings:** Can book another test only after the closing date of current booking has passed
- **Maximum Tests:** Students can write maximum of 2 tests per year
- **Test Validity:** Tests are valid for 3 years from date of booking
- **Change Booking:** Students can change bookings before closing date
- **Test Types:** AQL (Academic and Quantitative Literacy) or AQL+MAT (includes Mathematics)

### 2.3 Payment Rules
- **Installment Payments:** Payments can be made in installments until complete
- **Payment Order:** Payments applied in order of tests being written
- **Variable Costs:** Test costs vary by Intake Year
- **Full Payment Required:** Only fully paid tests can be viewed/downloaded by students
- **Staff Access:** Staff and Admin can view all tests regardless of payment status
- **EasyPay Integration:** All online payments through EasyPay gateway
- **Payment Tracking:** Complete audit trail of all payment transactions

### 2.4 Test Results
- **Result Components:**
  - **AQL Test:** AL (Academic Literacy) and QL (Quantitative Literacy) scores
  - **MAT Test:** AL, QL, and MAT (Mathematics) scores
- **Performance Levels:** Basic Lower, Basic Upper, Intermediate Lower, Intermediate Upper, Proficient Lower, Proficient Upper, etc.
- **Barcode System:** Each test has a unique barcode to identify the answer sheet
- **Multiple Tests:** Students writing 2 tests will have different barcodes
- **Result Access:** Students can only view fully paid test results
- **PDF Certificate:** Downloadable PDF certificate with barcode
- **Staff Access:** Staff/Admin can view all results regardless of payment

### 2.5 Venue Management
- **Venue Types:**
  - National (regular test centers)
  - Special Session (accommodation for special needs)
  - Research (for research projects)
  - Other (to be defined)
- **Venue Availability:** Venues may not be available for certain test dates
- **Test Date Calendar:** Table of test dates with closing booking dates
- **Sunday Tests:** Highlighted in calendar (different color)
- **Online Tests:** Can be written from anywhere with computer, video, sound, and internet

### 2.6 Special Sessions
- Remote writer sessions for students unable to attend regular venues
- Special accommodation for disabilities
- Custom venue and invigilator arrangements
- Automatic routing to NBT remote administration team

---

## 3. Coding Standards

### 3.1 C# Standards
```csharp
// Naming Conventions
- Classes: PascalCase (Student, Registration)
- Methods: PascalCase (GenerateNBTNumber, ValidatePayment)
- Properties: PascalCase (FirstName, NBTNumber)
- Private Fields: _camelCase (_studentRepository, _logger)
- Constants: UPPER_CASE (MAX_TESTS_PER_YEAR)

// Code Organization
- One class per file
- File name matches class name
- Interfaces start with 'I' (IStudentRepository)
- Async methods end with 'Async' (GetStudentAsync)
- Use statements at top, grouped and sorted
```

### 3.2 Entity Framework Standards
```csharp
// Entity Configuration
- Use Fluent API for complex relationships
- Index frequently queried fields
- Required fields marked with [Required]
- String lengths defined with [StringLength]
- Navigation properties virtual for lazy loading
- Soft delete using IsDeleted flag

// Migration Standards
- Descriptive migration names
- Review migrations before applying
- Rollback plan for production migrations
```

### 3.3 API Standards
```csharp
// REST API Conventions
- Use HTTP verbs correctly (GET, POST, PUT, DELETE)
- Plural resource names (/api/students, /api/registrations)
- Version APIs (/api/v1/students)
- Return appropriate status codes (200, 201, 400, 404, 500)
- Use DTOs for requests/responses
- Consistent error response format

// Endpoints
/api/students
/api/registrations
/api/bookings
/api/payments
/api/results
/api/venues
/api/test-sessions
/api/reports
```

### 3.4 Blazor Standards
```csharp
// Component Organization
- Use code-behind pattern (.razor + .razor.cs)
- Keep markup clean and readable
- Use Fluent UI components exclusively
- State management in ViewModels
- Dependency injection for services

// Naming
- Components: PascalCase (StudentList.razor)
- Parameters: PascalCase with [Parameter]
- Event callbacks: PascalCase (OnStudentSelected)
```

---

## 4. Performance Standards

### 4.1 Load Time Requirements
- Initial page load: < 3 seconds
- Navigation between pages: < 1 second
- API response time: < 500ms (excluding external services)
- Database queries: < 200ms average

### 4.2 Optimization Techniques
- Use pagination for large datasets (50 items per page)
- Implement lazy loading for related entities
- Cache frequently accessed data (venues, test dates)
- Optimize images and assets
- Use async/await for all I/O operations

---

## 5. Testing Standards

### 5.1 Test Coverage
- Unit tests for all business logic (minimum 80% coverage)
- Integration tests for API endpoints
- E2E tests for critical workflows
- Load testing for concurrent users

### 5.2 Test Organization
```
NBT.Tests/
├── Unit/
│   ├── Domain/
│   ├── Application/
│   └── Infrastructure/
├── Integration/
│   ├── API/
│   └── Database/
└── E2E/
    └── Workflows/
```

---

## 6. Accessibility Standards

### 6.1 WCAG 2.1 AA Compliance
- Keyboard navigation support
- Screen reader compatibility
- Color contrast ratios (minimum 4.5:1)
- Alt text for images
- ARIA labels for interactive elements
- Form field labels and error messages

### 6.2 Responsive Design
- Mobile-first approach
- Breakpoints: 320px, 768px, 1024px, 1440px
- Touch-friendly UI (minimum 44x44px touch targets)
- Fluid typography and spacing

---

## 7. CI/CD Standards

### 7.1 Git Workflow
```
main (production)
├── develop (integration)
│   ├── feature/* (new features)
│   ├── bugfix/* (bug fixes)
│   └── hotfix/* (urgent production fixes)
```

### 7.2 Branch Rules
- All features in feature branches
- Pull requests required for main and develop
- Code review by at least one team member
- All tests must pass before merge
- Squash commits on merge

### 7.3 GitHub Actions
- Build and test on every push
- Deploy to staging on develop merge
- Deploy to production on main merge
- Run security scans
- Generate code coverage reports

---

## 8. Documentation Standards

### 8.1 Code Documentation
- XML comments for public methods and classes
- README for each major component
- API documentation using Swagger/OpenAPI
- Database schema diagrams

### 8.2 User Documentation
- User guides for each role (Student, Staff, Admin)
- Quick start guides
- FAQ section
- Video tutorials for complex workflows

---

## 9. Monitoring and Logging

### 9.1 Logging Standards
- Use structured logging (Serilog)
- Log levels: Trace, Debug, Information, Warning, Error, Critical
- Log all API requests and responses (excluding sensitive data)
- Log all database operations
- Log all authentication attempts

### 9.2 Audit Trail
- Track all CRUD operations
- Record user, timestamp, and action
- Include before/after values for updates
- Immutable audit logs

---

## 10. Compliance and Data Protection

### 10.1 POPIA Compliance
- Student consent for data processing
- Data minimization principle
- Right to access personal data
- Right to rectify incorrect data
- Right to erasure (soft delete)
- Data retention policies

### 10.2 Data Backup
- Daily automated backups
- 30-day retention period
- Offsite backup storage
- Regular restore testing

---

## Appendix A: Technology Versions

- .NET: 9.0
- Entity Framework Core: 9.0
- Fluent UI Blazor: Latest stable
- SQL Server: 2019 or higher
- Node.js: 18 LTS (for build tools)

---

## Appendix B: External Integrations

### EasyPay Gateway
- Payment reference format: EP-{RegistrationId}-{Timestamp}
- Callback URL: /api/payments/easypay-callback
- Test mode for development
- Production credentials stored in Azure Key Vault

### Email Service
- SMTP or SendGrid for transactional emails
- Templates for: Registration, Payment confirmation, Test reminders, Results

### SMS Service (Optional)
- Clickatell or Twilio
- OTP verification
- Critical notifications

---

**Approved by:** NBT Development Team  
**Review Date:** Quarterly  
**Next Review:** 2025-02-09
