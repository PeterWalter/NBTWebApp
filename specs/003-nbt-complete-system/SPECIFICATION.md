# NBT Integrated Web Application - Complete Specification

## Executive Summary
Complete Blazor WebAssembly Interactive Auto application for the National Benchmark Tests (NBT) system. This specification extends the existing project shell to ensure all missing features, services, and data bindings are identified and implemented.

## 1. User Workflows

### 1.1 Student/Applicant Journey
**Registration & Account Creation**
1. Landing page with menus: Applicants, Institutions, Educators (with submenus matching current NBT website)
2. Account creation with SA ID or Foreign ID/Passport
3. Duplicate prevention and OTP verification
4. NBT Number generation (14-digit Luhn-validated) upon successful registration

**Registration Wizard** (Multi-Step Progressive)
- **Step 1: Account & Personal Information Combined**
  - Email, password, password confirmation
  - Personal details: Full name, Date of Birth, Gender, Ethnicity
  - SA ID (auto-extracts DOB and Gender) OR Foreign ID/Passport
  - Age calculation from DOB (not manually entered)
  - Address and contact information
  - Auto-save progress at each step
  
- **Step 2: Academic & Test Preferences Combined**
  - Academic background (high school, grade, subjects)
  - Test type selection: AQL only OR AQL and MAT
  - Venue selection with date availability
  - Special accommodation requests
  - Auto-save progress

- **Step 3: Background Survey Questionnaire**
  - Pre-test questionnaire for research and equity reporting
  - Optional but encouraged
  - Auto-save progress

**Registration Recovery**
- If registration interrupted, student can log in and resume from last completed step
- No need to redo completed sections
- Progress indicator shows completion status

**Booking & Payment**
- Test booking workflow:
  1. Select test type (AQL or AQL+MAT)
  2. Choose venue and date (online or physical)
  3. Review booking details
  4. Generate EasyPay payment reference
  5. Make payment (installments allowed)
  6. Receive confirmation
- Rules enforced:
  - One active booking at a time
  - Can book next test only after current booking closing date
  - Maximum 2 tests per year
  - Tests valid for 3 years
  - Booking changes before closing date
- Payment status tracking and updates
- Bank payment file upload support

**Special/Remote Sessions**
- Complete special session request form
- Provide invigilator details
- Specify remote venue information
- Auto-route to NBT remote administration team

**Dashboard & Navigation**
- Upon login, student taken to dashboard
- Left-side menu with navigation
- Dashboard sections:
  - Profile summary
  - Active bookings
  - Payment status
  - Test history
  - Results access
  - Document uploads

**Results Access**
- View test results once released
- Download PDF certificate (only for fully paid tests)
- Results display:
  - AQL test: AL and QL scores with performance levels
  - Math test: AL, QL, and MAT scores with performance levels
  - Performance levels: Basic Lower/Upper, Intermediate Lower/Upper, Proficient Lower/Upper
  - Barcode identifying specific test/answer sheet
  - Test date and venue information

**Profile Management**
- Update personal details
- Upload supporting documents
- Password reset
- Email/phone verification
- View audit log of changes

**Notifications**
- Email/SMS for:
  - Registration confirmation
  - NBT number assignment
  - Booking confirmation
  - Payment received
  - Test reminders (7 days, 1 day before)
  - Results availability
  - Profile changes

### 1.2 Staff Workflows
**Dashboard**
- Left-side menu navigation
- Overview statistics
- Recent activities
- Task queue

**Student Management**
- View all applicants
- Search and filter
- CRUD operations on student data
- View registration status
- Access to partial registrations
- Communication tools

**Booking Management**
- View all bookings
- Filter by date, venue, test type
- Modify bookings (with audit log)
- Cancel bookings
- Special session management

**Payment Processing**
- View payment records
- Process manual payments
- Upload bank payment files
- Reconcile payments
- Generate payment reports
- Track installment payments
- View payment history by student

**Results Management**
- Import test results (bulk upload)
- View results by student
- Generate result reports
- Export results data
- Barcode-based result tracking
- Performance level assignment

**Venue Management**
- View venue schedules
- Check capacity
- Manage room allocations
- Update venue availability

### 1.3 Admin Workflows
**All Staff Capabilities Plus:**
- User management (create/edit/disable users)
- Role assignment
- Venue CRUD operations
- Test date calendar management
- System configuration
- Report generation and exports
- Audit log access
- Test cost configuration by intake year

**Advanced Features:**
- Dashboard analytics
- Data exports (Excel, PDF)
- System health monitoring
- Bulk operations
- Custom report builder

### 1.4 SuperUser Workflows
**All Admin Capabilities Plus:**
- System configuration
- Integration management
- Database maintenance
- Advanced audit trails
- Security settings
- Performance monitoring

## 2. Functional Modules

### 2.1 Registration Wizard Module
**Components:**
- `RegistrationWizard.razor` - Main wizard container
- `Step1AccountPersonal.razor` - Account and personal info
- `Step2AcademicTest.razor` - Academic background and test selection
- `Step3Survey.razor` - Background questionnaire
- `WizardNavigation.razor` - Navigation component
- `ProgressIndicator.razor` - Visual progress tracker

**Services:**
- `IRegistrationService` - Registration operations
- `INbtNumberGenerator` - NBT number generation with Luhn
- `ISaIdValidator` - SA ID validation and data extraction
- `IWizardStateService` - Save/restore wizard progress

**ViewModels:**
- `RegistrationWizardViewModel` - Main wizard state
- `AccountPersonalViewModel` - Step 1 data
- `AcademicTestViewModel` - Step 2 data
- `SurveyViewModel` - Step 3 data

**Validation:**
- Email format and uniqueness
- Password strength requirements
- SA ID Luhn validation and DOB/Gender extraction
- Foreign ID/Passport validation
- Phone number format
- Required field validation
- Age verification (from DOB)

### 2.2 Booking & Payment Module
**Components:**
- `BookingWizard.razor` - Booking workflow
- `TestSelection.razor` - Test type selection
- `VenueSelection.razor` - Venue and date picker
- `BookingReview.razor` - Review booking details
- `PaymentIntegration.razor` - EasyPay integration
- `PaymentStatus.razor` - Payment tracking
- `PaymentUpload.razor` - Bank payment file upload

**Services:**
- `IBookingService` - Booking CRUD operations
- `IPaymentService` - Payment processing
- `IEasyPayService` - EasyPay integration
- `IPaymentFileProcessor` - Bank file parsing
- `IVenueAvailabilityService` - Date/venue availability

**ViewModels:**
- `BookingViewModel` - Booking data
- `PaymentViewModel` - Payment information
- `VenueAvailabilityViewModel` - Available dates/venues

**Business Logic:**
- One active booking enforcement
- Test per year limit (2 max)
- 3-year validity tracking
- Closing date validation
- Installment payment tracking
- Payment to test association
- Cost calculation by intake year

### 2.3 Staff/Admin Dashboard Module
**Components:**
- `StaffDashboard.razor` - Staff landing page
- `AdminDashboard.razor` - Admin landing page
- `StudentManagement.razor` - Student CRUD
- `BookingManagement.razor` - Booking operations
- `PaymentManagement.razor` - Payment processing
- `ResultsManagement.razor` - Results import/view
- `VenueManagement.razor` - Venue administration
- `UserManagement.razor` - User/role admin
- `AuditLog.razor` - Audit trail viewer
- `SideMenu.razor` - Left-side navigation menu

**Services:**
- `IStudentManagementService` - Student operations
- `IBookingManagementService` - Booking admin
- `IPaymentManagementService` - Payment admin
- `IResultsImportService` - Bulk result import
- `IVenueManagementService` - Venue admin
- `IUserManagementService` - User/role management
- `IAuditService` - Audit logging

**ViewModels:**
- `DashboardStatisticsViewModel` - Overview stats
- `StudentListViewModel` - Student grid data
- `BookingListViewModel` - Booking grid data
- `PaymentListViewModel` - Payment records

### 2.4 Venue & Room Management Module
**Components:**
- `VenueList.razor` - Venue listing
- `VenueEditor.razor` - Create/edit venue
- `VenueCalendar.razor` - Availability calendar
- `TestDateManager.razor` - Test date configuration
- `OnlineTestScheduler.razor` - Online test dates

**Services:**
- `IVenueService` - Venue CRUD
- `ITestDateService` - Test date management
- `ICapacityService` - Venue capacity tracking

**ViewModels:**
- `VenueViewModel` - Venue details
- `TestDateViewModel` - Test date with closing date
- `VenueAvailabilityViewModel` - Calendar data

**Venue Types:**
- National
- Special Session
- Research
- Other

**Calendar Features:**
- Highlight Sunday tests
- Highlight Online tests
- Show closing booking dates
- Venue availability by date

### 2.5 Reporting & Analytics Module
**Components:**
- `ReportDashboard.razor` - Report selection
- `StudentReport.razor` - Student analytics
- `BookingReport.razor` - Booking statistics
- `PaymentReport.razor` - Financial reports
- `ResultsReport.razor` - Performance analytics
- `VenueReport.razor` - Venue utilization
- `CustomReportBuilder.razor` - Ad-hoc reports

**Services:**
- `IReportingService` - Report generation
- `IExcelExportService` - Excel exports
- `IPdfExportService` - PDF generation
- `IAnalyticsService` - Data analytics

**Export Formats:**
- Excel (XLSX)
- PDF
- CSV
- JSON

**Report Types:**
- Registration summary
- Booking statistics
- Payment reconciliation
- Test results analysis
- Venue utilization
- User activity
- Audit reports

### 2.6 Results Management Module
**Components:**
- `ResultsImport.razor` - Bulk import interface
- `ResultsViewer.razor` - View student results
- `ResultsCertificate.razor` - PDF certificate generator
- `PerformanceLevelEditor.razor` - Performance level config

**Services:**
- `IResultsService` - Results CRUD
- `IResultsImportService` - Bulk import
- `ICertificateService` - PDF certificate generation
- `IBarcodeService` - Barcode generation/validation

**Result Structure:**
- Student NBT number
- Test type (AQL, AQL+MAT)
- Test date and venue
- Barcode (unique per test/answer sheet)
- Domain scores:
  - AL (Academic Literacy) with performance level
  - QL (Quantitative Literacy) with performance level
  - MAT (Mathematics) with performance level (if applicable)
- Performance levels:
  - Basic Lower
  - Basic Upper
  - Intermediate Lower
  - Intermediate Upper
  - Proficient Lower
  - Proficient Upper

**Certificate Features:**
- Available only for fully paid tests (student view)
- Staff/Admin can view all certificates
- PDF download
- Official NBT branding
- Barcode included
- Verification code

### 2.7 Security & Authentication Module
**Components:**
- `Login.razor` - Login page
- `Register.razor` - Account creation
- `ForgotPassword.razor` - Password reset
- `ChangePassword.razor` - Password change
- `TwoFactorAuth.razor` - 2FA setup

**Services:**
- `IAuthenticationService` - JWT authentication
- `IAuthorizationService` - Role-based access
- `ITokenService` - JWT token management
- `IPasswordService` - Password hashing/validation
- `IOtpService` - OTP generation/verification

**Roles:**
- Student/Applicant
- Staff
- Admin
- SuperUser

**Permissions:**
- View own data
- View all data
- Edit own data
- Edit all data
- Delete data
- Manage users
- System configuration

## 3. Landing Page & Public Content

### 3.1 Landing Page Structure
**Main Menus:**
- **Applicants** (with submenus matching current NBT website)
  - About the Tests
  - How to Register
  - Test Dates & Venues
  - Prepare for Tests
  - Results & Certificates
  - FAQs
  - Contact Us

- **Institutions** (with submenus)
  - For Universities
  - For Colleges
  - Using NBT Results
  - Research & Reports
  - Partnership Opportunities
  - Contact Us

- **Educators** (with submenus)
  - Teaching Resources
  - Preparation Materials
  - Professional Development
  - Research Publications
  - Educator Portal
  - Contact Us

**Content Features:**
- Responsive design
- Video integration (where available from current NBT website)
- Dynamic content management
- News and announcements
- Quick links
- Call-to-action buttons

### 3.2 Public Pages
- About NBT
- Test Information
- Registration Guide
- Payment Information
- Special Sessions
- Contact Information
- Privacy Policy
- Terms of Service
- Cookie Policy

## 4. API Endpoints

### 4.1 Registration APIs
```
POST   /api/registration/create          # Create new registration
PUT    /api/registration/{id}            # Update registration
GET    /api/registration/{id}            # Get registration
POST   /api/registration/generate-nbt    # Generate NBT number
POST   /api/registration/validate-said   # Validate SA ID
GET    /api/registration/resume/{userId} # Resume interrupted registration
```

### 4.2 Booking APIs
```
POST   /api/booking/create               # Create booking
PUT    /api/booking/{id}                 # Update booking
DELETE /api/booking/{id}                 # Cancel booking
GET    /api/booking/{id}                 # Get booking
GET    /api/booking/student/{studentId}  # Get student bookings
GET    /api/booking/venue/{venueId}      # Get venue bookings
```

### 4.3 Payment APIs
```
POST   /api/payment/create               # Record payment
GET    /api/payment/{id}                 # Get payment
GET    /api/payment/student/{studentId}  # Get student payments
POST   /api/payment/easypay-callback     # EasyPay webhook
POST   /api/payment/upload-file          # Upload bank payment file
GET    /api/payment/status/{bookingId}   # Check payment status
```

### 4.4 Results APIs
```
POST   /api/results/import               # Bulk import results
GET    /api/results/{id}                 # Get result
GET    /api/results/student/{studentId}  # Get student results
GET    /api/results/certificate/{id}     # Generate certificate PDF
GET    /api/results/verify/{barcode}     # Verify result by barcode
```

### 4.5 Venue APIs
```
POST   /api/venue/create                 # Create venue
PUT    /api/venue/{id}                   # Update venue
DELETE /api/venue/{id}                   # Delete venue
GET    /api/venue/{id}                   # Get venue
GET    /api/venue/list                   # List venues
GET    /api/venue/availability/{venueId} # Check availability
POST   /api/venue/test-dates             # Manage test dates
```

### 4.6 Staff/Admin APIs
```
GET    /api/students/list                # List all students
GET    /api/students/{id}                # Get student
PUT    /api/students/{id}                # Update student
POST   /api/users/create                 # Create user
PUT    /api/users/{id}/role              # Update user role
GET    /api/audit/logs                   # Get audit logs
```

### 4.7 Report APIs
```
GET    /api/reports/registrations        # Registration report
GET    /api/reports/bookings             # Booking report
GET    /api/reports/payments             # Payment report
GET    /api/reports/results              # Results report
POST   /api/reports/custom               # Custom report
GET    /api/reports/export/{id}          # Export report
```

## 5. Data Models

### 5.1 Core Entities
- **Student**: Personal information, NBT number, account details
- **Registration**: Registration data, wizard progress
- **Booking**: Test booking details, status
- **Payment**: Payment records, installments, EasyPay reference
- **Result**: Test results, performance levels, barcode
- **Venue**: Venue details, type, capacity
- **TestSession**: Test date, venue link, session details
- **TestDate**: Available test dates with closing dates
- **User**: Authentication and role information
- **AuditLog**: Change tracking

### 5.2 Supporting Entities
- **SpecialSession**: Remote/special test arrangements
- **Document**: Uploaded supporting documents
- **Notification**: Email/SMS notifications
- **PaymentInstallment**: Installment tracking
- **PerformanceLevel**: Performance level definitions
- **IntakeYear**: Yearly test cost configuration

## 6. Integration Points

### 6.1 EasyPay Integration
- Payment reference generation
- Webhook for payment confirmations
- Status updates
- Reconciliation

### 6.2 Email/SMS Service
- Transactional emails
- SMS notifications
- Template management
- Delivery tracking

### 6.3 File Storage
- Document uploads
- Result files
- Payment files
- Certificate PDFs

### 6.4 Reporting Services
- Excel generation
- PDF generation
- Data analytics

## 7. Quality Requirements

### 7.1 Performance
- Page load: < 3 seconds
- API response: < 500ms
- Concurrent users: 1000+
- Database optimization

### 7.2 Security
- HTTPS only
- JWT authentication
- Role-based authorization
- Audit logging
- Data encryption

### 7.3 Accessibility
- WCAG 2.1 AA compliance
- Screen reader support
- Keyboard navigation
- High contrast mode

### 7.4 Usability
- Intuitive navigation
- Clear error messages
- Progress indicators
- Help documentation
- Mobile responsive

## 8. Success Criteria
- All workflows operational end-to-end
- Zero critical bugs
- 80%+ test coverage
- WCAG 2.1 AA compliance
- < 3s page load time
- Successful UAT completion
- Production deployment ready
