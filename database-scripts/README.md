# Database Scripts for NBT Website

This folder contains SQL scripts for manual database setup and verification.

## Script Execution Order

Execute the scripts in the following order:

### 1. Create Database
```sql
-- File: 01-CreateDatabase.sql
-- Creates the NBTWebsite database with appropriate settings
```

### 2. Create Tables
```sql
-- File: 02-CreateTables.sql
-- Creates all required tables including:
-- - ASP.NET Core Identity tables (Users, Roles, etc.)
-- - ContentPages
-- - Announcements
-- - ContactInquiries
-- - DownloadableResources
```

### 3. Seed Initial Data
```sql
-- File: 03-SeedData.sql
-- Inserts initial seed data:
-- - Default roles (Administrator, Staff, Educator, Institution, Applicant)
-- - Sample content pages (About, Privacy Policy, Terms)
-- - Sample announcements
-- - Sample downloadable resources
```

### 4. Verify Database
```sql
-- File: 04-VerifyDatabase.sql
-- Verifies database structure and displays row counts
```

## Automatic Database Setup

The application will automatically create and seed the database on first run using Entity Framework Core migrations. The `Program.cs` file includes code to:

1. Apply pending migrations
2. Seed initial data

### Connection String

Update the connection string in `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=NBTWebsite;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

For production, update `appsettings.json` or use environment variables.

## Manual Database Setup

If you prefer manual database setup:

1. Open SQL Server Management Studio (SSMS)
2. Connect to your SQL Server instance
3. Execute scripts 01 through 03 in order
4. Run script 04 to verify

## Entity Framework Migrations

### Create New Migration
```bash
cd src\NBT.Infrastructure
dotnet ef migrations add MigrationName --startup-project ..\NBT.WebAPI
```

### Update Database
```bash
cd src\NBT.Infrastructure
dotnet ef database update --startup-project ..\NBT.WebAPI
```

### Remove Last Migration
```bash
cd src\NBT.Infrastructure
dotnet ef migrations remove --startup-project ..\NBT.WebAPI
```

### Generate SQL Script from Migrations
```bash
cd src\NBT.Infrastructure
dotnet ef migrations script --startup-project ..\NBT.WebAPI --output ..\..\database-scripts\migration.sql
```

## Database Schema

### Identity Tables
- **AspNetUsers** - User accounts with custom fields
- **AspNetRoles** - User roles
- **AspNetUserRoles** - User-role relationships
- **AspNetUserClaims** - User claims
- **AspNetUserLogins** - External login providers
- **AspNetUserTokens** - User authentication tokens
- **AspNetRoleClaims** - Role-based claims

### Application Tables
- **ContentPages** - Static content pages (About, Policies, etc.)
- **Announcements** - News and announcements
- **ContactInquiries** - Contact form submissions
- **DownloadableResources** - Files available for download

## Default Credentials

After seeding, a default administrator account is created:

- **Email**: admin@nbt.ac.za
- **Password**: Admin@123!

**⚠️ IMPORTANT**: Change this password immediately in production!

## Security Notes

1. Never commit connection strings with production credentials
2. Use Azure Key Vault or similar for production secrets
3. Enable SQL Server encryption for sensitive data
4. Implement regular database backups
5. Use least-privilege database accounts for the application

## Troubleshooting

### Cannot connect to LocalDB
```bash
# Check if LocalDB is installed
sqllocaldb info

# Create instance if needed
sqllocaldb create MSSQLLocalDB

# Start instance
sqllocaldb start MSSQLLocalDB
```

### Migration conflicts
```bash
# Reset database (CAUTION: Deletes all data)
dotnet ef database drop --startup-project ..\NBT.WebAPI
dotnet ef database update --startup-project ..\NBT.WebAPI
```

### Check applied migrations
```bash
dotnet ef migrations list --startup-project ..\NBT.WebAPI
```
