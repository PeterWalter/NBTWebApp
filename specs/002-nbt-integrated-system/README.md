# NBT Integrated System - Complete Specification Suite

**Feature**: 002-nbt-integrated-system  
**Created**: 2025-11-08  
**Status**: ✅ READY FOR IMPLEMENTATION  
**Version**: 1.0

---

## 📚 DOCUMENTATION INDEX

This folder contains the **complete specification suite** for implementing the NBT Integrated Web Application, extending the existing project shell to a fully functional system.

### Core Documents (4)

| Document | Purpose | Size | Status |
|----------|---------|------|--------|
| **[contracts.md](./contracts.md)** | Data contracts, API schemas, entity definitions | 31,870 chars | ✅ Complete |
| **[plan.md](./plan.md)** | 12-week implementation roadmap with phases | 38,495 chars | ✅ Complete |
| **[review.md](./review.md)** | Shell audit, gap analysis, compliance check | 24,807 chars | ✅ Complete |
| **[tasks.md](./tasks.md)** | Granular task breakdown (485 tasks) | 39,482 chars | ✅ Complete |

### Quick Reference Documents (2)

| Document | Purpose | Size | Status |
|----------|---------|------|--------|
| **[quickstart.md](./quickstart.md)** | Developer onboarding guide (5-20 min setup) | 23,566 chars | ✅ Complete |
| **[README.md](./README.md)** | This file - Navigation and overview | 8,200 chars | ✅ Complete |

**Total Documentation**: ~166,420 characters (166KB)

---

## 🚀 QUICK START FOR DEVELOPERS

### Prerequisites Checklist
- [ ] .NET 9 SDK installed
- [ ] SQL Server 2019+ accessible
- [ ] Git installed
- [ ] 15-20 minutes for setup

### Setup Steps (5 minutes)
```bash
# 1. Clone and restore (2 min)
git clone <repo-url>
cd NBTWebApp
dotnet restore

# 2. Database setup (2 min)
cd src/NBT.WebAPI
dotnet ef database update

# 3. Run applications (1 min)
# Terminal 1: API
cd src/NBT.WebAPI
dotnet run

# Terminal 2: Blazor UI
cd src/NBT.WebUI
dotnet run

# 4. Verify (30 sec)
# Open: https://localhost:5003
# Login: admin@nbt.ac.za / Admin@123
```

**📖 Full Instructions**: See [quickstart.md](./quickstart.md)

---

## 📋 PROJECT STATUS

### Current State (Shell Audit Results)

| Category | Completion | Status | Priority |
|----------|------------|--------|----------|
| **Architecture** | 100% | ✅ EXCELLENT | - |
| **Domain Layer** | 40% | 🟡 PARTIAL | CRITICAL |
| **Application Layer** | 15% | 🔴 CRITICAL | CRITICAL |
| **API Layer** | 10% | 🔴 CRITICAL | CRITICAL |
| **UI Layer** | 25% | 🟡 PARTIAL | HIGH |
| **Infrastructure** | 80% | 🟢 GOOD | MEDIUM |
| **Security** | 90% | 🟢 GOOD | LOW |
| **Testing** | 0% | 🔴 CRITICAL | CRITICAL |
| **Overall** | **40%** | 🟡 PARTIAL | - |

### What's Working (40%)

✅ **6 Entities**: User, Announcement, ContentPage, ContactInquiry, DownloadableResource, SystemSetting  
✅ **6 Services**: Authentication, Announcements, ContentPages, ContactInquiries, Resources  
✅ **26 API Endpoints**: Auth (3), Announcements (5), ContentPages (5), ContactInquiries (4), Resources (5), SystemSettings (4)  
✅ **13 Blazor Pages**: 7 public + 6 admin pages  
✅ **JWT Authentication**: Fully configured  
✅ **Fluent UI**: Integrated and themed  
✅ **Clean Architecture**: Correctly implemented  
✅ **Database**: SQL Server with EF Core 9  

### What's Missing (60%)

❌ **9 Core Entities**: Student, Registration, Payment, TestSession, Venue, Room, RoomAllocation, TestResult, AuditLog  
❌ **2 Value Objects**: NBTNumber (14-digit Luhn), SAIDNumber (SA ID validation)  
❌ **12 Services**: StudentService, RegistrationService, PaymentService, EasyPayService, etc.  
❌ **64 API Endpoints**: Students (9), Registrations (7), Booking (4), Payments (7), Venues (10), Sessions (8), Results (6), Staff (5), Reports (8)  
❌ **25 UI Pages**: Registration wizard, Admin CRUD pages, Staff dashboard  
❌ **Student Workflows**: Account creation, booking limits (1 active, 2/year), test validity (3 years), pre-test questionnaire  
❌ **Notifications**: Email/SMS for registration, payment, test reminders, results  
❌ **Test Suite**: 0% coverage (need 80%+)  
❌ **Audit Logging**: Not implemented (Constitution violation)

---

## 👨‍🎓 STUDENT ACTIVITIES & WORKFLOWS

### Complete Student Journey

Students (applicants/writers) interact with the NBT Web Application through the following activities:

#### 1. Account Creation & Login
- **Register** new account or **sign in**
- Duplicate prevention via SA ID number
- OTP verification for secure onboarding
- Account remains active for future test cycles

#### 2. NBT Number Generation
- **Automatic generation** upon successful registration
- **Format**: 14-digit YYYYSSSSSSSSSC (Year + Sequence + Luhn checksum)
- **Unique identifier** linking all bookings, payments, and results
- **Validation**: Must pass Luhn (modulus-10) algorithm

#### 3. Registration Wizard (Multi-Step)
- **Step 1**: Personal details (SA ID, contact info)
- **Step 2**: Academic background (school, grade)
- **Step 3**: Test preferences and venue selection
- **Step 4**: Special accommodation requests
- **Auto-save**: Progress saved automatically at each step
- **Resumable**: Can continue later if interrupted

#### 4. Booking & Payment (Critical Business Rules)

**Test Types**:
- **AQL**: Academic & Quantitative Literacy
- **MAT**: Mathematics
- Can select one or both tests

**Booking Rules** (MANDATORY ENFORCEMENT):
- 📅 Bookings open from **1 April** each year (Year Intake start)
- 🔒 **ONE ACTIVE BOOKING LIMIT**: Student can only book one test at a time
- ♻️ **Rebooking Allowed**: Can book another test only after closing date of current booking passes
- 📊 **Annual Limit**: Maximum **2 tests per calendar year**
- ⏰ **Test Validity**: Tests valid for **3 years** from date of booking
- ✏️ **Modification Allowed**: Student can change booking **before close of booking date**

**Payment Processing**:
- EasyPay payment reference generated automatically
- Payment status tracked via webhook integration
- Email confirmation sent after successful payment

#### 5. Special or Remote Sessions
- **Off-site testing** requires special form completion
- Invigilator and remote venue details required
- Automatically routed to **NBT remote administration team**
- Additional verification and approval workflow

#### 6. Pre-Test Questionnaire
- **Completed online** after registration
- Background information for research and equity reporting
- **Mandatory** before test participation
- Data supports NBT research initiatives

#### 7. Results Access
- **Secure login** required to view results
- View and **download** AQL and MAT results once released
- **Performance band** and **percentile** displayed
- Results available for **3 years** from test date

#### 8. Profile Management
- Update **personal** or **academic** details
- Upload supporting documents (if required)
- **Password reset** functionality
- All edits **logged for audit tracking**

#### 9. Automated Notifications
Email/SMS alerts sent for:
- ✅ Registration confirmation
- ✅ Payment confirmation
- ✅ Test reminders (7 days, 1 day before)
- ✅ Result availability
- ✅ Booking modifications
- ✅ Account security events

#### 10. Account Retention
- Accounts remain **active indefinitely**
- Historical data preserved for reporting
- Access to **past results and bookings**
- Continuity for **multiple test cycles**

### Business Rule Summary

| Rule | Constraint | Enforcement |
|------|-----------|-------------|
| Active Bookings | 1 per student | Database + API |
| Annual Tests | 2 per year | Service layer |
| Test Validity | 3 years | Calculated field |
| Booking Modification | Before close date | Date validation |
| NBT Number Format | 14 digits (Luhn) | Domain validation |
| SA ID Format | 13 digits (Luhn) | Domain validation |

---

## 🎯 IMPLEMENTATION ROADMAP

### Phase Overview (12 Weeks)

| Phase | Week | Hours | Priority | Focus |
|-------|------|-------|----------|-------|
| **Phase 0** | Pre-work | 8 | ✅ DONE | Shell audit |
| **Phase 1** | Week 1 | 40 | 🔴 CRITICAL | Foundation & entities |
| **Phase 2** | Week 2 | 40 | 🔴 CRITICAL | Student management |
| **Phase 3** | Week 3-4 | 80 | 🔴 CRITICAL | Registration wizard |
| **Phase 4** | Week 5 | 40 | 🔴 CRITICAL | Payment (EasyPay) |
| **Phase 5** | Week 6 | 40 | 🟡 HIGH | Venues & rooms |
| **Phase 6** | Week 7 | 40 | 🟡 HIGH | Test sessions |
| **Phase 7** | Week 8 | 40 | 🟡 HIGH | Results import |
| **Phase 8** | Week 9 | 40 | 🟡 MEDIUM | Dashboards |
| **Phase 9** | Week 10 | 40 | 🟡 MEDIUM | Reports & analytics |
| **Phase 10** | Week 11-12 | 120 | 🔴 CRITICAL | Testing & deployment |
| **TOTAL** | 12 weeks | **580** | | |

### Critical Path Dependencies

```
Phase 1 (Foundation)
    ↓
Phase 2 (Students) ──→ Phase 3 (Registration) ──→ Phase 4 (Payments)
    ↓                       ↓                          ↓
Phase 5 (Venues) ──→ Phase 6 (Sessions) ──────────→ Phase 7 (Results)
    ↓                       ↓                          ↓
    └──────────────────→ Phase 8 (Dashboards) ←───────┘
                            ↓
                       Phase 9 (Reports)
                            ↓
                       Phase 10 (Testing & Deploy) 🚀
```

**📖 Full Roadmap**: See [plan.md](./plan.md)

---

## 📖 HOW TO USE THIS DOCUMENTATION

### For Project Managers

1. **Start with**: [review.md](./review.md) - Understand current state and gaps
2. **Then read**: [plan.md](./plan.md) - Review timeline and resource allocation
3. **Track progress**: [tasks.md](./tasks.md) - Monitor 485 individual tasks
4. **Constitution check**: [review.md - Section 14](./review.md#14-constitution-compliance-checklist) - 55% → 100%

### For Developers

1. **Start with**: [quickstart.md](./quickstart.md) - Get environment running (15 min)
2. **Then review**: [contracts.md](./contracts.md) - Understand data structures and APIs
3. **Pick tasks from**: [tasks.md](./tasks.md) - Start with Phase 1, Task T016
4. **Reference**: [plan.md](./plan.md) - See how your tasks fit the big picture

### For Architects

1. **Start with**: [contracts.md](./contracts.md) - Review entity relationships and API design
2. **Then read**: [review.md - Section 1](./review.md#1-architecture-integrity-review) - Architecture assessment
3. **Validate**: [plan.md - Section 2](./plan.md#2-architecture-overview) - Solution structure
4. **Constitution**: Ensure all designs comply with Constitution.md

### For QA Engineers

1. **Start with**: [plan.md - Section 5](./plan.md#5-testing-strategy) - Testing strategy
2. **Then review**: [tasks.md - Phase 10](./tasks.md#-testing-security--deployment) - 126 test tasks
3. **Track coverage**: Target 80% (300+ tests)
4. **Security**: [review.md - Section 7](./review.md#7-security-review) - Security audit findings

---

## 🔑 KEY DELIVERABLES BY PHASE

### Phase 1: Foundation (Week 1)
- ✅ 9 new entities with relationships
- ✅ 2 value objects (NBTNumber, SAIDNumber) with Luhn validation
- ✅ 5 new enums
- ✅ 9 EF Core configurations
- ✅ Database migration (9 new tables)
- ✅ 35+ unit tests

### Phase 2: Student Module (Week 2)
- ✅ Student CRUD service
- ✅ NBT number generator
- ✅ 9 API endpoints
- ✅ 3 admin pages (Index, Create, Edit)
- ✅ Search and filtering
- ✅ 30+ tests

### Phase 3: Registration (Week 3-4)
- ✅ 4-step registration wizard
- ✅ Session selection
- ✅ Capacity management
- ✅ 11 API endpoints
- ✅ 40+ tests

### Phase 4: Payments (Week 5)
- ✅ EasyPay integration
- ✅ Payment callback handling
- ✅ Invoice generation (PDF)
- ✅ 7 API endpoints
- ✅ HMAC signature verification
- ✅ 25+ tests

### Phase 10: Production (Week 11-12)
- ✅ 80%+ test coverage
- ✅ Zero critical vulnerabilities
- ✅ <3s page load time
- ✅ Azure deployment
- ✅ Monitoring configured
- ✅ **GO LIVE** 🚀

---

## 📊 METRICS & KPIs

### Code Quality Metrics

| Metric | Current | Target | Status |
|--------|---------|--------|--------|
| Domain Entities | 6 | 15 | 🔴 40% |
| Services | 6 | 18 | 🔴 33% |
| API Endpoints | 26 | 90 | 🔴 29% |
| UI Pages | 13 | 38 | 🟡 34% |
| Test Coverage | 0% | 80% | 🔴 0% |
| Code Documentation | 60% | 90% | 🟡 60% |

### Constitution Compliance

| Requirement | Section | Status | Notes |
|-------------|---------|--------|-------|
| Clean Architecture | 3.1 | ✅ PASS | Correctly implemented |
| JWT Authentication | 4.2 | ✅ PASS | RS256 configured |
| **NBT Luhn Validation** | 4.3 | ❌ FAIL | **Phase 1 priority** |
| **SA ID Validation** | 4.3 | ❌ FAIL | **Phase 1 priority** |
| WCAG 2.1 AA | 5 | 🟡 PARTIAL | Existing pages compliant |
| **80% Test Coverage** | 7.1 | ❌ FAIL | **Phase 10 priority** |
| **Audit Logging** | 8 | ❌ FAIL | **Phase 1 priority** |

**Overall Compliance**: 55% (11 of 20 requirements met)  
**Target**: 100% by end of Phase 10

---

## 🚨 CRITICAL BLOCKERS (Must Fix First)

| # | Blocker | Phase | Impact |
|---|---------|-------|--------|
| 1 | Missing NBTNumber value object | Phase 1 | 🔴 CRITICAL - Blocks registration |
| 2 | Missing SAIDNumber value object | Phase 1 | 🔴 CRITICAL - Blocks ID validation |
| 3 | Missing Student entity | Phase 1 | 🔴 CRITICAL - Blocks all workflows |
| 4 | Missing Registration entity | Phase 1 | 🔴 CRITICAL - Blocks registration |
| 5 | Missing Payment entity | Phase 1 | 🔴 CRITICAL - Blocks payments |
| 6 | Missing EF Core migration | Phase 1 | 🔴 CRITICAL - Database incomplete |
| 7 | No test suite | Phase 10 | 🔴 CRITICAL - Quality assurance |
| 8 | No audit logging | Phase 1 | 🔴 CRITICAL - Constitution violation |

**Action**: All Phase 1 blockers MUST be resolved before proceeding to Phase 2

---

## 🎓 LEARNING RESOURCES

### Technology Stack

- [.NET 9 Documentation](https://docs.microsoft.com/dotnet/core/whats-new/dotnet-9)
- [EF Core 9 Guide](https://docs.microsoft.com/ef/core/)
- [Blazor Web App Tutorial](https://docs.microsoft.com/aspnet/core/blazor/)
- [Fluent UI Blazor Components](https://www.fluentui-blazor.net/)
- [Clean Architecture Pattern](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)

### Domain-Specific

- [Luhn Algorithm Explanation](https://en.wikipedia.org/wiki/Luhn_algorithm)
- [South African ID Number Format](https://en.wikipedia.org/wiki/South_African_identity_card)
- [WCAG 2.1 AA Guidelines](https://www.w3.org/WAI/WCAG21/quickref/)

### Project Documentation

- [Constitution](../../CONSTITUTION.md) - Non-negotiable rules
- [Database Documentation](../../database-scripts/README.md) - Schema details
- [Project Status](../../PROJECT-STATUS.md) - Current progress
- [How to Run](../../HOW-TO-RUN.md) - Running instructions

---

## 🤝 TEAM WORKFLOW

### Git Branching Strategy

```
main (protected)
  ├── develop (integration branch)
      ├── feature/phase1-value-objects
      ├── feature/phase1-entities
      ├── feature/phase2-student-service
      ├── feature/phase2-student-api
      └── feature/phase2-student-ui
```

### Pull Request Process

1. Create feature branch from `develop`
2. Implement tasks from [tasks.md](./tasks.md)
3. Write unit tests (80% coverage minimum)
4. Update documentation
5. Create PR to `develop`
6. Code review by senior developer
7. Merge after approval
8. Update task status

### Definition of Done

- [ ] Code implemented and tested
- [ ] Unit tests written (80%+ coverage)
- [ ] Integration tests added (for API endpoints)
- [ ] Documentation updated
- [ ] Code reviewed and approved
- [ ] No linting errors
- [ ] Constitution compliance verified
- [ ] Merged to develop branch

---

## 📞 SUPPORT & COMMUNICATION

### For Questions About:

- **Architecture**: Review [contracts.md](./contracts.md) and [plan.md](./plan.md)
- **Setup Issues**: See [quickstart.md](./quickstart.md) troubleshooting section
- **Task Assignments**: Check [tasks.md](./tasks.md) for task dependencies
- **Constitution Compliance**: Refer to [review.md - Section 14](./review.md#14-constitution-compliance-checklist)

### Document Maintenance

These specifications are **living documents**. Update as needed:

- **contracts.md**: Add new endpoints or entities
- **plan.md**: Adjust timelines or phases
- **review.md**: Update findings after shell changes
- **tasks.md**: Mark tasks complete, add new tasks
- **quickstart.md**: Update setup instructions
- **README.md**: Keep navigation current

---

## 🎯 SUCCESS CRITERIA

### Technical Success

- ✅ All 90 API endpoints functional
- ✅ All 38 UI pages operational
- ✅ 80%+ test coverage
- ✅ <3 second page load
- ✅ <500ms API response time
- ✅ WCAG 2.1 AA compliant
- ✅ Zero critical vulnerabilities

### Business Success

- ✅ Students can register online
- ✅ Payments processed via EasyPay
- ✅ Staff can manage all entities
- ✅ Results can be imported (Excel)
- ✅ Reports can be generated
- ✅ System is production-ready

### Compliance Success

- ✅ 100% Constitution compliance
- ✅ All audit requirements met
- ✅ Security hardened
- ✅ Performance targets achieved
- ✅ Documentation complete

---

## 📅 TIMELINE SUMMARY

| Milestone | Date | Status |
|-----------|------|--------|
| Phase 0: Shell Audit | Week 0 | ✅ COMPLETE |
| Phase 1: Foundation | Week 1 | 🔴 CRITICAL |
| Phase 2: Student Module | Week 2 | 🔴 CRITICAL |
| Phase 3: Registration | Week 3-4 | 🔴 CRITICAL |
| Phase 4: Payments | Week 5 | 🔴 CRITICAL |
| Phase 5: Venues | Week 6 | 🟡 HIGH |
| Phase 6: Sessions | Week 7 | 🟡 HIGH |
| Phase 7: Results | Week 8 | 🟡 HIGH |
| Phase 8: Dashboards | Week 9 | 🟡 MEDIUM |
| Phase 9: Reports | Week 10 | 🟡 MEDIUM |
| Phase 10: Testing | Week 11-12 | 🔴 CRITICAL |
| **GO LIVE** | **End Week 12** | **🚀 TARGET** |

---

## 🏁 GETTING STARTED

### Immediate Actions (Today)

1. ✅ Read this README.md (5 minutes)
2. ✅ Run through [quickstart.md](./quickstart.md) (15 minutes)
3. ✅ Review [review.md](./review.md) findings (30 minutes)
4. ✅ Read [contracts.md](./contracts.md) - Student entity (30 minutes)
5. ✅ Start Phase 1, Task T016 from [tasks.md](./tasks.md)

### Week 1 Goals

- Complete all 56 Phase 1 tasks
- 9 entities created
- 2 value objects implemented
- Database migration applied
- 35+ unit tests passing

### Week 2 Goals

- Complete all 42 Phase 2 tasks
- Student module operational
- 9 API endpoints live
- Admin CRUD pages functional
- 30+ additional tests passing

---

## 📝 VERSION HISTORY

| Version | Date | Changes | Author |
|---------|------|---------|--------|
| 1.0 | 2025-11-08 | Initial complete specification suite | NBT Team |

---

## 📄 LICENSE & CONFIDENTIALITY

This documentation is **confidential** and proprietary to the National Benchmark Tests (NBT) organization. Unauthorized distribution is prohibited.

---

**DOCUMENTATION STATUS**: ✅ COMPLETE  
**READY FOR IMPLEMENTATION**: ✅ YES  
**NEXT ACTION**: Begin Phase 1, Task T016

**Let's build something amazing! 🚀**
