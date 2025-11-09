using NBT.WebUI.Models;

namespace NBT.WebUI.Services;

public interface IRegistrationService
{
    Task<RegistrationResult> RegisterStudentAsync(RegistrationFormModel model);
    Task<bool> CheckDuplicateAsync(string idNumber, string idType);
    Task<bool> ValidateIDNumberAsync(string idNumber, string idType);
}

public class RegistrationResult
{
    public bool Success { get; set; }
    public string? NBTNumber { get; set; }
    public string? ErrorMessage { get; set; }
    public Dictionary<string, string[]>? ValidationErrors { get; set; }
}
