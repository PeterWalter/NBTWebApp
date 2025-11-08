# Registration Wizard Quick Start

## 🚀 Get Started in 5 Minutes

### Prerequisites
- .NET 9.0 SDK
- Visual Studio 2022 or VS Code
- SQL Server (for backend API)

### Step 1: Navigate to Client Project
```powershell
cd "D:\projects\source code\NBTWebApp\src\NBT.WebUI.Client"
```

### Step 2: Restore Packages
```powershell
dotnet restore
```

### Step 3: Run the Application
```powershell
dotnet run
```

### Step 4: Open Browser
Navigate to: `https://localhost:5001` or the URL shown in terminal

### Step 5: Test Registration
1. Click **"Register Now"** button on home page
2. Fill in the wizard steps
3. Submit registration

---

## 📁 Project Structure

```
NBT.WebUI.Client/
├── Pages/
│   ├── Home.razor                    # Landing page with CTA
│   └── Registration/
│       └── Register.razor            # Main registration wizard
├── Models/
│   └── RegistrationFormModel.cs      # Form data model
├── Services/
│   ├── IRegistrationService.cs       # Service interface
│   └── RegistrationService.cs        # API communication service
├── Layout/
│   └── NavMenu.razor                 # Navigation (includes Register link)
└── wwwroot/
    └── appsettings.json              # API configuration
```

---

## 🎯 Key Features

### Multi-Step Wizard
1. **ID Verification** - SA ID / Foreign ID / Passport
2. **Personal Info** - Name, DOB, Gender
3. **Contact** - Email, Phone
4. **Address** - Full residential address
5. **Academic** - School, Grade
6. **Accommodations** - Special requirements
7. **Review** - Confirm all details

### Smart Validation
- ✅ Real-time ID validation with Luhn algorithm
- ✅ Duplicate ID detection
- ✅ Email and phone validation
- ✅ Required field checking

---

## 🔧 Configuration

### API Endpoint
Edit `wwwroot/appsettings.json`:
```json
{
  "ApiBaseUrl": "https://localhost:7001"
}
```

### Update API URL for Your Environment
```powershell
# Local development
"ApiBaseUrl": "https://localhost:7001"

# Staging
"ApiBaseUrl": "https://staging-api.nbt.ac.za"

# Production
"ApiBaseUrl": "https://api.nbt.ac.za"
```

---

## 🧪 Testing the Wizard

### Test with SA ID
1. Select **"South African ID"**
2. Enter valid 13-digit ID: `9001015009087`
3. Complete remaining steps
4. Submit and verify NBT number is generated

### Test with Foreign ID
1. Select **"Foreign ID"**
2. Enter Foreign ID: `AB123456`
3. Enter Nationality: `Nigerian`
4. Enter Country: `Nigeria`
5. Complete remaining steps

### Test with Passport
1. Select **"Passport"**
2. Enter Passport: `P1234567`
3. Enter Nationality and Country
4. Complete remaining steps

---

## 🐛 Common Issues & Solutions

### Issue: API Connection Fails
**Solution:**
1. Verify API is running
2. Check `appsettings.json` has correct URL
3. Ensure CORS is configured on API

### Issue: Fluent UI Not Rendering
**Solution:**
1. Run `dotnet restore`
2. Clear browser cache
3. Check browser console for errors

### Issue: Validation Not Working
**Solution:**
1. Check browser console for JavaScript errors
2. Verify Fluent UI packages are installed
3. Ensure internet connection for CDN resources

---

## 📊 Expected API Endpoints

The wizard expects these API endpoints to exist:

### POST /api/students
**Request:**
```json
{
  "firstName": "John",
  "lastName": "Doe",
  "idType": "SA_ID",
  "idNumber": "9001015009087",
  "email": "john.doe@example.com",
  "phoneNumber": "0123456789",
  ...
}
```

**Response:**
```json
{
  "id": "guid",
  "nbtNumber": "202400015",
  "firstName": "John",
  "lastName": "Doe",
  ...
}
```

### GET /api/students/check-duplicate?idNumber=xxx&idType=xxx
**Response:**
```json
{
  "exists": false
}
```

---

## 🎨 Customization

### Change Theme Colors
Edit `Register.razor.css`:
```css
.registration-container {
    background: linear-gradient(135deg, #YOUR_COLOR 0%, #YOUR_COLOR 100%);
}
```

### Add Custom Fields
1. Update `RegistrationFormModel.cs`
2. Add fields to wizard steps in `Register.razor`
3. Update API contract

### Modify Validation
Edit `RegistrationService.cs` → `ValidateIDNumberAsync()`

---

## 📱 Responsive Design

The wizard is fully responsive and tested on:
- 📱 Mobile (320px - 767px)
- 📱 Tablet (768px - 1024px)
- 💻 Desktop (1025px+)

---

## 🔒 Security Notes

- ✅ Client-side validation for UX only
- ✅ Server must validate all inputs
- ✅ Use HTTPS in production
- ✅ Implement rate limiting on API
- ✅ Add CAPTCHA for production

---

## 📚 Additional Resources

### Fluent UI Documentation
https://www.fluentui-blazor.net/

### Blazor WebAssembly Docs
https://learn.microsoft.com/en-us/aspnet/core/blazor/

### Luhn Algorithm
https://en.wikipedia.org/wiki/Luhn_algorithm

---

## 🎯 Next Steps

1. ✅ **Test the wizard** - Complete a registration
2. ✅ **Verify API integration** - Check data reaches backend
3. ✅ **Test validation** - Try invalid inputs
4. ✅ **Mobile test** - Open on phone/tablet
5. ✅ **Add authentication** - Implement login after registration

---

## 💡 Quick Commands

### Build
```powershell
dotnet build
```

### Clean Build
```powershell
dotnet clean
dotnet build
```

### Publish
```powershell
dotnet publish -c Release -o ./publish
```

### Watch (Hot Reload)
```powershell
dotnet watch run
```

---

## 📞 Support

Need help? Check:
1. `REGISTRATION-WIZARD-COMPLETE.md` - Full documentation
2. Browser console for errors
3. API logs for backend issues
4. Network tab for API call details

---

## ✅ Success Checklist

- [ ] Application runs without errors
- [ ] Home page displays "Register Now" button
- [ ] Wizard opens when clicking Register
- [ ] All 7 steps are accessible
- [ ] Validation works on ID field
- [ ] Can navigate forward and backward
- [ ] Review page shows entered data
- [ ] Success page displays after submission
- [ ] NBT number is shown on success
- [ ] Responsive on mobile device

---

**Happy Coding! 🎉**

The registration wizard is ready to use. If you encounter any issues, refer to the complete documentation in `REGISTRATION-WIZARD-COMPLETE.md`.
