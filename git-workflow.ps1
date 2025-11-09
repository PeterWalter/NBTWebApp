# Git Workflow Automation for NBT Web Application
# Manages branching, building, testing, and merging for phase-based development

param(
    [Parameter(Mandatory=$true)]
    [ValidateSet('start-phase', 'complete-phase', 'push-changes', 'status')]
    [string]$Action,
    
    [Parameter(Mandatory=$false)]
    [string]$PhaseName = "",
    
    [Parameter(Mandatory=$false)]
    [string]$CommitMessage = ""
)

$ErrorActionPreference = "Stop"

function Write-Status {
    param([string]$Message, [string]$Type = "Info")
    $color = switch($Type) {
        "Success" { "Green" }
        "Error" { "Red" }
        "Warning" { "Yellow" }
        default { "Cyan" }
    }
    Write-Host "[$Type] $Message" -ForegroundColor $color
}

function Test-GitStatus {
    $status = git status --porcelain
    return $status.Length -gt 0
}

function Invoke-Build {
    Write-Status "Building solution..." "Info"
    dotnet build NBTWebApp.sln --no-incremental
    if ($LASTEXITCODE -ne 0) {
        throw "Build failed"
    }
    Write-Status "Build successful!" "Success"
}

function Invoke-Tests {
    Write-Status "Running tests..." "Info"
    $testProjects = Get-ChildItem -Path "src" -Filter "*.Tests.csproj" -Recurse
    
    if ($testProjects.Count -eq 0) {
        Write-Status "No test projects found, skipping tests" "Warning"
        return
    }
    
    foreach ($testProject in $testProjects) {
        Write-Status "Running tests in $($testProject.Name)..." "Info"
        dotnet test $testProject.FullName --no-build --verbosity minimal
        if ($LASTEXITCODE -ne 0) {
            throw "Tests failed in $($testProject.Name)"
        }
    }
    Write-Status "All tests passed!" "Success"
}

function Start-Phase {
    param([string]$PhaseName)
    
    Write-Status "Starting new phase: $PhaseName" "Info"
    
    # Ensure we're on main and up to date
    $currentBranch = git rev-parse --abbrev-ref HEAD
    if ($currentBranch -ne "main") {
        Write-Status "Switching to main branch..." "Info"
        git checkout main
    }
    
    Write-Status "Pulling latest changes from origin..." "Info"
    git pull origin main
    
    # Create new branch for phase
    $branchName = "phase/$PhaseName"
    Write-Status "Creating branch: $branchName" "Info"
    git checkout -b $branchName
    
    Write-Status "Phase branch created successfully!" "Success"
    Write-Status "Branch: $branchName" "Info"
}

function Complete-Phase {
    param([string]$PhaseName)
    
    Write-Status "Completing phase: $PhaseName" "Info"
    
    # Get current branch
    $currentBranch = git rev-parse --abbrev-ref HEAD
    
    if (-not $currentBranch.StartsWith("phase/")) {
        throw "Not on a phase branch. Current branch: $currentBranch"
    }
    
    # Check for uncommitted changes
    if (Test-GitStatus) {
        Write-Status "Uncommitted changes detected. Committing..." "Warning"
        git add .
        git commit -m "Complete phase: $PhaseName"
    }
    
    # Build and test
    Invoke-Build
    Invoke-Tests
    
    # Push phase branch
    Write-Status "Pushing phase branch to origin..." "Info"
    git push origin $currentBranch
    
    # Switch to main and merge
    Write-Status "Switching to main branch..." "Info"
    git checkout main
    
    Write-Status "Pulling latest changes..." "Info"
    git pull origin main
    
    Write-Status "Merging phase branch into main..." "Info"
    git merge --no-ff $currentBranch -m "Merge phase: $PhaseName"
    
    # Push to main
    Write-Status "Pushing to main..." "Info"
    git push origin main
    
    # Optionally delete the phase branch
    Write-Status "Deleting local phase branch..." "Info"
    git branch -d $currentBranch
    
    Write-Status "Deleting remote phase branch..." "Info"
    git push origin --delete $currentBranch
    
    Write-Status "Phase completed and merged successfully!" "Success"
}

function Push-Changes {
    param([string]$Message)
    
    Write-Status "Pushing changes..." "Info"
    
    if (-not (Test-GitStatus)) {
        Write-Status "No changes to commit" "Warning"
        return
    }
    
    # Add all changes
    git add .
    
    # Commit with message
    if ([string]::IsNullOrWhiteSpace($Message)) {
        $Message = "Update: $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')"
    }
    
    Write-Status "Committing with message: $Message" "Info"
    git commit -m $Message
    
    # Build before pushing
    Invoke-Build
    
    # Get current branch
    $currentBranch = git rev-parse --abbrev-ref HEAD
    
    # Push to origin
    Write-Status "Pushing to origin/$currentBranch..." "Info"
    git push origin $currentBranch
    
    Write-Status "Changes pushed successfully!" "Success"
}

function Show-Status {
    Write-Status "Current Git Status:" "Info"
    git status
    
    Write-Status "`nRecent commits:" "Info"
    git log --oneline -5
    
    Write-Status "`nBranches:" "Info"
    git branch -a
}

# Main execution
try {
    switch ($Action) {
        'start-phase' {
            if ([string]::IsNullOrWhiteSpace($PhaseName)) {
                throw "PhaseName is required for start-phase action"
            }
            Start-Phase -PhaseName $PhaseName
        }
        'complete-phase' {
            if ([string]::IsNullOrWhiteSpace($PhaseName)) {
                throw "PhaseName is required for complete-phase action"
            }
            Complete-Phase -PhaseName $PhaseName
        }
        'push-changes' {
            Push-Changes -Message $CommitMessage
        }
        'status' {
            Show-Status
        }
    }
}
catch {
    Write-Status "Error: $_" "Error"
    exit 1
}
