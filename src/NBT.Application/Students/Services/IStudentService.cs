using NBT.Application.Students.DTOs;

namespace NBT.Application.Students.Services;

public interface IStudentService
{
    Task<StudentDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<StudentDto?> GetByNBTNumberAsync(string nbtNumber, CancellationToken cancellationToken = default);
    Task<StudentDto?> GetBySAIDNumberAsync(string saIdNumber, CancellationToken cancellationToken = default);
    Task<IEnumerable<StudentDto>> GetAllAsync(int page = 1, int pageSize = 50, CancellationToken cancellationToken = default);
    Task<IEnumerable<StudentDto>> SearchAsync(string searchTerm, int page = 1, int pageSize = 50, CancellationToken cancellationToken = default);
    Task<StudentDto> CreateAsync(CreateStudentDto dto, CancellationToken cancellationToken = default);
    Task<StudentDto> UpdateAsync(UpdateStudentDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ValidateNBTNumberAsync(string nbtNumber, CancellationToken cancellationToken = default);
    Task<bool> ValidateSAIDNumberAsync(string saIdNumber, CancellationToken cancellationToken = default);
    Task<string> GenerateNBTNumberAsync(CancellationToken cancellationToken = default);
    Task<bool> CheckDuplicateAsync(string idNumber, string idType, CancellationToken cancellationToken = default);
}
