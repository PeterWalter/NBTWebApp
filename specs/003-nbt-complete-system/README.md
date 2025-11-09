# NBT Integrated Web Application - Complete System Specification

## 📋 Overview
This specification defines the complete implementation of the National Benchmark Tests (NBT) Integrated Web Application. It extends the existing Blazor WebAssembly + ASP.NET Core Web API shell project to deliver a fully functional, production-ready system.

## 🎯 Purpose
To provide a comprehensive blueprint for implementing all missing features, services, and components required to deliver an end-to-end digital platform for NBT test administration, from student registration through test booking, payment, results delivery, and administrative management.

## 📚 Documentation Structure

### 1. [CONSTITUTION.md](./CONSTITUTION.md)
**Non-Negotiable Principles & Standards**
- Architecture standards (Clean Architecture, .NET 9.0, Fluent UI)
- Security requirements (HTTPS, JWT, RBAC, audit logging)
- Data validation rules (NBT number, SA ID, Foreign ID)
- Performance standards (< 3s load, < 500ms API response)
- Accessibility (WCAG 2.1 AA compliance)
- Testing requirements (80%+ coverage)
- CI/CD requirements
- Business rules enforcement
- Code standards and conventions

**When to use**: Reference this document to understand what is mandatory and cannot be compromised.

### 2. [SPECIFICATION.md](./SPECIFICATION.md)
**Complete Functional Specification**
- User workflows (Students, Staff, Admin, SuperUser)
- Functional modules in detail
- Registration wizard (3-step process)
- Booking and payment system
- Results management with barcodes
- Venue and test session management
- Reporting and analytics
- Landing page and public content
- API endpoint definitions
- Data models overview
- Integration points (EasyPay, Email/SMS)
- Quality requirements
- Success criteria

**When to use**: Reference this document to understand WHAT needs to be built and HOW it should work.

### 3. [PLAN.md](./PLAN.md)
**Implementation Plan**
- Phase 0: Shell Audit & Gap Analysis
- Phase 1: Foundation & Infrastructure
- Phase 2: Registration Wizard
- Phase 3: Booking & Payment
- Phase 4: Staff/Admin Dashboards
- Phase 5: Venue Management
- Phase 6: Results Management
- Phase 7: Reporting & Analytics
- Phase 8: Landing Page & Public Content
- Phase 9: Fluent UI Migration
- Phase 10: Special Features & Polish
- Phase 11: Testing & QA
- Phase 12: CI/CD & Deployment
- Phase 13: User Acceptance Testing
- Phase 14: Go-Live Preparation
- Timeline estimates
- Risk mitigation

**When to use**: Reference this document to understand the implementation sequence and approach.

### 4. [CONTRACTS.md](./CONTRACTS.md)
**Data Contracts & API Schemas**
- Complete entity definitions with all properties
- Enumerations (Gender, Ethnicity, VenueType, etc.)
- DTO (Data Transfer Object) definitions
- API endpoint specifications
- Request/response schemas
- Validation rules
- JSON schema examples
- Error response formats

**When to use**: Reference this document when implementing entities, DTOs, or API endpoints.

### 5. [TASKS.md](./TASKS.md)
**Detailed Task Breakdown**
- 140+ granular tasks organized by phase
- Priority levels (P0-P3)
- Effort estimates (S, M, L, XL)
- Dependencies between tasks
- Acceptance criteria for each task
- Status tracking
- Component lists
- API endpoint lists

**When to use**: Reference this document for day-to-day development planning and tracking.

## 🚀 Quick Start

### For Developers
1. Start with **CONSTITUTION.md** to understand the rules
2. Review **SPECIFICATION.md** to understand the system
3. Follow **PLAN.md** for implementation sequence
4. Use **CONTRACTS.md** as reference during coding
5. Track progress with **TASKS.md**

### For Project Managers
1. Review **SPECIFICATION.md** for scope
2. Use **PLAN.md** for timeline planning
3. Track **TASKS.md** for progress monitoring
4. Reference **CONSTITUTION.md** for quality gates

### For Architects
1. Study **CONSTITUTION.md** for architectural constraints
2. Review **SPECIFICATION.md** for system design
3. Use **CONTRACTS.md** for data modeling
4. Follow **PLAN.md** for implementation phases

## 🎨 System Overview

### Technology Stack
- **Frontend**: Blazor WebAssembly with Fluent UI (replacing MudBlazor)
- **Backend**: ASP.NET Core 9.0 Web API
- **Database**: MS SQL Server with EF Core 9.0
- **Authentication**: JWT with refresh tokens
- **Authorization**: Role-based (Student, Staff, Admin, SuperUser)
- **Architecture**: Clean Architecture (Domain, Application, Infrastructure, API, Client)

### Key Features
- **Student Registration Wizard**: 3-step progressive registration with auto-save
- **NBT Number Generation**: 14-digit Luhn-validated unique identifier
- **SA ID Validation**: With DOB and gender extraction
- **Foreign ID/Passport Support**: For non-SA applicants
- **Booking System**: With business rules (1 active, 2/year max, 3-year validity)
- **Payment Processing**: Installments, EasyPay integration, bank file uploads
- **Results Management**: With barcodes, performance levels, PDF certificates
- **Venue Management**: Multiple types, date-based availability
- **Staff/Admin Dashboards**: Comprehensive CRUD operations
- **Reporting**: Excel/PDF exports, custom reports
- **Public Website**: Landing page with menus matching current NBT site

### Business Rules Highlights
- **Registration**: Resume from interruption, auto-save progress
- **Booking**: One active booking, can book next after closing date, 2 tests/year max
- **Payment**: Installments allowed, paid by test order, costs vary by intake year
- **Results**: Unique barcode per test, performance levels, certificate only for paid tests
- **Test Sessions**: Linked to venues (not rooms), Sunday and Online tests highlighted
- **Venues**: Types (National, Special, Research, Other), date-based availability

## 📊 Implementation Phases

```
Phase 0: Audit (1 week)
↓
Phase 1: Foundation (1 week)
↓
Phase 2: Registration (1 week)
↓
Phase 3: Booking & Payment (1 week)
↓
Phase 4: Dashboards (1 week)
↓
Phase 5-7: Venue, Results, Reports (3 weeks)
↓
Phase 8-10: Landing, Migration, Polish (3 weeks)
↓
Phase 11-12: Testing & CI/CD (2 weeks)
↓
Phase 13-14: UAT & Go-Live (2 weeks)
```

**Total Estimated Time**: 12-16 weeks (with buffer)

## ✅ Success Criteria
- [ ] All workflows operational end-to-end
- [ ] Zero critical bugs in production
- [ ] 80%+ test coverage achieved
- [ ] < 3s page load time validated
- [ ] WCAG 2.1 AA compliance achieved
- [ ] 1000+ concurrent users supported
- [ ] Successful UAT completion
- [ ] Production deployment successful

## 📞 Key Contacts
- **Project Owner**: [TBD]
- **Lead Developer**: [TBD]
- **Technical Architect**: [TBD]
- **QA Lead**: [TBD]
- **Business Analyst**: [TBD]

## 📅 Version History
| Version | Date | Author | Description |
|---------|------|--------|-------------|
| 1.0 | 2025-11-09 | AI Assistant | Initial comprehensive specification created |

## 🔗 Related Documents
- [Current NBT Website](https://nbt.ac.za) - Reference for public content structure
- [Project Repository](https://github.com/[org]/NBTWebApp) - Source code
- [Azure DevOps](https://dev.azure.com/[org]/NBTWebApp) - Project tracking (if applicable)

## 📝 Notes
- This specification consolidates all previous requirements and updates
- All MudBlazor components must be replaced with Fluent UI
- Test sessions are linked to Venues, not Rooms
- Registration wizard must be 3 steps (not more)
- Payment installments are supported
- Results include barcodes to distinguish multiple tests
- Special/remote sessions supported
- Landing page must match current NBT website structure

## 🤝 Contributing
When implementing features:
1. Always reference CONSTITUTION.md for rules
2. Check SPECIFICATION.md for functional requirements
3. Follow PLAN.md for sequence
4. Use CONTRACTS.md for data structures
5. Update TASKS.md with progress

## ⚖️ License
[Specify license for NBT application]

---

**Last Updated**: 2025-11-09  
**Specification Version**: 1.0  
**Status**: Ready for Implementation
