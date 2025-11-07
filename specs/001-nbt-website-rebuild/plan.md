# Implementation Plan: NBT Website Rebuild

**Feature**: 001-nbt-website-rebuild  
**Created**: 2025-01-07  
**Status**: Ready for Implementation

---

## 1. Technical Architecture

### Technology Stack

**Frontend**: Blazor Web Application (Interactive Auto) with .NET 8, Fluent UI Blazor v4.x  
**Backend**: ASP.NET Core Web API 8.0  
**Database**: Microsoft SQL Server 2019+  
**Authentication**: ASP.NET Core Identity with JWT tokens  
**Testing**: xUnit, bUnit, Playwright  
**CI/CD**: GitHub Actions, Azure App Service

### Clean Architecture Layers

```
NBT.Domain → NBT.Application → NBT.Infrastructure
                                      ↓
                              NBT.WebAPI + NBT.WebUI
```

**Projects**:
1. **NBT.Domain**: Entities, Enums, Exceptions (no dependencies)
2. **NBT.Application**: DTOs, Interfaces, Use Cases, Validators
3. **NBT.Infrastructure**: DbContext, Repositories, Email, File Storage
4. **NBT.WebAPI**: Controllers, Middleware, Program.cs
5. **NBT.WebUI**: Blazor pages, Components, Services

### Key Entities

- ContentPage (slug, title, body, meta)
- Announcement (title, content, category, featured)
- ContactInquiry (name, email, subject, message, status)
- User (Identity user with roles: Admin, InstitutionalUser, Staff)
- DownloadableResource (title, file, category)

### API Endpoints

**Base URL**: `/api/v1/`

- `GET /contentpages/{slug}` - Get page by slug
- `GET /announcements?category&page&size` - List announcements
- `GET /announcements/featured` - Get featured announcements
- `POST /contact/inquiries` - Submit contact form
- `POST /auth/login` - User authentication
- `POST /auth/logout` - User logout
- `GET /resources?category&page` - List downloadable resources

---

## 2. Constitution Compliance

✅ **Clean Architecture**: 4-layer separation, dependency inversion, DI throughout  
✅ **Accessibility**: Fluent UI with WCAG 2.1 AA, automated testing with axe-core  
✅ **Responsive**: Mobile-first, breakpoints <768px, 768-1024px, >1024px  
✅ **Security**: HTTPS, Identity, EF Core parameterized queries, FluentValidation, CORS  
✅ **Testing**: 80%+ coverage, xUnit, bUnit, Playwright E2E  
✅ **Performance**: Blazor Interactive Auto (pre-rendering), CDN, caching, monitoring  
✅ **API Design**: REST, proper status codes, versioning, Swagger, pagination

---

## 3. Implementation Phases

### Phase 1: Project Setup (Week 1-2)

**Goal**: Establish solution structure and infrastructure

**Tasks**:
1. Create solution in Visual Studio 2022 with 5 projects (Domain, Application, Infrastructure, WebAPI, WebUI)
2. Add NuGet packages: EF Core, Identity, FluentValidation, AutoMapper, Serilog, Fluent UI
3. Define domain entities and enums
4. Create ApplicationDbContext with entity configurations
5. Generate initial EF Core migration
6. Set up GitHub Actions CI/CD pipeline
7. Provision Azure Dev environment (App Service, SQL Database, Key Vault)

**Deliverables**: Clean architecture solution, Database schema, CI/CD functional

---

### Phase 2: Empty Website Shell (Week 2-3)

**Goal**: Create navigable website skeleton

**Tasks**:
1. Configure Blazor Web Application (Interactive Auto) with Fluent UI
2. Create MainLayout with header, navigation, footer
3. Implement NavMenu (desktop) and MobileNavMenu (hamburger)
4. Create empty pages: Index, About, Applicants, Educators, Institutions, WhatsNew, Contact, Login
5. Add responsive breakpoints and theme customization
6. Implement PageHero and LoadingSpinner components
7. Deploy to Dev environment

**Deliverables**: Empty website with all pages, Responsive navigation, Deployed

---

### Phase 3: Website Pages Development (Week 3-6)

**Goal**: Implement all page content and forms

**Tasks**:

**Landing Page (Day 13-15)**:
- Hero section with NBT branding
- Quick links to main sections
- Featured announcements widget (placeholder)
- "What's New" highlights

**About Page (Day 15-16)**:
- History, Mission, Structure sections
- Rich text content with images

**Applicants Page (Day 16-18)**:
- FAQ accordion component
- Test information and registration guidance
- Cost information table
- Mobile-optimized layout

**Educators Page (Day 18-19)**:
- Resources download section
- Resource request form
- Group registration guidance

**Institutions Page (Day 19-20)**:
- Institutional benefits
- Score interpretation guide
- Prominent login link

**What's New Page (Day 20-22)**:
- Announcement list with filtering (client-side)
- AnnouncementCard component
- Pagination

**Contact Page (Day 22-24)**:
- Contact form with validation
- Character counter (1000 max)
- Office info and map
- Success/error messaging

**Login Page (Day 24-25)**:
- Login form with validation
- "Forgot Password" link
- "Remember Me" checkbox

**Deliverables**: All pages with content, Forms functional, WCAG 2.1 AA compliant

---

### Phase 4: Database Development (Week 6-7)

**Goal**: Finalize schema and create seed data

**Tasks**:
1. Refine entity configurations (indexes, constraints)
2. Create seed data for ContentPages (About, policies)
3. Create seed data for Announcements (10-15 samples)
4. Create seed data for Users (admin, test users)
5. Create seed data for DownloadableResources (5-10 samples)
6. Implement ApplicationDbContextSeed.cs
7. Test data integrity and relationships

**Deliverables**: Production-ready schema, Comprehensive seed data

---

### Phase 5: API Development (Week 7-9)

**Goal**: Build complete REST API

**Application Layer (Day 32-36)**:
- Queries: GetContentPageQuery, GetAnnouncementsQuery, GetResourcesQuery
- Commands: CreateContactInquiryCommand, LoginCommand, ResetPasswordCommand
- DTOs and AutoMapper profiles
- FluentValidation validators

**Infrastructure Layer (Day 36-38)**:
- EmailService (SendGrid integration)
- FileStorageService (Azure Blob Storage)
- CurrentUserService
- Serilog with Application Insights

**WebAPI Layer (Day 38-44)**:
- ContentPagesController, AnnouncementsController, ContactController
- AuthenticationController, ResourcesController
- ExceptionHandlingMiddleware, ValidationFilter
- ASP.NET Core Identity configuration
- JWT token generation and validation
- Swagger/OpenAPI documentation

**Deliverables**: Fully functional API, Authentication working, Swagger docs

---

### Phase 6: API Integration (Week 9-10)

**Goal**: Connect frontend to backend

**Tasks**:
1. Create ApiClient service with HttpClient
2. Implement AuthStateProvider for login/logout
3. Connect Index page to featured announcements API
4. Connect WhatsNew page to announcements list API
5. Connect Contact form to inquiry submission API
6. Connect Login page to authentication API
7. Implement loading spinners and error handling
8. Add CAPTCHA/honeypot to contact form
9. Test end-to-end workflows

**Deliverables**: Fully integrated application, Authentication end-to-end

---

### Phase 7: Testing & QA (Week 10-12)

**Goal**: Comprehensive testing and bug fixing

**Unit Tests (Day 55-58)**:
- Domain entities, Application use cases, Validators
- Target: 80%+ coverage

**Component Tests (Day 58-60)**:
- bUnit tests for Blazor components
- Navigation, forms, announcement cards

**Integration Tests (Day 60-62)**:
- API endpoints, database operations
- Authentication flows

**E2E Tests (Day 62-65)**:
- Playwright tests for critical journeys
- Contact form submission, login/logout, navigation
- Responsive layouts, cross-browser

**Accessibility (Day 65-67)**:
- Automated testing (axe-core)
- Manual keyboard navigation
- Screen reader testing (NVDA/JAWS)
- Fix all WCAG 2.1 AA violations

**Security (Day 67-69)**:
- OWASP ZAP scan
- SQL injection, XSS, CSRF testing
- Verify HTTPS, security headers
- Fix critical/high vulnerabilities

**Performance (Day 69-71)**:
- Lighthouse audits
- Core Web Vitals measurement
- 3G load time testing
- Database query profiling
- Load test (1000+ concurrent users)

**UAT & Bug Fixing (Day 71-78)**:
- Deploy to Staging
- Stakeholder testing
- Fix all critical and high bugs
- Regression testing

**Deliverables**: 80%+ coverage, Zero WCAG violations, Zero critical bugs, Performance >85

---

### Phase 8: Deployment & Launch (Week 12-13)

**Goal**: Production launch

**Tasks**:
1. Provision production Azure resources (P1V2 App Service, S2 SQL Database)
2. Configure Azure Key Vault, CDN, Application Insights
3. Set up SSL certificate for nbt.ac.za
4. Deploy API and frontend to production
5. Run database migrations
6. Seed production database
7. Smoke testing (all pages, forms, login)
8. Configure monitoring alerts (error rate, response time)
9. Update DNS to point to new website
10. Monitor traffic and errors
11. Post-launch support (Day 86-90)

**Deliverables**: Website live at nbt.ac.za, Monitoring configured, Support ready

---

## 4. Fluent UI Configuration

**Program.cs** (NBT.WebUI):
```csharp
builder.Services.AddFluentUIComponents();

// Custom theme
builder.Services.Configure<FluentUIOptions>(options =>
{
    options.Theme = new FluentUITheme
    {
        PrimaryColor = "#003366",  // NBT brand blue
        BackgroundColor = "#FFFFFF",
        TextColor = "#1A1A1A"
    };
});
```

**CSS Structure**:
- `app.css` - Custom application styles
- `fluent-overrides.css` - Fluent UI customizations
- Responsive breakpoints: 768px, 1024px

---

## 5. Entity Framework Setup

**ApplicationDbContext**:
```csharp
public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public DbSet<ContentPage> ContentPages { get; set; }
    public DbSet<Announcement> Announcements { get; set; }
    public DbSet<ContactInquiry> ContactInquiries { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<DownloadableResource> Resources { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
```

**Migration Commands**:
```bash
# Add migration
dotnet ef migrations add InitialCreate --project src/NBT.Infrastructure --startup-project src/NBT.WebAPI

# Update database
dotnet ef database update --project src/NBT.Infrastructure --startup-project src/NBT.WebAPI
```

---

## 6. Authentication Setup

**JWT Configuration**:
```csharp
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["Jwt:Issuer"],
            ValidAudience = configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
        };
    });
```

**Roles**: Admin, InstitutionalUser, Staff  
**Session Timeout**: 30 minutes inactivity  
**Failed Login Limit**: 5 attempts, 15-minute lockout

---

## 7. CI/CD Pipeline

**GitHub Actions** (.github/workflows/ci.yml):

```yaml
name: CI/CD

on:
  push:
    branches: [ main, develop, 001-nbt-website-rebuild ]
  pull_request:
    branches: [ main, develop ]

jobs:
  build-test:
    runs-on: ubuntu-latest
    steps:
    - uses: actions/checkout@v3
    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: '8.0.x'
    - name: Restore
      run: dotnet restore
    - name: Build
      run: dotnet build --no-restore --configuration Release
    - name: Test
      run: dotnet test --no-build --configuration Release
    - name: Code Coverage
      run: dotnet test /p:CollectCoverage=true

  deploy-dev:
    needs: build-test
    if: github.ref == 'refs/heads/develop'
    runs-on: ubuntu-latest
    steps:
    - name: Deploy to Dev
      run: az webapp deploy --resource-group NBT-Dev --name nbt-website-dev
```

---

## 8. Testing Strategy

**Unit Tests**: Domain, Application (80%+ coverage)  
**Component Tests**: Blazor components (bUnit)  
**Integration Tests**: API endpoints, database  
**E2E Tests**: Critical paths (Playwright)  
**Accessibility**: Automated (axe-core) + manual  
**Security**: OWASP ZAP, dependency scanning  
**Performance**: Lighthouse, load testing

---

## 9. Deployment Environments

**Development**:
- URL: https://nbt-website-dev.azurewebsites.net
- Azure App Service B1, SQL Basic
- Auto-deploy from `develop` branch

**Staging**:
- URL: https://nbt-website-staging.azurewebsites.net
- Azure App Service S1, SQL Standard S0
- Manual deployment for UAT

**Production**:
- URL: https://www.nbt.ac.za
- Azure App Service P1V2, SQL Standard S2
- Manual deployment with approval
- CDN: Azure CDN
- WAF: Azure Application Gateway

---

## 10. Success Metrics

**Technical**:
- Code coverage ≥80%
- API response <200ms (95%)
- Page load <3s on 3G
- Lighthouse score ≥85

**Accessibility**:
- Zero WCAG 2.1 AA violations
- Keyboard navigation functional
- Screen reader compatible

**Security**:
- Zero critical/high vulnerabilities
- HTTPS enforced
- OWASP Top 10 addressed

**Performance**:
- Core Web Vitals "Good"
- 1,000+ concurrent users supported
- 99.5% uptime

---

## 11. Risk Management

| Risk | Likelihood | Impact | Mitigation |
|------|-----------|--------|------------|
| Blazor performance issues | Medium | High | Pre-rendering, lazy loading, optimization |
| Timeline delays | Medium | Medium | Buffer time, prioritize MVP |
| Security vulnerabilities | Low | Critical | Security testing throughout, regular updates |
| Content migration delays | Medium | Medium | Start early, use placeholders |

---

## 12. Post-Launch Roadmap

**3-6 months**: CMS, Advanced analytics, Search functionality  
**6-12 months**: Online registration, Results portal, Institutional dashboard  
**12+ months**: Multilingual support, Mobile apps, Public API

---

## Summary

**Timeline**: 13 weeks (3 months)  
**Effort**: 90 developer-days  
**Phases**: 8 phases from setup to launch  
**Status**: ✅ APPROVED - Ready for Implementation

**Next Action**: Begin Phase 1 - Project Setup & Infrastructure
