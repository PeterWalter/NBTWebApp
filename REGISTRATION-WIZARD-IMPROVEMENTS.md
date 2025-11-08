# Registration Wizard Improvements

## Overview
Updated the registration wizard to combine steps, extract DOB and Gender from SA ID numbers automatically, and streamline the user experience.

## Changes Made

### 1. Step Consolidation
The wizard has been reduced from 6 steps to 5 steps:

#### **Step 1: Personal & Contact Information** (formerly Steps 1 & 2)
- Identity Verification (ID Type, ID Number)
- Foreign ID fields (Nationality, Country of Origin) - shown conditionally
- Personal Details (First Name, Last Name, DOB, Gender, Ethnicity)
- Contact Information (Email, Phone Numbers)

#### **Step 2: Address Information**
- Residential address details
- No changes from original

#### **Step 3: Academic Information & Survey Questions** (formerly Steps 3 & 4)
- Academic Details (School Name, School Province, Current Grade, Home Language)
- Pre-Test Questionnaire (Motivation, Career Interests, Study Field, Computer Access, Additional Comments)

#### **Step 4: Special Accommodations**
- Accommodation requirements
- No changes from original

#### **Step 5: Review and Submit** (formerly Step 6)
- Review all information
- Submit registration
- Updated validation to check for step 5 instead of step 6

### 2. SA ID Auto-Extraction
When a valid 13-digit South African ID is entered:

#### **Date of Birth Extraction**
- Automatically extracts DOB from the first 6 digits (YYMMDD format)
- Intelligently determines century (1900s vs 2000s)
- Disables the DOB field when extracted from ID
- `_dobExtractedFromID` flag tracks extraction state

#### **Gender Extraction**
- Automatically extracts gender from the 7th digit (0-4 = Female, 5-9 = Male)
- Disables the Gender dropdown when extracted from ID
- `_genderExtractedFromID` flag tracks extraction state

### 3. Age Field Removal
- Removed `Age` property from `RegistrationFormModel`
- Removed age calculation logic (`OnDateOfBirthChanged` method)
- Removed age display from wizard UI
- Age is not needed if DOB is present

### 4. Survey Data Integration
Added survey question data to the registration DTO sent to the API:
- `motivationForTesting`
- `careerInterests`
- `preferredStudyField`
- `hasAccessToComputer`
- `hasInternetAccess`
- `additionalComments`

### 5. Ethnicity Field Addition
- Added `ethnicity` to the registration DTO
- Ensures ethnicity data is captured and sent to the backend

## Technical Details

### New Code in Register.razor

```csharp
// New flags for tracking auto-extraction
private bool _dobExtractedFromID = false;
private bool _genderExtractedFromID = false;

// Enhanced OnIDNumberChanged method
private async Task OnIDNumberChanged(ChangeEventArgs e)
{
    // ... existing validation code ...
    
    // Extract DOB and Gender from SA ID
    if (_model.IDType == "SA_ID" && idNumber.Length == 13 && idNumber.All(char.IsDigit))
    {
        ExtractDataFromSAID(idNumber);
    }
    else
    {
        _dobExtractedFromID = false;
        _genderExtractedFromID = false;
    }
    
    // ... rest of validation ...
}

// New extraction method
private void ExtractDataFromSAID(string idNumber)
{
    try
    {
        // Extract date of birth (first 6 digits: YYMMDD)
        var year = int.Parse(idNumber.Substring(0, 2));
        var month = int.Parse(idNumber.Substring(2, 2));
        var day = int.Parse(idNumber.Substring(4, 2));
        
        // Determine century
        var currentYearLastTwo = DateTime.Now.Year % 100;
        var century = year > currentYearLastTwo ? 1900 : 2000;
        var fullYear = century + year;
        
        _model.DateOfBirth = new DateTime(fullYear, month, day);
        _dobExtractedFromID = true;
        
        // Extract gender (7th digit: 0-4 = Female, 5-9 = Male)
        var genderDigit = int.Parse(idNumber.Substring(6, 1));
        _model.Gender = genderDigit < 5 ? "Female" : "Male";
        _genderExtractedFromID = true;
    }
    catch
    {
        _dobExtractedFromID = false;
        _genderExtractedFromID = false;
    }
}
```

### UI Updates

```razor
<!-- DOB field is disabled when extracted from SA ID -->
<FluentDatePicker Label="Date of Birth *" 
                @bind-Value="_model.DateOfBirth"
                Style="width: 100%;"
                Required="true"
                Max="@DateTime.Today.AddYears(-15)"
                Disabled="@_dobExtractedFromID" />

<!-- Gender field is disabled when extracted from SA ID -->
<FluentSelect Label="Gender *" 
            @bind-Value="_model.Gender"
            TOption="string"
            Items="@_genderOptions"
            OptionValue="@(x => x)"
            OptionText="@(x => x)"
            Style="width: 100%;"
            Required="true"
            Disabled="@_genderExtractedFromID" />
```

## User Experience Improvements

1. **Fewer Steps**: Reduced from 6 to 5 steps, making the wizard feel shorter and less overwhelming
2. **Smart Data Entry**: SA ID holders only need to enter their ID once - DOB and Gender are extracted automatically
3. **Clear Grouping**: Related information is now grouped together logically
4. **Consistent Flow**: Personal → Address → Academic/Survey → Accommodations → Review

## Foreign ID Support

For applicants without a South African ID:
- Can register with `FOREIGN_ID` or `PASSPORT` ID types
- Must manually enter all personal information including DOB and Gender
- Additional fields for Nationality and Country of Origin are shown

## Validation

- Step 5 (Review) validation updated to reflect new step count
- All required field validation remains in place
- SA ID Luhn checksum validation still active
- Duplicate ID checking still functional

## Next Steps

To test the wizard:
1. Navigate to `/register`
2. Try entering a valid SA ID number (13 digits)
3. Observe DOB and Gender auto-populate and become disabled
4. Complete the remaining wizard steps
5. Verify NBT number generation and registration success

Example SA ID for testing: `9801015800089` (DOB: 1998-01-01, Male)

## Files Modified

1. `src/NBT.WebUI.Client/Pages/Registration/Register.razor`
   - Combined wizard steps
   - Added SA ID extraction logic
   - Updated validation for 5 steps instead of 6

2. `src/NBT.WebUI.Client/Models/RegistrationFormModel.cs`
   - Removed `Age` property

3. `src/NBT.WebUI.Client/Services/RegistrationService.cs`
   - Added survey data fields to registration DTO
   - Added ethnicity field to registration DTO
