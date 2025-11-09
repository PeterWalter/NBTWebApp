# Continue Development - Quick Guide

## Current Status
✅ **Registration Wizard Fixed** - All steps working properly with validation  
🔄 **Next Phase**: Test Booking Module (Phase 3)

---

## Quick Start Commands

### Start Development Servers
```powershell
# Terminal 1 - WebAPI (Backend)
cd "D:\projects\source code\NBTWebApp\src\NBT.WebAPI"
dotnet run

# Terminal 2 - WebUI (Frontend)
cd "D:\projects\source code\NBTWebApp\src\NBT.WebUI"
dotnet run
```

### Build & Test
```powershell
# Build entire solution
cd "D:\projects\source code\NBTWebApp"
dotnet build

# Run tests
dotnet test

# Apply/create migrations
cd src\NBT.Infrastructure
dotnet ef migrations add MigrationName --startup-project ..\NBT.WebAPI
dotnet ef database update --startup-project ..\NBT.WebAPI
```

### Git Workflow
```powershell
# Current branch
git branch  # Should be on: feature/comprehensive-nbt-implementation

# Commit changes
git add -A
git commit -m "Your descriptive message"

# Push to GitHub
git push origin feature/comprehensive-nbt-implementation

# Pull latest
git pull origin feature/comprehensive-nbt-implementation
```

---

## URLs

| Service | URL | Description |
|---------|-----|-------------|
| **WebUI** | https://localhost:5001 | Main application |
| **WebAPI** | https://localhost:7001 | API backend |
| **Swagger** | https://localhost:7001/swagger | API documentation |
| **Registration** | https://localhost:5001/register | Student registration |
| **Login** | https://localhost:5001/login | User login |

---

## Next Phase: Test Booking Module

### What to Build

#### 1. Booking Wizard (Similar to Registration Wizard)
Create: `src\NBT.WebUI.Client\Pages\Booking\BookTest.razor`

**Steps:**
1. **Test Selection** - Choose AQL only or AQL + MAT
2. **Venue Selection** - Choose from available venues by type
3. **Date Selection** - Pick test date (check closing dates)
4. **Review & Confirm** - Review booking and submit

#### 2. Booking Service
Create: `src\NBT.WebUI.Client\Services\BookingService.cs`

**Methods:**
- `Task<List<VenueDto>> GetAvailableVenuesAsync()`
- `Task<List<TestDateDto>> GetTestDatesAsync(Guid venueId)`
- `Task<BookingResult> CreateBookingAsync(BookingModel model)`
- `Task<bool> CanBookTestAsync(string nbtNumber)` - Check business rules

#### 3. Booking Controller (API)
Already exists: `src\NBT.WebAPI\Controllers\BookingsController.cs`

**Add/Update Endpoints:**
- `GET /api/bookings/available-venues` - Get venues with availability
- `GET /api/bookings/test-dates` - Get test dates
- `POST /api/bookings` - Create new booking
- `PUT /api/bookings/{id}` - Update booking (before closing date)
- `GET /api/bookings/student/{nbtNumber}` - Get student's bookings

#### 4. Business Rules to Implement

```csharp
// In BookingService.cs
private async Task<BookingValidationResult> ValidateBookingAsync(string nbtNumber)
{
    // 1. Check for active booking
    var activeBooking = await GetActiveBookingAsync(nbtNumber);
    if (activeBooking != null && activeBooking.ClosingDate > DateTime.Now)
    {
        return BookingValidationResult.Fail("You already have an active booking");
    }

    // 2. Check tests per year (max 2)
    var currentYearTests = await GetTestsInCurrentYearAsync(nbtNumber);
    if (currentYearTests.Count >= 2)
    {
        return BookingValidationResult.Fail("Maximum 2 tests per year reached");
    }

    // 3. Check booking is within intake period (after April 1)
    var intakeStartDate = new DateTime(DateTime.Now.Year, 4, 1);
    if (DateTime.Now < intakeStartDate)
    {
        return BookingValidationResult.Fail($"Bookings open on {intakeStartDate:dd MMMM yyyy}");
    }

    return BookingValidationResult.Success();
}
```

### Key Files to Modify/Create

1. **Frontend (WebUI.Client)**
   - `Pages\Booking\BookTest.razor` - New booking wizard
   - `Models\BookingFormModel.cs` - Booking form data model
   - `Services\IBookingService.cs` - Interface
   - `Services\BookingService.cs` - Implementation

2. **Backend (WebAPI)**
   - `Controllers\BookingsController.cs` - Update endpoints
   - Add validation methods

3. **Application Layer**
   - `Application\Services\BookingService.cs` - Business logic
   - `Application\DTOs\BookingDto.cs` - Update DTOs
   - `Application\DTOs\VenueDto.cs` - Venue info
   - `Application\DTOs\TestDateDto.cs` - Test date info

---

## Business Rules Reference

### Booking Rules
- ✅ One active booking at a time
- ✅ Can book another test only after closing date passes
- ✅ Maximum 2 tests per year
- ✅ Tests valid for 3 years from booking date
- ✅ Booking changes allowed before closing date
- ✅ Bookings open from April 1 each year
- ✅ Test types: AQL only, or AQL + MAT combined

### Payment Rules (Phase 4)
- Installment payments allowed
- Payments in order of test dates
- Costs vary by intake year
- Only paid tests downloadable as PDF
- Bank payment file uploads

### Results Rules (Phase 5)
- Each test has unique Barcode
- Performance levels for AL, QL, MAT
- Multiple test tracking
- Results viewable after payment

### Venue Rules
- Types: National, Special Session, Research, Other
- TestSession linked to Venue (not Room)
- Online tests with video/sound requirements
- Sunday tests highlighted

---

## Code Templates

### Booking Wizard Template

```razor
@page "/book-test"
@using NBT.WebUI.Client.Models
@using NBT.WebUI.Client.Services
@inject IBookingService BookingService
@inject NavigationManager Navigation

<PageTitle>Book NBT Test</PageTitle>

<div class="booking-container">
    <FluentCard class="wizard-card">
        <h1 class="wizard-title">
            <FluentIcon Value="@(new Icons.Regular.Size24.CalendarAdd())" />
            Book Your NBT Test
        </h1>

        @if (!_bookingComplete)
        {
            <FluentWizard @ref="_wizard"
                          @bind-Value="@_currentStepIndex" 
                          StepperPosition="StepperPosition.Top"
                          Height="auto"
                          DisplayStepNumber="true">
                
                <FluentWizardStep Label="Test Selection" 
                                Summary="Choose your test type"
                                OnChange="@OnStepChangeAsync">
                    <div class="step-content">
                        <h3>Select Test Type</h3>
                        <FluentRadioGroup @bind-Value="_model.TestType">
                            <FluentRadio Value="AQL">AQL Only</FluentRadio>
                            <FluentRadio Value="AQL_MAT">AQL + Mathematics</FluentRadio>
                        </FluentRadioGroup>
                    </div>
                </FluentWizardStep>

                <FluentWizardStep Label="Venue" 
                                Summary="Choose test venue"
                                OnChange="@OnStepChangeAsync">
                    <div class="step-content">
                        <h3>Select Venue</h3>
                        <FluentSelect Label="Venue Type" 
                                    @bind-Value="_model.VenueType"
                                    TOption="string"
                                    Items="@_venueTypes">
                        </FluentSelect>
                        
                        <FluentSelect Label="Venue" 
                                    @bind-Value="_model.VenueId"
                                    TOption="Guid"
                                    Items="@_availableVenues">
                        </FluentSelect>
                    </div>
                </FluentWizardStep>

                <FluentWizardStep Label="Date" 
                                Summary="Choose test date"
                                OnChange="@OnStepChangeAsync">
                    <div class="step-content">
                        <h3>Select Test Date</h3>
                        <FluentDatePicker @bind-Value="_model.TestDate"
                                        Label="Test Date" />
                        <p>Closing Date: @_selectedClosingDate</p>
                    </div>
                </FluentWizardStep>

                <FluentWizardStep Label="Confirm" 
                                Summary="Review booking">
                    <div class="step-content">
                        <h3>Confirm Your Booking</h3>
                        <!-- Review details -->
                        <FluentButton Appearance="Appearance.Accent" 
                                    OnClick="@HandleSubmitAsync">
                            Complete Booking
                        </FluentButton>
                    </div>
                </FluentWizardStep>
            </FluentWizard>
        }
        else
        {
            <div class="success-content">
                <h2>Booking Successful!</h2>
                <p>Booking Reference: @_bookingReference</p>
            </div>
        }
    </FluentCard>
</div>

@code {
    private FluentWizard? _wizard;
    private BookingFormModel _model = new();
    private int _currentStepIndex = 0;
    private bool _bookingComplete = false;
    private string? _bookingReference;
    
    // ... implementation
}
```

### Booking Service Template

```csharp
using NBT.WebUI.Client.Models;
using System.Net.Http.Json;

namespace NBT.WebUI.Client.Services;

public class BookingService : IBookingService
{
    private readonly HttpClient _httpClient;

    public BookingService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<BookingResult> CreateBookingAsync(BookingFormModel model)
    {
        try
        {
            var createDto = new
            {
                nbtNumber = model.NBTNumber,
                testType = model.TestType,
                venueId = model.VenueId,
                testDate = model.TestDate
            };

            var response = await _httpClient.PostAsJsonAsync("/api/bookings", createDto);

            if (response.IsSuccessStatusCode)
            {
                var bookingDto = await response.Content.ReadFromJsonAsync<BookingResponseDto>();
                return new BookingResult
                {
                    Success = true,
                    BookingReference = bookingDto?.Reference
                };
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                return new BookingResult
                {
                    Success = false,
                    ErrorMessage = error
                };
            }
        }
        catch (Exception ex)
        {
            return new BookingResult
            {
                Success = false,
                ErrorMessage = $"An error occurred: {ex.Message}"
            };
        }
    }

    public async Task<List<VenueDto>> GetAvailableVenuesAsync()
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<List<VenueDto>>("/api/bookings/available-venues");
            return response ?? new List<VenueDto>();
        }
        catch
        {
            return new List<VenueDto>();
        }
    }

    // ... other methods
}
```

---

## JSON Fix (If Needed)

If you encounter JSON property name errors, run:

```powershell
.\APPLY-JSON-FIX.ps1
```

Or manually ensure all DTOs have:
```csharp
using System.Text.Json.Serialization;

public class MyDto
{
    [JsonPropertyName("myProperty")]
    public string MyProperty { get; set; }
}
```

---

## Testing Checklist

### Registration Wizard (✅ Done)
- [x] Step 1: Personal & Contact validation
- [x] Step 2: Academic & Survey validation
- [x] Step 3: Review and submit
- [x] SA ID auto-extraction
- [x] Duplicate detection
- [x] NBT number generation
- [x] Success screen display

### Booking Wizard (⏳ To Do)
- [ ] Test type selection
- [ ] Venue selection with filtering
- [ ] Date selection with availability
- [ ] Closing date validation
- [ ] One-active-booking rule
- [ ] Max 2 tests per year rule
- [ ] Booking change functionality
- [ ] Success screen with reference

---

## Helpful Resources

### Documentation
- [Constitution](specs/003-nbt-complete-system/CONSTITUTION.md)
- [Specification](specs/003-nbt-complete-system/SPECIFICATION.md)
- [Implementation Status](specs/003-nbt-complete-system/IMPLEMENTATION-STATUS.md)
- [Task Breakdown](specs/003-nbt-complete-system/TASKS.md)

### API Documentation
- Swagger UI: https://localhost:7001/swagger
- Test with Postman or curl

### Database
```sql
-- View students
SELECT * FROM Students;

-- View bookings
SELECT * FROM Bookings;

-- View test sessions
SELECT * FROM TestSessions;

-- View venues
SELECT * FROM TestVenues;
```

---

## Common Issues & Solutions

### Issue: JSON Property Error
**Solution**: Run `.\APPLY-JSON-FIX.ps1` or add `[JsonPropertyName]` attributes

### Issue: Build Errors
**Solution**: 
```powershell
dotnet clean
dotnet restore
dotnet build
```

### Issue: Database Out of Sync
**Solution**: 
```powershell
cd src\NBT.Infrastructure
dotnet ef database update --startup-project ..\NBT.WebAPI
```

### Issue: Port Already in Use
**Solution**: Change ports in `launchSettings.json` or kill the process:
```powershell
Get-Process -Name dotnet | Stop-Process
```

---

## Contact & Support

- **GitHub Issues**: https://github.com/PeterWalter/NBTWebApp/issues
- **Branch**: feature/comprehensive-nbt-implementation
- **Documentation**: See `/specs` folder

---

**Last Updated**: 2025-11-09  
**Status**: Ready for Phase 3 (Test Booking Module)  
**Next Milestone**: Complete booking wizard with full validation

---

## Quick Commands Summary

```powershell
# Start development
cd "D:\projects\source code\NBTWebApp"
# Terminal 1: cd src\NBT.WebAPI; dotnet run
# Terminal 2: cd src\NBT.WebUI; dotnet run

# Build
dotnet build

# Test
dotnet test

# Migrations
cd src\NBT.Infrastructure
dotnet ef migrations add Name --startup-project ..\NBT.WebAPI
dotnet ef database update --startup-project ..\NBT.WebAPI

# Git
git add -A
git commit -m "Message"
git push origin feature/comprehensive-nbt-implementation

# JSON Fix
.\APPLY-JSON-FIX.ps1
```

---

🚀 **Ready to continue development!** Start with the Test Booking Module (Phase 3).
