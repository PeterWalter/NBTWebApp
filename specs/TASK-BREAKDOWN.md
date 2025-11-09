# NBT Web Application - Complete Task Breakdown

## Document Control
- **Version**: 2.0
- **Last Updated**: 2025-11-09
- **Status**: ACTIVE - Ready for Execution
- **Related**: IMPLEMENTATION-PLAN-COMPLETE.md

---

## Task Status Legend
- ⏳ **Pending**: Not started
- 🔄 **In Progress**: Currently being worked on
- ✅ **Complete**: Finished and tested
- ❌ **Blocked**: Cannot proceed due to dependency
- ⚠️ **On Hold**: Temporarily paused

---

## PHASE 0: Foundation & Cleanup (Week 1)

### 0.1 FluentUI Migration
- ⏳ **TASK-001**: Audit all `.razor` files for MudBlazor references
- ⏳ **TASK-002**: Create FluentUI component mapping document
- ⏳ **TASK-003**: Replace `MudButton` with `FluentButton`
- ⏳ **TASK-004**: Replace `MudTextField` with `FluentTextField`
- ⏳ **TASK-005**: Replace `MudSelect` with `FluentSelect`
- ⏳ **TASK-006**: Replace `MudDataGrid` with `FluentDataGrid`
- ⏳ **TASK-007**: Replace `MudDialog` with `FluentDialog`
- ⏳ **TASK-008**: Replace `MudCard` with `FluentCard`
- ⏳ **TASK-009**: Update theme configuration in `Program.cs`
- ⏳ **TASK-010**: Remove MudBlazor NuGet packages
- ⏳ **TASK-011**: Test all pages for UI consistency
- ⏳ **TASK-012**: Fix any layout issues

**Deliverable**: Zero MudBlazor references, fully FluentUI application

### 0.2 Architecture Review
- ⏳ **TASK-013**: Review Clean Architecture folder structure
- ⏳ **TASK-014**: Verify dependency injection registrations
- ⏳ **TASK-015**: Review entity relationships and navigation properties
- ⏳ **TASK-016**: Validate repository pattern implementation
- ⏳ **TASK-017**: Ensure service layer separation
- ⏳ **TASK-018**: Document architecture decisions (ADR)

**Deliverable**: Architecture review document

### 0.3 CI/CD Setup
- ⏳ **TASK-019**: Create `.github/workflows/build-test.yml`
- ⏳ **TASK-020**: Configure build steps (restore, build, test)
- ⏳ **TASK-021**: Add automated testing step
- ⏳ **TASK-022**: Create `.github/workflows/deploy-staging.yml`
- ⏳ **TASK-023**: Configure Azure credentials for deployment
- ⏳ **TASK-024**: Set up branch protection rules on main
- ⏳ **TASK-025**: Require PR reviews before merge
- ⏳ **TASK-026**: Test CI/CD pipeline with sample PR

**Deliverable**: Functional CI/CD pipeline

### 0.4 Code Standards
- ⏳ **TASK-027**: Create `.editorconfig` with C# style rules
- ⏳ **TASK-028**: Configure code analysis rules
- ⏳ **TASK-029**: Document coding guidelines in `docs/CODING-GUIDELINES.md`
- ⏳ **TASK-030**: Set up pre-commit hooks (optional)

**Deliverable**: Code standards enforced

### 0.5 Database Review
- ⏳ **TASK-031**: Review existing EF Core migrations
- ⏳ **TASK-032**: Verify entity configurations (Fluent API)
- ⏳ **TASK-033**: Add missing indexes for performance
- ⏳ **TASK-034**: Create seed data for reference tables
- ⏳ **TASK-035**: Document database schema (ER diagram)

**Deliverable**: Optimized database with seed data

---

## PHASE 1: Registration & NBT Number (Weeks 2-3)

### 1.1 Domain Models
- ⏳ **TASK-101**: Update `Student` entity with all required fields
- ⏳ **TASK-102**: Add `ForeignId` and `PassportNumber` fields
- ⏳ **TASK-103**: Add `Age`, `Gender`, `Ethnicity` fields
- ⏳ **TASK-104**: Create `Registration` entity
- ⏳ **TASK-105**: Create `BackgroundQuestionnaire` entity
- ⏳ **TASK-106**: Add EF Core entity configurations
- ⏳ **TASK-107**: Create and apply migration

**Deliverable**: Updated domain models

### 1.2 NBT Number Generation
- ⏳ **TASK-108**: Create `LuhnValidator` service
- ⏳ **TASK-109**: Implement Luhn algorithm for validation
- ⏳ **TASK-110**: Create `NBTNumberGenerator` service
- ⏳ **TASK-111**: Implement 14-digit NBT number generation
- ⏳ **TASK-112**: Add uniqueness check against database
- ⏳ **TASK-113**: Create unit tests for Luhn validation
- ⏳ **TASK-114**: Create unit tests for NBT number generation
- ⏳ **TASK-115**: Document algorithm in code comments

**Deliverable**: NBT number generation service with tests

### 1.3 SA ID Validation
- ⏳ **TASK-116**: Create `SAIdValidator` service
- ⏳ **TASK-117**: Implement SA ID Luhn validation
- ⏳ **TASK-118**: Implement DOB extraction (positions 1-6)
- ⏳ **TASK-119**: Implement Gender extraction (position 7)
- ⏳ **TASK-120**: Validate DOB is valid date
- ⏳ **TASK-121**: Create unit tests for SA ID validation
- ⏳ **TASK-122**: Create unit tests for DOB extraction
- ⏳ **TASK-123**: Create unit tests for Gender extraction

**Deliverable**: SA ID validation service with tests

### 1.4 Registration Wizard - Step 1
- ⏳ **TASK-124**: Create `RegistrationWizard.razor` parent component
- ⏳ **TASK-125**: Implement step navigation logic
- ⏳ **TASK-126**: Create progress indicator (1 of 4, 2 of 4, etc.)
- ⏳ **TASK-127**: Create `Step1AccountPersonal.razor`
- ⏳ **TASK-128**: Add email and password fields
- ⏳ **TASK-129**: Add SA ID field with Luhn validation
- ⏳ **TASK-130**: Add Foreign ID / Passport alternative fields
- ⏳ **TASK-131**: Implement conditional display (SA ID OR Foreign ID)
- ⏳ **TASK-132**: Auto-extract DOB and Gender from SA ID
- ⏳ **TASK-133**: Add Name and Surname fields
- ⏳ **TASK-134**: Calculate Age from DOB (display only, not editable)
- ⏳ **TASK-135**: Add Gender field (auto-filled from SA ID or manual)
- ⏳ **TASK-136**: Add Ethnicity dropdown
- ⏳ **TASK-137**: Add phone number field with validation
- ⏳ **TASK-138**: Implement client-side validation
- ⏳ **TASK-139**: Implement "Next" button with server validation
- ⏳ **TASK-140**: Save progress to database

**Deliverable**: Functional Step 1 of wizard

### 1.5 Registration Wizard - Step 2
- ⏳ **TASK-141**: Create `Step2AcademicTest.razor`
- ⏳ **TASK-142**: Add school/institution name field
- ⏳ **TASK-143**: Add current grade dropdown
- ⏳ **TASK-144**: Add test type selector (AQL or AQL+MAT)
- ⏳ **TASK-145**: Add preferred language dropdown
- ⏳ **TASK-146**: Add special accommodation checkbox
- ⏳ **TASK-147**: Add accommodation details textarea (conditional)
- ⏳ **TASK-148**: Implement validation
- ⏳ **TASK-149**: Implement "Previous" and "Next" buttons
- ⏳ **TASK-150**: Save progress to database

**Deliverable**: Functional Step 2 of wizard

### 1.6 Registration Wizard - Step 3
- ⏳ **TASK-151**: Create `Step3VenueBooking.razor`
- ⏳ **TASK-152**: Add venue type selector (National, Special, Online, etc.)
- ⏳ **TASK-153**: Create test date picker component
- ⏳ **TASK-154**: Fetch available test dates from API
- ⏳ **TASK-155**: Highlight Sunday tests (color-coded)
- ⏳ **TASK-156**: Highlight Online tests (color-coded)
- ⏳ **TASK-157**: Add venue dropdown (filtered by date and type)
- ⏳ **TASK-158**: Display venue details (address, capacity)
- ⏳ **TASK-159**: Implement validation
- ⏳ **TASK-160**: Implement "Previous" and "Next" buttons
- ⏳ **TASK-161**: Save progress to database

**Deliverable**: Functional Step 3 of wizard

### 1.7 Registration Wizard - Step 4
- ⏳ **TASK-162**: Create `Step4Survey.razor`
- ⏳ **TASK-163**: Design background questionnaire questions
- ⏳ **TASK-164**: Create dynamic question components
- ⏳ **TASK-165**: Implement question validation
- ⏳ **TASK-166**: Serialize answers to JSON
- ⏳ **TASK-167**: Implement "Previous" button
- ⏳ **TASK-168**: Implement "Submit" button
- ⏳ **TASK-169**: Save questionnaire data to database

**Deliverable**: Functional Step 4 of wizard

### 1.8 Wizard Completion
- ⏳ **TASK-170**: Generate NBT number on wizard completion
- ⏳ **TASK-171**: Create student account in database
- ⏳ **TASK-172**: Send OTP via email for verification
- ⏳ **TASK-173**: Create OTP verification page
- ⏳ **TASK-174**: Validate OTP code
- ⏳ **TASK-175**: Activate student account
- ⏳ **TASK-176**: Send welcome email with NBT number
- ⏳ **TASK-177**: Redirect to student dashboard
- ⏳ **TASK-178**: Display success message with NBT number

**Deliverable**: Complete registration flow

### 1.9 Resume Capability
- ⏳ **TASK-179**: Store `CurrentStep` field in `Registration` entity
- ⏳ **TASK-180**: Check for incomplete registrations on login
- ⏳ **TASK-181**: Redirect to correct wizard step
- ⏳ **TASK-182**: Load saved data into form fields
- ⏳ **TASK-183**: Test resume flow after interruption

**Deliverable**: Resume registration capability

### 1.10 API Endpoints
- ⏳ **TASK-184**: Create `RegistrationController`
- ⏳ **TASK-185**: Implement `POST /api/v1/registration/start`
- ⏳ **TASK-186**: Implement `GET /api/v1/registration/{id}`
- ⏳ **TASK-187**: Implement `PUT /api/v1/registration/{id}/step`
- ⏳ **TASK-188**: Implement `POST /api/v1/registration/{id}/complete`
- ⏳ **TASK-189**: Implement `GET /api/v1/registration/resume/{studentId}`
- ⏳ **TASK-190**: Implement `POST /api/v1/registration/validate-id`
- ⏳ **TASK-191**: Implement `POST /api/v1/registration/generate-nbt-number`
- ⏳ **TASK-192**: Add Swagger documentation for all endpoints

**Deliverable**: Registration API endpoints

### 1.11 Testing
- ⏳ **TASK-193**: Unit tests for `LuhnValidator`
- ⏳ **TASK-194**: Unit tests for `NBTNumberGenerator`
- ⏳ **TASK-195**: Unit tests for `SAIdValidator`
- ⏳ **TASK-196**: Unit tests for DOB/Gender extraction
- ⏳ **TASK-197**: Integration tests for registration API
- ⏳ **TASK-198**: E2E test: Complete registration (SA ID)
- ⏳ **TASK-199**: E2E test: Complete registration (Foreign ID)
- ⏳ **TASK-200**: E2E test: Resume interrupted registration

**Deliverable**: Full test coverage for Phase 1

---

## PHASE 2: Booking System (Weeks 4-5)

### 2.1 Domain Models
- ⏳ **TASK-201**: Create/update `Booking` entity
- ⏳ **TASK-202**: Add booking rules fields (test cost by year, etc.)
- ⏳ **TASK-203**: Create `TestSession` entity
- ⏳ **TASK-204**: Link TestSession to Venue (not Room)
- ⏳ **TASK-205**: Create `TestDate` entity for calendar
- ⏳ **TASK-206**: Add EF Core configurations
- ⏳ **TASK-207**: Create and apply migration

**Deliverable**: Booking domain models

### 2.2 Booking Eligibility Service
- ⏳ **TASK-208**: Create `BookingEligibilityService`
- ⏳ **TASK-209**: Implement check for active bookings
- ⏳ **TASK-210**: Implement check for previous test closing date
- ⏳ **TASK-211**: Implement annual limit check (max 2 per year)
- ⏳ **TASK-212**: Implement test validity check (3 years)
- ⏳ **TASK-213**: Create eligibility result DTO
- ⏳ **TASK-214**: Unit tests for eligibility logic

**Deliverable**: Booking eligibility service with tests

### 2.3 Test Calendar Component
- ⏳ **TASK-215**: Create `TestCalendar.razor` FluentUI component
- ⏳ **TASK-216**: Fetch available test dates from API
- ⏳ **TASK-217**: Display calendar view
- ⏳ **TASK-218**: Highlight Sunday tests (e.g., orange color)
- ⏳ **TASK-219**: Highlight Online tests (e.g., blue color)
- ⏳ **TASK-220**: Show closing dates below test dates
- ⏳ **TASK-221**: Add legend for color coding
- ⏳ **TASK-222**: Implement date selection
- ⏳ **TASK-223**: Filter by test type (AQL, MAT, Both)

**Deliverable**: Test calendar component

### 2.4 Booking Workflow
- ⏳ **TASK-224**: Create `BookingInitiation.razor` page
- ⏳ **TASK-225**: Check eligibility on page load
- ⏳ **TASK-226**: Display eligibility status to user
- ⏳ **TASK-227**: Add test type selection (AQL or AQL+MAT)
- ⏳ **TASK-228**: Display test calendar
- ⏳ **TASK-229**: Add venue selection dropdown
- ⏳ **TASK-230**: Display venue details (capacity, address)
- ⏳ **TASK-231**: Create `BookingConfirmation.razor` page
- ⏳ **TASK-232**: Display booking summary
- ⏳ **TASK-233**: Implement "Confirm Booking" button
- ⏳ **TASK-234**: Save booking to database
- ⏳ **TASK-235**: Send booking confirmation email
- ⏳ **TASK-236**: Redirect to payment page

**Deliverable**: Complete booking workflow

### 2.5 Booking Modification
- ⏳ **TASK-237**: Create `BookingModification.razor` page
- ⏳ **TASK-238**: Check if modification allowed (before closing date)
- ⏳ **TASK-239**: Load current booking details
- ⏳ **TASK-240**: Allow venue change
- ⏳ **TASK-241**: Allow test date change
- ⏳ **TASK-242**: Update booking record
- ⏳ **TASK-243**: Send modification confirmation email

**Deliverable**: Booking modification capability

### 2.6 Online Test Support
- ⏳ **TASK-244**: Add `IsOnlineTest` flag to TestSession
- ⏳ **TASK-245**: Create `OnlineTestRequirements.razor` page
- ⏳ **TASK-246**: Display technical requirements (video, sound, internet)
- ⏳ **TASK-247**: Add terms acceptance checkbox
- ⏳ **TASK-248**: Store online test preferences

**Deliverable**: Online test support

### 2.7 API Endpoints
- ⏳ **TASK-249**: Create `BookingController`
- ⏳ **TASK-250**: Implement `GET /api/v1/booking/available-sessions`
- ⏳ **TASK-251**: Implement `POST /api/v1/booking/create`
- ⏳ **TASK-252**: Implement `GET /api/v1/booking/{id}`
- ⏳ **TASK-253**: Implement `PUT /api/v1/booking/{id}`
- ⏳ **TASK-254**: Implement `DELETE /api/v1/booking/{id}`
- ⏳ **TASK-255**: Implement `GET /api/v1/booking/student/{studentId}`
- ⏳ **TASK-256**: Implement `GET /api/v1/booking/check-eligibility/{studentId}`
- ⏳ **TASK-257**: Add Swagger documentation

**Deliverable**: Booking API endpoints

### 2.8 Testing
- ⏳ **TASK-258**: Unit tests for eligibility service
- ⏳ **TASK-259**: Unit tests for booking validation
- ⏳ **TASK-260**: Integration tests for booking API
- ⏳ **TASK-261**: E2E test: Complete booking flow
- ⏳ **TASK-262**: E2E test: Eligibility check scenarios
- ⏳ **TASK-263**: E2E test: Booking modification
- ⏳ **TASK-264**: E2E test: Online test booking

**Deliverable**: Full test coverage for Phase 2

---

## PHASE 3: Payment Integration (Weeks 6-7)

### 3.1 Domain Models
- ⏳ **TASK-301**: Create/update `Payment` entity
- ⏳ **TASK-302**: Add installment tracking fields
- ⏳ **TASK-303**: Add intake year cost tracking
- ⏳ **TASK-304**: Add payment method enum
- ⏳ **TASK-305**: Add EasyPay reference field
- ⏳ **TASK-306**: Add EF Core configurations
- ⏳ **TASK-307**: Create and apply migration

**Deliverable**: Payment domain models

### 3.2 Payment Calculation Service
- ⏳ **TASK-308**: Create `PaymentCalculationService`
- ⏳ **TASK-309**: Get test cost by intake year
- ⏳ **TASK-310**: Calculate total amount paid for booking
- ⏳ **TASK-311**: Calculate remaining balance
- ⏳ **TASK-312**: Determine payment order (oldest first)
- ⏳ **TASK-313**: Track installments
- ⏳ **TASK-314**: Unit tests for calculations

**Deliverable**: Payment calculation service with tests

### 3.3 EasyPay Integration
- ⏳ **TASK-315**: Create `EasyPayService`
- ⏳ **TASK-316**: Configure EasyPay API credentials (appsettings)
- ⏳ **TASK-317**: Implement payment reference generation
- ⏳ **TASK-318**: Implement payment initiation API call
- ⏳ **TASK-319**: Create webhook endpoint for EasyPay callbacks
- ⏳ **TASK-320**: Implement webhook signature validation
- ⏳ **TASK-321**: Handle payment status updates from webhook
- ⏳ **TASK-322**: Test with EasyPay sandbox environment

**Deliverable**: EasyPay integration

### 3.4 Payment UI
- ⏳ **TASK-323**: Create `PaymentInitiation.razor` page
- ⏳ **TASK-324**: Display test cost by intake year
- ⏳ **TASK-325**: Show payment breakdown (amount paid, remaining)
- ⏳ **TASK-326**: Generate and display EasyPay reference
- ⏳ **TASK-327**: Display payment instructions
- ⏳ **TASK-328**: Add "Proceed to EasyPay" button
- ⏳ **TASK-329**: Create `PaymentStatus.razor` page
- ⏳ **TASK-330**: Poll for payment status updates
- ⏳ **TASK-331**: Display payment confirmation

**Deliverable**: Payment UI pages

### 3.5 Installment Payment
- ⏳ **TASK-332**: Create `InstallmentPlan.razor` component
- ⏳ **TASK-333**: Display installment schedule
- ⏳ **TASK-334**: Track partial payments
- ⏳ **TASK-335**: Calculate remaining balance after each payment
- ⏳ **TASK-336**: Update booking status when fully paid
- ⏳ **TASK-337**: Send payment reminder emails

**Deliverable**: Installment payment capability

### 3.6 Bank Payment Upload
- ⏳ **TASK-338**: Define bank payment file format (CSV/Excel)
- ⏳ **TASK-339**: Create `BankPaymentUpload.razor` page (Staff)
- ⏳ **TASK-340**: Add file upload component
- ⏳ **TASK-341**: Create `BankPaymentParser` service
- ⏳ **TASK-342**: Parse CSV/Excel file
- ⏳ **TASK-343**: Validate payment data
- ⏳ **TASK-344**: Match payments to bookings by reference
- ⏳ **TASK-345**: Update payment records
- ⏳ **TASK-346**: Generate processing report

**Deliverable**: Bank payment upload functionality

### 3.7 Payment History
- ⏳ **TASK-347**: Create `PaymentHistory.razor` page
- ⏳ **TASK-348**: Display all payments for student
- ⏳ **TASK-349**: Show payment status (Pending, Completed, Failed)
- ⏳ **TASK-350**: Show remaining balance
- ⏳ **TASK-351**: Add download receipt button
- ⏳ **TASK-352**: Generate PDF receipt

**Deliverable**: Payment history page

### 3.8 API Endpoints
- ⏳ **TASK-353**: Create `PaymentController`
- ⏳ **TASK-354**: Implement `POST /api/v1/payments/initiate`
- ⏳ **TASK-355**: Implement `GET /api/v1/payments/{id}`
- ⏳ **TASK-356**: Implement `PUT /api/v1/payments/{id}/confirm`
- ⏳ **TASK-357**: Implement `GET /api/v1/payments/booking/{bookingId}`
- ⏳ **TASK-358**: Implement `POST /api/v1/payments/bank-upload`
- ⏳ **TASK-359**: Implement `GET /api/v1/payments/student/{studentId}`
- ⏳ **TASK-360**: Implement `GET /api/v1/payments/status/{easyPayReference}`
- ⏳ **TASK-361**: Implement `POST /api/v1/payments/webhook` (EasyPay)
- ⏳ **TASK-362**: Add Swagger documentation

**Deliverable**: Payment API endpoints

### 3.9 Testing
- ⏳ **TASK-363**: Unit tests for payment calculations
- ⏳ **TASK-364**: Unit tests for EasyPay service (mocked)
- ⏳ **TASK-365**: Integration tests with EasyPay sandbox
- ⏳ **TASK-366**: Unit tests for bank payment parser
- ⏳ **TASK-367**: Integration tests for payment API
- ⏳ **TASK-368**: E2E test: Complete payment flow (EasyPay)
- ⏳ **TASK-369**: E2E test: Installment payment
- ⏳ **TASK-370**: E2E test: Bank payment upload

**Deliverable**: Full test coverage for Phase 3

---

## PHASE 4: Results Management (Week 8)

### 4.1 Domain Models
- ⏳ **TASK-401**: Create/update `TestResult` entity
- ⏳ **TASK-402**: Add barcode field (unique identifier)
- ⏳ **TASK-403**: Add performance level fields (AL, QL, MAT)
- ⏳ **TASK-404**: Add visibility flag (based on payment)
- ⏳ **TASK-405**: Add EF Core configurations
- ⏳ **TASK-406**: Create and apply migration

**Deliverable**: Result domain models

### 4.2 Result Import Service
- ⏳ **TASK-407**: Create `ResultImportService`
- ⏳ **TASK-408**: Define import file format (CSV/Excel)
- ⏳ **TASK-409**: Create file parser
- ⏳ **TASK-410**: Validate result data (barcode, student ID, scores)
- ⏳ **TASK-411**: Match results to students and bookings
- ⏳ **TASK-412**: Assign unique barcodes
- ⏳ **TASK-413**: Unit tests for import logic

**Deliverable**: Result import service with tests

### 4.3 Result Visibility Logic
- ⏳ **TASK-414**: Create `ResultVisibilityService`
- ⏳ **TASK-415**: Check payment status for booking
- ⏳ **TASK-416**: Set visibility flag on result
- ⏳ **TASK-417**: Student can only see fully paid results
- ⏳ **TASK-418**: Staff/Admin can see all results
- ⏳ **TASK-419**: Unit tests for visibility logic

**Deliverable**: Result visibility service with tests

### 4.4 Result Import UI (Staff)
- ⏳ **TASK-420**: Create `ResultImport.razor` page
- ⏳ **TASK-421**: Add file upload component
- ⏳ **TASK-422**: Display import preview table
- ⏳ **TASK-423**: Show validation errors
- ⏳ **TASK-424**: Implement "Confirm Import" button
- ⏳ **TASK-425**: Display import results summary

**Deliverable**: Result import UI

### 4.5 Result Display UI (Student)
- ⏳ **TASK-426**: Create `MyResults.razor` page
- ⏳ **TASK-427**: Fetch visible results for student
- ⏳ **TASK-428**: Display result cards (one per test)
- ⏳ **TASK-429**: Show AL score and performance level
- ⏳ **TASK-430**: Show QL score and performance level
- ⏳ **TASK-431**: Show MAT score and performance level (if applicable)
- ⏳ **TASK-432**: Display barcode
- ⏳ **TASK-433**: Show test date
- ⏳ **TASK-434**: Add "Download Certificate" button (if paid)

**Deliverable**: Result display UI

### 4.6 PDF Certificate Generation
- ⏳ **TASK-435**: Create `PdfGenerationService`
- ⏳ **TASK-436**: Design certificate template
- ⏳ **TASK-437**: Implement PDF generation with iText7
- ⏳ **TASK-438**: Include student details
- ⏳ **TASK-439**: Include test scores and performance levels
- ⏳ **TASK-440**: Add barcode to certificate
- ⏳ **TASK-441**: Add NBT logo and branding
- ⏳ **TASK-442**: Test PDF generation with sample data

**Deliverable**: PDF certificate generation

### 4.7 Result Notifications
- ⏳ **TASK-443**: Create email template for result notification
- ⏳ **TASK-444**: Send email when results available (only for paid tests)
- ⏳ **TASK-445**: Include result summary in email
- ⏳ **TASK-446**: Add link to view results online

**Deliverable**: Result notification emails

### 4.8 Staff Result Management
- ⏳ **TASK-447**: Create `StaffResultManagement.razor` page
- ⏳ **TASK-448**: Display all results (FluentDataGrid)
- ⏳ **TASK-449**: Add search by student, NBT number, barcode
- ⏳ **TASK-450**: Add filter by payment status
- ⏳ **TASK-451**: Create `ManualResultEntry.razor` page
- ⏳ **TASK-452**: Allow manual result entry
- ⏳ **TASK-453**: Allow result editing (with audit log)

**Deliverable**: Staff result management

### 4.9 API Endpoints
- ⏳ **TASK-454**: Create `ResultsController`
- ⏳ **TASK-455**: Implement `GET /api/v1/results/student/{studentId}`
- ⏳ **TASK-456**: Implement `GET /api/v1/results/{id}`
- ⏳ **TASK-457**: Implement `POST /api/v1/results/import`
- ⏳ **TASK-458**: Implement `GET /api/v1/results/{id}/pdf`
- ⏳ **TASK-459**: Implement `GET /api/v1/results/barcode/{barcode}`
- ⏳ **TASK-460**: Implement `PUT /api/v1/results/{id}/visibility`
- ⏳ **TASK-461**: Implement `POST /api/v1/results` (manual entry)
- ⏳ **TASK-462**: Implement `PUT /api/v1/results/{id}` (edit)
- ⏳ **TASK-463**: Add Swagger documentation

**Deliverable**: Results API endpoints

### 4.10 Testing
- ⏳ **TASK-464**: Unit tests for import service
- ⏳ **TASK-465**: Unit tests for visibility service
- ⏳ **TASK-466**: Unit tests for PDF generation
- ⏳ **TASK-467**: Integration tests for results API
- ⏳ **TASK-468**: E2E test: Result import flow
- ⏳ **TASK-469**: E2E test: Student views result (paid)
- ⏳ **TASK-470**: E2E test: Student cannot view result (unpaid)

**Deliverable**: Full test coverage for Phase 4

---

## PHASE 5: Venue & Calendar Management (Week 9)

### 5.1 Domain Models
- ⏳ **TASK-501**: Update `Venue` entity with all fields
- ⏳ **TASK-502**: Add `VenueType` enum (National, Special Session, Research, Other)
- ⏳ **TASK-503**: Create `VenueDateAvailability` entity
- ⏳ **TASK-504**: Update `Room` entity (information only)
- ⏳ **TASK-505**: Update `TestSession` to link to Venue (not Room)
- ⏳ **TASK-506**: Add EF Core configurations
- ⏳ **TASK-507**: Create and apply migration

**Deliverable**: Venue domain models

### 5.2 Venue Management (Admin/Staff)
- ⏳ **TASK-508**: Create `VenueList.razor` page
- ⏳ **TASK-509**: Display venues in FluentDataGrid
- ⏳ **TASK-510**: Add search and filter
- ⏳ **TASK-511**: Create `VenueEdit.razor` page
- ⏳ **TASK-512**: Add venue type selector
- ⏳ **TASK-513**: Add address fields
- ⏳ **TASK-514**: Add capacity field
- ⏳ **TASK-515**: Implement Create/Update/Delete operations

**Deliverable**: Venue management UI

### 5.3 Date Availability Management
- ⏳ **TASK-516**: Create `VenueDateAvailability.razor` page
- ⏳ **TASK-517**: Display availability calendar
- ⏳ **TASK-518**: Mark dates as available/unavailable
- ⏳ **TASK-519**: Add reason field for unavailability
- ⏳ **TASK-520**: Implement bulk date updates
- ⏳ **TASK-521**: Visual indicator for availability status

**Deliverable**: Date availability management

### 5.4 Room Management
- ⏳ **TASK-522**: Create `RoomList.razor` page (per venue)
- ⏳ **TASK-523**: Display rooms in FluentDataGrid
- ⏳ **TASK-524**: Create `RoomEdit.razor` page
- ⏳ **TASK-525**: Add room number and name fields
- ⏳ **TASK-526**: Add capacity field
- ⏳ **TASK-527**: Note in UI: Rooms are for information only

**Deliverable**: Room management UI

### 5.5 Test Session Management
- ⏳ **TASK-528**: Create `TestSessionList.razor` page
- ⏳ **TASK-529**: Display sessions in FluentDataGrid
- ⏳ **TASK-530**: Create `TestSessionEdit.razor` page
- ⏳ **TASK-531**: Link to Venue (not Room)
- ⏳ **TASK-532**: Set date, time, capacity
- ⏳ **TASK-533**: Add Sunday test checkbox
- ⏳ **TASK-534**: Add online test checkbox
- ⏳ **TASK-535**: Track registered count

**Deliverable**: Test session management UI

### 5.6 Venue Selection (Student)
- ⏳ **TASK-536**: Update booking flow to filter venues by date
- ⏳ **TASK-537**: Display only available venues for selected date
- ⏳ **TASK-538**: Filter by venue type
- ⏳ **TASK-539**: Show capacity status (Available, Limited, Full)
- ⏳ **TASK-540**: Display venue details (address, map link)

**Deliverable**: Enhanced venue selection

### 5.7 API Endpoints
- ⏳ **TASK-541**: Create `VenueController`
- ⏳ **TASK-542**: Implement `GET /api/v1/venues`
- ⏳ **TASK-543**: Implement `GET /api/v1/venues/{id}`
- ⏳ **TASK-544**: Implement `POST /api/v1/venues`
- ⏳ **TASK-545**: Implement `PUT /api/v1/venues/{id}`
- ⏳ **TASK-546**: Implement `DELETE /api/v1/venues/{id}`
- ⏳ **TASK-547**: Implement `GET /api/v1/venues/available/{date}`
- ⏳ **TASK-548**: Implement `POST /api/v1/venues/{id}/availability`
- ⏳ **TASK-549**: Create `TestSessionController`
- ⏳ **TASK-550**: Implement `GET /api/v1/test-sessions`
- ⏳ **TASK-551**: Implement `POST /api/v1/test-sessions`
- ⏳ **TASK-552**: Implement `PUT /api/v1/test-sessions/{id}`
- ⏳ **TASK-553**: Implement `GET /api/v1/test-sessions/calendar`
- ⏳ **TASK-554**: Add Swagger documentation

**Deliverable**: Venue and session API endpoints

### 5.8 Testing
- ⏳ **TASK-555**: Unit tests for venue services
- ⏳ **TASK-556**: Unit tests for availability logic
- ⏳ **TASK-557**: Integration tests for venue API
- ⏳ **TASK-558**: Integration tests for test session API
- ⏳ **TASK-559**: E2E test: Create and manage venue
- ⏳ **TASK-560**: E2E test: Set venue availability

**Deliverable**: Full test coverage for Phase 5

---

## PHASE 6: Dashboards & Reports (Weeks 10-11)

### 6.1 Student Dashboard
- ⏳ **TASK-601**: Create `StudentDashboard.razor` layout
- ⏳ **TASK-602**: Add left-side navigation menu
- ⏳ **TASK-603**: Create summary widgets (bookings, payments, results)
- ⏳ **TASK-604**: Display recent activity timeline
- ⏳ **TASK-605**: Add quick action buttons
- ⏳ **TASK-606**: Create profile page
- ⏳ **TASK-607**: Create my bookings page
- ⏳ **TASK-608**: Create my payments page
- ⏳ **TASK-609**: Create my results page (already done in Phase 4)

**Deliverable**: Student dashboard

### 6.2 Staff Dashboard
- ⏳ **TASK-610**: Create `StaffDashboard.razor` layout
- ⏳ **TASK-611**: Add left-side navigation menu
- ⏳ **TASK-612**: Create summary widgets (registrations, payments, etc.)
- ⏳ **TASK-613**: Display pending actions section
- ⏳ **TASK-614**: Create student management page with search/filter
- ⏳ **TASK-615**: Create booking management page
- ⏳ **TASK-616**: Create payment management page
- ⏳ **TASK-617**: Create result management page (already done in Phase 4)
- ⏳ **TASK-618**: Add quick action buttons

**Deliverable**: Staff dashboard

### 6.3 Admin Dashboard
- ⏳ **TASK-619**: Create `AdminDashboard.razor` layout
- ⏳ **TASK-620**: Add system overview widgets
- ⏳ **TASK-621**: Create user management page
- ⏳ **TASK-622**: Create role management
- ⏳ **TASK-623**: Create configuration page
- ⏳ **TASK-624**: Create audit log viewer
- ⏳ **TASK-625**: Add system health indicators

**Deliverable**: Admin dashboard

### 6.4 Special Session Management (Staff)
- ⏳ **TASK-626**: Create `SpecialSessionRequests.razor` page
- ⏳ **TASK-627**: Display pending requests in FluentDataGrid
- ⏳ **TASK-628**: Show request details (invigilator, venue)
- ⏳ **TASK-629**: Add approval workflow
- ⏳ **TASK-630**: Add rejection workflow
- ⏳ **TASK-631**: Capture reviewer comments
- ⏳ **TASK-632**: Send notifications to applicant

**Deliverable**: Special session management

### 6.5 Report Generation
- ⏳ **TASK-633**: Create `ReportGeneration.razor` page
- ⏳ **TASK-634**: Add report type selector
- ⏳ **TASK-635**: Add date range picker
- ⏳ **TASK-636**: Add filter options
- ⏳ **TASK-637**: Create registration report service
- ⏳ **TASK-638**: Create payment report service
- ⏳ **TASK-639**: Create result report service
- ⏳ **TASK-640**: Create venue utilization report service
- ⏳ **TASK-641**: Display report preview

**Deliverable**: Report generation UI

### 6.6 Excel Export
- ⏳ **TASK-642**: Install EPPlus NuGet package
- ⏳ **TASK-643**: Create `ExcelExportService`
- ⏳ **TASK-644**: Implement registration report Excel generation
- ⏳ **TASK-645**: Implement payment report Excel generation
- ⏳ **TASK-646**: Implement result report Excel generation
- ⏳ **TASK-647**: Format worksheets with headers and styling
- ⏳ **TASK-648**: Add charts and summaries
- ⏳ **TASK-649**: Test with large datasets

**Deliverable**: Excel export functionality

### 6.7 PDF Export
- ⏳ **TASK-650**: Install iText7 NuGet package
- ⏳ **TASK-651**: Create `PdfReportService`
- ⏳ **TASK-652**: Design PDF report templates
- ⏳ **TASK-653**: Implement registration report PDF generation
- ⏳ **TASK-654**: Implement payment report PDF generation
- ⏳ **TASK-655**: Implement result report PDF generation
- ⏳ **TASK-656**: Add charts and tables
- ⏳ **TASK-657**: Test with large datasets

**Deliverable**: PDF export functionality

### 6.8 CRUD Operations (Staff/Admin)
- ⏳ **TASK-658**: Students: View, Edit, Disable/Enable
- ⏳ **TASK-659**: Bookings: View, Edit, Cancel
- ⏳ **TASK-660**: Payments: View, Manual Entry, Adjust
- ⏳ **TASK-661**: Results: View All, Edit, Import (done in Phase 4)
- ⏳ **TASK-662**: Venues: Full CRUD (done in Phase 5)
- ⏳ **TASK-663**: Test Sessions: Full CRUD (done in Phase 5)
- ⏳ **TASK-664**: Users: Full CRUD (Admin only)

**Deliverable**: Complete CRUD operations

### 6.9 API Endpoints
- ⏳ **TASK-665**: Create `StaffController`
- ⏳ **TASK-666**: Implement `GET /api/v1/staff/students`
- ⏳ **TASK-667**: Implement `PUT /api/v1/staff/students/{id}`
- ⏳ **TASK-668**: Implement `GET /api/v1/staff/bookings`
- ⏳ **TASK-669**: Implement `GET /api/v1/staff/payments`
- ⏳ **TASK-670**: Implement `POST /api/v1/staff/payments/manual-adjustment`
- ⏳ **TASK-671**: Implement `GET /api/v1/staff/special-sessions`
- ⏳ **TASK-672**: Implement `PUT /api/v1/staff/special-sessions/{id}/approve`
- ⏳ **TASK-673**: Implement `PUT /api/v1/staff/special-sessions/{id}/reject`
- ⏳ **TASK-674**: Create `ReportsController`
- ⏳ **TASK-675**: Implement `GET /api/v1/reports/registrations`
- ⏳ **TASK-676**: Implement `GET /api/v1/reports/payments`
- ⏳ **TASK-677**: Implement `GET /api/v1/reports/results`
- ⏳ **TASK-678**: Implement `GET /api/v1/reports/venues`
- ⏳ **TASK-679**: Implement `POST /api/v1/reports/export/excel`
- ⏳ **TASK-680**: Implement `POST /api/v1/reports/export/pdf`
- ⏳ **TASK-681**: Add Swagger documentation

**Deliverable**: Staff and reports API endpoints

### 6.10 Testing
- ⏳ **TASK-682**: Test all dashboard pages
- ⏳ **TASK-683**: Test CRUD operations
- ⏳ **TASK-684**: Test report generation
- ⏳ **TASK-685**: Test Excel export
- ⏳ **TASK-686**: Test PDF export
- ⏳ **TASK-687**: E2E tests for workflows

**Deliverable**: Full test coverage for Phase 6

---

## PHASE 7: Landing Page & Content (Week 12)

### 7.1 Landing Page Design
- ⏳ **TASK-701**: Design landing page layout (wireframe)
- ⏳ **TASK-702**: Create `LandingPage.razor`
- ⏳ **TASK-703**: Build hero section with main message
- ⏳ **TASK-704**: Create main navigation (Applicants, Institutions, Educators)
- ⏳ **TASK-705**: Build footer with links and contact info
- ⏳ **TASK-706**: Implement responsive design (mobile, tablet, desktop)

**Deliverable**: Landing page layout

### 7.2 Applicants Menu
- ⏳ **TASK-707**: Research current NBT website menu structure
- ⏳ **TASK-708**: Create submenus matching current site
- ⏳ **TASK-709**: Create "About the NBT" page
- ⏳ **TASK-710**: Create "Test Information" page
- ⏳ **TASK-711**: Create "How to Register" page
- ⏳ **TASK-712**: Create "How to Prepare" page
- ⏳ **TASK-713**: Create "Test Centers" page
- ⏳ **TASK-714**: Create "FAQs for Applicants" page
- ⏳ **TASK-715**: Add "Register" call-to-action button
- ⏳ **TASK-716**: Add "Login" link

**Deliverable**: Applicants menu and pages

### 7.3 Institutions Menu
- ⏳ **TASK-717**: Research current NBT website menu structure
- ⏳ **TASK-718**: Create submenus matching current site
- ⏳ **TASK-719**: Create "About Institutional Use" page
- ⏳ **TASK-720**: Create "How to Register Institution" page
- ⏳ **TASK-721**: Create "Bulk Booking Information" page
- ⏳ **TASK-722**: Create "Result Access for Institutions" page
- ⏳ **TASK-723**: Create "Reports for Institutions" page
- ⏳ **TASK-724**: Create "FAQs for Institutions" page
- ⏳ **TASK-725**: Add institutional login link

**Deliverable**: Institutions menu and pages

### 7.4 Educators Menu
- ⏳ **TASK-726**: Research current NBT website menu structure
- ⏳ **TASK-727**: Create submenus matching current site
- ⏳ **TASK-728**: Create "Resources for Educators" page
- ⏳ **TASK-729**: Create "Test Specifications" page
- ⏳ **TASK-730**: Create "Teaching Materials" page
- ⏳ **TASK-731**: Create "Professional Development" page
- ⏳ **TASK-732**: Create "FAQs for Educators" page
- ⏳ **TASK-733**: Add resource download functionality

**Deliverable**: Educators menu and pages

### 7.5 Static Pages
- ⏳ **TASK-734**: Create "About Us" page
- ⏳ **TASK-735**: Create "Contact Us" page with form
- ⏳ **TASK-736**: Create "Privacy Policy" page
- ⏳ **TASK-737**: Create "Terms of Service" page
- ⏳ **TASK-738**: Create "Accessibility Statement" page
- ⏳ **TASK-739**: Create sitemap.xml

**Deliverable**: Static pages

### 7.6 Video Integration
- ⏳ **TASK-740**: Identify videos from current NBT website
- ⏳ **TASK-741**: Embed videos on relevant pages
- ⏳ **TASK-742**: Add video player controls (FluentUI)
- ⏳ **TASK-743**: Ensure accessibility (captions, transcripts)
- ⏳ **TASK-744**: Test video loading and playback

**Deliverable**: Video integration

### 7.7 Content Management
- ⏳ **TASK-745**: Decide content management approach (CMS or markdown)
- ⏳ **TASK-746**: Set up content storage
- ⏳ **TASK-747**: Version control for content
- ⏳ **TASK-748**: Content review workflow

**Deliverable**: Content management system

### 7.8 SEO & Analytics
- ⏳ **TASK-749**: Add meta tags to all pages
- ⏳ **TASK-750**: Create robots.txt
- ⏳ **TASK-751**: Generate sitemap.xml
- ⏳ **TASK-752**: Integrate Google Analytics
- ⏳ **TASK-753**: Test SEO optimization with tools

**Deliverable**: SEO and analytics setup

### 7.9 Testing
- ⏳ **TASK-754**: Test all navigation menus
- ⏳ **TASK-755**: Test responsive design on various devices
- ⏳ **TASK-756**: Test video playback
- ⏳ **TASK-757**: Test contact form
- ⏳ **TASK-758**: Accessibility testing (WCAG 2.1 AA)

**Deliverable**: Fully tested landing page and content

---

## PHASE 8: Testing & Deployment (Weeks 13-14)

### 8.1 Unit Testing
- ⏳ **TASK-801**: Ensure >80% code coverage
- ⏳ **TASK-802**: Review and improve existing unit tests
- ⏳ **TASK-803**: Add missing unit tests
- ⏳ **TASK-804**: Test all services
- ⏳ **TASK-805**: Test all validation logic
- ⏳ **TASK-806**: Test all calculations

**Deliverable**: Comprehensive unit tests

### 8.2 Integration Testing
- ⏳ **TASK-807**: Test all API endpoints
- ⏳ **TASK-808**: Test database interactions
- ⏳ **TASK-809**: Test EasyPay integration (sandbox)
- ⏳ **TASK-810**: Test file uploads
- ⏳ **TASK-811**: Test PDF generation
- ⏳ **TASK-812**: Test Excel generation

**Deliverable**: Comprehensive integration tests

### 8.3 End-to-End Testing
- ⏳ **TASK-813**: Set up Playwright for E2E tests
- ⏳ **TASK-814**: E2E test: Complete registration flow
- ⏳ **TASK-815**: E2E test: Complete booking flow
- ⏳ **TASK-816**: E2E test: Complete payment flow
- ⏳ **TASK-817**: E2E test: Result access flow
- ⏳ **TASK-818**: E2E test: Admin workflows
- ⏳ **TASK-819**: Test on multiple browsers (Chrome, Firefox, Edge)
- ⏳ **TASK-820**: Test on mobile devices (iOS, Android)

**Deliverable**: Comprehensive E2E tests

### 8.4 Performance Testing
- ⏳ **TASK-821**: Set up JMeter or k6 for load testing
- ⏳ **TASK-822**: Define performance scenarios
- ⏳ **TASK-823**: Run load tests (1000+ concurrent users)
- ⏳ **TASK-824**: Run stress tests
- ⏳ **TASK-825**: Identify bottlenecks
- ⏳ **TASK-826**: Optimize database queries
- ⏳ **TASK-827**: Implement caching strategies
- ⏳ **TASK-828**: Achieve <3s page load time
- ⏳ **TASK-829**: Achieve <500ms API response time

**Deliverable**: Performance optimized application

### 8.5 Security Testing
- ⏳ **TASK-830**: Conduct penetration testing
- ⏳ **TASK-831**: Run vulnerability scanning tools
- ⏳ **TASK-832**: Test authentication and authorization
- ⏳ **TASK-833**: Test input validation
- ⏳ **TASK-834**: Test SQL injection prevention
- ⏳ **TASK-835**: Test XSS prevention
- ⏳ **TASK-836**: Review HTTPS configuration
- ⏳ **TASK-837**: Review secret management
- ⏳ **TASK-838**: Fix any identified vulnerabilities

**Deliverable**: Security hardened application

### 8.6 Accessibility Testing
- ⏳ **TASK-839**: Run automated accessibility testing tools
- ⏳ **TASK-840**: Manual keyboard navigation testing
- ⏳ **TASK-841**: Screen reader testing (NVDA, JAWS)
- ⏳ **TASK-842**: Color contrast verification
- ⏳ **TASK-843**: WCAG 2.1 AA compliance check
- ⏳ **TASK-844**: Fix any identified issues

**Deliverable**: WCAG 2.1 AA compliant application

### 8.7 User Acceptance Testing
- ⏳ **TASK-845**: Create UAT test scenarios
- ⏳ **TASK-846**: Recruit test users (students, staff, admin)
- ⏳ **TASK-847**: Conduct UAT sessions
- ⏳ **TASK-848**: Collect feedback
- ⏳ **TASK-849**: Fix identified issues
- ⏳ **TASK-850**: Re-test and sign-off

**Deliverable**: UAT sign-off

### 8.8 Documentation
- ⏳ **TASK-851**: Complete API documentation (Swagger)
- ⏳ **TASK-852**: Write deployment guide
- ⏳ **TASK-853**: Create student user manual
- ⏳ **TASK-854**: Create staff user manual
- ⏳ **TASK-855**: Create admin user manual
- ⏳ **TASK-856**: Record video tutorials
- ⏳ **TASK-857**: Update README files

**Deliverable**: Complete documentation

### 8.9 Production Preparation
- ⏳ **TASK-858**: Set up Azure App Service for production
- ⏳ **TASK-859**: Configure Azure SQL Database
- ⏳ **TASK-860**: Set up Azure Key Vault for secrets
- ⏳ **TASK-861**: Configure Azure CDN for static assets
- ⏳ **TASK-862**: Set up Azure Application Insights
- ⏳ **TASK-863**: Configure backup and disaster recovery
- ⏳ **TASK-864**: Set up custom domain and SSL certificate
- ⏳ **TASK-865**: Configure firewall rules

**Deliverable**: Production environment ready

### 8.10 Deployment
- ⏳ **TASK-866**: Final code review
- ⏳ **TASK-867**: Create deployment checklist
- ⏳ **TASK-868**: Deploy to production
- ⏳ **TASK-869**: Run smoke tests
- ⏳ **TASK-870**: Verify all functionality
- ⏳ **TASK-871**: Monitor for errors
- ⏳ **TASK-872**: Update DNS (if needed)
- ⏳ **TASK-873**: Announce go-live
- ⏳ **TASK-874**: Provide user support during launch

**Deliverable**: Live production system

### 8.11 Post-Deployment
- ⏳ **TASK-875**: Monitor application health (Application Insights)
- ⏳ **TASK-876**: Monitor performance metrics
- ⏳ **TASK-877**: Collect user feedback
- ⏳ **TASK-878**: Fix any production issues
- ⏳ **TASK-879**: Plan for future enhancements
- ⏳ **TASK-880**: Conduct retrospective

**Deliverable**: Stable production system with monitoring

---

## Summary

**Total Tasks**: 880  
**Estimated Duration**: 14 weeks (3.5 months)  
**Team Size**: 3-5 developers recommended  

### Task Distribution
- Phase 0: 35 tasks (Foundation)
- Phase 1: 100 tasks (Registration)
- Phase 2: 64 tasks (Booking)
- Phase 3: 70 tasks (Payment)
- Phase 4: 70 tasks (Results)
- Phase 5: 60 tasks (Venue)
- Phase 6: 87 tasks (Dashboards)
- Phase 7: 58 tasks (Landing Page)
- Phase 8: 80 tasks (Testing & Deployment)

---

## Next Actions

1. ✅ Review and approve task breakdown
2. ⏳ Assign tasks to team members
3. ⏳ Set up project management tool (GitHub Projects, Jira, etc.)
4. ⏳ Begin Phase 0: Foundation & Cleanup
5. ⏳ Schedule daily standups
6. ⏳ Plan sprint reviews

---

**Document Owner**: Development Team  
**Last Updated**: 2025-11-09  
**Status**: APPROVED - Ready for Execution

**END OF DOCUMENT**
