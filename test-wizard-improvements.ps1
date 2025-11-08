#!/usr/bin/env pwsh
# Test script for Registration Wizard Improvements

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Registration Wizard Improvements Test" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Test SA ID extraction logic
function Test-SAIDExtraction {
    param (
        [string]$SAID
    )
    
    Write-Host "Testing SA ID: $SAID" -ForegroundColor Yellow
    
    # Extract DOB
    $year = [int]$SAID.Substring(0, 2)
    $month = [int]$SAID.Substring(2, 2)
    $day = [int]$SAID.Substring(4, 2)
    
    $currentYearLastTwo = (Get-Date).Year % 100
    $century = if ($year -gt $currentYearLastTwo) { 1900 } else { 2000 }
    $fullYear = $century + $year
    
    $dob = Get-Date -Year $fullYear -Month $month -Day $day
    
    # Extract Gender
    $genderDigit = [int]$SAID.Substring(6, 1)
    $gender = if ($genderDigit -lt 5) { "Female" } else { "Male" }
    
    Write-Host "  DOB: $($dob.ToString('yyyy-MM-dd'))" -ForegroundColor Green
    Write-Host "  Gender: $gender" -ForegroundColor Green
    Write-Host ""
}

Write-Host "Testing SA ID Extraction Logic:" -ForegroundColor Cyan
Write-Host ""

# Test cases
Test-SAIDExtraction "9801015800089"  # Male born 1998-01-01
Test-SAIDExtraction "0512200234088"  # Female born 2005-12-20
Test-SAIDExtraction "7503035123456"  # Male born 1975-03-03
Test-SAIDExtraction "8906154321087"  # Female born 1989-06-15

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Wizard Structure Summary:" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Step 1: Personal & Contact Information" -ForegroundColor Green
Write-Host "  - ID Type & Number (with auto-extraction for SA IDs)"
Write-Host "  - Personal Details (Name, DOB, Gender, Ethnicity)"
Write-Host "  - Contact Info (Email, Phone)"
Write-Host ""
Write-Host "Step 2: Address Information" -ForegroundColor Green
Write-Host "  - Residential Address"
Write-Host ""
Write-Host "Step 3: Academic & Survey" -ForegroundColor Green
Write-Host "  - School Details"
Write-Host "  - Pre-Test Questionnaire"
Write-Host ""
Write-Host "Step 4: Special Accommodations" -ForegroundColor Green
Write-Host "  - Accommodation Requirements"
Write-Host ""
Write-Host "Step 5: Review & Submit" -ForegroundColor Green
Write-Host "  - Review All Information"
Write-Host "  - Submit Registration"
Write-Host ""

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "To Run the Application:" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "1. Start the Web API:" -ForegroundColor Yellow
Write-Host "   cd src\NBT.WebAPI"
Write-Host "   dotnet run"
Write-Host ""
Write-Host "2. Start the Blazor Web UI (in another terminal):" -ForegroundColor Yellow
Write-Host "   cd src\NBT.WebUI"
Write-Host "   dotnet run"
Write-Host ""
Write-Host "3. Navigate to:" -ForegroundColor Yellow
Write-Host "   https://localhost:5001/register"
Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Key Features:" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "✓ Combined steps (6 steps → 5 steps)" -ForegroundColor Green
Write-Host "✓ Automatic DOB extraction from SA ID" -ForegroundColor Green
Write-Host "✓ Automatic Gender extraction from SA ID" -ForegroundColor Green
Write-Host "✓ Disabled fields when auto-extracted" -ForegroundColor Green
Write-Host "✓ Removed Age field (not needed with DOB)" -ForegroundColor Green
Write-Host "✓ Survey data now sent to API" -ForegroundColor Green
Write-Host "✓ Ethnicity data now sent to API" -ForegroundColor Green
Write-Host "✓ Support for Foreign ID and Passport" -ForegroundColor Green
Write-Host ""
