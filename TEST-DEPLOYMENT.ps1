# NBT Web Application - Deployment Test Script
# This script verifies that the application is running and accessible

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "NBT Web Application - Deployment Test" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Test API Health
Write-Host "Testing API endpoint..." -ForegroundColor Yellow
try {
    $apiResponse = Invoke-WebRequest -Uri "https://localhost:7001/api/health" -SkipCertificateCheck -ErrorAction Stop
    if ($apiResponse.StatusCode -eq 200) {
        Write-Host "✅ API is running at https://localhost:7001" -ForegroundColor Green
    }
} catch {
    Write-Host "⚠️  API health check failed: $_" -ForegroundColor Red
    Write-Host "   This is normal if the API doesn't have a health endpoint yet" -ForegroundColor Yellow
}

Write-Host ""

# Test Web UI
Write-Host "Testing Web UI..." -ForegroundColor Yellow
try {
    $uiResponse = Invoke-WebRequest -Uri "https://localhost:5001" -SkipCertificateCheck -ErrorAction Stop
    if ($uiResponse.StatusCode -eq 200) {
        Write-Host "✅ Web UI is running at https://localhost:5001" -ForegroundColor Green
    }
} catch {
    Write-Host "⚠️  Web UI check failed: $_" -ForegroundColor Red
}

Write-Host ""

# Check if processes are running
Write-Host "Checking running processes..." -ForegroundColor Yellow
$dotnetProcesses = Get-Process dotnet -ErrorAction SilentlyContinue
if ($dotnetProcesses) {
    Write-Host "✅ Found $($dotnetProcesses.Count) dotnet process(es) running" -ForegroundColor Green
} else {
    Write-Host "❌ No dotnet processes found" -ForegroundColor Red
}

Write-Host ""

# Summary
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Test Summary" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Access the application at:" -ForegroundColor White
Write-Host "  🌐 Web UI:  https://localhost:5001" -ForegroundColor Cyan
Write-Host "  🔌 API:     https://localhost:7001" -ForegroundColor Cyan
Write-Host "  📚 Swagger: https://localhost:7001/swagger" -ForegroundColor Cyan
Write-Host ""
Write-Host "Default Admin Credentials:" -ForegroundColor White
Write-Host "  📧 Email:    admin@nbt.ac.za" -ForegroundColor Cyan
Write-Host "  🔑 Password: Admin@123" -ForegroundColor Cyan
Write-Host ""
Write-Host "✅ Deployment test complete!" -ForegroundColor Green
Write-Host ""
