using NBT.Application.Common;
using NBT.Application.Common.Models;
using NBT.Application.Staff.DTOs;

namespace NBT.Application.Staff.Interfaces;

public interface IStaffService
{
    Task<Result<PaginatedResult<StaffDto>>> GetAllStaffAsync(StaffFilterDto filter);
    Task<Result<StaffDto>> GetStaffByIdAsync(Guid id);
    Task<Result<StaffDto>> CreateStaffAsync(CreateStaffDto dto);
    Task<Result<StaffDto>> UpdateStaffAsync(Guid id, UpdateStaffDto dto);
    Task<Result<bool>> DeleteStaffAsync(Guid id);
    Task<Result<bool>> ActivateStaffAsync(Guid id);
    Task<Result<bool>> DeactivateStaffAsync(Guid id);
    Task<Result<bool>> ChangePasswordAsync(Guid id, ChangePasswordDto dto);
    Task<Result<bool>> ResetPasswordAsync(Guid id, string newPassword);
    Task<Result<List<StaffDto>>> GetStaffByRoleAsync(string role);
    Task<Result<List<StaffDto>>> GetStaffByInstitutionAsync(string institutionId);
}
