# NBT System - Implementation Quickstart (2025-11-09)

**Status:** 🚀 READY TO START  
**Date:** 2025-11-09  
**Version:** 3.1  
**Purpose:** Immediate implementation guide for updated requirements

---

## 🎯 EXECUTIVE SUMMARY

This quickstart provides step-by-step instructions to implement the critical updates to the NBT Integrated System:

### Critical Updates:
1. ✅ Fix Registration Wizard (step navigation, NBT number generation)
2. ✅ Add Registration Draft Resumption
3. ✅ Implement Result Barcode System
4. ✅ Add Bank Payment Upload & Reconciliation
5. ✅ Create Role-Based Dashboards with Landing Page

### Timeline: 4 weeks (160 hours)
### Team: 2 developers, 1 tester

---

## 🚨 PRIORITY 1: CRITICAL FIXES (Week 1)

### Day 1-2: Registration Wizard Fixes (16 hours)

#### Step 1: Fix Step Navigation Logic (8 hours)

**File**: `src/NBT.WebUI/Components/Registration/RegistrationWizard.razor`

```csharp
@page "/register"
@inject IRegistrationService RegistrationService
@inject IDraftService DraftService
@inject NavigationManager NavigationManager

<PageTitle>Register for NBT</PageTitle>

<FluentStack Orientation="Orientation.Vertical" HorizontalAlignment="HorizontalAlignment.Center">
    <h2>Student Registration</h2>
    
    @if (showResumptionDialog && existingDraft != null)
    {
        <DraftResumptionDialog 
            Email="@existingDraft.Email"
            LastUpdated="@existingDraft.UpdatedDate"
            OnChoice="HandleDraftChoice" />
    }
    
    <FluentWizard @ref="wizardRef" 
                  DisplayStepNumber="WizardStepStatus.Current"
                  aria-label="Student registration wizard"
                  Style="width: 800px;">
        
        <!-- STEP 1: Personal Information & Account -->
        <FluentWizardStep Label="Personal Information" 
                          CanMoveNext="@IsStep1Valid"
                          OnNext="OnStep1Next">
            <PersonalInformationStep @bind-Model="model.PersonalInfo" 
                                     OnValidationChanged="HandleStep1Validation" />
        </FluentWizardStep>
        
        <!-- STEP 2: Academic & Test Selection -->
        <FluentWizardStep Label="Academic & Test Selection" 
                          CanMoveNext="@IsStep2Valid"
                          OnNext="OnStep2Next">
            <AcademicTestSelectionStep @bind-Model="model.AcademicInfo" 
                                       OnValidationChanged="HandleStep2Validation" />
        </FluentWizardStep>
        
        <!-- STEP 3: Pre-Test Questionnaire -->
        <FluentWizardStep Label="Pre-Test Questionnaire" 
                          CanMoveNext="@IsStep3Valid"
                          OnNext="OnStep3Next">
            <SurveyQuestionnaireStep @bind-Model="model.QuestionnaireResponses" 
                                     OnValidationChanged="HandleStep3Validation" />
        </FluentWizardStep>
        
        <!-- STEP 4: Review & Confirmation -->
        <FluentWizardStep Label="Review & Confirmation">
            <ReviewConfirmationStep Model="@model" />
            
            <FluentStack Orientation="Orientation.Vertical" VerticalGap="10">
                <FluentCheckbox @bind-Value="termsAccepted" 
                                Label="I accept the terms and conditions" />
                
                <FluentButton Appearance="Appearance.Accent"
                              Disabled="@(!termsAccepted || isSubmitting)"
                              OnClick="HandleSubmit"
                              Loading="@isSubmitting">
                    @if (isSubmitting)
                    {
                        <FluentIcon Value="@(new Icons.Regular.Size16.Spinner())" />
                        <span>Registering...</span>
                    }
                    else
                    {
                        <span>Register</span>
                    }
                </FluentButton>
                
                @if (!string.IsNullOrEmpty(errorMessage))
                {
                    <FluentMessageBar Intent="MessageIntent.Error" Title="Registration Failed">
                        @errorMessage
                    </FluentMessageBar>
                }
            </FluentStack>
        </FluentWizardStep>
        
    </FluentWizard>
</FluentStack>

@code {
    private FluentWizard wizardRef;
    private RegistrationModel model = new();
    private RegistrationDraft? existingDraft;
    private bool showResumptionDialog = false;
    private bool termsAccepted = false;
    private bool isSubmitting = false;
    private string errorMessage = string.Empty;
    
    // Validation flags (computed properties - re-evaluated on state change)
    private bool IsStep1Valid => 
        model.PersonalInfo != null &&
        !string.IsNullOrWhiteSpace(model.PersonalInfo.Email) &&
        !string.IsNullOrWhiteSpace(model.PersonalInfo.FirstName) &&
        !string.IsNullOrWhiteSpace(model.PersonalInfo.LastName) &&
        !string.IsNullOrWhiteSpace(model.PersonalInfo.IDNumber) &&
        !string.IsNullOrWhiteSpace(model.PersonalInfo.Phone) &&
        model.PersonalInfo.DateOfBirth != default &&
        !string.IsNullOrWhiteSpace(model.PersonalInfo.Gender) &&
        !string.IsNullOrWhiteSpace(model.PersonalInfo.Ethnicity) &&
        model.PersonalInfo.IsValid();
    
    private bool IsStep2Valid => 
        model.AcademicInfo != null &&
        !string.IsNullOrWhiteSpace(model.AcademicInfo.School) &&
        !string.IsNullOrWhiteSpace(model.AcademicInfo.Grade) &&
        model.AcademicInfo.TestType != null &&
        model.AcademicInfo.VenueId != Guid.Empty &&
        model.AcademicInfo.SessionId != Guid.Empty &&
        model.AcademicInfo.IsValid();
    
    private bool IsStep3Valid => 
        model.QuestionnaireResponses != null &&
        model.QuestionnaireResponses.IsComplete();
    
    protected override async Task OnInitializedAsync()
    {
        // Check for existing draft
        var emailParam = GetQueryParameter("email");
        if (!string.IsNullOrEmpty(emailParam))
        {
            existingDraft = await DraftService.GetDraftAsync(emailParam);
            if (existingDraft != null && existingDraft.ExpiryDate > DateTime.UtcNow)
            {
                showResumptionDialog = true;
            }
        }
    }
    
    private async Task HandleDraftChoice(bool resume)
    {
        showResumptionDialog = false;
        
        if (resume && existingDraft != null)
        {
            // Restore draft data
            model = JsonSerializer.Deserialize<RegistrationModel>(existingDraft.DraftData);
            
            // Set wizard to saved step
            await wizardRef.GoToStepAsync(existingDraft.CurrentStep);
        }
        else if (!resume && existingDraft != null)
        {
            // Discard draft
            await DraftService.DiscardDraftAsync(existingDraft.Email);
        }
        
        StateHasChanged();
    }
    
    private async Task OnStep1Next()
    {
        // Validate step 1 with server
        var validationResult = await RegistrationService.ValidatePersonalInfoAsync(model.PersonalInfo);
        
        if (!validationResult.IsValid)
        {
            errorMessage = string.Join(", ", validationResult.Errors);
            return;
        }
        
        // Save draft
        await SaveDraft(0);
        
        // Clear error
        errorMessage = string.Empty;
    }
    
    private async Task OnStep2Next()
    {
        // Validate step 2 with server
        var validationResult = await RegistrationService.ValidateAcademicInfoAsync(model.AcademicInfo);
        
        if (!validationResult.IsValid)
        {
            errorMessage = string.Join(", ", validationResult.Errors);
            return;
        }
        
        // Check venue capacity
        var capacityCheck = await RegistrationService.CheckVenueCapacityAsync(model.AcademicInfo.VenueId, model.AcademicInfo.SessionId);
        if (!capacityCheck.HasCapacity)
        {
            errorMessage = "Selected session is full. Please choose another session.";
            return;
        }
        
        // Save draft
        await SaveDraft(1);
        
        // Clear error
        errorMessage = string.Empty;
    }
    
    private async Task OnStep3Next()
    {
        // Validate questionnaire completeness
        if (!model.QuestionnaireResponses.IsComplete())
        {
            errorMessage = "Please answer all required questions.";
            return;
        }
        
        // Save draft
        await SaveDraft(2);
        
        // Clear error
        errorMessage = string.Empty;
    }
    
    private async Task HandleSubmit()
    {
        if (!termsAccepted)
        {
            errorMessage = "You must accept the terms and conditions to register.";
            return;
        }
        
        isSubmitting = true;
        errorMessage = string.Empty;
        
        try
        {
            // Submit registration (NBT number generated server-side)
            var result = await RegistrationService.SubmitRegistrationAsync(model);
            
            if (result.Success)
            {
                // Clear draft
                if (!string.IsNullOrEmpty(model.PersonalInfo.Email))
                {
                    await DraftService.DiscardDraftAsync(model.PersonalInfo.Email);
                }
                
                // Navigate to login with success message
                NavigationManager.NavigateTo($"/login?message=Registration successful! Your NBT Number is {result.NBTNumber}. Please check your email for login credentials.");
            }
            else
            {
                errorMessage = result.ErrorMessage ?? "An error occurred during registration.";
            }
        }
        catch (Exception ex)
        {
            errorMessage = $"An unexpected error occurred: {ex.Message}";
        }
        finally
        {
            isSubmitting = false;
        }
    }
    
    private async Task SaveDraft(int completedStep)
    {
        if (string.IsNullOrEmpty(model.PersonalInfo?.Email))
            return;
        
        try
        {
            await DraftService.SaveDraftAsync(new SaveDraftRequest
            {
                Email = model.PersonalInfo.Email,
                CurrentStep = completedStep,
                CompletedSteps = Enumerable.Range(0, completedStep + 1).ToArray(),
                DraftData = model
            });
        }
        catch (Exception ex)
        {
            // Log error but don't block user
            Console.Error.WriteLine($"Failed to save draft: {ex.Message}");
        }
    }
    
    private void HandleStep1Validation(bool isValid)
    {
        // Re-render to update Next button state
        StateHasChanged();
    }
    
    private void HandleStep2Validation(bool isValid)
    {
        StateHasChanged();
    }
    
    private void HandleStep3Validation(bool isValid)
    {
        StateHasChanged();
    }
    
    private string? GetQueryParameter(string key)
    {
        var uri = new Uri(NavigationManager.Uri);
        var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
        return query[key];
    }
}
```

#### Step 2: Update PersonalInformationStep Component (4 hours)

**File**: `src/NBT.WebUI/Components/Registration/PersonalInformationStep.razor`

```csharp
@inject IStudentService StudentService

<FluentStack Orientation="Orientation.Vertical" VerticalGap="15">
    <h3>Personal Information</h3>
    
    <!-- ID Type Selection -->
    <FluentSelect Label="ID Type" 
                  @bind-Value="Model.IDType"
                  Required="true"
                  TOption="string">
        <FluentOption Value="@("SA_ID")">South African ID</FluentOption>
        <FluentOption Value="@("FOREIGN_ID")">Foreign ID</FluentOption>
        <FluentOption Value="@("PASSPORT")">Passport</FluentOption>
    </FluentSelect>
    
    <!-- ID Number -->
    <FluentTextField Label="ID Number" 
                     @bind-Value="Model.IDNumber"
                     @bind-Value:after="OnIDNumberChanged"
                     Required="true"
                     Placeholder="Enter your ID or Passport number"
                     Maxlength="20" />
    
    <!-- First Name -->
    <FluentTextField Label="First Name" 
                     @bind-Value="Model.FirstName"
                     @bind-Value:after="OnFieldChanged"
                     Required="true" />
    
    <!-- Last Name -->
    <FluentTextField Label="Last Name" 
                     @bind-Value="Model.LastName"
                     @bind-Value:after="OnFieldChanged"
                     Required="true" />
    
    <!-- Email -->
    <FluentTextField Label="Email" 
                     @bind-Value="Model.Email"
                     @bind-Value:after="OnFieldChanged"
                     Required="true"
                     Type="TextFieldType.Email" />
    
    <!-- Phone -->
    <FluentTextField Label="Phone Number" 
                     @bind-Value="Model.Phone"
                     @bind-Value:after="OnFieldChanged"
                     Required="true"
                     Placeholder="+27 XX XXX XXXX" />
    
    <!-- Date of Birth (manual entry if not SA ID) -->
    @if (Model.IDType != "SA_ID" || !isDateAutoFilled)
    {
        <FluentDatePicker Label="Date of Birth" 
                          @bind-Value="Model.DateOfBirth"
                          @bind-Value:after="OnFieldChanged"
                          Required="true" />
    }
    else
    {
        <FluentTextField Label="Date of Birth" 
                         Value="@Model.DateOfBirth.ToString("yyyy-MM-dd")"
                         Disabled="true"
                         Appearance="Appearance.Filled" />
        <small>Extracted from SA ID number</small>
    }
    
    <!-- Gender (manual if not SA ID) -->
    @if (Model.IDType != "SA_ID" || !isGenderAutoFilled)
    {
        <FluentSelect Label="Gender" 
                      @bind-Value="Model.Gender"
                      @bind-Value:after="OnFieldChanged"
                      Required="true"
                      TOption="string">
            <FluentOption Value="@("Male")">Male</FluentOption>
            <FluentOption Value="@("Female")">Female</FluentOption>
            <FluentOption Value="@("Other")">Other</FluentOption>
        </FluentSelect>
    }
    else
    {
        <FluentTextField Label="Gender" 
                         Value="@Model.Gender"
                         Disabled="true"
                         Appearance="Appearance.Filled" />
        <small>Extracted from SA ID number</small>
    }
    
    <!-- Ethnicity -->
    <FluentSelect Label="Ethnicity" 
                  @bind-Value="Model.Ethnicity"
                  @bind-Value:after="OnFieldChanged"
                  Required="true"
                  TOption="string">
        <FluentOption Value="@("African")">African</FluentOption>
        <FluentOption Value="@("Coloured")">Coloured</FluentOption>
        <FluentOption Value="@("Indian")">Indian</FluentOption>
        <FluentOption Value="@("White")">White</FluentOption>
        <FluentOption Value="@("Other")">Other</FluentOption>
    </FluentSelect>
    
    <!-- Nationality (for Foreign ID / Passport) -->
    @if (Model.IDType == "FOREIGN_ID" || Model.IDType == "PASSPORT")
    {
        <FluentTextField Label="Nationality" 
                         @bind-Value="Model.Nationality"
                         @bind-Value:after="OnFieldChanged"
                         Required="true" />
        
        <FluentTextField Label="Country of Origin" 
                         @bind-Value="Model.CountryOfOrigin"
                         @bind-Value:after="OnFieldChanged"
                         Required="true" />
    }
    
    @if (!string.IsNullOrEmpty(validationMessage))
    {
        <FluentMessageBar Intent="@(isValid ? MessageIntent.Success : MessageIntent.Error)">
            @validationMessage
        </FluentMessageBar>
    }
</FluentStack>

@code {
    [Parameter] public PersonalInformationModel Model { get; set; } = new();
    [Parameter] public EventCallback<bool> OnValidationChanged { get; set; }
    
    private bool isDateAutoFilled = false;
    private bool isGenderAutoFilled = false;
    private bool isValid = false;
    private string validationMessage = string.Empty;
    
    private async Task OnIDNumberChanged()
    {
        isValid = false;
        isDateAutoFilled = false;
        isGenderAutoFilled = false;
        validationMessage = string.Empty;
        
        if (string.IsNullOrWhiteSpace(Model.IDNumber))
        {
            await NotifyValidationChanged();
            return;
        }
        
        // Auto-extract from SA ID
        if (Model.IDType == "SA_ID" && Model.IDNumber.Length == 13)
        {
            try
            {
                // Extract DOB: YYMMDD format
                var year = int.Parse(Model.IDNumber.Substring(0, 2));
                var month = int.Parse(Model.IDNumber.Substring(2, 2));
                var day = int.Parse(Model.IDNumber.Substring(4, 2));
                
                // Determine century
                var fullYear = year >= 0 && year <= 30 ? 2000 + year : 1900 + year;
                
                Model.DateOfBirth = new DateTime(fullYear, month, day);
                isDateAutoFilled = true;
                
                // Extract Gender: 5000-9999 = Male, 0000-4999 = Female
                var genderDigit = int.Parse(Model.IDNumber.Substring(6, 4));
                Model.Gender = genderDigit >= 5000 ? "Male" : "Female";
                isGenderAutoFilled = true;
                
                validationMessage = "Date of birth and gender extracted from SA ID";
            }
            catch
            {
                validationMessage = "Invalid SA ID format";
            }
        }
        
        // Check for duplicate
        var isDuplicate = await StudentService.CheckDuplicateAsync(Model.IDNumber);
        if (isDuplicate)
        {
            validationMessage = "This ID number is already registered. Please contact support if this is an error.";
            isValid = false;
        }
        else if (Model.IsValid())
        {
            validationMessage = "ID number is valid and available";
            isValid = true;
        }
        
        // CRITICAL: Do NOT automatically advance to next step
        // Only notify parent of validation state change
        await NotifyValidationChanged();
    }
    
    private async Task OnFieldChanged()
    {
        // Re-validate model
        isValid = Model.IsValid();
        
        // CRITICAL: Do NOT automatically advance to next step
        // Only notify parent of validation state change
        await NotifyValidationChanged();
    }
    
    private async Task NotifyValidationChanged()
    {
        await OnValidationChanged.InvokeAsync(isValid);
    }
}
```

#### Step 3: Test Wizard Navigation (4 hours)

Create comprehensive test suite:

**File**: `tests/NBT.WebUI.Tests/RegistrationWizardTests.cs`

```csharp
[TestClass]
public class RegistrationWizardTests : TestContext
{
    [TestMethod]
    public void Step1_NextButton_DisabledUntilAllFieldsValid()
    {
        // Arrange
        var cut = RenderComponent<RegistrationWizard>();
        
        // Act - Enter partial data
        cut.Find("#idNumber").Change("9001015009087");
        
        // Assert - Next button should still be disabled
        var nextButton = cut.Find("button:contains('Next')");
        Assert.IsTrue(nextButton.HasAttribute("disabled"));
        
        // Act - Complete all required fields
        cut.Find("#firstName").Change("John");
        cut.Find("#lastName").Change("Doe");
        cut.Find("#email").Change("john.doe@example.com");
        cut.Find("#phone").Change("+27123456789");
        cut.Find("#ethnicity").Change("African");
        
        // Assert - Next button should now be enabled
        Assert.IsFalse(nextButton.HasAttribute("disabled"));
    }
    
    [TestMethod]
    public void SAIDAutoFill_DoesNotAdvanceStep()
    {
        // Arrange
        var cut = RenderComponent<RegistrationWizard>();
        
        // Act - Enter SA ID with auto-fill
        cut.Find("#idType").Change("SA_ID");
        cut.Find("#idNumber").Change("9001015009087");
        
        // Assert - Should still be on step 1
        var currentStep = cut.Find(".wizard-step.current");
        Assert.AreEqual("Personal Information", currentStep.TextContent);
        
        // Assert - Step 2 should not be visible
        Assert.ThrowsException<ElementNotFoundException>(() => 
            cut.Find("h3:contains('Academic & Test Selection')")
        );
    }
    
    [TestMethod]
    public async Task WizardFlow_CompletesAllSteps_InSequence()
    {
        // Arrange
        var cut = RenderComponent<RegistrationWizard>();
        
        // Act - Complete Step 1
        CompleteStep1(cut);
        cut.Find("button:contains('Next')").Click();
        await Task.Delay(500); // Allow state update
        
        // Assert - On Step 2
        Assert.IsNotNull(cut.Find("h3:contains('Academic & Test Selection')"));
        
        // Act - Complete Step 2
        CompleteStep2(cut);
        cut.Find("button:contains('Next')").Click();
        await Task.Delay(500);
        
        // Assert - On Step 3
        Assert.IsNotNull(cut.Find("h3:contains('Pre-Test Questionnaire')"));
        
        // Act - Complete Step 3
        CompleteStep3(cut);
        cut.Find("button:contains('Next')").Click();
        await Task.Delay(500);
        
        // Assert - On Step 4 (Review)
        Assert.IsNotNull(cut.Find("h3:contains('Review & Confirmation')"));
        
        // Act - Accept terms and submit
        cut.Find("#termsCheckbox").Change(true);
        var registerButton = cut.Find("button:contains('Register')");
        Assert.IsFalse(registerButton.HasAttribute("disabled"));
    }
    
    private void CompleteStep1(IRenderedComponent<RegistrationWizard> cut)
    {
        cut.Find("#idType").Change("SA_ID");
        cut.Find("#idNumber").Change("9001015009087");
        cut.Find("#firstName").Change("John");
        cut.Find("#lastName").Change("Doe");
        cut.Find("#email").Change("john.doe@example.com");
        cut.Find("#phone").Change("+27123456789");
        cut.Find("#ethnicity").Change("African");
    }
    
    private void CompleteStep2(IRenderedComponent<RegistrationWizard> cut)
    {
        cut.Find("#school").Change("Test High School");
        cut.Find("#grade").Change("Grade 12");
        cut.Find("#testType").Change("AQL");
        // ... select venue and session
    }
    
    private void CompleteStep3(IRenderedComponent<RegistrationWizard> cut)
    {
        // Answer all questionnaire questions
        // ...
    }
}
```

---

### Day 3-4: Draft Resumption Implementation (16 hours)

[Follow tasks T302A through T302K from TASKS-UPDATED-2025-11-09.md]

---

### Day 5: Result Barcode System (8 hours)

[Follow tasks T701A through T701H from TASKS-UPDATED-2025-11-09.md]

---

## 🔄 DEPLOYMENT WORKFLOW

### Step 1: Create Feature Branch
```bash
git checkout main
git pull origin main
git checkout -b feature/registration-wizard-fixes
```

### Step 2: Implement Changes
[Complete all tasks for the feature]

### Step 3: Run Tests
```bash
cd src/NBT.WebUI.Tests
dotnet test

cd ../NBT.Application.Tests
dotnet test

cd ../NBT.Infrastructure.Tests
dotnet test
```

### Step 4: Build and Verify
```bash
cd ../../
dotnet build --configuration Release
```

### Step 5: Push to GitHub
```bash
git add .
git commit -m "Fix registration wizard step navigation and add draft resumption"
git push origin feature/registration-wizard-fixes
```

### Step 6: Create Pull Request
- Navigate to GitHub repository
- Create Pull Request from feature branch to main
- Wait for CI/CD checks to pass
- Request review from team lead

### Step 7: Merge and Deploy
- Approve and merge PR (squash and merge)
- Automatic deployment to staging via GitHub Actions
- Perform UAT on staging
- Approve production deployment

---

## 📋 CHECKLIST

### Registration Wizard Fixes
- [ ] Step 1 Next button activation logic fixed
- [ ] Step 2 Next button activation logic fixed
- [ ] Step 3 Next button activation logic fixed
- [ ] Step 4 Register button activation logic fixed
- [ ] NBT number generation moved to server-side final submit
- [ ] Wizard skip-to-end bug fixed
- [ ] Step validation enforcement added
- [ ] All wizard flow tests passing

### Draft Resumption
- [ ] RegistrationDraft entity created
- [ ] Database migration applied
- [ ] IDraftService interface and implementation created
- [ ] Draft API endpoints created
- [ ] DraftResumptionDialog component created
- [ ] Wizard integrated with draft save/restore
- [ ] Background cleanup job created
- [ ] All draft resumption tests passing

### Result Barcode System
- [ ] Barcode column added to TestResult
- [ ] IBarcodeGenerator interface and implementation created
- [ ] Results import generates barcodes
- [ ] Barcode lookup API created
- [ ] PDF generator includes barcode
- [ ] All barcode tests passing

---

## 🎯 NEXT STEPS

After completing Priority 1 fixes:
1. ✅ Implement Payment Upload & Reconciliation (Week 2)
2. ✅ Create Landing Page & Role-Based Dashboards (Week 3)
3. ✅ Comprehensive Testing & UAT (Week 4)
4. ✅ Production Deployment

---

## 📞 SUPPORT

**Questions?** Contact NBT Technical Team  
**Slack Channel:** #nbt-development  
**Email:** tech@nbt.ac.za

---

**Status:** ✅ READY TO START IMPLEMENTATION

**Last Updated:** 2025-11-09
