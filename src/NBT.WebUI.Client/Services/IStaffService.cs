using NBT.Application.Common;
using NBT.Application.Staff.DTOs;

namespace NBT.WebUI.Client.Services;

public interface IStaffService
{
    Task<PaginatedResult<StaffDto>> GetAllStaffAsync(StaffFilterDto filter);
    Task<StaffDto?> GetStaffByIdAsync(Guid id);
    Task<StaffDto?> CreateStaffAsync(CreateStaffDto dto);
    Task<StaffDto?> UpdateStaffAsync(Guid id, UpdateStaffDto dto);
    Task<bool> DeleteStaffAsync(Guid id);
    Task<bool> ActivateStaffAsync(Guid id);
    Task<bool> DeactivateStaffAsync(Guid id);
    Task<bool> ChangePasswordAsync(Guid id, ChangePasswordDto dto);
    Task<bool> ResetPasswordAsync(Guid id, string newPassword);
    Task<List<StaffDto>> GetStaffByRoleAsync(string role);
    Task<List<StaffDto>> GetStaffByInstitutionAsync(string institutionId);
}
