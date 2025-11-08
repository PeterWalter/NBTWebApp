# Apply JSON Serialization Fix - NBT Web Application
# This script applies the critical JSON property serialization fix

Write-Host "=====================================" -ForegroundColor Cyan
Write-Host "NBT JSON Serialization Fix Script" -ForegroundColor Cyan
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host ""

$ErrorActionPreference = "Stop"
$projectRoot = $PSScriptRoot

# Step 1: Check if WebAPI Program.cs needs updating
Write-Host "[1/5] Checking WebAPI Program.cs..." -ForegroundColor Yellow

$webApiProgramPath = Join-Path $projectRoot "src\NBT.WebAPI\Program.cs"

if (Test-Path $webApiProgramPath) {
    $content = Get-Content $webApiProgramPath -Raw
    
    if ($content -match "JsonNamingPolicy\.CamelCase") {
        Write-Host "  ✓ WebAPI JSON configuration already applied" -ForegroundColor Green
    } else {
        Write-Host "  ⚠ WebAPI needs JSON configuration update" -ForegroundColor Red
        Write-Host "  → Add JSON options to AddControllers() in Program.cs" -ForegroundColor Cyan
        Write-Host ""
        Write-Host "  Add this code after builder.Services.AddControllers():" -ForegroundColor Yellow
        Write-Host @"
  
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented = builder.Environment.IsDevelopment();
});
"@ -ForegroundColor Gray
    }
} else {
    Write-Host "  ✗ WebAPI Program.cs not found at expected location" -ForegroundColor Red
}

Write-Host ""

# Step 2: Check if WebUI Program.cs needs updating
Write-Host "[2/5] Checking WebUI Program.cs..." -ForegroundColor Yellow

$webUiProgramPath = Join-Path $projectRoot "src\NBT.WebUI\Program.cs"

if (Test-Path $webUiProgramPath) {
    $content = Get-Content $webUiProgramPath -Raw
    
    if ($content -match "PropertyNamingPolicy.*CamelCase") {
        Write-Host "  ✓ WebUI JSON configuration already applied" -ForegroundColor Green
    } else {
        Write-Host "  ⚠ WebUI needs JSON configuration update" -ForegroundColor Red
        Write-Host "  → Configure HttpClient JSON options in Program.cs" -ForegroundColor Cyan
    }
} else {
    Write-Host "  ✗ WebUI Program.cs not found at expected location" -ForegroundColor Red
}

Write-Host ""

# Step 3: List DTOs that need [JsonPropertyName] attributes
Write-Host "[3/5] Scanning for DTOs..." -ForegroundColor Yellow

$applicationPath = Join-Path $projectRoot "src\NBT.Application"

if (Test-Path $applicationPath) {
    $dtoFiles = Get-ChildItem -Path $applicationPath -Filter "*Dto.cs" -Recurse
    $requestFiles = Get-ChildItem -Path $applicationPath -Filter "*Request.cs" -Recurse
    $responseFiles = Get-ChildItem -Path $applicationPath -Filter "*Response.cs" -Recurse
    
    $allDtos = $dtoFiles + $requestFiles + $responseFiles
    
    Write-Host "  Found $($allDtos.Count) DTO files" -ForegroundColor Cyan
    
    $needsUpdate = @()
    
    foreach ($dto in $allDtos) {
        $content = Get-Content $dto.FullName -Raw
        
        # Check if file contains properties but no JsonPropertyName attribute
        if ($content -match "public\s+\w+\s+\w+\s*{\s*get" -and $content -notmatch "\[JsonPropertyName\(") {
            $needsUpdate += $dto.Name
        }
    }
    
    if ($needsUpdate.Count -eq 0) {
        Write-Host "  ✓ All DTOs have [JsonPropertyName] attributes" -ForegroundColor Green
    } else {
        Write-Host "  ⚠ $($needsUpdate.Count) DTOs need [JsonPropertyName] attributes:" -ForegroundColor Red
        foreach ($file in $needsUpdate) {
            Write-Host "    - $file" -ForegroundColor Gray
        }
    }
} else {
    Write-Host "  ✗ Application layer not found" -ForegroundColor Red
}

Write-Host ""

# Step 4: Check for System.Text.Json usings
Write-Host "[4/5] Checking for required using statements..." -ForegroundColor Yellow

$missingUsings = @()

foreach ($dto in $allDtos) {
    $content = Get-Content $dto.FullName -Raw
    
    if ($content -notmatch "using System\.Text\.Json\.Serialization;") {
        $missingUsings += $dto.Name
    }
}

if ($missingUsings.Count -eq 0) {
    Write-Host "  ✓ All DTOs have required using statements" -ForegroundColor Green
} else {
    Write-Host "  ⚠ $($missingUsings.Count) DTOs missing 'using System.Text.Json.Serialization;'" -ForegroundColor Red
}

Write-Host ""

# Step 5: Generate sample DTO with correct attributes
Write-Host "[5/5] Generating sample DTO template..." -ForegroundColor Yellow

$sampleDto = @"
using System;
using System.Text.Json.Serialization;

namespace NBT.Application.DTOs;

/// <summary>
/// Sample DTO with correct JSON serialization attributes
/// </summary>
public class SampleDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;
    
    [JsonPropertyName("createdDate")]
    public DateTime CreatedDate { get; set; }
    
    [JsonPropertyName("isActive")]
    public bool IsActive { get; set; }
    
    [JsonPropertyName("status")]
    public StatusEnum Status { get; set; }
    
    [JsonPropertyName("count")]
    public int? Count { get; set; }
}

public enum StatusEnum
{
    Pending = 0,
    Active = 1,
    Completed = 2
}
"@

$samplePath = Join-Path $projectRoot "specs\002-nbt-integrated-system\sample-dto-template.cs"
$sampleDto | Out-File -FilePath $samplePath -Encoding UTF8

Write-Host "  ✓ Sample template created: specs\002-nbt-integrated-system\sample-dto-template.cs" -ForegroundColor Green

Write-Host ""
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host "Summary" -ForegroundColor Cyan
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Next Steps:" -ForegroundColor Yellow
Write-Host "1. Update WebAPI Program.cs with JSON configuration" -ForegroundColor White
Write-Host "2. Update WebUI Program.cs with HttpClient JSON configuration" -ForegroundColor White
Write-Host "3. Add [JsonPropertyName] attributes to all DTOs" -ForegroundColor White
Write-Host "4. Add 'using System.Text.Json.Serialization;' to DTO files" -ForegroundColor White
Write-Host "5. Test all API endpoints after changes" -ForegroundColor White
Write-Host ""
Write-Host "For detailed instructions, see:" -ForegroundColor Cyan
Write-Host "  specs\002-nbt-integrated-system\CRITICAL-UPDATES.md" -ForegroundColor Gray
Write-Host ""
Write-Host "✓ Script completed" -ForegroundColor Green
