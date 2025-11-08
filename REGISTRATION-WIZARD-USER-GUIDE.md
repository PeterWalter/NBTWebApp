# Registration Wizard - User Guide

## Quick Start

### For Developers

1. **Run the Application**:
   ```powershell
   .\start-app.ps1
   ```

2. **Access the Registration Wizard**:
   - Open browser to: `https://localhost:5001/register` (or configured port)
   - Or click "Register" in the navigation menu

3. **Test the Wizard**:
   - Use the test data below to complete a registration
   - Verify NBT number is generated
   - Check database for new student record

---

### For Students (End Users)

#### Step-by-Step Guide

##### Step 1: ID Verification

1. **Select your ID Type**:
   - **South African ID**: If you have a 13-digit SA ID number
   - **Foreign ID**: If you have a foreign national ID
   - **Passport**: If you're using a passport

2. **Enter your ID Number**:
   - SA ID: Enter your 13-digit ID number
   - Foreign ID/Passport: Enter your document number
   
3. **Wait for Validation**:
   - ✅ Green message: ID is valid and not registered
   - ❌ Red message: ID is invalid or already registered

4. **For Foreign ID/Passport users**:
   - Enter your Nationality
   - Enter your Country of Origin

---

##### Step 2: Personal Information

1. Enter your **First Name** and **Last Name**
2. Select your **Date of Birth** (must be at least 15 years old)
3. Select your **Gender** (Male, Female, Other)

---

##### Step 3: Contact Information

1. Enter your **Email Address** (used for notifications)
2. Enter your **Phone Number** (primary contact)
3. *Optional*: Enter **Alternative Phone Number**

---

##### Step 4: Address Information

1. Enter your **Address Line 1** (street address)
2. *Optional*: Enter **Address Line 2** (apartment, unit, etc.)
3. Enter your **City**
4. Select your **Province** (from dropdown)
5. Enter your **Postal Code**
6. **Country** is pre-filled with "South Africa"

---

##### Step 5: Academic Information

1. Enter your **School Name**
2. *Optional*: Select your **School Province**
3. *Optional*: Enter your **Current Grade** (10, 11, or 12)
4. *Optional*: Enter your **Home Language** (e.g., English, Afrikaans, Zulu)

---

##### Step 6: Special Accommodations

1. Check the box if you **Require Special Accommodations**
2. If checked, describe your needs in the text area:
   - Examples:
     - Extra time due to learning disability
     - Wheelchair access required
     - Braille materials needed
     - Remote writer assistance
     - Sign language interpreter

---

##### Step 7: Review and Submit

1. **Review all your information** carefully
2. Check for any errors or missing information
3. Use the "Previous" button if you need to make changes
4. Click "Finish" to submit your registration

---

### Success Screen

After successful registration, you will see:

1. ✅ **Confirmation Message**
2. 🔢 **Your NBT Number** (14 digits)
   - **IMPORTANT**: Save this number!
   - You'll need it for:
     - Booking tests
     - Making payments
     - Viewing results
     - All future interactions with NBT

3. 📧 **Email Confirmation**
   - Check your email for a confirmation message
   - Save the email for your records

4. **Next Steps**:
   - Click "Proceed to Login" to log in
   - Or click "Return to Home" to go back

---

## Test Data (For Developers)

### Test Case 1: South African Student

```
ID Type: SA_ID
ID Number: 9001015009087 (valid SA ID with checksum)
First Name: John
Last Name: Doe
Date of Birth: 1990-01-01
Gender: Male
Email: john.doe@example.com
Phone: 0123456789
Address Line 1: 123 Main Street
City: Johannesburg
Province: Gauteng
Postal Code: 2000
School Name: Johannesburg High School
Current Grade: 12
Home Language: English
```

### Test Case 2: Foreign Student with Passport

```
ID Type: PASSPORT
ID Number: A12345678
Nationality: Nigerian
Country of Origin: Nigeria
First Name: Amaka
Last Name: Okonkwo
Date of Birth: 2005-03-15
Gender: Female
Email: amaka.okonkwo@example.com
Phone: 0112223333
Address Line 1: 456 University Road
City: Cape Town
Province: Western Cape
Postal Code: 8001
School Name: Cape Town International School
Current Grade: 11
Home Language: English
```

### Test Case 3: Student with Special Accommodations

```
ID Type: SA_ID
ID Number: 0102035001083 (valid SA ID)
First Name: Sarah
Last Name: Williams
Date of Birth: 2001-02-03
Gender: Female
Email: sarah.williams@example.com
Phone: 0214445555
Requires Accommodation: ✓ (checked)
Accommodation Details: "I require extra time (25%) due to dyslexia and need a separate quiet room for testing."
School Name: Durban Girls High School
Current Grade: 12
Province: KwaZulu-Natal
Home Language: English
```

---

## Validation Rules

### SA ID Number Validation
- Must be **exactly 13 digits**
- Must be **numeric only**
- Must pass **Luhn checksum validation**
- Must **not already be registered**

**Example Valid SA IDs**:
- `9001015009087` (1990-01-01, Male)
- `0102035001083` (2001-02-03, Male)
- `9505205045087` (1995-05-20, Female)

**Common Errors**:
- Too short/long → Must be 13 digits
- Invalid checksum → Last digit doesn't match Luhn calculation
- Already registered → ID is in the database

---

### Foreign ID / Passport Validation
- Length: **6 to 20 characters**
- Can be **alphanumeric**
- Must **not already be registered**

---

### Email Validation
- Must be valid email format
- Example: `user@example.com`

---

### Phone Number Validation
- Must be valid phone format
- South African format expected
- Example: `0123456789` or `+27123456789`

---

### Age Validation
- Date of Birth must make you **at least 15 years old**
- Example: If today is 2025-11-08, DOB must be 2010-11-08 or earlier

---

### Grade Validation
- Must be between **10 and 12**
- Only these values accepted

---

## Common Issues and Solutions

### Issue 1: "Invalid SA ID checksum"
**Cause**: The last digit doesn't match the Luhn calculation  
**Solution**: 
- Double-check you entered the ID correctly
- Ensure you're using a real SA ID number (not made up)
- Use one of the test IDs provided above

---

### Issue 2: "ID number is already registered"
**Cause**: This ID is already in the database  
**Solution**: 
- If it's you, use "Login" instead of "Register"
- If it's not you, contact support
- For testing, use a different test ID

---

### Issue 3: "Email is required" or validation errors
**Cause**: Required field is empty or invalid  
**Solution**: 
- Fill in all fields marked with * (asterisk)
- Ensure email is in correct format
- Check that all validations pass before proceeding

---

### Issue 4: Can't proceed to next step
**Cause**: Current step has validation errors  
**Solution**: 
- Scroll up to see error messages (red text)
- Fix all validation errors
- Wait for success message (green text)

---

### Issue 5: "Network error" or "500 Internal Server Error"
**Cause**: API is not running or database issue  
**Solution**: 
- Ensure WebAPI is running (`dotnet run` in WebAPI project)
- Check database connection string
- Check API logs for errors
- Restart the API

---

## Navigation

### Accessing the Wizard
1. **From Home Page**: Click "Register" button
2. **From Navigation Menu**: Click "Register" link (top menu)
3. **Direct URL**: Navigate to `/register`

### Exiting the Wizard
1. **Cancel/Go Back**: Click browser back button or use nav menu
2. **After Success**: Click "Proceed to Login" or "Return to Home"

---

## Browser Compatibility

### Supported Browsers
- ✅ **Chrome** (v90+)
- ✅ **Microsoft Edge** (v90+)
- ✅ **Firefox** (v88+)
- ✅ **Safari** (v14+)

### Mobile Browsers
- ✅ **Chrome Mobile** (Android)
- ✅ **Safari Mobile** (iOS)
- ✅ **Samsung Internet**

### Requirements
- JavaScript enabled
- WebAssembly support
- Modern browser (last 2 years)

---

## Security and Privacy

### Data Protection
- All communication uses **HTTPS encryption**
- Personal data is **never stored in browser**
- Passwords are **hashed** (for future login feature)
- ID numbers are **validated but not stored in plain text in logs**

### What We Collect
- Personal identification (ID number, name)
- Contact information (email, phone)
- Academic information (school, grade)
- Address (optional)
- Special accommodation needs (optional)

### What We Use It For
- **NBT registration and testing**
- **Communication** (test reminders, results)
- **Research and equity reporting** (anonymized)
- **Accommodation planning**

---

## Accessibility

### Keyboard Navigation
- **Tab**: Move to next field
- **Shift+Tab**: Move to previous field
- **Enter**: Submit/proceed (when enabled)
- **Arrow Keys**: Navigate dropdowns

### Screen Reader Support
- All fields have proper labels
- Error messages are announced
- Progress is announced as you move through steps

### High Contrast Mode
- UI adapts to system high contrast settings
- Color is not the only indicator (icons + text)

---

## Support

### Need Help?
- **Technical Issues**: Contact NBT IT Support
- **Registration Questions**: Contact NBT Administration
- **Accommodation Requests**: Contact NBT Special Accommodations Team

### Feedback
- Report bugs or suggest improvements to the development team

---

## Change Log

### Version 1.0.0 (2025-11-08)
- ✅ Initial release
- ✅ 7-step registration wizard
- ✅ SA ID, Foreign ID, and Passport support
- ✅ Real-time validation
- ✅ NBT number generation
- ✅ Special accommodations
- ✅ Responsive design
- ✅ WCAG 2.1 AA accessibility

---

**Last Updated**: 2025-11-08  
**Version**: 1.0.0
