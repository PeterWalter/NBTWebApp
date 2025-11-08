# NBT Integrated Web Application - Constitution

**Version:** 1.1  
**Last Updated:** 2025-11-08  
**Status:** BINDING - Non-Negotiable Principles  
**Architecture:** Blazor Web App Interactive Auto Mode

---

## 1. FOUNDATIONAL PRINCIPLES

### 1.1 Core Mission
This constitution defines the immutable architectural, security, and quality standards for the National Benchmark Tests (NBT) Integrated Web Application. All code, configurations, and deployments MUST comply with these principles without exception.

### 1.2 Scope
These principles apply to:
- All source code in the repository
- All database schemas and migrations
- All deployment configurations
- All third-party integrations
- All documentation

### 1.3 Authority
- **Enforcement:** Automated via CI/CD pipelines
- **Violations:** Code that violates these principles MUST NOT be merged or deployed
- **Changes:** This constitution can only be modified through formal architecture review

---

## 2. TECHNOLOGY STACK (IMMUTABLE)

### 2.1 Frontend Stack
```yaml
Framework: Blazor Web App Interactive Auto (.NET 8+)
Render Mode: InteractiveAuto (Server + WebAssembly hybrid)
UI Library: Microsoft Fluent UI Blazor Components
State Management: Built-in Blazor state management
HTTP Client: HttpClient with typed clients (WASM) / Direct DI services (Server)
Authentication: JWT Bearer tokens + Cookie authentication
```

**PRINCIPLE:** No alternative frontend frameworks (React, Angular, Vue) are permitted. InteractiveAuto render mode MUST be used for optimal performance.

### 2.2 Backend Stack
```yaml
Framework: ASP.NET Core Web API (.NET 8+)
Architecture: Clean Architecture (4 layers)
ORM: Entity Framework Core 8+
Database: Microsoft SQL Server 2019+
Authentication: JWT with refresh tokens
Authorization: Role-based (RBAC) + Claims-based
```

**PRINCIPLE:** No direct SQL queries outside repository pattern. All data access MUST use EF Core.

### 2.3 Database
```yaml
RDBMS: MS SQL Server 2019+
Migrations: EF Core Code-First Migrations
Connection: Encrypted connections only (TrustServerCertificate=false in production)
Backup: Automated daily backups with 30-day retention
```

**PRINCIPLE:** Database schema changes MUST be versioned through EF Core migrations.

---

## 3. ARCHITECTURAL STANDARDS

### 3.1 Clean Architecture (Mandatory)

#### Layer Structure
```
NBTWebApp.Domain/          # Enterprise Business Rules
NBTWebApp.Application/     # Application Business Rules
NBTWebApp.Infrastructure/  # External Interfaces (DB, APIs)
NBTWebApp.Web/            # Blazor Web App (Server + API)
NBTWebApp.Client/         # Blazor Interactive Components (Auto mode)
```

#### Dependency Rules (IMMUTABLE)
1. **Domain Layer:** Zero external dependencies
2. **Application Layer:** Depends ONLY on Domain
3. **Infrastructure Layer:** Depends on Application & Domain
4. **Presentation Layer:** Depends on Application & Domain (NOT Infrastructure)

**PRINCIPLE:** Dependencies flow inward. Inner layers MUST NOT reference outer layers.

### 3.2 Dependency Injection
```csharp
// ALL services MUST be registered via DI
// Singleton: Stateless, thread-safe services
// Scoped: Per-request services (DbContext, repositories)
// Transient: Stateful, lightweight services

// FORBIDDEN: new keyword for service instantiation
// REQUIRED: Constructor injection for all dependencies
```

**PRINCIPLE:** No service locator pattern. No static dependencies. All dependencies MUST be injected.

### 3.3 Repository Pattern
```csharp
// REQUIRED for all data access
public interface IRepository<T> where T : class
{
    Task<T> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
}
```

**PRINCIPLE:** Controllers/Services MUST NOT directly access DbContext.

---

## 4. SECURITY REQUIREMENTS (NON-NEGOTIABLE)

### 4.1 Transport Security
```yaml
Protocol: HTTPS ONLY (TLS 1.2 minimum, TLS 1.3 preferred)
HSTS: Enabled with max-age=31536000
Certificate: Valid SSL/TLS certificate (no self-signed in production)
```

**PRINCIPLE:** HTTP connections are FORBIDDEN in production. All API endpoints MUST reject non-HTTPS requests.

### 4.2 Authentication & Authorization

#### Authentication Requirements
```yaml
Server-Side: Cookie authentication with secure, HttpOnly cookies
Client-Side (WASM): JWT Bearer tokens with refresh mechanism
Algorithm: RS256 (asymmetric) for JWT
Expiry: Access token = 15 minutes, Refresh token = 7 days
Storage: HttpOnly cookies (server), Secure storage (WASM)
Claims: UserId, Email, Roles, Permissions
Hybrid Mode: Seamless authentication across Server and WASM render modes
```

#### Role-Based Access Control (RBAC)
```yaml
Roles:
  - Staff: Read-only access to reports and student data
  - Admin: Full CRUD on students, results, sessions, registrations
  - SuperUser: System configuration, user management, data imports, audit logs

Enforcement: [Authorize(Roles = "Admin,SuperUser")] on all protected endpoints
```

**PRINCIPLE:** All API endpoints MUST have explicit authorization attributes. No anonymous access except login/register.

### 4.3 Data Validation

#### NBT Number Validation (Luhn Algorithm)
```csharp
// REQUIRED: All NBT numbers MUST pass Luhn check
// Format: 9 digits (e.g., 123456789)
// Implementation: LuhnValidator in Domain layer
```

#### South African ID Number Validation
```csharp
// REQUIRED: All SA ID numbers MUST:
// 1. Be 13 digits
// 2. Pass Luhn checksum validation
// 3. Have valid date portion (YYMMDD)
// 4. Have valid citizenship digit (0 or 1)
// 5. Have valid gender digit (0-4 female, 5-9 male)
```

**PRINCIPLE:** All user inputs MUST be validated at API level. Client-side validation is supplementary, not primary.

### 4.4 Data Protection
```yaml
Passwords: Hashed with PBKDF2 (100k iterations minimum) or bcrypt
Sensitive Fields: Encrypted at rest (e.g., ID numbers, contact details)
Audit Logging: All CRUD operations logged with user, timestamp, action
PII Handling: Comply with POPIA (Protection of Personal Information Act)
```

**PRINCIPLE:** Passwords MUST NEVER be stored in plain text. PII MUST be encrypted.

---

## 5. ACCESSIBILITY STANDARDS (WCAG 2.1 AA)

### 5.1 Mandatory Compliance
```yaml
Level: WCAG 2.1 Level AA (minimum)
Testing: Automated (axe-core) + Manual testing
Keyboard Navigation: All interactive elements accessible via keyboard
Screen Readers: ARIA labels on all form controls and dynamic content
Color Contrast: 4.5:1 for normal text, 3:1 for large text
```

### 5.2 Implementation Requirements
```html
<!-- REQUIRED: All form inputs -->
<FluentTextField Label="Student Name" 
                 Required="true"
                 aria-required="true"
                 aria-label="Student full name" />

<!-- REQUIRED: All buttons -->
<FluentButton Appearance="Accent" 
              aria-label="Submit student registration">
    Submit
</FluentButton>

<!-- REQUIRED: All data tables -->
<FluentDataGrid Items="@students" 
                aria-label="Student registration list">
```

**PRINCIPLE:** Accessibility is NOT optional. All UI components MUST be keyboard-navigable and screen-reader compatible.

---

## 6. PERFORMANCE STANDARDS

### 6.1 Load Time Requirements
```yaml
Initial Page Load: < 3 seconds (measured at 3G speeds)
  - Server prerendering MUST be enabled for faster initial render
  - Progressive enhancement via InteractiveAuto mode
API Response Time: < 500ms (95th percentile)
Database Queries: < 100ms (95th percentile)
Component Interactivity: < 100ms after initial load
```

### 6.2 Optimization Techniques (Mandatory)
```csharp
// 1. Pagination for all list endpoints
[HttpGet]
public async Task<PagedResult<Student>> GetStudents(int page = 1, int pageSize = 50)

// 2. Lazy loading disabled, explicit loading required
optionsBuilder.UseLazyLoadingProxies(false);

// 3. Caching for reference data
[ResponseCache(Duration = 3600)] // 1 hour for test types, sessions

// 4. Async/await for all I/O operations
public async Task<Student> GetStudentAsync(int id)

// 5. Projection queries (select only needed columns)
var students = await context.Students
    .Select(s => new StudentDto { Id = s.Id, Name = s.Name })
    .ToListAsync();

// 6. Streaming rendering for large datasets (Blazor Web App)
@attribute [StreamRendering(true)]

// 7. Component render mode optimization
@rendermode InteractiveAuto  // Use for interactive components only
```

**PRINCIPLE:** All list operations MUST be paginated. Lazy loading is FORBIDDEN. Use streaming rendering for data-heavy pages.

---

## 7. CODE QUALITY STANDARDS

### 7.1 Testing Requirements (Mandatory)
```yaml
Unit Tests:
  - Coverage: Minimum 80% for business logic
  - Framework: xUnit
  - Mocking: Moq
  - Location: Tests/{Project}.Tests/

Integration Tests:
  - Coverage: All API endpoints
  - Database: In-memory or test database
  - Location: Tests/{Project}.IntegrationTests/

UI Tests:
  - Framework: bUnit for Blazor components
  - Coverage: Critical user flows
  - Test both Server and WASM render modes
```

**PRINCIPLE:** No pull request shall be merged without accompanying unit tests for new/modified code.

### 7.2 Naming Conventions
```csharp
// Classes: PascalCase
public class StudentRegistration { }

// Interfaces: IPascalCase
public interface IStudentService { }

// Methods: PascalCase
public async Task<Student> GetStudentAsync(int id)

// Variables: camelCase
var studentName = "John Doe";

// Constants: PascalCase
public const int MaxRegistrationAttempts = 3;

// Private fields: _camelCase
private readonly IStudentService _studentService;
```

### 7.3 Code Documentation
```csharp
/// <summary>
/// Validates and registers a student for an NBT session.
/// </summary>
/// <param name="registration">Student registration details including NBT number and session ID</param>
/// <returns>Registration confirmation with invoice number</returns>
/// <exception cref="ValidationException">Thrown when NBT number fails Luhn validation</exception>
public async Task<RegistrationResult> RegisterStudentAsync(StudentRegistration registration)
```

**PRINCIPLE:** All public methods MUST have XML documentation comments.

---

## 8. AUDIT LOGGING REQUIREMENTS

### 8.1 Mandatory Logging Events
```yaml
Authentication:
  - Login attempts (success/failure)
  - Logout events
  - Token refresh attempts

CRUD Operations:
  - Create: Entity type, ID, user, timestamp, data snapshot
  - Update: Entity type, ID, user, timestamp, before/after values
  - Delete: Entity type, ID, user, timestamp, data snapshot

Data Import:
  - File name, user, timestamp, row count, success/failure
  - Validation errors with row numbers
  - Duplicate detection results

Security Events:
  - Authorization failures
  - Invalid token attempts
  - Password change events
```

### 8.2 Audit Log Schema
```csharp
public class AuditLog
{
    public int Id { get; set; }
    public DateTime Timestamp { get; set; }
    public string UserId { get; set; }
    public string UserEmail { get; set; }
    public string Action { get; set; } // Create, Update, Delete, Login, Import
    public string EntityType { get; set; }
    public string EntityId { get; set; }
    public string BeforeValue { get; set; } // JSON
    public string AfterValue { get; set; }  // JSON
    public string IpAddress { get; set; }
    public string UserAgent { get; set; }
}
```

**PRINCIPLE:** ALL data modifications MUST be logged. Audit logs MUST be immutable and retained for 7 years.

---

## 9. CI/CD ENFORCEMENT

### 9.1 Build Pipeline (Mandatory Stages)
```yaml
1. Code Checkout:
   - Clean working directory
   - Fetch latest code

2. Build:
   - dotnet restore
   - dotnet build --configuration Release
   - Fail on warnings

3. Test:
   - dotnet test
   - Minimum 80% coverage
   - Zero test failures

4. Code Analysis:
   - Static analysis (Roslyn analyzers)
   - Security scanning (OWASP dependency check)
   - Code quality gates

5. Publish:
   - dotnet publish for Blazor Web App (includes Server + WASM)
   - Verify InteractiveAuto components bundle correctly
   - Generate artifacts

6. Deploy:
   - Staging environment (automatic)
   - Production environment (manual approval)
```

**PRINCIPLE:** All code MUST pass CI pipeline before merge. Failed builds block deployment.

### 9.2 Branch Protection Rules
```yaml
Main Branch:
  - Require pull request reviews (minimum 1 approval)
  - Require status checks to pass
  - Require branches to be up to date
  - No force pushes
  - No deletions

Develop Branch:
  - Require status checks to pass
  - No force pushes

Feature Branches:
  - Naming: feature/{ticket-number}-{description}
  - Must be merged via PR to develop
```

---

## 10. DATA IMPORT STANDARDS

### 10.1 Excel Import Requirements
```yaml
Supported Formats: .xlsx, .xls
Maximum File Size: 10 MB
Maximum Rows: 10,000 per import

Validation Rules:
  - Duplicate detection (NBT number + session)
  - Luhn validation for NBT numbers
  - ID number validation
  - Required field validation
  - Data type validation

Error Handling:
  - Rollback on any validation error
  - Return detailed error report with row numbers
  - Log all import attempts
```

### 10.2 Import Process (Mandatory Steps)
```csharp
1. File Upload Validation
   - Check file size
   - Check file extension
   - Virus scan (if available)

2. Data Parsing
   - Read Excel file
   - Map columns to entity properties
   - Validate headers

3. Business Validation
   - Luhn check for NBT numbers
   - ID number validation
   - Duplicate detection
   - Foreign key validation

4. Database Transaction
   - Begin transaction
   - Insert valid records
   - Commit or rollback
   - Create audit log entry

5. Response Generation
   - Success count
   - Error count
   - Detailed error list
   - Import summary
```

**PRINCIPLE:** All imports MUST be transactional (all-or-nothing). Partial imports are FORBIDDEN.

---

## 11. ERROR HANDLING

### 11.1 API Error Responses (Standardized)
```csharp
public class ApiError
{
    public string Message { get; set; }
    public string ErrorCode { get; set; }
    public Dictionary<string, string[]> ValidationErrors { get; set; }
    public DateTime Timestamp { get; set; }
    public string TraceId { get; set; }
}

// HTTP Status Codes
200 OK              - Successful GET
201 Created         - Successful POST
204 No Content      - Successful DELETE
400 Bad Request     - Validation error
401 Unauthorized    - Missing/invalid token
403 Forbidden       - Insufficient permissions
404 Not Found       - Resource doesn't exist
409 Conflict        - Duplicate resource
500 Internal Error  - Server error
```

### 11.2 Exception Handling Middleware
```csharp
// REQUIRED: Global exception handler
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        // Log exception
        // Return standardized error response
        // Never expose stack traces in production
    });
});
```

**PRINCIPLE:** Never expose internal error details to clients. All exceptions MUST be logged server-side.

---

## 12. DATABASE MANAGEMENT

### 12.1 Migration Strategy
```bash
# Development
dotnet ef migrations add MigrationName
dotnet ef database update

# Staging/Production
# Migrations applied via CI/CD pipeline
# Require explicit approval for production
```

### 12.2 Naming Conventions
```sql
Tables: PascalCase (Student, TestResult, Session)
Columns: PascalCase (NBTNumber, FirstName, DateOfBirth)
Foreign Keys: FK_{Table}_{ReferencedTable}_{Column}
Indexes: IX_{Table}_{Column}
Primary Keys: PK_{Table}
```

**PRINCIPLE:** All schema changes MUST be reversible. Down migrations MUST be tested.

---

## 13. MONITORING & OBSERVABILITY

### 13.1 Logging Requirements
```yaml
Framework: Serilog or Microsoft.Extensions.Logging
Levels:
  - Information: Application lifecycle events
  - Warning: Recoverable errors, validation failures
  - Error: Unhandled exceptions, critical failures
  - Debug: Detailed diagnostic information (dev only)

Structured Logging: JSON format with correlation IDs
Log Retention: 90 days minimum
```

### 13.2 Application Insights (Production)
```yaml
Telemetry:
  - Request/response times
  - Dependency tracking (DB, external APIs)
  - Exception tracking
  - Custom metrics (registrations/hour, imports/day)

Alerts:
  - API response time > 1 second
  - Exception rate > 5/minute
  - Database connection failures
```

**PRINCIPLE:** All production environments MUST have comprehensive logging and monitoring.

---

## 14. CONFIGURATION MANAGEMENT

### 14.1 Settings Hierarchy
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Store in Azure Key Vault"
  },
  "Jwt": {
    "SecretKey": "Store in Azure Key Vault",
    "Issuer": "Configuration",
    "Audience": "Configuration"
  },
  "Features": {
    "EnableExcelImport": true,
    "EnableAuditLogs": true
  }
}
```

### 14.2 Secret Management
```yaml
Development: User Secrets (dotnet user-secrets)
Staging/Production: Azure Key Vault or AWS Secrets Manager
FORBIDDEN: Secrets in appsettings.json or source control
```

**PRINCIPLE:** No secrets in source code. All sensitive configuration MUST use secure storage.

---

## 15. DOCUMENTATION REQUIREMENTS

### 15.1 Mandatory Documentation
```yaml
README.md:
  - Project overview
  - Setup instructions
  - Running the application
  - Testing instructions

API Documentation:
  - Swagger/OpenAPI enabled
  - All endpoints documented
  - Request/response examples

Architecture Diagrams:
  - System architecture
  - Database schema
  - Authentication flow
```

### 15.2 Code Comments
```csharp
// REQUIRED: Comments for complex business logic
// FORBIDDEN: Commented-out code (use version control)
// REQUIRED: TODO comments must have ticket numbers
// TODO: [NBT-123] Implement bulk registration feature
```

---

## 16. COMPLIANCE CHECKLIST

Before any deployment, verify:

- [ ] All code follows Clean Architecture principles
- [ ] All dependencies injected via DI
- [ ] All API endpoints have authorization attributes
- [ ] NBT numbers validated with Luhn algorithm
- [ ] ID numbers validated per SA ID rules
- [ ] HTTPS enforced, no HTTP endpoints
- [ ] Audit logs implemented for all CRUD operations
- [ ] Unit tests achieve 80% coverage minimum
- [ ] Integration tests pass for all endpoints
- [ ] WCAG 2.1 AA accessibility verified
- [ ] Page load times < 3 seconds
- [ ] API response times < 500ms
- [ ] All database queries use EF Core (no raw SQL)
- [ ] Pagination implemented for all list operations
- [ ] Error handling middleware configured
- [ ] Secrets stored in Key Vault (not config files)
- [ ] Swagger documentation complete
- [ ] CI/CD pipeline passing all stages
- [ ] Branch protection rules enforced
- [ ] Code review completed and approved

---

## 17. VIOLATION CONSEQUENCES

### 17.1 Automated Enforcement
- CI/CD pipeline WILL fail builds that violate these standards
- Pull requests WILL be blocked without passing tests
- Deployments WILL be rejected without security scans

### 17.2 Review Process
- Architecture violations MUST be corrected before merge
- Security violations REQUIRE immediate remediation
- Performance regressions MUST be investigated and resolved

---

## 18. AMENDMENT PROCESS

This constitution can only be modified through:

1. **Proposal:** Documented RFC (Request for Comments)
2. **Review:** Architecture review board approval
3. **Testing:** Impact analysis and validation
4. **Approval:** Technical lead sign-off
5. **Implementation:** Update CI/CD enforcement rules
6. **Communication:** Team notification and training

**PRINCIPLE:** This constitution is binding until formally amended through the process above.

---

## SIGNATURES

**Effective Date:** 2025-11-08  
**Constitution Version:** 1.1  
**Architecture Change:** Updated to Blazor Web App Interactive Auto (from WebAssembly)  
**Next Review Date:** 2026-11-08

---

*This constitution represents the architectural integrity and quality standards of the NBT Integrated Web Application. All contributors must read, understand, and comply with these principles.*

