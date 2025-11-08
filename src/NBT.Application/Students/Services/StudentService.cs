using Microsoft.EntityFrameworkCore;
using NBT.Application.Common.Interfaces;
using NBT.Application.Students.DTOs;
using NBT.Domain.Entities;
using NBT.Domain.ValueObjects;

namespace NBT.Application.Students.Services;

public class StudentService : IStudentService
{
    private readonly IApplicationDbContext _context;
    private readonly INBTNumberGenerator _nbtNumberGenerator;

    public StudentService(
        IApplicationDbContext context,
        INBTNumberGenerator nbtNumberGenerator)
    {
        _context = context;
        _nbtNumberGenerator = nbtNumberGenerator;
    }

    public async Task<StudentDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var student = await _context.Students
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

        return student == null ? null : MapToDto(student);
    }

    public async Task<StudentDto?> GetByNBTNumberAsync(string nbtNumber, CancellationToken cancellationToken = default)
    {
        var student = await _context.Students
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.NBTNumber == nbtNumber, cancellationToken);

        return student == null ? null : MapToDto(student);
    }

    public async Task<StudentDto?> GetBySAIDNumberAsync(string saIdNumber, CancellationToken cancellationToken = default)
    {
        var student = await _context.Students
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.IDNumber == saIdNumber, cancellationToken);

        return student == null ? null : MapToDto(student);
    }

    public async Task<IEnumerable<StudentDto>> GetAllAsync(int page = 1, int pageSize = 50, CancellationToken cancellationToken = default)
    {
        var students = await _context.Students
            .AsNoTracking()
            .OrderByDescending(s => s.CreatedDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return students.Select(MapToDto);
    }

    public async Task<IEnumerable<StudentDto>> SearchAsync(string searchTerm, int page = 1, int pageSize = 50, CancellationToken cancellationToken = default)
    {
        var query = _context.Students.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            searchTerm = searchTerm.ToLower();
            query = query.Where(s =>
                s.FirstName.ToLower().Contains(searchTerm) ||
                s.LastName.ToLower().Contains(searchTerm) ||
                s.Email.ToLower().Contains(searchTerm) ||
                s.NBTNumber.Contains(searchTerm) ||
                s.IDNumber.Contains(searchTerm));
        }

        var students = await query
            .OrderByDescending(s => s.CreatedDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return students.Select(MapToDto);
    }

    public async Task<StudentDto> CreateAsync(CreateStudentDto dto, CancellationToken cancellationToken = default)
    {
        // Validate ID Number based on ID Type
        if (dto.IDType == "SA_ID")
        {
            var saIdNumber = SAIDNumber.Create(dto.IDNumber);
        }
        else if (dto.IDType == "FOREIGN_ID" || dto.IDType == "PASSPORT")
        {
            var foreignId = ForeignIDNumber.Create(dto.IDNumber);
        }
        else
        {
            throw new InvalidOperationException($"Invalid ID Type: {dto.IDType}. Must be SA_ID, FOREIGN_ID, or PASSPORT.");
        }

        // Check if student already exists
        var existingStudent = await _context.Students
            .FirstOrDefaultAsync(s => s.IDNumber == dto.IDNumber, cancellationToken);

        if (existingStudent != null)
        {
            throw new InvalidOperationException($"Student with ID number {dto.IDNumber} already exists.");
        }

        // Generate NBT number
        var nbtNumber = await _nbtNumberGenerator.GenerateAsync(cancellationToken);

        // Parse ID type enum
        if (!Enum.TryParse<Domain.Enums.IDType>(dto.IDType, out var idType))
        {
            throw new InvalidOperationException($"Invalid ID Type: {dto.IDType}");
        }

        var student = new Student
        {
            Id = Guid.NewGuid(),
            NBTNumber = nbtNumber,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            IDType = idType,
            IDNumber = dto.IDNumber,
            Nationality = dto.Nationality,
            CountryOfOrigin = dto.CountryOfOrigin,
            DateOfBirth = dto.DateOfBirth,
            Gender = dto.Gender,
            Age = dto.Age,
            Ethnicity = dto.Ethnicity,
            Email = dto.Email,
            Phone = dto.PhoneNumber,
            Address = dto.AddressLine1,
            City = dto.City,
            Province = dto.Province,
            PostalCode = dto.PostalCode,
            SchoolName = dto.SchoolName,
            Grade = dto.GradeYear,
            HomeLanguage = dto.HomeLanguage,
            SpecialAccommodation = dto.RequiresAccommodation ? dto.AccommodationDetails : null,
            MotivationForTesting = dto.MotivationForTesting,
            CareerInterests = dto.CareerInterests,
            PreferredStudyField = dto.PreferredStudyField,
            HasAccessToComputer = dto.HasAccessToComputer,
            HasInternetAccess = dto.HasInternetAccess,
            AdditionalComments = dto.AdditionalComments,
            IsActive = true
        };

        _context.Students.Add(student);
        await _context.SaveChangesAsync(cancellationToken);

        return MapToDto(student);
    }

    public async Task<StudentDto> UpdateAsync(UpdateStudentDto dto, CancellationToken cancellationToken = default)
    {
        var student = await _context.Students
            .FirstOrDefaultAsync(s => s.Id == dto.Id, cancellationToken);

        if (student == null)
        {
            throw new InvalidOperationException($"Student with ID {dto.Id} not found.");
        }

        student.FirstName = dto.FirstName;
        student.LastName = dto.LastName;
        student.Email = dto.Email;
        student.Phone = dto.PhoneNumber;
        student.Address = dto.AddressLine1;
        student.City = dto.City;
        student.Province = dto.Province;
        student.PostalCode = dto.PostalCode;
        student.SchoolName = dto.SchoolName;
        student.Grade = dto.GradeYear;
        student.HomeLanguage = dto.HomeLanguage;
        student.SpecialAccommodation = dto.RequiresAccommodation ? dto.AccommodationDetails : null;
        student.IsActive = dto.IsActive;

        await _context.SaveChangesAsync(cancellationToken);

        return MapToDto(student);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var student = await _context.Students
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

        if (student == null)
        {
            return false;
        }

        student.IsActive = false;
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> ValidateNBTNumberAsync(string nbtNumber, CancellationToken cancellationToken = default)
    {
        if (!NBTNumber.IsValid(nbtNumber))
        {
            return false;
        }

        var exists = await _context.Students
            .AnyAsync(s => s.NBTNumber == nbtNumber, cancellationToken);

        return !exists;
    }

    public Task<bool> ValidateSAIDNumberAsync(string saIdNumber, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(SAIDNumber.IsValid(saIdNumber));
    }

    public async Task<string> GenerateNBTNumberAsync(CancellationToken cancellationToken = default)
    {
        return await _nbtNumberGenerator.GenerateAsync(cancellationToken);
    }

    public async Task<bool> CheckDuplicateAsync(string idNumber, string idType, CancellationToken cancellationToken = default)
    {
        var exists = await _context.Students
            .AnyAsync(s => s.IDNumber == idNumber, cancellationToken);
        return exists;
    }

    private static StudentDto MapToDto(Student student)
    {
        return new StudentDto
        {
            Id = student.Id,
            NBTNumber = student.NBTNumber,
            FirstName = student.FirstName,
            LastName = student.LastName,
            IDType = student.IDType.ToString(),
            IDNumber = student.IDNumber,
            Nationality = student.Nationality,
            CountryOfOrigin = student.CountryOfOrigin,
            DateOfBirth = student.DateOfBirth,
            Gender = student.Gender,
            Age = student.Age,
            Ethnicity = student.Ethnicity,
            Email = student.Email,
            PhoneNumber = student.Phone,
            AlternativePhoneNumber = null,
            AddressLine1 = student.Address,
            AddressLine2 = null,
            City = student.City,
            Province = student.Province,
            PostalCode = student.PostalCode,
            Country = student.CountryOfOrigin ?? "South Africa",
            SchoolName = student.SchoolName ?? string.Empty,
            SchoolProvince = null,
            GradeYear = student.Grade,
            HomeLanguage = student.HomeLanguage,
            RequiresAccommodation = !string.IsNullOrEmpty(student.SpecialAccommodation),
            AccommodationDetails = student.SpecialAccommodation,
            MotivationForTesting = student.MotivationForTesting,
            CareerInterests = student.CareerInterests,
            PreferredStudyField = student.PreferredStudyField,
            HasAccessToComputer = student.HasAccessToComputer,
            HasInternetAccess = student.HasInternetAccess,
            AdditionalComments = student.AdditionalComments,
            IsActive = student.IsActive,
            CreatedDate = student.CreatedDate,
            CreatedBy = student.CreatedBy,
            LastModifiedDate = student.LastModifiedDate,
            LastModifiedBy = student.LastModifiedBy
        };
    }
}
