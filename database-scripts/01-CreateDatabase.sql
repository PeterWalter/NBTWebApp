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

-- Create the database
CREATE DATABASE NBTWebsite
ON PRIMARY 
(
    NAME = N'NBTWebsite_Data',
    FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\NBTWebsite.mdf',
    SIZE = 100MB,
    MAXSIZE = UNLIMITED,
    FILEGROWTH = 10MB
)
LOG ON 
(
    NAME = N'NBTWebsite_Log',
    FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\NBTWebsite_log.ldf',
    SIZE = 50MB,
    MAXSIZE = 500MB,
    FILEGROWTH = 10MB
);
GO

-- Set database options
ALTER DATABASE NBTWebsite SET RECOVERY FULL;
ALTER DATABASE NBTWebsite SET AUTO_CREATE_STATISTICS ON;
ALTER DATABASE NBTWebsite SET AUTO_UPDATE_STATISTICS ON;
GO

USE NBTWebsite;
GO

PRINT 'Database NBTWebsite created successfully.';
GO
