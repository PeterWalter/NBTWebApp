# NBT Web Application - Developer Quickstart Guide

## Document Control
- **Version**: 2.0
- **Last Updated**: 2025-11-09
- **Status**: ACTIVE
- **For**: Developers setting up the project for the first time

---

## Prerequisites

### Required Software
- ✅ **Visual Studio 2022** (v17.8 or later) OR **VS Code** with C# extension
- ✅ **.NET 9.0 SDK** (latest version)
- ✅ **SQL Server 2019+** or **SQL Server Express** or **Azure SQL Database**
- ✅ **Git** for version control
- ✅ **Node.js** (v18+) - for build tools

### Optional Tools
- **SQL Server Management Studio** (SSMS) - for database management
- **Postman** - for API testing
- **Azure CLI** - for Azure deployments

---

## Quick Start (5 Minutes)

### 1. Clone Repository
```powershell
git clone https://github.com/yourusername/NBTWebApp.git
cd NBTWebApp
```

### 2. Configure Database Connection
Edit `src\NBTWebApp\appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=NBTWebApp;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

**Or for SQL Server Express**:
```json
"DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=NBTWebApp;Trusted_Connection=True;TrustServerCertificate=True"
```

**Or for Azure SQL**:
```json
"DefaultConnection": "Server=tcp:yourserver.database.windows.net,1433;Initial Catalog=NBTWebApp;Persist Security Info=False;User ID=yourusername;Password=yourpassword;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
```

### 3. Restore NuGet Packages
```powershell
cd "D:\projects\source code\NBTWebApp"
dotnet restore
```

### 4. Apply Database Migrations
```powershell
cd "src\NBTWebApp.Infrastructure"
dotnet ef database update --startup-project ..\NBTWebApp
```

**Or from solution root**:
```powershell
dotnet ef database update --project src\NBTWebApp.Infrastructure --startup-project src\NBTWebApp
```

### 5. Build Solution
```powershell
cd "D:\projects\source code\NBTWebApp"
dotnet build
```

### 6. Run Application
**Option A: Run Web API and Blazor separately**
```powershell
# Terminal 1: Run Web API
cd src\NBTWebApp
dotnet run

# Terminal 2: Run Blazor Client
cd src\NBTWebApp.Client
dotnet run
```

**Option B: Run Blazor Web App (Recommended)**
```powershell
cd src\NBTWebApp
dotnet run
```

### 7. Open in Browser
- **Application**: https://localhost:7001
- **API**: https://localhost:7001/api
- **Swagger**: https://localhost:7001/swagger

---

## Detailed Setup Guide

### Step 1: Environment Setup

#### Install .NET 9.0 SDK
```powershell
# Check if .NET 9 is installed
dotnet --version

# If not installed, download from:
# https://dotnet.microsoft.com/download/dotnet/9.0
```

#### Install SQL Server
**Option A: SQL Server Express (Free)**
1. Download from: https://www.microsoft.com/en-us/sql-server/sql-server-downloads
2. Install with default settings
3. Server name will be: `localhost\SQLEXPRESS`

**Option B: SQL Server Developer Edition (Free)**
1. Download from same link above
2. Install with default settings
3. Server name will be: `localhost`

**Option C: Use Azure SQL Database**
1. Create Azure SQL Database in Azure Portal
2. Configure firewall rules to allow your IP
3. Use connection string from Azure Portal

#### Install Visual Studio 2022 (Recommended)
1. Download from: https://visualstudio.microsoft.com/
2. Install with **ASP.NET and web development** workload
3. Install **.NET desktop development** workload

**Or Install VS Code**
1. Download from: https://code.visualstudio.com/
2. Install **C# Dev Kit** extension
3. Install **C# extension** by Microsoft

### Step 2: Clone and Explore

#### Clone Repository
```powershell
git clone https://github.com/yourusername/NBTWebApp.git
cd NBTWebApp
```

#### Explore Project Structure
```
NBTWebApp/
├── src/
│   ├── NBTWebApp.Client/              # Blazor WebAssembly client
│   ├── NBTWebApp/                     # Blazor Server host & API
│   ├── NBTWebApp.Core/                # Domain models & interfaces
│   ├── NBTWebApp.Infrastructure/      # Data access & services
│   └── NBTWebApp.Tests/               # Test projects
├── specs/                             # Specification documents
├── database-scripts/                  # SQL scripts
└── NBTWebApp.sln                      # Solution file
```

### Step 3: Configuration

#### User Secrets (Development)
For development, use user secrets to store sensitive data:

```powershell
cd src\NBTWebApp

# Initialize user secrets
dotnet user-secrets init

# Set database connection string
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Database=NBTWebApp;Trusted_Connection=True;TrustServerCertificate=True"

# Set JWT secret
dotnet user-secrets set "JwtSettings:Secret" "your-super-secret-key-minimum-32-characters-long"

# Set EasyPay credentials (sandbox)
dotnet user-secrets set "EasyPay:ApiKey" "your-easypay-sandbox-key"
dotnet user-secrets set "EasyPay:ApiSecret" "your-easypay-sandbox-secret"
```

#### appsettings.json Configuration
Edit `src\NBTWebApp\appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=NBTWebApp;Trusted_Connection=True;TrustServerCertificate=True"
  },
  "JwtSettings": {
    "Secret": "your-super-secret-key-minimum-32-characters-long",
    "Issuer": "NBTWebApp",
    "Audience": "NBTWebApp",
    "AccessTokenExpirationMinutes": 15,
    "RefreshTokenExpirationDays": 7
  },
  "EasyPay": {
    "BaseUrl": "https://sandbox.easypay.co.za/api",
    "ApiKey": "",
    "ApiSecret": "",
    "WebhookSecret": ""
  },
  "EmailSettings": {
    "SmtpServer": "smtp.gmail.com",
    "SmtpPort": 587,
    "SenderEmail": "noreply@nbt.ac.za",
    "SenderName": "NBT Web Application",
    "Username": "",
    "Password": ""
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

### Step 4: Database Setup

#### Create Database
```powershell
# Option 1: Let EF Core create database (recommended)
cd src\NBTWebApp.Infrastructure
dotnet ef database update --startup-project ..\NBTWebApp

# Option 2: Create database manually with SSMS
# Then run migrations
```

#### Verify Database Creation
```sql
-- Connect to SQL Server with SSMS
-- Run this query to verify tables were created

USE NBTWebApp;
GO

SELECT TABLE_NAME 
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_TYPE = 'BASE TABLE'
ORDER BY TABLE_NAME;
```

#### Seed Test Data
```powershell
# Run seed data script
cd database-scripts
sqlcmd -S localhost -d NBTWebApp -E -i seed-data.sql

# Or run from C# application (if seeding implemented in Program.cs)
cd ..\src\NBTWebApp
dotnet run --seed
```

### Step 5: Build and Run

#### Build Solution
```powershell
# Build entire solution
cd "D:\projects\source code\NBTWebApp"
dotnet build

# Check for errors
# Fix any errors before proceeding
```

#### Run Tests
```powershell
# Run all tests
dotnet test

# Run specific test project
cd src\NBTWebApp.Tests
dotnet test
```

#### Run Application
```powershell
# Run Blazor Web App (hosts both server and client)
cd src\NBTWebApp
dotnet run

# Application will start on:
# https://localhost:7001
# http://localhost:5001
```

#### Watch Mode (Auto-reload on code changes)
```powershell
cd src\NBTWebApp
dotnet watch run
```

### Step 6: Verify Installation

#### Check Landing Page
1. Open browser: https://localhost:7001
2. You should see the NBT landing page
3. Verify navigation menus work

#### Check API
1. Open Swagger: https://localhost:7001/swagger
2. Verify API endpoints are listed
3. Try a test API call (e.g., GET /api/v1/venues)

#### Check Database
```sql
-- Verify seed data
USE NBTWebApp;
GO

SELECT COUNT(*) FROM Students;
SELECT COUNT(*) FROM Venues;
SELECT COUNT(*) FROM TestSessions;
```

#### Test User Login
1. Navigate to: https://localhost:7001/login
2. Use test credentials:
   - **Email**: admin@nbt.ac.za
   - **Password**: Admin@123
3. Verify you are redirected to dashboard

---

## Common Issues & Solutions

### Issue 1: "Database does not exist"
**Solution**:
```powershell
cd src\NBTWebApp.Infrastructure
dotnet ef database update --startup-project ..\NBTWebApp
```

### Issue 2: "Unable to connect to SQL Server"
**Solutions**:
- Check SQL Server is running (Services → SQL Server)
- Verify connection string in appsettings.json
- Check firewall isn't blocking SQL Server (port 1433)
- For Express edition, use: `Server=localhost\SQLEXPRESS`

### Issue 3: "Migration failed"
**Solution**:
```powershell
# Drop database and recreate
dotnet ef database drop --startup-project ..\NBTWebApp
dotnet ef database update --startup-project ..\NBTWebApp
```

### Issue 4: "Port already in use"
**Solution**:
Edit `src\NBTWebApp\Properties\launchSettings.json` and change port:
```json
"applicationUrl": "https://localhost:7002;http://localhost:5002"
```

### Issue 5: "FluentUI components not rendering"
**Solution**:
1. Check NuGet package is installed: `Microsoft.FluentUI.AspNetCore.Components`
2. Verify service registration in `Program.cs`:
```csharp
builder.Services.AddFluentUIComponents();
```
3. Verify import in `_Imports.razor`:
```razor
@using Microsoft.FluentUI.AspNetCore.Components
```

### Issue 6: "JSON property name error"
**Solution**:
Run the JSON fix script:
```powershell
.\APPLY-JSON-FIX.ps1
```

### Issue 7: "Cannot find JSON fix script"
**Solution**:
Create the script manually or use:
```powershell
# Find all files with the issue
Get-ChildItem -Path "src" -Recurse -Filter "*.cs" | Select-String -Pattern 'JsonPropertyName\("(\w+)"\)' | ForEach-Object {
    $file = $_.Path
    $content = Get-Content $file -Raw
    # Apply fix (lowercase first letter)
    $content = $content -replace 'JsonPropertyName\("([A-Z])(\w+)"\)', {
        param($match)
        'JsonPropertyName("' + $match.Groups[1].Value.ToLower() + $match.Groups[2].Value + '")'
    }
    Set-Content $file $content -NoNewline
}
```

---

## Development Workflow

### Daily Development

#### 1. Pull Latest Changes
```powershell
git pull origin main
```

#### 2. Create Feature Branch
```powershell
git checkout -b feature/your-feature-name
```

#### 3. Make Changes
- Write code
- Write tests
- Run tests: `dotnet test`

#### 4. Run Application
```powershell
cd src\NBTWebApp
dotnet watch run
```

#### 5. Commit Changes
```powershell
git add .
git commit -m "feat: description of your feature"
```

#### 6. Push to Remote
```powershell
git push origin feature/your-feature-name
```

#### 7. Create Pull Request
- Go to GitHub repository
- Click "New Pull Request"
- Select your branch
- Request review from team member

### Running Specific Tests

#### Unit Tests Only
```powershell
dotnet test --filter "Category=Unit"
```

#### Integration Tests Only
```powershell
dotnet test --filter "Category=Integration"
```

#### E2E Tests Only
```powershell
dotnet test --filter "Category=E2E"
```

#### Test with Coverage
```powershell
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```

### Database Migrations

#### Create New Migration
```powershell
cd src\NBTWebApp.Infrastructure
dotnet ef migrations add MigrationName --startup-project ..\NBTWebApp
```

#### Apply Migration
```powershell
dotnet ef database update --startup-project ..\NBTWebApp
```

#### Remove Last Migration (if not applied)
```powershell
dotnet ef migrations remove --startup-project ..\NBTWebApp
```

#### Rollback to Specific Migration
```powershell
dotnet ef database update MigrationName --startup-project ..\NBTWebApp
```

### Code Quality

#### Format Code
```powershell
dotnet format
```

#### Run Code Analysis
```powershell
dotnet build /p:TreatWarningsAsErrors=true
```

#### Check Code Coverage
```powershell
dotnet test /p:CollectCoverage=true
# Report will be in: TestResults/coverage.opencover.xml
```

---

## Useful Scripts

### Start Application (Quick)
Create `start-app.ps1`:
```powershell
# Restore packages
Write-Host "Restoring NuGet packages..." -ForegroundColor Green
dotnet restore

# Build solution
Write-Host "Building solution..." -ForegroundColor Green
dotnet build --no-restore

# Apply migrations
Write-Host "Applying database migrations..." -ForegroundColor Green
cd src\NBTWebApp.Infrastructure
dotnet ef database update --startup-project ..\NBTWebApp
cd ..\..

# Run application
Write-Host "Starting application..." -ForegroundColor Green
cd src\NBTWebApp
dotnet run

# Application will be available at: https://localhost:7001
```

### Clean Build
Create `clean-build.ps1`:
```powershell
Write-Host "Cleaning solution..." -ForegroundColor Green
dotnet clean

Write-Host "Removing bin and obj folders..." -ForegroundColor Green
Get-ChildItem -Path . -Include bin,obj -Recurse -Directory | Remove-Item -Recurse -Force

Write-Host "Restoring packages..." -ForegroundColor Green
dotnet restore

Write-Host "Building solution..." -ForegroundColor Green
dotnet build

Write-Host "Running tests..." -ForegroundColor Green
dotnet test

Write-Host "Done!" -ForegroundColor Green
```

### Reset Database
Create `reset-database.ps1`:
```powershell
Write-Host "Dropping database..." -ForegroundColor Yellow
cd src\NBTWebApp.Infrastructure
dotnet ef database drop --force --startup-project ..\NBTWebApp

Write-Host "Recreating database..." -ForegroundColor Green
dotnet ef database update --startup-project ..\NBTWebApp

Write-Host "Seeding data..." -ForegroundColor Green
cd ..\NBTWebApp
dotnet run --seed

cd ..\..
Write-Host "Database reset complete!" -ForegroundColor Green
```

---

## IDE Setup

### Visual Studio 2022

#### Recommended Extensions
1. **GitHub Copilot** - AI pair programmer
2. **Web Essentials** - Web development tools
3. **CodeMaid** - Code cleanup and organization
4. **ReSharper** (optional) - Advanced code analysis

#### Configuration
1. **Tools → Options → Text Editor → C# → Code Style**
   - Set naming conventions
   - Set formatting rules
2. **Tools → Options → Projects and Solutions → Build and Run**
   - Set "On Run, when build or deployment errors occur" to "Do not launch"

### VS Code

#### Recommended Extensions
1. **C# Dev Kit** by Microsoft
2. **C#** by Microsoft
3. **NuGet Package Manager**
4. **REST Client** - Test API endpoints
5. **Thunder Client** - Alternative API tester
6. **GitLens** - Git integration
7. **Prettier** - Code formatter

#### Configuration
Create `.vscode/settings.json`:
```json
{
  "omnisharp.enableRoslynAnalyzers": true,
  "omnisharp.enableEditorConfigSupport": true,
  "editor.formatOnSave": true,
  "editor.codeActionsOnSave": {
    "source.fixAll": true
  },
  "files.exclude": {
    "**/bin": true,
    "**/obj": true
  }
}
```

Create `.vscode/launch.json`:
```json
{
  "version": "0.2.0",
  "configurations": [
    {
      "name": ".NET Core Launch (web)",
      "type": "coreclr",
      "request": "launch",
      "preLaunchTask": "build",
      "program": "${workspaceFolder}/src/NBTWebApp/bin/Debug/net9.0/NBTWebApp.dll",
      "args": [],
      "cwd": "${workspaceFolder}/src/NBTWebApp",
      "stopAtEntry": false,
      "serverReadyAction": {
        "action": "openExternally",
        "pattern": "\\bNow listening on:\\s+(https?://\\S+)"
      },
      "env": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      },
      "sourceFileMap": {
        "/Views": "${workspaceFolder}/Views"
      }
    }
  ]
}
```

---

## Troubleshooting Checklist

Before asking for help, check:

- [ ] .NET 9.0 SDK is installed (`dotnet --version`)
- [ ] SQL Server is running
- [ ] Connection string is correct in appsettings.json
- [ ] Database migrations are applied (`dotnet ef database update`)
- [ ] NuGet packages are restored (`dotnet restore`)
- [ ] Solution builds without errors (`dotnet build`)
- [ ] All tests pass (`dotnet test`)
- [ ] Ports 7001 and 5001 are not in use
- [ ] Firewall allows SQL Server (port 1433)
- [ ] FluentUI packages are installed
- [ ] User secrets are configured (for sensitive data)

---

## Getting Help

### Documentation
- **Constitution**: `specs/CONSTITUTION.md`
- **Specification**: `specs/COMPLETE-SPECIFICATION.md`
- **Implementation Plan**: `specs/IMPLEMENTATION-PLAN-COMPLETE.md`
- **Task Breakdown**: `specs/TASK-BREAKDOWN.md`

### Resources
- **Blazor Docs**: https://learn.microsoft.com/en-us/aspnet/core/blazor
- **FluentUI Blazor**: https://www.fluentui-blazor.net/
- **EF Core Docs**: https://learn.microsoft.com/en-us/ef/core/
- **.NET API Docs**: https://learn.microsoft.com/en-us/dotnet/api/

### Contact
- **Email**: dev-team@nbt.ac.za
- **Teams**: NBT Development Channel
- **GitHub Issues**: https://github.com/yourusername/NBTWebApp/issues

---

## Next Steps

Once you have the application running:

1. ✅ Read the **CONSTITUTION.md** to understand non-negotiable principles
2. ✅ Review **COMPLETE-SPECIFICATION.md** for full system requirements
3. ✅ Check **IMPLEMENTATION-PLAN-COMPLETE.md** for development phases
4. ✅ Review **TASK-BREAKDOWN.md** to see all tasks
5. ⏳ Pick a task from Phase 0 to start
6. ⏳ Create feature branch and begin development
7. ⏳ Write tests for your code
8. ⏳ Submit pull request when done

---

**Welcome to the NBT Web Application development team! 🚀**

---

**Document Owner**: Development Team  
**Last Updated**: 2025-11-09  
**Status**: ACTIVE

**END OF DOCUMENT**
