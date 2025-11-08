# NBT Developer Quick Reference Card

**Version**: 2.0 | **Date**: 2025-11-08 | **Status**: Active

---

## 🚀 Quick Start (5 Minutes)

```bash
# Clone & setup
git clone https://github.com/your-org/NBTWebApp.git
cd NBTWebApp
dotnet restore

# Configure database (update appsettings.Development.json first)
dotnet ef database update --project src/NBT.Infrastructure --startup-project src/NBT.WebAPI

# Run (2 terminals)
cd src/NBT.WebAPI && dotnet run      # Terminal 1 - API (port 5001)
cd src/NBT.WebUI && dotnet run       # Terminal 2 - UI (port 5002)

# Verify
# API: https://localhost:5001/swagger
# UI:  https://localhost:5002
# Login: admin@nbt.ac.za / Admin@123
```

---

## 📁 Project Structure

```
src/
├── NBT.Domain/          # Entities, enums, value objects (NO dependencies)
├── NBT.Application/     # Services, DTOs, interfaces (depends on Domain)
├── NBT.Infrastructure/  # EF Core, external services (depends on Application)
├── NBT.WebAPI/          # REST API controllers (depends on Infrastructure)
└── NBT.WebUI/           # Blazor Web App (depends on WebAPI)

tests/
├── NBT.Domain.Tests/        # Unit tests for domain logic
├── NBT.Application.Tests/   # Unit tests for services
└── NBT.IntegrationTests/    # API integration tests

specs/002-nbt-integrated-system/
├── constitution.md         # Architecture principles (READ FIRST)
├── contracts.md           # API schemas & entities
├── plan.md               # Implementation timeline
├── tasks.md              # Granular task breakdown
├── CRITICAL-UPDATES.md   # Latest requirements & fixes
└── quickstart.md         # Setup guide
```

---

## 🎯 Critical Business Rules (MEMORIZE)

### NBT Number
- **Format**: 14 digits (YYYYSSSSSSSSSC)
- **YYYY**: Year (4 digits)
- **SSSSSSSSS**: Sequence (9 digits)
- **C**: Luhn checksum (1 digit)
- **Validation**: MUST pass Luhn algorithm
- **Usage**: Unique identifier for ALL student activities

### ID Types (3 Supported)
1. **SA_ID**: 13-digit South African ID (Luhn validated)
2. **FOREIGN_ID**: 6-20 alphanumeric (foreign nationals)
3. **PASSPORT**: 6-20 alphanumeric (international)

### Booking Rules (ENFORCE STRICTLY)
| Rule | Description |
|------|-------------|
| ✅ **Intake Start** | Bookings open April 1 annually |
| ✅ **One Active** | Student can have only 1 active booking |
| ✅ **Rebooking** | Only after closing date of previous booking |
| ✅ **Annual Limit** | Max 2 tests per calendar year |
| ✅ **Validity** | Tests valid for 3 years from booking date |
| ✅ **Changes** | Allowed BEFORE close date only |

### TestSession Relationship (CRITICAL)
- ✅ `TestSession` → `Venue` (many-to-one)
- ❌ `TestSession` → `Room` (NO DIRECT LINK)
- ✅ `RoomAllocation` links students to rooms within a session

---

## 🔧 Common Commands

### Database
```bash
# Create migration
dotnet ef migrations add MigrationName --project src/NBT.Infrastructure --startup-project src/NBT.WebAPI

# Apply migration
dotnet ef database update --project src/NBT.Infrastructure --startup-project src/NBT.WebAPI

# Rollback migration
dotnet ef database update PreviousMigration --project src/NBT.Infrastructure --startup-project src/NBT.WebAPI

# Generate SQL script
dotnet ef migrations script --project src/NBT.Infrastructure --startup-project src/NBT.WebAPI --output migration.sql

# Drop database (DEV ONLY)
dotnet ef database drop --project src/NBT.Infrastructure --startup-project src/NBT.WebAPI
```

### Testing
```bash
# Run all tests
dotnet test

# Run specific test project
dotnet test tests/NBT.Domain.Tests/

# Run with coverage
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover

# Run specific test
dotnet test --filter "FullyQualifiedName~NBTNumberTests.Generate_ValidInput_ReturnsValidNBTNumber"
```

### Build & Run
```bash
# Build (all projects)
dotnet build

# Build specific project
dotnet build src/NBT.WebAPI/NBT.WebAPI.csproj

# Run API with hot reload
dotnet watch run --project src/NBT.WebAPI

# Run UI with hot reload
dotnet watch run --project src/NBT.WebUI

# Publish for production
dotnet publish src/NBT.WebAPI -c Release -o publish/api
dotnet publish src/NBT.WebUI -c Release -o publish/ui
```

---

## 🚨 CRITICAL FIX: JSON Serialization (DO FIRST)

### Problem
"property value in JSON" errors due to inconsistent serialization.

### Solution (3 Steps)

#### Step 1: Update WebAPI Program.cs

```csharp
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = builder.Environment.IsDevelopment();
    });
```

#### Step 2: Update WebUI Program.cs

```csharp
builder.Services.AddHttpClient("NBT.WebAPI", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiUrl"] ?? "https://localhost:5001");
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
```

#### Step 3: Add [JsonPropertyName] to ALL DTOs

```csharp
using System.Text.Json.Serialization;

public class StudentDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    
    [JsonPropertyName("nbtNumber")]
    public string NBTNumber { get; set; }
    
    [JsonPropertyName("firstName")]
    public string FirstName { get; set; }
    
    // ... all properties
}
```

**Test**: Run `.\APPLY-JSON-FIX.ps1` for diagnostics

---

## 📝 Code Templates

### Domain Entity
```csharp
namespace NBT.Domain.Entities;

public class EntityName : BaseEntity, IAuditableEntity
{
    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;
    
    // Navigation properties
    public ICollection<RelatedEntity> RelatedEntities { get; set; } = new List<RelatedEntity>();
    
    // IAuditableEntity
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime? UpdatedDate { get; set; }
    public string? UpdatedBy { get; set; }
}
```

### EF Core Configuration
```csharp
public class EntityNameConfiguration : IEntityTypeConfiguration<EntityName>
{
    public void Configure(EntityTypeBuilder<EntityName> builder)
    {
        builder.ToTable("TableName");
        builder.HasKey(e => e.Id);
        
        // Unique constraints
        builder.HasIndex(e => e.Name).IsUnique();
        
        // Relationships
        builder.HasMany(e => e.RelatedEntities)
            .WithOne(r => r.Parent)
            .HasForeignKey(r => r.ParentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
```

### Application Service
```csharp
public interface IEntityService
{
    Task<PagedResult<EntityDto>> GetAllAsync(int page, int pageSize, CancellationToken ct = default);
    Task<EntityDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<EntityDto> CreateAsync(CreateEntityRequest request, CancellationToken ct = default);
    Task<EntityDto> UpdateAsync(Guid id, UpdateEntityRequest request, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}

public class EntityService : IEntityService
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public EntityService(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<PagedResult<EntityDto>> GetAllAsync(int page, int pageSize, CancellationToken ct = default)
    {
        var query = _context.Entities.AsNoTracking();
        
        var totalCount = await query.CountAsync(ct);
        
        var items = await query
            .OrderByDescending(e => e.CreatedDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ProjectTo<EntityDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);
        
        return new PagedResult<EntityDto>(items, totalCount, page, pageSize);
    }
    
    // ... other methods
}
```

### API Controller
```csharp
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,Staff")]
public class EntitiesController : ControllerBase
{
    private readonly IEntityService _service;
    
    public EntitiesController(IEntityService service)
    {
        _service = service;
    }
    
    [HttpGet]
    public async Task<ActionResult<PagedResult<EntityDto>>> GetAll(
        [FromQuery] int page = 1, 
        [FromQuery] int pageSize = 50,
        CancellationToken ct = default)
    {
        var result = await _service.GetAllAsync(page, pageSize, ct);
        return Ok(result);
    }
    
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<EntityDto>> Create(
        [FromBody] CreateEntityRequest request,
        CancellationToken ct = default)
    {
        var entity = await _service.CreateAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
    }
    
    // ... other endpoints
}
```

### Unit Test
```csharp
public class EntityServiceTests
{
    private readonly Mock<IApplicationDbContext> _contextMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly EntityService _service;
    
    public EntityServiceTests()
    {
        _contextMock = new Mock<IApplicationDbContext>();
        _mapperMock = new Mock<IMapper>();
        _service = new EntityService(_contextMock.Object, _mapperMock.Object);
    }
    
    [Fact]
    public async Task CreateAsync_ValidRequest_ReturnsCreatedEntity()
    {
        // Arrange
        var request = new CreateEntityRequest { Name = "Test" };
        var entity = new Entity { Id = Guid.NewGuid(), Name = "Test" };
        var dto = new EntityDto { Id = entity.Id, Name = "Test" };
        
        _mapperMock.Setup(m => m.Map<Entity>(request)).Returns(entity);
        _mapperMock.Setup(m => m.Map<EntityDto>(entity)).Returns(dto);
        
        // Act
        var result = await _service.CreateAsync(request);
        
        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Test");
        _contextMock.Verify(c => c.Entities.Add(entity), Times.Once);
        _contextMock.Verify(c => c.SaveChangesAsync(default), Times.Once);
    }
}
```

---

## 🔍 Validation Patterns

### FluentValidation
```csharp
public class CreateEntityValidator : AbstractValidator<CreateEntityRequest>
{
    public CreateEntityValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name too long");
        
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MustAsync(BeUniqueEmail).WithMessage("Email already exists");
    }
    
    private async Task<bool> BeUniqueEmail(string email, CancellationToken ct)
    {
        // Check database
        return true;
    }
}
```

### NBT Number Validation
```csharp
// Generate new NBT number
var nbtNumber = NBTNumber.Generate(year: 2025, sequence: 12345);
// Result: "20250000123451" (14 digits with checksum)

// Validate existing NBT number
bool isValid = NBTNumber.IsValid("20250000123451");
```

### SA ID Number Validation
```csharp
// Validate and extract info
var idNumber = SAIDNumber.Create("0001015800089");

Console.WriteLine(idNumber.DateOfBirth); // 2000-01-01
Console.WriteLine(idNumber.Gender);      // "M"
Console.WriteLine(idNumber.IsSACitizen); // true
```

---

## 📊 Common Queries

### Get Students with Registrations
```csharp
var students = await _context.Students
    .Include(s => s.Registrations)
        .ThenInclude(r => r.Payment)
    .Include(s => s.TestResults)
    .AsNoTracking()
    .Where(s => s.Grade == 12)
    .OrderBy(s => s.LastName)
    .ToListAsync();
```

### Get Available Test Sessions
```csharp
var today = DateTime.Now.Date;

var sessions = await _context.TestSessions
    .Include(s => s.Venue)
    .AsNoTracking()
    .Where(s => s.SessionDate >= today)
    .Where(s => s.Status == SessionStatus.Open)
    .Where(s => s.AvailableSeats > 0)
    .OrderBy(s => s.SessionDate)
    .ToListAsync();
```

### Check Booking Limit
```csharp
var year = DateTime.Now.Year;
var startDate = new DateTime(year, 1, 1);
var endDate = new DateTime(year, 12, 31);

var bookingCount = await _context.Registrations
    .Where(r => r.StudentId == studentId)
    .Where(r => r.RegistrationDate >= startDate && r.RegistrationDate <= endDate)
    .Where(r => r.Status != RegistrationStatus.Cancelled)
    .CountAsync();

bool canBook = bookingCount < 2;
```

---

## 🔐 Authorization Examples

### Controller Level
```csharp
[Authorize(Roles = "Admin,SuperUser")]  // Admin OR SuperUser
public class AdminController : ControllerBase { }
```

### Action Level
```csharp
[HttpPost]
[Authorize(Roles = "SuperUser")]  // Only SuperUser
public async Task<IActionResult> ImportResults() { }
```

### Conditional Logic
```csharp
if (User.IsInRole("Admin") || User.IsInRole("SuperUser"))
{
    // Can perform action
}
```

---

## 📞 Help & Resources

### Documentation
- **Constitution** (principles): `specs/002-nbt-integrated-system/constitution.md`
- **Contracts** (API/entities): `specs/002-nbt-integrated-system/contracts.md`
- **Tasks** (what to do): `specs/002-nbt-integrated-system/tasks.md`
- **Critical Updates** (latest): `specs/002-nbt-integrated-system/CRITICAL-UPDATES.md`

### Common Issues

| Issue | Solution |
|-------|----------|
| JSON errors | Run `.\APPLY-JSON-FIX.ps1` and apply fixes |
| Migration fails | Check DbContext, rollback, fix, re-run |
| Test fails | Check test data setup, mock configuration |
| Unauthorized | Check JWT token, role assignment |
| Duplicate NBT number | Use `NBTNumberGenerator` service |

### Escalation
1. Check documentation (`specs/` folder)
2. Check similar code in project
3. Ask team member
4. Escalate to Tech Lead

---

## 🎯 Current Phase: Phase 1 (Week 1)

**Status**: Ready to start  
**First Task**: T016 - Create ValueObject Base Class  
**Location**: `src/NBT.Domain/Common/ValueObject.cs`  
**Time**: 1 hour

**Next 5 Tasks**:
1. T016: Create ValueObject base class (1h)
2. T017: Create DomainException (30m)
3. T018: Implement NBTNumber value object (4h)
4. T019: Write NBTNumber unit tests (3h)
5. T020: Implement SAIDNumber value object (5h)

**See**: `specs/002-nbt-integrated-system/tasks.md` for full breakdown

---

## ✅ Daily Checklist

- [ ] Pull latest code (`git pull origin develop`)
- [ ] Run tests before starting (`dotnet test`)
- [ ] Create feature branch (`git checkout -b feature/task-name`)
- [ ] Write code + tests together
- [ ] Run tests (`dotnet test`)
- [ ] Check code coverage (>80%)
- [ ] Commit with clear message
- [ ] Push and create PR
- [ ] Request code review
- [ ] Update task status in tracker

---

**Quick Reference Version**: 2.0  
**Last Updated**: 2025-11-08  
**Next Review**: Weekly during implementation

**Print this and keep it handy! 📋**
