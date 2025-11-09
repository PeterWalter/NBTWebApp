# Quick Push Script - Build, Test, and Push Changes
# Usage: .\quick-push.ps1 "Your commit message"

param(
    [Parameter(Mandatory=$false)]
    [string]$CommitMessage = "Update: $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')"
)

$ErrorActionPreference = "Stop"

Write-Host "=== Quick Push Workflow ===" -ForegroundColor Cyan
Write-Host ""

# Check for changes
$changes = git status --porcelain
if ($changes.Length -eq 0) {
    Write-Host "No changes to commit" -ForegroundColor Yellow
    exit 0
}

Write-Host "Changes detected:" -ForegroundColor Green
git status --short

Write-Host ""
Write-Host "Building solution..." -ForegroundColor Cyan
dotnet build NBTWebApp.sln --no-incremental

if ($LASTEXITCODE -ne 0) {
    Write-Host "Build failed! Aborting push." -ForegroundColor Red
    exit 1
}

Write-Host "Build successful!" -ForegroundColor Green
Write-Host ""

# Add and commit
Write-Host "Adding changes..." -ForegroundColor Cyan
git add .

Write-Host "Committing with message: $CommitMessage" -ForegroundColor Cyan
git commit -m $CommitMessage

# Get current branch
$currentBranch = git rev-parse --abbrev-ref HEAD

Write-Host "Pushing to origin/$currentBranch..." -ForegroundColor Cyan
git push origin $currentBranch

if ($LASTEXITCODE -eq 0) {
    Write-Host ""
    Write-Host "Successfully pushed to origin/$currentBranch!" -ForegroundColor Green
} else {
    Write-Host "Push failed!" -ForegroundColor Red
    exit 1
}
