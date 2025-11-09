# NBT Integrated Web Application - Complete Specification

## Document Control
- **Version**: 1.0
- **Date**: 2025-11-09
- **Status**: ACTIVE
- **Extends**: Constitution v1.0
- **Project Type**: Blazor Web App Interactive Auto + ASP.NET Core Web API

---

## 1. SYSTEM OVERVIEW

### 1.1 Purpose
The NBT Integrated Web Application provides a complete end-to-end digital platform for the National Benchmark Tests (NBT) program, enabling:
- Student self-service registration and booking
- Automated payment processing and tracking
- Test administration and result distribution
- Venue and session management
- Comprehensive reporting and analytics
- Staff and administrative operations

### 1.2 Technology Stack
- **Frontend**: Blazor WebAssembly with Interactive Auto rendering
- **UI Framework**: Microsoft Fluent UI components
- **Backend**: ASP.NET Core Web API (.NET 9)
- **Database**: MS SQL Server with Entity Framework Core
- **Authentication**: JWT tokens with role-based authorization
- **Payment Gateway**: EasyPay integration
- **Hosting**: Azure App Service + Azure SQL Database

### 1.3 User Roles
1. **Student/Applicant** - Register, book tests, make payments, view results
2. **Staff** - Manage bookings, payments, venues, and results
3. **Admin** - Full CRUD operations, reporting, system configuration
4. **SuperUser** - System administration, user management, audit access
5. **Invigilator** - Special session coordination (future phase)
6. **Institution** - Bulk result access (future phase)

---

## 2. FUNCTIONAL SPECIFICATIONS

### 2.1 Student Registration Wizard

#### 2.1.1 Wizard Structure (Revised)
**Step 1: Account & Personal Information (Combined)**
- Account creation (email, password, confirm password)
- Personal details: First Name, Last Name, Date of Birth
- ID Type selection: SA ID, Foreign ID, or Passport ID
- ID Number entry (with validation)
- Auto-extraction: DOB and Gender from SA ID if applicable
- Manual entry: Gender, Ethnicity, Age (calculated from DOB)
- Contact: Phone Number, Alternative Email

**Step 2: Academic & Test Information (Combined)**
- School Name, School Type, Grade, Year Completed
- Test Type selection: AQL, MAT, or AQL+MAT
- Preferred Test Date and Venue
- Special Accommodation requests (checkbox + details)
- Remote writer registration (if applicable)

**Step 3: Survey Questions**
- Pre-test background questionnaire
- Demographic data for research and equity reporting
- Optional questions with skip logic
- Multi-choice and free-text responses

**Wizard Features:**
- Progress indicator (Step 1 of 3, 2 of 3, 3 of 3)
- Auto-save on each field blur
- Resume capability: return to last incomplete step
- Validation: inline errors + summary panel
- Previous/Next navigation with state preservation
- Cancel with confirmation dialog
- NBT number generation on final submission
- Automatic redirect to login page after completion

#### 2.1.2 Validation Rules
- **SA ID**: 13 digits, Luhn validation, DOB extraction, gender digit check
- **Foreign ID**: 10-20 alphanumeric characters
- **Passport ID**: 6-15 alphanumeric characters, international format
- **Email**: RFC 5322 format, duplicate check
- **Password**: Minimum 8 characters, uppercase, lowercase, number, special char
- **Phone**: International format validation, SMS verification
- **Date of Birth**: Age 15-80 years, realistic range
- **Grade**: Grades 10-12 or Completed (Grade 13+)

#### 2.1.3 NBT Number Generation
- Triggered on final wizard submission
- Algorithm: Luhn modulus-10 checksum
- Format: 14 digits (YYYYXXXXXXX + check digit)
- Uniqueness: Database constraint
- Display: Confirmation dialog with copy-to-clipboard
- Storage: Immutable, linked to Student entity
- Notification: Email/SMS with NBT number

### 2.2 Authentication & Account Management

#### 2.2.1 Login Flow
- Email/Password authentication
- OTP verification (first login or suspicious activity)
- JWT token issuance (access + refresh tokens)
- Role-based redirection:
  - Student → Student Dashboard
  - Staff → Staff Dashboard
  - Admin → Admin Dashboard
  - SuperUser → SuperUser Dashboard

#### 2.2.2 Password Management
- Password strength indicator during registration
- Forgot password: Email reset link (15-minute expiry)
- Change password: Require current password
- Password history: Prevent reuse of last 5 passwords

#### 2.2.3 Profile Management
- Edit personal details (audit logged)
- Upload supporting documents (ID copy, disability certificate)
- View booking history
- View payment history
- View test results (if fully paid)
- Update contact information

### 2.3 Test Booking Module

#### 2.3.1 Booking Workflow
1. **Prerequisites Check**
   - Student must be registered with NBT number
   - No active booking (only 1 booking at a time)
   - Previous booking closing date must have passed
   - Maximum 2 tests per year not exceeded

2. **Test Selection**
   - Test Type: AQL, MAT, or AQL+MAT
   - Display cost based on current intake year
   - Show test validity period (3 years from booking)

3. **Venue & Date Selection**
   - Display available venues by type:
     - National venues (physical locations)
     - Online test venues (remote with video/audio)
     - Special session venues (by request)
   - Filter by date availability
   - Show venue capacity and remaining slots
   - Highlight Sunday tests (different color)
   - Highlight Online tests (different color)
   - Display closing date for each test date

4. **Special Accommodations**
   - Checkbox: "I require special accommodations"
   - Conditional fields: Nature of accommodation, supporting documents
   - Automatic routing to NBT remote administration team

5. **Remote Writer Registration**
   - Checkbox: "I require a remote writer"
   - Conditional fields: Invigilator details, off-site venue address
   - Contact information for coordination

6. **Booking Confirmation**
   - Summary panel: Test type, venue, date, cost
   - Terms and conditions acceptance
   - Submit button: Create booking + generate payment reference
   - Confirmation email with booking details

#### 2.3.2 Booking Modification
- Allowed until closing date
- Change venue or date (subject to availability)
- Cancel booking: Refund policy applies
- Audit log all changes with timestamps

#### 2.3.3 Booking Business Rules
- TestSession is linked to TestVenue (not individual Room)
- Room allocation happens during check-in (not at booking)
- Capacity tracking at venue level
- Overbooking prevention with real-time validation
- Waitlist functionality (future phase)

### 2.4 Payment Module

#### 2.4.1 Payment Integration (EasyPay)
- Generate unique payment reference on booking creation
- Payment reference format: NBT{NBTNumber}{BookingID}
- Redirect to EasyPay payment gateway
- Support payment methods:
  - Credit/Debit card
  - EFT (Electronic Funds Transfer)
  - Bank deposits (manual upload)

#### 2.4.2 Installment Payments
- Allow partial payments until full cost is reached
- Track payment history with transaction IDs
- Calculate outstanding balance per booking
- Payment order: Chronological by booking date
- Cost variation by intake year:
  - 2025 Intake: AQL = R150, MAT = R150, AQL+MAT = R250
  - 2026 Intake: Prices may change (configurable)

#### 2.4.3 Payment Status Tracking
- Status: Pending, Partial, Completed, Failed, Refunded
- Automatic update on payment confirmation from EasyPay
- Webhook endpoint: `/api/payments/easypay-callback`
- Manual verification: Staff override for bank deposits
- Reconciliation report: Daily payment summary

#### 2.4.4 Bank Payment Upload
- File format: CSV or Excel
- Columns: Reference Number, Amount, Date, Bank, Transaction ID
- Batch processing with validation
- Match to existing bookings by reference
- Error reporting for unmatched payments
- Manual review queue for Staff

#### 2.4.5 Payment Access Rules
- **Students**: View/download ONLY fully paid test results
- **Staff/Admin**: View all results regardless of payment status
- Payment confirmation email on completion
- Receipt generation (PDF) with transaction details

### 2.5 Test Results Module

#### 2.5.1 Result Structure
**AQL Test Results:**
- Academic Literacy (AL) score
- Quantitative Literacy (QL) score
- Performance level for each domain:
  - Basic Lower (0-29%)
  - Basic Upper (30-49%)
  - Intermediate Lower (50-59%)
  - Intermediate Upper (60-69%)
  - Proficient Lower (70-84%)
  - Proficient Upper (85-100%)

**MAT Test Results:**
- Academic Literacy (AL) score
- Quantitative Literacy (QL) score
- Mathematics (MAT) score
- Performance level for each domain (same scale)

**Common Fields:**
- Test Barcode (unique per answer sheet)
- Test Date Written
- Test Venue
- NBT Number
- Student Name
- Test Type
- Result Release Date
- Validity Period (3 years from test date)

#### 2.5.2 Barcode System
- Unique barcode printed on physical answer sheet
- Scanned barcode linked to digital result record
- Multiple tests distinguished by barcode
- Barcode format: NBT{Year}{Sequence}{CheckDigit}
- Prevents result duplication
- Enables result verification

#### 2.5.3 Result Import
- CSV/Excel file upload by Staff/Admin
- Batch import with validation:
  - Barcode uniqueness check
  - NBT number match to existing student
  - Score range validation (0-100)
  - Performance level calculation
- Error reporting with line numbers
- Confirmation summary before commit
- Automatic email notification to students on result release

#### 2.5.4 Result Access
- Student login → Dashboard → "My Results" section
- List view: Test type, date written, status (Paid/Unpaid)
- Click to view detailed result (if fully paid)
- Download PDF certificate (if fully paid)
- Staff/Admin: View all results with payment status indicator

#### 2.5.5 PDF Certificate Generation
- Official NBT result certificate template
- Includes:
  - NBT logo and branding
  - Student details (Name, NBT Number, ID Number)
  - Test details (Type, Date, Venue, Barcode)
  - Scores and performance levels
  - Validity statement
  - Digital signature (cryptographic hash)
- Watermark: "Official NBT Result"
- Download filename: NBT_Result_{NBTNumber}_{Barcode}.pdf

### 2.6 Venue Management Module

#### 2.6.1 Venue Types
1. **National Venues**: Physical test centers across South Africa
2. **Special Session Venues**: One-off or exception-based locations
3. **Research Venues**: NBT research projects or trials
4. **Online Venues**: Virtual test environment (remote with proctoring)
5. **Other**: Configurable for future needs

#### 2.6.2 Venue Attributes
- Venue Name
- Venue Type (enum)
- Physical Address (if applicable)
- Province/Region
- Total Capacity (across all rooms)
- Active Status (enabled/disabled)
- Venue Coordinator (contact person)
- Special Instructions (e.g., parking, accessibility)

#### 2.6.3 Room Management (within Venue)
- Room Number/Name
- Seating Capacity
- Equipment (projector, computers, etc.)
- Accessibility features (wheelchair access, etc.)
- Room Status (available, maintenance, closed)
- Linked to Venue (parent-child relationship)

#### 2.6.4 Venue Availability
- Test Date Calendar: Define available dates per venue
- Closing Date: Booking cutoff date (e.g., 7 days before test)
- Unavailable Dates: Block out maintenance, holidays
- Online Venue: Specific dates configured for remote testing
- Sunday Tests: Flagged with visual indicator
- Color coding:
  - Green: Available with slots
  - Yellow: Limited slots remaining
  - Red: Fully booked
  - Blue: Sunday test
  - Purple: Online test

#### 2.6.5 TestSession Management
- TestSession linked to TestVenue (not individual Room)
- Session Date and Time
- Test Type (AQL, MAT, or AQL+MAT)
- Session Capacity (based on venue total capacity)
- Bookings count (real-time)
- Session Status: Open, Closed, In Progress, Completed
- Room allocation happens on test day (check-in process)

### 2.7 Staff & Admin Dashboards

#### 2.7.1 Dashboard Layout
- **Left-side navigation menu** (collapsible)
  - Dashboard Home
  - Students/Applicants
  - Bookings
  - Payments
  - Results
  - Venues
  - Reports
  - Settings (Admin only)
  - Audit Logs (Admin/SuperUser only)

- **Dashboard Home**
  - Summary cards:
    - Total Registrations (this month/year)
    - Active Bookings
    - Pending Payments
    - Results Released (this month)
  - Quick actions:
    - Import Results
    - Upload Bank Payments
    - Create Venue
    - Generate Report
  - Recent activity feed
  - Notifications panel

#### 2.7.2 Student Management (Staff/Admin)
- Search: By NBT number, name, ID number, email
- List view with pagination (50 per page)
- Filters: Registration date, status, test type
- Actions per student:
  - View full profile
  - Edit details (with audit log)
  - View booking history
  - View payment history
  - View results
  - Reset password
  - Deactivate account
- Bulk actions:
  - Export to Excel
  - Send notification email
  - Mark for review

#### 2.7.3 Booking Management (Staff/Admin)
- Search: By NBT number, booking ID, test date
- List view with filters:
  - Test type
  - Venue
  - Date range
  - Payment status
- Actions per booking:
  - View details
  - Modify booking (if before closing date)
  - Cancel booking
  - Override payment status (with justification)
  - Send reminder email
- Bulk actions:
  - Export attendance list
  - Generate venue summary
  - Mark as attended

#### 2.7.4 Payment Management (Staff/Admin)
- Search: By payment reference, NBT number, transaction ID
- List view with filters:
  - Payment status
  - Payment method
  - Date range
  - Amount range
- Actions per payment:
  - View transaction details
  - Verify payment (for bank uploads)
  - Issue refund
  - Mark as reconciled
  - Generate receipt
- Bulk actions:
  - Reconciliation report
  - Export to Excel
  - Generate financial summary

#### 2.7.5 Result Management (Staff/Admin)
- Import results: CSV/Excel upload
- Search: By NBT number, barcode, test date
- List view with filters:
  - Test type
  - Test date
  - Payment status
  - Release status
- Actions per result:
  - View detailed scores
  - Edit result (with justification + audit log)
  - Release result (make visible to student)
  - Withdraw result (e.g., irregularity)
  - Generate certificate
- Bulk actions:
  - Batch release
  - Export to Excel
  - Generate performance summary

#### 2.7.6 Venue Management (Staff/Admin)
- Create new venue (form with validation)
- List view with filters:
  - Venue type
  - Province
  - Status (active/inactive)
- Actions per venue:
  - View details
  - Edit venue
  - Manage rooms (add/edit/delete)
  - Set availability dates
  - View booking summary
  - Deactivate venue
- Room management:
  - Add room to venue
  - Set capacity
  - Mark for maintenance
  - Reallocate bookings (if room closed)

### 2.8 Reporting & Analytics Module

#### 2.8.1 Standard Reports
1. **Registration Report**
   - Total registrations by date range
   - Demographic breakdown (gender, ethnicity, province)
   - School type analysis
   - Export to Excel/PDF

2. **Booking Report**
   - Bookings by test type
   - Bookings by venue
   - Bookings by date range
   - Special accommodations summary
   - Export to Excel/PDF

3. **Payment Report**
   - Total revenue by period
   - Payment method breakdown
   - Outstanding payments
   - Refund summary
   - Reconciliation report
   - Export to Excel/PDF

4. **Result Report**
   - Results released by date
   - Performance level distribution
   - Test type analysis
   - Venue performance comparison
   - Export to Excel/PDF

5. **Venue Utilization Report**
   - Capacity vs. bookings
   - Most/least popular venues
   - Sunday test attendance
   - Online test participation
   - Export to Excel/PDF

6. **Equity & Research Report**
   - Demographic performance analysis
   - School type performance
   - Special accommodation analysis
   - Pre-test questionnaire summary
   - Export to Excel/PDF

#### 2.8.2 Chart Visualizations
- Bar charts: Registrations per month
- Pie charts: Test type distribution
- Line charts: Revenue trends
- Heatmap: Venue availability
- Gauge charts: Capacity utilization

#### 2.8.3 Custom Report Builder (Future Phase)
- Drag-and-drop report designer
- Custom filters and grouping
- Scheduled report generation
- Email distribution list

### 2.9 Public Landing Page & Menus

#### 2.9.1 Landing Page Structure
**Header:**
- NBT Logo
- Navigation menu:
  - **Applicants** (dropdown)
    - Register for NBT
    - How to Book a Test
    - Test Types Explained (AQL, MAT)
    - Frequently Asked Questions
    - Special Accommodations
    - Results and Certificates
    - Video Tutorials
  - **Institutions** (dropdown)
    - Using NBT Scores
    - Bulk Result Access (future)
    - Research and Insights
    - Contact NBT Team
  - **Educators** (dropdown)
    - Preparing Students for NBT
    - Sample Questions
    - Performance Level Guide
    - School Partnerships
    - Resources and Videos
  - **Login** (button)

**Hero Section:**
- Welcome message
- Key statistics (tests written this year, institutions using NBT)
- Call-to-action buttons:
  - Register Now
  - Book a Test
  - Check Results

**Information Sections:**
- About NBT program
- Test types overview
- How it works (3-step visual)
- Testimonials
- Upcoming test dates (calendar widget)

**Footer:**
- Contact information
- Social media links
- Privacy policy
- Terms and conditions
- Accessibility statement

#### 2.9.2 Video Integration
- Embed videos on:
  - Registration help page
  - Booking process page
  - Test preparation page
  - Result interpretation page
- Video sources:
  - YouTube embeds (NBT official channel)
  - Direct video hosting (future)
- Video library page with categories

### 2.10 Notifications & Communication

#### 2.10.1 Automated Notifications
**Email Notifications:**
1. **Registration Confirmation**
   - Welcome message
   - NBT number
   - Next steps (book a test)
   - Login credentials reminder

2. **Booking Confirmation**
   - Test type, venue, date
   - Payment reference and amount due
   - Closing date reminder
   - What to bring on test day

3. **Payment Confirmation**
   - Transaction details
   - Receipt (PDF attachment)
   - Outstanding balance (if installment)
   - Thank you message

4. **Payment Reminder**
   - Outstanding balance
   - Closing date warning
   - Payment options
   - Contact information for queries

5. **Test Reminder**
   - 7 days before test
   - 1 day before test
   - Venue details and directions
   - What to bring
   - Arrive 30 minutes early

6. **Result Release Notification**
   - Result now available
   - Login instructions
   - Payment status (if unpaid)
   - Certificate download link (if paid)

7. **Password Reset**
   - Reset link (15-minute expiry)
   - Security tips
   - Contact support if not requested

**SMS Notifications:**
- OTP verification codes
- Booking confirmation (short summary)
- Payment confirmation (amount received)
- Test reminder (1 day before)
- Result release alert

#### 2.10.2 Notification Preferences
- Student profile settings:
  - Email notifications (on/off per type)
  - SMS notifications (on/off per type)
  - Language preference (English, Afrikaans, Zulu, etc.)

### 2.11 Audit Logging & Security

#### 2.11.1 Audit Log Requirements
**Log Every Action:**
- User login/logout
- Registration creation/edit
- Booking creation/modification/cancellation
- Payment creation/verification
- Result import/edit/release
- Venue creation/modification
- Staff user creation/deactivation
- Settings changes

**Audit Log Fields:**
- Timestamp (UTC)
- User ID and Role
- Action Type (Create, Read, Update, Delete)
- Entity Type (Student, Booking, Payment, etc.)
- Entity ID
- Old Value (JSON)
- New Value (JSON)
- IP Address
- User Agent
- Result (Success, Failure, Error)

**Audit Log Access:**
- Read-only for all users except SuperUser
- SuperUser can export audit logs
- Retention period: 7 years (compliance)
- Encrypted storage

#### 2.11.2 Security Measures
- **Authentication**:
  - JWT access token (15-minute expiry)
  - JWT refresh token (7-day expiry)
  - OTP for sensitive actions

- **Authorization**:
  - Role-based access control (RBAC)
  - Policy-based authorization for fine-grained control

- **Data Protection**:
  - Password hashing (bcrypt with salt)
  - Sensitive data encryption at rest
  - HTTPS-only (TLS 1.2+)
  - CORS restricted to production domains

- **Input Validation**:
  - Client-side (Fluent UI validation)
  - Server-side (FluentValidation library)
  - SQL injection prevention (parameterized queries)
  - XSS prevention (output encoding)

- **Rate Limiting**:
  - Login attempts: 5 per 15 minutes
  - API requests: 100 per minute per user
  - Payment webhook: 10 per minute (IP-based)

---

## 3. NON-FUNCTIONAL REQUIREMENTS

### 3.1 Performance
- **Page Load**: < 3 seconds (95th percentile)
- **API Response**: < 500ms (95th percentile)
- **Database Query**: < 100ms (simple queries)
- **Concurrent Users**: Support 1,000 simultaneous users
- **File Upload**: Max 10MB, < 10 seconds processing

### 3.2 Scalability
- Horizontal scaling for Web API (load balancer)
- Database connection pooling
- Caching strategy (Redis for session state)
- CDN for static assets

### 3.3 Availability
- **Uptime**: 99.9% (excluding planned maintenance)
- **Maintenance Window**: Sundays 2:00 AM - 4:00 AM SAST
- **Backup**: Daily database backups, 30-day retention
- **Disaster Recovery**: RTO 4 hours, RPO 1 hour

### 3.4 Accessibility
- **WCAG 2.1 AA Compliance**:
  - Keyboard navigation
  - Screen reader support
  - Color contrast ratios
  - Alt text for images
  - ARIA labels for interactive elements
  - Focus indicators

### 3.5 Browser Support
- Chrome 100+
- Edge 100+
- Firefox 100+
- Safari 15+
- Mobile browsers (Chrome, Safari on iOS/Android)

### 3.6 Localization
- Primary language: English
- Future support: Afrikaans, Zulu, Xhosa, Sotho
- Date format: YYYY-MM-DD (ISO 8601)
- Currency: South African Rand (ZAR)
- Timezone: SAST (UTC+2)

---

## 4. DATA MIGRATION & INTEGRATION

### 4.1 Legacy Data Import
- Import existing student records from legacy system
- Map old NBT numbers to new format
- Preserve historical test results
- Migration validation reports

### 4.2 External Integrations
- **EasyPay Payment Gateway**: REST API
- **Email Service**: SendGrid or SMTP
- **SMS Service**: Twilio or BulkSMS
- **File Storage**: Azure Blob Storage or local filesystem
- **Monitoring**: Application Insights

---

## 5. TESTING STRATEGY

### 5.1 Unit Testing
- Business logic in Application layer
- Validators (FluentValidation)
- Utility functions (Luhn algorithm, etc.)
- Target coverage: 80%

### 5.2 Integration Testing
- API endpoints with in-memory database
- EF Core repository tests
- Authentication/authorization flows
- Payment webhook processing

### 5.3 End-to-End Testing
- Registration wizard (full flow)
- Booking and payment (full flow)
- Result access (full flow)
- Staff dashboard operations

### 5.4 Manual Testing
- UAT by stakeholders
- Accessibility testing with screen readers
- Cross-browser testing
- Mobile responsiveness testing

---

## 6. DEPLOYMENT & DEVOPS

### 6.1 CI/CD Pipeline
- **Source Control**: GitHub
- **CI Tool**: GitHub Actions
- **Build**: Automated on push to dev branch
- **Test**: Run unit + integration tests
- **Deploy**: Automatic to staging, manual to production
- **Rollback**: One-click rollback to previous version

### 6.2 Environment Configuration
- **Development**: Local developer machines
- **Staging**: Azure-hosted mirror of production
- **Production**: Azure App Service + Azure SQL Database

### 6.3 Monitoring & Logging
- **Application Logs**: Serilog to Application Insights
- **Performance Monitoring**: Application Insights
- **Error Tracking**: Application Insights with alerts
- **Health Checks**: /health endpoint (heartbeat)

---

## 7. ACCEPTANCE CRITERIA

### 7.1 Phase 1: Registration & Authentication
- [ ] Student can register with SA ID, Foreign ID, or Passport ID
- [ ] Wizard saves progress and allows resume
- [ ] NBT number generated on completion
- [ ] Login redirects to appropriate dashboard
- [ ] Password reset functional

### 7.2 Phase 2: Booking & Payment
- [ ] Student can book test with venue and date selection
- [ ] Booking rules enforced (1 at a time, 2 per year, etc.)
- [ ] EasyPay payment reference generated
- [ ] Installment payments tracked
- [ ] Bank payment upload functional

### 7.3 Phase 3: Staff Dashboards
- [ ] Staff can view all students, bookings, payments
- [ ] CRUD operations functional with audit logging
- [ ] Search and filter working
- [ ] Bulk actions operational

### 7.4 Phase 4: Results & Reporting
- [ ] Result import from CSV/Excel
- [ ] Students see results only if fully paid
- [ ] PDF certificate generation
- [ ] Barcode system operational
- [ ] Excel/PDF report exports working

### 7.5 Phase 5: Venue Management
- [ ] Venue CRUD operations
- [ ] Room management within venues
- [ ] Test date calendar with availability
- [ ] Online and Sunday test highlighting
- [ ] Capacity tracking and overbooking prevention

### 7.6 Phase 6: Production Readiness
- [ ] All unit tests passing (80%+ coverage)
- [ ] Integration tests passing
- [ ] End-to-end tests passing
- [ ] UAT sign-off
- [ ] Performance requirements met
- [ ] Accessibility compliance verified
- [ ] Security audit completed
- [ ] CI/CD pipeline operational
- [ ] Production deployment successful

---

## 8. CHANGE LOG

| Version | Date       | Author | Changes                              |
|---------|------------|--------|--------------------------------------|
| 1.0     | 2025-11-09 | System | Initial specification created        |

---

## 9. APPROVALS

**Technical Lead**: _____________________  
**Product Owner**: _____________________  
**NBT Director**: _____________________  

**Date**: 2025-11-09

---

*This specification document is the authoritative source for all functional and technical requirements of the NBT Integrated Web Application. All development work must align with this specification and the Constitution document.*
