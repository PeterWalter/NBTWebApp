# NBT Web Application Startup Script
# This script ensures a clean start of both API and WebUI

Write-Host "🚀 Starting NBT Web Application..." -ForegroundColor Green
Write-Host ""

# Stop any existing dotnet processes
Write-Host "🛑 Stopping existing dotnet processes..." -ForegroundColor Yellow
Stop-Process -Name "dotnet" -Force -ErrorAction SilentlyContinue
Start-Sleep -Seconds 2

# Clean and rebuild
Write-Host ""
Write-Host "🧹 Cleaning solution..." -ForegroundColor Cyan
Set-Location "D:\projects\source code\NBTWebApp"
dotnet clean --nologo --verbosity quiet

Write-Host ""
Write-Host "🔨 Building solution..." -ForegroundColor Cyan
dotnet build --nologo --verbosity quiet

if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Build failed! Please fix errors first." -ForegroundColor Red
    exit 1
}

Write-Host ""
Write-Host "✅ Build successful!" -ForegroundColor Green

# Start API in background
Write-Host ""
Write-Host "🌐 Starting API on https://localhost:7227..." -ForegroundColor Cyan
$apiPath = "D:\projects\source code\NBTWebApp\src\NBT.WebAPI"
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd '$apiPath'; Write-Host '🔥 API Server Running' -ForegroundColor Green; dotnet run --no-build --launch-profile https"

# Wait for API to start
Write-Host "⏳ Waiting for API to start..." -ForegroundColor Yellow
Start-Sleep -Seconds 8

# Test API
try {
    $response = Invoke-WebRequest -Uri "https://localhost:7227/api/announcements" -TimeoutSec 5 -ErrorAction Stop -SkipCertificateCheck
    Write-Host "✅ API is responding!" -ForegroundColor Green
} catch {
    Write-Host "⚠️  API not responding yet, but continuing..." -ForegroundColor Yellow
}

# Start WebUI
Write-Host ""
Write-Host "🖥️  Starting WebUI on https://localhost:5089..." -ForegroundColor Cyan
$webUIPath = "D:\projects\source code\NBTWebApp\src\NBT.WebUI"

Write-Host ""
Write-Host "════════════════════════════════════════════════════════" -ForegroundColor Green
Write-Host "✨ NBT Web Application Started!" -ForegroundColor Green
Write-Host "════════════════════════════════════════════════════════" -ForegroundColor Green
Write-Host ""
Write-Host "🌐 API:      https://localhost:7227" -ForegroundColor Cyan
Write-Host "🌐 Swagger:  https://localhost:7227/swagger" -ForegroundColor Cyan
Write-Host "🖥️  WebUI:    https://localhost:5089" -ForegroundColor Cyan
Write-Host "🛡️  Admin:    https://localhost:5089/admin" -ForegroundColor Cyan
Write-Host ""
Write-Host "📊 Press Ctrl+C in the API window to stop the API" -ForegroundColor Yellow
Write-Host "📊 Press Ctrl+C here to stop the WebUI" -ForegroundColor Yellow
Write-Host ""
Write-Host "════════════════════════════════════════════════════════" -ForegroundColor Green
Write-Host ""

# Run WebUI in foreground  
Set-Location $webUIPath
dotnet run --no-build --urls "https://localhost:5089"
