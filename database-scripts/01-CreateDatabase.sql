-- =============================================
-- NBT Website Database Creation Script
-- =============================================
-- This script creates the NBTWebsite database
-- Execute this on SQL Server 2019 or later
-- =============================================

USE master;
GO

-- Drop database if it exists (use with caution in production)
IF EXISTS (SELECT name FROM sys.databases WHERE name = 'NBTWebsite')
BEGIN
    ALTER DATABASE NBTWebsite SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE NBTWebsite;
END
GO

-- Create the database (LocalDB)
CREATE DATABASE NBTWebsite;
GO

USE NBTWebsite;
GO

PRINT 'Database NBTWebsite created successfully.';
GO
