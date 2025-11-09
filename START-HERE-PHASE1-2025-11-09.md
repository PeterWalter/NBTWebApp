# 🎯 START HERE - Phase 1 Implementation Guide

**Date:** 2025-11-09  
**Current Phase:** Phase 1 - Registration Wizard (Resumable)  
**Status:** ✅ READY TO START

---

## 🚀 Quick Start (5 Minutes)

### 1. Read This First
You are about to implement the **Resumable Registration Wizard** for the NBT Web Application.

**What You Need to Know:**
- ✅ Phase 0 (Shell Audit) is **COMPLETE**
- ✅ All entities are in place
- ✅ Database is migrated
- ✅ Full specification is available
- ✅ You have everything you need to succeed

### 2. Essential Documents (Read in Order)

📘 **START HERE:**
1. This document (you're reading it)
2. `SPECKIT-COMPLETE-IMPLEMENTATION-2025-11-09.md` → Section 2.3 (Registration Wizard)
3. `START-IMPLEMENTATION-NOW-2025-11-09.md` → Phase 1 Checklist

📗 **Reference While Coding:**
- Constitution → Non-negotiable rules
- Data Contracts → Entity fields and DTOs
- API Endpoints → Interface contracts

📕 **When You're Done:**
- Code Review Checklist → Before committing
- Session Complete → Document your completion

---

## 🎯 Phase 1 Objective

**Build a 3-step resumable registration wizard** that:
1. Collects student information in 3 steps
2. Auto-saves progress after each step
3. Allows students to resume if interrupted
4. Generates NBT number on completion
5. Sends OTP for email verification
6. Validates SA ID and extracts DOB/Gender
7. Supports Foreign ID and Passport

---

## 📋 What You're Building

### Step 1: Personal & ID Information

**Fields:**
- ID Type: Dropdown (SA ID, Foreign ID, Passport)
- ID Number: Text with validation
- **If SA ID:**
  - Auto-extract Date of Birth
  - Auto-extract Gender
- **If Foreign/Passport:**
  - Date of Birth: Date picker
  - Gender: Dropdown
  - Nationality: Dropdown
  - Country of Origin: Dropdown
- First Name
- Last Name
- Email (will receive OTP)
- Phone Number

**Validations:**
- SA ID: 13 digits, Luhn check
- Foreign/Passport: 6-20 alphanumeric
- Email: Valid format, unique
- Duplicate ID check

**On Save:**
- Create Student record (if new)
- Set RegistrationStep = 1
- Set IsRegistrationComplete = false

### Step 2: Contact & Academic Information

**Fields:**
- Address Line 1
- Address Line 2 (optional)
- City
- Province/State
- Postal Code
- School Name
- Grade
- Home Language
- Age (calculated from DOB, display only)
- Gender (from Step 1, editable)
- Ethnicity (optional)
- Special Accommodation Required? (Yes/No)
- If Yes: Describe needs

**On Save:**
- Update Student record
- Set RegistrationStep = 2

### Step 3: Survey Questionnaire

**Fields:**
- Why are you taking the NBT? (multi-select)
- What field do you plan to study?
- Career interests
- Do you have access to a computer? (Yes/No)
- Do you have internet access? (Yes/No)
- If yes: Internet speed
- Additional comments (optional)

**On Completion:**
- Update Student record
- Set RegistrationStep = 3
- Set IsRegistrationComplete = true
- Set RegistrationCompletedDate
- **Generate NBT Number** (14 digits, Luhn)
- Send OTP email
- Redirect to Login page

### Resume Logic

**When student returns:**
```csharp
if (!student.IsRegistrationComplete)
{
    // Resume from last step
    switch (student.RegistrationStep)
    {
        case 0:
        case 1:
            NavigateTo("/register/step1");
            break;
        case 2:
            NavigateTo("/register/step2");
            break;
        case 3:
            NavigateTo("/register/step3");
            break;
    }
}
else
{
    // Registration complete
    NavigateTo("/dashboard");
}
```

---

## 🛠️ Implementation Checklist

### A. Blazor Components (Frontend)

#### 1. Create Wizard Orchestrator
**File:** `src/NBT.WebUI.Client/Pages/Registration/RegistrationWizard.razor`

```razor
@page "/register"
@inject IStudentService StudentService
@inject NavigationManager Navigation

<FluentCard>
    <FluentProgressRing Value="@CurrentStep" Max="3" />
    
    @if (CurrentStep == 1)
    {
        <Step1PersonalInfo OnNext="HandleStep1Complete" />
    }
    else if (CurrentStep == 2)
    {
        <Step2ContactAcademic OnNext="HandleStep2Complete" OnBack="GoBack" />
    }
    else if (CurrentStep == 3)
    {
        <Step3Survey OnComplete="HandleStep3Complete" OnBack="GoBack" />
    }
</FluentCard>
```

#### 2. Create Step 1 Component
**File:** `src/NBT.WebUI.Client/Pages/Registration/Step1PersonalInfo.razor`

**Key Features:**
- [ ] ID Type selector
- [ ] ID Number input with validation
- [ ] SA ID validation (13 digits, Luhn)
- [ ] Auto-extract DOB/Gender for SA ID
- [ ] Manual DOB/Gender for Foreign/Passport
- [ ] Email and Phone inputs
- [ ] Duplicate check on blur
- [ ] Next button (validates before proceeding)

#### 3. Create Step 2 Component
**File:** `src/NBT.WebUI.Client/Pages/Registration/Step2ContactAcademic.razor`

**Key Features:**
- [ ] Address fields
- [ ] School information
- [ ] Ethnicity dropdown
- [ ] Special accommodation checkbox
- [ ] Back and Next buttons

#### 4. Create Step 3 Component
**File:** `src/NBT.WebUI.Client/Pages/Registration/Step3Survey.razor`

**Key Features:**
- [ ] Survey questions
- [ ] Multi-select for motivation
- [ ] Text areas for interests
- [ ] Computer/Internet access checkboxes
- [ ] Back and Complete buttons

### B. Services (Backend Logic)

#### 5. Create Student Service
**File:** `src/NBT.Application/Students/StudentService.cs`

**Methods:**
- [ ] `RegisterStep1Async(RegisterStudentStep1Request)`
- [ ] `RegisterStep2Async(RegisterStudentStep2Request)`
- [ ] `RegisterStep3Async(RegisterStudentStep3Request)`
- [ ] `GetRegistrationProgressAsync(studentId)`
- [ ] `CheckDuplicateIdAsync(idNumber)`

#### 6. Create NBT Number Generator
**File:** `src/NBT.Application/Students/NBTNumberGenerator.cs`

**Algorithm:**
```csharp
// Format: YYYY + 10-digit sequence + check digit = 14 digits
// Example: 20240000000123 (where 3 is Luhn check digit)

public string GenerateNBTNumber(int year)
{
    var sequence = GetNextSequence(); // From database
    var partial = $"{year}{sequence:D10}";
    var checkDigit = CalculateLuhnCheckDigit(partial);
    return $"{partial}{checkDigit}";
}
```

#### 7. Create SA ID Validator
**File:** `src/NBT.Application/Students/SAIDValidator.cs`

**Methods:**
- [ ] `IsValidSAID(string idNumber)` → Luhn check
- [ ] `ExtractDateOfBirth(string idNumber)` → Parse YYMMDD
- [ ] `ExtractGender(string idNumber)` → Parse G digit

#### 8. Create OTP Service
**File:** `src/NBT.Application/Students/OTPService.cs`

**Methods:**
- [ ] `GenerateOTP()` → 6-digit code
- [ ] `SendOTPEmail(email, otp)`
- [ ] `VerifyOTP(studentId, otp)` → Check and expire

### C. API Endpoints

#### 9. Create Students Controller
**File:** `src/NBT.WebAPI/Controllers/StudentsController.cs`

**Endpoints:**
```csharp
[HttpPost("register/step1")]
public async Task<IActionResult> RegisterStep1(RegisterStudentStep1Request request)

[HttpPost("register/step2")]
public async Task<IActionResult> RegisterStep2(RegisterStudentStep2Request request)

[HttpPost("register/step3")]
public async Task<IActionResult> RegisterStep3(RegisterStudentStep3Request request)

[HttpGet("registration-progress/{id}")]
public async Task<IActionResult> GetRegistrationProgress(Guid id)

[HttpGet("check-duplicate")]
public async Task<IActionResult> CheckDuplicate([FromQuery] string idNumber)

[HttpPost("verify-otp")]
public async Task<IActionResult> VerifyOTP(VerifyOTPRequest request)
```

### D. DTOs (Data Transfer Objects)

#### 10. Create Request DTOs
**File:** `src/NBT.Application/Students/DTOs/RegisterStudentStep1Request.cs`

See master specification for complete DTO definitions.

#### 11. Create Response DTOs
**File:** `src/NBT.Application/Students/DTOs/StudentProfileResponse.cs`

See master specification for complete DTO definitions.

---

## 🧪 Testing Checklist

### Unit Tests
- [ ] NBT number generation (Luhn validation)
- [ ] SA ID validation
- [ ] DOB extraction from SA ID
- [ ] Gender extraction from SA ID
- [ ] OTP generation
- [ ] Duplicate detection

### Integration Tests
- [ ] POST /api/students/register/step1
- [ ] POST /api/students/register/step2
- [ ] POST /api/students/register/step3
- [ ] GET /api/students/registration-progress/{id}
- [ ] GET /api/students/check-duplicate
- [ ] POST /api/students/verify-otp

### E2E Tests
- [ ] Complete wizard flow (3 steps)
- [ ] Resume from Step 1
- [ ] Resume from Step 2
- [ ] Resume from Step 3
- [ ] SA ID auto-fill
- [ ] Foreign ID manual entry
- [ ] Duplicate prevention
- [ ] OTP verification
- [ ] NBT number generation

### Manual Tests
- [ ] Navigate through all 3 steps
- [ ] Enter SA ID → Check auto-fill
- [ ] Enter Foreign ID → Manual fields appear
- [ ] Close browser → Reopen → Resume works
- [ ] Try duplicate ID → Error shown
- [ ] Complete wizard → NBT number generated
- [ ] Receive OTP email
- [ ] Verify OTP → Account activated

---

## 🚦 Success Criteria

### Phase 1 Complete When:
- [ ] All 3 wizard steps work
- [ ] Auto-save after each step
- [ ] Resume functionality works
- [ ] NBT number generates correctly
- [ ] SA ID validation works
- [ ] DOB/Gender auto-extraction works
- [ ] Foreign ID manual entry works
- [ ] Duplicate checking works
- [ ] OTP email sends
- [ ] OTP verification works
- [ ] All unit tests pass
- [ ] All integration tests pass
- [ ] E2E test passes
- [ ] Manual testing completed
- [ ] Code review passed
- [ ] Build succeeds
- [ ] Committed to Git
- [ ] Pushed to GitHub
- [ ] Merged to main

---

## 📝 Development Workflow

### 1. Start Feature Branch
```bash
cd "D:\projects\source code\NBTWebApp"
git checkout -b feature/phase1-registration-wizard-resumable
```

### 2. Build and Run
```bash
# Build
dotnet build

# Run API (Terminal 1)
cd src/NBT.WebAPI
dotnet run

# Run Blazor (Terminal 2)
cd src/NBT.WebUI
dotnet run
```

### 3. Develop Components
- Start with backend services
- Then API endpoints
- Then Blazor components
- Test each piece as you go

### 4. Test Thoroughly
```bash
# Unit tests
dotnet test

# Manual testing
# Navigate to https://localhost:7002/register
# Complete full wizard flow
# Test resume functionality
```

### 5. Code Review
- [ ] Check against Code Review Checklist in master doc
- [ ] Verify all Fluent UI (NO MudBlazor)
- [ ] Confirm all validations work
- [ ] Ensure auto-save works
- [ ] Test resume functionality

### 6. Commit and Push
```bash
git add .
git commit -m "Phase 1: Complete resumable registration wizard"
git push origin feature/phase1-registration-wizard-resumable
```

### 7. Merge to Main
```bash
# After testing and review
git checkout main
git merge feature/phase1-registration-wizard-resumable
git push origin main
```

---

## 🔍 Common Pitfalls to Avoid

### ❌ DON'T:
1. Use MudBlazor components (Use Fluent UI)
2. Link TestSession to Room (Link to Venue)
3. Skip validation steps
4. Forget to save RegistrationStep
5. Forget to implement resume logic
6. Skip OTP verification
7. Forget Luhn validation on NBT number
8. Forget to check for duplicates

### ✅ DO:
1. Use Fluent UI components
2. Save after each step
3. Implement full resume logic
4. Validate SA ID with Luhn
5. Auto-extract DOB/Gender for SA ID
6. Generate NBT number on completion
7. Send OTP email
8. Check for duplicate IDs
9. Test thoroughly before committing
10. Follow the specification exactly

---

## 📚 Reference Links

### In This Repository:
- `SPECKIT-COMPLETE-IMPLEMENTATION-2025-11-09.md` → Full specification
- `SPECKIT-CONSTITUTION.md` → Non-negotiable principles
- `START-IMPLEMENTATION-NOW-2025-11-09.md` → Quick reference

### Code Locations:
- Entities: `src/NBT.Domain/Entities/`
- Services: `src/NBT.Application/Students/`
- API: `src/NBT.WebAPI/Controllers/`
- Blazor: `src/NBT.WebUI.Client/Pages/Registration/`

---

## 🆘 Need Help?

### If Stuck:
1. Check the master specification document
2. Review the Student entity definition
3. Look at existing service implementations
4. Check the API endpoint specifications
5. Review the DTO definitions

### Common Questions:

**Q: Where do I find the NBT number generation algorithm?**
A: Section 3.2 in `SPECKIT-COMPLETE-IMPLEMENTATION-2025-11-09.md`

**Q: What's the SA ID validation logic?**
A: Section 8.1 in the master specification

**Q: How do I implement auto-save?**
A: Call the API endpoint after each step's "Next" button click

**Q: How does resume work?**
A: Check `RegistrationStep` field on student, navigate to appropriate step

**Q: What if student closes browser mid-registration?**
A: On next login, check `IsRegistrationComplete`. If false, resume from `RegistrationStep`.

---

## ⏱️ Estimated Timeline

### Day 1 (6-8 hours):
- Morning: Create services and DTOs
- Afternoon: Create API endpoints
- Test API endpoints with Swagger

### Day 2 (6-8 hours):
- Morning: Create Blazor components (Steps 1-3)
- Afternoon: Create wizard orchestrator
- Test wizard flow

### Day 3 (4-6 hours):
- Morning: Implement resume logic
- Test resume functionality
- Bug fixes

### Day 4 (2-4 hours):
- Write tests
- Code review
- Documentation
- Commit and merge

**Total: 2-3 days**

---

## 🎯 Your Mission

Build a **production-ready, resumable 3-step registration wizard** that:
1. Follows the specification exactly
2. Uses Fluent UI components
3. Auto-saves progress
4. Allows resume on interruption
5. Generates NBT numbers correctly
6. Validates SA IDs properly
7. Supports Foreign IDs/Passports
8. Sends OTP emails
9. Passes all tests
10. Makes stakeholders happy! 🎉

---

## 🚀 Ready? Let's Go!

```bash
cd "D:\projects\source code\NBTWebApp"
git checkout -b feature/phase1-registration-wizard-resumable
code .
```

**You've got this!** 💪

The specification is complete. The plan is clear. The architecture is solid. All you need to do is code it! 🚀

---

**Created:** 2025-11-09  
**Phase:** 1 of 11  
**Status:** READY TO START  

**Good luck!** 🍀
