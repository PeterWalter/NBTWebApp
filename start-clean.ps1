# Clean startup script for NBT Web Application
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "NBT Web Application - Clean Start" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan

# Kill any existing dotnet processes
Write-Host "`n[1/5] Stopping existing dotnet processes..." -ForegroundColor Yellow
Get-Process -Name "dotnet" -ErrorAction SilentlyContinue | Stop-Process -Force
Start-Sleep -Seconds 2

# Kill processes on ports 5000-5003
Write-Host "[2/5] Clearing ports 5000-5003..." -ForegroundColor Yellow
$ports = @(5000, 5001, 5002, 5003)
foreach ($port in $ports) {
    $connections = netstat -ano | findstr ":$port"
    if ($connections) {
        $connections | ForEach-Object {
            $pid = ($_ -split '\s+')[-1]
            if ($pid -match '^\d+$') {
                try {
                    Stop-Process -Id $pid -Force -ErrorAction SilentlyContinue
                    Write-Host "  Killed process $pid on port $port" -ForegroundColor Gray
                } catch {}
            }
        }
    }
}
Start-Sleep -Seconds 2

# Clean and build
Write-Host "[3/5] Cleaning and building solution..." -ForegroundColor Yellow
Set-Location "D:\projects\source code\NBTWebApp"
dotnet clean --nologo -v quiet
dotnet build --nologo -v quiet
if ($LASTEXITCODE -ne 0) {
    Write-Host "Build failed!" -ForegroundColor Red
    exit 1
}

# Start WebAPI
Write-Host "[4/5] Starting WebAPI on https://localhost:5002..." -ForegroundColor Yellow
Set-Location "D:\projects\source code\NBTWebApp\src\NBT.WebAPI"
Start-Process powershell -ArgumentList "-NoExit", "-Command", "dotnet run --no-build" -WindowStyle Normal
Start-Sleep -Seconds 5

# Wait for API to be ready
Write-Host "  Waiting for API to be ready..." -ForegroundColor Gray
$maxAttempts = 20
$attempt = 0
$apiReady = $false
while ($attempt -lt $maxAttempts -and -not $apiReady) {
    try {
        $response = Invoke-WebRequest -Uri "https://localhost:5002/api/health" -SkipCertificateCheck -TimeoutSec 2 -ErrorAction Stop
        if ($response.StatusCode -eq 200) {
            $apiReady = $true
        }
    } catch {}
    if (-not $apiReady) {
        Start-Sleep -Seconds 1
        $attempt++
        Write-Host "." -NoNewline -ForegroundColor Gray
    }
}
Write-Host ""

if (-not $apiReady) {
    Write-Host "  Warning: API may not be fully ready" -ForegroundColor Yellow
}

# Start WebUI
Write-Host "[5/5] Starting WebUI on https://localhost:5001..." -ForegroundColor Yellow
Set-Location "D:\projects\source code\NBTWebApp\src\NBT.WebUI"
Start-Process powershell -ArgumentList "-NoExit", "-Command", "dotnet run --no-build" -WindowStyle Normal
Start-Sleep -Seconds 3

Write-Host "`n========================================" -ForegroundColor Green
Write-Host "Application Started Successfully!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green
Write-Host "WebAPI:  https://localhost:5002" -ForegroundColor White
Write-Host "WebUI:   https://localhost:5001" -ForegroundColor White
Write-Host "`nOpening browser in 3 seconds..." -ForegroundColor Gray
Start-Sleep -Seconds 3
Start-Process "https://localhost:5001"
