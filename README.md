# NBT Website Rebuild

**National Benchmark Tests Public Website**

A modern, accessible, and responsive web application serving as the primary information portal for applicants, educators, and higher education institutions.

## 🏗️ Architecture

This project follows **Clean Architecture** principles with clear separation of concerns:

```
├── NBT.Domain          (Core business entities, no dependencies)
├── NBT.Application     (Use cases, DTOs, interfaces)
├── NBT.Infrastructure  (Data access, external services)
├── NBT.WebAPI          (REST API endpoints)
└── NBT.WebUI           (Blazor Web App frontend)
```

## 🛠️ Technology Stack

- **Frontend**: Blazor Web Application (Interactive Auto) with .NET 8
- **Backend**: ASP.NET Core Web API 8.0
- **Database**: Microsoft SQL Server 2019+
- **UI Framework**: Fluent UI Blazor Components v4.x
- **Authentication**: ASP.NET Core Identity with JWT
- **Testing**: xUnit, bUnit, Playwright
- **CI/CD**: GitHub Actions, Azure App Service

## 📋 Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (8.0.100 or later)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) (v17.8+) or [Visual Studio Code](https://code.visualstudio.com/)
- [SQL Server 2019+](https://www.microsoft.com/sql-server) or SQL Server Express LocalDB
- [Node.js](https://nodejs.org/) 18.x LTS (for frontend tooling)
- [Azure CLI](https://learn.microsoft.com/cli/azure/) (for deployment)

## 🚀 Getting Started

### 1. Clone the repository

```bash
git clone <repository-url>
cd NBTWebApp
```

### 2. Restore dependencies

```bash
dotnet restore
```

### 3. Update database connection string

Edit `src/NBT.WebAPI/appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=NBTWebsite;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

### 4. Run database migrations

```bash
dotnet ef database update --project src/NBT.Infrastructure --startup-project src/NBT.WebAPI
```

### 5. Run the application

**Option A - Run both API and UI:**

```bash
# Terminal 1 - API
cd src/NBT.WebAPI
dotnet run

# Terminal 2 - UI
cd src/NBT.WebUI
dotnet run
```

**Option B - Using Visual Studio:**
1. Open `NBTWebApp.sln`
2. Set multiple startup projects (NBT.WebAPI and NBT.WebUI)
3. Press F5

### 6. Access the application

- **Frontend**: https://localhost:5001
- **API**: https://localhost:7001
- **Swagger**: https://localhost:7001/swagger

## 🏛️ Project Structure

```
NBTWebApp/
├── src/
│   ├── NBT.Domain/                  # Core business entities
│   │   ├── Entities/                # Domain entities
│   │   ├── Enums/                   # Domain enumerations
│   │   ├── Exceptions/              # Domain exceptions
│   │   └── Common/                  # Base classes, interfaces
│   │
│   ├── NBT.Application/             # Business logic & use cases
│   │   ├── Common/                  # Shared interfaces, models
│   │   ├── ContentPages/            # ContentPage use cases
│   │   ├── Announcements/           # Announcement use cases
│   │   ├── ContactInquiries/        # Contact inquiry use cases
│   │   ├── Authentication/          # Auth use cases
│   │   └── Resources/               # Resource use cases
│   │
│   ├── NBT.Infrastructure/          # External concerns
│   │   ├── Persistence/             # Database context & configs
│   │   ├── Identity/                # ASP.NET Core Identity
│   │   └── Services/                # Email, file storage
│   │
│   ├── NBT.WebAPI/                  # REST API
│   │   ├── Controllers/             # API endpoints
│   │   ├── Middleware/              # Custom middleware
│   │   └── Filters/                 # Action filters
│   │
│   └── NBT.WebUI/                   # Blazor frontend
│       ├── Pages/                   # Razor pages
│       ├── Components/              # Reusable components
│       ├── Services/                # HTTP clients, state
│       └── wwwroot/                 # Static assets
│
├── tests/                           # Test projects
│   ├── NBT.Domain.Tests/
│   ├── NBT.Application.Tests/
│   ├── NBT.Infrastructure.Tests/
│   ├── NBT.WebAPI.Tests/
│   └── NBT.WebUI.Tests/
│
├── specs/                           # Feature specifications
│   └── 001-nbt-website-rebuild/
│       ├── specification.md         # Functional specification
│       ├── plan.md                  # Implementation plan
│       └── tasks.md                 # Task breakdown
│
├── .github/
│   └── workflows/                   # CI/CD pipelines
│
├── NBTWebApp.sln                    # Solution file
├── Directory.Build.props            # Common MSBuild properties
└── README.md                        # This file
```

## 🧪 Testing

### Run all tests

```bash
dotnet test
```

### Run with coverage

```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```

### Run specific test project

```bash
dotnet test tests/NBT.Application.Tests
```

## 📦 Building for Production

```bash
dotnet publish src/NBT.WebAPI -c Release -o ./publish/api
dotnet publish src/NBT.WebUI -c Release -o ./publish/ui
```

## 🚢 Deployment

The application is deployed to Azure using GitHub Actions. See `.github/workflows/` for CI/CD pipelines.

**Environments:**
- **Development**: https://nbt-website-dev.azurewebsites.net
- **Staging**: https://nbt-website-staging.azurewebsites.net
- **Production**: https://www.nbt.ac.za

## 📖 Documentation

- [Specification](specs/001-nbt-website-rebuild/specification.md) - Functional requirements
- [Implementation Plan](specs/001-nbt-website-rebuild/plan.md) - Technical architecture and phases
- [Tasks](specs/001-nbt-website-rebuild/tasks.md) - Detailed task breakdown
- [Constitution](.specify/memory/constitution.md) - Project principles and standards

## 🎯 Key Features

### Public Pages
- 🏠 Landing Page with NBT branding
- ℹ️ About NBT organization and mission
- 🎓 Applicants information and FAQs
- 👨‍🏫 Educators resources and materials
- 🏛️ Institutions guidance and login
- 📰 What's New announcements
- 📞 Contact form with inquiry management

### Technical Features
- ♿ WCAG 2.1 AA accessibility compliance
- 📱 Responsive design (mobile-first)
- 🔒 Secure authentication (JWT + Identity)
- ⚡ Performance optimized (<3s page load)
- 🧪 80%+ code coverage
- 🔐 OWASP Top 10 security compliance

## 🤝 Contributing

1. Create a feature branch from `develop`
2. Follow Clean Architecture principles
3. Ensure all tests pass
4. Maintain 80%+ code coverage
5. Follow C# coding standards
6. Submit pull request for review

## 📝 Code Quality Standards

- ✅ Zero compiler warnings policy
- ✅ Clean Architecture enforcement
- ✅ SOLID principles
- ✅ Dependency injection (constructor injection)
- ✅ XML documentation for public APIs
- ✅ Unit tests for business logic
- ✅ Integration tests for APIs
- ✅ E2E tests for critical paths

## 📜 License

Copyright © 2025 National Benchmark Tests. All rights reserved.

## 📞 Support

For issues or questions, please contact the development team or open an issue in the repository.

---

**Built with ❤️ by the NBT Development Team**
