# Kill processes on ports 5000, 5001, 7000, 7001
Write-Host "Stopping existing processes on ports 5000, 5001, 7000, 7001..." -ForegroundColor Yellow

$ports = @(5000, 5001, 7000, 7001)
foreach ($port in $ports) {
    $connections = Get-NetTCPConnection -LocalPort $port -ErrorAction SilentlyContinue
    foreach ($conn in $connections) {
        $process = Get-Process -Id $conn.OwningProcess -ErrorAction SilentlyContinue
        if ($process) {
            Write-Host "  Killing process $($process.ProcessName) (PID: $($process.Id)) on port $port" -ForegroundColor Cyan
            Stop-Process -Id $process.Id -Force -ErrorAction SilentlyContinue
        }
    }
}

Start-Sleep -Seconds 2

# Start WebAPI
Write-Host "`nStarting NBT Web API on https://localhost:7001..." -ForegroundColor Green
$apiProcess = Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd 'D:\projects\source code\NBTWebApp\src\NBT.WebAPI'; dotnet run --no-build" -PassThru -WindowStyle Normal

Start-Sleep -Seconds 5

# Start WebUI
Write-Host "Starting NBT Web UI on https://localhost:5001..." -ForegroundColor Green
$webProcess = Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd 'D:\projects\source code\NBTWebApp\src\NBT.WebUI'; dotnet run --no-build" -PassThru -WindowStyle Normal

Start-Sleep -Seconds 5

Write-Host "`n✅ Application started successfully!" -ForegroundColor Green
Write-Host "   WebAPI: https://localhost:7001/swagger" -ForegroundColor Cyan
Write-Host "   WebUI:  https://localhost:5001" -ForegroundColor Cyan
Write-Host "`nPress Ctrl+C to stop all services..." -ForegroundColor Yellow

# Keep script running
try {
    while ($true) {
        Start-Sleep -Seconds 1
    }
}
finally {
    Write-Host "`nStopping services..." -ForegroundColor Yellow
    if ($apiProcess -and !$apiProcess.HasExited) {
        Stop-Process -Id $apiProcess.Id -Force -ErrorAction SilentlyContinue
    }
    if ($webProcess -and !$webProcess.HasExited) {
        Stop-Process -Id $webProcess.Id -Force -ErrorAction SilentlyContinue
    }
}
