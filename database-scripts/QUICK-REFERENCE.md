# Quick Reference - Connecting to NBT Database

## Development Database

### Connection Details
- **Server**: `04BF1B900A9D`
- **Database**: `NBTWebsite_Dev`
- **Authentication**: Windows Authentication (Integrated Security)

## How to Connect via SSMS

1. **Open SQL Server Management Studio (SSMS)**

2. **Connect to Database Engine**
   - Server name: `04BF1B900A9D`
   - Authentication: `Windows Authentication`
   - Click **Connect**

3. **Find Your Database**
   - Expand **Databases** folder
   - Look for `NBTWebsite_Dev`
   - If you don't see it, right-click **Databases** → **Refresh**

4. **View Tables**
   - Expand `NBTWebsite_Dev`
   - Expand **Tables** folder
   - You should see 12 tables

## Quick Commands

### View All Tables
```sql
USE NBTWebsite_Dev;
GO

SELECT TABLE_NAME 
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_TYPE = 'BASE TABLE'
ORDER BY TABLE_NAME;
```

### View ContentPages Data
```sql
SELECT Id, Title, Slug, Status, PublicationDate 
FROM ContentPages
ORDER BY PublicationDate DESC;
```

### View All Users
```sql
SELECT Id, UserName, Email, FirstName, LastName, Role, Status, LastLoginDate
FROM Users
ORDER BY UserName;
```

### View Contact Inquiries
```sql
SELECT Id, Name, Email, Subject, SubmittedDate, Status
FROM ContactInquiries
ORDER BY SubmittedDate DESC;
```

### View Announcements
```sql
SELECT Id, Title, PublishedDate, ExpiryDate, Status, Priority
FROM Announcements
ORDER BY PublishedDate DESC;
```

### Check Database Size
```sql
EXEC sp_spaceused;
```

### View Recent Migrations
```sql
SELECT MigrationId, ProductVersion 
FROM __EFMigrationsHistory
ORDER BY MigrationId DESC;
```

## Application Configuration Files

### Development Connection String
**File**: `src\NBT.WebAPI\appsettings.Development.json`
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=04BF1B900A9D;Database=NBTWebsite_Dev;Integrated Security=true;TrustServerCertificate=True;MultipleActiveResultSets=true;Encrypt=False"
  }
}
```

### Production Connection String Template
**File**: `src\NBT.WebAPI\appsettings.Production.json`
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_PRODUCTION_SERVER;Database=NBTWebsite;User Id=YOUR_USERNAME;Password=YOUR_PASSWORD;..."
  }
}
```

## Testing Database Connection

### Method 1: Using sqlcmd
```bash
sqlcmd -S 04BF1B900A9D -d NBTWebsite_Dev -E -Q "SELECT @@VERSION"
```

### Method 2: Using PowerShell
```powershell
$connectionString = "Server=04BF1B900A9D;Database=NBTWebsite_Dev;Integrated Security=true;"
$connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$connection.Open()
Write-Host "Connection successful!" -ForegroundColor Green
$connection.Close()
```

### Method 3: Using .NET CLI
```bash
cd src\NBT.WebAPI
dotnet ef dbcontext info
```

## Common Tasks

### Apply Pending Migrations
```bash
cd src\NBT.WebAPI
dotnet ef database update
```

### Create New Migration
```bash
cd src\NBT.WebAPI
dotnet ef migrations add MigrationName
```

### Rollback Migration
```bash
cd src\NBT.WebAPI
dotnet ef database update PreviousMigrationName
```

### Generate SQL Script
```bash
cd src\NBT.WebAPI
dotnet ef migrations script -o migration-script.sql
```

## Troubleshooting

### Issue: Can't see database in SSMS
**Solution**: 
1. Right-click **Databases** → **Refresh**
2. Verify server name is exactly: `04BF1B900A9D`
3. Check you selected Windows Authentication

### Issue: Login failed
**Solution**:
1. Ensure SQL Server service is running
2. Use Windows Authentication (not SQL Server Authentication)
3. Your Windows account must have access

### Issue: Tables are empty
**Solution**:
- The application seeds data on first run
- Start the API project: `dotnet run --project src\NBT.WebAPI`
- Check seed data in `ApplicationDbContextSeed.cs`

### Issue: Connection timeout
**Solution**:
1. Check SQL Server is running: 
   - Press `Win + R`, type `services.msc`
   - Find "SQL Server (MSSQLSERVER)"
   - Status should be "Running"
2. Restart SQL Server if needed

## Important Notes

✅ **Development Database Created**: NBTWebsite_Dev on server 04BF1B900A9D
✅ **All Tables Created**: 12 tables including ContentPages, Users, Announcements, etc.
✅ **Migrations Applied**: Schema is up to date
⚠️ **Production Database**: Not yet created - will be on SQL Server 2019 production server

## Next Steps

1. Start the API project to seed initial data
2. Use SSMS to browse tables and verify structure
3. Test API endpoints that interact with database
4. Plan for production SQL Server 2019 deployment
