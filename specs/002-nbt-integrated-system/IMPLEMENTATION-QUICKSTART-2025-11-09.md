# NBT Implementation Quickstart - 2025-11-09

## 🎯 Quick Reference for Current Implementation

This guide provides step-by-step instructions for implementing the latest NBT system requirements with test results, barcodes, and updated registration wizard.

---

## 📋 Prerequisites Checklist

```powershell
# Verify .NET 9 SDK
dotnet --version  # Should show 9.0.x

# Verify SQL Server
Test-NetConnection localhost -Port 1433

# Verify Git
git --version

# Navigate to project
cd "D:\projects\source code\NBTWebApp"
```

---

## 🚀 Phase 1: Update Domain Entities

### Step 1.1: Create TestResultDomain Entity

**File**: `src\NBT.Domain\Entities\TestResultDomain.cs`

```csharp
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NBT.Domain.Entities;

public class TestResultDomain
{
    [Key]
    public Guid TestResultDomainId { get; set; } = Guid.NewGuid();

    [Required]
    public Guid TestResultId { get; set; }

    [Required]
    public DomainType DomainType { get; set; }

    [Required]
    [Range(0, 100)]
    public decimal Score { get; set; }

    [Required]
    public PerformanceLevel PerformanceLevel { get; set; }

    [Range(0, 100)]
    public decimal Percentile { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [ForeignKey(nameof(TestResultId))]
    public virtual TestResult TestResult { get; set; } = null!;
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

### Step 1.2: Update TestResult Entity

**File**: `src\NBT.Domain\Entities\TestResult.cs`

Add these properties:
```csharp
[Required]
[StringLength(50)]
public string Barcode { get; set; } = string.Empty; // Unique per test write

[Required]
public TestType TestType { get; set; }

// Navigation property
public virtual ICollection<TestResultDomain> Domains { get; set; } = new List<TestResultDomain>();
```

Add enum:
```csharp
public enum TestType
{
    AQL,  // AL + QL only (2 domains)
    MAT   // AL + QL + MAT (3 domains)
}
```

### Step 1.3: Create PreTestQuestionnaire Entity

**File**: `src\NBT.Domain\Entities\PreTestQuestionnaire.cs`

```csharp
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NBT.Domain.Entities;

public class PreTestQuestionnaire
{
    [Key]
    public Guid QuestionnaireId { get; set; } = Guid.NewGuid();

    [Required]
    public Guid StudentId { get; set; }

    [Required]
    public Guid RegistrationId { get; set; }

    [Required]
    public string Responses { get; set; } = "{}"; // JSON storage

    public DateTime CompletedDate { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [ForeignKey(nameof(StudentId))]
    public virtual Student Student { get; set; } = null!;

    [ForeignKey(nameof(RegistrationId))]
    public virtual Registration Registration { get; set; } = null!;
}
```

### Step 1.4: Update Student Entity

**File**: `src\NBT.Domain\Entities\Student.cs`

Add these properties:
```csharp
public DateTime? DateOfBirth { get; set; }

public Gender? Gender { get; set; }

[StringLength(50)]
public string? Ethnicity { get; set; }

// Calculated property (not mapped to database)
[NotMapped]
public int Age => DateOfBirth.HasValue 
    ? DateTime.Now.Year - DateOfBirth.Value.Year 
    : 0;

// Navigation properties
public virtual ICollection<TestResult> TestResults { get; set; } = new List<TestResult>();
public virtual ICollection<PreTestQuestionnaire> Questionnaires { get; set; } = new List<PreTestQuestionnaire>();
```

Add enum:
```csharp
public enum Gender
{
    Male,
    Female,
    Other
}
```

---

## 🗄️ Phase 2: Update Database Context

### Step 2.1: Update DbContext

**File**: `src\NBT.Infrastructure\Data\ApplicationDbContext.cs`

```csharp
public DbSet<TestResultDomain> TestResultDomains { get; set; }
public DbSet<PreTestQuestionnaire> PreTestQuestionnaires { get; set; }
```

### Step 2.2: Configure Relationships

Add to `OnModelCreating`:

```csharp
// TestResult - TestResultDomain (1:N)
modelBuilder.Entity<TestResult>()
    .HasMany(tr => tr.Domains)
    .WithOne(trd => trd.TestResult)
    .HasForeignKey(trd => trd.TestResultId)
    .OnDelete(DeleteBehavior.Cascade);

// TestResult - Barcode unique index
modelBuilder.Entity<TestResult>()
    .HasIndex(tr => tr.Barcode)
    .IsUnique();

// Student - PreTestQuestionnaire (1:N)
modelBuilder.Entity<Student>()
    .HasMany(s => s.Questionnaires)
    .WithOne(q => q.Student)
    .HasForeignKey(q => q.StudentId)
    .OnDelete(DeleteBehavior.Cascade);

// Registration - PreTestQuestionnaire (1:1)
modelBuilder.Entity<Registration>()
    .HasOne<PreTestQuestionnaire>()
    .WithOne(q => q.Registration)
    .HasForeignKey<PreTestQuestionnaire>(q => q.RegistrationId)
    .OnDelete(DeleteBehavior.Restrict);
```

### Step 2.3: Create and Apply Migration

```powershell
# Navigate to Infrastructure project
cd "src\NBT.Infrastructure"

# Create migration
dotnet ef migrations add AddTestResultDomainsAndQuestionnaire --startup-project ..\NBT.WebAPI

# Apply migration
dotnet ef database update --startup-project ..\NBT.WebAPI

# Return to root
cd ..\..
```

---

## 📝 Phase 3: Update DTOs

### Step 3.1: Create TestResultDomainDto

**File**: `src\NBT.Application\Results\DTOs\TestResultDomainDto.cs`

```csharp
using System;
using NBT.Domain.Entities;

namespace NBT.Application.Results.DTOs;

public class TestResultDomainDto
{
    public Guid TestResultDomainId { get; set; }
    public Guid TestResultId { get; set; }
    public DomainType DomainType { get; set; }
    public decimal Score { get; set; }
    public PerformanceLevel PerformanceLevel { get; set; }
    public decimal Percentile { get; set; }
    public DateTime CreatedDate { get; set; }
}
```

### Step 3.2: Update TestResultDto

**File**: `src\NBT.Application\Results\DTOs\TestResultDto.cs`

Add:
```csharp
public string Barcode { get; set; } = string.Empty;
public TestType TestType { get; set; }
public List<TestResultDomainDto> Domains { get; set; } = new();
```

### Step 3.3: Create PreTestQuestionnaireDto

**File**: `src\NBT.Application\Students\DTOs\PreTestQuestionnaireDto.cs`

```csharp
using System;
using System.Text.Json;

namespace NBT.Application.Students.DTOs;

public class PreTestQuestionnaireDto
{
    public Guid QuestionnaireId { get; set; }
    public Guid StudentId { get; set; }
    public Guid RegistrationId { get; set; }
    public Dictionary<string, object> Responses { get; set; } = new();
    public DateTime CompletedDate { get; set; }
}
```

### Step 3.4: Update StudentDto

**File**: `src\NBT.Application\Students\DTOs\StudentDto.cs`

Add:
```csharp
public DateTime? DateOfBirth { get; set; }
public Gender? Gender { get; set; }
public string? Ethnicity { get; set; }
public int Age { get; set; }
```

---

## ⚙️ Phase 4: Update Services

### Step 4.1: Create ITestResultDomainService

**File**: `src\NBT.Application\Results\Interfaces\ITestResultDomainService.cs`

```csharp
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NBT.Application.Results.DTOs;

namespace NBT.Application.Results.Interfaces;

public interface ITestResultDomainService
{
    Task<List<TestResultDomainDto>> GetDomainsByTestResultIdAsync(Guid testResultId);
    Task<TestResultDomainDto> CreateDomainAsync(TestResultDomainDto domainDto);
    Task UpdateDomainAsync(Guid domainId, TestResultDomainDto domainDto);
    Task DeleteDomainAsync(Guid domainId);
}
```

### Step 4.2: Create IPreTestQuestionnaireService

**File**: `src\NBT.Application\Students\Interfaces\IPreTestQuestionnaireService.cs`

```csharp
using System;
using System.Threading.Tasks;
using NBT.Application.Students.DTOs;

namespace NBT.Application.Students.Interfaces;

public interface IPreTestQuestionnaireService
{
    Task<PreTestQuestionnaireDto> GetByRegistrationIdAsync(Guid registrationId);
    Task<PreTestQuestionnaireDto> CreateQuestionnaireAsync(PreTestQuestionnaireDto dto);
    Task UpdateQuestionnaireAsync(Guid questionnaireId, PreTestQuestionnaireDto dto);
}
```

### Step 4.3: Implement Services

Create implementation classes in `src\NBT.Application\Results\Services\` and `src\NBT.Application\Students\Services\`

---

## 🎨 Phase 5: Update Blazor Registration Wizard

### Step 5.1: Update RegistrationWizard.razor Structure

**File**: `src\NBT.WebUI.Client\Pages\Registration\RegistrationWizard.razor`

```razor
<FluentWizard @bind-Value="@CurrentStep" 
              DisplayStepNumber="true"
              Height="auto">

    <!-- Step 1: Account & Personal Information (COMBINED) -->
    <FluentWizardStep Label="Personal Information" OnNext="ValidateStep1">
        <h3>Account & Personal Information</h3>
        
        <!-- ID Type Selection -->
        <FluentSelect @bind-Value="Model.IDType" Label="ID Type">
            <FluentOption Value="SA_ID">South African ID</FluentOption>
            <FluentOption Value="FOREIGN_ID">Foreign ID</FluentOption>
            <FluentOption Value="PASSPORT">Passport</FluentOption>
        </FluentSelect>
        
        <!-- ID Number (with validation) -->
        <FluentTextField @bind-Value="Model.IDNumber" 
                         Label="ID Number" 
                         Required
                         @onblur="ValidateIDNumber" />
        
        <!-- Auto-fill for SA_ID -->
        @if (Model.IDType == "SA_ID" && IsValidSAID)
        {
            <p>Date of Birth: @ExtractedDOB</p>
            <p>Gender: @ExtractedGender</p>
        }
        
        <!-- Personal Details -->
        <FluentTextField @bind-Value="Model.FirstName" Label="First Name" Required />
        <FluentTextField @bind-Value="Model.LastName" Label="Last Name" Required />
        <FluentTextField @bind-Value="Model.Email" Label="Email" Type="email" Required />
        <FluentTextField @bind-Value="Model.Phone" Label="Phone" Required />
        
        <!-- DOB for non-SA_ID -->
        @if (Model.IDType != "SA_ID")
        {
            <FluentDatePicker @bind-Value="Model.DateOfBirth" Label="Date of Birth" Required />
        }
        
        <!-- Gender (manual if not SA_ID) -->
        @if (Model.IDType != "SA_ID")
        {
            <FluentSelect @bind-Value="Model.Gender" Label="Gender" Required>
                <FluentOption Value="Male">Male</FluentOption>
                <FluentOption Value="Female">Female</FluentOption>
                <FluentOption Value="Other">Other</FluentOption>
            </FluentSelect>
        }
        
        <!-- Ethnicity -->
        <FluentTextField @bind-Value="Model.Ethnicity" Label="Ethnicity" />
        
        <!-- Nationality for Foreign ID/Passport -->
        @if (Model.IDType != "SA_ID")
        {
            <FluentTextField @bind-Value="Model.Nationality" Label="Nationality" Required />
            <FluentTextField @bind-Value="Model.CountryOfOrigin" Label="Country of Origin" Required />
        }
    </FluentWizardStep>

    <!-- Step 2: Academic & Test Selection (COMBINED) -->
    <FluentWizardStep Label="Academic & Test" OnNext="ValidateStep2">
        <h3>Academic Background & Test Selection</h3>
        
        <!-- Academic Details -->
        <FluentTextField @bind-Value="Model.School" Label="School Name" Required />
        <FluentNumberField @bind-Value="Model.Grade" Label="Grade" Required />
        <FluentNumberField @bind-Value="Model.Year" Label="Year" Required />
        
        <!-- Test Type Selection -->
        <FluentRadioGroup @bind-Value="Model.TestType" Label="Test Type">
            <FluentRadio Value="AQL">AQL (Academic & Quantitative Literacy)</FluentRadio>
            <FluentRadio Value="MAT">MAT (includes AQL + Mathematics)</FluentRadio>
        </FluentRadioGroup>
        
        <!-- Venue Selection -->
        <FluentSelect @bind-Value="Model.VenueId" Label="Test Venue" Required>
            @foreach (var venue in Venues)
            {
                <FluentOption Value="@venue.VenueId.ToString()">@venue.Name - @venue.City</FluentOption>
            }
        </FluentSelect>
        
        <!-- Session Selection -->
        <FluentSelect @bind-Value="Model.SessionId" Label="Test Session" Required>
            @foreach (var session in Sessions)
            {
                <FluentOption Value="@session.TestSessionId.ToString()">
                    @session.SessionDate.ToString("dd MMM yyyy") - @session.SessionTime
                </FluentOption>
            }
        </FluentSelect>
        
        <!-- Special Accommodations -->
        <FluentCheckbox @bind-Value="Model.RequiresSpecialAccommodation" 
                       Label="I require special accommodations" />
        
        @if (Model.RequiresSpecialAccommodation)
        {
            <FluentTextArea @bind-Value="Model.SpecialAccommodationDetails" 
                           Label="Please describe" 
                           Rows="3" />
        }
    </FluentWizardStep>

    <!-- Step 3: Pre-Test Questionnaire -->
    <FluentWizardStep Label="Questionnaire" OnNext="ValidateStep3">
        <h3>Pre-Test Background Questionnaire</h3>
        <p>This information helps us with research and equity reporting.</p>
        
        @foreach (var question in QuestionnaireQuestions)
        {
            <div class="question-block">
                <label>@question.Text</label>
                @if (question.Type == "MultipleChoice")
                {
                    <FluentSelect @bind-Value="Model.Questionnaire[question.Id]">
                        @foreach (var option in question.Options)
                        {
                            <FluentOption Value="@option">@option</FluentOption>
                        }
                    </FluentSelect>
                }
                else
                {
                    <FluentTextField @bind-Value="Model.Questionnaire[question.Id]" />
                }
            </div>
        }
    </FluentWizardStep>

    <!-- Step 4: Review & Confirmation -->
    <FluentWizardStep Label="Review" OnSubmit="SubmitRegistration">
        <h3>Review Your Information</h3>
        
        <!-- Display Summary -->
        <FluentCard>
            <h4>Personal Information</h4>
            <p><strong>Name:</strong> @Model.FirstName @Model.LastName</p>
            <p><strong>ID:</strong> @Model.IDNumber (@Model.IDType)</p>
            <p><strong>Email:</strong> @Model.Email</p>
            <p><strong>Phone:</strong> @Model.Phone</p>
            @if (Model.DateOfBirth.HasValue)
            {
                <p><strong>Date of Birth:</strong> @Model.DateOfBirth.Value.ToString("dd MMM yyyy")</p>
                <p><strong>Age:</strong> @CalculateAge(Model.DateOfBirth.Value)</p>
            }
            <p><strong>Gender:</strong> @Model.Gender</p>
            <p><strong>Ethnicity:</strong> @Model.Ethnicity</p>
        </FluentCard>
        
        <FluentCard>
            <h4>Test Information</h4>
            <p><strong>Test Type:</strong> @Model.TestType</p>
            <p><strong>Venue:</strong> @GetVenueName(Model.VenueId)</p>
            <p><strong>Session:</strong> @GetSessionDate(Model.SessionId)</p>
        </FluentCard>
        
        <!-- Terms & Conditions -->
        <FluentCheckbox @bind-Value="Model.AcceptedTerms" Required>
            I accept the <a href="/terms">Terms and Conditions</a>
        </FluentCheckbox>
        
        <!-- Submit Button -->
        <FluentButton Appearance="Appearance.Accent" 
                     OnClick="SubmitRegistration"
                     Disabled="!Model.AcceptedTerms">
            Submit Registration
        </FluentButton>
    </FluentWizardStep>

</FluentWizard>
```

### Step 5.2: Update Code-Behind

```csharp
@code {
    private RegistrationModel Model = new();
    private int CurrentStep = 0;
    
    // Auto-extracted from SA ID
    private DateTime? ExtractedDOB;
    private string ExtractedGender;
    private bool IsValidSAID;
    
    private async Task ValidateIDNumber()
    {
        if (Model.IDType == "SA_ID")
        {
            // Call Luhn validator
            IsValidSAID = await LuhnValidator.ValidateSAID(Model.IDNumber);
            
            if (IsValidSAID)
            {
                ExtractedDOB = LuhnValidator.ExtractDateOfBirth(Model.IDNumber);
                ExtractedGender = LuhnValidator.ExtractGender(Model.IDNumber);
                Model.DateOfBirth = ExtractedDOB;
                Model.Gender = ExtractedGender;
            }
        }
        
        // Check for duplicates
        var isDuplicate = await StudentService.CheckDuplicateIDAsync(Model.IDNumber);
        if (isDuplicate)
        {
            // Show error message
            await DialogService.ShowErrorAsync("This ID number is already registered.");
        }
    }
    
    private async Task<bool> ValidateStep1()
    {
        // Validate personal information
        return !string.IsNullOrEmpty(Model.FirstName) 
            && !string.IsNullOrEmpty(Model.LastName)
            && !string.IsNullOrEmpty(Model.Email)
            && !string.IsNullOrEmpty(Model.IDNumber);
    }
    
    private async Task<bool> ValidateStep2()
    {
        // Validate academic and test selection
        return !string.IsNullOrEmpty(Model.School)
            && Model.VenueId != Guid.Empty
            && Model.SessionId != Guid.Empty;
    }
    
    private async Task<bool> ValidateStep3()
    {
        // Save questionnaire responses
        await QuestionnaireService.SaveResponsesAsync(Model.Questionnaire);
        return true;
    }
    
    private async Task SubmitRegistration()
    {
        try
        {
            // Submit registration
            var result = await RegistrationService.RegisterStudentAsync(Model);
            
            if (result.Success)
            {
                // Show success message with NBT number
                await DialogService.ShowSuccessAsync(
                    $"Registration successful! Your NBT Number is: {result.NBTNumber}");
                
                // Navigate to login
                Navigation.NavigateTo("/login");
            }
            else
            {
                await DialogService.ShowErrorAsync(result.ErrorMessage);
            }
        }
        catch (Exception ex)
        {
            await DialogService.ShowErrorAsync($"Registration failed: {ex.Message}");
        }
    }
    
    private int CalculateAge(DateTime dob)
    {
        return DateTime.Now.Year - dob.Year;
    }
}
```

---

## 🧪 Phase 6: Testing

### Step 6.1: Run Build

```powershell
# Build solution
dotnet build

# Restore packages if needed
dotnet restore
```

### Step 6.2: Apply Migrations

```powershell
dotnet ef database update --project src\NBT.Infrastructure --startup-project src\NBT.WebAPI
```

### Step 6.3: Run Application

```powershell
# Start API
cd src\NBT.WebAPI
Start-Process powershell -ArgumentList "dotnet run"

# Start Blazor UI
cd ..\NBT.WebUI
Start-Process powershell -ArgumentList "dotnet run"
```

### Step 6.4: Test Registration Wizard

1. Navigate to `/register`
2. Test SA_ID auto-fill (use: 9001015009087)
3. Test Foreign ID/Passport flow
4. Complete all 4 steps
5. Verify NBT number generated
6. Check redirect to login

---

## 📤 Phase 7: Push to GitHub

```powershell
# Create feature branch
git checkout -b feature/test-results-and-wizard-updates

# Add changes
git add .

# Commit with conventional message
git commit -m "feat: Add test result domains, barcodes, and updated registration wizard

- Add TestResultDomain entity for AL/QL/MAT domains
- Add PreTestQuestionnaire entity
- Update Student entity with DOB, Gender, Ethnicity
- Add barcode to TestResult for unique test identification
- Restructure registration wizard to 4 combined steps
- Implement SA_ID auto-extraction of DOB and Gender
- Add support for Foreign ID and Passport registration
- Update database migrations and relationships"

# Push to GitHub
git push -u origin feature/test-results-and-wizard-updates

# Create Pull Request on GitHub
# After review and approval, merge to main
```

---

## ✅ Verification Checklist

- [ ] All entities created and configured
- [ ] Database migration applied successfully
- [ ] DTOs created for all new entities
- [ ] Services implemented with interfaces
- [ ] Registration wizard updated to 4 steps
- [ ] SA_ID auto-extraction working
- [ ] Foreign ID/Passport support working
- [ ] Test result domains saved correctly
- [ ] Barcode generated uniquely per test
- [ ] Pre-test questionnaire completed
- [ ] NBT number generated on submission
- [ ] Navigation to login after success
- [ ] All builds passing
- [ ] Changes pushed to GitHub
- [ ] Pull request created and reviewed

---

## 📞 Support

**Issues?** Check:
1. Connection string in appsettings.json
2. Database migrations applied
3. All NuGet packages restored
4. Fluent UI components installed
5. API running on correct port

**Still stuck?** Review:
- Constitution: `specs\002-nbt-integrated-system\constitution.md`
- Updates: `specs\002-nbt-integrated-system\CONSTITUTION-UPDATES-2025-11-09.md`

---

**Last Updated**: 2025-11-09  
**Status**: READY FOR IMPLEMENTATION  
**Next Phase**: Testing & Deployment
