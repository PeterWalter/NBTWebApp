# GitHub Workflow Guide for NBT Web Application

## Overview
This guide describes the standardized Git workflow for the NBT Web Application project, including phase-based branching, automated builds, and merge procedures.

## Workflow Principles

1. **Main Branch Protection**: The `main` branch contains production-ready code
2. **Phase-Based Development**: Each major phase gets its own feature branch
3. **Build Before Push**: All code must build successfully before pushing
4. **Test Before Merge**: All tests must pass before merging to main
5. **Clean History**: Use meaningful commit messages and merge commits

## Scripts Available

### 1. `git-workflow.ps1` - Full Workflow Management

#### Start a New Phase
```powershell
.\git-workflow.ps1 -Action start-phase -PhaseName "venue-management"
```
This will:
- Switch to main branch
- Pull latest changes
- Create a new branch: `phase/venue-management`
- Switch to the new branch

#### Complete a Phase
```powershell
.\git-workflow.ps1 -Action complete-phase -PhaseName "venue-management"
```
This will:
- Commit any uncommitted changes
- Build the solution (fails if build errors)
- Run all tests (fails if tests fail)
- Push phase branch to origin
- Switch to main
- Merge phase branch into main
- Push main to origin
- Delete the phase branch (local and remote)

#### Push Changes During Development
```powershell
.\git-workflow.ps1 -Action push-changes -CommitMessage "Add venue capacity validation"
```
This will:
- Add all changes
- Commit with provided message
- Build the solution
- Push to current branch

#### Check Status
```powershell
.\git-workflow.ps1 -Action status
```

### 2. `quick-push.ps1` - Fast Build and Push

```powershell
.\quick-push.ps1 "Your commit message"
```
This will:
- Build the solution
- Add and commit all changes
- Push to current branch

## Standard Workflow

### Starting a New Phase

```powershell
# Example: Starting Venue Management Phase
.\git-workflow.ps1 -Action start-phase -PhaseName "venue-management"

# Now you're on branch: phase/venue-management
# Make your changes...
```

### During Development

```powershell
# Option 1: Quick push after making changes
.\quick-push.ps1 "Implement venue CRUD operations"

# Option 2: Use full workflow script
.\git-workflow.ps1 -Action push-changes -CommitMessage "Add room capacity tracking"
```

### Completing a Phase

```powershell
# When phase is complete and tested
.\git-workflow.ps1 -Action complete-phase -PhaseName "venue-management"

# This merges to main and pushes
```

## Phase Naming Conventions

Use lowercase with hyphens for phase names:

- `registration-wizard`
- `booking-payment`
- `staff-dashboard`
- `venue-management`
- `reporting-analytics`
- `security-roles`
- `mudblazor-migration`

## Manual Git Commands

If you prefer manual control:

### Start Phase
```bash
git checkout main
git pull origin main
git checkout -b phase/my-phase
```

### During Development
```bash
git add .
git commit -m "Descriptive message"
dotnet build NBTWebApp.sln
git push origin phase/my-phase
```

### Complete Phase
```bash
# Build and test first
dotnet build NBTWebApp.sln
dotnet test

# Merge to main
git checkout main
git pull origin main
git merge --no-ff phase/my-phase -m "Merge phase: my-phase"
git push origin main

# Clean up
git branch -d phase/my-phase
git push origin --delete phase/my-phase
```

## Current Phase Structure

### Completed Phases (should be on main)
- ✅ Initial Setup
- ✅ Database Schema
- ✅ Authentication
- ✅ API Foundation
- ✅ MudBlazor to FluentUI Migration

### Active/Upcoming Phases
- 🔄 Registration Wizard (needs fixes)
- ⏳ Booking & Payment
- ⏳ Staff Dashboard
- ⏳ Venue Management
- ⏳ Reporting & Analytics
- ⏳ Security & Roles

## Best Practices

1. **Always build before pushing**
   ```powershell
   dotnet build NBTWebApp.sln
   ```

2. **Run tests before completing a phase**
   ```powershell
   dotnet test
   ```

3. **Use descriptive commit messages**
   - ✅ "Add venue capacity validation with business rules"
   - ✅ "Fix registration wizard step navigation"
   - ❌ "Fixed bug"
   - ❌ "Update"

4. **Commit related changes together**
   - Group related files in a single commit
   - Don't mix unrelated changes

5. **Pull before starting work**
   ```bash
   git checkout main
   git pull origin main
   ```

6. **Keep phases focused**
   - One phase = one major feature
   - Don't mix unrelated features in a phase

## Troubleshooting

### Build Fails
```powershell
# Check for errors
dotnet build NBTWebApp.sln

# Clean and rebuild
dotnet clean
dotnet build
```

### Merge Conflicts
```powershell
# Manually resolve conflicts in VS Code or Visual Studio
# Then:
git add .
git commit -m "Resolve merge conflicts"
```

### Forgot to Create Phase Branch
```powershell
# Create branch from current state
git checkout -b phase/my-phase

# Or stash changes and start properly
git stash
.\git-workflow.ps1 -Action start-phase -PhaseName "my-phase"
git stash pop
```

## Quick Reference

| Task | Command |
|------|---------|
| Start phase | `.\git-workflow.ps1 -Action start-phase -PhaseName "phase-name"` |
| Push changes | `.\quick-push.ps1 "message"` |
| Complete phase | `.\git-workflow.ps1 -Action complete-phase -PhaseName "phase-name"` |
| Check status | `.\git-workflow.ps1 -Action status` |
| Build only | `dotnet build NBTWebApp.sln` |
| Test only | `dotnet test` |

## Integration with CI/CD

Once GitHub Actions are configured:
- Every push triggers automated build
- Pull requests require passing tests
- Main branch protected from direct pushes
- Automated deployment on successful main merge

## Notes

- Always ensure `appsettings.json` doesn't contain sensitive data before pushing
- Use `.gitignore` to exclude local configuration files
- Phase branches are temporary and deleted after merge
- Keep main branch stable and deployable at all times
