# NBT Integrated Web Application - Constitution

## Non-Negotiable Principles

### 1. Architecture Standards
- **Clean Architecture**: Strict separation of concerns with Domain, Application, Infrastructure, and Presentation layers
- **Technology Stack**:
  - Frontend: Blazor WebAssembly with Fluent UI components (NO MudBlazor)
  - Backend: ASP.NET Core 9.0 Web API
  - Database: MS SQL Server with Entity Framework Core 9.0
  - Authentication: JWT-based with role-based authorization
- **Dependency Injection**: All services must be registered and injected via DI container
- **API Design**: RESTful endpoints with consistent naming and response patterns

### 2. Security Requirements
- **HTTPS Only**: All data transfer must use HTTPS/TLS
- **Authentication**: JWT tokens with refresh token support
- **Authorization**: Role-based access control (Admin, Staff, SuperUser, Student)
- **Audit Logging**: Complete audit trail for all data modifications
- **Data Protection**: Sensitive data encrypted at rest and in transit
- **Password Policy**: Strong passwords with complexity requirements
- **Session Management**: Secure session handling with timeout policies

### 3. Data Validation Rules
- **NBT Number**: 14-digit number validated using Luhn (modulus-10) algorithm
- **SA ID Number**: 13-digit South African ID with Luhn validation, DOB and gender extraction
- **Foreign ID/Passport**: Support for non-SA applicants with passport/foreign ID registration
- **Email Validation**: RFC 5322 compliant email validation
- **Phone Numbers**: South African and international format support
- **File Uploads**: Type, size, and content validation for all uploads

### 4. Performance Standards
- **Page Load**: < 3 seconds for initial load
- **API Response**: < 500ms for standard CRUD operations
- **Concurrent Users**: Support minimum 1000 concurrent users
- **Database Queries**: All queries must use proper indexing and optimization
- **Caching**: Implement caching strategies for frequently accessed data

### 5. Accessibility & UX
- **WCAG 2.1 AA Compliance**: All pages must meet accessibility standards
- **Responsive Design**: Mobile-first approach, support all device sizes
- **Browser Support**: Chrome, Firefox, Edge, Safari (latest 2 versions)
- **Progressive Enhancement**: Core functionality works without JavaScript
- **Error Handling**: User-friendly error messages with recovery options

### 6. Testing Requirements
- **Unit Tests**: Minimum 80% code coverage
- **Integration Tests**: All API endpoints tested
- **E2E Tests**: Critical user workflows automated
- **Performance Tests**: Load testing for concurrent users
- **Security Tests**: OWASP Top 10 vulnerability scanning

### 7. CI/CD Requirements
- **Version Control**: Git with feature branch workflow
- **Build Pipeline**: Automated build on every commit
- **Testing Pipeline**: Automated test execution
- **Deployment**: Automated deployment to staging and production
- **Rollback**: Automated rollback capability
- **Monitoring**: Application performance and error monitoring

### 8. Business Rules
- **NBT Number Generation**: Unique 14-digit number using Luhn algorithm upon registration
- **Test Booking**: 
  - One active booking at a time
  - Can book another test only after closing date of current booking passes
  - Maximum 2 tests per year
  - Tests valid for 3 years from booking date
  - Booking changes allowed before closing date
  - Bookings open from start of Year Intake (typically 1 April)
- **Payment Rules**:
  - Installment payments allowed until complete
  - Payments applied in order of tests being written
  - Test costs vary by intake year
  - Only fully paid tests downloadable by students as PDF certificates
  - Staff/Admin can view all tests regardless of payment status
  - Bank payment file uploads supported in specific format
- **Test Sessions**:
  - Linked to TestVenue (not Room)
  - Online tests: Written remotely with video/sound/internet requirements
  - Sunday tests and Online tests highlighted in calendar
  - Test dates with closing booking dates available in calendar
- **Results**:
  - AQL test: AL and QL results with performance levels
  - Math test: AL, QL, and MAT results with performance levels
  - Performance levels: Basic Lower/Upper, Intermediate Lower/Upper, Proficient Lower/Upper, etc.
  - Each test identified by unique Barcode (distinguishes answer sheet)
  - Multiple test results tracked separately by Barcode
- **Venues**:
  - Types: National, Special Session, Research, Other
  - Date-based availability
  - Test dates with closing booking dates
- **Registration Recovery**: Students can resume interrupted registration from last completed step without redoing completed work
- **Special/Remote Sessions**: Off-site testing with invigilator and venue details routed to NBT remote administration team

### 9. User Roles & Permissions
- **Student/Applicant**: 
  - Account creation with SA ID or Foreign ID/Passport
  - Registration, booking, payment, results access
  - Profile management with document uploads
  - Pre-test questionnaire completion
  - Dashboard with left-side menu navigation
- **Staff**: View/edit student data, manage bookings, process payments, manage results
- **Admin**: Full CRUD operations, user management, venue management, reporting
- **SuperUser**: System configuration, audit logs, advanced administration

### 10. Data Retention & Compliance
- **Account Retention**: Active for future access and new test cycles, preserving academic history
- **Audit Trail**: All modifications logged with user, timestamp, and change details
- **Data Privacy**: POPIA compliance for South African personal data
- **Backup**: Daily automated backups with point-in-time recovery
- **Archive**: Historical data archived after retention period

## Code Standards

### Naming Conventions
- **Classes**: PascalCase (e.g., `StudentRegistrationService`)
- **Methods**: PascalCase (e.g., `GenerateNbtNumber()`)
- **Variables**: camelCase (e.g., `studentId`)
- **Constants**: UPPER_SNAKE_CASE (e.g., `MAX_TESTS_PER_YEAR`)
- **Interfaces**: PascalCase with 'I' prefix (e.g., `IStudentRepository`)

### File Organization
```
src/
├── NBTWebApp.Domain/          # Entities, ValueObjects, Enums
├── NBTWebApp.Application/     # Services, DTOs, Interfaces
├── NBTWebApp.Infrastructure/  # EF Context, Repositories, External Services
├── NBTWebApp.API/            # Controllers, Middleware
└── NBTWebApp.Client/         # Blazor Pages, Components, ViewModels
```

### Error Handling
- Use structured exception handling
- Log all errors with context
- Return appropriate HTTP status codes
- Provide user-friendly error messages
- Implement global exception middleware

### Documentation
- XML comments for all public APIs
- README files for each module
- Architecture decision records (ADRs)
- API documentation via Swagger/OpenAPI
- User guides for end users

## Enforcement
- Pull requests require code review
- Automated linting and code analysis
- Build fails on test failures
- Security scanning on every build
- Performance benchmarks tracked
