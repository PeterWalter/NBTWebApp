# Task Breakdown - NBT Integrated System Implementation

**Feature**: 002-nbt-integrated-system  
**Version**: 1.0  
**Created**: 2025-11-08  
**Status**: READY FOR IMPLEMENTATION  
**Total Tasks**: 485  
**Total Effort**: 580 hours

---

## 📋 TASK OVERVIEW

This document provides a **granular task breakdown** for implementing the NBT Integrated System. Each task includes:
- ✅ Unique task ID
- 📝 Clear description
- ⏱️ Time estimate
- 🔗 Dependencies
- 🎯 Phase assignment
- 🏷️ Shell integration notes (extends/completes existing code)

---

## 📊 TASK SUMMARY BY PHASE

| Phase | Tasks | Hours | Status | Shell Impact |
|-------|-------|-------|--------|--------------|
| **Phase 0: Shell Audit** | 15 | 8 | ✅ COMPLETE | Audit existing |
| **Phase 1: Foundation** | 45 | 40 | 🔴 CRITICAL | Add new entities |
| **Phase 2: Student Module** | 42 | 40 | 🔴 CRITICAL | New module |
| **Phase 3: Registration** | 58 | 80 | 🔴 CRITICAL | New workflow |
| **Phase 4: Payments** | 38 | 40 | 🔴 CRITICAL | New integration |
| **Phase 5: Venues** | 32 | 40 | 🟡 HIGH | New module |
| **Phase 6: Sessions** | 35 | 40 | 🟡 HIGH | New module |
| **Phase 7: Results** | 28 | 40 | 🟡 HIGH | New module |
| **Phase 8: Dashboards** | 36 | 40 | 🟡 MEDIUM | Extend admin |
| **Phase 9: Reports** | 30 | 40 | 🟡 MEDIUM | New module |
| **Phase 10: Testing** | 126 | 120 | 🔴 CRITICAL | New test suite |
| **TOTAL** | **485** | **580** | | |

---

## 🔍 PHASE 0: SHELL AUDIT & VERIFICATION

**Duration**: 1 day (8 hours)  
**Status**: ✅ COMPLETE (Already performed)  
**Purpose**: Verify existing shell and document gaps

### 0.1 Environment Verification (2 hours)

**T001**: Verify Prerequisites Installation  
**Estimate**: 30 minutes  
**Shell Impact**: N/A (verification only)  
**Steps**:
- [ ] Check .NET 9 SDK installed (`dotnet --version`)
- [ ] Check SQL Server accessible
- [ ] Check Git installed
- [ ] Run verification script from quickstart.md

**T002**: Clone Repository and Restore Packages  
**Estimate**: 30 minutes  
**Shell Impact**: Uses existing structure  
**Steps**:
- [ ] Clone repository
- [ ] Run `dotnet restore`
- [ ] Verify all packages restored successfully

**T003**: Configure Database Connection  
**Estimate**: 30 minutes  
**Shell Impact**: Uses existing appsettings.json  
**Steps**:
- [ ] Update connection string in appsettings.Development.json
- [ ] Test SQL Server connection
- [ ] Apply existing migration (`dotnet ef database update`)
- [ ] Verify database created with 6 tables

**T004**: Run and Verify Shell Application  
**Estimate**: 30 minutes  
**Shell Impact**: Tests existing functionality  
**Steps**:
- [ ] Run API project (verify port 5000/5001)
- [ ] Run Blazor UI project (verify port 5002/5003)
- [ ] Login with admin credentials
- [ ] Test all 13 existing pages
- [ ] Verify Fluent UI theme loads

### 0.2 Code Audit (4 hours)

**T005**: Audit Domain Layer  
**Estimate**: 1 hour  
**Shell Impact**: Documents existing entities  
**Deliverable**: List of 6 existing entities, identify 9 missing

**T006**: Audit Application Layer  
**Estimate**: 1 hour  
**Shell Impact**: Documents existing services  
**Deliverable**: List of 6 existing services, identify 12 missing

**T007**: Audit Infrastructure Layer  
**Estimate**: 1 hour  
**Shell Impact**: Documents EF Core setup  
**Deliverable**: Review DbContext, configurations, migration

**T008**: Audit API Layer  
**Estimate**: 30 minutes  
**Shell Impact**: Documents existing controllers  
**Deliverable**: List 26 existing endpoints, identify 64 missing

**T009**: Audit UI Layer  
**Estimate**: 30 minutes  
**Shell Impact**: Documents existing pages  
**Deliverable**: List 13 existing pages, identify 25 missing

### 0.3 Gap Analysis Documentation (2 hours)

**T010**: Document Missing Entities  
**Estimate**: 30 minutes  
**Deliverable**: Complete list with specifications

**T011**: Document Missing Services  
**Estimate**: 30 minutes  
**Deliverable**: Service interfaces and responsibilities

**T012**: Document Missing API Endpoints  
**Estimate**: 30 minutes  
**Deliverable**: API endpoint specifications

**T013**: Document Missing UI Pages  
**Estimate**: 30 minutes  
**Deliverable**: Page wireframes and requirements

**T014**: Create Implementation Priority Matrix  
**Estimate**: 30 minutes  
**Deliverable**: Task prioritization by dependencies

**T015**: Review Findings with Team  
**Estimate**: 30 minutes  
**Deliverable**: Sign-off on audit findings

---

## 🏗️ PHASE 1: FOUNDATION & DOMAIN SETUP

**Duration**: Week 1 (5 days, 40 hours)  
**Priority**: 🔴 CRITICAL  
**Dependencies**: Phase 0 complete  
**Shell Impact**: **ADDS NEW** - Creates missing domain layer

### 1.1 Value Objects (Day 1-2: 16 hours)

**T016**: Create ValueObject Base Class  
**Estimate**: 1 hour  
**Location**: `src/NBT.Domain/Common/ValueObject.cs`  
**Shell Impact**: **NEW FILE** - Base class for value objects  
**Steps**:
- [ ] Create abstract ValueObject class
- [ ] Implement GetEqualityComponents()
- [ ] Override Equals() and GetHashCode()
- [ ] Add comparison operators

**T017**: Create DomainException Class  
**Estimate**: 30 minutes  
**Location**: `src/NBT.Domain/Exceptions/DomainException.cs`  
**Shell Impact**: **NEW FILE** - Domain-specific exceptions  
**Steps**:
- [ ] Create DomainException : Exception
- [ ] Add constructors for message and inner exception

**T018**: Implement NBTNumber Value Object  
**Estimate**: 4 hours  
**Location**: `src/NBT.Domain/ValueObjects/NBTNumber.cs`  
**Shell Impact**: **NEW FILE** - NBT number with Luhn validation  
**Constitution**: Section 4.3 compliance  
**Steps**:
- [ ] Create NBTNumber class inheriting ValueObject
- [ ] Implement Generate(year, sequence) method
- [ ] Implement CalculateLuhnChecksum() algorithm
- [ ] Implement ValidateLuhnChecksum() method
- [ ] Implement IsValid() method
- [ ] Add format validation (9 digits)
- [ ] Add ToString() override
- [ ] Add XML documentation

**T019**: Write Unit Tests for NBTNumber  
**Estimate**: 3 hours  
**Location**: `tests/NBT.Domain.Tests/ValueObjects/NBTNumberTests.cs`  
**Shell Impact**: **NEW TEST PROJECT** - First tests  
**Steps**:
- [ ] Create test project structure
- [ ] Test Generate() with valid inputs
- [ ] Test Generate() with invalid inputs (exceptions)
- [ ] Test IsValid() with valid NBT numbers
- [ ] Test IsValid() with invalid NBT numbers
- [ ] Test Luhn checksum calculation
- [ ] Test edge cases (year boundaries, sequence limits)
- [ ] Verify 15+ test cases pass

**T020**: Implement SAIDNumber Value Object  
**Estimate**: 5 hours  
**Location**: `src/NBT.Domain/ValueObjects/SAIDNumber.cs`  
**Shell Impact**: **NEW FILE** - SA ID validation  
**Constitution**: Section 4.3 compliance  
**Steps**:
- [ ] Create SAIDNumber class inheriting ValueObject
- [ ] Implement Create() factory method
- [ ] Implement IsValid() with format check
- [ ] Implement IsValidDate() for date portion
- [ ] Implement ValidateLuhnChecksum() for SA ID
- [ ] Implement ExtractDateOfBirth() method
- [ ] Implement ExtractGender() method (0-4=F, 5-9=M)
- [ ] Implement ExtractCitizenship() method
- [ ] Add properties: DateOfBirth, Gender, IsSACitizen
- [ ] Add comprehensive validation

**T021**: Write Unit Tests for SAIDNumber  
**Estimate**: 2.5 hours  
**Location**: `tests/NBT.Domain.Tests/ValueObjects/SAIDNumberTests.cs`  
**Shell Impact**: **NEW FILE** - SA ID tests  
**Steps**:
- [ ] Test Create() with valid SA ID
- [ ] Test Create() with invalid SA ID (exceptions)
- [ ] Test date extraction (various dates)
- [ ] Test gender extraction
- [ ] Test citizenship extraction
- [ ] Test Luhn validation
- [ ] Test edge cases (leap years, century boundaries)
- [ ] Verify 20+ test cases pass

### 1.2 Enums (Day 2: 2 hours)

**T022**: Create RegistrationStatus Enum  
**Estimate**: 15 minutes  
**Location**: `src/NBT.Domain/Enums/RegistrationStatus.cs`  
**Shell Impact**: **NEW FILE**  
**Values**: Pending, Confirmed, Cancelled, NoShow, Completed

**T023**: Create PaymentStatus Enum  
**Estimate**: 15 minutes  
**Location**: `src/NBT.Domain/Enums/PaymentStatus.cs`  
**Shell Impact**: **NEW FILE**  
**Values**: Pending, Paid, Failed, Refunded, PartialRefund

**T024**: Create SessionStatus Enum  
**Estimate**: 15 minutes  
**Location**: `src/NBT.Domain/Enums/SessionStatus.cs`  
**Shell Impact**: **NEW FILE**  
**Values**: Open, Full, Closed, Completed, Cancelled

**T025**: Create TestType Enum  
**Estimate**: 15 minutes  
**Location**: `src/NBT.Domain/Enums/TestType.cs`  
**Shell Impact**: **NEW FILE**  
**Values**: AcademicLiteracy, QuantitativeLiteracy, Mathematics

**T026**: Create PerformanceBand Enum  
**Estimate**: 15 minutes  
**Location**: `src/NBT.Domain/Enums/PerformanceBand.cs`  
**Shell Impact**: **NEW FILE**  
**Values**: Elementary, Basic, Intermediate, Proficient

**T027**: Update BaseEntity with Timestamps  
**Estimate**: 45 minutes  
**Location**: `src/NBT.Domain/Common/BaseEntity.cs`  
**Shell Impact**: **EXTENDS EXISTING** - Add audit fields if missing  
**Steps**:
- [ ] Verify IAuditableEntity interface exists
- [ ] Add CreatedDate, CreatedBy, UpdatedDate, UpdatedBy if missing

### 1.3 Domain Entities (Day 2-3: 14 hours)

**T028**: Create Student Entity  
**Estimate**: 2 hours  
**Location**: `src/NBT.Domain/Entities/Student.cs`  
**Shell Impact**: **NEW FILE** - Core entity  
**Steps**:
- [ ] Create Student : BaseEntity, IAuditableEntity
- [ ] Add NBTNumber property (string, 9 digits)
- [ ] Add IDNumber property (string, 13 digits)
- [ ] Add FirstName, LastName, Email, Phone
- [ ] Add DateOfBirth, Gender (computed from ID)
- [ ] Add Address, City, Province, PostalCode
- [ ] Add SchoolName, Grade, HomeLanguage
- [ ] Add SpecialAccommodation (optional)
- [ ] Add validation attributes ([Required], [StringLength], etc.)
- [ ] Add navigation properties (Registrations, TestResults)
- [ ] Add XML documentation

**T029**: Create Registration Entity  
**Estimate**: 2 hours  
**Location**: `src/NBT.Domain/Entities/Registration.cs`  
**Shell Impact**: **NEW FILE**  
**Steps**:
- [ ] Create Registration : BaseEntity, IAuditableEntity
- [ ] Add RegistrationNumber (REG-YYYY-NNNNNN format)
- [ ] Add StudentId, TestSessionId (foreign keys)
- [ ] Add Status (RegistrationStatus enum)
- [ ] Add TestTypesSelected (JSON array string)
- [ ] Add IsRemoteWriter, RemoteLocation
- [ ] Add SpecialSessionType (optional)
- [ ] Add RegistrationDate, ConfirmationDate, CancellationDate
- [ ] Add CancellationReason (optional)
- [ ] Add navigation properties (Student, TestSession, Payment)

**T030**: Create Payment Entity  
**Estimate**: 2 hours  
**Location**: `src/NBT.Domain/Entities/Payment.cs`  
**Shell Impact**: **NEW FILE**  
**Steps**:
- [ ] Create Payment : BaseEntity, IAuditableEntity
- [ ] Add RegistrationId (foreign key)
- [ ] Add InvoiceNumber (INV-YYYY-NNNNNN format)
- [ ] Add Amount (decimal)
- [ ] Add PaymentMethod (string: EasyPay, Cash, EFT, Card)
- [ ] Add Status (PaymentStatus enum)
- [ ] Add EasyPayReference, EasyPayTransactionId
- [ ] Add PaidDate, RefundedDate, RefundReason
- [ ] Add navigation property (Registration)

**T031**: Create TestSession Entity  
**Estimate**: 2 hours  
**Location**: `src/NBT.Domain/Entities/TestSession.cs`  
**Shell Impact**: **NEW FILE**  
**Steps**:
- [ ] Create TestSession : BaseEntity, IAuditableEntity
- [ ] Add SessionCode (CITY-YYYY-MM-DD-PERIOD format)
- [ ] Add SessionName, SessionDate
- [ ] Add StartTime, EndTime (TimeSpan)
- [ ] Add VenueId (foreign key)
- [ ] Add Capacity, CurrentRegistrations
- [ ] Add computed property: AvailableSeats
- [ ] Add Status (SessionStatus enum)
- [ ] Add IsSpecialSession, SpecialSessionNotes
- [ ] Add navigation properties (Venue, Registrations, RoomAllocations)

**T032**: Create Venue Entity  
**Estimate**: 1.5 hours  
**Location**: `src/NBT.Domain/Entities/Venue.cs`  
**Shell Impact**: **NEW FILE**  
**Steps**:
- [ ] Create Venue : BaseEntity, IAuditableEntity
- [ ] Add VenueName, VenueCode (unique)
- [ ] Add Address, City, Province, PostalCode
- [ ] Add ContactPerson, ContactEmail, ContactPhone
- [ ] Add TotalCapacity (int)
- [ ] Add IsAccessible (bool)
- [ ] Add Status (Active, Inactive, UnderMaintenance)
- [ ] Add Notes (optional)
- [ ] Add navigation properties (Rooms, TestSessions)

**T033**: Create Room Entity  
**Estimate**: 1.5 hours  
**Location**: `src/NBT.Domain/Entities/Room.cs`  
**Shell Impact**: **NEW FILE**  
**Steps**:
- [ ] Create Room : BaseEntity
- [ ] Add VenueId (foreign key)
- [ ] Add RoomName, RoomNumber
- [ ] Add Capacity (int)
- [ ] Add RoomType (ComputerLab, Classroom, Hall, ExamRoom)
- [ ] Add HasComputers, ComputerCount (optional)
- [ ] Add IsAccessible (bool)
- [ ] Add Status (Available, Unavailable, UnderMaintenance)
- [ ] Add Notes (optional)
- [ ] Add navigation properties (Venue, RoomAllocations)

**T034**: Create RoomAllocation Entity  
**Estimate**: 1 hour  
**Location**: `src/NBT.Domain/Entities/RoomAllocation.cs`  
**Shell Impact**: **NEW FILE**  
**Steps**:
- [ ] Create RoomAllocation : BaseEntity
- [ ] Add TestSessionId, RoomId (foreign keys)
- [ ] Add AllocatedStudents (int)
- [ ] Add navigation properties (TestSession, Room)

**T035**: Create TestResult Entity  
**Estimate**: 1.5 hours  
**Location**: `src/NBT.Domain/Entities/TestResult.cs`  
**Shell Impact**: **NEW FILE**  
**Steps**:
- [ ] Create TestResult : BaseEntity, IAuditableEntity
- [ ] Add StudentId, TestSessionId (foreign keys)
- [ ] Add TestType (string: AcademicLiteracy, etc.)
- [ ] Add RawScore (decimal, 0-100)
- [ ] Add Percentile (int, 1-99)
- [ ] Add PerformanceBand (string)
- [ ] Add IsReleased (bool)
- [ ] Add TestDate, ResultDate, ReleasedDate
- [ ] Add navigation properties (Student, TestSession)

**T036**: Create AuditLog Entity  
**Estimate**: 1.5 hours  
**Location**: `src/NBT.Domain/Entities/AuditLog.cs`  
**Shell Impact**: **NEW FILE** - Immutable audit trail  
**Constitution**: Section 8 compliance  
**Steps**:
- [ ] Create AuditLog : BaseEntity (immutable)
- [ ] Add Timestamp (DateTime, required)
- [ ] Add UserId, UserEmail (required)
- [ ] Add Action (Create, Update, Delete, Login, Import)
- [ ] Add EntityType, EntityId
- [ ] Add BeforeValue, AfterValue (JSON)
- [ ] Add IpAddress, UserAgent
- [ ] Mark as immutable (no update/delete methods)

### 1.4 EF Core Configurations (Day 3-4: 8 hours)

**T037**: Create StudentConfiguration  
**Estimate**: 1 hour  
**Location**: `src/NBT.Infrastructure/Persistence/Configurations/StudentConfiguration.cs`  
**Shell Impact**: **NEW FILE**  
**Steps**:
- [ ] Implement IEntityTypeConfiguration<Student>
- [ ] Configure table name "Students"
- [ ] Configure unique index on NBTNumber
- [ ] Configure unique index on IDNumber
- [ ] Configure index on Email
- [ ] Configure relationships with Registrations (Restrict)
- [ ] Configure relationships with TestResults (Restrict)
- [ ] Set string length constraints
- [ ] Configure required fields

**T038**: Create RegistrationConfiguration  
**Estimate**: 1 hour  
**Location**: `src/NBT.Infrastructure/Persistence/Configurations/RegistrationConfiguration.cs`  
**Shell Impact**: **NEW FILE**  
**Steps**:
- [ ] Configure table name "Registrations"
- [ ] Configure unique index on RegistrationNumber
- [ ] Configure relationships with Student (Restrict)
- [ ] Configure relationships with TestSession (Restrict)
- [ ] Configure relationship with Payment (Cascade)
- [ ] Configure Status enum conversion
- [ ] Set string length constraints

**T039**: Create PaymentConfiguration  
**Estimate**: 45 minutes  
**Location**: `src/NBT.Infrastructure/Persistence/Configurations/PaymentConfiguration.cs`  
**Shell Impact**: **NEW FILE**  
**Steps**:
- [ ] Configure table name "Payments"
- [ ] Configure unique index on InvoiceNumber
- [ ] Configure unique index on EasyPayReference
- [ ] Configure relationship with Registration
- [ ] Configure Status enum conversion
- [ ] Configure decimal precision (Amount: 18,2)

**T040**: Create TestSessionConfiguration  
**Estimate**: 1 hour  
**Location**: `src/NBT.Infrastructure/Persistence/Configurations/TestSessionConfiguration.cs`  
**Shell Impact**: **NEW FILE**  
**Steps**:
- [ ] Configure table name "TestSessions"
- [ ] Configure unique index on SessionCode
- [ ] Configure relationship with Venue
- [ ] Configure relationships with Registrations
- [ ] Configure relationships with RoomAllocations
- [ ] Configure Status enum conversion
- [ ] Add check constraint: CurrentRegistrations <= Capacity

**T041**: Create VenueConfiguration  
**Estimate**: 45 minutes  
**Location**: `src/NBT.Infrastructure/Persistence/Configurations/VenueConfiguration.cs`  
**Shell Impact**: **NEW FILE**  
**Steps**:
- [ ] Configure table name "Venues"
- [ ] Configure unique index on VenueCode
- [ ] Configure relationships with Rooms (Cascade)
- [ ] Configure relationships with TestSessions (Restrict)

**T042**: Create RoomConfiguration  
**Estimate**: 45 minutes  
**Location**: `src/NBT.Infrastructure/Persistence/Configurations/RoomConfiguration.cs`  
**Shell Impact**: **NEW FILE**  
**Steps**:
- [ ] Configure table name "Rooms"
- [ ] Configure relationship with Venue
- [ ] Configure relationships with RoomAllocations
- [ ] Add check constraint: ComputerCount <= Capacity

**T043**: Create RoomAllocationConfiguration  
**Estimate**: 30 minutes  
**Location**: `src/NBT.Infrastructure/Persistence/Configurations/RoomAllocationConfiguration.cs`  
**Shell Impact**: **NEW FILE**  
**Steps**:
- [ ] Configure table name "RoomAllocations"
- [ ] Configure composite unique index (TestSessionId, RoomId)
- [ ] Configure relationships with TestSession and Room

**T044**: Create TestResultConfiguration  
**Estimate**: 1 hour  
**Location**: `src/NBT.Infrastructure/Persistence/Configurations/TestResultConfiguration.cs`  
**Shell Impact**: **NEW FILE**  
**Steps**:
- [ ] Configure table name "TestResults"
- [ ] Configure composite unique index (StudentId, TestSessionId, TestType)
- [ ] Configure relationships with Student and TestSession
- [ ] Configure decimal precision (RawScore: 5,2)

**T045**: Create AuditLogConfiguration  
**Estimate**: 45 minutes  
**Location**: `src/NBT.Infrastructure/Persistence/Configurations/AuditLogConfiguration.cs`  
**Shell Impact**: **NEW FILE**  
**Steps**:
- [ ] Configure table name "AuditLogs"
- [ ] Configure index on Timestamp (descending)
- [ ] Configure index on UserId
- [ ] Configure index on EntityType
- [ ] Mark as immutable (no updates/deletes)
- [ ] Configure JSON columns for BeforeValue/AfterValue

**T046**: Update ApplicationDbContext  
**Estimate**: 1.5 hours  
**Location**: `src/NBT.Infrastructure/Persistence/ApplicationDbContext.cs`  
**Shell Impact**: **EXTENDS EXISTING** - Add new DbSets  
**Steps**:
- [ ] Add DbSet<Student> Students
- [ ] Add DbSet<Registration> Registrations
- [ ] Add DbSet<Payment> Payments
- [ ] Add DbSet<TestSession> TestSessions
- [ ] Add DbSet<Venue> Venues
- [ ] Add DbSet<Room> Rooms
- [ ] Add DbSet<RoomAllocation> RoomAllocations
- [ ] Add DbSet<TestResult> TestResults
- [ ] Add DbSet<AuditLog> AuditLogs
- [ ] Apply all configurations in OnModelCreating()

### 1.5 Database Migration (Day 4-5: 4 hours)

**T047**: Generate Migration  
**Estimate**: 30 minutes  
**Location**: `src/NBT.Infrastructure/Migrations/`  
**Shell Impact**: **NEW MIGRATION** - Adds 9 tables  
**Steps**:
- [ ] Run: `dotnet ef migrations add AddCoreEntities`
- [ ] Review generated migration file
- [ ] Verify all 9 tables created
- [ ] Verify all indexes created
- [ ] Verify all foreign keys created

**T048**: Review and Test Migration SQL  
**Estimate**: 1 hour  
**Shell Impact**: Database schema expansion  
**Steps**:
- [ ] Generate SQL script: `dotnet ef migrations script`
- [ ] Review SQL for correctness
- [ ] Check for potential data loss
- [ ] Verify constraint definitions
- [ ] Test migration on clean database

**T049**: Apply Migration to Development  
**Estimate**: 30 minutes  
**Steps**:
- [ ] Backup existing database (if data exists)
- [ ] Run: `dotnet ef database update`
- [ ] Verify all tables created successfully
- [ ] Query sys.tables to confirm

**T050**: Create Seed Data  
**Estimate**: 1.5 hours  
**Location**: `src/NBT.Infrastructure/Persistence/ApplicationDbContextSeed.cs`  
**Shell Impact**: **EXTENDS EXISTING** - Add new seed data  
**Steps**:
- [ ] Add 5 sample venues (JHB, CPT, DBN, PTA, PE)
- [ ] Add 20 sample rooms (4 per venue)
- [ ] Add 10 sample test sessions (upcoming dates)
- [ ] Add system settings for fees
- [ ] Update SeedAsync() method
- [ ] Test seed data creation

**T051**: Verify Database Schema  
**Estimate**: 30 minutes  
**Steps**:
- [ ] Verify all 15 tables exist (6 old + 9 new)
- [ ] Verify relationships are correct
- [ ] Verify indexes are created
- [ ] Verify constraints are applied
- [ ] Query sample data

**T052**: Update Database Documentation  
**Estimate**: 1 hour  
**Location**: `database-scripts/README.md`  
**Shell Impact**: **EXTENDS EXISTING** documentation  
**Steps**:
- [ ] Document new tables and relationships
- [ ] Add ER diagram (if possible)
- [ ] Document seed data
- [ ] Update migration history

### 1.6 Phase 1 Testing & Review (Day 5: 4 hours)

**T053**: Run All Domain Tests  
**Estimate**: 30 minutes  
**Steps**:
- [ ] Run: `dotnet test tests/NBT.Domain.Tests/`
- [ ] Verify 35+ tests pass
- [ ] Check code coverage (should be >90% for value objects)

**T054**: Integration Test - Database Operations  
**Estimate**: 1 hour  
**Steps**:
- [ ] Create, read, update, delete Student
- [ ] Verify relationships work (Student → Registration)
- [ ] Verify unique constraints work
- [ ] Verify cascades work correctly

**T055**: Code Review - Domain Layer  
**Estimate**: 1.5 hours  
**Steps**:
- [ ] Review all entity definitions
- [ ] Review value object implementations
- [ ] Check XML documentation completeness
- [ ] Verify naming conventions
- [ ] Check for code smells

**T056**: Phase 1 Sign-Off  
**Estimate**: 1 hour  
**Steps**:
- [ ] Demo to team: NBT number generation
- [ ] Demo: SA ID validation
- [ ] Show database schema
- [ ] Confirm all Phase 1 tasks complete
- [ ] Get approval to proceed to Phase 2

**Phase 1 Deliverables**:
- ✅ 9 new entities
- ✅ 2 value objects with Luhn validation
- ✅ 5 new enums
- ✅ 9 EF Core configurations
- ✅ 1 migration applied
- ✅ Seed data created
- ✅ 35+ unit tests passing
- ✅ Database schema updated

---

## 👤 PHASE 2: STUDENT MANAGEMENT MODULE

**Duration**: Week 2 (5 days, 40 hours)  
**Priority**: 🔴 CRITICAL  
**Dependencies**: Phase 1 complete  
**Shell Impact**: **NEW MODULE** - First business module

### 2.1 Application Layer - DTOs (Day 6: 4 hours)

**T057**: Create StudentDto  
**Estimate**: 30 minutes  
**Location**: `src/NBT.Application/Students/DTOs/StudentDto.cs`  
**Shell Impact**: **NEW MODULE** - Students folder  
**Steps**:
- [ ] Create Students module folder structure
- [ ] Create StudentDto with all properties
- [ ] Add computed FullName property
- [ ] Add AutoMapper mapping in MappingProfile

**T058**: Create CreateStudentRequest  
**Estimate**: 45 minutes  
**Location**: `src/NBT.Application/Students/DTOs/CreateStudentRequest.cs`  
**Steps**:
- [ ] Create CreateStudentRequest DTO
- [ ] Add validation attributes
- [ ] Add FluentValidation validator

**T059**: Create UpdateStudentRequest  
**Estimate**: 30 minutes  
**Location**: `src/NBT.Application/Students/DTOs/UpdateStudentRequest.cs`  
**Steps**:
- [ ] Create UpdateStudentRequest DTO
- [ ] Add validation attributes

**T060**: Create GenerateNBTNumberRequest/Response  
**Estimate**: 30 minutes  
**Location**: `src/NBT.Application/Students/DTOs/`  
**Steps**:
- [ ] Create GenerateNBTNumberRequest
- [ ] Create GenerateNBTNumberResponse
- [ ] Add Success, Message properties

**T061**: Create CreateStudentValidator  
**Estimate**: 1.5 hours  
**Location**: `src/NBT.Application/Students/Validators/CreateStudentValidator.cs`  
**Shell Impact**: **NEW FILE**  
**Steps**:
- [ ] Create validator : AbstractValidator<CreateStudentRequest>
- [ ] Validate IDNumber (13 digits, Luhn check)
- [ ] Validate Email (format, uniqueness check)
- [ ] Validate Phone (SA format)
- [ ] Validate Grade (10-12 if provided)
- [ ] Add custom validation messages

**T062**: Configure AutoMapper for Students  
**Estimate**: 45 minutes  
**Location**: `src/NBT.Application/Common/Mappings/MappingProfile.cs`  
**Shell Impact**: **EXTENDS EXISTING** - Add new mappings  
**Steps**:
- [ ] Add Student → StudentDto mapping
- [ ] Add CreateStudentRequest → Student mapping
- [ ] Add UpdateStudentRequest → Student mapping
- [ ] Configure custom property mappings

### 2.2 Application Layer - Services (Day 6-7: 8 hours)

**T063**: Create IStudentService Interface  
**Estimate**: 1 hour  
**Location**: `src/NBT.Application/Students/Interfaces/IStudentService.cs`  
**Shell Impact**: **NEW FILE**  
**Steps**:
- [ ] Define GetAllAsync() method (paginated)
- [ ] Define GetByIdAsync() method
- [ ] Define GetByNBTNumberAsync() method
- [ ] Define GetByIDNumberAsync() method
- [ ] Define CreateAsync() method
- [ ] Define UpdateAsync() method
- [ ] Define DeleteAsync() method
- [ ] Define SearchAsync() method
- [ ] Add XML documentation

**T064**: Implement StudentService  
**Estimate**: 5 hours  
**Location**: `src/NBT.Application/Students/Services/StudentService.cs`  
**Shell Impact**: **NEW FILE**  
**Steps**:
- [ ] Create StudentService : IStudentService
- [ ] Inject IApplicationDbContext, IMapper, INBTNumberGenerator
- [ ] Implement GetAllAsync() with pagination
- [ ] Implement GetByIdAsync() with error handling
- [ ] Implement GetByNBTNumberAsync()
- [ ] Implement CreateAsync() with NBT number generation
- [ ] Implement UpdateAsync()
- [ ] Implement DeleteAsync() with soft delete
- [ ] Implement SearchAsync() with filtering
- [ ] Add logging throughout
- [ ] Add validation checks

**T065**: Create INBTNumberGenerator Interface  
**Estimate**: 30 minutes  
**Location**: `src/NBT.Application/Students/Interfaces/INBTNumberGenerator.cs`  
**Shell Impact**: **NEW FILE**  
**Steps**:
- [ ] Define GenerateAsync() method
- [ ] Define GetNextSequenceAsync() method

**T066**: Implement NBTNumberGenerator Service  
**Estimate**: 1.5 hours  
**Location**: `src/NBT.Application/Students/Services/NBTNumberGenerator.cs`  
**Shell Impact**: **NEW FILE**  
**Steps**:
- [ ] Create NBTNumberGenerator : INBTNumberGenerator
- [ ] Implement GetNextSequenceAsync() (query max from DB)
- [ ] Implement GenerateAsync() using NBTNumber.Generate()
- [ ] Add retry logic for collisions
- [ ] Add transaction support
- [ ] Add logging

### 2.3 Application Layer - Unit Tests (Day 7: 4 hours)

**T067**: Create StudentService Unit Tests  
**Estimate**: 4 hours  
**Location**: `tests/NBT.Application.Tests/Students/StudentServiceTests.cs`  
**Shell Impact**: **NEW TEST PROJECT**  
**Steps**:
- [ ] Setup test project with xUnit, Moq, FluentAssertions
- [ ] Mock IApplicationDbContext
- [ ] Test GetAllAsync() - returns paged results
- [ ] Test GetByIdAsync() - found scenario
- [ ] Test GetByIdAsync() - not found scenario
- [ ] Test CreateAsync() - valid student
- [ ] Test CreateAsync() - NBT number auto-generated
- [ ] Test CreateAsync() - duplicate ID number (fails)
- [ ] Test UpdateAsync() - success
- [ ] Test UpdateAsync() - not found
- [ ] Test DeleteAsync() - success
- [ ] Test SearchAsync() - by name
- [ ] Test SearchAsync() - by NBT number
- [ ] Test SearchAsync() - by grade
- [ ] Verify 15+ test cases pass

### 2.4 API Layer - Controller (Day 7-8: 8 hours)

**T068**: Create StudentsController  
**Estimate**: 5 hours  
**Location**: `src/NBT.WebAPI/Controllers/StudentsController.cs`  
**Shell Impact**: **NEW FILE**  
**Steps**:
- [ ] Create StudentsController : ControllerBase
- [ ] Add [Authorize(Roles = "Admin,Staff")]
- [ ] Inject IStudentService
- [ ] Implement GET /api/students (paginated)
- [ ] Implement GET /api/students/{id}
- [ ] Implement GET /api/students/nbt/{nbtNumber}
- [ ] Implement GET /api/students/id/{idNumber}
- [ ] Implement POST /api/students [Admin only]
- [ ] Implement PUT /api/students/{id} [Admin only]
- [ ] Implement DELETE /api/students/{id} [Admin only]
- [ ] Implement POST /api/students/generate-nbt-number [Admin]
- [ ] Implement POST /api/students/validate-id [AllowAnonymous]
- [ ] Add Swagger documentation
- [ ] Add error handling

**T069**: Add Integration Tests for StudentsController  
**Estimate**: 3 hours  
**Location**: `tests/NBT.IntegrationTests/Controllers/StudentsControllerTests.cs`  
**Shell Impact**: **NEW TEST PROJECT**  
**Steps**:
- [ ] Setup WebApplicationFactory
- [ ] Setup test database
- [ ] Test GET /api/students - returns 200
- [ ] Test GET /api/students/{id} - returns 200 or 404
- [ ] Test POST /api/students - creates student (201)
- [ ] Test POST /api/students - duplicate ID fails (400)
- [ ] Test PUT /api/students/{id} - updates (200)
- [ ] Test DELETE /api/students/{id} - deletes (204)
- [ ] Test authorization (401 without token)
- [ ] Test role authorization (403 for Staff on POST)
- [ ] Verify 10+ integration tests pass

### 2.5 Dependency Injection (Day 8: 1 hour)

**T070**: Register Student Services in DI  
**Estimate**: 30 minutes  
**Location**: `src/NBT.Infrastructure/DependencyInjection.cs`  
**Shell Impact**: **EXTENDS EXISTING**  
**Steps**:
- [ ] Add builder.Services.AddScoped<IStudentService, StudentService>()
- [ ] Add builder.Services.AddScoped<INBTNumberGenerator, NBTNumberGenerator>()
- [ ] Verify registration in Program.cs

**T071**: Configure FluentValidation  
**Estimate**: 30 minutes  
**Location**: `src/NBT.WebAPI/Program.cs`  
**Shell Impact**: **EXTENDS EXISTING**  
**Steps**:
- [ ] Add FluentValidation.AspNetCore package if missing
- [ ] Register validators assembly
- [ ] Configure automatic validation

### 2.6 UI Layer - API Service (Day 8: 4 hours)

**T072**: Create StudentApiService  
**Estimate**: 2 hours  
**Location**: `src/NBT.WebUI/Services/StudentApiService.cs`  
**Shell Impact**: **NEW FILE**  
**Steps**:
- [ ] Create StudentApiService class
- [ ] Inject HttpClient
- [ ] Implement GetAllAsync() - calls GET /api/students
- [ ] Implement GetByIdAsync() - calls GET /api/students/{id}
- [ ] Implement CreateAsync() - calls POST /api/students
- [ ] Implement UpdateAsync() - calls PUT /api/students/{id}
- [ ] Implement DeleteAsync() - calls DELETE /api/students/{id}
- [ ] Implement GenerateNBTNumberAsync()
- [ ] Implement ValidateIDNumberAsync()
- [ ] Add error handling and logging

**T073**: Register StudentApiService in DI  
**Estimate**: 15 minutes  
**Location**: `src/NBT.WebUI/Program.cs`  
**Shell Impact**: **EXTENDS EXISTING**  
**Steps**:
- [ ] Add builder.Services.AddScoped<StudentApiService>()
- [ ] Configure HttpClient base address

**T074**: Create StudentGrid Component  
**Estimate**: 1.5 hours  
**Location**: `src/NBT.WebUI/Components/DataGrids/StudentGrid.razor`  
**Shell Impact**: **NEW COMPONENT**  
**Steps**:
- [ ] Create reusable FluentDataGrid for students
- [ ] Add columns: NBT Number, Name, ID Number, Email, School, Grade
- [ ] Add sorting support
- [ ] Add row click event
- [ ] Add Fluent UI styling

**T075**: Create StudentForm Component  
**Estimate**: 30 minutes  
**Location**: `src/NBT.WebUI/Components/Forms/StudentForm.razor`  
**Shell Impact**: **NEW COMPONENT**  
**Steps**:
- [ ] Create reusable form component
- [ ] Add fields: ID Number, First/Last Name, Email, Phone
- [ ] Add validation
- [ ] Add Fluent UI styling

### 2.7 UI Layer - Admin Pages (Day 9: 8 hours)

**T076**: Create Students Index Page  
**Estimate**: 3 hours  
**Location**: `src/NBT.WebUI/Pages/Admin/Students/Index.razor`  
**Shell Impact**: **NEW PAGE**  
**Steps**:
- [ ] Create page with @page "/admin/students"
- [ ] Add [Authorize(Roles = "Admin,Staff")]
- [ ] Load students on page init
- [ ] Display StudentGrid component
- [ ] Add search box (name, NBT, ID)
- [ ] Add filter dropdown (grade)
- [ ] Add "Add Student" button (Admin only)
- [ ] Add pagination controls
- [ ] Add loading spinner
- [ ] Add error messages
- [ ] Style with Fluent UI

**T077**: Create Student Create Page  
**Estimate**: 2.5 hours  
**Location**: `src/NBT.WebUI/Pages/Admin/Students/Create.razor`  
**Shell Impact**: **NEW PAGE**  
**Steps**:
- [ ] Create page with @page "/admin/students/create"
- [ ] Add [Authorize(Roles = "Admin")]
- [ ] Use StudentForm component
- [ ] Add ID number validation on blur
- [ ] Auto-populate DOB and Gender from ID
- [ ] Generate NBT number on submit
- [ ] Handle form submission
- [ ] Navigate to Index on success
- [ ] Add cancel button

**T078**: Create Student Edit Page  
**Estimate**: 2.5 hours  
**Location**: `src/NBT.WebUI/Pages/Admin/Students/Edit.razor`  
**Shell Impact**: **NEW PAGE**  
**Steps**:
- [ ] Create page with @page "/admin/students/edit/{id:guid}"
- [ ] Add [Authorize(Roles = "Admin")]
- [ ] Load student by ID
- [ ] Pre-populate StudentForm component
- [ ] Disable ID number field (immutable)
- [ ] Handle form submission
- [ ] Navigate to Index on success
- [ ] Add delete button with confirmation

### 2.8 UI Layer - Navigation (Day 9: 1 hour)

**T079**: Update Admin Navigation Menu  
**Estimate**: 30 minutes  
**Location**: `src/NBT.WebUI/Components/Layout/AdminNavMenu.razor`  
**Shell Impact**: **EXTENDS EXISTING**  
**Steps**:
- [ ] Add "Students" menu item
- [ ] Link to /admin/students
- [ ] Add icon (FluentIcon)
- [ ] Position after "Users"

**T080**: Add Route Configuration  
**Estimate**: 30 minutes  
**Location**: `src/NBT.WebUI/App.razor` or routing configuration  
**Shell Impact**: **EXTENDS EXISTING**  
**Steps**:
- [ ] Verify route /admin/students is registered
- [ ] Verify route /admin/students/create is registered
- [ ] Verify route /admin/students/edit/{id} is registered
- [ ] Test navigation works

### 2.9 Phase 2 Testing & Documentation (Day 10: 4 hours)

**T081**: Manual Testing - Student CRUD  
**Estimate**: 2 hours  
**Steps**:
- [ ] Test create student with valid data
- [ ] Test create student with invalid ID number (should fail)
- [ ] Test create student with duplicate email (should fail)
- [ ] Test NBT number auto-generation
- [ ] Test search by name
- [ ] Test search by NBT number
- [ ] Test filter by grade
- [ ] Test update student
- [ ] Test delete student with confirmation
- [ ] Test pagination (create 60+ students)

**T082**: API Documentation  
**Estimate**: 1 hour  
**Location**: Swagger UI  
**Steps**:
- [ ] Open http://localhost:5000/swagger
- [ ] Verify all 9 Students endpoints documented
- [ ] Test each endpoint in Swagger
- [ ] Verify request/response schemas correct
- [ ] Add XML comments if missing

**T083**: User Documentation  
**Estimate**: 30 minutes  
**Location**: `docs/user-guide/student-management.md`  
**Shell Impact**: **NEW DOCUMENTATION**  
**Steps**:
- [ ] Document how to add students
- [ ] Document search functionality
- [ ] Document NBT number generation
- [ ] Add screenshots

**T084**: Phase 2 Sign-Off  
**Estimate**: 30 minutes  
**Steps**:
- [ ] Demo student CRUD to team
- [ ] Demo NBT number generation
- [ ] Show 30+ tests passing
- [ ] Confirm Phase 2 complete
- [ ] Get approval to proceed to Phase 3

**Phase 2 Deliverables**:
- ✅ Student service with CRUD operations
- ✅ NBT number generator with Luhn checksum
- ✅ 9 API endpoints (100% functional)
- ✅ 3 admin pages (Index, Create, Edit)
- ✅ Search and filtering
- ✅ 30+ tests (unit + integration)
- ✅ API documentation

---

## 📝 REGISTRATION WIZARD & BOOKING MODULE

**Duration**: Week 3-4 (10 days, 80 hours)  
**Priority**: 🔴 CRITICAL  
**Dependencies**: Phase 2 complete  
**Shell Impact**: **NEW WORKFLOW** - Multi-step wizard

*[Continues with 58 tasks for Phase 3...]*

---

## 💳 PAYMENT INTEGRATION (EASYPAY)

**Duration**: Week 5 (5 days, 40 hours)  
**Priority**: 🔴 CRITICAL  
**Dependencies**: Phase 3 complete  
**Shell Impact**: **NEW INTEGRATION** - External service

*[Continues with 38 tasks for Phase 4...]*

---

## 🏢 VENUE & ROOM MANAGEMENT

**Duration**: Week 6 (5 days, 40 hours)  
**Priority**: 🟡 HIGH  
**Dependencies**: Phase 1 complete  
**Shell Impact**: **NEW MODULE**

*[Continues with 32 tasks for Phase 5...]*

---

## 📅 TEST SESSIONS MANAGEMENT

**Duration**: Week 7 (5 days, 40 hours)  
**Priority**: 🟡 HIGH  
**Dependencies**: Phase 5 complete  
**Shell Impact**: **NEW MODULE**

*[Continues with 35 tasks for Phase 6...]*

---

## 📊 TEST RESULTS IMPORT & MANAGEMENT

**Duration**: Week 8 (5 days, 40 hours)  
**Priority**: 🟡 HIGH  
**Dependencies**: Phase 2 complete  
**Shell Impact**: **NEW MODULE** - Excel integration

*[Continues with 28 tasks for Phase 7...]*

---

## 🎛️ STAFF & ADMIN DASHBOARDS

**Duration**: Week 9 (5 days, 40 hours)  
**Priority**: 🟡 MEDIUM  
**Dependencies**: Phase 2-7 complete  
**Shell Impact**: **EXTENDS EXISTING** - Admin area expansion

*[Continues with 36 tasks for Phase 8...]*

---

## 📈 REPORTING & ANALYTICS

**Duration**: Week 10 (5 days, 40 hours)  
**Priority**: 🟡 MEDIUM  
**Dependencies**: All data modules complete  
**Shell Impact**: **NEW MODULE** - Reporting engine

*[Continues with 30 tasks for Phase 9...]*

---

## 🧪 TESTING, SECURITY & DEPLOYMENT

**Duration**: Week 11-12 (10 days, 120 hours)  
**Priority**: 🔴 CRITICAL  
**Dependencies**: All phases complete  
**Shell Impact**: **QUALITY ASSURANCE** - Testing & hardening

*[Continues with 126 tasks for Phase 10...]*

---

## 📊 TASK TRACKING TEMPLATE

Use this template for tracking task progress:

```markdown
## Task ID: T###
**Status**: [ ] Not Started | [🔄] In Progress | [✅] Complete | [❌] Blocked
**Assignee**: [Name]
**Started**: YYYY-MM-DD
**Completed**: YYYY-MM-DD
**Actual Hours**: X.X
**Notes**: [Any important notes, blockers, or decisions]
```

---

## 🎯 CRITICAL SUCCESS FACTORS

1. **Phase 1 Must Complete First** - All subsequent phases depend on entities
2. **Test Coverage Mandatory** - 80% minimum before moving to next phase
3. **Code Review Required** - All PRs must be reviewed before merge
4. **Shell Integration** - Mark each task as NEW, EXTENDS, or COMPLETES existing
5. **Documentation Updated** - Keep docs in sync with code

---

**TASK BREAKDOWN VERSION**: 1.0  
**LAST UPDATED**: 2025-11-08  
**TOTAL TASKS**: 485  
**READY TO BEGIN**: ✅ YES

**Next Action**: Begin Phase 1, Task T016 (Create ValueObject Base Class)
