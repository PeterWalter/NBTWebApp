using NBT.Domain.Enums;

namespace NBT.Application.Staff.DTOs;

public class StaffFilterDto
{
    public string? SearchTerm { get; set; }
    public UserRole? Role { get; set; }
    public string? Status { get; set; }
    public string? InstitutionId { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SortBy { get; set; } = "CreatedDate";
    public bool SortDescending { get; set; } = true;
}
