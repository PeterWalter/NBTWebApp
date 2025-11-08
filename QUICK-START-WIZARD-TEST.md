# Quick Start: Test Updated Registration Wizard

## 🚀 Quick Start (5 Minutes)

### Step 1: Start the Application
```powershell
# Terminal 1 - Start API
cd "D:\projects\source code\NBTWebApp\src\NBT.WebAPI"
dotnet run

# Terminal 2 - Start Web UI
cd "D:\projects\source code\NBTWebApp\src\NBT.WebUI"
dotnet run
```

### Step 2: Navigate to Registration
Open browser: `https://localhost:5001/register`

### Step 3: Test SA ID Auto-Extraction

#### Test Case 1: Male SA ID
1. Select ID Type: "South African ID"
2. Enter ID Number: `9801015800089`
3. ✅ Watch DOB auto-fill: 1998-01-01
4. ✅ Watch Gender auto-select: Male
5. ✅ Fields become disabled

#### Test Case 2: Female SA ID
1. Select ID Type: "South African ID"
2. Enter ID Number: `0512200234088`
3. ✅ Watch DOB auto-fill: 2005-12-20
4. ✅ Watch Gender auto-select: Female
5. ✅ Fields become disabled

### Step 4: Complete Registration
1. **Step 1:** Fill in remaining personal & contact info
   - First Name: Test
   - Last Name: User
   - Ethnicity: Select one
   - Email: test@example.com
   - Phone: 0123456789

2. **Step 2:** Address (optional fields)
   - Skip or fill as desired

3. **Step 3:** Academic & Survey
   - School Name: Test High School
   - Fill survey questions (optional)

4. **Step 4:** Accommodations
   - Skip if not needed

5. **Step 5:** Review & Submit
   - Review all info
   - Click "Submit Registration"
   - ✅ NBT Number generated
   - ✅ Success message displayed

## 🧪 Test Scenarios

### ✅ Scenario 1: SA ID Registration
- ID Type: SA_ID
- ID: Valid 13-digit SA ID
- Expected: DOB and Gender auto-filled
- Time: ~3 minutes

### ✅ Scenario 2: Foreign ID Registration
- ID Type: FOREIGN_ID
- ID: ABC123456
- Nationality: Nigeria
- Country: Nigeria
- Expected: Manual DOB and Gender entry
- Time: ~4 minutes

### ✅ Scenario 3: Passport Registration
- ID Type: PASSPORT
- ID: P1234567
- Nationality: Kenya
- Country: Kenya
- Expected: Manual DOB and Gender entry
- Time: ~4 minutes

### ✅ Scenario 4: Invalid SA ID
- ID: 123 (too short)
- Expected: Validation error message
- Time: ~30 seconds

### ✅ Scenario 5: Duplicate ID
- ID: Previously registered ID
- Expected: Duplicate error message
- Time: ~30 seconds

## 🔍 What to Verify

### Visual Checks
- [ ] DOB field disabled when SA ID entered
- [ ] Gender field disabled when SA ID entered
- [ ] Correct DOB extracted from SA ID
- [ ] Correct Gender extracted from SA ID
- [ ] Progress indicator shows 5 steps (not 6)
- [ ] All validation messages display correctly
- [ ] Success screen shows NBT number

### Functional Checks
- [ ] Can complete all 5 steps
- [ ] Can navigate back and forth
- [ ] Can change ID type after selection
- [ ] Fields re-enable when switching from SA ID to other types
- [ ] Survey data is optional
- [ ] Address fields are optional
- [ ] Email validation works
- [ ] Phone validation works
- [ ] NBT number generated successfully

### Data Checks
- [ ] All form data submitted correctly
- [ ] Survey responses saved
- [ ] Ethnicity saved
- [ ] DOB format correct in database
- [ ] Gender value correct in database

## 📊 Test Data

### Valid SA IDs
```
9801015800089  # Male, DOB: 1998-01-01
0512200234088  # Female, DOB: 2005-12-20
7503035123456  # Male, DOB: 1975-03-03
8906154321087  # Female, DOB: 1989-06-15
```

### Foreign IDs
```
ABC123456789   # Generic foreign ID
P1234567890    # Passport format
FID987654321   # Another format
```

### Test Emails
```
test1@example.com
test2@example.com
student@test.com
```

### Test Phones
```
0123456789
0827654321
0715551234
```

## 🐛 Troubleshooting

### Issue: DOB not auto-filling
**Fix:** Ensure SA ID is exactly 13 digits and all numeric

### Issue: Build errors
**Fix:** Run `dotnet build` to see specific errors

### Issue: Cannot start app
**Fix:** Check if ports 5000/5001 are already in use

### Issue: Database errors
**Fix:** Run migrations: `dotnet ef database update`

### Issue: Validation always fails
**Fix:** Check that all required fields (*) are filled

## 📝 Verification Script

Run the automated test:
```powershell
cd "D:\projects\source code\NBTWebApp"
.\test-wizard-improvements.ps1
```

Expected output:
```
✓ Combined steps (6 steps → 5 steps)
✓ Automatic DOB extraction from SA ID
✓ Automatic Gender extraction from SA ID
✓ Disabled fields when auto-extracted
✓ Removed Age field
✓ Survey data now sent to API
✓ Ethnicity data now sent to API
✓ Support for Foreign ID and Passport
```

## 📚 Documentation Reference

- **Technical Details:** `REGISTRATION-WIZARD-IMPROVEMENTS.md`
- **User Guide:** `WIZARD-QUICK-REFERENCE.md`
- **Summary:** `WIZARD-UPDATE-SUMMARY.md`

## ✨ Key Features Demonstrated

1. **5-Step Wizard** - Streamlined from 6 steps
2. **Smart Data Entry** - Auto-extraction from SA ID
3. **Disabled Fields** - Visual feedback for auto-filled data
4. **Foreign ID Support** - Full support for non-SA applicants
5. **Survey Integration** - Complete questionnaire data capture
6. **Ethnicity Tracking** - Demographic data collection
7. **Validation** - Real-time feedback and error handling
8. **NBT Generation** - Automatic unique number creation

## 🎯 Success Criteria

- [ ] All 5 test scenarios pass
- [ ] All visual checks complete
- [ ] All functional checks complete
- [ ] All data checks complete
- [ ] No console errors
- [ ] No validation errors (when data is valid)
- [ ] NBT number generated
- [ ] Success message displayed

## ⏱️ Time Estimates

- Initial setup: 2 minutes
- Single registration test: 3-5 minutes
- All test scenarios: 15-20 minutes
- Full verification: 30 minutes

## 🚦 Next Steps After Testing

1. ✅ Verify all tests pass
2. ✅ Document any issues found
3. ✅ Run with live backend
4. ✅ User acceptance testing
5. ✅ Deploy to staging
6. ✅ Monitor for issues
7. ✅ Deploy to production

---

**Happy Testing! 🎉**

For issues or questions, refer to the documentation or contact the development team.
