# Developer Quickstart Guide - NBT Project Shell

**Last Updated**: 2025-11-08  
**Version**: 1.0  
**Prerequisites Check Time**: ~5 minutes  
**Initial Setup Time**: ~15-20 minutes  
**Status**: ✅ READY FOR DEVELOPMENT

---

## 🚀 QUICK START (5 Minutes)

### Step 1: Clone & Restore (2 minutes)

```bash
# Clone repository
git clone <repository-url>
cd NBTWebApp

# Restore all NuGet packages
dotnet restore
```

### Step 2: Database Setup (2 minutes)

```bash
# Update connection string in appsettings.Development.json
# Then apply migrations
cd src/NBT.WebAPI
dotnet ef database update
```

### Step 3: Run Applications (1 minute)

```bash
# Terminal 1 - Run API (port 5000)
cd src/NBT.WebAPI
dotnet run

# Terminal 2 - Run Blazor UI (port 5001)
cd src/NBT.WebUI
dotnet run
```

### Step 4: Verify (30 seconds)

- Open browser: `https://localhost:5001`
- Login: `admin@nbt.ac.za` / `Admin@123`
- Navigate to `/admin` - should see dashboard

**✅ If all above works, you're ready to start development!**

---

## 📋 PREREQUISITES

### Required Software

| Software | Version | Download | Check Command |
|----------|---------|----------|---------------|
| .NET SDK | 9.0+ | [Download](https://dotnet.microsoft.com/download/dotnet/9.0) | `dotnet --version` |
| SQL Server | 2019+ | [Download](https://www.microsoft.com/sql-server) | `sqlcmd -?` |
| Git | Latest | [Download](https://git-scm.com/) | `git --version` |
| Visual Studio 2022 | 17.8+ | [Download](https://visualstudio.microsoft.com/) | Optional |
| VS Code | Latest | [Download](https://code.visualstudio.com/) | Optional |

### Verify Prerequisites

```powershell
# Run this verification script
Write-Host "Checking prerequisites..." -ForegroundColor Cyan

# Check .NET
$dotnetVersion = dotnet --version
if ($dotnetVersion -match "^9\.") {
    Write-Host "✓ .NET 9.x installed: $dotnetVersion" -ForegroundColor Green
} else {
    Write-Host "✗ .NET 9.x required. Current: $dotnetVersion" -ForegroundColor Red
}

# Check SQL Server
try {
    $sqlCheck = sqlcmd -Q "SELECT @@VERSION" -b
    Write-Host "✓ SQL Server accessible" -ForegroundColor Green
} catch {
    Write-Host "✗ SQL Server not accessible" -ForegroundColor Red
}

# Check Git
$gitVersion = git --version
Write-Host "✓ Git installed: $gitVersion" -ForegroundColor Green

Write-Host "`nAll checks complete!" -ForegroundColor Cyan
```

---

## 🔧 DETAILED SETUP INSTRUCTIONS

### 1. Clone Repository

```bash
# Clone the repository
git clone https://github.com/your-org/NBTWebApp.git
cd NBTWebApp

# Check current branch
git branch

# Should be on 'main' or 'develop'
```

**Expected Structure**:
```
NBTWebApp/
├── src/
│   ├── NBT.Domain/
│   ├── NBT.Application/
│   ├── NBT.Infrastructure/
│   ├── NBT.WebAPI/
│   └── NBT.WebUI/
├── tests/                    # Coming soon
├── database-scripts/
├── specs/
├── NBTWebApp.sln
└── README.md
```

### 2. Restore NuGet Packages

```bash
# Restore all projects in solution
dotnet restore

# Or restore specific projects
dotnet restore src/NBT.Domain/NBT.Domain.csproj
dotnet restore src/NBT.Application/NBT.Application.csproj
dotnet restore src/NBT.Infrastructure/NBT.Infrastructure.csproj
dotnet restore src/NBT.WebAPI/NBT.WebAPI.csproj
dotnet restore src/NBT.WebUI/NBT.WebUI.csproj
```

**Expected Output**:
```
Determining projects to restore...
Restored NBT.Domain (in 1.2 sec)
Restored NBT.Application (in 2.3 sec)
Restored NBT.Infrastructure (in 3.1 sec)
Restored NBT.WebAPI (in 2.8 sec)
Restored NBT.WebUI (in 3.5 sec)
```

**Key Packages Installed**:
- ✅ Microsoft.EntityFrameworkCore (9.0)
- ✅ Microsoft.AspNetCore.Identity (9.0)
- ✅ Microsoft.FluentUI.AspNetCore.Components (4.9.0)
- ✅ FluentValidation (11.9.0)
- ✅ AutoMapper (13.0.0)

### 3. Configure Database Connection String

**Location**: `src/NBT.WebAPI/appsettings.Development.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=NBTWebsite;Trusted_Connection=true;MultipleActiveResultSets=true"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore": "Information"
    }
  }
}
```

**Connection String Options**:

#### Option A: SQL Server LocalDB (Recommended for Development)
```json
"DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=NBTWebsite;Trusted_Connection=true;MultipleActiveResultSets=true"
```

#### Option B: SQL Server Express
```json
"DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=NBTWebsite;Trusted_Connection=true;MultipleActiveResultSets=true"
```

#### Option C: SQL Server with Credentials
```json
"DefaultConnection": "Server=localhost;Database=NBTWebsite;User Id=sa;Password=YourPassword;TrustServerCertificate=true;MultipleActiveResultSets=true"
```

#### Option D: Azure SQL Database
```json
"DefaultConnection": "Server=tcp:your-server.database.windows.net,1433;Database=NBTWebsite;User ID=your-username;Password=your-password;Encrypt=True;TrustServerCertificate=False;MultipleActiveResultSets=True"
```

**Test Connection**:
```bash
# Test SQL Server connection
sqlcmd -S "(localdb)\mssqllocaldb" -Q "SELECT @@VERSION"
```

### 4. Apply EF Core Migrations

```bash
# Navigate to API project
cd src/NBT.WebAPI

# Apply migrations to create database
dotnet ef database update

# Expected output:
# Build started...
# Build succeeded.
# Applying migration '20251107113354_InitialCreate'.
# Done.
```

**Verify Database Created**:
```bash
# List all databases
sqlcmd -S "(localdb)\mssqllocaldb" -Q "SELECT name FROM sys.databases WHERE name = 'NBTWebsite'"

# Expected output:
# name
# ----------
# NBTWebsite
```

**Verify Tables Created**:
```bash
sqlcmd -S "(localdb)\mssqllocaldb" -d NBTWebsite -Q "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE'"
```

**Expected Tables**:
- Users (ASP.NET Identity tables)
- Announcements
- ContactInquiries
- ContentPages
- DownloadableResources
- SystemSettings

### 5. Seed Test Data (Optional but Recommended)

The database will auto-seed on first run with:
- ✅ 1 Admin user (`admin@nbt.ac.za` / `Admin@123`)
- ✅ 3 Sample announcements
- ✅ 2 Content pages (About, FAQ)
- ✅ System settings

**Manual Seed** (if needed):
```bash
cd src/NBT.WebAPI
dotnet run --seed-database
```

**Verify Seed Data**:
```bash
# Check if admin user exists
sqlcmd -S "(localdb)\mssqllocaldb" -d NBTWebsite -Q "SELECT UserName, Email FROM Users"
```

### 6. Build Solution

```bash
# Build entire solution
dotnet build

# Expected output:
# Build succeeded.
#     0 Warning(s)
#     0 Error(s)
```

**Build Individual Projects** (if needed):
```bash
dotnet build src/NBT.Domain/NBT.Domain.csproj
dotnet build src/NBT.Application/NBT.Application.csproj
dotnet build src/NBT.Infrastructure/NBT.Infrastructure.csproj
dotnet build src/NBT.WebAPI/NBT.WebAPI.csproj
dotnet build src/NBT.WebUI/NBT.WebUI.csproj
```

### 7. Run Web API

```bash
# Terminal 1 - Run API
cd src/NBT.WebAPI
dotnet run

# Expected output:
# info: Microsoft.Hosting.Lifetime[14]
#       Now listening on: http://localhost:5000
#       Now listening on: https://localhost:5001
# info: Microsoft.Hosting.Lifetime[0]
#       Application started. Press Ctrl+C to shut down.
```

**Verify API is Running**:

Open browser or use curl:
```bash
# Test health check
curl http://localhost:5000/health

# Expected: {"status":"Healthy"}

# Test Swagger UI
# Open: http://localhost:5000/swagger
```

**Available API Endpoints** (26 total):

**Authentication** (3 endpoints):
- POST `/api/auth/login`
- POST `/api/auth/register`
- POST `/api/auth/refresh-token`

**Announcements** (5 endpoints):
- GET `/api/announcements`
- GET `/api/announcements/{id}`
- POST `/api/announcements`
- PUT `/api/announcements/{id}`
- DELETE `/api/announcements/{id}`

**Content Pages** (5 endpoints):
- GET `/api/contentpages`
- GET `/api/contentpages/{id}`
- POST `/api/contentpages`
- PUT `/api/contentpages/{id}`
- DELETE `/api/contentpages/{id}`

**Contact Inquiries** (4 endpoints):
- GET `/api/contactinquiries`
- GET `/api/contactinquiries/{id}`
- POST `/api/contactinquiries`
- PUT `/api/contactinquiries/{id}/status`

**Resources** (5 endpoints):
- GET `/api/resources`
- GET `/api/resources/{id}`
- POST `/api/resources`
- PUT `/api/resources/{id}`
- DELETE `/api/resources/{id}`

**System Settings** (4 endpoints):
- GET `/api/systemsettings`
- GET `/api/systemsettings/{key}`
- POST `/api/systemsettings`
- PUT `/api/systemsettings/{id}`

### 8. Run Blazor Web UI

```bash
# Terminal 2 - Run Blazor UI
cd src/NBT.WebUI
dotnet run

# Expected output:
# info: Microsoft.Hosting.Lifetime[14]
#       Now listening on: http://localhost:5002
#       Now listening on: https://localhost:5003
```

**Update WebUI API Configuration** (if needed):

Edit `src/NBT.WebUI/appsettings.Development.json`:
```json
{
  "ApiBaseUrl": "http://localhost:5000/",
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

### 9. Verify Application

#### 9.1 Open Browser

Navigate to: `https://localhost:5003` (or the port shown in terminal)

**Expected**: NBT Website landing page should load with Fluent UI theme

#### 9.2 Verify Public Pages

Test all public pages:
- ✅ `/` - Landing page
- ✅ `/about` - About NBT page
- ✅ `/applicants` - Information for applicants
- ✅ `/educators` - Information for educators
- ✅ `/institutions` - Information for institutions
- ✅ `/news` - Announcements list
- ✅ `/contact` - Contact form

#### 9.3 Verify Fluent UI Theme

Check that Fluent UI components are rendering correctly:
- FluentButton should have blue accent color
- FluentCard should have rounded corners
- FluentStack should align items correctly
- Dark/Light theme toggle should work

#### 9.4 Test Authentication

**Login to Admin Portal**:

1. Navigate to `/admin`
2. You should be redirected to login page
3. Enter credentials:
   - **Email**: `admin@nbt.ac.za`
   - **Password**: `Admin@123`
4. Click **Login**
5. Should redirect to `/admin` dashboard

**Verify Admin Pages**:
- ✅ `/admin` - Admin dashboard
- ✅ `/admin/announcements` - Announcement management
- ✅ `/admin/content` - Content page management
- ✅ `/admin/inquiries` - Contact inquiries
- ✅ `/admin/resources` - Downloadable resources
- ✅ `/admin/users` - User management

#### 9.5 Test Sample Data

**Test Announcement CRUD**:

1. Go to `/admin/announcements`
2. Click **Add Announcement**
3. Fill in form:
   - Title: "Test Announcement"
   - Content: "This is a test"
   - Category: General
4. Click **Save**
5. Verify announcement appears in list
6. Click **Edit** - verify form loads with data
7. Click **Delete** - verify confirmation dialog

**Expected**: All CRUD operations should work without errors

#### 9.6 Verify Navigation

Test navigation between pages:
- Click navigation links in header
- Use browser back/forward buttons
- Direct URL navigation
- All links should work without 404 errors

#### 9.7 Check Browser Console

Open Developer Tools (F12) and check:
- ✅ No JavaScript errors
- ✅ No 404 errors for assets
- ✅ Blazor SignalR connection established
- ✅ No CORS errors

**Expected Console Output**:
```
[2025-11-08T16:47:18.725Z] Information: Blazor Server started.
[2025-11-08T16:47:19.123Z] Information: Connected to SignalR hub.
```

---

## 🧪 RUNNING TESTS (Coming Soon)

Currently, no tests exist. Tests will be added in Phase 10 of implementation.

**Future Commands**:
```bash
# Run all tests
dotnet test

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"

# Run specific test project
dotnet test tests/NBT.Domain.Tests/NBT.Domain.Tests.csproj
```

---

## 🔍 SHELL AUDIT FINDINGS

Based on comprehensive review, the existing shell has:

### ✅ Working Components (40%)

**Domain Layer**:
- 6 entities (User, Announcement, ContentPage, ContactInquiry, DownloadableResource, SystemSetting)
- 4 enums (UserRole, AnnouncementCategory, InquiryType, InquiryStatus)
- Clean Architecture structure

**Application Layer**:
- 6 services (Authentication, Announcements, ContentPages, ContactInquiries, Resources)
- DTOs for all existing entities
- FluentValidation configured
- AutoMapper configured

**Infrastructure Layer**:
- EF Core configured
- 1 migration applied (InitialCreate)
- 5 entity configurations
- Database seed data

**API Layer**:
- 6 controllers (26 endpoints)
- JWT authentication
- Swagger documentation
- Health checks

**UI Layer**:
- 13 Blazor pages (7 public + 6 admin)
- Fluent UI integrated
- Authentication flow
- Responsive layout

### ❌ Missing Components (60%)

**Critical Missing Entities** (9):
- ❌ Student
- ❌ Registration
- ❌ Payment
- ❌ TestSession
- ❌ Venue
- ❌ Room
- ❌ RoomAllocation
- ❌ TestResult
- ❌ AuditLog

**Missing Value Objects** (2):
- ❌ NBTNumber (Luhn validation)
- ❌ SAIDNumber (SA ID validation)

**Missing Services** (12):
- ❌ IStudentService
- ❌ IRegistrationService
- ❌ IPaymentService
- ❌ IEasyPayService
- ❌ ITestSessionService
- ❌ IVenueService
- ❌ ITestResultService
- ❌ IReportService
- ❌ IExcelService
- ❌ IPdfService
- ❌ IAuditService
- ❌ NBTNumberGenerator

**Missing API Controllers** (9):
- ❌ StudentsController
- ❌ RegistrationsController
- ❌ BookingController
- ❌ PaymentsController
- ❌ VenuesController
- ❌ SessionsController
- ❌ ResultsController
- ❌ StaffController
- ❌ ReportsController

**Missing UI Pages** (25):
- ❌ Registration wizard (7 pages/components)
- ❌ Admin student management (3 pages)
- ❌ Admin registration management (2 pages)
- ❌ Admin payment management (1 page)
- ❌ Admin venue management (4 pages)
- ❌ Admin session management (3 pages)
- ❌ Admin results management (2 pages)
- ❌ Admin reports (2 pages)
- ❌ Staff dashboard (1 page)

---

## 📦 NUGET PACKAGES REFERENCE

### Installed Packages (Verified)

**NBT.Domain**:
```xml
<PackageReference Include="Microsoft.AspNetCore.Identity.EntityFrameworkCore" Version="9.0.0" />
```

**NBT.Application**:
```xml
<PackageReference Include="AutoMapper" Version="13.0.0" />
<PackageReference Include="FluentValidation" Version="11.9.0" />
<PackageReference Include="FluentValidation.DependencyInjectionExtensions" Version="11.9.0" />
<PackageReference Include="Microsoft.Extensions.Logging.Abstractions" Version="9.0.0" />
```

**NBT.Infrastructure**:
```xml
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="9.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="9.0.0" />
<PackageReference Include="Microsoft.AspNetCore.Identity.EntityFrameworkCore" Version="9.0.0" />
```

**NBT.WebAPI**:
```xml
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="9.0.0" />
<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="9.0.0" />
<PackageReference Include="Swashbuckle.AspNetCore" Version="6.5.0" />
<PackageReference Include="Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore" Version="9.0.0" />
```

**NBT.WebUI**:
```xml
<PackageReference Include="Microsoft.FluentUI.AspNetCore.Components" Version="4.9.0" />
<PackageReference Include="Microsoft.AspNetCore.Components.Authorization" Version="9.0.0" />
```

### Packages to Add (Phase 1-10)

**For Excel Import/Export**:
```xml
<PackageReference Include="ClosedXML" Version="0.102.1" />
<PackageReference Include="EPPlus" Version="7.0.0" />
```

**For PDF Generation**:
```xml
<PackageReference Include="QuestPDF" Version="2023.12.0" />
```

**For Testing**:
```xml
<PackageReference Include="xUnit" Version="2.6.2" />
<PackageReference Include="xUnit.runner.visualstudio" Version="2.5.4" />
<PackageReference Include="Moq" Version="4.20.70" />
<PackageReference Include="FluentAssertions" Version="6.12.0" />
<PackageReference Include="Microsoft.AspNetCore.Mvc.Testing" Version="9.0.0" />
<PackageReference Include="bUnit" Version="1.26.64" />
```

---

## 🚨 COMMON ISSUES & TROUBLESHOOTING

### Issue 1: Database Connection Failed

**Error**:
```
A network-related or instance-specific error occurred while establishing a connection to SQL Server.
```

**Solution**:
```bash
# Check if SQL Server LocalDB is running
sqllocaldb info

# Start LocalDB if stopped
sqllocaldb start MSSQLLocalDB

# Verify connection
sqlcmd -S "(localdb)\mssqllocaldb" -Q "SELECT @@VERSION"
```

### Issue 2: Migration Failed

**Error**:
```
Unable to create an object of type 'ApplicationDbContext'.
```

**Solution**:
```bash
# Ensure you're in the correct directory
cd src/NBT.WebAPI

# Rebuild project
dotnet build

# Try migration again
dotnet ef database update --verbose
```

### Issue 3: Port Already in Use

**Error**:
```
Failed to bind to address http://localhost:5000: address already in use.
```

**Solution**:
```bash
# Find process using port 5000
netstat -ano | findstr :5000

# Kill the process (replace PID)
taskkill /PID <process_id> /F

# Or change port in launchSettings.json
```

### Issue 4: Blazor Hot Reload Not Working

**Solution**:
```bash
# Disable hot reload temporarily
set DOTNET_WATCH_RESTART_ON_RUDE_EDIT=true

# Or restart with watch
dotnet watch run
```

### Issue 5: Fluent UI Components Not Rendering

**Solution**:

Check `_Imports.razor`:
```csharp
@using Microsoft.FluentUI.AspNetCore.Components
```

Check `Program.cs`:
```csharp
builder.Services.AddFluentUIComponents();
```

### Issue 6: JWT Token Expired

**Solution**:

Clear browser storage:
```javascript
// Open browser console (F12)
localStorage.clear();
sessionStorage.clear();
```

Then login again.

### Issue 7: CORS Errors

**Error**:
```
Access to fetch at 'http://localhost:5000/api/...' from origin 'https://localhost:5003' has been blocked by CORS policy
```

**Solution**:

Verify CORS configuration in `NBT.WebAPI/Program.cs`:
```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient", policy =>
    {
        policy.WithOrigins("https://localhost:5003", "http://localhost:5002")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

app.UseCors("AllowBlazorClient");
```

---

## 🔐 SECURITY NOTES

### Development Secrets

**NEVER commit these to source control**:
- JWT Secret Key
- Database passwords
- EasyPay API keys (when added)
- Azure connection strings

**Use User Secrets** (Development):
```bash
# Initialize user secrets
cd src/NBT.WebAPI
dotnet user-secrets init

# Set JWT secret
dotnet user-secrets set "Jwt:SecretKey" "your-super-secret-key-minimum-32-characters"

# Set database password (if using SQL auth)
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=...;Password=YourPassword"
```

**Use Azure Key Vault** (Production):
```json
{
  "KeyVault": {
    "VaultUri": "https://your-keyvault.vault.azure.net/"
  }
}
```

### Default Credentials

**Admin User** (Development Only):
- Email: `admin@nbt.ac.za`
- Password: `Admin@123`

**⚠️ Change this password before deploying to production!**

---

## 📝 NEXT STEPS

After verifying the shell works correctly, proceed with implementation:

### Week 1: Foundation & Domain Setup
1. Create 9 missing entities (Student, Registration, Payment, etc.)
2. Implement value objects (NBTNumber, SAIDNumber) with Luhn validation
3. Create EF Core configurations
4. Generate and apply migration

### Week 2: Student Management Module
1. Create StudentService with CRUD operations
2. Implement NBT number generator
3. Build StudentsController with 9 endpoints
4. Create admin UI pages for student management

### Weeks 3-12: Complete Remaining Modules
- Registration wizard
- Payment integration (EasyPay)
- Venue and room management
- Test sessions
- Results import
- Dashboards and reporting

**Refer to**: `specs/002-nbt-integrated-system/plan.md` for complete roadmap

---

## 📚 USEFUL COMMANDS REFERENCE

### Git Commands
```bash
# Check status
git status

# Create feature branch
git checkout -b feature/student-module

# Commit changes
git add .
git commit -m "Add Student entity and service"

# Push to remote
git push origin feature/student-module
```

### .NET CLI Commands
```bash
# Build
dotnet build
dotnet build --configuration Release

# Run
dotnet run
dotnet watch run  # With hot reload

# Clean
dotnet clean

# Restore
dotnet restore

# Publish
dotnet publish -c Release -o ./publish
```

### EF Core Commands
```bash
# Add migration
dotnet ef migrations add MigrationName --project src/NBT.Infrastructure --startup-project src/NBT.WebAPI

# Update database
dotnet ef database update --project src/NBT.Infrastructure --startup-project src/NBT.WebAPI

# Remove last migration
dotnet ef migrations remove --project src/NBT.Infrastructure --startup-project src/NBT.WebAPI

# List migrations
dotnet ef migrations list --project src/NBT.Infrastructure --startup-project src/NBT.WebAPI

# Generate SQL script
dotnet ef migrations script --project src/NBT.Infrastructure --startup-project src/NBT.WebAPI
```

### Testing Commands (Coming Soon)
```bash
# Run all tests
dotnet test

# Run with detailed output
dotnet test --verbosity normal

# Run specific test project
dotnet test tests/NBT.Domain.Tests/

# Run with coverage
dotnet test --collect:"XPlat Code Coverage"
```

---

## 🎯 SUCCESS CRITERIA

Your development environment is ready when:

- ✅ `dotnet build` succeeds without errors
- ✅ API runs on port 5000/5001
- ✅ Blazor UI runs on port 5002/5003
- ✅ Database has 6 tables with seed data
- ✅ Can login as admin@nbt.ac.za
- ✅ All 13 existing pages load correctly
- ✅ Fluent UI theme renders properly
- ✅ No console errors in browser
- ✅ API health check returns "Healthy"
- ✅ Swagger UI accessible at /swagger

**If all criteria met**: 🎉 **You're ready to start development!**

---

## 📞 SUPPORT & RESOURCES

### Documentation
- [Implementation Plan](./plan.md)
- [Contracts Document](./contracts.md)
- [Project Review](./review.md)
- [Constitution](../../CONSTITUTION.md)
- [Database Documentation](../../database-scripts/README.md)

### External Resources
- [.NET 9 Documentation](https://docs.microsoft.com/dotnet/core/whats-new/dotnet-9)
- [EF Core 9 Documentation](https://docs.microsoft.com/ef/core/)
- [Blazor Documentation](https://docs.microsoft.com/aspnet/core/blazor/)
- [Fluent UI Blazor](https://www.fluentui-blazor.net/)
- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)

### Team Communication
- Create GitHub issues for bugs
- Use pull requests for code reviews
- Follow the implementation plan phases
- Update documentation as you go

---

**QUICKSTART VERSION**: 1.0  
**LAST UPDATED**: 2025-11-08  
**STATUS**: ✅ READY FOR DEVELOPMENT

**Happy Coding! 🚀**
