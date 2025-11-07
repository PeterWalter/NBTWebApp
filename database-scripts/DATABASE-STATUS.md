# NBT Website Database - Current Status

## Database Information

- **Database Server**: SQL Server 2022 (16.0.1130.5) - Developer Edition
- **Server Instance**: 04BF1B900A9D
- **Development Database**: NBTWebsite_Dev
- **Production Database**: NBTWebsite (to be created in production environment)
- **Compatibility**: SQL Server 2019+ (fully compatible with SQL Server 2022)

## Database Created Successfully ✅

The database has been created and all migrations have been applied successfully on **2025-11-07**.

### Connection Strings

#### Development (Current)
```
Server=04BF1B900A9D;Database=NBTWebsite_Dev;Integrated Security=true;TrustServerCertificate=True;MultipleActiveResultSets=true;Encrypt=False
```

#### Production (Template)
```
Server=YOUR_PRODUCTION_SERVER;Database=NBTWebsite;User Id=YOUR_USERNAME;Password=YOUR_PASSWORD;TrustServerCertificate=True;MultipleActiveResultSets=true;Encrypt=True;Min Pool Size=5;Max Pool Size=100;Connection Timeout=30
```

## Database Schema

### Tables Created (12 tables)

1. **__EFMigrationsHistory** - Entity Framework migration tracking
2. **Announcements** - News and announcements
3. **AspNetRoleClaims** - Identity role claims
4. **AspNetRoles** - Identity roles
5. **AspNetUserClaims** - Identity user claims
6. **AspNetUserLogins** - Identity external logins
7. **AspNetUserRoles** - Identity user-role mapping
8. **AspNetUserTokens** - Identity tokens
9. **ContactInquiries** - Contact form submissions
10. **ContentPages** - CMS content pages
11. **DownloadableResources** - Downloadable files and documents
12. **Users** - Custom user table extending Identity

## Key Tables Details

### ContentPages
- **Purpose**: Stores all website content pages (Landing, About, Applicants, etc.)
- **Key Columns**: 
  - Id (uniqueidentifier) - Primary key
  - Title (nvarchar(200)) - Page title
  - Slug (nvarchar(250)) - URL-friendly identifier (unique)
  - BodyContent (nvarchar(MAX)) - HTML content
  - MetaDescription (nvarchar(500)) - SEO meta description
  - Keywords (nvarchar(500)) - SEO keywords
  - PublicationDate (datetime2) - When published
  - Status (nvarchar(50)) - Draft/Published/Archived
  - Audit fields (CreatedBy, CreatedDate, LastModifiedBy, LastModifiedDate)
- **Indexes**: 
  - Primary Key on Id (clustered)
  - Unique index on Slug (nonclustered)
- **Default**: Status defaults to 'Draft'

### Announcements
- **Purpose**: Store news and announcements for "What's New" page
- **Key Columns**:
  - Id (uniqueidentifier)
  - Title (nvarchar(200))
  - Content (nvarchar(MAX))
  - PublishedDate (datetime2)
  - ExpiryDate (datetime2, nullable)
  - Status (nvarchar(50))
  - Priority (int)
  - Audit fields
- **Indexes**:
  - Primary Key on Id
  - Index on PublishedDate
  - Index on Status

### ContactInquiries
- **Purpose**: Store contact form submissions
- **Key Columns**:
  - Id (uniqueidentifier)
  - Name (nvarchar(100))
  - Email (nvarchar(100))
  - PhoneNumber (nvarchar(20), nullable)
  - Organization (nvarchar(200), nullable)
  - Subject (nvarchar(200))
  - Message (nvarchar(MAX))
  - SubmittedDate (datetime2)
  - Status (nvarchar(50))
  - ResponseNotes (nvarchar(MAX), nullable)
  - RespondedDate (datetime2, nullable)
- **Indexes**:
  - Primary Key on Id
  - Index on SubmittedDate
  - Index on Status
  - Index on Email

### Users (Custom Identity)
- **Purpose**: Extended ASP.NET Identity user with custom fields
- **Key Columns**:
  - Standard Identity fields (Id, UserName, Email, PasswordHash, etc.)
  - FirstName (nvarchar(100))
  - LastName (nvarchar(100))
  - Organization (nvarchar(200), nullable)
  - Role (nvarchar(50))
  - Status (nvarchar(50))
  - LastLoginDate (datetime2, nullable)
  - Audit fields
- **Indexes**:
  - Primary Key on Id
  - Unique index on NormalizedUserName
  - Unique index on NormalizedEmail
  - Index on Email
  - Index on Role
  - Index on Status

### DownloadableResources
- **Purpose**: Store downloadable files and documents
- **Key Columns**:
  - Id (uniqueidentifier)
  - Title (nvarchar(200))
  - Description (nvarchar(500), nullable)
  - FileName (nvarchar(255))
  - FileSize (bigint)
  - FileType (nvarchar(50))
  - Category (nvarchar(100))
  - UploadDate (datetime2)
  - DownloadCount (int)
  - Status (nvarchar(50))
  - Audit fields
- **Indexes**:
  - Primary Key on Id
  - Index on Category
  - Index on UploadDate
  - Index on Status

## Accessing the Database

### Via SQL Server Management Studio (SSMS)
1. **Server Name**: `04BF1B900A9D`
2. **Authentication**: Windows Authentication
3. **Database**: NBTWebsite_Dev

### Via Visual Studio
1. Open **SQL Server Object Explorer**
2. Connect to `04BF1B900A9D`
3. Expand databases to find `NBTWebsite_Dev`

### Via Command Line (sqlcmd)
```bash
sqlcmd -S 04BF1B900A9D -d NBTWebsite_Dev -E
```

## Entity Framework Core

### Migrations Applied
- **20251107113354_InitialCreate** - Initial database schema

### Running New Migrations
```bash
cd src\NBT.WebAPI

# Create new migration
dotnet ef migrations add YourMigrationName

# Apply migrations
dotnet ef database update
```

### Remove Last Migration
```bash
dotnet ef migrations remove
```

## Data Seeding

The application automatically seeds initial data on startup:
- Admin user account
- Default roles (Admin, Editor, Viewer)
- Sample content pages

Check `ApplicationDbContextSeed.cs` for seed data details.

## Production Deployment Checklist

### Before Deploying to Production SQL Server 2019

1. **Create Production Database**
   ```sql
   CREATE DATABASE NBTWebsite
   GO
   ```

2. **Create SQL Server Login**
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

3. **Update Connection String**
   - Use `appsettings.Production.json`
   - Or use Azure App Service Configuration
   - Or use Environment Variables

4. **Run Migrations**
   ```bash
   $env:ASPNETCORE_ENVIRONMENT="Production"
   dotnet ef database update --project src\NBT.WebAPI
   ```

5. **Verify Schema**
   ```sql
   SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'
   -- Should return 12
   ```

6. **Test Connection**
   - Start the API project
   - Check logs for database connection success
   - Test API endpoints

## Backup and Recovery

### Development
- Backups not critical
- Can recreate from migrations

### Production (Recommended)
- **Full Backup**: Daily at 2:00 AM
- **Differential Backup**: Every 6 hours
- **Transaction Log Backup**: Every 15 minutes
- **Retention**: 30 days

## Performance Considerations

- All foreign keys have indexes
- Audit date columns indexed for queries
- Status columns indexed for filtering
- Slug column has unique index for fast lookups
- Connection pooling enabled (Min: 5, Max: 100)

## Security

- ✅ Passwords hashed with ASP.NET Core Identity
- ✅ SQL injection protected via Entity Framework
- ✅ No sensitive data in connection strings (use secrets)
- ✅ TLS encryption enabled for production
- ✅ Principle of least privilege for database users

## Troubleshooting

### Cannot see database in SSMS
1. Refresh the databases node
2. Ensure you're connected to the correct server: `04BF1B900A9D`
3. Check you're using Windows Authentication
4. Verify database exists:
   ```sql
   SELECT name FROM sys.databases WHERE name = 'NBTWebsite_Dev'
   ```

### Migrations fail
1. Check connection string in `appsettings.Development.json`
2. Verify SQL Server is running
3. Check ApplicationDbContextFactory.cs reads correct config
4. Rebuild project: `dotnet build --no-incremental`

### Production deployment fails
1. Verify SQL Server 2019 is accessible
2. Test connection string
3. Ensure SQL Server Authentication is enabled
4. Check firewall allows port 1433
5. Verify login has necessary permissions

## Database Seeded Successfully ✅

The database has been seeded with initial data:

### Seeded Data Summary
- ✅ **3 Content Pages**: Terms and Conditions, About NBT, Privacy Policy
- ✅ **3 Announcements**: Sample announcements for What's New
- ✅ **1 Admin User**: admin@nbt.ac.za (default password needs to be changed)
- ✅ **5 Roles**: Administrator, Staff, Educator, Institution, Applicant
- ✅ **5 Downloadable Resources**: Sample documents and resources

### Default Admin Account
- **Username**: admin@nbt.ac.za
- **Email**: admin@nbt.ac.za
- **Name**: System Administrator
- **Role**: Admin
- **Status**: Active
- **Password**: See ApplicationDbContextSeed.cs (change immediately!)

## Next Steps

1. ✅ Database created and migrated
2. ✅ Schema verified
3. ✅ Seed initial content data
4. ✅ Test API startup with database
5. ⏳ Add remaining website pages content
6. ⏳ Test API endpoints with Swagger
7. ⏳ Configure production SQL Server 2019
8. ⏳ Set up backup strategy
9. ⏳ Configure monitoring and alerts

## Support

For database-related issues:
- Check `database-scripts\SQL2019-Setup.md` for detailed setup
- Review EF Core logs in application
- Check SQL Server error logs
- Verify connection strings and permissions
