# NBT Website Database Development Documentation

## Overview

The NBT (National Benchmark Tests) Website database has been successfully developed and deployed. This document provides comprehensive information about the database structure, setup, and usage.

## Database Information

- **Database Name**: NBTWebsite
- **Database Engine**: Microsoft SQL Server 2019 / LocalDB
- **Connection String**: `Server=(localdb)\\mssqllocaldb;Database=NBTWebsite;Trusted_Connection=true;MultipleActiveResultSets=true`
- **ORM**: Entity Framework Core 8.0
- **Migration Status**: ✅ Applied (Initial migration: 20251107113354_InitialCreate)

## Database Schema

### Identity Tables (ASP.NET Core Identity)

#### AspNetUsers
Extends the standard Identity user with custom NBT-specific fields.

| Column | Type | Description |
|--------|------|-------------|
| Id | uniqueidentifier | Primary Key |
| UserName | nvarchar(256) | Username |
| Email | nvarchar(256) | Email address |
| FirstName | nvarchar(max) | User's first name |
| LastName | nvarchar(max) | User's last name |
| Role | int | User role enum (Admin=1, Staff=2, InstitutionalUser=3) |
| InstitutionId | nvarchar(max) | Associated institution ID |
| Status | nvarchar(max) | Account status (Active, Inactive, Locked) |
| LastLoginDate | datetime2 | Last successful login timestamp |
| CreatedDate | datetime2 | Account creation date |
| PasswordResetToken | nvarchar(max) | Token for password reset |
| TokenExpiryDate | datetime2 | Token expiry timestamp |

#### AspNetRoles
Standard Identity roles table.

| Column | Type | Description |
|--------|------|-------------|
| Id | uniqueidentifier | Primary Key |
| Name | nvarchar(256) | Role name |
| NormalizedName | nvarchar(256) | Normalized role name |

**Seeded Roles:**
- Administrator
- Staff
- Educator
- Institution
- Applicant

#### AspNetUserRoles, AspNetUserClaims, AspNetUserLogins, AspNetUserTokens, AspNetRoleClaims
Standard ASP.NET Core Identity junction and support tables.

---

### Application Tables

#### ContentPages
Stores static content pages (About, Privacy Policy, Terms, etc.).

| Column | Type | Description |
|--------|------|-------------|
| Id | uniqueidentifier | Primary Key |
| Title | nvarchar(200) | Page title |
| Slug | nvarchar(250) | URL-friendly identifier (unique) |
| BodyContent | nvarchar(max) | HTML content |
| MetaDescription | nvarchar(500) | SEO meta description |
| Keywords | nvarchar(500) | SEO keywords |
| PublicationDate | datetime2 | Publication date |
| Status | nvarchar(50) | Status (Draft, Published, Archived) |
| CreatedDate | datetime2 | Creation timestamp |
| LastModifiedDate | datetime2 | Last modification timestamp |
| CreatedBy | nvarchar(256) | Creator username |
| LastModifiedBy | nvarchar(256) | Last modifier username |

**Indexes:**
- Unique index on `Slug`

**Seeded Pages:**
1. About NBT (slug: `about`)
2. Privacy Policy (slug: `privacy-policy`)
3. Terms and Conditions (slug: `terms-and-conditions`)

#### Announcements
Stores news, announcements, and updates.

| Column | Type | Description |
|--------|------|-------------|
| Id | uniqueidentifier | Primary Key |
| Title | nvarchar(250) | Announcement title |
| Summary | nvarchar(500) | Brief summary |
| FullContent | nvarchar(max) | Full HTML content |
| Category | nvarchar(50) | Category enum (PolicyUpdate=1, TestDates=2, FeeChanges=3, GeneralNews=4) |
| PublicationDate | datetime2 | Publication date |
| Status | nvarchar(50) | Status (Draft, Published, Archived) |
| IsFeatured | bit | Featured on homepage flag |
| CreatedDate | datetime2 | Creation timestamp |
| LastModifiedDate | datetime2 | Last modification timestamp |
| CreatedBy | nvarchar(256) | Creator username |
| LastModifiedBy | nvarchar(256) | Last modifier username |

**Indexes:**
- Index on `PublicationDate`
- Index on `Category`
- Index on `IsFeatured`

**Seeded Announcements:**
1. "2025 Test Dates Announced" (Featured)
2. "New Venues Added in Eastern Cape"
3. "Updated Registration Process" (Featured)

#### ContactInquiries
Stores contact form submissions and inquiries.

| Column | Type | Description |
|--------|------|-------------|
| Id | uniqueidentifier | Primary Key |
| SubmissionDateTime | datetime2 | Submission timestamp |
| Name | nvarchar(200) | Inquirer's name |
| Email | nvarchar(256) | Inquirer's email |
| Phone | nvarchar(20) | Inquirer's phone (optional) |
| InquiryType | nvarchar(50) | Type enum (General, Technical, Registration, Results, Institutional) |
| Subject | nvarchar(250) | Inquiry subject |
| Message | nvarchar(2000) | Inquiry message |
| Status | nvarchar(50) | Status enum (New, InProgress, Resolved, Closed) |
| AssignedToId | uniqueidentifier | Assigned staff member ID (nullable) |
| Response | nvarchar(2000) | Staff response (nullable) |
| PrivacyConsent | bit | Privacy policy consent flag |
| ReferenceNumber | nvarchar(50) | Unique tracking reference |
| CreatedDate | datetime2 | Creation timestamp |
| LastModifiedDate | datetime2 | Last modification timestamp |

**Indexes:**
- Unique index on `ReferenceNumber`
- Index on `Status`
- Index on `SubmissionDateTime`

#### DownloadableResources
Stores downloadable files (PDFs, documents, guides).

| Column | Type | Description |
|--------|------|-------------|
| Id | uniqueidentifier | Primary Key |
| Title | nvarchar(max) | Resource title |
| Description | nvarchar(max) | Resource description |
| FilePath | nvarchar(max) | File path or URL |
| FileType | nvarchar(max) | File type (PDF, DOCX, etc.) |
| FileSize | bigint | File size in bytes |
| Category | nvarchar(max) | Category (General, Educator, Institution) |
| UploadDate | datetime2 | Upload timestamp |
| DownloadCount | int | Download counter for analytics |
| Status | nvarchar(max) | Status (Active, Archived) |
| CreatedDate | datetime2 | Creation timestamp |
| LastModifiedDate | datetime2 | Last modification timestamp |
| CreatedBy | nvarchar(256) | Creator username |
| LastModifiedBy | nvarchar(256) | Last modifier username |

**Seeded Resources:**
1. NBT Information Brochure (2.4 MB PDF)
2. Sample Questions - Academic Literacy (1.5 MB PDF)
3. Sample Questions - Quantitative Literacy (1.8 MB PDF)
4. Educator Guide to NBT Results (3 MB PDF)
5. Institution Data Integration Specification (2 MB PDF)

---

## Entity Relationships

### Key Relationships:

1. **AspNetUsers ↔ AspNetRoles** (Many-to-Many via AspNetUserRoles)
   - Users can have multiple roles
   
2. **ContactInquiries → AspNetUsers** (Foreign Key: AssignedToId)
   - Inquiries can be assigned to staff members

3. **ContentPages, Announcements, DownloadableResources**
   - Track `CreatedBy` and `LastModifiedBy` as usernames
   - Implement soft audit trail

---

## Enumerations

### UserRole
```csharp
public enum UserRole
{
    Admin = 1,              // System administrator
    Staff = 2,              // NBT staff member
    InstitutionalUser = 3   // Higher education institution user
}
```

### AnnouncementCategory
```csharp
public enum AnnouncementCategory
{
    PolicyUpdate = 1,    // Policy updates
    TestDates = 2,       // Test dates and schedules
    FeeChanges = 3,      // Fee structure changes
    GeneralNews = 4      // General news
}
```

### InquiryType
```csharp
public enum InquiryType
{
    General = 1,        // General inquiries
    Technical = 2,      // Technical support
    Registration = 3,   // Registration issues
    Results = 4,        // Test results inquiries
    Institutional = 5   // Institutional queries
}
```

### InquiryStatus
```csharp
public enum InquiryStatus
{
    New = 1,         // New inquiry
    InProgress = 2,  // Being processed
    Resolved = 3,    // Resolved
    Closed = 4       // Closed/Archived
}
```

---

## Database Setup

### Automatic Setup (Recommended)

The database is automatically created and seeded when you run the application:

```bash
cd src\NBT.WebAPI
dotnet run
```

On first run:
1. ✅ Database `NBTWebsite` is created
2. ✅ All tables are created via EF Core migrations
3. ✅ Identity tables are configured
4. ✅ Seed data is inserted:
   - 5 user roles
   - 1 admin user (admin@nbt.ac.za / Admin@123!)
   - 3 content pages
   - 3 announcements
   - 5 downloadable resources

### Manual Setup

Execute SQL scripts in order:

```bash
# Script 1: Create Database
database-scripts\01-CreateDatabase.sql

# Script 2: Create Tables
database-scripts\02-CreateTables.sql

# Script 3: Seed Data
database-scripts\03-SeedData.sql

# Script 4: Verify Setup
database-scripts\04-VerifyDatabase.sql
```

---

## Database Verification

### Check Migration Status

```bash
cd src\NBT.Infrastructure
dotnet ef migrations list --startup-project ..\NBT.WebAPI
```

### Verify Row Counts

```sql
USE NBTWebsite;

SELECT 
    t.name AS TableName,
    SUM(p.rows) AS RowCount
FROM sys.tables t
INNER JOIN sys.partitions p ON t.object_id = p.object_id
WHERE t.is_ms_shipped = 0 AND p.index_id IN (0,1)
GROUP BY t.name
ORDER BY t.name;
```

**Expected Results:**
- Announcements: 3 rows
- ContentPages: 3 rows
- DownloadableResources: 5 rows
- AspNetRoles: 5 rows
- AspNetUsers: 1 row (admin)
- ContactInquiries: 0 rows (initially)

### Query Sample Data

```sql
-- View Content Pages
SELECT Id, Title, Slug, Status FROM ContentPages;

-- View Announcements
SELECT Id, Title, Category, IsFeatured, PublicationDate 
FROM Announcements 
ORDER BY PublicationDate DESC;

-- View Resources
SELECT Id, Title, Category, FileType, FileSize, DownloadCount
FROM DownloadableResources;

-- View Roles
SELECT Id, Name FROM AspNetRoles;

-- View Admin User
SELECT Id, UserName, Email, FirstName, LastName, Role 
FROM AspNetUsers;
```

---

## Backup and Restore

### Backup Database

```sql
BACKUP DATABASE NBTWebsite
TO DISK = 'C:\Backups\NBTWebsite_Full.bak'
WITH FORMAT,
     MEDIANAME = 'NBTWebsite_Backup',
     NAME = 'Full Backup of NBTWebsite';
```

### Restore Database

```sql
USE master;
GO

ALTER DATABASE NBTWebsite SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
GO

RESTORE DATABASE NBTWebsite
FROM DISK = 'C:\Backups\NBTWebsite_Full.bak'
WITH REPLACE;
GO

ALTER DATABASE NBTWebsite SET MULTI_USER;
GO
```

---

## Security Considerations

### Default Admin Credentials
⚠️ **IMPORTANT**: Change the default admin password immediately!

- **Email**: admin@nbt.ac.za
- **Password**: Admin@123!

### Password Policy
- Minimum 6 characters
- Requires uppercase letter
- Requires lowercase letter
- Requires digit
- Requires non-alphanumeric character

### Data Protection
- Passwords are hashed using ASP.NET Core Identity
- Personal information should be encrypted at rest (configure in production)
- Implement SSL/TLS for data in transit
- Regular security audits recommended

### Connection String Security
- Never commit connection strings to source control
- Use Azure Key Vault or environment variables in production
- Use Managed Identity for Azure SQL Database

---

## Maintenance

### Adding New Migrations

```bash
cd src\NBT.Infrastructure
dotnet ef migrations add MigrationName --startup-project ..\NBT.WebAPI
```

### Updating Database

```bash
cd src\NBT.Infrastructure
dotnet ef database update --startup-project ..\NBT.WebAPI
```

### Generating Migration SQL Script

```bash
cd src\NBT.Infrastructure
dotnet ef migrations script --startup-project ..\NBT.WebAPI --output ..\..\database-scripts\migration.sql
```

### Reset Database (Development Only)

```bash
cd src\NBT.Infrastructure
dotnet ef database drop --startup-project ..\NBT.WebAPI --force
dotnet ef database update --startup-project ..\NBT.WebAPI
```

---

## Performance Optimization

### Recommended Indexes
All critical indexes are already created:
- ContentPages: Slug (unique)
- Announcements: PublicationDate, Category, IsFeatured
- ContactInquiries: ReferenceNumber (unique), Status, SubmissionDateTime

### Query Optimization Tips
1. Use indexed columns in WHERE clauses
2. Avoid SELECT * - specify columns
3. Use pagination for large result sets
4. Enable query statistics for analysis

### Maintenance Tasks
```sql
-- Update Statistics
UPDATE STATISTICS Announcements;
UPDATE STATISTICS ContentPages;
UPDATE STATISTICS ContactInquiries;
UPDATE STATISTICS DownloadableResources;

-- Rebuild Indexes
ALTER INDEX ALL ON Announcements REBUILD;
ALTER INDEX ALL ON ContentPages REBUILD;
ALTER INDEX ALL ON ContactInquiries REBUILD;
ALTER INDEX ALL ON DownloadableResources REBUILD;
```

---

## Troubleshooting

### Cannot connect to LocalDB

```bash
# Check LocalDB instances
sqllocaldb info

# Start instance
sqllocaldb start mssqllocaldb

# Create instance if missing
sqllocaldb create mssqllocaldb
```

### Migration errors

```bash
# Remove last migration
dotnet ef migrations remove --startup-project ..\NBT.WebAPI

# Reset migrations (CAUTION: Data loss)
dotnet ef database drop --startup-project ..\NBT.WebAPI --force
dotnet ef migrations add InitialCreate --startup-project ..\NBT.WebAPI
dotnet ef database update --startup-project ..\NBT.WebAPI
```

### Connection timeout

Update connection string timeout:
```
Server=(localdb)\\mssqllocaldb;Database=NBTWebsite;Trusted_Connection=true;MultipleActiveResultSets=true;Connection Timeout=60
```

---

## Additional Resources

- [Entity Framework Core Documentation](https://docs.microsoft.com/ef/core/)
- [ASP.NET Core Identity](https://docs.microsoft.com/aspnet/core/security/authentication/identity)
- [SQL Server Best Practices](https://docs.microsoft.com/sql/relational-databases/security/sql-server-security-best-practices)

---

## Status Summary

✅ Database created: NBTWebsite  
✅ Tables created: 14 tables  
✅ Migrations applied: Initial migration  
✅ Seed data inserted: Complete  
✅ Indexes created: All recommended indexes  
✅ Relationships configured: Foreign keys and constraints  
✅ Identity configured: ASP.NET Core Identity with custom fields  
✅ Documentation: Complete  

**Database development is complete and ready for use!**
