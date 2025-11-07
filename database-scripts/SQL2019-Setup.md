# MS SQL Server 2019 Setup Guide

## Connection String Options

### Development (Local SQL Server Instance)
```
Server=04BF1B900A9D;Database=NBTWebsite_Dev;Integrated Security=true;TrustServerCertificate=True;MultipleActiveResultSets=true;Encrypt=False
```

### Production (MS SQL Server 2019)
```
Server=YOUR_PRODUCTION_SERVER;Database=NBTWebsite;User Id=YOUR_USERNAME;Password=YOUR_PASSWORD;TrustServerCertificate=True;MultipleActiveResultSets=true;Encrypt=True;Min Pool Size=5;Max Pool Size=100;Connection Timeout=30
```

## Key Differences from LocalDB

1. **Server Name**: Use actual SQL Server instance name instead of `(localdb)\...`
2. **Authentication**: 
   - Development: Windows Authentication (Integrated Security)
   - Production: SQL Server Authentication (User Id/Password)
3. **Encryption**: Enabled in production for security
4. **Connection Pooling**: Configured for production performance
5. **Database Name**: Separate databases for Dev/Prod

## SQL Server 2019 Compatibility

The database schema is fully compatible with SQL Server 2019:
- Uses standard T-SQL syntax
- Entity Framework Core 8.0 supports SQL Server 2019
- Identity tables use standard structure
- All indexes and constraints are SQL Server 2019 compatible

## Migration Steps

### 1. Create Database (Production)
```sql
CREATE DATABASE NBTWebsite
GO

USE NBTWebsite
GO
```

### 2. Create SQL Login (Production)
```sql
CREATE LOGIN nbt_webapp_user WITH PASSWORD = 'YourStrongPassword123!';
GO

USE NBTWebsite;
GO

CREATE USER nbt_webapp_user FOR LOGIN nbt_webapp_user;
GO

ALTER ROLE db_owner ADD MEMBER nbt_webapp_user;
GO
```

### 3. Run EF Core Migrations
```bash
# Set environment variable
$env:ASPNETCORE_ENVIRONMENT="Production"

# Navigate to API project
cd src\NBT.WebAPI

# Apply migrations
dotnet ef database update
```

## Security Best Practices

1. **Never commit production credentials** to source control
2. Use **Azure Key Vault** or **Environment Variables** for production connection strings
3. Use **SQL Server Authentication** with strong passwords
4. Enable **SSL/TLS encryption** (Encrypt=True)
5. Implement **principle of least privilege** for database users
6. Use **separate databases** for Dev/Test/Prod environments

## Environment-Specific Configuration

### Option 1: User Secrets (Development)
```bash
dotnet user-secrets init --project src\NBT.WebAPI
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=04BF1B900A9D;..." --project src\NBT.WebAPI
```

### Option 2: Environment Variables (Production)
```bash
# Windows
setx ConnectionStrings__DefaultConnection "Server=...;Database=NBTWebsite;..."

# Linux/Docker
export ConnectionStrings__DefaultConnection="Server=...;Database=NBTWebsite;..."
```

### Option 3: Azure App Service Configuration
Add connection string in Azure Portal:
- Name: `DefaultConnection`
- Value: `Server=...;Database=NBTWebsite;...`
- Type: `SQLAzure`

## Database Backup Strategy

### Development
- Regular backups not critical
- Can recreate from migrations

### Production
```sql
-- Full Backup (Daily)
BACKUP DATABASE NBTWebsite 
TO DISK = 'D:\Backups\NBTWebsite_Full.bak'
WITH FORMAT, COMPRESSION;

-- Differential Backup (Every 6 hours)
BACKUP DATABASE NBTWebsite 
TO DISK = 'D:\Backups\NBTWebsite_Diff.bak'
WITH DIFFERENTIAL, COMPRESSION;

-- Transaction Log Backup (Every 15 minutes)
BACKUP LOG NBTWebsite 
TO DISK = 'D:\Backups\NBTWebsite_Log.trn'
WITH COMPRESSION;
```

## Performance Tuning for SQL Server 2019

### Recommended Settings
```sql
-- Enable Query Store
ALTER DATABASE NBTWebsite SET QUERY_STORE = ON;
ALTER DATABASE NBTWebsite SET QUERY_STORE (OPERATION_MODE = READ_WRITE);

-- Set compatibility level
ALTER DATABASE NBTWebsite SET COMPATIBILITY_LEVEL = 150; -- SQL Server 2019

-- Enable automatic tuning (optional)
ALTER DATABASE NBTWebsite SET AUTOMATIC_TUNING (FORCE_LAST_GOOD_PLAN = ON);
```

## Monitoring

Monitor these metrics in production:
- Database size and growth
- Query performance (via Query Store)
- Connection pool usage
- Deadlocks and timeouts
- Index fragmentation

## Troubleshooting

### Cannot connect to SQL Server
1. Check SQL Server is running: `services.msc` → SQL Server (MSSQLSERVER)
2. Verify TCP/IP is enabled: SQL Server Configuration Manager
3. Check firewall rules allow port 1433
4. Test connection: `Test-NetConnection -ComputerName YOUR_SERVER -Port 1433`

### Migrations fail
1. Verify connection string is correct
2. Check SQL Server login has db_owner permissions
3. Ensure database exists
4. Review migration error messages

### Permission denied
Grant appropriate permissions:
```sql
USE NBTWebsite;
GRANT SELECT, INSERT, UPDATE, DELETE ON SCHEMA::dbo TO nbt_webapp_user;
```
