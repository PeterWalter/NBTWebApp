# Testable Modules - NBT Web Application

## APIs Available for Testing (All Running on https://localhost:7001)

### 1. ✅ Authentication Module (`/api/auth`)
- POST `/api/auth/register` - User registration
- POST `/api/auth/login` - User login
- POST `/api/auth/refresh-token` - Token refresh

### 2. ✅ Students Module (`/api/students`)
- GET `/api/students` - List all students
- GET `/api/students/{id}` - Get student by ID
- POST `/api/students` - Create new student
- PUT `/api/students/{id}` - Update student
- DELETE `/api/students/{id}` - Delete student
- GET `/api/students/by-nbt-number/{nbtNumber}` - Get by NBT number
- GET `/api/students/by-id-number/{idNumber}` - Get by ID number

### 3. ✅ Registrations Module (`/api/registrations`)
- GET `/api/registrations` - List all registrations
- GET `/api/registrations/{id}` - Get registration by ID
- POST `/api/registrations` - Create new registration
- PUT `/api/registrations/{id}` - Update registration
- DELETE `/api/registrations/{id}` - Delete registration
- POST `/api/registrations/generate-nbt-number` - Generate NBT number
- GET `/api/registrations/by-student/{studentId}` - Get by student

### 4. ✅ Bookings Module (`/api/bookings`)
- GET `/api/bookings` - List all bookings
- GET `/api/bookings/{id}` - Get booking by ID
- POST `/api/bookings` - Create new booking
- PUT `/api/bookings/{id}` - Update booking
- DELETE `/api/bookings/{id}` - Delete booking
- GET `/api/bookings/by-student/{studentId}` - Get student bookings
- GET `/api/bookings/by-venue/{venueId}` - Get venue bookings

### 5. ✅ Payments Module (`/api/payments`)
- GET `/api/payments` - List all payments
- GET `/api/payments/{id}` - Get payment by ID
- POST `/api/payments` - Create new payment
- PUT `/api/payments/{id}` - Update payment
- DELETE `/api/payments/{id}` - Delete payment
- GET `/api/payments/by-student/{studentId}` - Get student payments
- POST `/api/payments/process-easypay` - Process EasyPay payment

### 6. ✅ Venues Module (`/api/venues`)
- GET `/api/venues` - List all venues
- GET `/api/venues/{id}` - Get venue by ID
- POST `/api/venues` - Create new venue
- PUT `/api/venues/{id}` - Update venue
- DELETE `/api/venues/{id}` - Delete venue
- GET `/api/venues/by-type/{type}` - Get venues by type (National, Special, Research)
- GET `/api/venues/available` - Get available venues

### 7. ✅ Rooms Module (`/api/rooms`)
- GET `/api/rooms` - List all rooms
- GET `/api/rooms/{id}` - Get room by ID
- POST `/api/rooms` - Create new room
- PUT `/api/rooms/{id}` - Update room
- DELETE `/api/rooms/{id}` - Delete room
- GET `/api/rooms/by-venue/{venueId}` - Get rooms by venue
- GET `/api/rooms/available/{venueId}` - Get available rooms for venue

### 8. ✅ Reports Module (`/api/reports`)
- GET `/api/reports/students` - Student report
- GET `/api/reports/bookings` - Bookings report
- GET `/api/reports/payments` - Payments report
- GET `/api/reports/venues` - Venues report
- POST `/api/reports/export-excel` - Export to Excel
- POST `/api/reports/export-pdf` - Export to PDF

### 9. ✅ System Settings Module (`/api/systemsettings`)
- GET `/api/systemsettings` - List all settings
- GET `/api/systemsettings/{key}` - Get setting by key
- PUT `/api/systemsettings/{key}` - Update setting

### 10. ✅ Content Management (`/api/contentpages`, `/api/announcements`, `/api/resources`)
- Full CRUD operations for website content

---

## Modules to Test NOW (Skipping Registration Wizard)

### Priority 1: Student Management Dashboard
**Purpose:** Test CRUD operations for student records

**Test Steps:**
1. Create a simple admin page that lists students
2. Test creating a new student (manual entry without wizard)
3. Test viewing student details
4. Test updating student information
5. Test searching students by NBT number or ID number

**API Endpoints to Use:**
- `GET /api/students`
- `POST /api/students`
- `PUT /api/students/{id}`
- `GET /api/students/by-nbt-number/{nbtNumber}`

---

### Priority 2: Venue Management
**Purpose:** Test venue and room administration

**Test Steps:**
1. Create venue management page
2. Test creating venues (National, Special, Research)
3. Test adding rooms to venues with capacity
4. Test marking venues as available/unavailable for dates
5. Test viewing venue occupancy

**API Endpoints to Use:**
- `GET /api/venues`
- `POST /api/venues`
- `GET /api/rooms/by-venue/{venueId}`
- `POST /api/rooms`

---

### Priority 3: Booking Management
**Purpose:** Test test booking workflow

**Test Steps:**
1. Create booking dashboard for staff
2. Test manually creating a booking for an existing student
3. Test viewing all bookings
4. Test filtering bookings by venue, date, status
5. Test updating booking status

**API Endpoints to Use:**
- `GET /api/bookings`
- `POST /api/bookings`
- `PUT /api/bookings/{id}`
- `GET /api/bookings/by-venue/{venueId}`

---

### Priority 4: Payment Tracking
**Purpose:** Test payment recording and tracking

**Test Steps:**
1. Create payment dashboard
2. Test recording manual payments
3. Test installment payments
4. Test viewing payment history per student
5. Test payment status updates (Pending, Complete, Partial)

**API Endpoints to Use:**
- `GET /api/payments`
- `POST /api/payments`
- `GET /api/payments/by-student/{studentId}`
- `PUT /api/payments/{id}`

---

### Priority 5: Reports & Analytics
**Purpose:** Test reporting capabilities

**Test Steps:**
1. Create reports dashboard
2. Test student summary report
3. Test bookings by venue report
4. Test payment collection report
5. Test Excel export functionality
6. Test PDF export functionality

**API Endpoints to Use:**
- `GET /api/reports/students`
- `GET /api/reports/bookings`
- `GET /api/reports/payments`
- `POST /api/reports/export-excel`
- `POST /api/reports/export-pdf`

---

## Testing Strategy

### Phase 1: API Testing (Using Swagger/Postman)
1. Open https://localhost:7001/swagger
2. Test each endpoint with sample data
3. Verify responses and error handling
4. Document working endpoints

### Phase 2: Frontend Pages (Simple CRUD)
1. Create basic admin pages using FluentUI
2. Connect to APIs using HttpClient
3. Implement data tables with filtering
4. Add create/edit forms
5. Test end-to-end workflows

### Phase 3: Integration Testing
1. Test complete workflows:
   - Student → Booking → Payment → Results
   - Venue → Room → Capacity → Allocation
2. Test data validation
3. Test error handling
4. Test concurrent operations

---

## Next Steps

1. **Start API now** (✅ DONE - Running on port 7001)
2. **Open Swagger UI** - https://localhost:7001/swagger
3. **Test endpoints manually** - Verify each controller works
4. **Create admin dashboard pages** - Skip registration wizard
5. **Test venue management** - First complete module
6. **Test booking system** - Second complete module
7. **Return to registration wizard** - After other modules work

---

## Notes

- API is running and healthy
- Database is connected and seeded
- All controllers are available
- Swagger UI can be used for manual testing
- Focus on modules that don't depend on registration wizard
- Registration wizard can be fixed once other parts are stable

