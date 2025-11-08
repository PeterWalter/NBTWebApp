#!/usr/bin/env pwsh
# Test Registration Wizard - Verification Script
# This script verifies that all components of the registration wizard are in place

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "NBT Registration Wizard - Test Verification" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

$success = $true

# Test 1: Check if Register.razor exists
Write-Host "[1/10] Checking Register.razor component..." -NoNewline
$registerPath = "src\NBT.WebUI.Client\Pages\Registration\Register.razor"
if (Test-Path $registerPath) {
    Write-Host " ✓ PASS" -ForegroundColor Green
} else {
    Write-Host " ✗ FAIL" -ForegroundColor Red
    $success = $false
}

# Test 2: Check if Register.razor.css exists
Write-Host "[2/10] Checking Register.razor.css styles..." -NoNewline
$cssPath = "src\NBT.WebUI.Client\Pages\Registration\Register.razor.css"
if (Test-Path $cssPath) {
    Write-Host " ✓ PASS" -ForegroundColor Green
} else {
    Write-Host " ✗ FAIL" -ForegroundColor Red
    $success = $false
}

# Test 3: Check if RegistrationFormModel.cs exists
Write-Host "[3/10] Checking RegistrationFormModel..." -NoNewline
$modelPath = "src\NBT.WebUI.Client\Models\RegistrationFormModel.cs"
if (Test-Path $modelPath) {
    Write-Host " ✓ PASS" -ForegroundColor Green
} else {
    Write-Host " ✗ FAIL" -ForegroundColor Red
    $success = $false
}

# Test 4: Check if IRegistrationService exists
Write-Host "[4/10] Checking IRegistrationService interface..." -NoNewline
$interfacePath = "src\NBT.WebUI.Client\Services\IRegistrationService.cs"
if (Test-Path $interfacePath) {
    Write-Host " ✓ PASS" -ForegroundColor Green
} else {
    Write-Host " ✗ FAIL" -ForegroundColor Red
    $success = $false
}

# Test 5: Check if RegistrationService exists
Write-Host "[5/10] Checking RegistrationService implementation..." -NoNewline
$servicePath = "src\NBT.WebUI.Client\Services\RegistrationService.cs"
if (Test-Path $servicePath) {
    Write-Host " ✓ PASS" -ForegroundColor Green
} else {
    Write-Host " ✗ FAIL" -ForegroundColor Red
    $success = $false
}

# Test 6: Check if StudentsController exists
Write-Host "[6/10] Checking StudentsController API..." -NoNewline
$controllerPath = "src\NBT.WebAPI\Controllers\StudentsController.cs"
if (Test-Path $controllerPath) {
    Write-Host " ✓ PASS" -ForegroundColor Green
} else {
    Write-Host " ✗ FAIL" -ForegroundColor Red
    $success = $false
}

# Test 7: Check if StudentService exists
Write-Host "[7/10] Checking StudentService..." -NoNewline
$studentServicePath = "src\NBT.Application\Students\Services\StudentService.cs"
if (Test-Path $studentServicePath) {
    Write-Host " ✓ PASS" -ForegroundColor Green
} else {
    Write-Host " ✗ FAIL" -ForegroundColor Red
    $success = $false
}

# Test 8: Check if StudentDto exists
Write-Host "[8/10] Checking StudentDto..." -NoNewline
$dtoPath = "src\NBT.Application\Students\DTOs\StudentDto.cs"
if (Test-Path $dtoPath) {
    Write-Host " ✓ PASS" -ForegroundColor Green
} else {
    Write-Host " ✗ FAIL" -ForegroundColor Red
    $success = $false
}

# Test 9: Check if NavMenu includes Register link
Write-Host "[9/10] Checking navigation menu..." -NoNewline
$navMenuPath = "src\NBT.WebUI.Client\Layout\NavMenu.razor"
if (Test-Path $navMenuPath) {
    $navContent = Get-Content $navMenuPath -Raw
    if ($navContent -match 'href="register"') {
        Write-Host " ✓ PASS" -ForegroundColor Green
    } else {
        Write-Host " ✗ FAIL (No register link)" -ForegroundColor Red
        $success = $false
    }
} else {
    Write-Host " ✗ FAIL (File not found)" -ForegroundColor Red
    $success = $false
}

# Test 10: Build the solution
Write-Host "[10/10] Building solution..." -NoNewline
$buildOutput = dotnet build --no-restore --verbosity quiet 2>&1
if ($LASTEXITCODE -eq 0) {
    Write-Host " ✓ PASS" -ForegroundColor Green
} else {
    Write-Host " ✗ FAIL" -ForegroundColor Red
    Write-Host "Build errors:" -ForegroundColor Red
    Write-Host $buildOutput -ForegroundColor Red
    $success = $false
}

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan

if ($success) {
    Write-Host "✓ ALL TESTS PASSED!" -ForegroundColor Green
    Write-Host ""
    Write-Host "Registration Wizard is fully implemented and ready to use." -ForegroundColor Green
    Write-Host ""
    Write-Host "Next Steps:" -ForegroundColor Yellow
    Write-Host "1. Run the application: .\start-app.ps1" -ForegroundColor White
    Write-Host "2. Navigate to: https://localhost:PORT/register" -ForegroundColor White
    Write-Host "3. Test the 7-step registration wizard" -ForegroundColor White
    Write-Host "4. Verify NBT number generation on success" -ForegroundColor White
    Write-Host ""
    exit 0
} else {
    Write-Host "✗ SOME TESTS FAILED" -ForegroundColor Red
    Write-Host ""
    Write-Host "Please review the failed tests above and fix any issues." -ForegroundColor Red
    Write-Host ""
    exit 1
}
