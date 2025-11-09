# NBT Web Application - Development Continuation Status

**Date**: November 9, 2025  
**Branch**: `feature/comprehensive-nbt-implementation`  
**Status**: JSON Serialization Fix Applied ✅

---

## Latest Changes Applied

### 1. JSON Serialization Configuration (COMPLETED ✅)

#### WebAPI Program.cs
- ✅ Added `System.Text.Json` and `System.Text.Json.Serialization` usings
- ✅ Configured JSON options on `AddControllers()`:
  - CamelCase property naming policy
  - Ignore null values
  - JSON string enum converter
  - Reference handler to ignore cycles
  - Indented formatting in development

#### WebUI Program.cs
- ✅ Added JSON serialization configuration
- ✅ Configured global JSON options for HttpClient services
- ✅ Applied consistent serialization settings

#### Build Status
- ✅ Solution builds successfully (15.7s)
- ✅ All projects compile without errors
- ✅ Changes committed to Git (commit: e601f67)
- ✅ Changes pushed to GitHub

---

## Current Application State

### Running Services
- **WebAPI**: Running on https://localhost:7001 and http://localhost:7000
- **WebUI**: Running on https://localhost:5001 and http://localhost:5000
- **Database**: Seeded and migrations applied
- **Authentication**: JWT configured with Admin, Staff, SuperUser roles

### Architecture Components
✅ **Domain Layer** - Complete with all entities
✅ **Application Layer** - Services and DTOs defined
✅ **Infrastructure Layer** - EF Core, Identity, and repositories
✅ **WebAPI** - RESTful endpoints with Swagger
✅ **WebUI** - Blazor Server with Fluent UI

---

## Requirements Summary (From User Input)

### Core Business Rules

#### Student/Applicant Activities
1. **Account Creation & Login**
   - Register with SA ID, Foreign ID, or Passport
   - OTP verification and duplicate prevention
   - Profile management with audit logging

2. **NBT Number Generation**
   - 14-digit number using Luhn algorithm (modulus-10)
   - Generated upon successful registration
   - Links all bookings, payments, and results

3. **Registration Wizard** (NEEDS ENHANCEMENT ⚠️)
   - Multi-step process with auto-save
   - Personal info: Age, Gender, Ethnicity, DOB
   - If SA ID provided: Extract DOB and Gender automatically
   - Academic and contact details
   - Test preferences and venue selection
   - Special accommodation requests
   - **ISSUE**: Currently skips steps and goes straight to "Done"
   - **REQUIREMENT**: Combine Steps 1+2 into one, Steps 3+4 into one
   - **REQUIREMENT**: Add survey questions as final step
   - **REQUIREMENT**: Resume interrupted registrations

4. **Booking & Payment**
   - Test types: AQL (AL + QL) or AQL and MAT
   - Bookings open from April 1st (Intake Year start)
   - **One test at a time** - can only book another after closing date passes
   - Maximum 2 tests per year
   - Tests valid for 3 years from booking date
   - Can change booking before closing date
   - **Payment in installments** until complete
   - **Payment order**: In order of tests being written
   - **Cost variation**: By intake year
   - EasyPay payment reference generation
   - Bank payment file uploads (specific format)

5. **Special/Remote Sessions**
   - Off-site testing form with invigilator details
   - Routed to NBT remote administration team

6. **Pre-Test Questionnaire**
   - Online background questionnaire after registration
   - For research and equity reporting

7. **Results Access**
   - **AQL Test Results**: AL (Academic Literacy) + QL (Quantitative Literacy)
   - **MAT Test Results**: AL + QL + MAT (Mathematics)
   - Each domain has performance levels (Basic Lower, Basic Upper, Intermediate Lower, Proficient Lower, etc.)
   - **Barcode per test** - differentiates actual answer sheet used
   - Multiple tests have different barcodes
   - **Only fully paid tests** can be viewed/downloaded by students
   - PDF certificate download for students
   - Staff/Admin can view all results regardless of payment status

8. **Notifications**
   - Email/SMS alerts for registration, payment, test reminders, results

#### Venue Management

1. **Venue Types**
   - National
   - Special Session
   - Research
   - Other (to be decided)

2. **Venue Availability**
   - Venues may not be available for certain test dates
   - Availability tracked during the year

3. **Test Calendar**
   - Table of test dates with closing booking dates
   - Tests highlighted if written on Sunday
   - Tests highlighted if Online Test
   - **Online Tests**: Written from anywhere with computer, video, sound, internet
   - Online tests have specific dates on calendar

4. **Room Management**
   - **ISSUE**: Test sessions linked to Venue, not Room (needs clarification)
   - Room capacity tracking

#### User Interface

1. **Landing Page**
   - Menus: Applicants, Institutions, Educators
   - Submenus based on current NBT website
   - Video content where available

2. **Dashboards**
   - After login, users directed to role-appropriate dashboards
   - Left-side menus for navigation
   - Staff/Admin: CRUD operations for applicants, payments, results, venues
   - Students: Registration status, bookings, results, profile

---

## Identified Issues & Next Steps

### High Priority 🔴

#### 1. Registration Wizard Issues (CRITICAL)
**Problem**: Wizard skips directly to "Done" without showing form steps
**Required Actions**:
- [ ] Combine Step 1 + Step 2 into single step
- [ ] Combine Step 3 + Step 4 into single step
- [ ] Add Age, Gender, Ethnicity fields to personal information
- [ ] Implement SA ID parsing for DOB and Gender extraction
- [ ] Add survey questions as final wizard step
- [ ] Implement session state persistence for interrupted registrations
- [ ] Fix navigation flow between steps
- [ ] Ensure NBT number generation happens only at final submission

#### 2. Payment Module Enhancement
**Required Actions**:
- [ ] Implement installment payment tracking
- [ ] Add payment ordering logic (by test write order)
- [ ] Implement cost variation by intake year
- [ ] Add bank payment file upload functionality
- [ ] Implement payment completion validation for result viewing
- [ ] Create payment dashboard for students

#### 3. Results Module Enhancement
**Required Actions**:
- [ ] Add barcode field to Result entity
- [ ] Implement performance level enumeration
- [ ] Add domain-specific results (AL, QL, MAT)
- [ ] Implement PDF certificate generation
- [ ] Add payment status check for student result access
- [ ] Create staff/admin result viewing (unrestricted)

### Medium Priority 🟡

#### 4. Venue & Calendar Management
**Required Actions**:
- [ ] Add venue type enumeration (National, Special Session, Research, Other)
- [ ] Implement venue availability tracking
- [ ] Create test calendar with dates and closing dates
- [ ] Add Sunday test highlighting
- [ ] Add online test indicators
- [ ] Clarify Room vs Venue linkage for test sessions

#### 5. Landing Page & Navigation
**Required Actions**:
- [ ] Create landing page menus (Applicants, Institutions, Educators)
- [ ] Implement submenus from current NBT website
- [ ] Add video content integration
- [ ] Implement role-based dashboard routing

#### 6. Dashboard Enhancements
**Required Actions**:
- [ ] Design left-side menu layouts
- [ ] Implement staff/admin CRUD interfaces
- [ ] Create student progress tracking dashboard
- [ ] Add booking management interface

### Low Priority 🟢

#### 7. DTO JSON Attributes
**Note**: Application works with current JSON configuration, but for explicit control:
- [ ] Add `[JsonPropertyName]` attributes to 20 DTOs (optional enhancement)
- [ ] Add `using System.Text.Json.Serialization;` to DTO files

---

## Testing Requirements

### Functional Testing
- [ ] Test registration wizard flow end-to-end
- [ ] Test SA ID extraction for DOB and Gender
- [ ] Test foreign ID/passport registration
- [ ] Test session resumption after interruption
- [ ] Test booking constraints (one at a time, max 2 per year)
- [ ] Test payment installments
- [ ] Test result access based on payment status
- [ ] Test venue availability filtering
- [ ] Test online test calendar highlighting

### Integration Testing
- [ ] API to UI data flow
- [ ] EasyPay integration
- [ ] Bank file upload processing
- [ ] PDF certificate generation
- [ ] Email/SMS notification triggers

### User Acceptance Testing
- [ ] Student registration journey
- [ ] Staff CRUD operations
- [ ] Admin reporting and analytics
- [ ] Payment processing flow

---

## Development Commands

### Quick Start
```powershell
# Start both services
cd "D:\projects\source code\NBTWebApp\src\NBT.WebAPI"
dotnet run  # Runs on https://localhost:7001

cd "D:\projects\source code\NBTWebApp\src\NBT.WebUI"
dotnet run  # Runs on https://localhost:5001
```

### Apply JSON Fix Anytime
```powershell
cd "D:\projects\source code\NBTWebApp"
.\APPLY-JSON-FIX.ps1
```

### Build & Test
```powershell
dotnet build --no-incremental
dotnet test
```

### Git Workflow
```powershell
# Current branch
git status

# Commit changes
git add -A
git commit -m "Description of changes"

# Push to GitHub
git push origin feature/comprehensive-nbt-implementation

# When phase complete, merge to main
git checkout main
git merge feature/comprehensive-nbt-implementation
git push origin main
```

---

## Recommended Development Phases

### Phase 1: Fix Registration Wizard (IMMEDIATE)
1. Analyze current wizard implementation
2. Restructure step components
3. Add missing fields (Age, Gender, Ethnicity, Survey)
4. Implement SA ID parsing
5. Add session state persistence
6. Test complete registration flow

### Phase 2: Payment Enhancement
1. Add installment tracking
2. Implement payment ordering
3. Add cost variation logic
4. Create bank file upload
5. Integrate payment status with results access

### Phase 3: Results Module
1. Add barcode functionality
2. Implement performance levels
3. Create PDF certificate generator
4. Implement access control based on payment
5. Build staff/admin result viewing

### Phase 4: Venue & Calendar
1. Add venue types and availability
2. Create test calendar interface
3. Implement highlighting for Sunday/Online tests
4. Resolve Room vs Venue linkage

### Phase 5: Landing Page & Dashboards
1. Build landing page structure
2. Implement menu navigation
3. Add video integration
4. Create role-based dashboards

### Phase 6: Testing & Refinement
1. Comprehensive functional testing
2. Integration testing
3. Performance optimization
4. UAT with stakeholders

---

## Notes for Continuation

### JSON Configuration Already Applied ✅
The JSON serialization fix has been fully applied and is working. The configuration ensures:
- Property names are camelCase in JSON
- Null values are ignored
- Enums serialize as strings
- Circular references are handled
- Output is formatted in development mode

### Current Session Management
- WebAPI session: `webapi` (running)
- WebUI session: `webui` (running)
- Main development session: `main`

### Git Status
- Clean working tree
- Latest commit pushed to GitHub
- Branch: `feature/comprehensive-nbt-implementation`

---

## Quick Reference URLs

- **WebUI**: https://localhost:5001
- **WebAPI**: https://localhost:7001
- **Swagger**: https://localhost:7001/swagger
- **Health Check**: https://localhost:7001/health

---

## Contact & Support

For any questions or clarifications on requirements, refer to:
- CONSTITUTION.md - Core principles and standards
- SPECKIT-SPECIFICATION.md - Detailed specifications
- IMPLEMENTATION-PLAN.md - Implementation roadmap
- Database schema documentation in `database-scripts/`

---

**Status**: ✅ JSON Fix Complete | ⚠️ Registration Wizard Needs Attention  
**Next Action**: Fix Registration Wizard Flow (Phase 1)
