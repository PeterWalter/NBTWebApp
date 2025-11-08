# NBT WebApp - Quick Reference Guide

## 🚀 Quick Start

```powershell
# Automated (Recommended)
.\RUN-APP.ps1

# Manual
# Terminal 1:
cd src\NBT.WebAPI && dotnet run --urls http://localhost:5000

# Terminal 2:
cd src\NBT.WebUI && dotnet run --urls http://localhost:5001
```

**Open**: http://localhost:5001

---

## 🔐 Login Credentials

| Role | Email | Password |
|------|-------|----------|
| Admin | admin@nbt.ac.za | Admin@123 |
| Student | student@test.com | Student@123 |
| Institution | institution@test.com | Institution@123 |

---

## 🌐 URLs

| Service | URL | Purpose |
|---------|-----|---------|
| WebUI | http://localhost:5001 | Frontend |
| WebAPI | http://localhost:5000 | Backend |
| Swagger | http://localhost:5000/swagger | API Docs |

---

## 📁 Project Structure

```
NBTWebApp/
├── src/
│   ├── NBT.Domain/         # Entities, Enums
│   ├── NBT.Application/    # Business Logic, DTOs
│   ├── NBT.Infrastructure/ # Database, Services
│   ├── NBT.WebAPI/         # REST API
│   └── NBT.WebUI/          # Blazor Frontend
├── docs/                   # Documentation
├── database-scripts/       # SQL Scripts
└── specs/                  # Specifications
```

---

## 🎯 Available Features

### Public (No Login)
- Home, About, Applicants, Educators, Institutions
- News/Announcements
- Contact Form
- Resources (view)

### Admin (Login: admin@nbt.ac.za)
- Dashboard
- Manage Announcements (Create/Edit/Delete) ✅
- Manage Content ⏳
- Manage Resources ⏳
- View Inquiries ⏳
- User Management ⏳

---

## 🔧 Common Commands

```powershell
# Build
dotnet build

# Clean & Build
dotnet clean && dotnet build

# Run Tests
dotnet test

# Database Update
cd src\NBT.Infrastructure
dotnet ef database update --startup-project ..\NBT.WebAPI

# Add Migration
dotnet ef migrations add MigrationName --startup-project ..\NBT.WebAPI
```

---

## 🐛 Troubleshooting

### Port Already in Use
```powershell
Get-NetTCPConnection -LocalPort 5000 | ForEach-Object { Stop-Process -Id $_.OwningProcess -Force }
Get-NetTCPConnection -LocalPort 5001 | ForEach-Object { Stop-Process -Id $_.OwningProcess -Force }
```

### Reconnection Issues
1. Refresh page (F5)
2. Clear cache (Ctrl+Shift+Delete)
3. Restart both apps

### Build Errors
```powershell
dotnet clean
dotnet restore
dotnet build
```

---

## 📚 Documentation

| File | Purpose |
|------|---------|
| RUNNING-THE-APP.md | Complete running guide |
| BLAZOR-FIXES-COMPLETE.md | Technical fixes |
| SESSION-COMPLETE-SUMMARY.md | Session summary |
| PROGRESS-UPDATE.md | Current progress |
| QUICK-REFERENCE.md | This file |

---

## ✅ Status Summary

| Phase | Status | Completion |
|-------|--------|------------|
| 1-5 | ✅ Complete | 100% |
| 6 | 🔄 In Progress | 85% |
| 7 | 🔄 In Progress | 70% |
| 8 | ⏳ Pending | 10% |

**Overall**: ~78% Complete

---

## 🎯 Next Tasks

1. Complete admin interfaces
2. Add contact form submission
3. Implement file upload
4. User management
5. Testing suite

---

## 📞 Support

- Check browser console (F12)
- Review documentation
- Check GitHub issues
- Contact development team

---

**Last Updated**: 2025-01-08
**Version**: 1.0.0
**Status**: Stable ✅
