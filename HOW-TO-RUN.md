# How to Run NBT Web Application

## Prerequisites
- .NET 9.0 SDK installed
- SQL Server running (LocalDB or full instance)
- Ports 5000, 5001, 5089, and 5090 available

## Step-by-Step Running Instructions

### Method 1: Using PowerShell Script (Recommended)

1. **Navigate to project root:**
   ```powershell
   cd "D:\projects\source code\NBTWebApp"
   ```

2. **Kill any existing processes on required ports:**
   ```powershell
   # Kill processes on ports 5000, 5001, 5089, 5090
   Get-NetTCPConnection -LocalPort 5000,5001,5089,5090 -ErrorAction SilentlyContinue | 
       ForEach-Object { Stop-Process -Id $_.OwningProcess -Force -ErrorAction SilentlyContinue }
   ```

3. **Run the application:**
   ```powershell
   .\start-app.ps1
   ```

### Method 2: Manual Start (Required for proper operation)

#### Step 1: Clean All Processes
```powershell
# Kill all dotnet processes
Get-Process -Name "dotnet" -ErrorAction SilentlyContinue | Stop-Process -Force

# Kill processes on specific ports
Get-NetTCPConnection -LocalPort 5000,5001,5089,5090 -ErrorAction SilentlyContinue | 
    ForEach-Object { Stop-Process -Id $_.OwningProcess -Force -ErrorAction SilentlyContinue }

# Wait for ports to be released
Start-Sleep -Seconds 3
```

#### Step 2: Start WebAPI (Backend)
```powershell
# Navigate to WebAPI project
cd "D:\projects\source code\NBTWebApp\src\NBT.WebAPI"

# Start WebAPI on ports 5089 (HTTP) and 5090 (HTTPS)
Start-Process powershell -ArgumentList "-NoExit", "-Command", "dotnet run --urls `"http://localhost:5089;https://localhost:5090`""

# Wait for API to start
Start-Sleep -Seconds 10
```

#### Step 3: Start WebUI (Frontend)
```powershell
# Navigate to WebUI project
cd "D:\projects\source code\NBTWebApp\src\NBT.WebUI"

# Start WebUI on ports 5000 (HTTP) and 5001 (HTTPS)
Start-Process powershell -ArgumentList "-NoExit", "-Command", "dotnet run --urls `"http://localhost:5000;https://localhost:5001`""

# Wait for UI to start
Start-Sleep -Seconds 10
```

#### Step 4: Open Browser
```powershell
# Open the application in default browser
Start-Process "http://localhost:5000"
```

### Method 3: Individual Project Start

If you need to run projects separately for debugging:

**Terminal 1 - WebAPI:**
```powershell
cd "D:\projects\source code\NBTWebApp\src\NBT.WebAPI"
dotnet run --urls "http://localhost:5089;https://localhost:5090"
```

**Terminal 2 - WebUI:**
```powershell
cd "D:\projects\source code\NBTWebApp\src\NBT.WebUI"
dotnet run --urls "http://localhost:5000;https://localhost:5001"
```

## Application URLs

- **Frontend (WebUI):** http://localhost:5000 or https://localhost:5001
- **Backend API (WebAPI):** http://localhost:5089 or https://localhost:5090
- **API Documentation:** http://localhost:5089/swagger (when running in Development mode)

## Troubleshooting

### Problem: "Attempting to reconnect to the server"

**Causes:**
- WebAPI backend is not running
- Port conflicts
- Blazor circuit timeout
- Connection issues between WebUI and WebAPI

**Solutions:**
1. Ensure WebAPI is running first before starting WebUI
2. Kill all processes and restart following Manual Start steps
3. Check that both services are running on correct ports
4. Clear browser cache and reload

### Problem: "Localhost refused to connect"

**Solution:**
```powershell
# Kill all dotnet processes
Get-Process -Name "dotnet" -ErrorAction SilentlyContinue | Stop-Process -Force

# Clean up ports
Get-NetTCPConnection -LocalPort 5000,5001,5089,5090 -ErrorAction SilentlyContinue | 
    ForEach-Object { Stop-Process -Id $_.OwningProcess -Force -ErrorAction SilentlyContinue }

# Wait and restart
Start-Sleep -Seconds 5
```

### Problem: Port Already in Use

**Solution:**
```powershell
# Find process using port (example: 5000)
Get-NetTCPConnection -LocalPort 5000 -ErrorAction SilentlyContinue | 
    Select-Object -ExpandProperty OwningProcess | 
    ForEach-Object { Stop-Process -Id $_ -Force }
```

### Problem: Database Connection Issues

**Solution:**
1. Verify SQL Server is running
2. Check connection string in `appsettings.json`
3. Run database migrations:
   ```powershell
   cd "D:\projects\source code\NBTWebApp\src\NBT.WebAPI"
   dotnet ef database update
   ```

### Problem: Build Errors

**Solution:**
```powershell
# Clean solution
dotnet clean

# Restore packages
dotnet restore

# Rebuild
dotnet build
```

## Port Configuration

### Why These Ports?

- **5000/5001:** Standard ASP.NET Core development ports for frontend
- **5089/5090:** Separate ports for WebAPI to avoid conflicts
- **HTTP (5000, 5089):** For local development without SSL issues
- **HTTPS (5001, 5090):** For testing secure connections

### Changing Ports

Edit `launchSettings.json` in each project's Properties folder:

**WebUI:** `src/NBT.WebUI/Properties/launchSettings.json`
**WebAPI:** `src/NBT.WebAPI/Properties/launchSettings.json`

## Development vs Production

### Development Mode
- Uses local SQL Server (LocalDB)
- Detailed error pages
- Hot reload enabled
- Swagger API documentation available

### Production Mode
- Uses Azure SQL Database
- Generic error pages
- Optimized performance
- Swagger disabled

## Quick Commands Reference

```powershell
# Full cleanup and restart
Get-Process -Name "dotnet" | Stop-Process -Force; .\start-app.ps1

# Check what's running on ports
Get-NetTCPConnection -LocalPort 5000,5001,5089,5090

# View WebAPI logs
cd "D:\projects\source code\NBTWebApp\src\NBT.WebAPI"
dotnet run --urls "http://localhost:5089" --verbosity detailed

# View WebUI logs
cd "D:\projects\source code\NBTWebApp\src\NBT.WebUI"
dotnet run --urls "http://localhost:5000" --verbosity detailed
```

## Architecture Notes

### Blazor Server Architecture
- Uses SignalR for client-server communication
- Maintains stateful connection
- Circuit timeout default: 3 minutes
- Reconnection attempts: 8 tries with exponential backoff

### API Communication
- WebUI calls WebAPI via HttpClient
- Base URL configured in appsettings.json
- API URL: http://localhost:5089

## Security Notes

### Development Certificates
```powershell
# Trust development certificates
dotnet dev-certs https --trust
```

### Authentication
- JWT tokens used for API authentication
- Cookie authentication for WebUI
- Admin role required for restricted pages

## Common Issues and Fixes

1. **"Required endpoints are not mapped"**
   - Ensure `AddInteractiveServerRenderMode()` is called in Program.cs
   
2. **"400 Bad Request" on form submission**
   - Check API is running
   - Verify API URL in WebUI configuration
   - Check request payload format

3. **Navigation not working**
   - Verify render modes are correctly set
   - Check that routes are defined in RouteData
   - Ensure NavigationManager is injected

4. **Buttons not responding**
   - Check browser console for JavaScript errors
   - Verify Blazor circuit is connected
   - Check that event handlers are properly bound

## Support

For issues not covered here, check:
- Project documentation in `/specs` folder
- Session findings in `SESSION-FINDINGS.md`
- Project status in `PROJECT-STATUS.md`

## Version Information

- .NET Version: 9.0
- Blazor Mode: Interactive Server
- Database: SQL Server / Azure SQL
- Authentication: JWT + Cookie
