# NBT Constitution Critical Updates - 2025-11-09

## 🎯 Purpose
This document captures all critical business rules, entity updates, and workflow changes discussed and confirmed for the NBT Integrated System.

---

## 🔥 CRITICAL BUSINESS RULES

### 1. Test Results Structure

**AQL Test** generates results for **2 domains**:
- **Academic Literacy (AL)** with performance level
- **Quantitative Literacy (QL)** with performance level

**MAT Test** (includes AQL + Mathematics) generates results for **3 domains**:
- **Academic Literacy (AL)** with performance level
- **Quantitative Literacy (QL)** with performance level
- **Mathematics (MAT)** with performance level

**Performance Levels** (applied to each domain):
- Basic Lower
- Basic Upper
- Intermediate Lower
- Intermediate Upper
- Proficient Lower
- Proficient Upper

**Barcode System**:
- Each test write has a **unique barcode**
- Barcode identifies the actual answer sheet used
- If a student writes 2 tests, each has a **different barcode**
- Essential for differentiating multiple test attempts

### 2. Student Registration & ID Types

**Three ID Types Supported:**
1. **SA_ID** (South African ID Number)
   - 13 digits
   - Luhn validation required
   - Auto-extract DOB and Gender from ID
   
2. **FOREIGN_ID** (Foreign ID Number)
   - 6-20 characters
   - Alphanumeric validation
   - Requires Nationality and Country of Origin
   
3. **PASSPORT** (Passport Number)
   - 6-20 characters
   - Alphanumeric validation
   - Requires Nationality and Country of Origin

### 3. Student Booking Rules (STRICTLY ENFORCED)

- **Booking Period**: Anytime after April 1 (Year Intake start date)
- **One Active Booking**: Student can only book **one test at a time**
- **Rebooking**: Only allowed **after closing date** of current booking has passed
- **Annual Limit**: Maximum **2 tests per year**
- **Test Validity**: Tests valid for **3 years from booking date**
- **Booking Changes**: Allowed **before close of booking date**

### 4. Registration Wizard Structure (4 Steps)

**Step 1: Account & Personal Information (COMBINED)**
- ID Type selection (SA_ID, FOREIGN_ID, PASSPORT)
- ID Number entry with validation
- Duplicate prevention check
- Personal details: First Name, Last Name, Email, Phone
- If SA_ID: Auto-extract DOB and Gender
- Manual entry: DOB (if not SA_ID), Gender, Ethnicity
- **Age NOT required** if DOB is present (calculated automatically)

**Step 2: Academic & Test Selection (COMBINED)**
- Academic background (school, grade, year)
- Test type selection (AQL or MAT)
- Venue and session selection
- Special accommodations

**Step 3: Pre-Test Questionnaire**
- Survey questions for research and equity reporting
- Background questionnaire responses

**Step 4: Review & Confirmation**
- Summary of all information
- Terms acceptance
- **NBT Number generated automatically upon submission**
- Navigate to login page after success

### 5. Test Session Relationships (CRITICAL)

**IMPORTANT**: `TestSession` entity is linked to **`TestVenue`**, NOT to `Room`

- TestSession belongs to Venue (many-to-one)
- Room entities managed separately under Venue
- RoomAllocation links students to specific rooms within a venue for a session

### 6. CI/CD & GitHub Workflow

- **Successful builds** MUST be followed by push to GitHub
- **New phases** start with a GitHub branch
- **Phase completion** requires full testing before merge to main
- **Branch naming**: `feature/{phase-name}`
- **Merge strategy**: Squash and merge

---

## 📊 UPDATED ENTITY STRUCTURE

### TestResult Entity
```csharp
public class TestResult
{
    public Guid TestResultId { get; set; }
    public Guid StudentId { get; set; }
    public Guid TestSessionId { get; set; }
    public string Barcode { get; set; } // Unique per test write
    public TestType TestType { get; set; } // AQL or MAT
    public DateTime TestDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    
    // Navigation
    public Student Student { get; set; }
    public TestSession TestSession { get; set; }
    public ICollection<TestResultDomain> Domains { get; set; }
}

public enum TestType
{
    AQL,  // AL + QL only
    MAT   // AL + QL + MAT
}
```

### TestResultDomain Entity (NEW)
```csharp
public class TestResultDomain
{
    public Guid TestResultDomainId { get; set; }
    public Guid TestResultId { get; set; }
    public DomainType DomainType { get; set; } // AL, QL, or MAT
    public decimal Score { get; set; } // 0-100
    public PerformanceLevel PerformanceLevel { get; set; }
    public decimal Percentile { get; set; } // 0-100
    public DateTime CreatedDate { get; set; }
    
    // Navigation
    public TestResult TestResult { get; set; }
}

public enum DomainType
{
    AL,   // Academic Literacy
    QL,   // Quantitative Literacy
    MAT   // Mathematics
}

public enum PerformanceLevel
{
    BasicLower,
    BasicUpper,
    IntermediateLower,
    IntermediateUpper,
    ProficientLower,
    ProficientUpper
}
```

### Student Entity (UPDATED)
```csharp
public class Student
{
    public Guid StudentId { get; set; }
    public string NBTNumber { get; set; } // 14-digit Luhn-validated
    public IDType IDType { get; set; } // SA_ID, FOREIGN_ID, PASSPORT
    public string IDNumber { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    
    // Auto-extracted from SA_ID or manually entered
    public DateTime? DateOfBirth { get; set; }
    public Gender? Gender { get; set; }
    
    // Manually entered
    public string Ethnicity { get; set; }
    
    // For Foreign ID and Passport
    public string Nationality { get; set; }
    public string CountryOfOrigin { get; set; }
    
    // Calculated (not stored separately if DOB exists)
    public int Age => DateOfBirth.HasValue 
        ? DateTime.Now.Year - DateOfBirth.Value.Year 
        : 0;
    
    // Navigation
    public ICollection<Registration> Registrations { get; set; }
    public ICollection<TestResult> TestResults { get; set; }
    public ICollection<PreTestQuestionnaire> Questionnaires { get; set; }
}

public enum IDType
{
    SA_ID,
    FOREIGN_ID,
    PASSPORT
}

public enum Gender
{
    Male,
    Female,
    Other
}
```

### PreTestQuestionnaire Entity (NEW)
```csharp
public class PreTestQuestionnaire
{
    public Guid QuestionnaireId { get; set; }
    public Guid StudentId { get; set; }
    public Guid RegistrationId { get; set; }
    public string Responses { get; set; } // JSON storage
    public DateTime CompletedDate { get; set; }
    
    // Navigation
    public Student Student { get; set; }
    public Registration Registration { get; set; }
}
```

---

## 🔄 UPDATED RELATIONSHIPS

```yaml
Entity Relationships:
  - Student 1:N Registration
  - Student 1:N TestResult
  - Student 1:N PreTestQuestionnaire
  
  - Registration 1:1 Payment
  - Registration 1:1 PreTestQuestionnaire
  
  - TestResult 1:N TestResultDomain
    * AQL test: 2 domains (AL, QL)
    * MAT test: 3 domains (AL, QL, MAT)
  
  - TestSession N:1 Venue (CRITICAL: NOT linked to Room)
  - Venue 1:N Room
  
  - TestSession 1:N RoomAllocation
  - Room 1:N RoomAllocation
  - Student 1:N RoomAllocation
```

---

## 🎨 TECHNOLOGY STACK

### Frontend
- **Framework**: Blazor Web App Interactive Auto (.NET 9.0)
  - NOTE: .NET 9 passed RC over a year ago, now stable
- **UI Library**: Microsoft Fluent UI Blazor Components
  - **NO MudBlazor** - All MudBlazor components must be replaced

### Backend
- **Framework**: ASP.NET Core Web API (.NET 9.0)
- **Database**: MS SQL Server with EF Core
- **Authentication**: JWT with role-based access
- **Payment**: EasyPay integration

### Reporting
- **Excel Export**: EPPlus or ClosedXML
- **PDF Export**: QuestPDF or iText7
- **Charts**: FluentUI Charts

---

## 📋 STUDENT ACTIVITIES SUMMARY

1. **Account Creation & Login**
   - Register with SA_ID, Foreign ID, or Passport
   - OTP verification
   - Duplicate prevention

2. **NBT Number Generation**
   - Automatic upon successful registration
   - 14-digit Luhn-validated unique identifier

3. **Registration Wizard**
   - 4-step process with combined sections
   - Auto-save progress
   - Resume capability

4. **Booking & Payment**
   - One active booking at a time
   - Max 2 tests per year
   - 3-year validity from booking date
   - EasyPay integration

5. **Special Sessions**
   - Remote writer support
   - Automatic routing to NBT admin team

6. **Pre-Test Questionnaire**
   - Research and equity reporting
   - Completed after registration

7. **Results Access**
   - View results per domain (AL, QL, MAT)
   - Unique barcode per test
   - Performance levels displayed
   - Download certificates

8. **Profile Management**
   - Update personal details
   - Upload documents
   - Password reset
   - Full audit logging

9. **Notifications**
   - Email/SMS for registration, payment, reminders, results

10. **Account Retention**
    - Active for future cycles
    - Historical data preserved

---

## ✅ NON-NEGOTIABLE RULES

1. All code must pass CI/CD pipeline before merge
2. No direct database access from UI layers
3. All API endpoints must be authorized
4. All inputs validated server-side (required)
5. All data modifications audited
6. Secrets in Azure Key Vault or User Secrets
7. Luhn validation for NBT numbers and SA IDs
8. **FluentUI only** - NO MudBlazor components
9. TestSessions linked to Venue, NOT Room
10. Student booking rules strictly enforced
11. Result barcodes unique per test write
12. Foreign ID/Passport support mandatory
13. Successful builds followed by GitHub push
14. New phases start on GitHub branch
15. .NET 9.0 framework (stable, passed RC)

---

## 📝 IMPLEMENTATION PHASES

### Completed Phases
- ✅ Phase 0: Shell Audit
- ✅ Phase 1: Foundation & Database
- ✅ Phase 2: Student Module
- ✅ Phase 3: Registration Wizard (in progress)
- ✅ Phase 4: Booking & Payment
- ✅ Phase 5: Reporting
- ✅ Phase 6: Authentication

### Remaining Phases
- 🔄 Phase 3: Registration Wizard (needs barcode + domain updates)
- 🔄 Phase 5: Results Management (needs domain entities)
- 🔄 Phase 7: Venue Management (needs test session fixes)
- 🔜 Phase 8: Testing & Deployment

---

## 🚀 NEXT ACTIONS

1. **Update TestResult entities** to include Barcode and TestResultDomain relationship
2. **Fix Registration Wizard** to match 4-step structure with combined sections
3. **Implement PreTestQuestionnaire** entity and wizard step
4. **Update Student entity** with Age, Gender, Ethnicity fields
5. **Replace all MudBlazor** components with FluentUI
6. **Implement booking validation** service to enforce rules
7. **Create TestResultDomain** CRUD operations
8. **Update results import** to handle domains and barcodes
9. **Test all workflows** end-to-end
10. **Push to GitHub** after successful build

---

**Document Version**: 1.0  
**Last Updated**: 2025-11-09  
**Status**: ACTIVE - READY FOR IMPLEMENTATION
