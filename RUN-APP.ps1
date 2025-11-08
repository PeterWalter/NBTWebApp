#!/usr/bin/env pwsh
# NBT WebApp Startup Script
# This script properly starts both WebAPI and WebUI with port cleanup

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  NBT WebApp Startup Script" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Set error action preference
$ErrorActionPreference = "Stop"

# Navigate to project root
$projectRoot = "D:\projects\source code\NBTWebApp"
Set-Location $projectRoot

# Function to kill processes on specific ports
function Stop-ProcessOnPort {
    param(
        [int]$Port
    )
    
    Write-Host "Checking for processes on port $Port..." -ForegroundColor Yellow
    
    try {
        $connections = Get-NetTCPConnection -LocalPort $Port -ErrorAction SilentlyContinue
        
        if ($connections) {
            foreach ($conn in $connections) {
                $process = Get-Process -Id $conn.OwningProcess -ErrorAction SilentlyContinue
                if ($process) {
                    Write-Host "  Stopping process: $($process.Name) (PID: $($process.Id))" -ForegroundColor Red
                    Stop-Process -Id $process.Id -Force -ErrorAction SilentlyContinue
                    Start-Sleep -Seconds 1
                }
            }
            Write-Host "  Port $Port is now free" -ForegroundColor Green
        } else {
            Write-Host "  Port $Port is already free" -ForegroundColor Green
        }
    } catch {
        Write-Host "  Could not check port $Port : $($_.Exception.Message)" -ForegroundColor Yellow
    }
}

# Clean up ports
Write-Host "`nStep 1: Cleaning up ports..." -ForegroundColor Cyan
Stop-ProcessOnPort -Port 5000
Stop-ProcessOnPort -Port 5001
Start-Sleep -Seconds 2

# Build the solution
Write-Host "`nStep 2: Building solution..." -ForegroundColor Cyan
try {
    dotnet build "$projectRoot\NBTWebApp.sln" --configuration Release --verbosity minimal
    if ($LASTEXITCODE -ne 0) {
        throw "Build failed"
    }
    Write-Host "  Build completed successfully" -ForegroundColor Green
} catch {
    Write-Host "  Build failed: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

# Start WebAPI
Write-Host "`nStep 3: Starting WebAPI on port 5000..." -ForegroundColor Cyan
$apiPath = "$projectRoot\src\NBT.WebAPI"

if (Test-Path $apiPath) {
    Start-Process pwsh -ArgumentList "-NoExit", "-Command", "cd '$apiPath'; dotnet run --urls http://localhost:5000" -WindowStyle Normal
    Write-Host "  WebAPI starting..." -ForegroundColor Yellow
    Start-Sleep -Seconds 5
    
    # Verify API is running
    try {
        $response = Invoke-WebRequest -Uri "http://localhost:5000/api/health" -TimeoutSec 5 -ErrorAction SilentlyContinue
        Write-Host "  WebAPI is running successfully!" -ForegroundColor Green
    } catch {
        Write-Host "  WebAPI health check failed, but continuing..." -ForegroundColor Yellow
    }
} else {
    Write-Host "  WebAPI path not found: $apiPath" -ForegroundColor Red
    exit 1
}

# Start WebUI
Write-Host "`nStep 4: Starting WebUI on port 5001..." -ForegroundColor Cyan
$uiPath = "$projectRoot\src\NBT.WebUI"

if (Test-Path $uiPath) {
    Start-Process pwsh -ArgumentList "-NoExit", "-Command", "cd '$uiPath'; dotnet run --urls http://localhost:5001" -WindowStyle Normal
    Write-Host "  WebUI starting..." -ForegroundColor Yellow
    Start-Sleep -Seconds 5
    
    Write-Host "`n========================================" -ForegroundColor Cyan
    Write-Host "  NBT WebApp Started Successfully!" -ForegroundColor Green
    Write-Host "========================================" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "  WebAPI:  http://localhost:5000" -ForegroundColor White
    Write-Host "  WebUI:   http://localhost:5001" -ForegroundColor White
    Write-Host ""
    Write-Host "  Opening browser..." -ForegroundColor Yellow
    Start-Sleep -Seconds 3
    Start-Process "http://localhost:5001"
    
    Write-Host "`n  Press Ctrl+C in the terminal windows to stop the applications" -ForegroundColor Yellow
    Write-Host ""
} else {
    Write-Host "  WebUI path not found: $uiPath" -ForegroundColor Red
    exit 1
}
