# NBT Web Application - Complete Specification

## Comprehensive System Specification

**Version:** 2.0  
**Date:** 2025-11-09  
**Status:** Active

---

## 1. System Overview

The National Benchmark Tests (NBT) Integrated Web Application is a comprehensive platform for managing student registrations, test bookings, payments, venue management, and results distribution for the NBT assessment program.

### 1.1 System Architecture
```
┌─────────────────────────────────────────────────────────────┐
│                    Blazor WebAssembly Client                │
│                    (Fluent UI Components)                    │
└────────────────────┬────────────────────────────────────────┘
                     │ HTTPS/JSON
┌────────────────────▼────────────────────────────────────────┐
│                  ASP.NET Core Web API                        │
│                  (JWT Authentication)                        │
└────────────────────┬────────────────────────────────────────┘
                     │ Entity Framework Core
┌────────────────────▼────────────────────────────────────────┐
│                    MS SQL Server Database                    │
│              (Entities, Relationships, Indexes)              │
└──────────────────────────────────────────────────────────────┘

External Integrations:
├── EasyPay Payment Gateway
├── Email Service (SMTP/SendGrid)
└── SMS Service (Clickatell/Twilio)
```

---

## 2. User Roles and Permissions

### 2.1 Student Role
**Capabilities:**
- Register new account (SA ID, Foreign ID, or Passport)
- Log in and manage profile
- Complete registration wizard (multi-step)
- Generate NBT number automatically
- Book tests (AQL or AQL+MAT)
- Make payments (full or installment)
- View/download paid test results
- Access test certificates (PDF with barcode)
- Update personal information
- Request special accommodations
- View test calendar and availability

**Restrictions:**
- Cannot view unpaid test results
- Cannot book more than one test at a time
- Cannot exceed 2 tests per year
- Cannot modify booking after closing date

### 2.2 Staff Role
**Capabilities:**
- View all students and registrations
- View all test results (paid and unpaid)
- Process manual payments
- Generate reports
- Manage venues and rooms
- Schedule test sessions
- View audit logs
- Assist students with booking issues

**Restrictions:**
- Cannot delete students
- Cannot modify test results
- Limited admin configuration access

### 2.3 Admin Role
**Capabilities:**
- All Staff capabilities
- Create/edit/delete venues
- Create/edit/delete test sessions
- Manage test dates and pricing
- Import test results
- Export comprehensive reports
- Configure system settings
- Manage user accounts
- Full audit log access

**Restrictions:**
- Cannot modify historical audit logs
- Cannot delete paid transactions

### 2.4 SuperUser Role
**Capabilities:**
- All Admin capabilities
- System configuration
- Database migrations
- Security settings
- Role assignment
- Critical data operations

---

## 3. Core Functional Areas

### 3.1 Student Registration Wizard

**Workflow Steps:**

**Step 1: Personal & ID Information**
- ID Type selection (SA ID, Foreign ID, Passport)
- ID Number input with validation
- For SA ID: Auto-extract DOB and Gender
- For Foreign ID/Passport: Manual DOB, Gender, Nationality, Country
- First Name, Last Name
- Email and Phone
- Duplicate prevention check

**Step 2: Contact & Academic Information**
- Address, City, Province, Postal Code
- School Name, Grade, Home Language
- Ethnicity (optional)
- Special Accommodation requests
- NBT number generation upon completion

**Step 3: Survey Questionnaire**
- Motivation for testing
- Career interests
- Preferred study field
- Access to computer
- Internet access
- Additional comments

**Business Rules:**
- All steps must complete successfully
- Progress saved automatically
- Validation on each step before proceeding
- NBT number generated after all steps complete
- Email verification (OTP) before account activation

### 3.2 NBT Number Generation

**Algorithm: Luhn Modulus-10**

```
Format: YYYY + Sequential (9 digits) + Check Digit = 14 digits
Example: 20240000000123 (where 3 is the check digit)

Steps:
1. Start with year: 2024
2. Add sequential number: 0000000012
3. Calculate Luhn check digit: 3
4. Result: 20240000000123
```

**Validation:**
- Must be unique across system
- Must pass Luhn validation
- Cannot be modified after generation
- Automatically generated on registration completion

### 3.3 Test Booking Module

**Booking Process:**

1. **Test Selection**
   - Choose test type: AQL or AQL+MAT
   - View test descriptions and prices
   - Price varies by Intake Year

2. **Venue Selection**
   - View available venues by type:
     - National (regular centers)
     - Special Session (accommodations)
     - Research
     - Online (remote with video/audio)
   - Filter by province/city
   - View venue capacity and availability

3. **Date Selection**
   - View test calendar with available dates
   - Sunday tests highlighted
   - Online test dates marked
   - Closing dates displayed
   - Check seat availability

4. **Booking Confirmation**
   - Review booking details
   - Generate booking reference
   - Create payment invoice
   - Send confirmation email

**Business Rules:**
- Student can only have one active booking
- Cannot book if previous test closing date not passed
- Maximum 2 tests per year
- Can modify booking before closing date
- Tests valid for 3 years from booking date

### 3.4 Payment Integration

**Payment Methods:**
- EasyPay (online)
- EFT (bank transfer)
- Cash (at venue)
- Card (manual processing)

**Payment Process:**

1. **Generate Invoice**
   - Format: INV-YYYY-NNNNNN
   - Calculate total based on test type and year
   - Create EasyPay reference: EP-{RegistrationId}-{Timestamp}

2. **Payment Options**
   - Full payment
   - Installment payments
   - Multiple payment methods

3. **Payment Tracking**
   - Record each transaction
   - Update payment status
   - Track balance remaining
   - Apply payments in order of tests

4. **Payment Confirmation**
   - EasyPay callback processing
   - Email confirmation
   - Update registration status
   - Unlock result access when fully paid

**Business Rules:**
- Installment payments allowed
- Payments applied in test booking order
- Only fully paid tests visible to students
- Staff/Admin can view all regardless of payment
- Complete audit trail required
- Test costs vary by Intake Year

### 3.5 Test Results Management

**Result Components:**

**AQL Test Results:**
- Academic Literacy (AL) Score
- Quantitative Literacy (QL) Score
- Performance Levels for each domain
- Overall performance band

**AQL+MAT Test Results:**
- Academic Literacy (AL) Score
- Quantitative Literacy (QL) Score
- Mathematics (MAT) Score
- Performance Levels for each domain
- Overall performance band

**Performance Levels:**
- Basic Lower
- Basic Upper
- Intermediate Lower
- Intermediate Upper
- Proficient Lower
- Proficient Upper
- Outstanding

**Barcode System:**
- Unique barcode per answer sheet
- Format: BC-{NBTNumber}-{TestDate}-{Sequence}
- Links to specific test attempt
- Distinguishes multiple tests by same student
- Printed on certificate

**Result Access:**
- Students: Only fully paid tests
- Staff/Admin: All tests regardless of payment
- PDF certificate download
- Include barcode on certificate
- Display performance levels and scores

**Result Import:**
- Bulk import from Excel/CSV
- Validation before import
- Match to student by NBT number and barcode
- Automatic email notification on result release

### 3.6 Venue Management

**Venue Types:**
1. **National** - Regular test centers across South Africa
2. **Special Session** - For special accommodations
3. **Research** - For research projects and pilots
4. **Online** - Remote testing with monitoring

**Venue Information:**
- Venue Code (unique identifier)
- Venue Name
- Physical Address
- Contact Person, Email, Phone
- Total Capacity
- Accessibility features
- Status (Active, Inactive, Under Maintenance)

**Room Management:**
- Room number/name
- Capacity
- Accessibility
- Equipment available
- Status

**Venue Availability:**
- Configure available test dates per venue
- Block unavailable dates
- Set capacity limits per session
- Track registrations vs. capacity

### 3.7 Test Date Calendar

**Calendar Configuration:**
- Test Date
- Closing Booking Date
- Venue Availability
- Online/Physical indicator
- Sunday indicator (highlighted)
- Capacity status

**Rules:**
- Bookings close on specified closing date
- Sunday tests visually distinguished
- Online tests available worldwide
- Venue-specific date configurations

### 3.8 Special Sessions & Remote Writers

**Special Session Request:**
- Student indicates special need
- Provides custom venue details
- Specifies invigilator information
- Describes accommodation requirements

**Processing:**
- Automatic routing to NBT remote admin team
- Approval workflow
- Custom venue setup
- Invigilator coordination
- Equipment verification

**Remote Writer Setup:**
- Video/audio requirement verification
- Internet connection testing
- Pre-test technical check
- Real-time monitoring during test

### 3.9 Staff Dashboard

**Student Management:**
- Search students (by name, NBT number, ID number)
- View student profiles
- Edit student information
- View registration history
- View payment history
- View test results

**Registration Management:**
- View all registrations
- Filter by status, date, venue
- Process manual registrations
- Cancel/modify registrations
- Generate registration reports

**Payment Management:**
- View all payments
- Process manual payments
- Record cash/EFT payments
- Generate payment receipts
- View payment analytics

**Result Management:**
- Import test results
- View all results
- Generate result reports
- Release results to students
- Download certificates

### 3.10 Admin Dashboard

**All Staff capabilities plus:**

**Venue Management:**
- Create/edit/delete venues
- Manage rooms
- Configure venue availability
- Set capacity limits

**Test Session Management:**
- Create test sessions
- Schedule dates and times
- Assign venues
- Set capacity
- Manage session status

**System Configuration:**
- Test pricing by year
- Email templates
- System settings
- User role management
- Audit log viewing

**Reporting:**
- Registration statistics
- Payment analytics
- Result summaries
- Venue utilization
- Financial reports
- Export to Excel/PDF

### 3.11 Reporting & Analytics

**Student Reports:**
- Registration summary
- Demographics analysis
- Test type distribution
- Geographic distribution

**Financial Reports:**
- Revenue by test type
- Revenue by period
- Payment method breakdown
- Outstanding payments
- Installment tracking

**Operational Reports:**
- Venue utilization
- Session capacity analysis
- Result release timeline
- Special accommodation requests

**Export Formats:**
- Excel (XLSX)
- PDF
- CSV

---

## 4. Data Models

### 4.1 Core Entities

**Student**
```
- Id (Guid)
- NBTNumber (string, 14 digits, unique)
- IDType (enum: SA_ID, FOREIGN_ID, PASSPORT)
- IDNumber (string, unique)
- Nationality (string, nullable)
- CountryOfOrigin (string, nullable)
- FirstName (string)
- LastName (string)
- Email (string, unique)
- Phone (string)
- DateOfBirth (DateTime)
- Gender (string)
- Age (int, calculated)
- Ethnicity (string, nullable)
- Address, City, Province, PostalCode
- SchoolName, Grade, HomeLanguage
- SpecialAccommodation (string, nullable)
- Survey fields (motivation, career, etc.)
- IsActive (bool)
- CreatedAt, UpdatedAt, CreatedBy, UpdatedBy
- Relationships: Registrations, TestResults, RoomAllocations
```

**Registration**
```
- Id (Guid)
- RegistrationNumber (string, REG-YYYY-NNNNNN)
- StudentId (Guid, FK)
- TestSessionId (Guid, FK)
- Status (enum: Pending, Confirmed, Cancelled, Completed)
- TestTypesSelected (string, JSON array)
- IsRemoteWriter (bool)
- RemoteLocation (string, nullable)
- SpecialSessionType (string, nullable)
- RegistrationDate, ConfirmationDate, CancellationDate
- CancellationReason (string, nullable)
- CreatedAt, UpdatedAt
- Relationships: Student, TestSession, Payment
```

**Payment**
```
- Id (Guid)
- RegistrationId (Guid, FK)
- InvoiceNumber (string, INV-YYYY-NNNNNN)
- Amount (decimal)
- TotalAmount (decimal)
- AmountPaid (decimal)
- Balance (decimal, calculated)
- PaymentMethod (string)
- Status (enum: Pending, Partial, Completed, Refunded, Failed)
- EasyPayReference (string, nullable)
- EasyPayTransactionId (string, nullable)
- PaidDate, RefundedDate
- RefundReason (string, nullable)
- Notes (string, nullable)
- IntakeYear (int)
- CreatedAt, UpdatedAt
- Relationships: Registration, PaymentTransactions
```

**PaymentTransaction** (new)
```
- Id (Guid)
- PaymentId (Guid, FK)
- TransactionDate (DateTime)
- Amount (decimal)
- PaymentMethod (string)
- TransactionReference (string)
- Status (enum: Success, Failed, Pending)
- Notes (string, nullable)
- CreatedBy (string)
- CreatedAt
```

**TestResult**
```
- Id (Guid)
- StudentId (Guid, FK)
- TestSessionId (Guid, FK)
- RegistrationId (Guid, FK)
- Barcode (string, unique)
- TestType (string: AQL, AQL_MAT)
- ALScore (decimal, nullable)
- ALPerformanceLevel (string, nullable)
- QLScore (decimal, nullable)
- QLPerformanceLevel (string, nullable)
- MATScore (decimal, nullable)
- MATPerformanceLevel (string, nullable)
- OverallPerformanceBand (string)
- Percentile (int)
- IsReleased (bool)
- TestDate, ResultDate, ReleasedDate
- CreatedAt, UpdatedAt
- Relationships: Student, TestSession, Registration
```

**TestSession**
```
- Id (Guid)
- SessionCode (string, CITY-YYYY-MM-DD-PERIOD)
- SessionName (string)
- SessionDate (DateTime)
- StartTime (TimeSpan)
- EndTime (TimeSpan)
- VenueId (Guid, FK)
- Capacity (int)
- CurrentRegistrations (int)
- AvailableSeats (int, calculated)
- Status (enum: Open, Closed, Completed, Cancelled)
- IsSpecialSession (bool)
- IsOnline (bool)
- IsSunday (bool)
- SpecialSessionNotes (string, nullable)
- Notes (string, nullable)
- CreatedAt, UpdatedAt
- Relationships: Venue, Registrations, RoomAllocations
```

**Venue**
```
- Id (Guid)
- VenueName (string)
- VenueCode (string, unique)
- VenueType (enum: National, SpecialSession, Research, Online, Other)
- Address, City, Province, PostalCode
- ContactPerson, ContactEmail, ContactPhone
- TotalCapacity (int)
- IsAccessible (bool)
- Status (string: Active, Inactive, UnderMaintenance)
- Notes (string, nullable)
- CreatedAt, UpdatedAt
- Relationships: Rooms, TestSessions, VenueAvailability
```

**VenueAvailability** (new)
```
- Id (Guid)
- VenueId (Guid, FK)
- TestDate (DateTime)
- IsAvailable (bool)
- Reason (string, nullable)
- CreatedAt, UpdatedAt
```

**TestDateCalendar** (new)
```
- Id (Guid)
- TestDate (DateTime)
- ClosingBookingDate (DateTime)
- IsSunday (bool)
- IsOnline (bool)
- IsActive (bool)
- IntakeYear (int)
- Notes (string, nullable)
- CreatedAt, UpdatedAt
```

**TestPricing** (new)
```
- Id (Guid)
- IntakeYear (int)
- TestType (string: AQL, AQL_MAT)
- Price (decimal)
- EffectiveFrom (DateTime)
- EffectiveTo (DateTime, nullable)
- IsActive (bool)
- CreatedAt, UpdatedAt
```

**Room**
```
- Id (Guid)
- VenueId (Guid, FK)
- RoomNumber (string)
- RoomName (string)
- Capacity (int)
- IsAccessible (bool)
- Status (string)
- CreatedAt, UpdatedAt
- Relationships: Venue, RoomAllocations
```

**RoomAllocation**
```
- Id (Guid)
- StudentId (Guid, FK)
- TestSessionId (Guid, FK)
- RoomId (Guid, FK)
- SeatNumber (string, nullable)
- CreatedAt, UpdatedAt
- Relationships: Student, TestSession, Room
```

**User**
```
- Id (Guid)
- Username (string, unique)
- Email (string, unique)
- PasswordHash (string)
- FirstName, LastName
- Role (enum: Admin, Staff, SuperUser, Student)
- IsActive (bool)
- LastLoginDate (DateTime, nullable)
- CreatedAt, UpdatedAt
```

**AuditLog**
```
- Id (Guid)
- UserId (Guid, nullable)
- UserName (string)
- Action (string)
- EntityType (string)
- EntityId (Guid, nullable)
- OldValues (string, JSON, nullable)
- NewValues (string, JSON, nullable)
- IpAddress (string)
- Timestamp (DateTime)
```

---

## 5. API Endpoints

### 5.1 Student Endpoints
```
POST   /api/students/register          - Register new student
POST   /api/students/login             - Student login
GET    /api/students/{id}              - Get student profile
PUT    /api/students/{id}              - Update student profile
GET    /api/students/{id}/registrations - Get student registrations
GET    /api/students/{id}/results      - Get student results
GET    /api/students/check-duplicate   - Check duplicate ID
POST   /api/students/verify-otp        - Verify OTP
```

### 5.2 Registration Endpoints
```
POST   /api/registrations              - Create registration
GET    /api/registrations/{id}         - Get registration
PUT    /api/registrations/{id}         - Update registration
DELETE /api/registrations/{id}         - Cancel registration
GET    /api/registrations              - List registrations (staff/admin)
GET    /api/registrations/student/{studentId} - Get student's registrations
```

### 5.3 Payment Endpoints
```
POST   /api/payments                   - Create payment
GET    /api/payments/{id}              - Get payment
PUT    /api/payments/{id}/status       - Update payment status
POST   /api/payments/easypay-callback  - EasyPay callback
POST   /api/payments/record-transaction - Record payment transaction
GET    /api/payments/registration/{regId} - Get registration payments
GET    /api/payments/invoice/{invoiceNumber} - Get payment by invoice
```

### 5.4 Test Result Endpoints
```
POST   /api/results/import             - Import results (admin)
GET    /api/results/student/{studentId} - Get student results
GET    /api/results/{id}/certificate   - Download certificate PDF
PUT    /api/results/{id}/release       - Release result to student
GET    /api/results                    - List all results (staff/admin)
GET    /api/results/barcode/{barcode}  - Get result by barcode
```

### 5.5 Venue Endpoints
```
POST   /api/venues                     - Create venue
GET    /api/venues/{id}                - Get venue
PUT    /api/venues/{id}                - Update venue
DELETE /api/venues/{id}                - Delete venue
GET    /api/venues                     - List venues
GET    /api/venues/available           - Get available venues
POST   /api/venues/{id}/availability   - Set venue availability
```

### 5.6 Test Session Endpoints
```
POST   /api/test-sessions              - Create session
GET    /api/test-sessions/{id}         - Get session
PUT    /api/test-sessions/{id}         - Update session
DELETE /api/test-sessions/{id}         - Delete session
GET    /api/test-sessions              - List sessions
GET    /api/test-sessions/available    - Get available sessions
```

### 5.7 Calendar Endpoints
```
GET    /api/calendar/test-dates        - Get test dates
POST   /api/calendar/test-dates        - Create test date
PUT    /api/calendar/test-dates/{id}   - Update test date
GET    /api/calendar/test-dates/{date} - Get test date details
```

### 5.8 Report Endpoints
```
GET    /api/reports/registrations      - Registration report
GET    /api/reports/payments           - Payment report
GET    /api/reports/results            - Results report
GET    /api/reports/venues             - Venue utilization report
GET    /api/reports/export/{type}      - Export report (Excel/PDF)
```

---

## 6. User Interface Specification

### 6.1 Student Portal

**Landing Page**
- Hero section with NBT overview
- Quick links: Register, Login, Test Information
- Test calendar preview
- News and announcements

**Registration Wizard**
- Multi-step form with progress indicator
- Step 1: Personal & ID Information
- Step 2: Contact & Academic Information
- Step 3: Survey Questionnaire
- Save progress functionality
- Validation on each step
- Success page with NBT number

**Dashboard**
- Welcome message with NBT number
- Quick actions: Book Test, View Results, Update Profile
- Current bookings with status
- Payment status summary
- Upcoming test dates

**Book Test Page**
- Test type selection cards
- Venue search and filter
- Date calendar with availability
- Booking summary and confirmation
- Payment options

**My Results Page**
- List of completed tests
- Filter by date, test type
- View detailed results (paid only)
- Download certificate button
- Performance level visualization

**Profile Page**
- View/edit personal information
- Change password
- Contact information
- Academic details
- Survey responses

### 6.2 Staff Portal

**Dashboard**
- Statistics cards (registrations, payments, results)
- Recent activity feed
- Quick actions
- Alerts and notifications

**Student Management**
- Search and filter students
- Student list with pagination
- View student details
- Edit student information
- Registration history
- Payment history

**Registration Management**
- Filter by status, date, venue
- Registration list
- View/edit registration
- Process manual registration
- Cancel registration

**Payment Management**
- Payment list
- Filter by status, method
- Record manual payment
- View payment details
- Generate receipt

**Result Management**
- Import results
- View all results
- Release results
- Generate reports

### 6.3 Admin Portal

**All Staff features plus:**

**Venue Management**
- Venue list
- Create/edit venue
- Manage rooms
- Set availability

**Test Session Management**
- Session calendar
- Create/edit session
- Manage capacity
- Session reports

**System Configuration**
- Test pricing
- Email templates
- User management
- System settings

**Reports & Analytics**
- Comprehensive reports
- Export functionality
- Data visualization
- Audit logs

---

## 7. Workflows

### 7.1 Student Registration Workflow
```
1. Student navigates to registration page
2. Completes Step 1 (Personal & ID Info)
   - Validates ID number
   - Checks for duplicates
   - Auto-fills DOB/Gender for SA ID
3. Completes Step 2 (Contact & Academic)
   - Validates all fields
   - Saves progress
4. Completes Step 3 (Survey)
   - Optional fields
5. System generates NBT number
6. Sends verification email with OTP
7. Student verifies email
8. Account activated
9. Redirects to dashboard
```

### 7.2 Test Booking Workflow
```
1. Student logs in
2. Navigates to "Book Test"
3. Selects test type (AQL or AQL+MAT)
4. Views price for current intake year
5. Filters venues by location
6. Selects venue
7. Views available dates
8. Selects test date
9. Reviews booking summary
10. Confirms booking
11. System creates registration record
12. Generates invoice
13. Creates EasyPay reference
14. Sends confirmation email
15. Redirects to payment page
```

### 7.3 Payment Workflow
```
1. Student selects payment method
2. If EasyPay:
   a. Redirects to EasyPay gateway
   b. Student completes payment
   c. EasyPay sends callback
   d. System updates payment status
   e. Sends confirmation email
3. If Manual (EFT/Cash):
   a. Displays banking details
   b. Student makes payment
   c. Staff records payment
   d. System updates status
   e. Sends confirmation email
4. If Installment:
   a. Records partial payment
   b. Updates balance
   c. Allows additional payments
   d. Full payment unlocks results
```

### 7.4 Result Release Workflow
```
1. Admin imports results from CSV/Excel
2. System validates data
   - Matches NBT number
   - Validates barcode
   - Checks test session
3. Creates test result records
4. For each student:
   a. Check if test fully paid
   b. If paid, mark result as available
   c. Send email notification
   d. Student can view/download
5. If not paid:
   a. Result stored but not visible to student
   b. Staff/Admin can still view
6. Student logs in
7. Views results page
8. Downloads PDF certificate with barcode
```

---

## 8. Validation Rules

### 8.1 ID Validation

**SA ID Number:**
```
- Must be exactly 13 digits
- Format: YYMMDDGSSSCAZ
  - YYMMDD: Date of birth
  - G: Gender (0-4 female, 5-9 male)
  - SSS: Sequence
  - C: Citizenship (0 SA, 1 Foreign)
  - A: Usually 8
  - Z: Luhn check digit
- Must pass Luhn validation
- Extract DOB and Gender automatically
```

**Foreign ID:**
```
- 6-20 alphanumeric characters
- Must provide Nationality
- Must provide Country of Origin
```

**Passport:**
```
- 6-20 alphanumeric characters
- Must provide Nationality
- Must provide Country of Origin
```

### 8.2 NBT Number Validation
```
- Format: YYYYNNNNNNNNND (14 digits)
- YYYY: Current year
- NNNNNNNNNN: Sequential number (10 digits)
- D: Luhn check digit
- Must be unique
- Cannot be modified
```

### 8.3 Email Validation
```
- Valid email format
- Must be unique in system
- Required for all users
- OTP verification required
```

### 8.4 Booking Validation
```
- Student cannot have 2 active bookings
- Cannot book if previous test not past closing date
- Maximum 2 tests per year
- Must have available seats
- Venue must be available for selected date
```

### 8.5 Payment Validation
```
- Amount must be positive
- Cannot exceed total owed
- Payment method must be valid
- EasyPay reference must be unique
- Transaction tracking required
```

---

## 9. Performance Requirements

### 9.1 Response Times
- Page load: < 3 seconds
- API response: < 500ms
- Database query: < 200ms
- Payment callback: < 2 seconds
- Report generation: < 10 seconds

### 9.2 Scalability
- Support 10,000 concurrent users
- Handle 1,000 registrations per hour
- Process 500 payments per hour
- Store 100,000+ student records
- Generate reports for 50,000+ records

### 9.3 Availability
- 99.9% uptime
- Scheduled maintenance windows
- Automated backups every 6 hours
- Disaster recovery plan

---

## 10. Security Requirements

### 10.1 Authentication
- JWT tokens with 1-hour expiry
- Refresh tokens with 7-day expiry
- Password hashing with bcrypt
- Multi-factor authentication (OTP)
- Account lockout after 5 failed attempts

### 10.2 Authorization
- Role-based access control
- Route protection
- API endpoint authorization
- Data access filtering by role

### 10.3 Data Protection
- HTTPS only
- Input sanitization
- SQL injection prevention
- XSS protection
- CSRF tokens
- Sensitive data encryption

---

## 11. Integration Specifications

### 11.1 EasyPay Integration
```
Endpoint: https://www.easypay.co.za/api/process
Method: POST
Headers:
  - Authorization: Bearer {API_KEY}
  - Content-Type: application/json

Request:
{
  "merchantId": "{MERCHANT_ID}",
  "reference": "EP-{RegistrationId}-{Timestamp}",
  "amount": 500.00,
  "description": "NBT Test Registration",
  "returnUrl": "https://nbt.ac.za/payments/return",
  "callbackUrl": "https://nbt.ac.za/api/payments/easypay-callback"
}

Response:
{
  "success": true,
  "transactionId": "EP123456789",
  "paymentUrl": "https://www.easypay.co.za/pay/EP123456789"
}

Callback:
{
  "transactionId": "EP123456789",
  "reference": "EP-{RegistrationId}-{Timestamp}",
  "status": "SUCCESS",
  "amount": 500.00,
  "paidDate": "2024-06-15T10:30:00Z"
}
```

### 11.2 Email Service
```
Templates:
- Registration Confirmation
- Email Verification (OTP)
- Booking Confirmation
- Payment Confirmation
- Payment Reminder
- Test Reminder (48 hours before)
- Result Notification

Variables:
- {StudentName}
- {NBTNumber}
- {TestDate}
- {VenueName}
- {Amount}
- {BalanceDue}
- {OTP}
```

### 11.3 SMS Service (Optional)
```
Messages:
- OTP: "Your NBT verification code is: {OTP}"
- Booking: "Your NBT test is booked for {Date} at {Venue}"
- Payment: "Payment of R{Amount} received. Balance: R{Balance}"
- Reminder: "Reminder: NBT test tomorrow at {Time}, {Venue}"
```

---

## 12. Error Handling

### 12.1 Error Codes
```
400 - Bad Request (validation errors)
401 - Unauthorized (authentication required)
403 - Forbidden (insufficient permissions)
404 - Not Found (resource doesn't exist)
409 - Conflict (duplicate record)
422 - Unprocessable Entity (business rule violation)
500 - Internal Server Error
503 - Service Unavailable (maintenance)
```

### 12.2 Error Response Format
```json
{
  "success": false,
  "error": {
    "code": "DUPLICATE_ID",
    "message": "A student with this ID number already exists",
    "details": {
      "field": "IDNumber",
      "value": "9001015009087"
    }
  },
  "timestamp": "2024-06-15T10:30:00Z",
  "path": "/api/students/register"
}
```

---

## 13. Testing Strategy

### 13.1 Unit Tests
- All business logic
- Validation methods
- Luhn algorithm
- Date calculations
- Performance level mapping

### 13.2 Integration Tests
- API endpoints
- Database operations
- External service calls
- Payment processing

### 13.3 E2E Tests
- Registration wizard
- Booking flow
- Payment flow
- Result viewing

### 13.4 Performance Tests
- Load testing (1000 concurrent users)
- Stress testing
- Database query optimization

---

## 14. Deployment Strategy

### 14.1 Environments
- **Development**: Local machines
- **Staging**: Azure App Service (testing)
- **Production**: Azure App Service (live)

### 14.2 CI/CD Pipeline
```
1. Code commit to feature branch
2. GitHub Actions trigger
3. Build solution
4. Run unit tests
5. Run integration tests
6. Code quality analysis
7. Merge to develop
8. Deploy to staging
9. Run E2E tests
10. Merge to main
11. Deploy to production
12. Smoke tests
13. Notify team
```

### 14.3 Database Migrations
```
1. Create migration in development
2. Test migration locally
3. Review migration script
4. Deploy to staging
5. Verify data integrity
6. Deploy to production
7. Rollback plan ready
```

---

## 15. Maintenance and Support

### 15.1 Monitoring
- Application Performance Monitoring (APM)
- Error tracking (Sentry/Application Insights)
- Uptime monitoring
- Database performance
- API response times

### 15.2 Backup and Recovery
- Daily automated backups
- Weekly full backups
- 30-day retention
- Quarterly restore testing
- Disaster recovery plan

### 15.3 Support Channels
- Help desk ticketing system
- Email support
- Phone support (business hours)
- FAQ and knowledge base
- Video tutorials

---

**Approved by:** NBT Development Team  
**Next Review:** 2025-02-09
