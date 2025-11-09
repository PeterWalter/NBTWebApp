# NBT Integrated System - Constitution Updates (2025-11-09)

**Status:** COMPLETE  
**Date:** 2025-11-09  
**Version:** 3.1  
**Updated By:** SpecKit Comprehensive Review

---

## SUMMARY OF CRITICAL UPDATES

This document captures ALL new requirements, business rules, and architectural mandates added to the NBT Integrated System Constitution based on the comprehensive requirement gathering session.

---

## 1. REGISTRATION WIZARD CRITICAL FIXES

### 1.1 Step Structure (CORRECTED)
- **Step 1**: Personal Information + Account Creation (COMBINED)
  - ID Type, ID Number, Name, Contact, DOB/Gender/Ethnicity
  - SA ID auto-extraction for DOB and Gender
  
- **Step 2**: Academic + Test Selection (COMBINED)
  - School, Grade, Test Type, Venue/Session, Special Accommodations
  
- **Step 3**: Pre-Test Questionnaire (Survey)
  - Research and equity reporting questions
  
- **Step 4**: Review & Confirmation
  - Summary, Terms acceptance, Submit → NBT Number generation → Navigate to login

### 1.2 Critical Bug Fixes Required
```yaml
PROBLEM: Wizard jumps to end prematurely
CAUSE: 
  - Next button activation logic incorrect
  - Auto-fill from SA ID triggers step completion
  - Step validation not properly enforced
  
SOLUTION:
  - Next button MUST be disabled until current step valid
  - Auto-fill MUST NOT trigger navigation
  - Step progression MUST require explicit Next button click
  - Validation MUST run before enabling Next button
  - NBT number generation ONLY on final submission (server-side)
```

### 1.3 Registration Resumption (NEW REQUIREMENT)
```yaml
Feature: Resume Interrupted Registration
Implementation:
  - Save draft to database after each step completion
  - Store wizard state in sessionStorage (currentStep, completedSteps)
  - On page reload or return visit:
    * Fetch draft registration from database
    * Restore wizard to last completed step
    * Pre-fill all previously entered data
    * Allow continuation from interruption point
  
Draft Management:
  - Status: Draft (not submitted)
  - Expiry: 30 days of inactivity
  - Cleanup: Automated job deletes expired drafts
  
User Experience:
  - Welcome back message: "Continue your registration from Step X"
  - Option to start fresh (discards draft)
  - Progress indicator shows completed steps
```

---

## 2. PAYMENT SYSTEM ENHANCEMENTS

### 2.1 Bank Payment Upload (NEW REQUIREMENT)
```yaml
Feature: Upload Bank Payment Records
Purpose: Reconcile manual bank payments with bookings

File Format:
  - Type: CSV or Excel (.csv, .xlsx)
  - Required Columns:
    * StudentID or NBTNumber (identifier)
    * Amount (decimal, 2 decimals)
    * PaymentDate (date, format: YYYY-MM-DD)
    * BankReference (string, unique)
    * TransactionType (enum: EFT, Deposit, Transfer)
    * BranchCode (optional)
    * AccountNumber (optional, masked)

Validation Rules:
  1. File size max: 5 MB
  2. Max rows: 5,000
  3. NBTNumber must exist in system
  4. Amount must match expected payment or installment
  5. BankReference must be unique
  6. Date must be valid and within acceptable range (not future)

Processing Workflow:
  1. Admin uploads file via /admin/payments/upload
  2. System validates file format and headers
  3. System parses each row and validates data
  4. System matches payment to pending booking/installment
  5. System updates Payment status (Pending → Partial or Complete)
  6. System creates PaymentTransaction record
  7. System sends confirmation email to student
  8. System logs import in AuditLog

Reconciliation Logic:
  - Match by NBTNumber + Amount + Date (±3 days)
  - If multiple matches: Flag for manual review
  - If no match: Create unmatched payment record
  - Admin can manually link unmatched payments

Error Handling:
  - Collect all errors (don't fail on first)
  - Return detailed error report with row numbers
  - Transaction rollback if critical errors
  - Partial success allowed for non-critical errors

API Endpoint:
  POST /api/payments/upload
  [Authorize(Roles = "Admin,SuperUser")]
  Content-Type: multipart/form-data
  
Response:
  {
    "Success": true,
    "TotalRows": 150,
    "ProcessedCount": 145,
    "MatchedCount": 140,
    "UnmatchedCount": 5,
    "ErrorCount": 5,
    "Errors": [
      { "Row": 12, "Field": "NBTNumber", "Error": "Student not found" },
      { "Row": 45, "Field": "Amount", "Error": "No matching booking" }
    ],
    "UnmatchedPayments": [
      { "Row": 67, "NBTNumber": "202512345678", "Amount": 500.00, "Reason": "Multiple possible matches" }
    ]
  }
```

### 2.2 Payment Reconciliation Dashboard (NEW COMPONENT)
```yaml
Component: PaymentReconciliation.razor
Location: /admin/payments/reconciliation

Features:
  - View unmatched payments
  - Manually link payment to booking
  - View payment history per student
  - Filter by date range, status, amount
  - Export reconciliation report (Excel/PDF)

Actions:
  - Link Unmatched Payment (opens dialog, select booking)
  - Mark as Refund (payment reversal)
  - Mark as Duplicate (ignore)
  - Add Note (admin comments)
```

---

## 3. USER INTERFACE & NAVIGATION (NEW REQUIREMENTS)

### 3.1 Landing Page Structure
```yaml
Landing Page: / (Public Homepage)

Main Navigation Sections:
  1. Applicants
     - Register for NBT
     - NBT Information (What is NBT?)
     - Test Preparation Resources
     - View My Results (login required)
     - Frequently Asked Questions
     - Test Dates & Venues
     - Contact Us
  
  2. Institutions
     - Institutional Reports (login required)
     - Bulk Results Download (login required)
     - Research Data Access (login required)
     - Partner with NBT
     - Contact Institutional Services
  
  3. Educators
     - Teaching Resources
     - Workshops & Training
     - Assessment Tools
     - Research & Publications
     - Educator Portal (login required)
     - Contact Educator Services

Reference: Current NBT website (www.nbt.ac.za)
Content: Submenus MUST match current website structure
Updates: Content team responsible for keeping menus current

Video Integration:
  - Embed videos on relevant pages:
    * About NBT (homepage)
    * Test Preparation (resources page)
    * How to Register (registration info page)
    * Understanding Your Results (results page)
  - Source: Current NBT website video library
  - Player: YouTube embedded player
  - Accessibility: Captions required for all videos
```

### 3.2 Dashboard Navigation (Role-Based)
```yaml
After Login Flow:
  1. User authenticates (JWT token issued)
  2. System identifies user role (Student, Staff, Admin, SuperUser)
  3. System redirects to role-specific dashboard
  4. Dashboard displays left-side navigation menu

Student Dashboard: /dashboard/student
Left Menu:
  - 🏠 Home (dashboard overview)
  - 👤 My Profile (view/edit personal info)
  - 📅 My Bookings (view/manage bookings)
  - 📊 My Results (view/download results - payment restricted)
  - 📆 Test Dates (view available test sessions)
  - 💰 Payment History (view payments and installments)
  - 📞 Support (contact, FAQs, help)
  - 🚪 Logout

Staff Dashboard: /dashboard/staff
Left Menu:
  - 🏠 Home (dashboard overview)
  - 👥 Students (view student list - READ ONLY)
  - 📅 Bookings (view all bookings - READ ONLY)
  - 💰 Payments (view payment records - READ ONLY)
  - 📊 Results (view all results - READ ONLY)
  - 📈 Reports (generate reports)
  - 🚪 Logout

Admin Dashboard: /dashboard/admin
Left Menu:
  - 🏠 Home (dashboard overview)
  - 👥 Students (full CRUD)
  - 📅 Bookings (full CRUD)
  - 💰 Payments (full CRUD + upload bank payments)
  - 📊 Results (full CRUD + import results)
  - 🏢 Venues (full CRUD)
  - 🏠 Rooms (full CRUD)
  - 📆 Test Sessions (full CRUD)
  - 📈 Reports (generate + export)
  - 👤 My Profile
  - 🚪 Logout

SuperUser Dashboard: /dashboard/superuser
Left Menu:
  - 🏠 Home (dashboard overview)
  - 👥 Students (full CRUD)
  - 📅 Bookings (full CRUD)
  - 💰 Payments (full CRUD + upload + reconciliation)
  - 📊 Results (full CRUD + import)
  - 🏢 Venues (full CRUD)
  - 🏠 Rooms (full CRUD)
  - 📆 Test Sessions (full CRUD)
  - 📈 Reports (all reports + advanced analytics)
  - 👨‍💼 User Management (create/edit users, assign roles)
  - ⚙️ System Settings (configuration, integrations)
  - 📋 Audit Logs (view all system activity)
  - 📥 Data Imports (bulk imports, data migration)
  - 🚪 Logout

Design Standards:
  - Left sidebar: 240px width, collapsible
  - Active menu item: Highlighted with primary color
  - Icons: Fluent UI icons
  - Mobile: Hamburger menu, drawer overlay
  - Accessibility: Keyboard navigation, ARIA labels
```

---

## 4. TEST RESULT ENHANCEMENTS

### 4.1 Barcode System (CRITICAL REQUIREMENT)
```yaml
Purpose: Unique identifier for each test instance
Format: NBT{YYYYMMDD}-{TestType}-{SequenceNumber}
Example: NBT20250315-AQL-001234

Generation Rules:
  - Generated when result is imported
  - Format: "NBT" + TestDate(YYYYMMDD) + "-" + TestType + "-" + 6-digit sequence
  - Sequence: Daily counter per test type (resets each day)
  - Uniqueness: Database constraint (unique index on Barcode column)

Database Schema:
  TestResult.Barcode: string (20 chars, unique, not null)
  Index: CREATE UNIQUE INDEX IX_TestResult_Barcode ON TestResult(Barcode)

Usage:
  - Printed on result PDF certificate
  - Used for verification of authenticity
  - Differentiates multiple test attempts by same student
  - Barcode lookup API: GET /api/results/by-barcode/{barcode}

Validation:
  - Regex: ^NBT\d{8}-(AQL|MAT)-\d{6}$
  - Luhn checksum NOT used (barcode is separate from NBT number)
```

### 4.2 Result Domain Structure (CLARIFIED)
```yaml
AQL Test (Academic & Quantitative Literacy):
  Domains: 2
    1. Academic Literacy (AL)
       - Score: 0-100
       - Performance Level: BasicLower, BasicUpper, IntermediateLower, IntermediateUpper, ProficientLower, ProficientUpper
       - Percentile: 0-100
    
    2. Quantitative Literacy (QL)
       - Score: 0-100
       - Performance Level: BasicLower, BasicUpper, IntermediateLower, IntermediateUpper, ProficientLower, ProficientUpper
       - Percentile: 0-100

MAT Test (AQL + Mathematics):
  Domains: 3
    1. Academic Literacy (AL)
       - Same as AQL AL domain
    
    2. Quantitative Literacy (QL)
       - Same as AQL QL domain
    
    3. Mathematics (MAT)
       - Score: 0-100
       - Performance Level: BasicLower, BasicUpper, IntermediateLower, IntermediateUpper, ProficientLower, ProficientUpper
       - Percentile: 0-100

Database Relationships:
  TestResult 1:N TestResultDomain
  - AQL test: 2 domain records (AL, QL)
  - MAT test: 3 domain records (AL, QL, MAT)

Result PDF Certificate:
  - Header: Student Name, NBT Number, Test Date
  - Barcode: Unique test identifier (scannable barcode image)
  - Domains: Table with Score, Performance Level, Percentile
  - Footer: Validity period (3 years from test date)
  - Watermark: "Official NBT Result"
  - Security: Digital signature (optional future enhancement)
```

---

## 5. DATABASE SCHEMA ADDITIONS

### 5.1 New Entities

#### PaymentUpload
```sql
CREATE TABLE PaymentUploads (
    PaymentUploadId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    FileName NVARCHAR(255) NOT NULL,
    UploadedBy NVARCHAR(100) NOT NULL, -- User email
    UploadDate DATETIME NOT NULL DEFAULT GETDATE(),
    TotalRows INT NOT NULL,
    ProcessedCount INT NOT NULL,
    MatchedCount INT NOT NULL,
    UnmatchedCount INT NOT NULL,
    ErrorCount INT NOT NULL,
    Status NVARCHAR(20) NOT NULL, -- Processing, Completed, Failed
    ErrorReport NVARCHAR(MAX), -- JSON array of errors
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE()
);
```

#### BankPaymentRecord
```sql
CREATE TABLE BankPaymentRecords (
    BankPaymentRecordId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    PaymentUploadId UNIQUEIDENTIFIER NOT NULL,
    NBTNumber NVARCHAR(14) NOT NULL,
    Amount DECIMAL(18,2) NOT NULL,
    PaymentDate DATE NOT NULL,
    BankReference NVARCHAR(50) NOT NULL UNIQUE,
    TransactionType NVARCHAR(20) NOT NULL, -- EFT, Deposit, Transfer
    BranchCode NVARCHAR(10),
    AccountNumber NVARCHAR(20), -- Masked
    PaymentId UNIQUEIDENTIFIER NULL, -- FK to Payment (if matched)
    MatchStatus NVARCHAR(20) NOT NULL, -- Matched, Unmatched, ManualReview
    MatchedBy NVARCHAR(100), -- User who manually matched
    MatchedDate DATETIME,
    Notes NVARCHAR(500),
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    
    CONSTRAINT FK_BankPaymentRecord_PaymentUpload FOREIGN KEY (PaymentUploadId) REFERENCES PaymentUploads(PaymentUploadId),
    CONSTRAINT FK_BankPaymentRecord_Payment FOREIGN KEY (PaymentId) REFERENCES Payments(PaymentId)
);
```

#### RegistrationDraft
```sql
CREATE TABLE RegistrationDrafts (
    RegistrationDraftId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Email NVARCHAR(100) NOT NULL UNIQUE, -- Unique per email (one draft per user)
    CurrentStep INT NOT NULL, -- 1-4
    CompletedSteps NVARCHAR(10), -- Comma-separated: "1,2"
    DraftData NVARCHAR(MAX) NOT NULL, -- JSON serialized form data
    ExpiryDate DATETIME NOT NULL, -- 30 days from last update
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedDate DATETIME NOT NULL DEFAULT GETDATE()
);

CREATE INDEX IX_RegistrationDraft_Email ON RegistrationDrafts(Email);
CREATE INDEX IX_RegistrationDraft_ExpiryDate ON RegistrationDrafts(ExpiryDate);
```

### 5.2 Updated Entities

#### TestResult (Add Barcode Column)
```sql
ALTER TABLE TestResults
ADD Barcode NVARCHAR(20) NOT NULL UNIQUE;

CREATE UNIQUE INDEX IX_TestResult_Barcode ON TestResults(Barcode);
```

---

## 6. API ENDPOINTS - NEW & UPDATED

### 6.1 Payment Upload API
```csharp
POST /api/payments/upload
[Authorize(Roles = "Admin,SuperUser")]
Content-Type: multipart/form-data
Request: IFormFile file

Response: PaymentUploadResultDto
{
    "Success": bool,
    "UploadId": Guid,
    "TotalRows": int,
    "ProcessedCount": int,
    "MatchedCount": int,
    "UnmatchedCount": int,
    "ErrorCount": int,
    "Errors": ErrorDetail[],
    "UnmatchedPayments": UnmatchedPayment[]
}

GET /api/payments/uploads
[Authorize(Roles = "Admin,SuperUser")]
Returns: List<PaymentUploadDto> (paginated)

GET /api/payments/uploads/{id}
[Authorize(Roles = "Admin,SuperUser")]
Returns: PaymentUploadDto (with details)

GET /api/payments/unmatched
[Authorize(Roles = "Admin,SuperUser")]
Returns: List<BankPaymentRecordDto> (unmatched only)

POST /api/payments/unmatched/{id}/link
[Authorize(Roles = "Admin,SuperUser")]
Request: { "PaymentId": Guid }
Action: Manually link unmatched payment to booking payment
```

### 6.2 Registration Draft API
```csharp
POST /api/registration/draft/save
[AllowAnonymous]
Request: SaveDraftRequest
{
    "Email": string,
    "CurrentStep": int,
    "CompletedSteps": int[],
    "DraftData": object (form data)
}
Response: SaveDraftResponse { "Success": bool, "DraftId": Guid }

GET /api/registration/draft/{email}
[AllowAnonymous]
Returns: RegistrationDraftDto (if exists)
{
    "DraftId": Guid,
    "Email": string,
    "CurrentStep": int,
    "CompletedSteps": int[],
    "DraftData": object,
    "ExpiryDate": DateTime
}

DELETE /api/registration/draft/{email}
[AllowAnonymous]
Action: Discard draft (user wants to start fresh)
```

### 6.3 Result Barcode API
```csharp
GET /api/results/by-barcode/{barcode}
[Authorize]
Returns: TestResultDto (if user owns result or is staff/admin)

POST /api/results/generate-barcode
[Authorize(Roles = "Admin,SuperUser")]
Request: { "TestResultId": Guid }
Returns: { "Barcode": string }
Action: Generate barcode for result (if missing)
```

---

## 7. COMPONENT STRUCTURE UPDATES

### 7.1 New Components

```
src/NBT.WebUI/Components/
├── Registration/
│   ├── RegistrationWizard.razor (UPDATED - fix step navigation)
│   ├── DraftResumptionDialog.razor (NEW - "Resume or Start Fresh")
│   └── RegistrationDraftService.cs (NEW - manage draft state)
│
├── Payments/
│   ├── PaymentUpload.razor (NEW - bank payment upload)
│   ├── PaymentReconciliation.razor (NEW - match unmatched payments)
│   ├── UnmatchedPaymentList.razor (NEW - grid of unmatched)
│   └── LinkPaymentDialog.razor (NEW - manual linking)
│
├── Dashboard/
│   ├── StudentDashboard.razor (NEW)
│   ├── StaffDashboard.razor (NEW)
│   ├── AdminDashboard.razor (NEW)
│   ├── SuperUserDashboard.razor (NEW)
│   └── DashboardLayout.razor (NEW - shared layout with left menu)
│
├── Landing/
│   ├── LandingPage.razor (NEW - public homepage)
│   ├── ApplicantsMenu.razor (NEW - applicant submenu)
│   ├── InstitutionsMenu.razor (NEW - institution submenu)
│   ├── EducatorsMenu.razor (NEW - educator submenu)
│   └── VideoEmbed.razor (NEW - reusable video player)
│
└── Results/
    ├── ResultCertificatePDF.razor (UPDATED - add barcode)
    └── BarcodeGenerator.cs (NEW - generate barcode image)
```

### 7.2 Updated Components

#### RegistrationWizard.razor (CRITICAL FIXES)
```csharp
@page "/register"

<FluentWizard @ref="wizardRef" 
              StepChanged="OnStepChanged"
              aria-label="Student registration wizard">
    
    <FluentWizardStep Label="Personal Information" 
                      CanMoveNext="@IsStep1Valid"
                      OnValidate="ValidateStep1">
        <PersonalInformationStep @bind-Model="model.PersonalInfo" 
                                 OnValidationChanged="HandleStep1Validation" />
    </FluentWizardStep>
    
    <FluentWizardStep Label="Academic & Test Selection" 
                      CanMoveNext="@IsStep2Valid"
                      OnValidate="ValidateStep2">
        <AcademicTestSelectionStep @bind-Model="model.AcademicInfo" 
                                   OnValidationChanged="HandleStep2Validation" />
    </FluentWizardStep>
    
    <FluentWizardStep Label="Pre-Test Questionnaire" 
                      CanMoveNext="@IsStep3Valid"
                      OnValidate="ValidateStep3">
        <SurveyQuestionnaireStep @bind-Model="model.QuestionnaireResponses" 
                                 OnValidationChanged="HandleStep3Validation" />
    </FluentWizardStep>
    
    <FluentWizardStep Label="Review & Confirmation" 
                      CanMoveNext="false">
        <ReviewConfirmationStep Model="@model" 
                                OnSubmit="HandleSubmit"
                                @bind-TermsAccepted="termsAccepted" />
        
        <FluentButton Appearance="Appearance.Accent"
                      Disabled="@(!termsAccepted)"
                      OnClick="HandleSubmit">
            Register
        </FluentButton>
    </FluentWizardStep>
</FluentWizard>

@code {
    private FluentWizard wizardRef;
    private RegistrationModel model = new();
    private bool termsAccepted = false;
    
    // Step validation flags (controls Next button)
    private bool IsStep1Valid => model.PersonalInfo.IsValid();
    private bool IsStep2Valid => model.AcademicInfo.IsValid();
    private bool IsStep3Valid => model.QuestionnaireResponses.IsComplete();
    
    protected override async Task OnInitializedAsync()
    {
        // Check for existing draft
        var draft = await DraftService.GetDraftAsync(email);
        if (draft != null)
        {
            bool resume = await ShowResumptionDialog();
            if (resume)
            {
                model = draft.DraftData;
                wizardRef.SetCurrentStep(draft.CurrentStep);
            }
            else
            {
                await DraftService.DiscardDraftAsync(email);
            }
        }
    }
    
    private async Task OnStepChanged(int newStep)
    {
        // Save draft after each step
        await DraftService.SaveDraftAsync(new SaveDraftRequest
        {
            Email = model.PersonalInfo.Email,
            CurrentStep = newStep,
            CompletedSteps = GetCompletedSteps(),
            DraftData = model
        });
    }
    
    private async Task ValidateStep1()
    {
        // Server-side validation
        var result = await RegistrationService.ValidatePersonalInfoAsync(model.PersonalInfo);
        if (!result.IsValid)
        {
            // Show validation errors
        }
    }
    
    private async Task HandleSubmit()
    {
        try
        {
            // Submit registration (NBT number generated server-side)
            var result = await RegistrationService.SubmitRegistrationAsync(model);
            
            if (result.Success)
            {
                // Clear draft
                await DraftService.DiscardDraftAsync(model.PersonalInfo.Email);
                
                // Navigate to login with success message
                NavigationManager.NavigateTo($"/login?message=Registration successful! Your NBT Number is {result.NBTNumber}");
            }
            else
            {
                // Show error
                ErrorMessage = result.ErrorMessage;
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = "An error occurred during registration. Please try again.";
        }
    }
    
    private void HandleStep1Validation(bool isValid)
    {
        // Triggered when step 1 validation state changes
        // Re-render to update Next button state
        StateHasChanged();
    }
    
    // ... similar handlers for step 2 and 3
}
```

---

## 8. BUSINESS RULES SUMMARY

### 8.1 Registration Rules
1. ✅ Support SA ID, Foreign ID, and Passport ID
2. ✅ SA ID auto-extracts DOB and Gender
3. ✅ NBT number generated on final submission (server-side)
4. ✅ Registration resumable if interrupted (draft saved)
5. ✅ Draft expires after 30 days of inactivity
6. ✅ All 4 steps must be completed in sequence
7. ✅ Next button disabled until current step valid
8. ✅ Auto-fill does NOT trigger premature step completion

### 8.2 Booking Rules
1. ✅ Bookings open April 1 (intake start date)
2. ✅ One active booking at a time per student
3. ✅ Max 2 tests per year per student
4. ✅ Test valid for 3 years from booking date
5. ✅ Can change booking before close date
6. ✅ Cannot book if session at capacity
7. ✅ Cannot book if venue unavailable for test date
8. ✅ Online tests require specific calendar entries

### 8.3 Payment Rules
1. ✅ Payments can be made in installments
2. ✅ Test costs vary by intake year
3. ✅ Bank payments can be uploaded (CSV/Excel)
4. ✅ Payment reconciliation for unmatched payments
5. ✅ Students can only view fully paid results
6. ✅ Staff/Admin can view all results regardless of payment

### 8.4 Result Rules
1. ✅ Each result has unique barcode
2. ✅ AQL test: 2 domains (AL, QL)
3. ✅ MAT test: 3 domains (AL, QL, MAT)
4. ✅ Performance levels per domain
5. ✅ Payment status restricts student access
6. ✅ Results valid for 3 years
7. ✅ PDF certificate with barcode

### 8.5 Venue Rules
1. ✅ Venue types: National, Special Session, Research, Other
2. ✅ TestSession linked to Venue (NOT Room)
3. ✅ Venues may be unavailable for certain dates
4. ✅ Test calendar shows Sunday and Online tests
5. ✅ Online tests can be taken from anywhere

### 8.6 UI/UX Rules
1. ✅ Landing page: 3 main menus (Applicants, Institutions, Educators)
2. ✅ Submenus match current NBT website
3. ✅ Videos embedded on relevant pages
4. ✅ After login: Role-specific dashboard with left menu
5. ✅ Dashboard left menu: Role-based navigation

---

## 9. TESTING REQUIREMENTS

### 9.1 Registration Wizard Tests
```csharp
[Test] Wizard_Step1_NextButton_DisabledUntilValid()
[Test] Wizard_Step2_NextButton_DisabledUntilValid()
[Test] Wizard_Step3_NextButton_DisabledUntilValid()
[Test] Wizard_Step4_RegisterButton_DisabledUntilTermsAccepted()
[Test] Wizard_SAIDAutoFill_DoesNotTriggerPrematureCompletion()
[Test] Wizard_NBTNumber_GeneratedOnFinalSubmission()
[Test] Wizard_Draft_SavedAfterEachStep()
[Test] Wizard_Draft_RestoredOnReturn()
[Test] Wizard_Draft_ExpiresAfter30Days()
```

### 9.2 Payment Upload Tests
```csharp
[Test] PaymentUpload_ValidFile_ProcessesSuccessfully()
[Test] PaymentUpload_InvalidFormat_ReturnsErrors()
[Test] PaymentUpload_MatchesPayment_UpdatesStatus()
[Test] PaymentUpload_NoMatch_CreatesUnmatchedRecord()
[Test] PaymentUpload_DuplicateBankRef_RejectsRow()
[Test] PaymentReconciliation_ManualLink_UpdatesPayment()
```

### 9.3 Result Barcode Tests
```csharp
[Test] ResultBarcode_GeneratedOnImport_IsUnique()
[Test] ResultBarcode_Format_MatchesRegex()
[Test] ResultBarcode_Lookup_ReturnsCorrectResult()
[Test] ResultPDF_IncludesBarcode_AsImage()
```

---

## 10. DEPLOYMENT CHECKLIST

### Phase 1: Registration Wizard Fixes
- [ ] Fix step navigation logic (Next button activation)
- [ ] Implement draft save/restore functionality
- [ ] Add RegistrationDraft entity and API
- [ ] Test all 4 wizard steps in sequence
- [ ] Test SA ID auto-fill does not skip steps
- [ ] Test draft expiry cleanup job

### Phase 2: Payment Upload & Reconciliation
- [ ] Create PaymentUpload and BankPaymentRecord entities
- [ ] Implement file upload API endpoint
- [ ] Implement payment matching algorithm
- [ ] Create PaymentReconciliation dashboard component
- [ ] Test CSV and Excel file formats
- [ ] Test unmatched payment manual linking

### Phase 3: UI/UX Enhancements
- [ ] Create landing page with 3 main menus
- [ ] Populate submenus (match current NBT website)
- [ ] Embed videos on relevant pages
- [ ] Create role-specific dashboard layouts
- [ ] Implement left-side navigation menus
- [ ] Test responsive design (mobile, tablet, desktop)

### Phase 4: Result Barcode System
- [ ] Add Barcode column to TestResult entity
- [ ] Implement barcode generation service
- [ ] Update result import to generate barcodes
- [ ] Update result PDF to include barcode image
- [ ] Create barcode lookup API
- [ ] Test barcode uniqueness

### Phase 5: Integration Testing
- [ ] Test complete registration → booking → payment → result flow
- [ ] Test bank payment upload → reconciliation → result access
- [ ] Test all role-based dashboards
- [ ] Test draft resumption after interruption
- [ ] Test payment installments and intake year calculations
- [ ] Test result access restrictions (payment status)

### Phase 6: CI/CD & Deployment
- [ ] Push to GitHub (feature branches)
- [ ] Run all unit tests
- [ ] Run all integration tests
- [ ] Run all UI tests
- [ ] Merge to main after approval
- [ ] Deploy to staging environment
- [ ] Perform UAT (User Acceptance Testing)
- [ ] Deploy to production

---

## 11. IMPLEMENTATION PRIORITY

### Priority 1 (CRITICAL - Must Fix Immediately)
1. ✅ Registration wizard step navigation bug
2. ✅ NBT number generation on final submission (not separate step)
3. ✅ Result barcode system (required for result validity)

### Priority 2 (HIGH - Required for MVP)
1. ✅ Registration draft save/restore
2. ✅ Payment upload and reconciliation
3. ✅ Role-based dashboards with left menus
4. ✅ Landing page structure with correct menus

### Priority 3 (MEDIUM - Enhanced Features)
1. ✅ Video embedding on pages
2. ✅ Payment installment tracking
3. ✅ Test calendar with Sunday/Online highlighting
4. ✅ Venue availability management

### Priority 4 (LOW - Nice to Have)
1. ✅ Advanced analytics dashboard
2. ✅ Automated email reminders (7 days, 1 day before test)
3. ✅ SMS notifications
4. ✅ Digital signature on PDF certificates

---

## CONCLUSION

This comprehensive update to the NBT Integrated System Constitution addresses all critical requirements, business rules, and architectural mandates identified during the requirement gathering session. All updates are now binding and must be implemented before production deployment.

**Next Steps:**
1. Review and approve constitution updates
2. Update SpecKit tasks with new requirements
3. Implement Priority 1 fixes immediately
4. Plan sprints for Priority 2-4 features
5. Update CI/CD pipeline for new workflows

**Status:** ✅ READY FOR IMPLEMENTATION

---

**Document Owner:** NBT Technical Team  
**Last Updated:** 2025-11-09  
**Next Review:** 2025-12-09
