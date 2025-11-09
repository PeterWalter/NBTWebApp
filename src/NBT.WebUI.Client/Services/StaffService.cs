using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using NBT.Application.Common;
using NBT.Application.Staff.DTOs;

namespace NBT.WebUI.Client.Services;

public class StaffService : IStaffService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public StaffService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task<PaginatedResult<StaffDto>> GetAllStaffAsync(StaffFilterDto filter)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/staff/search", filter);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<PaginatedResult<StaffDto>>(_jsonOptions)
                ?? new PaginatedResult<StaffDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting staff: {ex.Message}");
            return new PaginatedResult<StaffDto>();
        }
    }

    public async Task<StaffDto?> GetStaffByIdAsync(Guid id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/staff/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<StaffDto>(_jsonOptions);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting staff by ID: {ex.Message}");
            return null;
        }
    }

    public async Task<StaffDto?> CreateStaffAsync(CreateStaffDto dto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/staff", dto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<StaffDto>(_jsonOptions);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating staff: {ex.Message}");
            return null;
        }
    }

    public async Task<StaffDto?> UpdateStaffAsync(Guid id, UpdateStaffDto dto)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"api/staff/{id}", dto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<StaffDto>(_jsonOptions);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating staff: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> DeleteStaffAsync(Guid id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/staff/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting staff: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> ActivateStaffAsync(Guid id)
    {
        try
        {
            var response = await _httpClient.PostAsync($"api/staff/{id}/activate", null);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error activating staff: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DeactivateStaffAsync(Guid id)
    {
        try
        {
            var response = await _httpClient.PostAsync($"api/staff/{id}/deactivate", null);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deactivating staff: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> ChangePasswordAsync(Guid id, ChangePasswordDto dto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"api/staff/{id}/change-password", dto);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error changing password: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> ResetPasswordAsync(Guid id, string newPassword)
    {
        try
        {
            var content = new StringContent(
                JsonSerializer.Serialize(newPassword),
                Encoding.UTF8,
                "application/json");
            var response = await _httpClient.PostAsync($"api/staff/{id}/reset-password", content);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error resetting password: {ex.Message}");
            return false;
        }
    }

    public async Task<List<StaffDto>> GetStaffByRoleAsync(string role)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/staff/by-role/{role}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<StaffDto>>(_jsonOptions)
                ?? new List<StaffDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting staff by role: {ex.Message}");
            return new List<StaffDto>();
        }
    }

    public async Task<List<StaffDto>> GetStaffByInstitutionAsync(string institutionId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/staff/by-institution/{institutionId}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<StaffDto>>(_jsonOptions)
                ?? new List<StaffDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting staff by institution: {ex.Message}");
            return new List<StaffDto>();
        }
    }
}
