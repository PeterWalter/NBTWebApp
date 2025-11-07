# Azure Infrastructure Setup Guide

This guide provides step-by-step instructions for setting up the Azure infrastructure required for the NBT Website application.

## Prerequisites

- Azure subscription with appropriate permissions
- Azure CLI installed locally (`az --version` to verify)
- GitHub repository access with admin permissions

## Environment Overview

| Environment | Purpose | Branch | URL Pattern |
|-------------|---------|--------|-------------|
| Development | Active development & testing | `develop` | nbt-website-*-dev.azurewebsites.net |
| Staging | Pre-production validation | `main` | nbt-website-*-staging.azurewebsites.net |
| Production | Live production environment | `main` (manual approval) | nbt-website-*-prod.azurewebsites.net |

---

## Step 1: Azure Resource Groups

Create resource groups for each environment:

```bash
# Development
az group create --name NBT-Dev --location southafricanorth

# Staging
az group create --name NBT-Staging --location southafricanorth

# Production
az group create --name NBT-Prod --location southafricanorth
```

---

## Step 2: Azure SQL Databases

### Development Database

```bash
# Create SQL Server
az sql server create \
  --name nbt-sql-dev \
  --resource-group NBT-Dev \
  --location southafricanorth \
  --admin-user nbtadmin \
  --admin-password '<YourSecurePassword123!>'

# Configure firewall (allow Azure services)
az sql server firewall-rule create \
  --resource-group NBT-Dev \
  --server nbt-sql-dev \
  --name AllowAzureServices \
  --start-ip-address 0.0.0.0 \
  --end-ip-address 0.0.0.0

# Create database
az sql db create \
  --resource-group NBT-Dev \
  --server nbt-sql-dev \
  --name NBTWebsite_Dev \
  --service-objective Basic \
  --backup-storage-redundancy Local
```

### Staging Database

```bash
# Create SQL Server
az sql server create \
  --name nbt-sql-staging \
  --resource-group NBT-Staging \
  --location southafricanorth \
  --admin-user nbtadmin \
  --admin-password '<YourSecurePassword123!>'

# Configure firewall
az sql server firewall-rule create \
  --resource-group NBT-Staging \
  --server nbt-sql-staging \
  --name AllowAzureServices \
  --start-ip-address 0.0.0.0 \
  --end-ip-address 0.0.0.0

# Create database
az sql db create \
  --resource-group NBT-Staging \
  --server nbt-sql-staging \
  --name NBTWebsite_Staging \
  --service-objective S0 \
  --backup-storage-redundancy Zone
```

### Production Database

```bash
# Create SQL Server
az sql server create \
  --name nbt-sql-prod \
  --resource-group NBT-Prod \
  --location southafricanorth \
  --admin-user nbtadmin \
  --admin-password '<YourSecurePassword123!>'

# Configure firewall
az sql server firewall-rule create \
  --resource-group NBT-Prod \
  --server nbt-sql-prod \
  --name AllowAzureServices \
  --start-ip-address 0.0.0.0 \
  --end-ip-address 0.0.0.0

# Create database
az sql db create \
  --resource-group NBT-Prod \
  --server nbt-sql-prod \
  --name NBTWebsite_Prod \
  --service-objective S1 \
  --backup-storage-redundancy Zone
```

**Connection Strings Format:**
```
Server=tcp:<server-name>.database.windows.net,1433;Initial Catalog=<database-name>;Persist Security Info=False;User ID=nbtadmin;Password=<password>;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;
```

---

## Step 3: Azure App Service Plans

Create App Service Plans for each environment:

```bash
# Development (B1 - Basic)
az appservice plan create \
  --name NBT-AppPlan-Dev \
  --resource-group NBT-Dev \
  --sku B1 \
  --is-linux

# Staging (S1 - Standard)
az appservice plan create \
  --name NBT-AppPlan-Staging \
  --resource-group NBT-Staging \
  --sku S1 \
  --is-linux

# Production (P1v2 - Premium)
az appservice plan create \
  --name NBT-AppPlan-Prod \
  --resource-group NBT-Prod \
  --sku P1v2 \
  --is-linux
```

---

## Step 4: Azure App Services (Web Apps)

### Development Environment

```bash
# WebAPI
az webapp create \
  --name nbt-website-api-dev \
  --resource-group NBT-Dev \
  --plan NBT-AppPlan-Dev \
  --runtime "DOTNET|8.0"

# WebUI
az webapp create \
  --name nbt-website-ui-dev \
  --resource-group NBT-Dev \
  --plan NBT-AppPlan-Dev \
  --runtime "DOTNET|8.0"

# Configure WebAPI settings
az webapp config appsettings set \
  --name nbt-website-api-dev \
  --resource-group NBT-Dev \
  --settings \
    ASPNETCORE_ENVIRONMENT=Development \
    WEBSITE_RUN_FROM_PACKAGE=1

# Configure WebUI settings
az webapp config appsettings set \
  --name nbt-website-ui-dev \
  --resource-group NBT-Dev \
  --settings \
    ASPNETCORE_ENVIRONMENT=Development \
    WEBSITE_RUN_FROM_PACKAGE=1
```

### Staging Environment

```bash
# WebAPI
az webapp create \
  --name nbt-website-api-staging \
  --resource-group NBT-Staging \
  --plan NBT-AppPlan-Staging \
  --runtime "DOTNET|8.0"

# WebUI
az webapp create \
  --name nbt-website-ui-staging \
  --resource-group NBT-Staging \
  --plan NBT-AppPlan-Staging \
  --runtime "DOTNET|8.0"

# Configure WebAPI settings
az webapp config appsettings set \
  --name nbt-website-api-staging \
  --resource-group NBT-Staging \
  --settings \
    ASPNETCORE_ENVIRONMENT=Staging \
    WEBSITE_RUN_FROM_PACKAGE=1

# Configure WebUI settings
az webapp config appsettings set \
  --name nbt-website-ui-staging \
  --resource-group NBT-Staging \
  --settings \
    ASPNETCORE_ENVIRONMENT=Staging \
    WEBSITE_RUN_FROM_PACKAGE=1
```

### Production Environment

```bash
# WebAPI
az webapp create \
  --name nbt-website-api-prod \
  --resource-group NBT-Prod \
  --plan NBT-AppPlan-Prod \
  --runtime "DOTNET|8.0"

# WebUI
az webapp create \
  --name nbt-website-ui-prod \
  --resource-group NBT-Prod \
  --plan NBT-AppPlan-Prod \
  --runtime "DOTNET|8.0"

# Configure WebAPI settings
az webapp config appsettings set \
  --name nbt-website-api-prod \
  --resource-group NBT-Prod \
  --settings \
    ASPNETCORE_ENVIRONMENT=Production \
    WEBSITE_RUN_FROM_PACKAGE=1

# Configure WebUI settings
az webapp config appsettings set \
  --name nbt-website-ui-prod \
  --resource-group NBT-Prod \
  --settings \
    ASPNETCORE_ENVIRONMENT=Production \
    WEBSITE_RUN_FROM_PACKAGE=1
```

---

## Step 5: Azure Key Vault

Create Key Vaults to store secrets:

```bash
# Development
az keyvault create \
  --name nbt-keyvault-dev \
  --resource-group NBT-Dev \
  --location southafricanorth \
  --sku standard

# Staging
az keyvault create \
  --name nbt-keyvault-staging \
  --resource-group NBT-Staging \
  --location southafricanorth \
  --sku standard

# Production
az keyvault create \
  --name nbt-keyvault-prod \
  --resource-group NBT-Prod \
  --location southafricanorth \
  --sku standard
```

### Store Database Connection Strings

```bash
# Development
az keyvault secret set \
  --vault-name nbt-keyvault-dev \
  --name ConnectionStrings--DefaultConnection \
  --value "Server=tcp:nbt-sql-dev.database.windows.net,1433;Initial Catalog=NBTWebsite_Dev;..."

# Staging
az keyvault secret set \
  --vault-name nbt-keyvault-staging \
  --name ConnectionStrings--DefaultConnection \
  --value "Server=tcp:nbt-sql-staging.database.windows.net,1433;Initial Catalog=NBTWebsite_Staging;..."

# Production
az keyvault secret set \
  --vault-name nbt-keyvault-prod \
  --name ConnectionStrings--DefaultConnection \
  --value "Server=tcp:nbt-sql-prod.database.windows.net,1433;Initial Catalog=NBTWebsite_Prod;..."
```

---

## Step 6: Configure Managed Identity

Enable System-Assigned Managed Identity for App Services:

```bash
# Development
az webapp identity assign --name nbt-website-api-dev --resource-group NBT-Dev
az webapp identity assign --name nbt-website-ui-dev --resource-group NBT-Dev

# Staging
az webapp identity assign --name nbt-website-api-staging --resource-group NBT-Staging
az webapp identity assign --name nbt-website-ui-staging --resource-group NBT-Staging

# Production
az webapp identity assign --name nbt-website-api-prod --resource-group NBT-Prod
az webapp identity assign --name nbt-website-ui-prod --resource-group NBT-Prod
```

Grant Key Vault access to Managed Identities:

```bash
# Get the principal IDs (object IDs) from the previous command output
# Then grant access policies:

# Development
az keyvault set-policy \
  --name nbt-keyvault-dev \
  --object-id <api-principal-id> \
  --secret-permissions get list

az keyvault set-policy \
  --name nbt-keyvault-dev \
  --object-id <ui-principal-id> \
  --secret-permissions get list

# Repeat for Staging and Production with their respective principal IDs
```

---

## Step 7: GitHub Secrets Configuration

Create a Service Principal for GitHub Actions:

```bash
az ad sp create-for-rbac \
  --name "GitHub-NBTWebApp-Deploy" \
  --role contributor \
  --scopes /subscriptions/<subscription-id>/resourceGroups/NBT-Dev \
           /subscriptions/<subscription-id>/resourceGroups/NBT-Staging \
           /subscriptions/<subscription-id>/resourceGroups/NBT-Prod \
  --sdk-auth
```

This will output JSON credentials. Add these as GitHub Secrets:

### Required GitHub Secrets

1. Go to your GitHub repository
2. Navigate to Settings → Secrets and variables → Actions
3. Add the following secrets:

| Secret Name | Value | Description |
|-------------|-------|-------------|
| `AZURE_CREDENTIALS_DEV` | JSON output from `az ad sp create-for-rbac` (Dev scope) | Development Azure credentials |
| `AZURE_CREDENTIALS_STAGING` | JSON output from `az ad sp create-for-rbac` (Staging scope) | Staging Azure credentials |
| `AZURE_CREDENTIALS_PROD` | JSON output from `az ad sp create-for-rbac` (Prod scope) | Production Azure credentials |

**Example JSON format:**
```json
{
  "clientId": "<client-id>",
  "clientSecret": "<client-secret>",
  "subscriptionId": "<subscription-id>",
  "tenantId": "<tenant-id>",
  "activeDirectoryEndpointUrl": "https://login.microsoftonline.com",
  "resourceManagerEndpointUrl": "https://management.azure.com/",
  "activeDirectoryGraphResourceId": "https://graph.windows.net/",
  "sqlManagementEndpointUrl": "https://management.core.windows.net:8443/",
  "galleryEndpointUrl": "https://gallery.azure.com/",
  "managementEndpointUrl": "https://management.core.windows.net/"
}
```

---

## Step 8: Application Insights (Optional but Recommended)

Set up monitoring for each environment:

```bash
# Development
az monitor app-insights component create \
  --app nbt-appinsights-dev \
  --location southafricanorth \
  --resource-group NBT-Dev \
  --application-type web

# Get instrumentation key
az monitor app-insights component show \
  --app nbt-appinsights-dev \
  --resource-group NBT-Dev \
  --query instrumentationKey

# Configure App Services
az webapp config appsettings set \
  --name nbt-website-api-dev \
  --resource-group NBT-Dev \
  --settings APPLICATIONINSIGHTS_CONNECTION_STRING="<connection-string>"

# Repeat for Staging and Production
```

---

## Step 9: Run Database Migrations

After deploying the WebAPI, run migrations:

```bash
# From local machine with appropriate connection string
dotnet ef database update --project src/NBT.Infrastructure --startup-project src/NBT.WebAPI --connection "<connection-string>"

# Or configure automatic migrations in WebAPI startup (not recommended for production)
```

---

## Step 10: Configure CORS

Update WebAPI CORS settings to allow WebUI domains:

```bash
# Development
az webapp cors add \
  --name nbt-website-api-dev \
  --resource-group NBT-Dev \
  --allowed-origins https://nbt-website-ui-dev.azurewebsites.net

# Staging
az webapp cors add \
  --name nbt-website-api-staging \
  --resource-group NBT-Staging \
  --allowed-origins https://nbt-website-ui-staging.azurewebsites.net

# Production
az webapp cors add \
  --name nbt-website-api-prod \
  --resource-group NBT-Prod \
  --allowed-origins https://nbt-website.co.za
```

---

## Step 11: Custom Domains (Production Only)

```bash
# Add custom domain
az webapp config hostname add \
  --webapp-name nbt-website-ui-prod \
  --resource-group NBT-Prod \
  --hostname www.nbt-website.co.za

# Enable HTTPS
az webapp config ssl bind \
  --name nbt-website-ui-prod \
  --resource-group NBT-Prod \
  --certificate-thumbprint <thumbprint> \
  --ssl-type SNI
```

---

## Verification Checklist

- [ ] All resource groups created
- [ ] SQL Servers and databases provisioned
- [ ] App Service Plans created
- [ ] Web Apps created and configured
- [ ] Key Vaults created and secrets stored
- [ ] Managed Identities enabled and access granted
- [ ] GitHub Secrets configured
- [ ] Application Insights configured
- [ ] CORS settings applied
- [ ] Database migrations run successfully
- [ ] CI/CD pipeline runs without errors

---

## Cost Estimation (Monthly)

| Environment | Resources | Estimated Cost (USD) |
|-------------|-----------|---------------------|
| Development | Basic SQL DB, B1 App Service Plan | ~$50-70 |
| Staging | S0 SQL DB, S1 App Service Plan | ~$120-150 |
| Production | S1 SQL DB, P1v2 App Service Plan | ~$250-300 |
| **Total** | | **~$420-520/month** |

*Costs may vary based on usage, data transfer, and additional services.*

---

## Cleanup Script (Delete Everything)

**⚠️ WARNING: This will delete all resources!**

```bash
az group delete --name NBT-Dev --yes --no-wait
az group delete --name NBT-Staging --yes --no-wait
az group delete --name NBT-Prod --yes --no-wait
```

---

## Troubleshooting

### Pipeline fails with authentication error
- Verify GitHub Secrets are correctly configured
- Ensure Service Principal has contributor role on resource groups

### App Service won't start
- Check Application Insights logs
- Verify connection strings in Key Vault
- Check App Service logs: `az webapp log tail --name <app-name> --resource-group <rg-name>`

### Database connection fails
- Verify firewall rules allow Azure services
- Check connection string format
- Ensure Managed Identity has access to Key Vault

---

## Additional Resources

- [Azure App Service Documentation](https://docs.microsoft.com/azure/app-service/)
- [Azure SQL Database Documentation](https://docs.microsoft.com/azure/azure-sql/)
- [GitHub Actions Documentation](https://docs.github.com/actions)
- [Azure Key Vault Documentation](https://docs.microsoft.com/azure/key-vault/)

---

**Last Updated:** November 7, 2025  
**Maintained By:** CEA Data Systems Team
