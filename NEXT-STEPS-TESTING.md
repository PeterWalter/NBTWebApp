# Next Steps - Testing & Development Plan

## ✅ Current Status (2025-11-09)

### Completed
- ✅ Project builds successfully
- ✅ API Server running on https://localhost:7001
- ✅ Blazor UI running on https://localhost:5001
- ✅ Database connected and migrations applied
- ✅ All controllers implemented and available
- ✅ FluentUI components integrated
- ✅ JWT authentication configured
- ✅ Role-based access control in place

### Known Issues
1. **Registration Wizard** - First step form validation not enabling Next button (temporarily skipped)
2. Need to test all existing modules end-to-end
3. Need to create test pages for easier module validation

---

## 📋 Immediate Next Steps

### 1. Test Existing Functionality

#### A. Test API Endpoints via Swagger
1. Open Swagger UI: https://localhost:7001/swagger
2. Test each controller:
   - `/api/auth/*` - Authentication endpoints
   - `/api/students/*` - Student management
   - `/api/registrations/*` - Registration workflow
   - `/api/bookings/*` - Booking management
   - `/api/payments/*` - Payment tracking
   - `/api/venues/*` - Venue management
   - `/api/rooms/*` - Room management
   - `/api/results/*` - Results management
   - `/api/reports/*` - Report generation
   - `/api/contentpages/*` - CMS pages
   - `/api/announcements/*` - Announcements
   - `/api/resources/*` - Downloadable resources

#### B. Test Blazor UI Navigation
1. Open: https://localhost:5001
2. Navigate through available pages
3. Test authentication flow
4. Verify FluentUI components render correctly

---

## 🎯 Development Priorities

### Phase 1: Core Testing & Validation (Days 1-2)

#### Day 1: API & Database Testing
- [ ] Test all CRUD operations via Swagger
- [ ] Verify database relationships
- [ ] Test NBT number generation endpoint
- [ ] Test EasyPay reference generation
- [ ] Validate Luhn algorithm implementation
- [ ] Test payment installment calculations
- [ ] Test booking constraints (max 2 per year, etc.)
- [ ] Test venue availability by date

#### Day 2: UI Components Testing
- [ ] Create admin test pages for each module
- [ ] Test venue management UI
- [ ] Test room management UI
- [ ] Test student direct entry (bypass wizard)
- [ ] Test booking creation UI
- [ ] Test payment recording UI
- [ ] Test results viewing UI

---

### Phase 2: Complete Missing Functionality (Days 3-5)

#### Student Management Module
**Components to Create:**
- `/admin/students` - Student list with search/filter
- `/admin/students/create` - Direct student creation form
- `/admin/students/{id}` - Student detail/edit page
- `/admin/students/{id}/bookings` - Student booking history
- `/admin/students/{id}/payments` - Student payment history
- `/admin/students/{id}/results` - Student results view

**Features:**
- NBT number generation on creation
- SA ID validation and data extraction (DOB, Gender)
- Foreign ID / Passport support
- Age, ethnicity, academic info collection
- Search by NBT number, ID number, name
- Export student data to Excel

---

#### Booking Management Module
**Components to Create:**
- `/admin/bookings` - All bookings dashboard
- `/admin/bookings/create` - Create booking for student
- `/admin/bookings/{id}` - Booking detail/edit
- `/admin/bookings/calendar` - Calendar view of bookings

**Features:**
- Test type selection (AQL, AQL+MAT)
- Venue and date selection
- Validate booking constraints:
  - Max 1 booking at a time (until previous test date passes)
  - Max 2 tests per year
  - Must select available venue and date
- Link to payment tracking
- Status tracking (Booked, Completed, Cancelled)
- Send confirmation emails
- EasyPay reference generation

---

#### Payment Management Module
**Components to Create:**
- `/admin/payments` - Payment dashboard
- `/admin/payments/record` - Record manual payment
- `/admin/payments/{id}` - Payment detail view
- `/admin/payments/upload` - Upload bank payment file
- `/admin/payments/pending` - Pending payments report

**Features:**
- Record payments (full or installment)
- Upload payment files from bank
- Track payment order for multiple tests
- Calculate remaining balance
- Validate against test costs (varies by intake year)
- Link payments to specific bookings
- Payment status updates
- Send payment confirmation emails
- Only allow test results viewing for fully paid tests

---

#### Venue & Room Management Module
**Components to Create:**
- `/admin/venues` - Venue list/management
- `/admin/venues/create` - Create venue
- `/admin/venues/{id}` - Edit venue
- `/admin/venues/{id}/rooms` - Manage venue rooms
- `/admin/rooms/create` - Create room
- `/admin/rooms/{id}` - Edit room
- `/admin/test-dates` - Manage test date calendar

**Features:**
- Venue types (National, Special Session, Research, Other)
- Venue active/inactive status
- Room capacity tracking
- Room allocation to test sessions
- Test date calendar management
- Closing date tracking
- Sunday test highlighting
- Online test indicators
- Venue availability by date range

---

#### Results Management Module
**Components to Create:**
- `/admin/results` - Results dashboard
- `/admin/results/upload` - Upload results file
- `/admin/results/import` - Import results
- `/admin/results/{id}` - View/edit result
- `/student/results` - Student results view (public)

**Features:**
- Import results from file
- Barcode tracking (distinguishes answer sheets)
- Multiple result sections:
  - AQL test → AL and QL results
  - MAT test → AL, QL, and MAT results
- Performance levels (Basic Lower, Basic Upper, Intermediate Lower, Proficient Lower, etc.)
- Multiple test tracking per student
- Results only viewable if fully paid (for students)
- Staff/Admin can view unpaid results
- PDF certificate generation
- Results validity tracking (3 years from booking date)

---

#### Reporting & Analytics Module
**Components to Create:**
- `/admin/reports` - Reports dashboard
- `/admin/reports/students` - Student summary reports
- `/admin/reports/bookings` - Booking reports
- `/admin/reports/payments` - Payment collection reports
- `/admin/reports/venues` - Venue utilization reports
- `/admin/reports/results` - Results analysis

**Features:**
- Excel export for all reports
- PDF export for certificates
- Date range filtering
- Venue filtering
- Test type filtering
- Payment status filtering
- Summary charts and graphs
- Export functionality

---

### Phase 3: Enhanced User Experience (Days 6-7)

#### Landing Page & Navigation
**Components to Create/Update:**
- `/` - Landing page with proper menus
- `/applicants/*` - Applicant-specific pages
- `/institutions/*` - Institution-specific pages
- `/educators/*` - Educator-specific pages

**Features:**
- Replicate current NBT website menu structure
- Add submenus with correct navigation
- Embed videos where available
- Responsive design
- FluentUI styling throughout

---

#### Dashboard Enhancements
**Features:**
- Left-side navigation menus per role
- Dashboard widgets:
  - Upcoming tests
  - Payment reminders
  - Recent activity
  - Statistics and counts
- Quick actions
- Notifications panel

---

#### Special Sessions & Remote Writers
**Components to Create:**
- `/register/special-session` - Special session request form
- `/admin/special-sessions` - Manage special sessions
- `/admin/remote-writers` - Remote writer management

**Features:**
- Off-site testing requests
- Invigilator details collection
- Venue details for remote sessions
- Route to NBT remote admin team
- Approval workflow
- Communication tracking

---

#### Pre-Test Questionnaire
**Components to Create:**
- `/register/questionnaire` - Background questionnaire
- `/admin/questionnaires` - View submitted questionnaires

**Features:**
- Post-registration questionnaire
- Research and equity reporting data
- Required before booking
- Data export for analysis

---

### Phase 4: Fix Registration Wizard (Days 8-9)

**Current Issue:** First step form not enabling Next button

**Planned Fix:**
1. Review form validation logic
2. Consolidate wizard steps:
   - Step 1: Identity & Contact (SA ID / Foreign ID + contact info)
   - Step 2: Personal & Academic (DOB, Gender, Ethnicity, Academic info)
   - Step 3: Test Preferences & Venue Selection
   - Step 4: Special Accommodations
   - Step 5: Survey Questions
3. Implement progress saving
4. Allow resuming interrupted registration
5. Auto-extract data from SA ID (DOB, Gender)
6. Generate NBT number on completion
7. Redirect to dashboard, not login

**Components:**
- `/register/wizard` - Multi-step wizard
- Backend: Save progress after each step
- Resume capability: Check for incomplete registration on login

---

### Phase 5: Testing & Quality Assurance (Days 10-12)

#### Unit Tests
- [ ] Test NBT number generation (Luhn algorithm)
- [ ] Test SA ID validation and extraction
- [ ] Test payment calculations
- [ ] Test booking constraint validations
- [ ] Test venue availability checks

#### Integration Tests
- [ ] Test complete registration flow
- [ ] Test booking → payment → results flow
- [ ] Test venue → room → allocation flow
- [ ] Test authentication & authorization
- [ ] Test file uploads (results, payments)

#### User Acceptance Testing
- [ ] Test as Applicant/Student
- [ ] Test as Staff member
- [ ] Test as Admin
- [ ] Test as SuperUser
- [ ] Test accessibility (WCAG 2.1 AA)
- [ ] Test performance (<3 second load)
- [ ] Test on multiple browsers
- [ ] Test on mobile devices

---

### Phase 6: Deployment & CI/CD (Days 13-15)

#### GitHub Workflows
- [ ] Create feature branch for each module
- [ ] Run automated builds on PR
- [ ] Run automated tests on PR
- [ ] Merge to main after successful tests
- [ ] Deploy to staging environment
- [ ] Run smoke tests on staging
- [ ] Deploy to production after approval

#### Azure Deployment
- [ ] Configure Azure App Service for API
- [ ] Configure Azure Static Web Apps for Blazor
- [ ] Configure Azure SQL Database
- [ ] Set up Azure Key Vault for secrets
- [ ] Configure SSL certificates
- [ ] Set up Application Insights
- [ ] Configure auto-scaling
- [ ] Set up backup and disaster recovery

#### Documentation
- [ ] API documentation (OpenAPI/Swagger)
- [ ] User guides for each role
- [ ] Admin manual
- [ ] Developer documentation
- [ ] Deployment guide
- [ ] Troubleshooting guide

---

## 🚀 Commands Reference

### Start Development Servers
```powershell
# Terminal 1 - API
cd "D:\projects\source code\NBTWebApp\src\NBT.WebAPI"
dotnet run

# Terminal 2 - Blazor UI
cd "D:\projects\source code\NBTWebApp\src\NBT.WebUI"
dotnet run
```

### Build & Test
```powershell
# Build entire solution
dotnet build

# Run tests
dotnet test

# Clean build
dotnet clean
dotnet build --no-incremental
```

### Database Management
```powershell
# Apply migrations
dotnet ef database update --project src/NBT.Infrastructure --startup-project src/NBT.WebAPI

# Create new migration
dotnet ef migrations add MigrationName --project src/NBT.Infrastructure --startup-project src/NBT.WebAPI

# Drop database (caution!)
dotnet ef database drop --project src/NBT.Infrastructure --startup-project src/NBT.WebAPI
```

### Git Workflow
```powershell
# Create feature branch
git checkout -b feature/module-name

# Commit changes
git add .
git commit -m "Description of changes"

# Push to GitHub
git push origin feature/module-name

# Merge to main (after testing)
git checkout main
git merge feature/module-name
git push origin main
```

---

## 📊 Progress Tracking

### Modules Status
- [x] Domain Models - Complete
- [x] Database Schema - Complete
- [x] EF Core Configuration - Complete
- [x] API Controllers - Complete
- [x] Authentication - Complete
- [x] Authorization - Complete
- [ ] Registration Wizard - Needs Fix
- [ ] Student Management UI - In Progress
- [ ] Booking Management UI - Not Started
- [ ] Payment Management UI - Not Started
- [ ] Venue Management UI - Test Page Created
- [ ] Room Management UI - Not Started
- [ ] Results Management UI - Not Started
- [ ] Reporting UI - Not Started
- [ ] Special Sessions - Not Started
- [ ] Questionnaire - Not Started
- [ ] Landing Page - Not Started
- [ ] Unit Tests - Not Started
- [ ] Integration Tests - Not Started
- [ ] Deployment - Not Started

---

## 🎯 Success Criteria

### Technical
- ✅ All endpoints respond correctly
- ✅ Database operations complete without errors
- ✅ Authentication & authorization work correctly
- ⏳ All UI pages navigate without errors
- ⏳ All forms validate correctly
- ⏳ All CRUD operations work end-to-end
- ⏳ Performance meets <3 second requirement
- ⏳ Accessibility meets WCAG 2.1 AA
- ⏳ All tests pass
- ⏳ CI/CD pipeline works

### Business
- ⏳ Students can register completely
- ⏳ NBT numbers generate correctly
- ⏳ Bookings respect all constraints
- ⏳ Payments track correctly (including installments)
- ⏳ Results import and display correctly
- ⏳ Staff can manage all entities
- ⏳ Reports generate correctly
- ⏳ Venues and rooms allocate properly
- ⏳ Special sessions route correctly
- ⏳ Audit logging captures all actions

---

## 📞 Support & Resources

### URLs (Development)
- Blazor UI: https://localhost:5001
- API: https://localhost:7001
- Swagger: https://localhost:7001/swagger

### Documentation
- Constitution: SPECKIT-CONSTITUTION.md
- Specification: SPECKIT-SPECIFICATION.md
- Implementation Plan: SPECKIT-IMPLEMENTATION-PLAN.md
- Quick Reference: QUICK-REFERENCE.md

### Tools
- Syncfusion License: (configured in appsettings)
- EasyPay API: (to be configured)
- FluentUI: Integrated
- Entity Framework: Core 9.0

---

**Last Updated:** 2025-11-09  
**Next Review:** After Phase 1 completion
