-- =============================================
-- NBT Website Seed Data Script
-- =============================================
-- Inserts initial seed data for development
-- =============================================

USE NBTWebsite;
GO

-- =============================================
-- Insert Roles
-- =============================================
DECLARE @AdminRoleId UNIQUEIDENTIFIER = NEWID();
DECLARE @StaffRoleId UNIQUEIDENTIFIER = NEWID();
DECLARE @EducatorRoleId UNIQUEIDENTIFIER = NEWID();
DECLARE @InstitutionRoleId UNIQUEIDENTIFIER = NEWID();
DECLARE @ApplicantRoleId UNIQUEIDENTIFIER = NEWID();

IF NOT EXISTS (SELECT * FROM AspNetRoles WHERE Name = 'Administrator')
BEGIN
    INSERT INTO AspNetRoles (Id, Name, NormalizedName)
    VALUES (@AdminRoleId, 'Administrator', 'ADMINISTRATOR');
    PRINT 'Administrator role created.';
END

IF NOT EXISTS (SELECT * FROM AspNetRoles WHERE Name = 'Staff')
BEGIN
    INSERT INTO AspNetRoles (Id, Name, NormalizedName)
    VALUES (@StaffRoleId, 'Staff', 'STAFF');
    PRINT 'Staff role created.';
END

IF NOT EXISTS (SELECT * FROM AspNetRoles WHERE Name = 'Educator')
BEGIN
    INSERT INTO AspNetRoles (Id, Name, NormalizedName)
    VALUES (@EducatorRoleId, 'Educator', 'EDUCATOR');
    PRINT 'Educator role created.';
END

IF NOT EXISTS (SELECT * FROM AspNetRoles WHERE Name = 'Institution')
BEGIN
    INSERT INTO AspNetRoles (Id, Name, NormalizedName)
    VALUES (@InstitutionRoleId, 'Institution', 'INSTITUTION');
    PRINT 'Institution role created.';
END

IF NOT EXISTS (SELECT * FROM AspNetRoles WHERE Name = 'Applicant')
BEGIN
    INSERT INTO AspNetRoles (Id, Name, NormalizedName)
    VALUES (@ApplicantRoleId, 'Applicant', 'APPLICANT');
    PRINT 'Applicant role created.';
END
GO

-- =============================================
-- Insert Content Pages
-- =============================================
IF NOT EXISTS (SELECT * FROM ContentPages WHERE Slug = 'about')
BEGIN
    INSERT INTO ContentPages (Id, Title, Slug, BodyContent, MetaDescription, Keywords, Status, PublicationDate, CreatedDate, CreatedBy)
    VALUES (
        NEWID(),
        'About NBT',
        'about',
        '<h2>About the National Benchmark Tests</h2>
<p>The National Benchmark Tests (NBT) project is a national initiative to assess academic readiness for university education in South Africa.</p>
<p>The tests complement and supplement the information provided by the National Senior Certificate (NSC).</p>
<h3>Purpose</h3>
<p>The NBTs are designed to provide a more holistic picture of an applicant''s academic potential and readiness for university-level study.</p>',
        'Learn about the National Benchmark Tests and their purpose in South African higher education.',
        'NBT, National Benchmark Tests, academic readiness, university admission',
        'Published',
        GETUTCDATE(),
        GETUTCDATE(),
        'System'
    );
    PRINT 'About page created.';
END

IF NOT EXISTS (SELECT * FROM ContentPages WHERE Slug = 'privacy-policy')
BEGIN
    INSERT INTO ContentPages (Id, Title, Slug, BodyContent, MetaDescription, Keywords, Status, PublicationDate, CreatedDate, CreatedBy)
    VALUES (
        NEWID(),
        'Privacy Policy',
        'privacy-policy',
        '<h2>Privacy Policy</h2>
<p>This privacy policy explains how NBT collects, uses, and protects your personal information.</p>
<h3>Information Collection</h3>
<p>We collect information that you provide when registering for the NBT tests or contacting us.</p>
<h3>Data Protection</h3>
<p>Your data is protected in accordance with POPIA (Protection of Personal Information Act) and GDPR standards.</p>',
        'NBT Privacy Policy - How we protect your personal information.',
        'privacy policy, data protection, POPIA, personal information',
        'Published',
        GETUTCDATE(),
        GETUTCDATE(),
        'System'
    );
    PRINT 'Privacy Policy page created.';
END

IF NOT EXISTS (SELECT * FROM ContentPages WHERE Slug = 'terms-and-conditions')
BEGIN
    INSERT INTO ContentPages (Id, Title, Slug, BodyContent, MetaDescription, Keywords, Status, PublicationDate, CreatedDate, CreatedBy)
    VALUES (
        NEWID(),
        'Terms and Conditions',
        'terms-and-conditions',
        '<h2>Terms and Conditions</h2>
<p>By using this website and registering for NBT tests, you agree to these terms and conditions.</p>
<h3>Test Registration</h3>
<p>All test registrations are subject to availability and payment of the required fees.</p>
<h3>Cancellation Policy</h3>
<p>Cancellations must be made at least 7 days before the scheduled test date for a refund.</p>',
        'NBT Terms and Conditions for test registration and website use.',
        'terms and conditions, test registration, NBT policies',
        'Published',
        GETUTCDATE(),
        GETUTCDATE(),
        'System'
    );
    PRINT 'Terms and Conditions page created.';
END
GO

-- =============================================
-- Insert Announcements
-- =============================================
IF NOT EXISTS (SELECT * FROM Announcements WHERE Title = '2025 Test Dates Announced')
BEGIN
    INSERT INTO Announcements (Id, Title, Summary, FullContent, Category, Status, IsFeatured, PublicationDate, CreatedDate, CreatedBy)
    VALUES (
        NEWID(),
        '2025 Test Dates Announced',
        'NBT test dates for 2025 have been announced. Register early to secure your preferred date and venue.',
        '<p>We are pleased to announce the NBT test dates for 2025. Tests will be conducted at venues across South Africa.</p>
<p>Early registration is encouraged as seats are limited. Visit the registration portal to book your test.</p>
<ul>
    <li>Term 1: February - March 2025</li>
    <li>Term 2: May - June 2025</li>
    <li>Term 3: August - September 2025</li>
</ul>',
        'GeneralNews',
        'Published',
        1,
        DATEADD(DAY, -5, GETUTCDATE()),
        DATEADD(DAY, -5, GETUTCDATE()),
        'System'
    );
    PRINT 'Announcement: 2025 Test Dates created.';
END

IF NOT EXISTS (SELECT * FROM Announcements WHERE Title = 'New Venues Added in Eastern Cape')
BEGIN
    INSERT INTO Announcements (Id, Title, Summary, FullContent, Category, Status, IsFeatured, PublicationDate, CreatedDate, CreatedBy)
    VALUES (
        NEWID(),
        'New Venues Added in Eastern Cape',
        'Additional test venues have been added in the Eastern Cape province to improve accessibility.',
        '<p>In response to increased demand, NBT has added new test venues in the Eastern Cape:</p>
<ul>
    <li>Mthatha</li>
    <li>East London</li>
    <li>Gqeberha (Port Elizabeth)</li>
</ul>
<p>These venues will be available for all 2025 test sessions.</p>',
        'TestVenues',
        'Published',
        0,
        DATEADD(DAY, -10, GETUTCDATE()),
        DATEADD(DAY, -10, GETUTCDATE()),
        'System'
    );
    PRINT 'Announcement: New Venues created.';
END

IF NOT EXISTS (SELECT * FROM Announcements WHERE Title = 'Updated Registration Process')
BEGIN
    INSERT INTO Announcements (Id, Title, Summary, FullContent, Category, Status, IsFeatured, PublicationDate, CreatedDate, CreatedBy)
    VALUES (
        NEWID(),
        'Updated Registration Process',
        'The NBT registration process has been streamlined for improved user experience.',
        '<p>We have updated our registration process to make it faster and more user-friendly:</p>
<ul>
    <li>Simplified online registration form</li>
    <li>Multiple payment options including EFT and credit card</li>
    <li>Instant confirmation emails</li>
    <li>Digital admit cards</li>
</ul>',
        'RegistrationUpdates',
        'Published',
        1,
        DATEADD(DAY, -2, GETUTCDATE()),
        DATEADD(DAY, -2, GETUTCDATE()),
        'System'
    );
    PRINT 'Announcement: Updated Registration created.';
END
GO

-- =============================================
-- Insert Downloadable Resources
-- =============================================
IF NOT EXISTS (SELECT * FROM DownloadableResources WHERE Title = 'NBT Information Brochure')
BEGIN
    INSERT INTO DownloadableResources (Id, Title, Description, FilePath, FileType, FileSize, Category, UploadDate, DownloadCount, Status, CreatedDate, CreatedBy)
    VALUES (
        NEWID(),
        'NBT Information Brochure',
        'Comprehensive information about the National Benchmark Tests, including test structure, content domains, and preparation tips.',
        '/resources/nbt-information-brochure.pdf',
        'PDF',
        2457600,
        'General',
        GETUTCDATE(),
        0,
        'Active',
        GETUTCDATE(),
        'System'
    );
    PRINT 'Resource: NBT Information Brochure created.';
END

IF NOT EXISTS (SELECT * FROM DownloadableResources WHERE Title = 'Sample Questions - Academic Literacy')
BEGIN
    INSERT INTO DownloadableResources (Id, Title, Description, FilePath, FileType, FileSize, Category, UploadDate, DownloadCount, Status, CreatedDate, CreatedBy)
    VALUES (
        NEWID(),
        'Sample Questions - Academic Literacy',
        'Sample questions for the Academic Literacy test component with answers and explanations.',
        '/resources/sample-questions-academic-literacy.pdf',
        'PDF',
        1536000,
        'Educator',
        GETUTCDATE(),
        0,
        'Active',
        GETUTCDATE(),
        'System'
    );
    PRINT 'Resource: Sample Questions - Academic Literacy created.';
END

IF NOT EXISTS (SELECT * FROM DownloadableResources WHERE Title = 'Sample Questions - Quantitative Literacy')
BEGIN
    INSERT INTO DownloadableResources (Id, Title, Description, FilePath, FileType, FileSize, Category, UploadDate, DownloadCount, Status, CreatedDate, CreatedBy)
    VALUES (
        NEWID(),
        'Sample Questions - Quantitative Literacy',
        'Sample questions for the Quantitative Literacy test component with detailed solutions.',
        '/resources/sample-questions-quantitative-literacy.pdf',
        'PDF',
        1843200,
        'Educator',
        GETUTCDATE(),
        0,
        'Active',
        GETUTCDATE(),
        'System'
    );
    PRINT 'Resource: Sample Questions - Quantitative Literacy created.';
END

IF NOT EXISTS (SELECT * FROM DownloadableResources WHERE Title = 'Educator Guide to NBT Results')
BEGIN
    INSERT INTO DownloadableResources (Id, Title, Description, FilePath, FileType, FileSize, Category, UploadDate, DownloadCount, Status, CreatedDate, CreatedBy)
    VALUES (
        NEWID(),
        'Educator Guide to NBT Results',
        'Guide for educators on how to interpret and use NBT results for student support and placement.',
        '/resources/educator-guide-nbt-results.pdf',
        'PDF',
        3072000,
        'Educator',
        GETUTCDATE(),
        0,
        'Active',
        GETUTCDATE(),
        'System'
    );
    PRINT 'Resource: Educator Guide to NBT Results created.';
END

IF NOT EXISTS (SELECT * FROM DownloadableResources WHERE Title = 'Institution Data Integration Specification')
BEGIN
    INSERT INTO DownloadableResources (Id, Title, Description, FilePath, FileType, FileSize, Category, UploadDate, DownloadCount, Status, CreatedDate, CreatedBy)
    VALUES (
        NEWID(),
        'Institution Data Integration Specification',
        'Technical specification for institutions integrating NBT results into their admission systems.',
        '/resources/institution-data-integration-spec.pdf',
        'PDF',
        2048000,
        'Institution',
        GETUTCDATE(),
        0,
        'Active',
        GETUTCDATE(),
        'System'
    );
    PRINT 'Resource: Institution Data Integration Specification created.';
END
GO

PRINT 'Seed data inserted successfully.';
GO
