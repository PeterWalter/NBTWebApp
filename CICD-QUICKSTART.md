# CI/CD Quick Start Guide

## ✅ What's Been Done

The CI/CD pipeline code is complete and pushed to GitHub. The GitHub Actions workflow will run automatically on every push to `main` or `develop` branches.

## 🚀 Next Steps to Enable Deployment

### Step 1: Provision Azure Resources (20-30 minutes)

Open PowerShell or Azure Cloud Shell and run the commands from `AZURE-SETUP.md`:

```powershell
# Login to Azure
az login

# Create Development environment
az group create --name NBT-Dev --location southafricanorth

az sql server create --name nbt-sql-dev --resource-group NBT-Dev --location southafricanorth --admin-user nbtadmin --admin-password 'YourPassword123!'

az sql server firewall-rule create --resource-group NBT-Dev --server nbt-sql-dev --name AllowAzureServices --start-ip-address 0.0.0.0 --end-ip-address 0.0.0.0

az sql db create --resource-group NBT-Dev --server nbt-sql-dev --name NBTWebsite_Dev --service-objective Basic

az appservice plan create --name NBT-AppPlan-Dev --resource-group NBT-Dev --sku B1 --is-linux

az webapp create --name nbt-website-api-dev --resource-group NBT-Dev --plan NBT-AppPlan-Dev --runtime "DOTNET|8.0"

az webapp create --name nbt-website-ui-dev --resource-group NBT-Dev --plan NBT-AppPlan-Dev --runtime "DOTNET|8.0"

az keyvault create --name nbt-keyvault-dev --resource-group NBT-Dev --location southafricanorth

# Repeat for Staging and Production (see AZURE-SETUP.md for full commands)
```

### Step 2: Configure GitHub Secrets (5 minutes)

```powershell
# Create Service Principal for GitHub Actions
az ad sp create-for-rbac --name "GitHub-NBTWebApp-Deploy" --role contributor --scopes /subscriptions/<your-subscription-id>/resourceGroups/NBT-Dev --sdk-auth
```

Copy the JSON output and add to GitHub:
1. Go to https://github.com/PeterWalter/NBTWebApp/settings/secrets/actions
2. Click "New repository secret"
3. Name: `AZURE_CREDENTIALS_DEV`
4. Value: Paste the JSON
5. Repeat for Staging and Production

### Step 3: Test the Pipeline (2 minutes)

```bash
# Make a small change to trigger the pipeline
git commit --allow-empty -m "Test CI/CD pipeline"
git push origin main
```

Then check: https://github.com/PeterWalter/NBTWebApp/actions

### Step 4: Configure Production Environment Protection (3 minutes)

1. Go to https://github.com/PeterWalter/NBTWebApp/settings/environments
2. Click "New environment" → Name: "Production"
3. Check "Required reviewers" → Add yourself
4. Save protection rules

## 📊 Pipeline Status

Check the pipeline status here:
https://github.com/PeterWalter/NBTWebApp/actions

## 🔍 Current Pipeline Run

The pipeline just triggered because we pushed to `main`. It will:
- ✅ Build the solution
- ✅ Run tests (when test projects exist)
- ✅ Scan for security vulnerabilities
- ✅ Check code quality
- ❌ Deployment will FAIL (no Azure resources yet)

This is expected! Once you provision Azure resources and add GitHub secrets, deployments will work.

## 📋 Quick Checklist

- [x] CI/CD workflow created
- [x] Pushed to GitHub
- [x] Pipeline triggered automatically
- [ ] Azure resources provisioned
- [ ] GitHub Secrets configured
- [ ] First successful deployment
- [ ] Production approval configured

## 💡 Tips

- Start with Dev environment only to test the setup
- Verify each App Service starts successfully before proceeding
- Use `az webapp log tail` to debug deployment issues
- Database connection strings can be stored in Key Vault or App Settings

## 📚 Full Documentation

- **AZURE-SETUP.md** - Complete Azure infrastructure guide (all commands)
- **CICD-COMPLETION.md** - Detailed implementation summary and troubleshooting
- **.github/workflows/ci.yml** - The actual pipeline workflow

## 🆘 Need Help?

**Pipeline fails on build:**
- Check the Actions tab on GitHub for detailed logs
- Ensure .NET 8 SDK is compatible with your code

**Deployment fails:**
- Verify Azure resources exist: `az webapp list --resource-group NBT-Dev`
- Check GitHub Secrets are correctly formatted (valid JSON)
- Review App Service logs: `az webapp log tail --name nbt-website-api-dev --resource-group NBT-Dev`

**Cost concerns:**
- Start with Dev environment only (~$60/month)
- Add Staging/Prod when ready for production release
- Use Basic tier for initial testing

---

**Estimated Time to Full Setup:** 30-45 minutes  
**Current Status:** Pipeline code complete, awaiting Azure setup  
**Next Action:** Run Azure CLI commands from AZURE-SETUP.md
