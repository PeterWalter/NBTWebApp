-- =============================================
-- NBT Website Database Verification Script
-- =============================================
-- Run this script to verify database structure
-- and check initial data
-- =============================================

USE NBTWebsite;
GO

PRINT '========================================';
PRINT 'NBT Website Database Verification';
PRINT '========================================';
PRINT '';

-- Check Database exists
PRINT 'Database: ' + DB_NAME();
PRINT '';

-- Check Tables
PRINT 'Checking Tables...';
PRINT '========================================';

SELECT 
    t.name AS TableName,
    SUM(p.rows) AS RowCount
FROM 
    sys.tables t
    INNER JOIN sys.partitions p ON t.object_id = p.object_id
WHERE 
    t.is_ms_shipped = 0
    AND p.index_id IN (0,1)
GROUP BY 
    t.name
ORDER BY 
    t.name;

PRINT '';
PRINT 'Checking Content Pages...';
PRINT '========================================';
SELECT Id, Title, Slug, Status, CreatedDate
FROM ContentPages
ORDER BY Title;

PRINT '';
PRINT 'Checking Announcements...';
PRINT '========================================';
SELECT Id, Title, Category, Status, IsFeatured, PublicationDate
FROM Announcements
ORDER BY PublicationDate DESC;

PRINT '';
PRINT 'Checking Downloadable Resources...';
PRINT '========================================';
SELECT Id, Title, Category, FileType, FileSize, DownloadCount
FROM DownloadableResources
ORDER BY Title;

PRINT '';
PRINT 'Checking Roles...';
PRINT '========================================';
SELECT Id, Name, NormalizedName
FROM AspNetRoles
ORDER BY Name;

PRINT '';
PRINT 'Checking Users...';
PRINT '========================================';
SELECT Id, UserName, Email, FirstName, LastName, Role, Status, CreatedDate
FROM AspNetUsers
ORDER BY CreatedDate;

PRINT '';
PRINT 'Checking Contact Inquiries...';
PRINT '========================================';
SELECT 
    COUNT(*) AS TotalInquiries,
    SUM(CASE WHEN Status = 'New' THEN 1 ELSE 0 END) AS NewInquiries,
    SUM(CASE WHEN Status = 'InProgress' THEN 1 ELSE 0 END) AS InProgressInquiries,
    SUM(CASE WHEN Status = 'Resolved' THEN 1 ELSE 0 END) AS ResolvedInquiries
FROM ContactInquiries;

PRINT '';
PRINT '========================================';
PRINT 'Verification Complete';
PRINT '========================================';
