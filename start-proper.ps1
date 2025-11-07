# NBT Web Application Startup Script
Write-Host "==================================================" -ForegroundColor Cyan
Write-Host " NBT Web Application - Starting..." -ForegroundColor Cyan
Write-Host "==================================================" -ForegroundColor Cyan

# Kill any existing dotnet processes
Write-Host "`nStopping existing dotnet processes..." -ForegroundColor Yellow
Get-Process | Where-Object {$_.ProcessName -like "*dotnet*"} | Stop-Process -Force -ErrorAction SilentlyContinue
Start-Sleep -Seconds 2

# Set locations
$scriptPath = Split-Path -Parent $MyInvocation.MyCommand.Path
$apiPath = Join-Path $scriptPath "src\NBT.WebAPI"
$webPath = Join-Path $scriptPath "src\NBT.WebUI"

# Start WebAPI on port 7001 (HTTPS) and 7000 (HTTP)
Write-Host "`n[1/2] Starting Web API on https://localhost:7001..." -ForegroundColor Green
Start-Process -FilePath "dotnet" -ArgumentList "run --project `"$apiPath`"" -WorkingDirectory $apiPath -WindowStyle Normal

# Wait for API to start
Write-Host "Waiting for Web API to initialize..." -ForegroundColor Yellow
Start-Sleep -Seconds 8

# Start WebUI on port 5001 (HTTPS) and 5000 (HTTP)
Write-Host "`n[2/2] Starting Web UI on https://localhost:5001..." -ForegroundColor Green
Start-Process -FilePath "dotnet" -ArgumentList "run --project `"$webPath`"" -WorkingDirectory $webPath -WindowStyle Normal

# Wait for WebUI to start
Write-Host "Waiting for Web UI to initialize..." -ForegroundColor Yellow
Start-Sleep -Seconds 5

Write-Host "`n==================================================" -ForegroundColor Cyan
Write-Host " Application Started Successfully!" -ForegroundColor Green
Write-Host "==================================================" -ForegroundColor Cyan
Write-Host "`nWeb API:  https://localhost:7001" -ForegroundColor White
Write-Host "Web UI:   https://localhost:5001" -ForegroundColor White
Write-Host "`nPress Ctrl+C to stop monitoring (apps will continue running)" -ForegroundColor Gray
Write-Host "To stop the apps, close their console windows or run:" -ForegroundColor Gray
Write-Host "  Get-Process | Where-Object {`$_.ProcessName -like '*dotnet*'} | Stop-Process -Force" -ForegroundColor Gray
Write-Host "`n==================================================" -ForegroundColor Cyan

# Open browser
Start-Sleep -Seconds 2
Start-Process "https://localhost:5001"

# Keep script running
Write-Host "`nMonitoring applications... Press Ctrl+C to exit." -ForegroundColor Yellow
try {
    while ($true) {
        Start-Sleep -Seconds 5
    }
}
finally {
    Write-Host "`nScript terminated. Applications are still running." -ForegroundColor Yellow
}
