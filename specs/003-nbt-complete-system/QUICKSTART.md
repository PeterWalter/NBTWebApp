# NBT Web Application - Developer Quickstart

## 🚀 Getting Started

This quickstart guide will help you set up the NBT Web Application development environment and understand the current project state.

## Prerequisites

### Required Software
- **Visual Studio 2022** (v17.8 or later) or **VS Code** with C# Dev Kit
- **.NET 9.0 SDK** ([Download](https://dotnet.microsoft.com/download/dotnet/9.0))
- **SQL Server 2019+** or **SQL Server Express** ([Download](https://www.microsoft.com/sql-server/sql-server-downloads))
- **Node.js 18+** (for frontend tooling) ([Download](https://nodejs.org/))
- **Git** ([Download](https://git-scm.com/downloads))

### Optional Tools
- **Azure Data Studio** (database management)
- **Postman** or **Insomnia** (API testing)
- **SQL Server Management Studio** (SSMS)

## Initial Setup

### 1. Clone Repository
```bash
git clone https://github.com/[your-org]/NBTWebApp.git
cd NBTWebApp
```

### 2. Verify .NET Version
```bash
dotnet --version
# Should show 9.0.x or higher
```

### 3. Restore NuGet Packages
```bash
cd src
dotnet restore
```

### 4. Configure Database Connection

Open `src/NBTWebApp.API/appsettings.json` and update the connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=NBTWebAppDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

For SQL Server Express, use:
```json
"DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=NBTWebAppDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
```

For Azure SQL or remote SQL Server:
```json
"DefaultConnection": "Server=your-server.database.windows.net;Database=NBTWebAppDb;User Id=your-username;Password=your-password;Encrypt=True;TrustServerCertificate=False;"
```

### 5. Apply Database Migrations

```bash
cd src/NBTWebApp.Infrastructure

# Create database and apply migrations
dotnet ef database update --startup-project ../NBTWebApp.API

# If migrations don't exist yet, create initial migration
dotnet ef migrations add InitialCreate --startup-project ../NBTWebApp.API
dotnet ef database update --startup-project ../NBTWebApp.API
```

### 6. Configure JWT Settings

Update `appsettings.json` with JWT configuration:

```json
{
  "Jwt": {
    "Key": "your-secret-key-min-32-characters-long",
    "Issuer": "NBTWebApp",
    "Audience": "NBTWebApp",
    "ExpiryInMinutes": 60
  }
}
```

**⚠️ Important**: In production, store JWT key in Azure Key Vault or environment variables, never in appsettings.json.

### 7. Seed Test Data (Optional)

```bash
cd src/NBTWebApp.API
dotnet run --seed-data
```

This will create:
- Sample venues
- Test dates
- Sample users (Admin, Staff, Student)
- Sample test data

## Running the Application

### Option 1: Visual Studio 2022

1. Open `NBTWebApp.sln`
2. Set **Multiple Startup Projects**:
   - Right-click solution → Properties → Multiple startup projects
   - Set both `NBTWebApp.API` and `NBTWebApp.Client` to **Start**
3. Press **F5** to run

The application will open:
- **API**: https://localhost:7001
- **Client**: https://localhost:7002
- **Swagger**: https://localhost:7001/swagger

### Option 2: Command Line

**Terminal 1 - API**:
```bash
cd src/NBTWebApp.API
dotnet run
```

**Terminal 2 - Client**:
```bash
cd src/NBTWebApp.Client
dotnet run
```

### Option 3: Using PowerShell Scripts

We provide convenient PowerShell scripts in the root directory:

```powershell
# Start both API and Client
.\start-app.ps1

# Start with clean build
.\start-clean.ps1

# Start with authentication enabled
.\start-with-auth.ps1
```

## Verify Installation

### 1. Check API Health
Open browser to: https://localhost:7001/health

Expected response:
```json
{
  "status": "Healthy",
  "results": {
    "database": "Healthy"
  }
}
```

### 2. Check Swagger Documentation
Open: https://localhost:7001/swagger

You should see all API endpoints documented.

### 3. Check Blazor Client
Open: https://localhost:7002

You should see the landing page or registration wizard.

### 4. Test Login
Navigate to login page and use test credentials:

**Admin**:
- Email: `admin@nbt.ac.za`
- Password: `Admin@123`

**Staff**:
- Email: `staff@nbt.ac.za`
- Password: `Staff@123`

**Student**:
- Email: `student@test.com`
- Password: `Student@123`

## Project Structure

```
NBTWebApp/
├── src/
│   ├── NBTWebApp.Domain/           # Domain entities, value objects, enums
│   ├── NBTWebApp.Application/      # Business logic, services, DTOs, interfaces
│   ├── NBTWebApp.Infrastructure/   # EF Core, repositories, external services
│   ├── NBTWebApp.API/             # Web API controllers, middleware
│   └── NBTWebApp.Client/          # Blazor WebAssembly app
├── tests/
│   ├── NBTWebApp.Domain.Tests/
│   ├── NBTWebApp.Application.Tests/
│   ├── NBTWebApp.Infrastructure.Tests/
│   ├── NBTWebApp.API.Tests/
│   └── NBTWebApp.Client.Tests/
├── specs/                          # Specifications
│   └── 003-nbt-complete-system/   # Current specification
├── database-scripts/               # SQL scripts
└── .github/
    └── workflows/                 # CI/CD pipelines
```

## Current Implementation Status

### ✅ Completed
- Clean Architecture structure
- EF Core with basic entities
- JWT authentication setup
- Basic API endpoints
- Blazor WebAssembly shell
- Database migrations

### 🚧 In Progress
- Registration wizard (partial)
- Student management (partial)
- Basic dashboards

### ❌ Not Started
- Complete registration wizard (3-step)
- Booking and payment system
- Results management
- Venue management
- Reporting and analytics
- Landing page with menus
- Fluent UI migration (remove MudBlazor)
- Special session workflow
- Document upload system
- Notification system

## Development Workflow

### 1. Pick a Task
Review `specs/003-nbt-complete-system/TASKS.md` and pick a task marked "Not Started".

### 2. Create Feature Branch
```bash
git checkout -b feature/TASK-XXX-description
```

### 3. Implement Feature
Follow the specifications in:
- `CONSTITUTION.md` - Rules and standards
- `SPECIFICATION.md` - Functional requirements
- `CONTRACTS.md` - Data contracts
- `PLAN.md` - Implementation approach

### 4. Write Tests
- Unit tests in appropriate `*.Tests` project
- Aim for 80%+ code coverage
- Include edge cases

### 5. Run Tests
```bash
cd tests
dotnet test
```

### 6. Commit Changes
```bash
git add .
git commit -m "feat(TASK-XXX): Description of changes"
```

Follow commit message convention:
- `feat`: New feature
- `fix`: Bug fix
- `refactor`: Code refactoring
- `test`: Adding tests
- `docs`: Documentation
- `chore`: Maintenance

### 7. Push and Create PR
```bash
git push origin feature/TASK-XXX-description
```

Create Pull Request on GitHub with:
- Description of changes
- Link to task
- Test results
- Screenshots (if UI changes)

## Common Commands

### Database
```bash
# Add new migration
dotnet ef migrations add MigrationName --startup-project ../NBTWebApp.API

# Update database
dotnet ef database update --startup-project ../NBTWebApp.API

# Drop database (careful!)
dotnet ef database drop --startup-project ../NBTWebApp.API

# Generate SQL script
dotnet ef migrations script --startup-project ../NBTWebApp.API
```

### Build
```bash
# Build solution
dotnet build

# Clean build
dotnet clean
dotnet build

# Build for production
dotnet build --configuration Release
```

### Run Tests
```bash
# Run all tests
dotnet test

# Run with coverage
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover

# Run specific test project
dotnet test tests/NBTWebApp.Application.Tests
```

### Code Formatting
```bash
# Format all code
dotnet format

# Check format without changing
dotnet format --verify-no-changes
```

## Troubleshooting

### Issue: Database Connection Failed
**Solution**: 
- Verify SQL Server is running
- Check connection string in appsettings.json
- Ensure database name doesn't conflict
- Try `(localdb)\\mssqllocaldb` for LocalDB

### Issue: Migrations Not Applied
**Solution**:
```bash
cd src/NBTWebApp.Infrastructure
dotnet ef database update --startup-project ../NBTWebApp.API --verbose
```

### Issue: Client Won't Start
**Solution**:
- Ensure .NET 9.0 SDK is installed
- Clear obj and bin folders: `dotnet clean`
- Rebuild: `dotnet build`
- Check for port conflicts (7001, 7002)

### Issue: CORS Errors
**Solution**:
- Verify API is running on https://localhost:7001
- Check CORS configuration in `Program.cs`
- Ensure client URL is allowed in CORS policy

### Issue: JWT Authentication Not Working
**Solution**:
- Verify JWT key length (min 32 characters)
- Check token expiry
- Ensure Authorization header format: `Bearer <token>`
- Verify role claims in token

## Next Steps

1. **Review Specifications**: Read through all docs in `specs/003-nbt-complete-system/`
2. **Understand Current State**: Run the app and explore existing features
3. **Pick First Task**: Start with TASK-001 (Project Structure Review) from TASKS.md
4. **Join Team Standup**: Coordinate with team on task assignments
5. **Ask Questions**: Don't hesitate to ask in team chat or create GitHub discussions

## Useful Resources

### Documentation
- [.NET 9.0 Documentation](https://docs.microsoft.com/dotnet)
- [Blazor Documentation](https://docs.microsoft.com/aspnet/core/blazor)
- [Fluent UI Blazor](https://www.fluentui-blazor.net/)
- [EF Core 9.0](https://docs.microsoft.com/ef/core)

### Learning
- [Clean Architecture Pattern](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [CQRS Pattern](https://docs.microsoft.com/azure/architecture/patterns/cqrs)
- [Repository Pattern](https://docs.microsoft.com/aspnet/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application)

### Tools
- [Visual Studio Code Extensions](https://marketplace.visualstudio.com/)
  - C# Dev Kit
  - GitLens
  - REST Client
  - EditorConfig

## Support

- **GitHub Issues**: Report bugs and issues
- **GitHub Discussions**: Ask questions and discuss features
- **Team Chat**: [Your team chat platform]
- **Email**: [Project email]

---

**Happy Coding! 🚀**

Last Updated: 2025-11-09
