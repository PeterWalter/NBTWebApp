# NBT Website - SQL Server 2019 Production Deployment Guide

## Overview

This guide covers deploying the NBT Website database to **Microsoft SQL Server 2019** in production.

## Current Status

✅ **Development Database Complete**
- Server: `04BF1B900A9D` (SQL Server 2022)
- Database: `NBTWebsite_Dev`
- All tables created (12 tables)
- Initial data seeded
- Application tested successfully

## SQL Server 2019 Compatibility

The NBT Website database is **fully compatible** with SQL Server 2019 and newer versions:

### Compatibility Level
```sql
ALTER DATABASE NBTWebsite SET COMPATIBILITY_LEVEL = 150; -- SQL Server 2019
```

### Features Used
- ✅ Standard T-SQL syntax
- ✅ GUID primary keys (uniqueidentifier)
- ✅ NVARCHAR(MAX) for large text
- ✅ DateTime2 for timestamps
- ✅ Standard indexes (clustered and nonclustered)
- ✅ Identity tables (ASP.NET Core Identity)
- ✅ Foreign keys and constraints

### No Advanced Features
- ❌ No temporal tables
- ❌ No graph database features
- ❌ No Always Encrypted
- ❌ No In-Memory OLTP
- ❌ No Columnstore indexes
- ❌ No JSON functions (can add if SQL 2016+)

## Production Deployment Steps

### Phase 1: Prepare Production Server

#### 1.1 Verify SQL Server 2019 Installation
```sql
SELECT @@VERSION;
-- Should return: Microsoft SQL Server 2019...
```

#### 1.2 Enable TCP/IP Protocol
1. Open **SQL Server Configuration Manager**
2. Expand **SQL Server Network Configuration**
3. Click **Protocols for MSSQLSERVER**
4. Right-click **TCP/IP** → **Enable**
5. Restart SQL Server service

#### 1.3 Configure Firewall
```powershell
# Allow SQL Server through Windows Firewall
New-NetFirewallRule -DisplayName "SQL Server" -Direction Inbound -Protocol TCP -LocalPort 1433 -Action Allow
```

#### 1.4 Enable SQL Server Authentication (Mixed Mode)
1. Open **SQL Server Management Studio**
2. Right-click server → **Properties**
3. Go to **Security** page
4. Select **SQL Server and Windows Authentication mode**
5. Click **OK** and restart SQL Server

### Phase 2: Create Database and Login

#### 2.1 Create Database
```sql
-- Connect to production SQL Server 2019
USE master;
GO

-- Create database
CREATE DATABASE NBTWebsite
ON PRIMARY 
(
    NAME = N'NBTWebsite_Data',
    FILENAME = N'D:\SQLData\NBTWebsite_Data.mdf',
    SIZE = 100MB,
    FILEGROWTH = 50MB
)
LOG ON 
(
    NAME = N'NBTWebsite_Log',
    FILENAME = N'D:\SQLLogs\NBTWebsite_Log.ldf',
    SIZE = 50MB,
    FILEGROWTH = 25MB
);
GO

-- Set compatibility level
ALTER DATABASE NBTWebsite SET COMPATIBILITY_LEVEL = 150;
GO

-- Set recovery model (FULL for production)
ALTER DATABASE NBTWebsite SET RECOVERY FULL;
GO
```

#### 2.2 Create Application Login
```sql
-- Create SQL Server login
CREATE LOGIN nbt_webapp_user WITH PASSWORD = 'YourStrongPassword123!@#';
GO

-- Switch to NBT database
USE NBTWebsite;
GO

-- Create database user
CREATE USER nbt_webapp_user FOR LOGIN nbt_webapp_user;
GO

-- Grant permissions (db_owner for simplicity, or specific permissions)
ALTER ROLE db_owner ADD MEMBER nbt_webapp_user;
GO

-- Alternative: Grant specific permissions (more secure)
/*
GRANT SELECT, INSERT, UPDATE, DELETE ON SCHEMA::dbo TO nbt_webapp_user;
GRANT CREATE TABLE TO nbt_webapp_user;
GRANT ALTER ON SCHEMA::dbo TO nbt_webapp_user;
*/
```

#### 2.3 Verify Login
```sql
-- Test the login
EXECUTE AS USER = 'nbt_webapp_user';
SELECT USER_NAME();
REVERT;
```

### Phase 3: Deploy Database Schema

#### Method 1: Using EF Core Migrations (Recommended)

**On Deployment Machine:**

1. Update `appsettings.Production.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_PROD_SERVER;Database=NBTWebsite;User Id=nbt_webapp_user;Password=YourStrongPassword123!@#;TrustServerCertificate=True;MultipleActiveResultSets=true;Encrypt=True;Connection Timeout=30"
  }
}
```

2. Apply migrations:
```bash
cd src\NBT.WebAPI
$env:ASPNETCORE_ENVIRONMENT="Production"
dotnet ef database update --no-build --verbose
```

#### Method 2: Using SQL Script

**Generate SQL script from development:**
```bash
cd src\NBT.WebAPI
dotnet ef migrations script -o ..\..\..\database-scripts\production-schema.sql
```

**Execute on production server:**
```sql
-- In SSMS, open production-schema.sql and execute
USE NBTWebsite;
GO
-- Then execute the script content
```

#### Method 3: Database Backup/Restore (For Testing)

**On development machine:**
```sql
BACKUP DATABASE NBTWebsite_Dev 
TO DISK = 'D:\Backups\NBTWebsite_Dev.bak'
WITH FORMAT, COMPRESSION;
```

**On production server:**
```sql
RESTORE DATABASE NBTWebsite
FROM DISK = 'D:\Backups\NBTWebsite_Dev.bak'
WITH 
    MOVE 'NBTWebsite_Dev' TO 'D:\SQLData\NBTWebsite_Data.mdf',
    MOVE 'NBTWebsite_Dev_log' TO 'D:\SQLLogs\NBTWebsite_Log.ldf',
    REPLACE;
```

### Phase 4: Verify Schema

```sql
USE NBTWebsite;
GO

-- Check table count
SELECT COUNT(*) AS TableCount
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_TYPE = 'BASE TABLE';
-- Should return: 12

-- List all tables
SELECT TABLE_NAME 
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_TYPE = 'BASE TABLE'
ORDER BY TABLE_NAME;

-- Check migration history
SELECT * FROM __EFMigrationsHistory;

-- Verify indexes
SELECT 
    OBJECT_NAME(i.object_id) AS TableName,
    i.name AS IndexName,
    i.type_desc AS IndexType
FROM sys.indexes i
WHERE OBJECTPROPERTY(i.object_id, 'IsUserTable') = 1
ORDER BY TableName, IndexName;
```

### Phase 5: Configure Application

#### 5.1 Update Connection String

**Option A: Environment Variable (Recommended)**
```bash
# Windows
setx ConnectionStrings__DefaultConnection "Server=PROD_SERVER;Database=NBTWebsite;User Id=nbt_webapp_user;Password=YourStrongPassword123!@#;TrustServerCertificate=True;MultipleActiveResultSets=true;Encrypt=True" /M

# Linux/Docker
export ConnectionStrings__DefaultConnection="Server=PROD_SERVER;Database=NBTWebsite;..."
```

**Option B: Azure App Service Configuration**
- Navigate to App Service → **Configuration** → **Connection strings**
- Add new connection string:
  - Name: `DefaultConnection`
  - Value: `Server=...;Database=NBTWebsite;...`
  - Type: `SQLServer`

**Option C: appsettings.Production.json** (least secure)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=PROD_SERVER;Database=NBTWebsite;..."
  }
}
```

#### 5.2 Update Web API Startup

Ensure `Program.cs` applies migrations on startup:
```csharp
// In Program.cs - already configured
await context.Database.MigrateAsync();
await ApplicationDbContextSeed.SeedAsync(context, userManager, roleManager);
```

### Phase 6: Security Hardening

#### 6.1 Change Default Admin Password
```csharp
// After first deployment, use Identity API to change admin password
// Or update in database directly (hashed)
```

#### 6.2 Disable SA Account
```sql
ALTER LOGIN sa DISABLE;
```

#### 6.3 Restrict Permissions
```sql
-- If using db_owner, consider more restrictive permissions
USE NBTWebsite;
GO

-- Remove from db_owner
ALTER ROLE db_owner DROP MEMBER nbt_webapp_user;

-- Grant specific permissions
GRANT SELECT, INSERT, UPDATE, DELETE ON SCHEMA::dbo TO nbt_webapp_user;
GRANT EXECUTE ON SCHEMA::dbo TO nbt_webapp_user;
```

#### 6.4 Enable Transparent Data Encryption (TDE)
```sql
-- Optional: Encrypt database at rest
USE master;
GO

CREATE MASTER KEY ENCRYPTION BY PASSWORD = 'YourMasterKeyPassword!';
GO

CREATE CERTIFICATE TDECert WITH SUBJECT = 'TDE Certificate';
GO

USE NBTWebsite;
GO

CREATE DATABASE ENCRYPTION KEY
WITH ALGORITHM = AES_256
ENCRYPTION BY SERVER CERTIFICATE TDECert;
GO

ALTER DATABASE NBTWebsite SET ENCRYPTION ON;
GO
```

### Phase 7: Backup Configuration

#### 7.1 Full Backup Script
```sql
-- Daily full backup at 2:00 AM
BACKUP DATABASE NBTWebsite 
TO DISK = 'D:\Backups\NBTWebsite_Full_' + CONVERT(VARCHAR(8), GETDATE(), 112) + '.bak'
WITH COMPRESSION, INIT, FORMAT,
     NAME = 'NBTWebsite Full Backup';
GO
```

#### 7.2 Differential Backup Script
```sql
-- Every 6 hours
BACKUP DATABASE NBTWebsite 
TO DISK = 'D:\Backups\NBTWebsite_Diff_' + CONVERT(VARCHAR(14), GETDATE(), 112) + '.bak'
WITH DIFFERENTIAL, COMPRESSION;
GO
```

#### 7.3 Transaction Log Backup Script
```sql
-- Every 15 minutes
BACKUP LOG NBTWebsite 
TO DISK = 'D:\Backups\NBTWebsite_Log_' + CONVERT(VARCHAR(14), GETDATE(), 112) + '.trn'
WITH COMPRESSION;
GO
```

#### 7.4 Create SQL Server Agent Jobs
1. Open **SQL Server Management Studio**
2. Expand **SQL Server Agent** → **Jobs**
3. Create jobs for:
   - Full Backup (Daily 2:00 AM)
   - Differential Backup (Every 6 hours)
   - Transaction Log Backup (Every 15 minutes)
   - Cleanup old backups (Weekly)

### Phase 8: Monitoring

#### 8.1 Enable Query Store
```sql
ALTER DATABASE NBTWebsite SET QUERY_STORE = ON;
ALTER DATABASE NBTWebsite SET QUERY_STORE (
    OPERATION_MODE = READ_WRITE,
    DATA_FLUSH_INTERVAL_SECONDS = 900,
    STATISTICS_COLLECTION_INTERVAL = 60
);
```

#### 8.2 Create Alerts
```sql
-- Monitor database size
-- Monitor failed logins
-- Monitor long-running queries
-- Monitor deadlocks
```

#### 8.3 Performance Baseline
```sql
-- Capture baseline metrics
SELECT 
    DB_NAME() AS DatabaseName,
    size * 8.0 / 1024 AS SizeMB,
    FILEPROPERTY(name, 'SpaceUsed') * 8.0 / 1024 AS UsedMB
FROM sys.database_files;

-- Check wait statistics
SELECT * FROM sys.dm_os_wait_stats
ORDER BY wait_time_ms DESC;
```

### Phase 9: Test Deployment

#### 9.1 Test Connection
```csharp
// Test from application server
using var connection = new SqlConnection(connectionString);
await connection.OpenAsync();
Console.WriteLine("Connected successfully!");
```

#### 9.2 Verify Data
```sql
-- Check tables exist
SELECT COUNT(*) FROM ContentPages;
SELECT COUNT(*) FROM Users;
SELECT COUNT(*) FROM AspNetRoles;

-- Verify admin user exists
SELECT * FROM Users WHERE Email = 'admin@nbt.ac.za';
```

#### 9.3 Test Application
1. Start Web API
2. Check logs for database connection success
3. Test Swagger endpoints
4. Test Blazor UI connectivity

### Phase 10: Post-Deployment

#### 10.1 Update Documentation
- Document production connection string location
- Document backup procedures
- Document recovery procedures
- Document admin credentials storage

#### 10.2 Monitor First 24 Hours
- Check error logs
- Monitor query performance
- Verify backups complete
- Check connection pool usage

#### 10.3 Handover
- Provide credentials to DBA team
- Share monitoring procedures
- Document escalation procedures

## Rollback Plan

If deployment fails:

1. **Restore from backup**:
```sql
RESTORE DATABASE NBTWebsite
FROM DISK = 'D:\Backups\NBTWebsite_Backup.bak'
WITH REPLACE;
```

2. **Revert application configuration**
3. **Notify stakeholders**

## Connection String Templates

### Development (SQL Server 2022)
```
Server=04BF1B900A9D;Database=NBTWebsite_Dev;Integrated Security=true;TrustServerCertificate=True;MultipleActiveResultSets=true;Encrypt=False
```

### Production (SQL Server 2019 - Windows Auth)
```
Server=PROD_SERVER;Database=NBTWebsite;Integrated Security=true;TrustServerCertificate=True;MultipleActiveResultSets=true;Encrypt=True;Connection Timeout=30
```

### Production (SQL Server 2019 - SQL Auth)
```
Server=PROD_SERVER;Database=NBTWebsite;User Id=nbt_webapp_user;Password=YourStrongPassword123!@#;TrustServerCertificate=True;MultipleActiveResultSets=true;Encrypt=True;Connection Timeout=30;Min Pool Size=5;Max Pool Size=100
```

### Production (Azure SQL Database)
```
Server=tcp:nbt-server.database.windows.net,1433;Database=NBTWebsite;User Id=nbt_webapp_user@nbt-server;Password=YourStrongPassword123!@#;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30
```

## Troubleshooting

### Cannot connect from application
1. Verify SQL Server service is running
2. Check firewall allows port 1433
3. Test connection with `sqlcmd -S SERVER -U USER -P PASSWORD`
4. Verify login credentials
5. Check application server can resolve DNS name

### Migrations fail
1. Check login has sufficient permissions
2. Verify database exists
3. Check connection string is correct
4. Review error logs for specific issue

### Slow performance
1. Check indexes are created
2. Update statistics: `EXEC sp_updatestats;`
3. Rebuild indexes if fragmented
4. Review Query Store for slow queries
5. Check connection pool settings

## Support Contacts

- **DBA Team**: [Contact details]
- **Application Team**: [Contact details]
- **Infrastructure Team**: [Contact details]

## References

- Entity Framework Core Migrations: https://docs.microsoft.com/ef/core/managing-schemas/migrations/
- SQL Server 2019 Documentation: https://docs.microsoft.com/sql/sql-server/
- ASP.NET Core Identity: https://docs.microsoft.com/aspnet/core/security/authentication/identity

---

**Document Version**: 1.0  
**Last Updated**: 2025-11-07  
**Author**: NBT Development Team  
**Classification**: Internal Use Only
