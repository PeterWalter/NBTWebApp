-- =============================================
-- NBT Website Table Creation Script
-- =============================================
-- Creates all tables for the NBT Website
-- This matches the Entity Framework migrations
-- =============================================

USE NBTWebsite;
GO

-- =============================================
-- Identity Tables (ASP.NET Core Identity)
-- =============================================

-- AspNetRoles
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'AspNetRoles')
BEGIN
    CREATE TABLE AspNetRoles (
        Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
        Name NVARCHAR(256) NULL,
        NormalizedName NVARCHAR(256) NULL,
        ConcurrencyStamp NVARCHAR(MAX) NULL
    );
    CREATE UNIQUE INDEX IX_AspNetRoles_NormalizedName ON AspNetRoles(NormalizedName) WHERE NormalizedName IS NOT NULL;
    PRINT 'Table AspNetRoles created.';
END
GO

-- AspNetUsers
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'AspNetUsers')
BEGIN
    CREATE TABLE AspNetUsers (
        Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
        UserName NVARCHAR(256) NULL,
        NormalizedUserName NVARCHAR(256) NULL,
        Email NVARCHAR(256) NULL,
        NormalizedEmail NVARCHAR(256) NULL,
        EmailConfirmed BIT NOT NULL DEFAULT 0,
        PasswordHash NVARCHAR(MAX) NULL,
        SecurityStamp NVARCHAR(MAX) NULL,
        ConcurrencyStamp NVARCHAR(MAX) NULL,
        PhoneNumber NVARCHAR(MAX) NULL,
        PhoneNumberConfirmed BIT NOT NULL DEFAULT 0,
        TwoFactorEnabled BIT NOT NULL DEFAULT 0,
        LockoutEnd DATETIMEOFFSET(7) NULL,
        LockoutEnabled BIT NOT NULL DEFAULT 0,
        AccessFailedCount INT NOT NULL DEFAULT 0,
        -- Custom fields
        FirstName NVARCHAR(MAX) NOT NULL DEFAULT '',
        LastName NVARCHAR(MAX) NOT NULL DEFAULT '',
        Role INT NOT NULL DEFAULT 0,
        InstitutionId NVARCHAR(MAX) NULL,
        Status NVARCHAR(MAX) NOT NULL DEFAULT 'Active',
        LastLoginDate DATETIME2(7) NULL,
        CreatedDate DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
        PasswordResetToken NVARCHAR(MAX) NULL,
        TokenExpiryDate DATETIME2(7) NULL
    );
    CREATE INDEX IX_AspNetUsers_NormalizedUserName ON AspNetUsers(NormalizedUserName);
    CREATE INDEX IX_AspNetUsers_NormalizedEmail ON AspNetUsers(NormalizedEmail);
    PRINT 'Table AspNetUsers created.';
END
GO

-- AspNetUserRoles
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'AspNetUserRoles')
BEGIN
    CREATE TABLE AspNetUserRoles (
        UserId UNIQUEIDENTIFIER NOT NULL,
        RoleId UNIQUEIDENTIFIER NOT NULL,
        PRIMARY KEY (UserId, RoleId),
        CONSTRAINT FK_AspNetUserRoles_AspNetUsers FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id) ON DELETE CASCADE,
        CONSTRAINT FK_AspNetUserRoles_AspNetRoles FOREIGN KEY (RoleId) REFERENCES AspNetRoles(Id) ON DELETE CASCADE
    );
    CREATE INDEX IX_AspNetUserRoles_RoleId ON AspNetUserRoles(RoleId);
    PRINT 'Table AspNetUserRoles created.';
END
GO

-- AspNetUserClaims
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'AspNetUserClaims')
BEGIN
    CREATE TABLE AspNetUserClaims (
        Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        UserId UNIQUEIDENTIFIER NOT NULL,
        ClaimType NVARCHAR(MAX) NULL,
        ClaimValue NVARCHAR(MAX) NULL,
        CONSTRAINT FK_AspNetUserClaims_AspNetUsers FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id) ON DELETE CASCADE
    );
    CREATE INDEX IX_AspNetUserClaims_UserId ON AspNetUserClaims(UserId);
    PRINT 'Table AspNetUserClaims created.';
END
GO

-- AspNetUserLogins
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'AspNetUserLogins')
BEGIN
    CREATE TABLE AspNetUserLogins (
        LoginProvider NVARCHAR(450) NOT NULL,
        ProviderKey NVARCHAR(450) NOT NULL,
        ProviderDisplayName NVARCHAR(MAX) NULL,
        UserId UNIQUEIDENTIFIER NOT NULL,
        PRIMARY KEY (LoginProvider, ProviderKey),
        CONSTRAINT FK_AspNetUserLogins_AspNetUsers FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id) ON DELETE CASCADE
    );
    CREATE INDEX IX_AspNetUserLogins_UserId ON AspNetUserLogins(UserId);
    PRINT 'Table AspNetUserLogins created.';
END
GO

-- AspNetUserTokens
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'AspNetUserTokens')
BEGIN
    CREATE TABLE AspNetUserTokens (
        UserId UNIQUEIDENTIFIER NOT NULL,
        LoginProvider NVARCHAR(450) NOT NULL,
        Name NVARCHAR(450) NOT NULL,
        Value NVARCHAR(MAX) NULL,
        PRIMARY KEY (UserId, LoginProvider, Name),
        CONSTRAINT FK_AspNetUserTokens_AspNetUsers FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id) ON DELETE CASCADE
    );
    PRINT 'Table AspNetUserTokens created.';
END
GO

-- AspNetRoleClaims
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'AspNetRoleClaims')
BEGIN
    CREATE TABLE AspNetRoleClaims (
        Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        RoleId UNIQUEIDENTIFIER NOT NULL,
        ClaimType NVARCHAR(MAX) NULL,
        ClaimValue NVARCHAR(MAX) NULL,
        CONSTRAINT FK_AspNetRoleClaims_AspNetRoles FOREIGN KEY (RoleId) REFERENCES AspNetRoles(Id) ON DELETE CASCADE
    );
    CREATE INDEX IX_AspNetRoleClaims_RoleId ON AspNetRoleClaims(RoleId);
    PRINT 'Table AspNetRoleClaims created.';
END
GO

-- =============================================
-- Application Tables
-- =============================================

-- ContentPages
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ContentPages')
BEGIN
    CREATE TABLE ContentPages (
        Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
        Title NVARCHAR(200) NOT NULL,
        Slug NVARCHAR(250) NOT NULL,
        BodyContent NVARCHAR(MAX) NOT NULL,
        MetaDescription NVARCHAR(500) NULL,
        Keywords NVARCHAR(500) NULL,
        PublicationDate DATETIME2(7) NOT NULL,
        Status NVARCHAR(50) NOT NULL DEFAULT 'Draft',
        CreatedDate DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
        LastModifiedDate DATETIME2(7) NULL,
        CreatedBy NVARCHAR(256) NULL,
        LastModifiedBy NVARCHAR(256) NULL
    );
    CREATE UNIQUE INDEX IX_ContentPages_Slug ON ContentPages(Slug);
    PRINT 'Table ContentPages created.';
END
GO

-- Announcements
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Announcements')
BEGIN
    CREATE TABLE Announcements (
        Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
        Title NVARCHAR(250) NOT NULL,
        Summary NVARCHAR(500) NOT NULL,
        FullContent NVARCHAR(MAX) NOT NULL,
        Category NVARCHAR(50) NOT NULL,
        PublicationDate DATETIME2(7) NOT NULL,
        Status NVARCHAR(50) NOT NULL DEFAULT 'Draft',
        IsFeatured BIT NOT NULL DEFAULT 0,
        CreatedDate DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
        LastModifiedDate DATETIME2(7) NULL,
        CreatedBy NVARCHAR(256) NULL,
        LastModifiedBy NVARCHAR(256) NULL
    );
    CREATE INDEX IX_Announcements_PublicationDate ON Announcements(PublicationDate);
    CREATE INDEX IX_Announcements_Category ON Announcements(Category);
    CREATE INDEX IX_Announcements_IsFeatured ON Announcements(IsFeatured);
    PRINT 'Table Announcements created.';
END
GO

-- ContactInquiries
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ContactInquiries')
BEGIN
    CREATE TABLE ContactInquiries (
        Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
        SubmissionDateTime DATETIME2(7) NOT NULL,
        Name NVARCHAR(200) NOT NULL,
        Email NVARCHAR(256) NOT NULL,
        Phone NVARCHAR(20) NULL,
        InquiryType NVARCHAR(50) NOT NULL,
        Subject NVARCHAR(250) NOT NULL,
        Message NVARCHAR(2000) NOT NULL,
        Status NVARCHAR(50) NOT NULL,
        AssignedToId UNIQUEIDENTIFIER NULL,
        Response NVARCHAR(2000) NULL,
        PrivacyConsent BIT NOT NULL,
        ReferenceNumber NVARCHAR(50) NOT NULL,
        CreatedDate DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
        LastModifiedDate DATETIME2(7) NULL
    );
    CREATE UNIQUE INDEX IX_ContactInquiries_ReferenceNumber ON ContactInquiries(ReferenceNumber);
    CREATE INDEX IX_ContactInquiries_Status ON ContactInquiries(Status);
    CREATE INDEX IX_ContactInquiries_SubmissionDateTime ON ContactInquiries(SubmissionDateTime);
    PRINT 'Table ContactInquiries created.';
END
GO

-- DownloadableResources
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'DownloadableResources')
BEGIN
    CREATE TABLE DownloadableResources (
        Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
        Title NVARCHAR(MAX) NOT NULL,
        Description NVARCHAR(MAX) NOT NULL,
        FilePath NVARCHAR(MAX) NOT NULL,
        FileType NVARCHAR(MAX) NOT NULL,
        FileSize BIGINT NOT NULL,
        Category NVARCHAR(MAX) NOT NULL,
        UploadDate DATETIME2(7) NOT NULL,
        DownloadCount INT NOT NULL DEFAULT 0,
        Status NVARCHAR(MAX) NOT NULL DEFAULT 'Active',
        CreatedDate DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
        LastModifiedDate DATETIME2(7) NULL,
        CreatedBy NVARCHAR(256) NULL,
        LastModifiedBy NVARCHAR(256) NULL
    );
    PRINT 'Table DownloadableResources created.';
END
GO

PRINT 'All tables created successfully.';
GO
