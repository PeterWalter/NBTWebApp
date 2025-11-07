<!--
Sync Impact Report:
- Version change: Template → 1.0.0 (Initial constitution)
- Initial principles established
- Templates requiring updates: ⚠ pending (will be created/updated as needed)
- Follow-up TODOs: None
-->

# NBT Website Redevelopment Constitution

## Core Principles

### I. Clean Architecture (NON-NEGOTIABLE)

The application MUST adhere to clean architecture principles with clear separation of concerns:
- **Domain Layer**: Business entities and logic with zero external dependencies
- **Application Layer**: Use cases, business workflows, interfaces (contracts)
- **Infrastructure Layer**: External concerns (database, API clients, file system)
- **Presentation Layer**: Blazor Web Application (Interactive Auto) UI components and pages
- Dependencies MUST flow inward only (outer layers depend on inner, never reverse)
- All cross-layer communication MUST use dependency injection with interface abstractions
- Domain and Application layers MUST remain framework-agnostic

**Rationale**: Clean architecture ensures maintainability, testability, and technology independence, enabling long-term project sustainability.

### II. Accessibility First (NON-NEGOTIABLE)

All user interfaces MUST meet WCAG 2.1 Level AA compliance:
- Semantic HTML with proper ARIA labels and roles
- Keyboard navigation support for all interactive elements
- Minimum 4.5:1 contrast ratio for normal text, 3:1 for large text
- Screen reader compatibility verified via automated and manual testing
- Focus indicators clearly visible
- No keyboard traps
- Alternative text for all non-decorative images
- Fluent UI Blazor components MUST be configured with accessibility features enabled

**Rationale**: Accessibility is a legal requirement and ethical imperative, ensuring the NBT website is usable by all individuals regardless of ability.

### III. Responsive Design (NON-NEGOTIABLE)

The application MUST provide optimal user experience across all device sizes:
- Mobile-first design approach (320px minimum width)
- Breakpoints: Mobile (<768px), Tablet (768px-1024px), Desktop (>1024px)
- Touch-friendly targets (minimum 44x44px tap areas on mobile)
- Fluent UI Blazor responsive grid system utilized consistently
- Images and media MUST be responsive with appropriate loading strategies
- Performance budget: <3s initial load on 3G, <1s on broadband

**Rationale**: Users access services across diverse devices; responsive design ensures consistent accessibility and usability regardless of viewport size.

### IV. Security & Data Protection (NON-NEGOTIABLE)

Security MUST be embedded in every layer of the application:
- **Authentication**: ASP.NET Core Identity with JWT tokens, secure password policies
- **Authorization**: Role-based access control (RBAC) with principle of least privilege
- **Data Protection**: Encryption at rest (SQL Server TDE) and in transit (HTTPS/TLS 1.2+)
- **Input Validation**: Server-side and Client-side validation mandatory; client-side for UX only
- **SQL Injection Prevention**: Entity Framework Core parameterized queries exclusively
- **XSS Protection**: Automatic encoding in Razor; CSP headers configured
- **CORS**: Strict origin policies; no wildcard origins in production
- **Secrets Management**: Azure Key Vault or user secrets (development only); no hardcoded credentials
- **Audit Logging**: All authentication, authorization, and sensitive data access logged
- OWASP Top 10 vulnerabilities MUST be addressed in design and implementation

**Rationale**: Data breaches have severe legal, financial, and reputational consequences. Security by design is non-negotiable for protecting user data and organizational integrity. South African POPIA Act should be followed and used as the guiding standard.

### V. Code Quality & Testing (NON-NEGOTIABLE)

Code MUST meet established quality standards before merging:
- **Unit Tests**: Minimum 80% code coverage for business logic (Application and Domain layers)
- **Integration Tests**: API endpoints, database operations, external service integrations
- **End-to-End Tests**: Critical user workflows (Playwright or bUnit for Blazor)
- **Code Reviews**: All changes require peer review; no self-merge
- **Static Analysis**: Analyzers configured (StyleCop, SonarAnalyzer); zero warnings policy
- **Naming Conventions**: Microsoft C# coding standards strictly followed
- **Documentation**: XML comments for public APIs; complex logic explained inline
- **Dependency Injection**: Constructor injection preferred; service locator pattern forbidden
- **SOLID Principles**: Single Responsibility, Open/Closed, Liskov Substitution, Interface Segregation, Dependency Inversion applied

**Rationale**: High code quality reduces defects, eases maintenance, and accelerates development velocity over time through reduced technical debt.

### VI. Performance Standards

The application MUST deliver responsive user experiences:
- **API Response Times**: <200ms for data queries, <500ms for complex operations
- **Database Queries**: Indexed appropriately; no N+1 queries; execution plans reviewed
- **Blazor WASM**: Code splitting and lazy loading for large components
- **Caching Strategy**: Redis/In-Memory for frequently accessed data; cache invalidation defined
- **Asset Optimization**: Minification, compression, CDN delivery for static assets
- **Monitoring**: Application Insights or similar APM tool integrated
- Performance testing conducted for high-traffic scenarios

**Rationale**: Poor performance degrades user experience and can render an accessible, secure application effectively unusable.

### VII. API Design Excellence

ASP.NET Core Web API MUST follow REST principles and best practices:
- RESTful resource naming (plural nouns, hierarchical relationships)
- HTTP methods used semantically (GET, POST, PUT, PATCH, DELETE)
- Appropriate status codes (200, 201, 400, 401, 403, 404, 500)
- Versioning strategy defined (URL, header, or media type)
- Pagination for list endpoints (offset-limit or cursor-based)
- Filtering, sorting, searching capabilities where appropriate
- Consistent error response format with problem details (RFC 7807)
- OpenAPI/Swagger documentation auto-generated and maintained
- Rate limiting and throttling implemented

**Rationale**: Well-designed APIs ensure maintainability, client integration ease, and long-term evolvability.

## Technology Stack Requirements

**Frontend**:
- Blazor Web Application (Interactive Auto) (.NET 8+)
- Fluent UI Blazor component library or Synfusion Blazor components
- C# 12+ language features

**Backend**:
- ASP.NET Core Web API (.NET 8+)
- Entity Framework Core for data access
- AutoMapper for DTO mapping
- FluentValidation for input validation

**Database**:
- Microsoft SQL Server (2019+)
- Entity Framework Core migrations for schema management

**DevOps**:
- Git version control and Github
- CI/CD pipeline ( GitHub Actions)
- Automated testing in pipeline
- Environment-specific configurations

All technology choices MUST align with Microsoft best practices and long-term support commitments.

## Quality Gates

Before any code reaches production, it MUST pass:

1. **Build Gate**: Clean build with zero errors and zero warnings
2. **Test Gate**: All automated tests pass (unit, integration, E2E)
3. **Security Gate**: No critical/high vulnerabilities in dependencies (Dependabot, Snyk)
4. **Code Review Gate**: Approved by at least one qualified reviewer
5. **Accessibility Gate**: Automated accessibility tests pass (axe-core, Pa11y)
6. **Performance Gate**: Performance budgets met in staging environment

Deployment to production requires explicit approval after successful staging validation.

## Governance

This constitution establishes the non-negotiable standards for the NBT website redevelopment project. All design decisions, code contributions, and architectural changes MUST comply with these principles.

**Amendment Process**:
- Constitution changes require documented justification and team consensus
- Version increments follow semantic versioning (MAJOR.MINOR.PATCH)
- All amendments include migration path for existing code if applicable

**Compliance**:
- Pull requests MUST reference relevant principles in description
- Code reviews MUST verify constitutional compliance
- Regular audits conducted to ensure ongoing adherence
- Violations addressed immediately with corrective action plan

**Conflict Resolution**:
- When principles appear to conflict, security and accessibility take precedence
- Technical debt MUST be justified, documented, and tracked with remediation plan
- Complexity MUST provide measurable value; simplicity preferred when equally effective

**Version**: 1.0.0 | **Ratified**: 2025-06-07 | **Last Amended**: 2025-06-07
