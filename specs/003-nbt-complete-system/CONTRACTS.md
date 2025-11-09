# NBT Integrated Web Application - Data Contracts & API Schemas

## Entity Definitions

### Student
```csharp
public class Student
{
    public int Id { get; set; }
    public string NbtNumber { get; set; } // 14-digit Luhn-validated
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    
    // Identity
    public string SaIdNumber { get; set; } // 13-digit for SA citizens
    public string ForeignId { get; set; } // For non-SA applicants
    public string PassportNumber { get; set; }
    public IdentificationType IdentificationType { get; set; }
    
    // Demographics
    public DateTime DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public Ethnicity Ethnicity { get; set; }
    
    // Address
    public string StreetAddress { get; set; }
    public string City { get; set; }
    public string Province { get; set; }
    public string PostalCode { get; set; }
    public string Country { get; set; }
    
    // Account
    public string PasswordHash { get; set; }
    public bool IsActive { get; set; }
    public bool EmailVerified { get; set; }
    public bool PhoneVerified { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
    
    // Navigation Properties
    public List<Registration> Registrations { get; set; }
    public List<Booking> Bookings { get; set; }
    public List<Payment> Payments { get; set; }
    public List<Result> Results { get; set; }
    public List<Document> Documents { get; set; }
    public List<AuditLog> AuditLogs { get; set; }
}

public enum IdentificationType
{
    SouthAfricanId,
    ForeignId,
    Passport
}

public enum Gender
{
    Male,
    Female,
    Other,
    PreferNotToSay
}

public enum Ethnicity
{
    African,
    Coloured,
    Indian,
    White,
    Other,
    PreferNotToSay
}
```

### Registration
```csharp
public class Registration
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public Student Student { get; set; }
    
    // Wizard Progress
    public RegistrationStatus Status { get; set; }
    public int CurrentStep { get; set; }
    public bool Step1Complete { get; set; }
    public bool Step2Complete { get; set; }
    public bool Step3Complete { get; set; }
    
    // Academic Information
    public string HighSchoolName { get; set; }
    public int CurrentGrade { get; set; }
    public string Subjects { get; set; }
    
    // Test Preferences
    public TestType PreferredTestType { get; set; }
    public int? PreferredVenueId { get; set; }
    public Venue PreferredVenue { get; set; }
    public DateTime? PreferredTestDate { get; set; }
    
    // Special Accommodation
    public bool RequiresSpecialAccommodation { get; set; }
    public string SpecialAccommodationDetails { get; set; }
    
    // Survey Data (JSON)
    public string SurveyResponses { get; set; }
    
    // Timestamps
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }
    
    // Auto-save tracking
    public DateTime? LastAutoSaveAt { get; set; }
}

public enum RegistrationStatus
{
    InProgress,
    Completed,
    Abandoned
}

public enum TestType
{
    AQL,
    AQLAndMath
}
```

### Booking
```csharp
public class Booking
{
    public int Id { get; set; }
    public string BookingReference { get; set; }
    
    public int StudentId { get; set; }
    public Student Student { get; set; }
    
    public int TestSessionId { get; set; }
    public TestSession TestSession { get; set; }
    
    public TestType TestType { get; set; }
    public BookingStatus Status { get; set; }
    
    // Dates
    public DateTime BookingDate { get; set; }
    public DateTime TestDate { get; set; }
    public DateTime ClosingDate { get; set; }
    
    // Payment
    public decimal TotalAmount { get; set; }
    public decimal AmountPaid { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    
    // Validation
    public int YearOfBooking { get; set; }
    public DateTime ValidUntil { get; set; } // 3 years from booking
    
    // Changes
    public int ChangeCount { get; set; }
    public DateTime? LastChangedAt { get; set; }
    public string ChangeReason { get; set; }
    
    // Timestamps
    public DateTime CreatedAt { get; set; }
    public DateTime? CancelledAt { get; set; }
    public string CancellationReason { get; set; }
    
    // Navigation
    public List<Payment> Payments { get; set; }
    public Result Result { get; set; }
}

public enum BookingStatus
{
    Active,
    Completed,
    Cancelled,
    Expired
}
```

### Payment
```csharp
public class Payment
{
    public int Id { get; set; }
    public string PaymentReference { get; set; }
    public string EasyPayReference { get; set; }
    
    public int StudentId { get; set; }
    public Student Student { get; set; }
    
    public int BookingId { get; set; }
    public Booking Booking { get; set; }
    
    public int? IntakeYearId { get; set; }
    public IntakeYear IntakeYear { get; set; }
    
    public decimal Amount { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public PaymentStatus Status { get; set; }
    
    public DateTime PaymentDate { get; set; }
    public DateTime? ProcessedAt { get; set; }
    
    public bool IsInstallment { get; set; }
    public int? InstallmentNumber { get; set; }
    
    public string TransactionId { get; set; }
    public string BankReference { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    
    // Navigation
    public List<PaymentInstallment> Installments { get; set; }
}

public enum PaymentMethod
{
    EasyPay,
    BankTransfer,
    Cash,
    Card,
    Other
}

public enum PaymentStatus
{
    Pending,
    Partial,
    Complete,
    Failed,
    Refunded
}
```

### PaymentInstallment
```csharp
public class PaymentInstallment
{
    public int Id { get; set; }
    public int PaymentId { get; set; }
    public Payment Payment { get; set; }
    
    public int InstallmentNumber { get; set; }
    public decimal Amount { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? PaidDate { get; set; }
    public PaymentStatus Status { get; set; }
    
    public DateTime CreatedAt { get; set; }
}
```

### IntakeYear
```csharp
public class IntakeYear
{
    public int Id { get; set; }
    public int Year { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    
    // Pricing
    public decimal AqlTestCost { get; set; }
    public decimal MathTestCost { get; set; }
    public decimal CombinedTestCost { get; set; }
    
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    
    // Navigation
    public List<Payment> Payments { get; set; }
}
```

### Result
```csharp
public class Result
{
    public int Id { get; set; }
    public string Barcode { get; set; } // Unique per test/answer sheet
    
    public int StudentId { get; set; }
    public Student Student { get; set; }
    
    public int BookingId { get; set; }
    public Booking Booking { get; set; }
    
    public TestType TestType { get; set; }
    public DateTime TestDate { get; set; }
    
    // Scores
    public int AcademicLiteracyScore { get; set; }
    public int QuantitativeLiteracyScore { get; set; }
    public int? MathematicsScore { get; set; }
    
    // Performance Levels
    public PerformanceLevel AlPerformanceLevel { get; set; }
    public PerformanceLevel QlPerformanceLevel { get; set; }
    public PerformanceLevel? MatPerformanceLevel { get; set; }
    
    // Status
    public ResultStatus Status { get; set; }
    public DateTime? ReleasedAt { get; set; }
    public bool CertificateGenerated { get; set; }
    
    // Venue
    public int VenueId { get; set; }
    public Venue Venue { get; set; }
    
    // Timestamps
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public enum PerformanceLevel
{
    BasicLower,
    BasicUpper,
    IntermediateLower,
    IntermediateUpper,
    ProficientLower,
    ProficientUpper
}

public enum ResultStatus
{
    Pending,
    Available,
    Withheld,
    UnderReview
}
```

### Venue
```csharp
public class Venue
{
    public int Id { get; set; }
    public string Name { get; set; }
    public VenueType Type { get; set; }
    
    // Location
    public string Address { get; set; }
    public string City { get; set; }
    public string Province { get; set; }
    public string PostalCode { get; set; }
    public string Country { get; set; }
    
    // Contact
    public string ContactPerson { get; set; }
    public string ContactEmail { get; set; }
    public string ContactPhone { get; set; }
    
    // Capacity
    public int Capacity { get; set; }
    public bool IsActive { get; set; }
    
    // Online venue specific
    public bool IsOnlineVenue { get; set; }
    public string OnlineInstructions { get; set; }
    
    // Timestamps
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    // Navigation
    public List<TestSession> TestSessions { get; set; }
    public List<VenueAvailability> Availabilities { get; set; }
    public List<Room> Rooms { get; set; }
}

public enum VenueType
{
    National,
    SpecialSession,
    Research,
    Other
}
```

### TestSession
```csharp
public class TestSession
{
    public int Id { get; set; }
    public string SessionCode { get; set; }
    
    public int VenueId { get; set; } // Linked to Venue, not Room
    public Venue Venue { get; set; }
    
    public int TestDateId { get; set; }
    public TestDate TestDate { get; set; }
    
    public DateTime SessionDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    
    public int Capacity { get; set; }
    public int BookedSeats { get; set; }
    public int AvailableSeats { get; set; }
    
    public bool IsOnline { get; set; }
    public bool IsSunday { get; set; }
    
    public SessionStatus Status { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    // Navigation
    public List<Booking> Bookings { get; set; }
}

public enum SessionStatus
{
    Scheduled,
    Open,
    Full,
    Closed,
    Completed,
    Cancelled
}
```

### TestDate
```csharp
public class TestDate
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public DateTime ClosingDate { get; set; }
    
    public bool IsSunday { get; set; }
    public bool IsOnline { get; set; }
    public bool IsActive { get; set; }
    
    public int IntakeYearId { get; set; }
    public IntakeYear IntakeYear { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    // Navigation
    public List<TestSession> TestSessions { get; set; }
}
```

### VenueAvailability
```csharp
public class VenueAvailability
{
    public int Id { get; set; }
    public int VenueId { get; set; }
    public Venue Venue { get; set; }
    
    public DateTime Date { get; set; }
    public bool IsAvailable { get; set; }
    public string Reason { get; set; }
    
    public DateTime CreatedAt { get; set; }
}
```

### Room
```csharp
public class Room
{
    public int Id { get; set; }
    public string RoomNumber { get; set; }
    public string RoomName { get; set; }
    
    public int VenueId { get; set; }
    public Venue Venue { get; set; }
    
    public int Capacity { get; set; }
    public bool IsActive { get; set; }
    
    public DateTime CreatedAt { get; set; }
}
```

### SpecialSession
```csharp
public class SpecialSession
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public Student Student { get; set; }
    
    public string RemoteVenueName { get; set; }
    public string RemoteVenueAddress { get; set; }
    public string RemoteVenueCity { get; set; }
    public string RemoteVenueProvince { get; set; }
    
    public string InvigilatorName { get; set; }
    public string InvigilatorEmail { get; set; }
    public string InvigilatorPhone { get; set; }
    
    public string Justification { get; set; }
    public SpecialSessionStatus Status { get; set; }
    
    public DateTime RequestedDate { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public string ApprovedBy { get; set; }
    public string ApprovalNotes { get; set; }
    
    public DateTime CreatedAt { get; set; }
}

public enum SpecialSessionStatus
{
    Pending,
    Approved,
    Rejected,
    Completed
}
```

### Document
```csharp
public class Document
{
    public int Id { get; set; }
    public string FileName { get; set; }
    public string ContentType { get; set; }
    public long FileSize { get; set; }
    public string StoragePath { get; set; }
    
    public int StudentId { get; set; }
    public Student Student { get; set; }
    
    public DocumentType Type { get; set; }
    public string Description { get; set; }
    
    public DateTime UploadedAt { get; set; }
    public bool IsVerified { get; set; }
    public DateTime? VerifiedAt { get; set; }
    public string VerifiedBy { get; set; }
}

public enum DocumentType
{
    IdDocument,
    AcademicTranscript,
    SpecialAccommodationProof,
    PaymentReceipt,
    Other
}
```

### Notification
```csharp
public class Notification
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public Student Student { get; set; }
    
    public NotificationType Type { get; set; }
    public string Subject { get; set; }
    public string Message { get; set; }
    
    public bool SendEmail { get; set; }
    public bool SendSms { get; set; }
    
    public NotificationStatus EmailStatus { get; set; }
    public NotificationStatus SmsStatus { get; set; }
    
    public DateTime? EmailSentAt { get; set; }
    public DateTime? SmsSentAt { get; set; }
    
    public string EmailError { get; set; }
    public string SmsError { get; set; }
    
    public DateTime CreatedAt { get; set; }
}

public enum NotificationType
{
    RegistrationConfirmation,
    NbtNumberAssignment,
    BookingConfirmation,
    PaymentReceived,
    TestReminder,
    ResultsAvailable,
    ProfileChanged,
    PasswordReset
}

public enum NotificationStatus
{
    Pending,
    Sent,
    Failed,
    NotRequired
}
```

### User
```csharp
public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    
    public UserRole Role { get; set; }
    public bool IsActive { get; set; }
    
    public string FirstName { get; set; }
    public string LastName { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
    
    // Navigation
    public List<AuditLog> AuditLogs { get; set; }
}

public enum UserRole
{
    Student,
    Staff,
    Admin,
    SuperUser
}
```

### AuditLog
```csharp
public class AuditLog
{
    public int Id { get; set; }
    public string EntityName { get; set; }
    public int EntityId { get; set; }
    public string Action { get; set; }
    
    public string OldValues { get; set; }
    public string NewValues { get; set; }
    
    public int? UserId { get; set; }
    public User User { get; set; }
    
    public int? StudentId { get; set; }
    public Student Student { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public string IpAddress { get; set; }
    public string UserAgent { get; set; }
}
```

## DTOs (Data Transfer Objects)

### Registration DTOs
```csharp
public class RegistrationCreateDto
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string SaIdNumber { get; set; }
    public string ForeignId { get; set; }
    public string PassportNumber { get; set; }
    public IdentificationType IdentificationType { get; set; }
    
    public DateTime? DateOfBirth { get; set; }
    public Gender? Gender { get; set; }
    public Ethnicity Ethnicity { get; set; }
    
    public string StreetAddress { get; set; }
    public string City { get; set; }
    public string Province { get; set; }
    public string PostalCode { get; set; }
    public string PhoneNumber { get; set; }
}

public class RegistrationStep2Dto
{
    public int RegistrationId { get; set; }
    public string HighSchoolName { get; set; }
    public int CurrentGrade { get; set; }
    public string Subjects { get; set; }
    public TestType PreferredTestType { get; set; }
    public int? PreferredVenueId { get; set; }
    public DateTime? PreferredTestDate { get; set; }
    public bool RequiresSpecialAccommodation { get; set; }
    public string SpecialAccommodationDetails { get; set; }
}

public class RegistrationStep3Dto
{
    public int RegistrationId { get; set; }
    public Dictionary<string, string> SurveyResponses { get; set; }
}

public class RegistrationResponseDto
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public string NbtNumber { get; set; }
    public RegistrationStatus Status { get; set; }
    public int CurrentStep { get; set; }
    public bool Step1Complete { get; set; }
    public bool Step2Complete { get; set; }
    public bool Step3Complete { get; set; }
}
```

### Booking DTOs
```csharp
public class BookingCreateDto
{
    public int StudentId { get; set; }
    public int TestSessionId { get; set; }
    public TestType TestType { get; set; }
}

public class BookingUpdateDto
{
    public int Id { get; set; }
    public int TestSessionId { get; set; }
    public string ChangeReason { get; set; }
}

public class BookingResponseDto
{
    public int Id { get; set; }
    public string BookingReference { get; set; }
    public int StudentId { get; set; }
    public string StudentName { get; set; }
    public TestType TestType { get; set; }
    public BookingStatus Status { get; set; }
    public DateTime TestDate { get; set; }
    public DateTime ClosingDate { get; set; }
    public string VenueName { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal AmountPaid { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
}
```

### Payment DTOs
```csharp
public class PaymentCreateDto
{
    public int StudentId { get; set; }
    public int BookingId { get; set; }
    public decimal Amount { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public bool IsInstallment { get; set; }
    public int? InstallmentNumber { get; set; }
}

public class PaymentResponseDto
{
    public int Id { get; set; }
    public string PaymentReference { get; set; }
    public string EasyPayReference { get; set; }
    public decimal Amount { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public PaymentStatus Status { get; set; }
    public DateTime PaymentDate { get; set; }
    public bool IsInstallment { get; set; }
    public int? InstallmentNumber { get; set; }
}

public class PaymentStatusDto
{
    public int BookingId { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal AmountPaid { get; set; }
    public decimal AmountRemaining { get; set; }
    public PaymentStatus Status { get; set; }
    public List<PaymentResponseDto> Payments { get; set; }
}
```

### Result DTOs
```csharp
public class ResultImportDto
{
    public string StudentNbtNumber { get; set; }
    public string Barcode { get; set; }
    public TestType TestType { get; set; }
    public DateTime TestDate { get; set; }
    public int VenueId { get; set; }
    public int AcademicLiteracyScore { get; set; }
    public int QuantitativeLiteracyScore { get; set; }
    public int? MathematicsScore { get; set; }
    public PerformanceLevel AlPerformanceLevel { get; set; }
    public PerformanceLevel QlPerformanceLevel { get; set; }
    public PerformanceLevel? MatPerformanceLevel { get; set; }
}

public class ResultResponseDto
{
    public int Id { get; set; }
    public string Barcode { get; set; }
    public string StudentName { get; set; }
    public string NbtNumber { get; set; }
    public TestType TestType { get; set; }
    public DateTime TestDate { get; set; }
    public string VenueName { get; set; }
    
    public int AcademicLiteracyScore { get; set; }
    public string AlPerformanceLevel { get; set; }
    
    public int QuantitativeLiteracyScore { get; set; }
    public string QlPerformanceLevel { get; set; }
    
    public int? MathematicsScore { get; set; }
    public string MatPerformanceLevel { get; set; }
    
    public ResultStatus Status { get; set; }
    public DateTime? ReleasedAt { get; set; }
    public bool CanDownloadCertificate { get; set; }
}
```

### Venue DTOs
```csharp
public class VenueCreateDto
{
    public string Name { get; set; }
    public VenueType Type { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string Province { get; set; }
    public string PostalCode { get; set; }
    public string ContactPerson { get; set; }
    public string ContactEmail { get; set; }
    public string ContactPhone { get; set; }
    public int Capacity { get; set; }
    public bool IsOnlineVenue { get; set; }
}

public class VenueResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public VenueType Type { get; set; }
    public string FullAddress { get; set; }
    public int Capacity { get; set; }
    public bool IsActive { get; set; }
    public bool IsOnlineVenue { get; set; }
}
```

## API Endpoints

### Authentication
```
POST   /api/auth/register          # Register new student account
POST   /api/auth/login             # Login
POST   /api/auth/refresh           # Refresh JWT token
POST   /api/auth/logout            # Logout
POST   /api/auth/forgot-password   # Request password reset
POST   /api/auth/reset-password    # Reset password
POST   /api/auth/verify-email      # Verify email
POST   /api/auth/send-otp          # Send OTP
POST   /api/auth/verify-otp        # Verify OTP
```

### Registration
```
POST   /api/registration/create           # Step 1: Create registration
PUT    /api/registration/{id}/step2       # Step 2: Academic info
PUT    /api/registration/{id}/step3       # Step 3: Survey
GET    /api/registration/{id}             # Get registration
GET    /api/registration/resume/{userId}  # Resume interrupted registration
POST   /api/registration/generate-nbt     # Generate NBT number
POST   /api/registration/validate-said    # Validate SA ID
POST   /api/registration/validate-foreign # Validate Foreign ID
DELETE /api/registration/{id}             # Delete registration
```

### Booking
```
POST   /api/booking/create               # Create booking
PUT    /api/booking/{id}                 # Update booking
DELETE /api/booking/{id}                 # Cancel booking
GET    /api/booking/{id}                 # Get booking
GET    /api/booking/student/{studentId}  # Get student bookings
GET    /api/booking/venue/{venueId}      # Get venue bookings
GET    /api/booking/check-eligibility/{studentId} # Check booking eligibility
```

### Payment
```
POST   /api/payment/create               # Record payment
GET    /api/payment/{id}                 # Get payment
GET    /api/payment/student/{studentId}  # Get student payments
GET    /api/payment/booking/{bookingId}  # Get booking payments
POST   /api/payment/easypay-callback     # EasyPay webhook
POST   /api/payment/upload-file          # Upload bank payment file
GET    /api/payment/status/{bookingId}   # Check payment status
POST   /api/payment/installment          # Record installment payment
```

### Results
```
POST   /api/results/import               # Bulk import results
GET    /api/results/{id}                 # Get result
GET    /api/results/student/{studentId}  # Get student results
GET    /api/results/certificate/{id}     # Generate certificate PDF
GET    /api/results/verify/{barcode}     # Verify result by barcode
PUT    /api/results/{id}                 # Update result
DELETE /api/results/{id}                 # Delete result
```

### Venue
```
POST   /api/venue/create                 # Create venue
PUT    /api/venue/{id}                   # Update venue
DELETE /api/venue/{id}                   # Delete venue
GET    /api/venue/{id}                   # Get venue
GET    /api/venue/list                   # List venues
GET    /api/venue/availability/{venueId} # Check availability
POST   /api/venue/set-availability       # Set venue availability
```

### Test Date
```
POST   /api/testdate/create              # Create test date
PUT    /api/testdate/{id}                # Update test date
DELETE /api/testdate/{id}                # Delete test date
GET    /api/testdate/list                # List test dates
GET    /api/testdate/calendar            # Get calendar view
GET    /api/testdate/{id}                # Get test date
```

### Test Session
```
POST   /api/testsession/create           # Create test session
PUT    /api/testsession/{id}             # Update test session
DELETE /api/testsession/{id}             # Delete test session
GET    /api/testsession/{id}             # Get test session
GET    /api/testsession/date/{dateId}    # Get sessions by date
GET    /api/testsession/venue/{venueId}  # Get sessions by venue
```

### Special Session
```
POST   /api/specialsession/create        # Request special session
GET    /api/specialsession/{id}          # Get special session
GET    /api/specialsession/student/{studentId} # Get student special sessions
PUT    /api/specialsession/{id}/approve  # Approve special session
PUT    /api/specialsession/{id}/reject   # Reject special session
GET    /api/specialsession/pending       # Get pending requests
```

### Students (Staff/Admin)
```
GET    /api/students/list                # List all students
GET    /api/students/{id}                # Get student
PUT    /api/students/{id}                # Update student
DELETE /api/students/{id}                # Delete student
GET    /api/students/search              # Search students
GET    /api/students/{id}/history        # Get student history
```

### Users (Admin)
```
POST   /api/users/create                 # Create user
PUT    /api/users/{id}                   # Update user
DELETE /api/users/{id}                   # Delete user
GET    /api/users/{id}                   # Get user
GET    /api/users/list                   # List users
PUT    /api/users/{id}/role              # Update user role
PUT    /api/users/{id}/activate          # Activate user
PUT    /api/users/{id}/deactivate        # Deactivate user
```

### Audit
```
GET    /api/audit/logs                   # Get audit logs
GET    /api/audit/entity/{entityName}/{entityId} # Get entity audit trail
GET    /api/audit/user/{userId}          # Get user activity
GET    /api/audit/export                 # Export audit logs
```

### Reports
```
GET    /api/reports/registrations        # Registration report
GET    /api/reports/bookings             # Booking report
GET    /api/reports/payments             # Payment report
GET    /api/reports/results              # Results report
GET    /api/reports/venues               # Venue utilization report
POST   /api/reports/custom               # Custom report
GET    /api/reports/export/{id}          # Export report
GET    /api/reports/dashboard            # Dashboard statistics
```

### Documents
```
POST   /api/documents/upload             # Upload document
GET    /api/documents/{id}               # Get document
GET    /api/documents/student/{studentId} # Get student documents
DELETE /api/documents/{id}               # Delete document
PUT    /api/documents/{id}/verify        # Verify document
GET    /api/documents/{id}/download      # Download document
```

### Notifications
```
POST   /api/notifications/send           # Send notification
GET    /api/notifications/student/{studentId} # Get student notifications
GET    /api/notifications/{id}           # Get notification
GET    /api/notifications/pending        # Get pending notifications
POST   /api/notifications/resend/{id}    # Resend notification
```

## Validation Rules

### NBT Number
- Must be exactly 14 digits
- Must pass Luhn (modulus-10) algorithm validation
- Must be unique across all students

### SA ID Number
- Must be exactly 13 digits
- Must pass Luhn algorithm validation
- First 6 digits: Date of birth (YYMMDD)
- Next 4 digits: Gender and citizenship
- Digit 11: SA citizen indicator (0 or 1)
- Last digit: Checksum

### Email
- RFC 5322 compliant
- Must be unique per student account

### Password
- Minimum 8 characters
- At least one uppercase letter
- At least one lowercase letter
- At least one digit
- At least one special character

### Phone Number
- Support SA format: +27 or 0 prefix
- Support international formats
- Digits only after prefix

### File Upload
- Maximum file size: 10MB
- Allowed types: PDF, JPG, PNG, DOCX
- Virus scanning required

## JSON Schema Examples

### Registration Create Request
```json
{
  "email": "student@example.com",
  "password": "SecureP@ss123",
  "confirmPassword": "SecureP@ss123",
  "firstName": "John",
  "lastName": "Doe",
  "identificationType": "SouthAfricanId",
  "saIdNumber": "9901015800086",
  "dateOfBirth": "1999-01-01",
  "gender": "Male",
  "ethnicity": "African",
  "streetAddress": "123 Main Street",
  "city": "Cape Town",
  "province": "Western Cape",
  "postalCode": "8001",
  "phoneNumber": "+27821234567"
}
```

### Booking Create Request
```json
{
  "studentId": 1,
  "testSessionId": 5,
  "testType": "AQLAndMath"
}
```

### Payment Create Request
```json
{
  "studentId": 1,
  "bookingId": 10,
  "amount": 500.00,
  "paymentMethod": "EasyPay",
  "isInstallment": true,
  "installmentNumber": 1
}
```

### Result Import Request
```json
{
  "studentNbtNumber": "12345678901234",
  "barcode": "NBT2024ABC123",
  "testType": "AQLAndMath",
  "testDate": "2024-06-15",
  "venueId": 3,
  "academicLiteracyScore": 65,
  "quantitativeLiteracyScore": 70,
  "mathematicsScore": 75,
  "alPerformanceLevel": "IntermediateLower",
  "qlPerformanceLevel": "IntermediateUpper",
  "matPerformanceLevel": "ProficientLower"
}
```

## Error Responses

### Standard Error Response
```json
{
  "error": {
    "code": "VALIDATION_ERROR",
    "message": "Validation failed",
    "details": [
      {
        "field": "email",
        "message": "Email is already in use"
      }
    ]
  },
  "timestamp": "2024-01-15T10:30:00Z",
  "path": "/api/registration/create"
}
```

### Error Codes
- `VALIDATION_ERROR`: Input validation failed
- `AUTHENTICATION_ERROR`: Authentication failed
- `AUTHORIZATION_ERROR`: Insufficient permissions
- `NOT_FOUND`: Resource not found
- `DUPLICATE_ERROR`: Duplicate entry
- `BUSINESS_RULE_VIOLATION`: Business rule violated
- `EXTERNAL_SERVICE_ERROR`: External service failure
- `INTERNAL_ERROR`: Internal server error
