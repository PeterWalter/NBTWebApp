# NBT Registration Wizard - Quick Start Guide

## 🚀 Start the Application

### Option 1: One-Click Start (Recommended)
```powershell
cd "D:\projects\source code\NBTWebApp"
.\start-app.ps1
```

### Option 2: Manual Start
```powershell
# Terminal 1 - Web API
cd "D:\projects\source code\NBTWebApp\src\NBT.WebAPI"
dotnet run --urls "http://localhost:5000"

# Terminal 2 - Web UI  
cd "D:\projects\source code\NBTWebApp\src\NBT.WebUI"
dotnet run --urls "http://localhost:5001"
```

## 🌐 Access the Application

- **Home Page:** http://localhost:5001
- **Registration:** http://localhost:5001/register
- **Admin Login:** http://localhost:5001/login

## 👤 Test Student Registration

### Test Case 1: South African Student
1. Navigate to http://localhost:5001/register
2. **Step 1 - ID Verification:**
   - ID Type: South African ID
   - ID Number: 9912315678089 (example valid SA ID)
3. **Step 2 - Personal Info:**
   - First Name: Thabo
   - Last Name: Mbeki
   - Date of Birth: 15/12/1999
   - Gender: Male
4. **Step 3 - Contact:**
   - Email: thabo.mbeki@example.com
   - Phone: 0821234567
5. **Step 4 - Address:**
   - Address Line 1: 123 Main Street
   - City: Johannesburg
   - Province: Gauteng
   - Postal Code: 2000
   - Country: South Africa
6. **Step 5 - Academic:**
   - School Name: Johannesburg High School
   - School Province: Gauteng
   - Current Grade: 12
   - Home Language: English
7. **Step 6 - Accommodations:**
   - Skip (or check if special needs)
8. **Step 7 - Review:**
   - Review all details
   - Click "Finish" to submit
9. **Success:**
   - NBT Number displayed (14 digits)
   - Confirmation message shown

### Test Case 2: Foreign Student with Passport
1. Navigate to http://localhost:5001/register
2. **Step 1 - ID Verification:**
   - ID Type: Passport
   - ID Number: A12345678
   - Nationality: Zimbabwe
   - Country of Origin: Zimbabwe
3. Complete remaining steps as above
4. **Success:** NBT Number generated for foreign student

### Test Case 3: Duplicate ID Check
1. Register a student with ID: 9912315678089
2. Try to register again with the same ID
3. **Expected:** Error message "This ID number is already registered"

## 🔐 Admin Access

### Login Credentials
- **Email:** admin@nbt.ac.za
- **Password:** Admin@123

### Admin Features
- View all registered students
- Manage bookings and payments
- Access reports and analytics
- Configure system settings

## 📊 Check Registration Data

### Using SQL Server Management Studio
```sql
-- View all registered students
SELECT 
    StudentId,
    FirstName,
    LastName,
    IDType,
    IDNumber,
    NBTNumber,
    Email,
    CreatedDate
FROM Students
ORDER BY CreatedDate DESC;

-- Count registrations by ID type
SELECT 
    IDType,
    COUNT(*) as Count
FROM Students
GROUP BY IDType;
```

### Using API Endpoint
```powershell
# Get all students (requires admin token)
Invoke-RestMethod -Uri "http://localhost:5000/api/students" -Method GET
```

## 🧪 Run Tests

```powershell
cd "D:\projects\source code\NBTWebApp"
dotnet test
```

## 🔄 Update Database

```powershell
cd "D:\projects\source code\NBTWebApp\src\NBT.Infrastructure"
dotnet ef database update --startup-project ../NBT.WebAPI
```

## 🛑 Stop the Application

```powershell
# Kill processes on ports 5000 and 5001
Get-NetTCPConnection -LocalPort 5000 | Stop-Process -Force
Get-NetTCPConnection -LocalPort 5001 | Stop-Process -Force
```

## 📝 Key Features

✅ **7-Step Wizard:** Intuitive multi-step registration process  
✅ **ID Type Support:** SA_ID, FOREIGN_ID, PASSPORT  
✅ **Real-Time Validation:** Instant feedback on ID numbers  
✅ **NBT Number Generation:** Automatic 14-digit unique identifier  
✅ **Duplicate Prevention:** Checks for existing registrations  
✅ **Fluent UI:** Modern, responsive design  
✅ **Special Accommodations:** Support for students with special needs  
✅ **Email Confirmation:** Automated notification system  

## 🐛 Troubleshooting

### Port Already in Use
```powershell
# Kill existing processes
.\start-app.ps1  # This script handles port cleanup automatically
```

### Database Connection Error
```powershell
# Check SQL Server is running
# Update connection string in appsettings.Development.json
# Run database update
dotnet ef database update --startup-project src/NBT.WebAPI --project src/NBT.Infrastructure
```

### Build Errors
```powershell
# Clean and rebuild
dotnet clean
dotnet build
```

## 📖 Documentation

- **Full Guide:** REGISTRATION-WIZARD-DEPLOYMENT-COMPLETE.md
- **Developer Reference:** DEVELOPER-QUICK-REFERENCE.md
- **How to Run:** HOW-TO-RUN.md
- **Project Status:** PROJECT-STATUS.md

## 🎯 Next Steps

1. **Test the registration wizard** thoroughly
2. **Verify email notifications** are sent
3. **Check database** for registered students
4. **Test different ID types** (SA_ID, FOREIGN_ID, PASSPORT)
5. **Proceed to Booking Module** implementation

---

**Quick Links:**
- Registration: http://localhost:5001/register
- Admin Login: http://localhost:5001/login
- API Docs: http://localhost:5000/swagger (if enabled)

**Need Help?** Check REGISTRATION-WIZARD-DEPLOYMENT-COMPLETE.md for detailed information.
