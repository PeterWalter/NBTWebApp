# Registration Wizard Restructuring - Complete

## Overview
The registration wizard has been successfully restructured according to requirements. The wizard now has a more streamlined flow with combined steps and additional demographic and survey fields.

## Changes Made

### 1. Wizard Structure (Reduced from 7 steps to 6 steps)

#### **Step 1: Personal Details** (Combined ID Verification + Personal Information)
- ID Type selection (SA_ID, Foreign ID, Passport)
- ID Number with real-time validation
- Nationality and Country of Origin (for Foreign ID/Passport)
- First Name and Last Name
- Date of Birth (auto-calculates age)
- **Age** (auto-calculated, read-only)
- **Gender** (Male, Female, Other)
- **Ethnicity** (Black African, Coloured, Indian/Asian, White, Other, Prefer not to say)

#### **Step 2: Contact & Address** (Combined Contact + Address Information)
- Email Address
- Phone Number and Alternative Phone
- Complete address fields (Address Line 1 & 2, City, Province, Postal Code, Country)

#### **Step 3: Academic Information**
- School Name
- School Province
- Current Grade (10-12)
- Home Language

#### **Step 4: Survey Questions** (NEW)
- What motivates you to take the NBT?
- Career Interests
- Preferred Study Field
- Access to Computer (Yes/No)
- Internet Access at Home (Yes/No)
- Additional Comments

#### **Step 5: Special Accommodations**
- Requires Accommodation checkbox
- Accommodation Details (if required)

#### **Step 6: Review and Submit**
- Complete review of all entered information
- Submit button to generate NBT number
- Success page with generated NBT number

### 2. Database Changes

#### New Fields Added to Student Entity:
```csharp
- Age (int?) - Calculated from date of birth
- Ethnicity (string?) - Required demographic field
- MotivationForTesting (string?) - Survey field
- CareerInterests (string?) - Survey field
- PreferredStudyField (string?) - Survey field
- HasAccessToComputer (bool) - Survey field
- HasInternetAccess (bool) - Survey field
- AdditionalComments (string?) - Survey field
```

#### Migration Applied:
- Migration: `AddStudentSurveyAndEthnicityFields`
- Applied successfully to database

### 3. DTO Updates

All student DTOs updated to include:
- `StudentDto` - includes all new fields
- `CreateStudentDto` - includes all new fields for creation
- Student service mapping updated to handle new fields

### 4. Frontend Updates

#### RegistrationFormModel.cs
- Added Age, Ethnicity properties
- Added survey question properties
- All fields properly validated

#### Register.razor
- Restructured wizard steps
- Added ethnicity dropdown with options
- Age auto-calculation on date of birth change
- Survey questions with text areas and checkboxes
- Review step displays all new fields
- Fixed step validation (now checks for step 6 instead of 7)

### 5. Key Features

#### Age Auto-Calculation
```csharp
private void OnDateOfBirthChanged(DateTime? dateOfBirth)
{
    _model.DateOfBirth = dateOfBirth;
    if (dateOfBirth.HasValue)
    {
        var today = DateTime.Today;
        var age = today.Year - dateOfBirth.Value.Year;
        if (dateOfBirth.Value.Date > today.AddYears(-age)) age--;
        _model.Age = age;
    }
}
```

#### Ethnicity Options
- Black African
- Coloured
- Indian/Asian
- White
- Other
- Prefer not to say

#### Survey Questions
Pre-test questionnaire collects:
- Motivation for testing
- Career aspirations
- Study field preferences
- Technology access information
- Additional feedback

## Fixed Issues

### NBT Number Generation Timing
The wizard was generating NBT numbers and navigating to login before completing all steps. This has been fixed by:
1. Only allowing submission on the final review step (Step 6)
2. Proper validation of all required fields before submission
3. NBT number is generated only on final submit, not during step navigation

## Testing

### Build Status
✅ Solution builds successfully with no errors

### Applications Running
✅ Web API running on: https://localhost:7001
✅ Blazor WebUI running on: https://localhost:5001

### Test Registration Flow
Navigate to: https://localhost:5001/register

#### Test Steps:
1. **Step 1 - Personal Details**
   - Select ID Type
   - Enter ID Number (validates in real-time)
   - For SA_ID: Enter 13-digit ID (e.g., 9001015009087)
   - For Foreign/Passport: Enter ID + Nationality + Country
   - Enter First Name, Last Name
   - Select Date of Birth (Age auto-calculates)
   - Select Gender
   - Select Ethnicity

2. **Step 2 - Contact & Address**
   - Enter Email
   - Enter Phone Number
   - Enter Address details
   - Select Province

3. **Step 3 - Academic Info**
   - Enter School Name
   - Select School Province (optional)
   - Enter Grade (10-12)
   - Enter Home Language

4. **Step 4 - Survey**
   - Answer survey questions
   - Check technology access boxes

5. **Step 5 - Accommodations**
   - Indicate if special accommodations needed
   - Provide details if required

6. **Step 6 - Review**
   - Review all information
   - Click "Submit Registration"
   - **NBT number is generated at this point**
   - Success page displays with NBT number
   - User can proceed to login

## Foreign ID Support

The system fully supports applicants without SA ID:
- **Foreign ID** option for foreign nationals
- **Passport** option for passport holders
- Additional fields: Nationality and Country of Origin
- Same validation and NBT number generation process

## Compliance

### WCAG 2.1 AA Accessibility
- All form fields have proper labels
- Required fields marked with asterisks
- Validation messages are screen-reader friendly
- Keyboard navigation fully supported
- Fluent UI components ensure accessibility

### Data Integrity
- All fields properly validated
- ID number validation (Luhn algorithm for SA ID)
- Duplicate prevention checks
- Audit fields (CreatedDate, CreatedBy) tracked

### NBT Number Generation
- Uses Luhn algorithm for checksum validation
- 14-digit format maintained
- Unique per student
- Generated only on successful registration

## Developer Notes

### Next Steps
1. ✅ Restructured wizard steps
2. ✅ Added Age, Gender, Ethnicity to Step 1
3. ✅ Combined Step 1 & 2
4. ✅ Combined Step 3 & 4
5. ✅ Added Survey Questions step
6. ✅ Updated database schema
7. ✅ Updated DTOs and services
8. ✅ Fixed NBT number generation timing
9. 🔄 Ready for testing and validation

### Configuration
No configuration changes required. All changes are code and database schema updates that have been applied.

### Code Locations
- **Frontend Model**: `src/NBT.WebUI.Client/Models/RegistrationFormModel.cs`
- **Frontend Page**: `src/NBT.WebUI.Client/Pages/Registration/Register.razor`
- **Entity**: `src/NBT.Domain/Entities/Student.cs`
- **DTOs**: `src/NBT.Application/Students/DTOs/StudentDto.cs`
- **Service**: `src/NBT.Application/Students/Services/StudentService.cs`
- **Migration**: `src/NBT.Infrastructure/Migrations/*_AddStudentSurveyAndEthnicityFields.cs`

## Summary

The registration wizard has been successfully restructured to:
1. ✅ Combine Step 1 & 2 into "Personal Details" (ID + Personal Info with Age, Gender, Ethnicity)
2. ✅ Combine Step 3 & 4 into "Contact & Address"
3. ✅ Keep Academic Info as Step 3
4. ✅ Add new "Survey Questions" as Step 4
5. ✅ Keep Special Accommodations as Step 5
6. ✅ Keep Review and Submit as Step 6
7. ✅ NBT number generated only after final submission (no premature navigation)
8. ✅ All demographic and survey fields captured
9. ✅ Foreign ID/Passport support maintained
10. ✅ Database updated with new fields

The system is now ready for user acceptance testing and deployment.
