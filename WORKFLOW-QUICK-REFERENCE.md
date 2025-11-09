# GitHub Workflow - Quick Reference Card

## 🚀 Essential Commands

### Start New Phase
```powershell
.\git-workflow.ps1 -Action start-phase -PhaseName "phase-name"
```
**Examples:**
- `.\git-workflow.ps1 -Action start-phase -PhaseName "booking-frontend"`
- `.\git-workflow.ps1 -Action start-phase -PhaseName "staff-dashboard"`

### Push Changes (Daily Work)
```powershell
.\quick-push.ps1 "Your descriptive commit message"
```
**Examples:**
- `.\quick-push.ps1 "Add booking form validation"`
- `.\quick-push.ps1 "Fix venue capacity calculation"`

### Complete Phase (Merge to Main)
```powershell
.\git-workflow.ps1 -Action complete-phase -PhaseName "phase-name"
```
**Examples:**
- `.\git-workflow.ps1 -Action complete-phase -PhaseName "booking-frontend"`
- `.\git-workflow.ps1 -Action complete-phase -PhaseName "staff-dashboard"`

### Check Status
```powershell
.\git-workflow.ps1 -Action status
```

---

## 📋 Workflow Steps

### Standard Development Cycle
```
1. START PHASE
   .\git-workflow.ps1 -Action start-phase -PhaseName "my-phase"
   ↓
2. DEVELOP & PUSH (repeat as needed)
   .\quick-push.ps1 "Add feature X"
   .\quick-push.ps1 "Fix bug Y"
   .\quick-push.ps1 "Update tests"
   ↓
3. COMPLETE PHASE
   .\git-workflow.ps1 -Action complete-phase -PhaseName "my-phase"
   ↓
4. REPEAT for next phase
```

---

## 🎯 Phase Names (Use These)

### Frontend Phases
- `booking-frontend`
- `staff-dashboard`
- `reports-frontend`
- `student-profile`
- `admin-panel`

### Backend Phases
- `booking-api`
- `payment-integration`
- `results-processing`
- `venue-api`
- `reporting-api`

### System Phases
- `security-enhancement`
- `testing-qa`
- `performance-optimization`
- `deployment-prep`
- `documentation`

---

## ✅ Best Practices

### Commit Messages
**Good:**
- ✅ "Add booking validation with NBT number check"
- ✅ "Implement venue capacity tracking"
- ✅ "Fix registration wizard step navigation"

**Bad:**
- ❌ "Update"
- ❌ "Fix bug"
- ❌ "Changes"

### Phase Management
- ✅ One phase = One major feature
- ✅ Keep phases focused
- ✅ Complete phases before starting new ones
- ✅ Always start with start-phase command

### Code Quality
- ✅ Build always passes (automatic check)
- ✅ Tests pass before merge
- ✅ Main branch always stable
- ✅ Descriptive commit messages

---

## 🔧 Common Scenarios

### Scenario 1: Starting Fresh Work
```powershell
# Start new phase
.\git-workflow.ps1 -Action start-phase -PhaseName "booking-frontend"

# You're now on: phase/booking-frontend
# Make changes, then push
.\quick-push.ps1 "Initial booking page layout"
```

### Scenario 2: Daily Development
```powershell
# Morning: Check status
.\git-workflow.ps1 -Action status

# Work on features, push often
.\quick-push.ps1 "Add date picker component"
.\quick-push.ps1 "Implement form validation"
.\quick-push.ps1 "Add unit tests"
```

### Scenario 3: Finishing Phase
```powershell
# Push final changes
.\quick-push.ps1 "Final phase cleanup and documentation"

# Complete and merge
.\git-workflow.ps1 -Action complete-phase -PhaseName "booking-frontend"
```

---

## 🚨 Troubleshooting

### Build Fails
```powershell
# Clean and rebuild
dotnet clean
dotnet build NBTWebApp.sln
```

### Wrong Branch
```powershell
# Check current branch
git branch

# Switch to correct branch
git checkout phase/correct-phase
```

### Uncommitted Changes
```powershell
# Stash changes
git stash

# Do what you need to do
# ...

# Apply stashed changes
git stash pop
```

### Need to Update from Main
```powershell
# From your phase branch
git checkout main
git pull origin main
git checkout phase/your-phase
git merge main
```

---

## 📊 Current Status Check

```powershell
# Quick status
.\git-workflow.ps1 -Action status

# Detailed git status
git status
git log --oneline -5
git branch -a
```

---

## 📁 Project Structure

```
NBTWebApp/
├── git-workflow.ps1           ← Master workflow script
├── quick-push.ps1             ← Quick push utility
├── GITHUB-WORKFLOW-GUIDE.md   ← Full documentation
└── src/
    ├── NBT.Domain/
    ├── NBT.Application/
    ├── NBT.Infrastructure/
    ├── NBT.WebAPI/
    └── NBT.WebUI/
```

---

## 🌐 Repository Info

- **URL**: https://github.com/PeterWalter/NBTWebApp.git
- **Main Branch**: `main` (protected, stable)
- **Phase Branches**: `phase/*` (temporary, deleted after merge)

---

## 📞 Need Help?

1. **Full Documentation**: `GITHUB-WORKFLOW-GUIDE.md`
2. **Implementation Details**: `WORKFLOW-AUTOMATION-COMPLETE.md`
3. **Check Status**: `.\git-workflow.ps1 -Action status`

---

## 🎓 Remember

1. **Always use scripts** for phase management
2. **Build before push** (automatic with scripts)
3. **Test before complete** (automatic with scripts)
4. **Main always stable** (enforced by workflow)
5. **Descriptive commits** (helps everyone)

---

## ⚡ Ultra-Quick Reference

| Task | Command |
|------|---------|
| Start phase | `.\git-workflow.ps1 -Action start-phase -PhaseName "name"` |
| Push changes | `.\quick-push.ps1 "message"` |
| Complete phase | `.\git-workflow.ps1 -Action complete-phase -PhaseName "name"` |
| Check status | `.\git-workflow.ps1 -Action status` |

---

**Keep this file open while working! 📌**
