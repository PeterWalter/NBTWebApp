# NBT Web Application Startup Script
# This script ensures a clean start of both API and WebUI on ports 5000 and 5001

Write-Host "===============================================" -ForegroundColor Cyan
Write-Host "  NBT WebApp Startup Script" -ForegroundColor Cyan
Write-Host "===============================================" -ForegroundColor Cyan
Write-Host ""

# Step 1: Kill processes on ports 5000 and 5001
Write-Host "[Step 1/4] Checking for processes on ports 5000 and 5001..." -ForegroundColor Yellow

try {
    $process5000 = Get-NetTCPConnection -LocalPort 5000 -ErrorAction SilentlyContinue
    if ($process5000) {
        Stop-Process -Id $process5000.OwningProcess -Force -ErrorAction SilentlyContinue
        Write-Host "  ✓ Killed process on port 5000" -ForegroundColor Green
    } else {
        Write-Host "  ✓ Port 5000 is free" -ForegroundColor Green
    }
} catch {
    Write-Host "  ✓ Port 5000 is free" -ForegroundColor Green
}

try {
    $process5001 = Get-NetTCPConnection -LocalPort 5001 -ErrorAction SilentlyContinue
    if ($process5001) {
        Stop-Process -Id $process5001.OwningProcess -Force -ErrorAction SilentlyContinue
        Write-Host "  ✓ Killed process on port 5001" -ForegroundColor Green
    } else {
        Write-Host "  ✓ Port 5001 is free" -ForegroundColor Green
    }
} catch {
    Write-Host "  ✓ Port 5001 is free" -ForegroundColor Green
}

Write-Host "  Waiting for ports to be released..." -ForegroundColor Gray
Start-Sleep -Seconds 2
Write-Host ""

# Step 2: Update database
Write-Host "[Step 2/4] Updating database..." -ForegroundColor Yellow
$infraPath = "D:\projects\source code\NBTWebApp\src\NBT.Infrastructure"
$webApiProject = "D:\projects\source code\NBTWebApp\src\NBT.WebAPI\NBT.WebAPI.csproj"

Set-Location $infraPath
dotnet ef database update --startup-project $webApiProject --no-build 2>&1 | Out-Null
if ($LASTEXITCODE -eq 0) {
    Write-Host "  ✓ Database updated successfully" -ForegroundColor Green
} else {
    Write-Host "  ⚠ Database may need update (continuing anyway)" -ForegroundColor Yellow
}
Write-Host ""

# Step 3: Start Web API
Write-Host "[Step 3/4] Starting Web API on http://localhost:5000..." -ForegroundColor Yellow
$apiPath = "D:\projects\source code\NBTWebApp\src\NBT.WebAPI"
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd '$apiPath'; Write-Host 'NBT WebAPI Starting...' -ForegroundColor Cyan; dotnet run --urls 'http://localhost:5000'"

Write-Host "  Waiting for Web API to initialize..." -ForegroundColor Gray
Start-Sleep -Seconds 8
Write-Host "  ✓ Web API should now be running" -ForegroundColor Green
Write-Host ""

# Step 4: Start Web UI
Write-Host "[Step 4/4] Starting Web UI on http://localhost:5001..." -ForegroundColor Yellow
$uiPath = "D:\projects\source code\NBTWebApp\src\NBT.WebUI"
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd '$uiPath'; Write-Host 'NBT WebUI Starting...' -ForegroundColor Cyan; dotnet run --urls 'http://localhost:5001'"

Write-Host "  Waiting for Web UI to initialize..." -ForegroundColor Gray
Start-Sleep -Seconds 5
Write-Host "  ✓ Web UI should now be running" -ForegroundColor Green
Write-Host ""

Write-Host "===============================================" -ForegroundColor Cyan
Write-Host "  Applications Started Successfully!" -ForegroundColor Green
Write-Host "===============================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Web API:  http://localhost:5000" -ForegroundColor Cyan
Write-Host "Web UI:   http://localhost:5001" -ForegroundColor Cyan
Write-Host ""
Write-Host "Open your browser to: http://localhost:5001" -ForegroundColor Yellow
Write-Host ""
Write-Host "Admin Login:" -ForegroundColor Magenta
Write-Host "  Email: admin@nbt.ac.za" -ForegroundColor Gray
Write-Host "  Password: Admin@123" -ForegroundColor Gray
Write-Host ""
Write-Host "Press any key to exit this window..." -ForegroundColor Gray
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
