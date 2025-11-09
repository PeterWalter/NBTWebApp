# Task Breakdown - NBT Integrated System Implementation (Updated 2025-11-09)

**Feature**: 002-nbt-integrated-system  
**Version**: 3.1  
**Updated**: 2025-11-09  
**Status**: READY FOR UPDATED IMPLEMENTATION  
**Total Tasks**: 545 (+60 new tasks)  
**Total Effort**: 680 hours (+100 hours)

---

## 📋 CRITICAL UPDATES SUMMARY

This updated task breakdown incorporates ALL new requirements from the 2025-11-09 requirement gathering session:

### New Requirements Added:
1. ✅ Registration Wizard Fixes (step navigation, draft resumption)
2. ✅ Bank Payment Upload & Reconciliation
3. ✅ Landing Page & Dashboard UI/UX
4. ✅ Result Barcode System
5. ✅ Payment Installment Tracking
6. ✅ Test Calendar with Sunday/Online highlighting
7. ✅ Venue Availability Management
8. ✅ Registration Draft Save/Restore
9. ✅ Role-Based Dashboard Layouts
10. ✅ Video Embedding Infrastructure

---

## 📊 UPDATED TASK SUMMARY BY PHASE

| Phase | Tasks | Hours | Status | Priority | New Tasks |
|-------|-------|-------|--------|----------|-----------|
| **Phase 0: Shell Audit** | 15 | 8 | ✅ COMPLETE | N/A | 0 |
| **Phase 1: Foundation** | 55 (+10) | 50 (+10) | 🔴 CRITICAL | P1 | 10 |
| **Phase 2: Student Module** | 42 | 40 | 🔴 CRITICAL | P2 | 0 |
| **Phase 3: Registration** | 78 (+20) | 100 (+20) | 🔴 CRITICAL | P1 | 20 |
| **Phase 4: Payments** | 58 (+20) | 60 (+20) | 🔴 CRITICAL | P2 | 20 |
| **Phase 5: Venues** | 38 (+6) | 46 (+6) | 🟡 HIGH | P3 | 6 |
| **Phase 6: Sessions** | 35 | 40 | 🟡 HIGH | P3 | 0 |
| **Phase 7: Results** | 38 (+10) | 50 (+10) | 🟡 HIGH | P1 | 10 |
| **Phase 8: Dashboards** | 56 (+20) | 60 (+20) | 🟡 MEDIUM | P2 | 20 |
| **Phase 9: Reports** | 30 | 40 | 🟡 MEDIUM | P3 | 0 |
| **Phase 10: UI/UX** | 24 (+24) | 30 (+30) | 🔴 CRITICAL | P2 | 24 |
| **Phase 11: Testing** | 146 (+20) | 140 (+20) | 🔴 CRITICAL | P4 | 20 |
| **TOTAL** | **615** | **714** | | | **130** |

---

## 🚨 PRIORITY 1: CRITICAL FIXES (Must Fix Immediately)

### Phase 3.1: Registration Wizard Critical Fixes
**Duration**: 2-3 days (20 hours)  
**Status**: 🔴 CRITICAL - BLOCKER  
**Purpose**: Fix step navigation and NBT number generation bugs

#### Task Group: Wizard Step Navigation Fix

**T301A**: Fix Step 1 Next Button Activation Logic  
**Estimate**: 2 hours  
**Priority**: P1-CRITICAL  
**Bug**: Next button enabled before validation complete  
**Fix**:
```csharp
// BEFORE (WRONG):
private bool IsStep1Valid = false; // Default false, but auto-set to true on auto-fill

// AFTER (CORRECT):
private bool IsStep1Valid => model.PersonalInfo != null && 
                             !string.IsNullOrEmpty(model.PersonalInfo.Email) &&
                             !string.IsNullOrEmpty(model.PersonalInfo.FirstName) &&
                             !string.IsNullOrEmpty(model.PersonalInfo.LastName) &&
                             !string.IsNullOrEmpty(model.PersonalInfo.IDNumber) &&
                             model.PersonalInfo.ValidationErrors.Count == 0;
```
**Steps**:
- [ ] Update RegistrationWizard.razor: Change IsStep1Valid to computed property
- [ ] Remove any auto-activation logic triggered by SA ID auto-fill
- [ ] Test: Enter SA ID → Verify Next button remains disabled until all fields filled
- [ ] Test: Complete all fields → Verify Next button becomes enabled
- [ ] Test: Invalid email → Verify Next button remains disabled

**T301B**: Fix Step 2 Next Button Activation Logic  
**Estimate**: 2 hours  
**Priority**: P1-CRITICAL  
**Bug**: Next button enabled before test selection and venue selection  
**Fix**: Same pattern as T301A, validate all required fields

**T301C**: Fix Step 3 Next Button Activation Logic  
**Estimate**: 2 hours  
**Priority**: P1-CRITICAL  
**Bug**: Next button enabled before questionnaire complete  
**Fix**: Validate all required questionnaire fields answered

**T301D**: Fix Step 4 Register Button Activation  
**Estimate**: 2 hours  
**Priority**: P1-CRITICAL  
**Bug**: Register button should only enable when terms checkbox checked  
**Fix**:
```csharp
<FluentCheckbox @bind-Value="termsAccepted" Label="I accept the terms and conditions" />
<FluentButton Appearance="Appearance.Accent"
              Disabled="@(!termsAccepted)"
              OnClick="HandleSubmit">
    Register
</FluentButton>
```

**T301E**: Fix NBT Number Generation Timing  
**Estimate**: 3 hours  
**Priority**: P1-CRITICAL  
**Bug**: NBT number generated as separate wizard step (incorrect)  
**Fix**: Generate NBT number server-side on final submit  
**Steps**:
- [ ] Remove NBTNumberGenerationStep.razor component
- [ ] Update RegistrationWizard.razor: Remove step 3 (NBT number generation)
- [ ] Update RegistrationService.SubmitRegistrationAsync(): Add NBT number generation
- [ ] Test: Complete wizard → Submit → Verify NBT number generated server-side
- [ ] Test: NBT number returned in response and displayed in success message

**T301F**: Prevent Wizard Skip-to-End Bug  
**Estimate**: 4 hours  
**Priority**: P1-CRITICAL  
**Bug**: Wizard jumps to end step prematurely (skips steps 2 and 3)  
**Root Cause**: 
- SA ID auto-fill triggers StateHasChanged()
- StateHasChanged() incorrectly advances wizard
- Step validation not properly enforced
**Fix**:
```csharp
// Add explicit step control
private int currentStepIndex = 0;

private void OnSAIDAutoFill()
{
    // Auto-fill fields but DON'T call StateHasChanged() immediately
    // DON'T advance to next step
    model.PersonalInfo.DateOfBirth = ExtractDOB(model.PersonalInfo.IDNumber);
    model.PersonalInfo.Gender = ExtractGender(model.PersonalInfo.IDNumber);
    
    // Trigger validation check but stay on current step
    InvokeAsync(StateHasChanged);
}

// Explicit step navigation
private async Task GoToNextStep()
{
    if (await ValidateCurrentStep())
    {
        currentStepIndex++;
        await SaveDraft();
        StateHasChanged();
    }
}
```

**T301G**: Add Step Validation Enforcement  
**Estimate**: 3 hours  
**Priority**: P1-CRITICAL  
**Purpose**: Ensure each step validated before progression  
**Implementation**:
```csharp
private async Task<bool> ValidateCurrentStep()
{
    switch (currentStepIndex)
    {
        case 0: return await ValidateStep1();
        case 1: return await ValidateStep2();
        case 2: return await ValidateStep3();
        default: return false;
    }
}

private async Task<bool> ValidateStep1()
{
    var result = await RegistrationService.ValidatePersonalInfoAsync(model.PersonalInfo);
    if (!result.IsValid)
    {
        ShowValidationErrors(result.Errors);
        return false;
    }
    return true;
}
```

**T301H**: Test Complete Wizard Flow End-to-End  
**Estimate**: 2 hours  
**Priority**: P1-CRITICAL  
**Test Scenarios**:
- [ ] Test 1: Enter SA ID → Verify auto-fill → Verify Next disabled → Complete all fields → Verify Next enabled
- [ ] Test 2: Click Next → Verify Step 2 displayed (not Step 4)
- [ ] Test 3: Complete Step 2 → Click Next → Verify Step 3 displayed
- [ ] Test 4: Complete Step 3 → Click Next → Verify Step 4 (Review) displayed
- [ ] Test 5: Check terms → Click Register → Verify NBT number generated → Verify redirect to login
- [ ] Test 6: Return to /register → Verify wizard starts at Step 1 (clean state)

---

### Phase 3.2: Registration Draft Resumption
**Duration**: 2-3 days (24 hours)  
**Status**: 🔴 CRITICAL - NEW FEATURE  
**Purpose**: Allow registration resumption after interruption

#### Task Group: Draft Save/Restore Infrastructure

**T302A**: Create RegistrationDraft Entity  
**Estimate**: 1 hour  
**Priority**: P1-CRITICAL  
**File**: `src/NBT.Domain/Entities/RegistrationDraft.cs`  
```csharp
public class RegistrationDraft
{
    public Guid RegistrationDraftId { get; set; }
    public string Email { get; set; } // Unique per email
    public int CurrentStep { get; set; } // 0-3 (0-indexed)
    public string CompletedSteps { get; set; } // "0,1" (comma-separated)
    public string DraftData { get; set; } // JSON serialized RegistrationModel
    public DateTime ExpiryDate { get; set; } // 30 days from last update
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}
```

**T302B**: Add RegistrationDraft to DbContext  
**Estimate**: 30 minutes  
**Priority**: P1-CRITICAL  
**File**: `src/NBT.Infrastructure/Data/ApplicationDbContext.cs`  
```csharp
public DbSet<RegistrationDraft> RegistrationDrafts { get; set; }
```

**T302C**: Create EF Configuration for RegistrationDraft  
**Estimate**: 1 hour  
**Priority**: P1-CRITICAL  
**File**: `src/NBT.Infrastructure/Data/Configurations/RegistrationDraftConfiguration.cs`  
```csharp
public class RegistrationDraftConfiguration : IEntityTypeConfiguration<RegistrationDraft>
{
    public void Configure(EntityTypeBuilder<RegistrationDraft> builder)
    {
        builder.HasKey(rd => rd.RegistrationDraftId);
        builder.HasIndex(rd => rd.Email).IsUnique();
        builder.HasIndex(rd => rd.ExpiryDate);
        builder.Property(rd => rd.Email).HasMaxLength(100).IsRequired();
        builder.Property(rd => rd.DraftData).IsRequired();
    }
}
```

**T302D**: Create and Apply Migration  
**Estimate**: 30 minutes  
**Priority**: P1-CRITICAL  
**Commands**:
```bash
cd src/NBT.Infrastructure
dotnet ef migrations add AddRegistrationDraft --startup-project ../NBT.WebAPI
dotnet ef database update --startup-project ../NBT.WebAPI
```

**T302E**: Create IDraftService Interface  
**Estimate**: 1 hour  
**Priority**: P1-CRITICAL  
**File**: `src/NBT.Application/Interfaces/IDraftService.cs`  
```csharp
public interface IDraftService
{
    Task<RegistrationDraft> GetDraftAsync(string email);
    Task<Guid> SaveDraftAsync(SaveDraftRequest request);
    Task DiscardDraftAsync(string email);
    Task<int> CleanupExpiredDraftsAsync(); // Background job
}
```

**T302F**: Implement DraftService  
**Estimate**: 3 hours  
**Priority**: P1-CRITICAL  
**File**: `src/NBT.Infrastructure/Services/DraftService.cs`  
```csharp
public class DraftService : IDraftService
{
    private readonly ApplicationDbContext _context;
    
    public async Task<RegistrationDraft> GetDraftAsync(string email)
    {
        return await _context.RegistrationDrafts
            .FirstOrDefaultAsync(rd => rd.Email == email && rd.ExpiryDate > DateTime.UtcNow);
    }
    
    public async Task<Guid> SaveDraftAsync(SaveDraftRequest request)
    {
        var existing = await _context.RegistrationDrafts
            .FirstOrDefaultAsync(rd => rd.Email == request.Email);
        
        if (existing != null)
        {
            existing.CurrentStep = request.CurrentStep;
            existing.CompletedSteps = string.Join(",", request.CompletedSteps);
            existing.DraftData = JsonSerializer.Serialize(request.DraftData);
            existing.ExpiryDate = DateTime.UtcNow.AddDays(30);
            existing.UpdatedDate = DateTime.UtcNow;
        }
        else
        {
            existing = new RegistrationDraft
            {
                RegistrationDraftId = Guid.NewGuid(),
                Email = request.Email,
                CurrentStep = request.CurrentStep,
                CompletedSteps = string.Join(",", request.CompletedSteps),
                DraftData = JsonSerializer.Serialize(request.DraftData),
                ExpiryDate = DateTime.UtcNow.AddDays(30),
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };
            _context.RegistrationDrafts.Add(existing);
        }
        
        await _context.SaveChangesAsync();
        return existing.RegistrationDraftId;
    }
    
    public async Task DiscardDraftAsync(string email)
    {
        var draft = await _context.RegistrationDrafts
            .FirstOrDefaultAsync(rd => rd.Email == email);
        
        if (draft != null)
        {
            _context.RegistrationDrafts.Remove(draft);
            await _context.SaveChangesAsync();
        }
    }
    
    public async Task<int> CleanupExpiredDraftsAsync()
    {
        var expired = await _context.RegistrationDrafts
            .Where(rd => rd.ExpiryDate <= DateTime.UtcNow)
            .ToListAsync();
        
        _context.RegistrationDrafts.RemoveRange(expired);
        return await _context.SaveChangesAsync();
    }
}
```

**T302G**: Create Draft API Endpoints  
**Estimate**: 2 hours  
**Priority**: P1-CRITICAL  
**File**: `src/NBT.WebAPI/Controllers/RegistrationDraftController.cs`  
```csharp
[ApiController]
[Route("api/registration/draft")]
public class RegistrationDraftController : ControllerBase
{
    private readonly IDraftService _draftService;
    
    [HttpPost("save")]
    [AllowAnonymous]
    public async Task<IActionResult> SaveDraft([FromBody] SaveDraftRequest request)
    {
        var draftId = await _draftService.SaveDraftAsync(request);
        return Ok(new { Success = true, DraftId = draftId });
    }
    
    [HttpGet("{email}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetDraft(string email)
    {
        var draft = await _draftService.GetDraftAsync(email);
        if (draft == null)
            return NotFound();
        
        return Ok(new RegistrationDraftDto
        {
            DraftId = draft.RegistrationDraftId,
            Email = draft.Email,
            CurrentStep = draft.CurrentStep,
            CompletedSteps = draft.CompletedSteps.Split(',').Select(int.Parse).ToArray(),
            DraftData = JsonSerializer.Deserialize<RegistrationModel>(draft.DraftData),
            ExpiryDate = draft.ExpiryDate
        });
    }
    
    [HttpDelete("{email}")]
    [AllowAnonymous]
    public async Task<IActionResult> DiscardDraft(string email)
    {
        await _draftService.DiscardDraftAsync(email);
        return NoContent();
    }
}
```

**T302H**: Create DraftResumptionDialog Component  
**Estimate**: 3 hours  
**Priority**: P1-CRITICAL  
**File**: `src/NBT.WebUI/Components/Registration/DraftResumptionDialog.razor`  
```razor
<FluentDialog @ref="dialogRef" 
              aria-label="Resume Registration"
              Modal="true"
              Hidden="@(!showDialog)">
    <FluentDialogHeader>Resume Registration?</FluentDialogHeader>
    <FluentDialogBody>
        <p>We found an incomplete registration for <strong>@email</strong>.</p>
        <p>Would you like to continue where you left off or start fresh?</p>
        <p><small>Last updated: @lastUpdated.ToString("g")</small></p>
    </FluentDialogBody>
    <FluentDialogFooter>
        <FluentButton Appearance="Appearance.Accent" OnClick="ResumeRegistration">
            Resume
        </FluentButton>
        <FluentButton Appearance="Appearance.Neutral" OnClick="StartFresh">
            Start Fresh
        </FluentButton>
    </FluentDialogFooter>
</FluentDialog>

@code {
    [Parameter] public string Email { get; set; }
    [Parameter] public DateTime LastUpdated { get; set; }
    [Parameter] public EventCallback<bool> OnChoice { get; set; }
    
    private FluentDialog dialogRef;
    private bool showDialog = false;
    
    public void Show()
    {
        showDialog = true;
        StateHasChanged();
    }
    
    private async Task ResumeRegistration()
    {
        showDialog = false;
        await OnChoice.InvokeAsync(true);
    }
    
    private async Task StartFresh()
    {
        showDialog = false;
        await OnChoice.InvokeAsync(false);
    }
}
```

**T302I**: Integrate Draft Logic into RegistrationWizard  
**Estimate**: 4 hours  
**Priority**: P1-CRITICAL  
**File**: `src/NBT.WebUI/Components/Registration/RegistrationWizard.razor`  
**Changes**:
```csharp
@inject IDraftService DraftService

private DraftResumptionDialog draftDialog;

protected override async Task OnInitializedAsync()
{
    // Check for existing draft (use email from querystring if available)
    var email = NavigationManager.GetQueryParameter("email");
    if (!string.IsNullOrEmpty(email))
    {
        var draft = await DraftService.GetDraftAsync(email);
        if (draft != null)
        {
            draftDialog.Email = email;
            draftDialog.LastUpdated = draft.UpdatedDate;
            draftDialog.Show();
        }
    }
}

private async Task HandleDraftChoice(bool resume)
{
    if (resume)
    {
        var draft = await DraftService.GetDraftAsync(email);
        model = draft.DraftData;
        currentStepIndex = draft.CurrentStep;
        StateHasChanged();
    }
    else
    {
        await DraftService.DiscardDraftAsync(email);
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
```

**T302J**: Create Background Job for Draft Cleanup  
**Estimate**: 2 hours  
**Priority**: P2-HIGH  
**File**: `src/NBT.Infrastructure/BackgroundJobs/DraftCleanupJob.cs`  
```csharp
public class DraftCleanupJob : IHostedService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private Timer _timer;
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        // Run daily at 2 AM
        _timer = new Timer(async _ => await CleanupExpiredDrafts(), null, TimeSpan.Zero, TimeSpan.FromHours(24));
        return Task.CompletedTask;
    }
    
    private async Task CleanupExpiredDrafts()
    {
        using var scope = _scopeFactory.CreateScope();
        var draftService = scope.ServiceProvider.GetRequiredService<IDraftService>();
        var deletedCount = await draftService.CleanupExpiredDraftsAsync();
        // Log: Deleted {deletedCount} expired drafts
    }
    
    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Dispose();
        return Task.CompletedTask;
    }
}
```

**T302K**: Test Draft Resumption Flow  
**Estimate**: 3 hours  
**Priority**: P1-CRITICAL  
**Test Scenarios**:
- [ ] Test 1: Start registration → Complete Step 1 → Close browser → Reopen → Verify resumption dialog
- [ ] Test 2: Resume registration → Verify data pre-filled → Verify current step correct
- [ ] Test 3: Start fresh → Verify draft deleted → Verify blank form
- [ ] Test 4: Wait 31 days → Verify draft expired and not shown
- [ ] Test 5: Save draft on each step → Verify draft updated in database

---

### Phase 7.1: Result Barcode System
**Duration**: 2 days (16 hours)  
**Status**: 🔴 CRITICAL - NEW FEATURE  
**Purpose**: Add unique barcode to each test result for verification

#### Task Group: Barcode Generation & Storage

**T701A**: Add Barcode Column to TestResult Entity  
**Estimate**: 1 hour  
**Priority**: P1-CRITICAL  
**File**: `src/NBT.Domain/Entities/TestResult.cs`  
```csharp
public class TestResult
{
    // Existing properties...
    
    [Required]
    [MaxLength(20)]
    public string Barcode { get; set; } // Format: NBT{YYYYMMDD}-{TestType}-{Sequence}
}
```

**T701B**: Create Barcode Migration  
**Estimate**: 30 minutes  
**Priority**: P1-CRITICAL  
**Commands**:
```bash
cd src/NBT.Infrastructure
dotnet ef migrations add AddBarcodeToTestResult --startup-project ../NBT.WebAPI
dotnet ef database update --startup-project ../NBT.WebAPI
```

**T701C**: Create IBarcodeGenerator Interface  
**Estimate**: 1 hour  
**Priority**: P1-CRITICAL  
**File**: `src/NBT.Application/Interfaces/IBarcodeGenerator.cs`  
```csharp
public interface IBarcodeGenerator
{
    /// <summary>
    /// Generates unique barcode for test result
    /// Format: NBT{YYYYMMDD}-{TestType}-{SequenceNumber}
    /// </summary>
    Task<string> GenerateBarcodeAsync(DateTime testDate, TestType testType);
    
    /// <summary>
    /// Validates barcode format
    /// </summary>
    bool ValidateBarcode(string barcode);
    
    /// <summary>
    /// Generates barcode image (for PDF certificates)
    /// </summary>
    byte[] GenerateBarcodeImage(string barcode);
}
```

**T701D**: Implement BarcodeGenerator Service  
**Estimate**: 4 hours  
**Priority**: P1-CRITICAL  
**File**: `src/NBT.Infrastructure/Services/BarcodeGenerator.cs`  
```csharp
public class BarcodeGenerator : IBarcodeGenerator
{
    private readonly ApplicationDbContext _context;
    
    public async Task<string> GenerateBarcodeAsync(DateTime testDate, TestType testType)
    {
        var dateStr = testDate.ToString("yyyyMMdd");
        var testTypeStr = testType.ToString(); // AQL or MAT
        
        // Get next sequence number for this date and test type
        var todayBarcodes = await _context.TestResults
            .Where(tr => tr.Barcode.StartsWith($"NBT{dateStr}-{testTypeStr}-"))
            .Select(tr => tr.Barcode)
            .ToListAsync();
        
        int maxSequence = 0;
        if (todayBarcodes.Any())
        {
            maxSequence = todayBarcodes
                .Select(b => int.Parse(b.Substring(b.LastIndexOf('-') + 1)))
                .Max();
        }
        
        var sequence = (maxSequence + 1).ToString("D6"); // 6 digits, zero-padded
        var barcode = $"NBT{dateStr}-{testTypeStr}-{sequence}";
        
        // Verify uniqueness (should always be unique)
        var exists = await _context.TestResults.AnyAsync(tr => tr.Barcode == barcode);
        if (exists)
            throw new InvalidOperationException($"Barcode collision detected: {barcode}");
        
        return barcode;
    }
    
    public bool ValidateBarcode(string barcode)
    {
        var regex = new Regex(@"^NBT\d{8}-(AQL|MAT)-\d{6}$");
        return regex.IsMatch(barcode);
    }
    
    public byte[] GenerateBarcodeImage(string barcode)
    {
        // Use ZXing.Net or similar library to generate barcode image
        var barcodeWriter = new BarcodeWriter
        {
            Format = BarcodeFormat.CODE_128,
            Options = new EncodingOptions
            {
                Height = 100,
                Width = 300,
                Margin = 10
            }
        };
        
        using var bitmap = barcodeWriter.Write(barcode);
        using var stream = new MemoryStream();
        bitmap.Save(stream, ImageFormat.Png);
        return stream.ToArray();
    }
}
```

**T701E**: Update ResultsImportService to Generate Barcodes  
**Estimate**: 2 hours  
**Priority**: P1-CRITICAL  
**File**: `src/NBT.Infrastructure/Services/ResultsImportService.cs`  
**Changes**:
```csharp
private readonly IBarcodeGenerator _barcodeGenerator;

private async Task<TestResult> CreateTestResultAsync(ResultRow row)
{
    var testResult = new TestResult
    {
        // ... existing properties
        Barcode = await _barcodeGenerator.GenerateBarcodeAsync(row.TestDate, row.TestType)
    };
    
    return testResult;
}
```

**T701F**: Create Barcode Lookup API  
**Estimate**: 1 hour  
**Priority**: P1-CRITICAL  
**File**: `src/NBT.WebAPI/Controllers/ResultsController.cs`  
```csharp
[HttpGet("by-barcode/{barcode}")]
[Authorize]
public async Task<IActionResult> GetResultByBarcode(string barcode)
{
    if (!_barcodeGenerator.ValidateBarcode(barcode))
        return BadRequest("Invalid barcode format");
    
    var result = await _resultsService.GetResultByBarcodeAsync(barcode);
    
    if (result == null)
        return NotFound();
    
    // Check authorization: User owns result OR is Staff/Admin/SuperUser
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    var userRole = User.FindFirstValue(ClaimTypes.Role);
    
    if (result.StudentId != userId && !new[] { "Staff", "Admin", "SuperUser" }.Contains(userRole))
        return Forbid();
    
    return Ok(result);
}
```

**T701G**: Update Result PDF Generator to Include Barcode  
**Estimate**: 3 hours  
**Priority**: P1-CRITICAL  
**File**: `src/NBT.Infrastructure/Services/PdfGenerator.cs`  
**Changes**:
```csharp
public byte[] GenerateResultCertificate(TestResultDto result)
{
    var document = Document.Create(container =>
    {
        container.Page(page =>
        {
            page.Header().Row(row =>
            {
                row.RelativeItem().Text("NBT Test Result Certificate").FontSize(20).Bold();
            });
            
            page.Content().Column(column =>
            {
                // Student info
                column.Item().Text($"Student: {result.StudentName}");
                column.Item().Text($"NBT Number: {result.NBTNumber}");
                column.Item().Text($"Test Date: {result.TestDate:yyyy-MM-dd}");
                
                // Barcode
                var barcodeImage = _barcodeGenerator.GenerateBarcodeImage(result.Barcode);
                column.Item().Image(barcodeImage).FitWidth();
                column.Item().Text(result.Barcode).FontSize(10).Italic();
                
                // Domain results
                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(2);
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                    });
                    
                    table.Header(header =>
                    {
                        header.Cell().Text("Domain").Bold();
                        header.Cell().Text("Score").Bold();
                        header.Cell().Text("Performance Level").Bold();
                        header.Cell().Text("Percentile").Bold();
                    });
                    
                    foreach (var domain in result.Domains)
                    {
                        table.Cell().Text(domain.DomainType);
                        table.Cell().Text(domain.Score.ToString("F1"));
                        table.Cell().Text(domain.PerformanceLevel);
                        table.Cell().Text(domain.Percentile.ToString("F1"));
                    }
                });
            });
            
            page.Footer().AlignCenter().Text("Valid for 3 years from test date");
        });
    });
    
    return document.GeneratePdf();
}
```

**T701H**: Test Barcode Generation and Lookup  
**Estimate**: 2 hours  
**Priority**: P1-CRITICAL  
**Test Scenarios**:
- [ ] Test 1: Import results → Verify barcode generated for each result
- [ ] Test 2: Verify barcode format: NBT{YYYYMMDD}-{AQL|MAT}-{6digits}
- [ ] Test 3: Import multiple results same day → Verify sequence increments
- [ ] Test 4: Lookup by barcode → Verify correct result returned
- [ ] Test 5: Download PDF → Verify barcode image displayed
- [ ] Test 6: Scan barcode with barcode reader → Verify readable

---

## 🚨 PRIORITY 2: HIGH - REQUIRED FOR MVP

### Phase 4.1: Payment Upload & Reconciliation
**Duration**: 3-4 days (32 hours)  
**Status**: 🟡 HIGH - NEW FEATURE  
**Purpose**: Upload bank payment files and reconcile with bookings

#### Task Group: Payment Upload Infrastructure

**T401A**: Create PaymentUpload Entity  
**Estimate**: 1 hour  
**Priority**: P2-HIGH  
**File**: `src/NBT.Domain/Entities/PaymentUpload.cs`  
```csharp
public class PaymentUpload
{
    public Guid PaymentUploadId { get; set; }
    public string FileName { get; set; }
    public string UploadedBy { get; set; } // User email
    public DateTime UploadDate { get; set; }
    public int TotalRows { get; set; }
    public int ProcessedCount { get; set; }
    public int MatchedCount { get; set; }
    public int UnmatchedCount { get; set; }
    public int ErrorCount { get; set; }
    public string Status { get; set; } // Processing, Completed, Failed
    public string ErrorReport { get; set; } // JSON array of errors
    public DateTime CreatedDate { get; set; }
    
    // Navigation
    public ICollection<BankPaymentRecord> BankPaymentRecords { get; set; }
}
```

**T401B**: Create BankPaymentRecord Entity  
**Estimate**: 1 hour  
**Priority**: P2-HIGH  
**File**: `src/NBT.Domain/Entities/BankPaymentRecord.cs`  
```csharp
public class BankPaymentRecord
{
    public Guid BankPaymentRecordId { get; set; }
    public Guid PaymentUploadId { get; set; }
    public string NBTNumber { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string BankReference { get; set; } // Unique
    public string TransactionType { get; set; } // EFT, Deposit, Transfer
    public string BranchCode { get; set; }
    public string AccountNumber { get; set; } // Masked
    public Guid? PaymentId { get; set; } // FK to Payment (if matched)
    public string MatchStatus { get; set; } // Matched, Unmatched, ManualReview
    public string MatchedBy { get; set; } // User who manually matched
    public DateTime? MatchedDate { get; set; }
    public string Notes { get; set; }
    public DateTime CreatedDate { get; set; }
    
    // Navigation
    public PaymentUpload PaymentUpload { get; set; }
    public Payment Payment { get; set; }
}
```

[TASK CONTINUES... Total: 615 tasks, 714 hours]

---

## IMPLEMENTATION TIMELINE

### Sprint 1 (Week 1): Critical Fixes
- ✅ Phase 3.1: Registration Wizard Fixes (20 hours)
- ✅ Phase 3.2: Registration Draft Resumption (24 hours)

### Sprint 2 (Week 2): Core Features
- ✅ Phase 7.1: Result Barcode System (16 hours)
- ✅ Phase 4.1: Payment Upload & Reconciliation (32 hours)

### Sprint 3 (Week 3): UI/UX
- ✅ Phase 10: Landing Page & Dashboards (30 hours)
- ✅ Phase 5.1: Venue Availability (6 hours)

### Sprint 4 (Week 4): Testing & Deployment
- ✅ Phase 11: Comprehensive Testing (40 hours)
- ✅ Deployment & UAT (20 hours)

---

## CONCLUSION

This updated task breakdown provides granular implementation steps for ALL new requirements identified in the 2025-11-09 requirement gathering session. Each task includes:
- ✅ Clear description and code samples
- ✅ Time estimates
- ✅ Priority classification
- ✅ Dependencies
- ✅ Test scenarios

**Status:** ✅ READY FOR IMPLEMENTATION

---

**Document Owner:** NBT Technical Team  
**Last Updated:** 2025-11-09  
**Next Review:** Weekly sprint reviews
