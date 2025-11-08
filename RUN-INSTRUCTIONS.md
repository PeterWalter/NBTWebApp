# NBT WebApp - How to Run the Application

## Prerequisites
- .NET 9 SDK installed
- SQL Server or SQL Server LocalDB installed
- Ports 5000 (WebAPI) and 5001 (WebUI) available

## Step-by-Step Instructions

### Step 1: Kill Any Processes on Required Ports
```powershell
# Kill process on port 5000 (WebAPI)
$process = Get-NetTCPConnection -LocalPort 5000 -ErrorAction SilentlyContinue
if ($process) {
    Stop-Process -Id $process.OwningProcess -Force
    Write-Host "Killed process on port 5000"
}

# Kill process on port 5001 (WebUI)
$process = Get-NetTCPConnection -LocalPort 5001 -ErrorAction SilentlyContinue
if ($process) {
    Stop-Process -Id $process.OwningProcess -Force
    Write-Host "Killed process on port 5001"
}

# Wait for ports to be released
Start-Sleep -Seconds 2
```

### Step 2: Update Database
```powershell
cd "D:\projects\source code\NBTWebApp\src\NBT.Infrastructure"
dotnet ef database update --startup-project ..\NBT.WebAPI\NBT.WebAPI.csproj
```

### Step 3: Start WebAPI (Terminal 1)
```powershell
cd "D:\projects\source code\NBTWebApp\src\NBT.WebAPI"
dotnet run --urls "http://localhost:5000"
```

Wait for message: "Now listening on: http://localhost:5000"

### Step 4: Start WebUI (Terminal 2 - New Window)
```powershell
cd "D:\projects\source code\NBTWebApp\src\NBT.WebUI"
dotnet run --urls "http://localhost:5001"
```

Wait for message: "Now listening on: http://localhost:5001"

### Step 5: Access the Application
Open your browser and navigate to: **http://localhost:5001**

## Quick Start Script

You can use the provided PowerShell script:

```powershell
.\start-app.ps1
```

This script will:
1. Kill processes on ports 5000 and 5001
2. Update the database
3. Start both WebAPI and WebUI in separate windows

## Troubleshooting

### Issue: "localhost refused to connect"
**Solution**: Make sure both WebAPI (port 5000) and WebUI (port 5001) are running. Check Step 3 and Step 4.

### Issue: "Attempting to reconnect to the server"
**Causes**:
- WebAPI is not running or crashed
- Database connection issue
- SignalR circuit timeout

**Solutions**:
1. Check WebAPI terminal for errors
2. Verify database connection string in `appsettings.json`
3. Restart both applications
4. Check browser console for specific errors

### Issue: Port already in use
**Solution**: Run Step 1 to kill processes on the required ports.

### Issue: Database errors
**Solution**: Run database update command from Step 2.

## Important Notes

- **Always start WebAPI first**, then WebUI
- Keep both terminal windows open while using the application
- Use **HTTP** (not HTTPS) for local development: `http://localhost:5001`
- The WebUI depends on WebAPI being available at `http://localhost:5000`

## Default Ports

| Service | Port | URL |
|---------|------|-----|
| WebAPI | 5000 | http://localhost:5000 |
| WebUI | 5001 | http://localhost:5001 |

## Login Credentials

### Admin Account
- Email: admin@nbt.ac.za
- Password: Admin@123

### Test Student Account
- Email: student@test.com
- Password: Student@123
