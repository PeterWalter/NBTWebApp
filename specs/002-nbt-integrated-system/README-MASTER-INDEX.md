# NBT Integrated System - Master Index & Navigation Guide

## 📚 Purpose

This is your **central navigation hub** for the NBT Integrated System SpecKit documentation. All requirements, specifications, tasks, and implementation guides are organized here.

---

## 🎯 Quick Navigation

### 🏛️ Core Documents (Must Read First)

| Document | Purpose | Last Updated | Status |
|----------|---------|--------------|---------|
| [constitution.md](./constitution.md) | Non-negotiable principles, tech stack, business rules | 2025-11-09 | ✅ ACTIVE |
| [CONSTITUTION-UPDATES-2025-11-09.md](./CONSTITUTION-UPDATES-2025-11-09.md) | Latest critical updates summary | 2025-11-09 | ✅ NEW |
| [specification.md](./specification.md) | Complete functional requirements | 2025-11-08 | ✅ ACTIVE |
| [plan.md](./plan.md) | Implementation roadmap and phases | 2025-11-08 | ✅ ACTIVE |

### 🚀 Implementation Guides

| Document | Purpose | For Role | Status |
|----------|---------|----------|---------|
| [IMPLEMENTATION-QUICKSTART-2025-11-09.md](./IMPLEMENTATION-QUICKSTART-2025-11-09.md) | Step-by-step implementation guide | Developers | ✅ NEW |
| [quickstart.md](./quickstart.md) | Original setup and run guide | All | ✅ ACTIVE |
| [tasks.md](./tasks.md) | Detailed task breakdown (485 tasks) | Project Managers | ✅ ACTIVE |

### 📋 Reference Documents

| Document | Purpose | Last Updated |
|----------|---------|--------------|
| [contracts.md](./contracts.md) | API contracts and data schemas | 2025-11-08 |
| [review.md](./review.md) | Shell project audit findings | 2025-11-08 |
| [INDEX.md](./INDEX.md) | Previous index (superseded) | 2025-11-08 |

---

## 🔥 What's New (2025-11-09)

### Critical Business Rule Updates

1. **Test Result Structure**
   - AQL test = 2 domains (AL, QL)
   - MAT test = 3 domains (AL, QL, MAT)
   - Unique barcode per test write
   - Performance levels per domain

2. **Registration Wizard Restructure**
   - Changed from 5 steps to 4 combined steps
   - Step 1: Account & Personal (combined)
   - Step 2: Academic & Test (combined)
   - Step 3: Pre-Test Questionnaire (new)
   - Step 4: Review & Confirmation

3. **Student ID Types**
   - SA_ID: Auto-extract DOB and Gender
   - FOREIGN_ID: Manual entry required
   - PASSPORT: Manual entry required

4. **Entity Updates**
   - NEW: TestResultDomain entity
   - NEW: PreTestQuestionnaire entity
   - UPDATED: TestResult (added Barcode, TestType)
   - UPDATED: Student (added DOB, Gender, Ethnicity)

---

## 📖 Documentation Hierarchy

```
📦 NBT Integrated System Documentation
│
├── 🏛️ **CONSTITUTIONAL LAYER** (Non-Negotiable)
│   ├── constitution.md                    ← Core principles & rules
│   └── CONSTITUTION-UPDATES-2025-11-09.md ← Latest critical updates
│
├── 📋 **SPECIFICATION LAYER** (What to Build)
│   ├── specification.md                   ← Functional requirements
│   ├── contracts.md                       ← API & data contracts
│   └── README.md                          ← General overview
│
├── 🗺️ **PLANNING LAYER** (How to Build)
│   ├── plan.md                            ← Implementation roadmap
│   ├── tasks.md                           ← Detailed task breakdown
│   └── review.md                          ← Shell project audit
│
├── 🚀 **IMPLEMENTATION LAYER** (Build It)
│   ├── IMPLEMENTATION-QUICKSTART-2025-11-09.md ← NEW! Step-by-step guide
│   ├── quickstart.md                      ← Setup & run instructions
│   └── SPECKIT-COMPLETE-IMPLEMENTATION.md ← Complete reference
│
└── 📑 **REFERENCE LAYER** (Supporting Docs)
    ├── INDEX.md                           ← Previous index
    ├── CRITICAL-UPDATES.md                ← Historical updates
    └── README-MASTER-INDEX.md             ← This document
```

---

## 🎯 Role-Based Quick Start

### 👨‍💼 Project Manager / Product Owner

**Read in this order:**
1. [CONSTITUTION-UPDATES-2025-11-09.md](./CONSTITUTION-UPDATES-2025-11-09.md) - Latest changes
2. [constitution.md](./constitution.md) - Business rules
3. [tasks.md](./tasks.md) - Task breakdown
4. [plan.md](./plan.md) - Implementation phases

**Key Focus:**
- Student booking rules (1 test at a time, max 2/year)
- Test result structure (domains, barcodes)
- Registration wizard flow (4 steps)

### 👨‍💻 Developer

**Read in this order:**
1. [IMPLEMENTATION-QUICKSTART-2025-11-09.md](./IMPLEMENTATION-QUICKSTART-2025-11-09.md) - Start here!
2. [CONSTITUTION-UPDATES-2025-11-09.md](./CONSTITUTION-UPDATES-2025-11-09.md) - Entity changes
3. [quickstart.md](./quickstart.md) - Setup environment
4. [contracts.md](./contracts.md) - API contracts

**Key Focus:**
- New entities: TestResultDomain, PreTestQuestionnaire
- Updated entities: TestResult, Student
- Registration wizard: 4-step structure
- SA_ID auto-extraction logic

### 🏗️ Architect / Tech Lead

**Read in this order:**
1. [constitution.md](./constitution.md) - Architecture principles
2. [CONSTITUTION-UPDATES-2025-11-09.md](./CONSTITUTION-UPDATES-2025-11-09.md) - Latest updates
3. [review.md](./review.md) - Shell audit findings
4. [plan.md](./plan.md) - Implementation strategy

**Key Focus:**
- Clean Architecture compliance
- Entity relationships (TestSession → Venue, NOT Room)
- CI/CD workflow (branch → test → merge)
- .NET 9 migration complete

### 🧪 QA / Tester

**Read in this order:**
1. [CONSTITUTION-UPDATES-2025-11-09.md](./CONSTITUTION-UPDATES-2025-11-09.md) - Test scenarios
2. [specification.md](./specification.md) - Functional requirements
3. [tasks.md](./tasks.md) - Phase 10 testing tasks

**Key Focus:**
- Registration wizard validation (4 steps)
- SA_ID vs Foreign ID flows
- Barcode uniqueness per test write
- Domain result validation (AQL=2, MAT=3)

---

## 📊 Implementation Status

### ✅ Completed Phases

- [x] Phase 0: Shell Audit
- [x] Phase 1: Foundation & Database
- [x] Phase 2: Student Module
- [x] Phase 4: Booking & Payment
- [x] Phase 5: Reporting
- [x] Phase 6: Authentication

### 🔄 In Progress

- [ ] **Phase 3: Registration Wizard** - Restructuring to 4 steps with new entities
- [ ] **Phase 7: Venue Management** - Fixing TestSession → Venue relationship
- [ ] **Phase 5 (Extended): Results Management** - Adding domains and barcodes

### 🔜 Upcoming

- [ ] Phase 8: Admin Dashboards
- [ ] Phase 9: Advanced Reporting
- [ ] Phase 10: Testing & Deployment

---

## 🔑 Key Entities Quick Reference

### Student
- NBTNumber (14-digit Luhn)
- IDType (SA_ID | FOREIGN_ID | PASSPORT)
- DateOfBirth (auto-extracted if SA_ID)
- Gender (auto-extracted if SA_ID)
- Ethnicity

### TestResult
- **NEW**: Barcode (unique per test write)
- **NEW**: TestType (AQL | MAT)
- Domains (1:N relationship)

### TestResultDomain (NEW)
- DomainType (AL | QL | MAT)
- Score (0-100)
- PerformanceLevel (6 levels)
- Percentile

### PreTestQuestionnaire (NEW)
- StudentId
- RegistrationId
- Responses (JSON)
- CompletedDate

---

## 📐 Relationships Diagram

```
Student (1) ----< (N) Registration
   |
   |----< (N) TestResult
   |              |
   |              |----< (N) TestResultDomain
   |                           (AL, QL, MAT)
   |
   |----< (N) PreTestQuestionnaire

TestSession (N) ----< (1) Venue
                           |
                           |----< (N) Room

TestSession (1) ----< (N) RoomAllocation ----< (1) Student
                           |
                           ----< (1) Room
```

---

## 🚦 Implementation Workflow

### For New Features

1. **Create Branch**
   ```bash
   git checkout -b feature/{feature-name}
   ```

2. **Implement Changes**
   - Follow [IMPLEMENTATION-QUICKSTART-2025-11-09.md](./IMPLEMENTATION-QUICKSTART-2025-11-09.md)
   - Reference [constitution.md](./constitution.md) for rules
   - Check [contracts.md](./contracts.md) for API design

3. **Test Locally**
   ```bash
   dotnet build
   dotnet test
   dotnet run
   ```

4. **Push to GitHub**
   ```bash
   git add .
   git commit -m "feat: description"
   git push -u origin feature/{feature-name}
   ```

5. **Create Pull Request**
   - Request review
   - Ensure CI/CD passes
   - Merge to main when approved

---

## 🎓 Learning Path

### New to Project?
1. Start with [quickstart.md](./quickstart.md) - Get app running
2. Read [CONSTITUTION-UPDATES-2025-11-09.md](./CONSTITUTION-UPDATES-2025-11-09.md) - Understand current state
3. Follow [IMPLEMENTATION-QUICKSTART-2025-11-09.md](./IMPLEMENTATION-QUICKSTART-2025-11-09.md) - Build features

### Already Familiar?
1. Review [CONSTITUTION-UPDATES-2025-11-09.md](./CONSTITUTION-UPDATES-2025-11-09.md) - See what changed
2. Jump to [IMPLEMENTATION-QUICKSTART-2025-11-09.md](./IMPLEMENTATION-QUICKSTART-2025-11-09.md) - Start coding

### Need Deep Dive?
1. Read full [constitution.md](./constitution.md)
2. Study [specification.md](./specification.md)
3. Review [tasks.md](./tasks.md) for all 485 tasks

---

## 🔍 Search Tips

### Find by Topic

| Topic | Documents to Check |
|-------|-------------------|
| **Business Rules** | constitution.md, CONSTITUTION-UPDATES |
| **Student Registration** | specification.md, IMPLEMENTATION-QUICKSTART |
| **Test Results** | CONSTITUTION-UPDATES, contracts.md |
| **Database Schema** | contracts.md, review.md |
| **API Endpoints** | contracts.md, specification.md |
| **Setup Instructions** | quickstart.md, IMPLEMENTATION-QUICKSTART |
| **Task Breakdown** | tasks.md, plan.md |

### Find by Phase

| Phase | Primary Document | Secondary Docs |
|-------|-----------------|----------------|
| Phase 0 | review.md | quickstart.md |
| Phase 1 | tasks.md (Foundation) | constitution.md |
| Phase 2 | tasks.md (Student) | IMPLEMENTATION-QUICKSTART |
| Phase 3 | IMPLEMENTATION-QUICKSTART | specification.md |
| Phase 4 | tasks.md (Payment) | contracts.md |
| Phase 5 | tasks.md (Reporting) | CONSTITUTION-UPDATES |

---

## ✅ Compliance Checklist

Before considering any phase "complete":

- [ ] All code follows [constitution.md](./constitution.md) principles
- [ ] All entities match [contracts.md](./contracts.md) schemas
- [ ] All changes comply with latest [CONSTITUTION-UPDATES-2025-11-09.md](./CONSTITUTION-UPDATES-2025-11-09.md)
- [ ] Tests written per [tasks.md](./tasks.md) Phase 10
- [ ] CI/CD pipeline passing
- [ ] Pull request reviewed and approved
- [ ] Documentation updated
- [ ] Changes merged to main

---

## 📞 Getting Help

### Documentation Issues?
- Check if your question is in [CONSTITUTION-UPDATES-2025-11-09.md](./CONSTITUTION-UPDATES-2025-11-09.md)
- Review [quickstart.md](./quickstart.md) for setup issues
- Check [IMPLEMENTATION-QUICKSTART-2025-11-09.md](./IMPLEMENTATION-QUICKSTART-2025-11-09.md) for code examples

### Implementation Issues?
- Follow steps in [IMPLEMENTATION-QUICKSTART-2025-11-09.md](./IMPLEMENTATION-QUICKSTART-2025-11-09.md)
- Verify against [constitution.md](./constitution.md) rules
- Check [contracts.md](./contracts.md) for API structure

### Still Stuck?
- Create a GitHub issue
- Reference the specific document and section
- Provide error messages and context

---

## 🏆 Success Criteria

Your implementation is successful when:

✅ All **18 non-negotiable rules** from constitution.md are followed  
✅ All **new entities** from CONSTITUTION-UPDATES are implemented  
✅ **Registration wizard** has 4 steps with combined sections  
✅ **Test results** have domains (AQL=2, MAT=3) and unique barcodes  
✅ **SA_ID** auto-extracts DOB and Gender  
✅ **Foreign ID/Passport** flow works correctly  
✅ All **tests pass** and coverage >= 70%  
✅ **CI/CD pipeline** passes all stages  
✅ Code **merged to main** after review  

---

## 📅 Version History

| Version | Date | Changes | Documents Affected |
|---------|------|---------|-------------------|
| 2.0 | 2025-11-09 | Test results domains, wizard restructure, entity updates | constitution.md, NEW: CONSTITUTION-UPDATES, NEW: IMPLEMENTATION-QUICKSTART |
| 1.0 | 2025-11-08 | Initial SpecKit documentation | All base documents |

---

## 🎯 Current Focus (2025-11-09)

### Immediate Priorities

1. **Update Domain Entities**
   - Add TestResultDomain entity
   - Add PreTestQuestionnaire entity
   - Update TestResult with Barcode and TestType
   - Update Student with DOB, Gender, Ethnicity

2. **Restructure Registration Wizard**
   - Combine Steps 1 & 2 (Account + Personal)
   - Combine Steps 3 & 4 (Academic + Test)
   - Add Step 3 (Pre-Test Questionnaire)
   - Update Step 4 (Review & Confirmation)

3. **Implement Auto-Extraction**
   - SA_ID → DOB extraction
   - SA_ID → Gender extraction
   - Luhn validation for SA_ID

4. **Test & Deploy**
   - Run full test suite
   - Verify all wizard steps
   - Push to GitHub
   - Merge to main

---

**Master Index Version**: 1.0  
**Last Updated**: 2025-11-09  
**Maintained By**: NBT Development Team  
**Next Review**: After Phase 3 completion

---

🎓 **Remember**: When in doubt, check [CONSTITUTION-UPDATES-2025-11-09.md](./CONSTITUTION-UPDATES-2025-11-09.md) first - it has the latest critical information!
