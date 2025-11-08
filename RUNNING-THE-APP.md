# Running the NBT WebApp

## Quick Start

### Option 1: Using the Automated Script (Recommended)

```powershell
.\RUN-APP.ps1
```

This script will:
1. Clean up ports 5000 and 5001
2. Build the solution
3. Start WebAPI on http://localhost:5000
4. Start WebUI on http://localhost:5001
5. Open your browser automatically

### Option 2: Manual Start

#### Step 1: Clean Up Ports (If needed)

```powershell
# Kill any processes on port 5000
Get-NetTCPConnection -LocalPort 5000 -ErrorAction SilentlyContinue | ForEach-Object { Stop-Process -Id $_.OwningProcess -Force }

# Kill any processes on port 5001
Get-NetTCPConnection -LocalPort 5001 -ErrorAction SilentlyContinue | ForEach-Object { Stop-Process -Id $_.OwningProcess -Force }
```

#### Step 2: Build the Solution

```powershell
dotnet build NBTWebApp.sln --configuration Release
```

#### Step 3: Start WebAPI

Open a new terminal window and run:

```powershell
cd src\NBT.WebAPI
dotnet run --urls http://localhost:5000
```

Wait until you see: `Now listening on: http://localhost:5000`

#### Step 4: Start WebUI

Open another terminal window and run:

```powershell
cd src\NBT.WebUI
dotnet run --urls http://localhost:5001
```

Wait until you see: `Now listening on: http://localhost:5001`

#### Step 5: Open Browser

Navigate to: **http://localhost:5001**

---

## Application URLs

- **WebUI (Frontend)**: http://localhost:5001
- **WebAPI (Backend)**: http://localhost:5000
- **Swagger Documentation**: http://localhost:5000/swagger

---

## Troubleshooting

### Issue: "Port already in use"

**Solution**: Run the port cleanup commands from Step 1 above.

### Issue: "Connection refused" or "localhost refused to connect"

**Solutions**:
1. Ensure WebAPI is running first (check terminal for "Now listening on")
2. Wait 5-10 seconds after starting WebAPI before starting WebUI
3. Check that no firewall is blocking ports 5000 or 5001
4. Restart both applications

### Issue: "Attempting to reconnect to the server"

**Solutions**:
1. Refresh the page (F5)
2. Clear browser cache (Ctrl+Shift+Delete)
3. Ensure WebAPI is still running
4. Check browser console for errors (F12)
5. Restart both WebAPI and WebUI

### Issue: "Failed to fetch" or API errors

**Solutions**:
1. Verify WebAPI is running on http://localhost:5000
2. Test API directly: http://localhost:5000/api/health
3. Check API terminal for errors
4. Ensure database is accessible

### Issue: Buttons not working on Admin page

**Cause**: Connection issues or WebAPI not responding

**Solutions**:
1. Check browser console (F12) for errors
2. Verify you're logged in as Admin
3. Ensure WebAPI is running
4. Check network tab in browser dev tools for failed requests

---

## Login Credentials

### Default Admin Account
- **Email**: admin@nbt.ac.za
- **Password**: Admin@123

### Default Test Accounts
- **Student**: student@test.com / Student@123
- **Institution**: institution@test.com / Institution@123

---

## Features Available

### Public Pages (No Login Required)
- Home (/)
- About (/about)
- For Applicants (/applicants)
- For Educators (/educators)
- For Institutions (/institutions)
- News (/news)
- Contact (/contact)

### Protected Pages (Login Required)

#### Admin Pages
- Admin Dashboard (/admin)
- Manage Announcements (/admin/announcements)
- Manage Content (/admin/content)
- Manage Resources (/admin/resources)
- View Inquiries (/admin/inquiries)
- User Management (/admin/users)

#### Student Pages
- Student Dashboard (/student-dashboard)

#### Institution Pages
- Institution Dashboard (/institution-dashboard)

---

## Development Notes

### Technology Stack
- **Frontend**: Blazor Server (.NET 9)
- **Backend**: ASP.NET Core Web API (.NET 9)
- **UI Library**: Microsoft Fluent UI
- **Database**: SQL Server (via Entity Framework Core)

### Render Mode
All pages use `@rendermode InteractiveServer` for:
- Consistent behavior across all pages
- Better stability (no WebAssembly switching)
- Improved reconnection handling
- Simplified state management

### Connection Configuration
The application is configured with:
- Max 8 reconnection attempts
- 3-second intervals between attempts
- Extended timeouts for long operations
- Buffered render batches for stability

---

## Stopping the Application

### If started with RUN-APP.ps1:
- Close both PowerShell windows that were opened
- Or press `Ctrl+C` in each terminal window

### If started manually:
- Press `Ctrl+C` in the WebAPI terminal
- Press `Ctrl+C` in the WebUI terminal

---

## Building for Production

```powershell
# Publish WebAPI
cd src\NBT.WebAPI
dotnet publish -c Release -o ..\..\publish\api

# Publish WebUI
cd ..\NBT.WebUI
dotnet publish -c Release -o ..\..\publish\ui
```

---

## Known Issues and Fixes

### ✅ FIXED: Connection Issues
- **Issue**: "Attempting to reconnect to server" appearing frequently
- **Fix**: Switched from InteractiveAuto to InteractiveServer mode
- **Status**: Resolved ✅

### ✅ FIXED: Admin Interface Buttons Not Working
- **Issue**: Create/Edit buttons unresponsive
- **Fix**: Added proper render mode and FormName attributes
- **Status**: Resolved ✅

### ✅ FIXED: Login Form Issues
- **Issue**: Missing FormName attribute error
- **Fix**: Added FormName="LoginForm" to EditForm
- **Status**: Resolved ✅

### ✅ FIXED: Inconsistent Port Usage
- **Issue**: App switching between ports causing confusion
- **Fix**: Standardized on 5000 (API) and 5001 (UI)
- **Status**: Resolved ✅

---

## Performance Tips

1. **First Load**: Initial page load may take 5-10 seconds
2. **Subsequent Loads**: Should be instant due to SignalR connection
3. **Database**: Ensure SQL Server is running for data operations
4. **Cache**: Clear browser cache if seeing stale data

---

## Support

For issues not covered here:
1. Check the browser console (F12) for errors
2. Check the WebAPI terminal for backend errors
3. Review the GitHub issues page
4. Contact the development team

---

**Last Updated**: 2025-01-08  
**Version**: 1.0.0  
**Status**: Production Ready ✅
