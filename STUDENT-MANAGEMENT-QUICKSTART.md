# Student Management Module - Quick Start Guide

## Access the Module

### URL Routes
- **List Students**: `https://localhost:7001/admin/students`
- **View Student**: `https://localhost:7001/admin/students/{id}`
- **Create Student**: `https://localhost:7001/admin/students/create`
- **Edit Student**: `https://localhost:7001/admin/students/edit/{id}`

### Required Role
You must be logged in as: **Admin**, **Staff**, or **SuperUser**

## Quick Actions

### 1. View All Students
1. Navigate to `/admin/students`
2. Use search box to filter by name, NBT number, ID number, or email
3. Click refresh button to reload data
4. Click action icons to view, edit, or deactivate students

### 2. Create New Student

#### Option A: South African Student
1. Click "Create New Student" button
2. Select ID Type: "South African ID"
3. Enter 13-digit SA ID number (e.g., 0101015800080)
4. **Magic happens**: Date of Birth and Gender auto-fill!
5. Fill remaining required fields:
   - First Name
   - Last Name
   - Email
   - Phone Number
   - School Name
6. Click "Create Student"
7. **Success**: NBT Number is auto-generated and displayed!

#### Option B: Foreign/Passport Student
1. Click "Create New Student" button
2. Select ID Type: "Foreign ID" or "Passport"
3. Enter ID/Passport number
4. Manually enter Date of Birth and Gender
5. Fill Nationality and Country of Origin
6. Complete other required fields
7. Click "Create Student"
8. **Success**: NBT Number is auto-generated!

### 3. View Student Details
1. From student list, click the eye icon
2. View complete student profile including:
   - Personal information
   - Contact details
   - Address
   - Academic information
   - Special accommodations
   - Audit trail
3. Use quick action buttons to view related:
   - Bookings
   - Payments
   - Results

### 4. Edit Student
1. From student details, click "Edit" button
2. Update allowed fields (Name, Contact, Address, School info)
3. Note: NBT Number and ID Number cannot be changed
4. Toggle Active/Inactive status if needed
5. Click "Save Changes"

### 5. Deactivate Student
1. From student list, click delete icon
2. Confirm deactivation
3. Student is soft-deleted (IsActive = false)
4. Student can be reactivated via Edit page

## Search Examples

### Search by NBT Number
```
14202511090001
```

### Search by Name
```
John
Smith
John Smith
```

### Search by Email
```
john@example.com
@example.com
```

### Search by ID Number
```
0101015800080
```

## SA ID Magic ✨

### How it Works
When you enter a 13-digit SA ID, the system:
1. Validates format (must be exactly 13 digits)
2. Extracts birth date from first 6 digits (YYMMDD)
3. Determines century (1900s or 2000s)
4. Extracts gender from 7th digit:
   - 0-4 = Female
   - 5-9 = Male
5. Auto-fills and locks Date of Birth field
6. Auto-fills and locks Gender field

### Example SA ID
**ID**: `0101015800080`
- **DOB**: 2001-01-01 (born January 1, 2001)
- **Gender**: Male (digit 5)

## NBT Number Format

Auto-generated 14-digit number:
- **Year**: 2025
- **Month**: 11
- **Day**: 09
- **Sequence**: 00001
- **Check Digit**: X (Luhn algorithm)

Example: `20251109000013`

## API Endpoints

### Get All Students
```http
GET /api/students?page=1&pageSize=50
Authorization: Bearer {token}
```

### Get Student by ID
```http
GET /api/students/{id}
Authorization: Bearer {token}
```

### Create Student
```http
POST /api/students
Content-Type: application/json

{
  "firstName": "John",
  "lastName": "Smith",
  "idType": "SA_ID",
  "idNumber": "0101015800080",
  "dateOfBirth": "2001-01-01",
  "gender": "Male",
  "email": "john@example.com",
  "phoneNumber": "0821234567",
  "schoolName": "Test High School"
}
```

### Update Student
```http
PUT /api/students/{id}
Authorization: Bearer {token}
Content-Type: application/json

{
  "id": "guid-here",
  "firstName": "John",
  "lastName": "Smith",
  "email": "newemail@example.com",
  ...
}
```

### Deactivate Student
```http
DELETE /api/students/{id}
Authorization: Bearer {token}
```

### Search Students
```http
GET /api/students/search?searchTerm=John&page=1&pageSize=50
Authorization: Bearer {token}
```

### Check Duplicate
```http
GET /api/students/check-duplicate?idNumber=0101015800080&idType=SA_ID
```

## Testing Checklist

### ✅ Basic Operations
- [ ] View student list
- [ ] Search students
- [ ] Create SA student (with auto DOB/Gender)
- [ ] Create foreign student
- [ ] View student details
- [ ] Edit student
- [ ] Deactivate student

### ✅ SA ID Features
- [ ] Enter 13-digit SA ID
- [ ] Verify DOB auto-fills correctly
- [ ] Verify Gender auto-fills correctly
- [ ] Verify fields are disabled after auto-fill
- [ ] Try invalid SA ID (should not auto-fill)

### ✅ Validation
- [ ] Submit form with missing required fields
- [ ] Enter invalid email format
- [ ] Enter invalid phone format
- [ ] Try duplicate ID number

### ✅ UI/UX
- [ ] Search box filters correctly
- [ ] Data grid sorts by column
- [ ] Status badges show correct colors
- [ ] Loading indicators display
- [ ] Success dialog shows NBT number
- [ ] Error messages display clearly
- [ ] Responsive layout on mobile

### ✅ Authorization
- [ ] Access requires login
- [ ] Only Admin/Staff/SuperUser can access
- [ ] Anonymous cannot access (except create endpoint)

## Common Issues & Solutions

### Issue: SA ID doesn't auto-fill
**Solution**: Ensure ID is exactly 13 digits and contains only numbers

### Issue: Can't create student
**Solution**: Check all required fields are filled:
- First Name
- Last Name
- ID Type
- ID Number
- Date of Birth
- Gender
- Email
- Phone Number
- School Name

### Issue: "Duplicate ID" error
**Solution**: Student with this ID already exists. Use search to find existing record.

### Issue: Can't edit NBT Number
**Solution**: This is by design. NBT numbers are immutable once generated.

### Issue: Can't access module
**Solution**: Ensure you're logged in with Admin, Staff, or SuperUser role.

## Next Steps

After Student Management is working:

1. **Test Integration**
   - Verify students appear correctly
   - Test all CRUD operations
   - Validate search and filtering

2. **Move to Next Module**
   - Booking Management (link students to test bookings)
   - Payment Management (track student payments)
   - Results Management (store and display test results)

3. **Production Readiness**
   - Performance testing with large datasets
   - Security audit
   - User acceptance testing
   - Documentation review

## Support & Resources

- **Full Documentation**: `STUDENT-MANAGEMENT-MODULE.md`
- **API Documentation**: Swagger UI at `https://localhost:7001/swagger`
- **Architecture**: `CONSTITUTION.md`
- **Project Status**: `IMPLEMENTATION-STATUS.md`

---

**Module Status**: ✅ COMPLETE
**Last Updated**: November 9, 2025
**Ready for**: Integration Testing & Next Module Development
