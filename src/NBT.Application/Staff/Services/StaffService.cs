using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NBT.Application.Common;
using NBT.Application.Common.Models;
using NBT.Application.Staff.DTOs;
using NBT.Application.Staff.Interfaces;
using NBT.Domain.Entities;
using NBT.Domain.Enums;

namespace NBT.Application.Staff.Services;

public class StaffService : IStaffService
{
    private readonly UserManager<User> _userManager;

    public StaffService(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<PaginatedResult<StaffDto>>> GetAllStaffAsync(StaffFilterDto filter)
    {
        try
        {
            var query = _userManager.Users
                .Where(u => u.Role == UserRole.Staff || u.Role == UserRole.Admin || u.Role == UserRole.SuperUser);

            // Apply filters
            if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
            {
                var searchTerm = filter.SearchTerm.ToLower();
                query = query.Where(u =>
                    (u.Email != null && u.Email.ToLower().Contains(searchTerm)) ||
                    u.FirstName.ToLower().Contains(searchTerm) ||
                    u.LastName.ToLower().Contains(searchTerm));
            }

            if (filter.Role.HasValue)
            {
                query = query.Where(u => u.Role == filter.Role.Value);
            }

            if (!string.IsNullOrWhiteSpace(filter.Status))
            {
                query = query.Where(u => u.Status == filter.Status);
            }

            if (!string.IsNullOrWhiteSpace(filter.InstitutionId))
            {
                query = query.Where(u => u.InstitutionId == filter.InstitutionId);
            }

            // Get total count
            var totalCount = await query.CountAsync();

            // Apply sorting
            query = filter.SortBy?.ToLower() switch
            {
                "email" => filter.SortDescending ? query.OrderByDescending(u => u.Email) : query.OrderBy(u => u.Email),
                "firstname" => filter.SortDescending ? query.OrderByDescending(u => u.FirstName) : query.OrderBy(u => u.FirstName),
                "lastname" => filter.SortDescending ? query.OrderByDescending(u => u.LastName) : query.OrderBy(u => u.LastName),
                "role" => filter.SortDescending ? query.OrderByDescending(u => u.Role) : query.OrderBy(u => u.Role),
                "status" => filter.SortDescending ? query.OrderByDescending(u => u.Status) : query.OrderBy(u => u.Status),
                "lastlogin" => filter.SortDescending ? query.OrderByDescending(u => u.LastLoginDate) : query.OrderBy(u => u.LastLoginDate),
                _ => filter.SortDescending ? query.OrderByDescending(u => u.CreatedDate) : query.OrderBy(u => u.CreatedDate)
            };

            // Apply pagination
            var staff = await query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            var staffDtos = staff.Select(MapToDto).ToList();

            var result = new PaginatedResult<StaffDto>
            {
                Items = staffDtos,
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };

            return Result<PaginatedResult<StaffDto>>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<PaginatedResult<StaffDto>>.Failure($"Error retrieving staff: {ex.Message}");
        }
    }

    public async Task<Result<StaffDto>> GetStaffByIdAsync(Guid id)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return Result<StaffDto>.Failure("Staff member not found");
            }

            if (user.Role == UserRole.Applicant)
            {
                return Result<StaffDto>.Failure("User is not a staff member");
            }

            return Result<StaffDto>.Success(MapToDto(user));
        }
        catch (Exception ex)
        {
            return Result<StaffDto>.Failure($"Error retrieving staff: {ex.Message}");
        }
    }

    public async Task<Result<StaffDto>> CreateStaffAsync(CreateStaffDto dto)
    {
        try
        {
            // Check if user already exists
            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser != null)
            {
                return Result<StaffDto>.Failure("User with this email already exists");
            }

            var user = new User
            {
                Email = dto.Email,
                UserName = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                PhoneNumber = dto.PhoneNumber,
                Role = dto.Role,
                InstitutionName = dto.InstitutionName,
                InstitutionId = dto.InstitutionId,
                Status = "Active",
                CreatedDate = DateTime.UtcNow,
                EmailConfirmed = true // Auto-confirm for staff
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return Result<StaffDto>.Failure($"Failed to create staff: {errors}");
            }

            // Assign role
            var roleResult = await _userManager.AddToRoleAsync(user, dto.Role.ToString());
            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user); // Rollback
                return Result<StaffDto>.Failure("Failed to assign role to staff");
            }

            return Result<StaffDto>.Success(MapToDto(user));
        }
        catch (Exception ex)
        {
            return Result<StaffDto>.Failure($"Error creating staff: {ex.Message}");
        }
    }

    public async Task<Result<StaffDto>> UpdateStaffAsync(Guid id, UpdateStaffDto dto)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return Result<StaffDto>.Failure("Staff member not found");
            }

            if (user.Role == UserRole.Applicant)
            {
                return Result<StaffDto>.Failure("Cannot update applicant as staff");
            }

            // Update properties
            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.PhoneNumber = dto.PhoneNumber;
            user.InstitutionName = dto.InstitutionName;
            user.InstitutionId = dto.InstitutionId;
            user.Status = dto.Status;

            // Update role if changed
            if (user.Role != dto.Role)
            {
                var oldRole = user.Role.ToString();
                var newRole = dto.Role.ToString();
                
                await _userManager.RemoveFromRoleAsync(user, oldRole);
                await _userManager.AddToRoleAsync(user, newRole);
                
                user.Role = dto.Role;
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return Result<StaffDto>.Failure($"Failed to update staff: {errors}");
            }

            return Result<StaffDto>.Success(MapToDto(user));
        }
        catch (Exception ex)
        {
            return Result<StaffDto>.Failure($"Error updating staff: {ex.Message}");
        }
    }

    public async Task<Result<bool>> DeleteStaffAsync(Guid id)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return Result<bool>.Failure("Staff member not found");
            }

            if (user.Role == UserRole.SuperUser)
            {
                return Result<bool>.Failure("Cannot delete SuperUser account");
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return Result<bool>.Failure($"Failed to delete staff: {errors}");
            }

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure($"Error deleting staff: {ex.Message}");
        }
    }

    public async Task<Result<bool>> ActivateStaffAsync(Guid id)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return Result<bool>.Failure("Staff member not found");
            }

            user.Status = "Active";
            var result = await _userManager.UpdateAsync(user);
            
            if (!result.Succeeded)
            {
                return Result<bool>.Failure("Failed to activate staff");
            }

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure($"Error activating staff: {ex.Message}");
        }
    }

    public async Task<Result<bool>> DeactivateStaffAsync(Guid id)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return Result<bool>.Failure("Staff member not found");
            }

            if (user.Role == UserRole.SuperUser)
            {
                return Result<bool>.Failure("Cannot deactivate SuperUser account");
            }

            user.Status = "Inactive";
            var result = await _userManager.UpdateAsync(user);
            
            if (!result.Succeeded)
            {
                return Result<bool>.Failure("Failed to deactivate staff");
            }

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure($"Error deactivating staff: {ex.Message}");
        }
    }

    public async Task<Result<bool>> ChangePasswordAsync(Guid id, ChangePasswordDto dto)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return Result<bool>.Failure("Staff member not found");
            }

            var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return Result<bool>.Failure($"Failed to change password: {errors}");
            }

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure($"Error changing password: {ex.Message}");
        }
    }

    public async Task<Result<bool>> ResetPasswordAsync(Guid id, string newPassword)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return Result<bool>.Failure("Staff member not found");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return Result<bool>.Failure($"Failed to reset password: {errors}");
            }

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure($"Error resetting password: {ex.Message}");
        }
    }

    public async Task<Result<List<StaffDto>>> GetStaffByRoleAsync(string role)
    {
        try
        {
            if (!Enum.TryParse<UserRole>(role, true, out var userRole))
            {
                return Result<List<StaffDto>>.Failure("Invalid role");
            }

            var users = await _userManager.Users
                .Where(u => u.Role == userRole)
                .ToListAsync();

            var staffDtos = users.Select(MapToDto).ToList();
            return Result<List<StaffDto>>.Success(staffDtos);
        }
        catch (Exception ex)
        {
            return Result<List<StaffDto>>.Failure($"Error retrieving staff by role: {ex.Message}");
        }
    }

    public async Task<Result<List<StaffDto>>> GetStaffByInstitutionAsync(string institutionId)
    {
        try
        {
            var users = await _userManager.Users
                .Where(u => u.InstitutionId == institutionId && u.Role != UserRole.Applicant)
                .ToListAsync();

            var staffDtos = users.Select(MapToDto).ToList();
            return Result<List<StaffDto>>.Success(staffDtos);
        }
        catch (Exception ex)
        {
            return Result<List<StaffDto>>.Failure($"Error retrieving staff by institution: {ex.Message}");
        }
    }

    private static StaffDto MapToDto(User user)
    {
        return new StaffDto
        {
            Id = user.Id,
            Email = user.Email ?? string.Empty,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber ?? string.Empty,
            Role = user.Role,
            Status = user.Status,
            InstitutionName = user.InstitutionName,
            InstitutionId = user.InstitutionId,
            LastLoginDate = user.LastLoginDate,
            CreatedDate = user.CreatedDate,
            EmailConfirmed = user.EmailConfirmed,
            PhoneNumberConfirmed = user.PhoneNumberConfirmed
        };
    }
}
