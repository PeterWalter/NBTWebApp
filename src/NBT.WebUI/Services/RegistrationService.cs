using NBT.WebUI.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace NBT.WebUI.Services;

public class RegistrationService : IRegistrationService
{
    private readonly HttpClient _httpClient;

    public RegistrationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<RegistrationResult> RegisterStudentAsync(RegistrationFormModel model)
    {
        try
        {
            var createDto = new
            {
                firstName = model.FirstName,
                lastName = model.LastName,
                idType = model.IDType,
                idNumber = model.IDNumber,
                nationality = model.Nationality,
                countryOfOrigin = model.CountryOfOrigin,
                dateOfBirth = model.DateOfBirth,
                gender = model.Gender,
                ethnicity = model.Ethnicity,
                email = model.Email,
                phoneNumber = model.PhoneNumber,
                alternativePhoneNumber = model.AlternativePhoneNumber,
                addressLine1 = model.AddressLine1,
                addressLine2 = model.AddressLine2,
                city = model.City,
                province = model.Province,
                postalCode = model.PostalCode,
                country = model.Country,
                schoolName = model.SchoolName,
                schoolProvince = model.SchoolProvince,
                gradeYear = model.GradeYear,
                homeLanguage = model.HomeLanguage,
                motivationForTesting = model.MotivationForTesting,
                careerInterests = model.CareerInterests,
                preferredStudyField = model.PreferredStudyField,
                hasAccessToComputer = model.HasAccessToComputer,
                hasInternetAccess = model.HasInternetAccess,
                additionalComments = model.AdditionalComments,
                requiresAccommodation = model.RequiresAccommodation,
                accommodationDetails = model.AccommodationDetails
            };

            var response = await _httpClient.PostAsJsonAsync("/api/students", createDto);

            if (response.IsSuccessStatusCode)
            {
                var studentDto = await response.Content.ReadFromJsonAsync<StudentResponseDto>();
                return new RegistrationResult
                {
                    Success = true,
                    NBTNumber = studentDto?.NbtNumber
                };
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                try
                {
                    var validationErrors = JsonSerializer.Deserialize<Dictionary<string, string[]>>(errorContent);
                    return new RegistrationResult
                    {
                        Success = false,
                        ErrorMessage = "Validation errors occurred. Please check your inputs.",
                        ValidationErrors = validationErrors
                    };
                }
                catch
                {
                    return new RegistrationResult
                    {
                        Success = false,
                        ErrorMessage = errorContent
                    };
                }
            }
            else
            {
                return new RegistrationResult
                {
                    Success = false,
                    ErrorMessage = $"Registration failed: {response.StatusCode}"
                };
            }
        }
        catch (Exception ex)
        {
            return new RegistrationResult
            {
                Success = false,
                ErrorMessage = $"An error occurred: {ex.Message}"
            };
        }
    }

    public async Task<bool> CheckDuplicateAsync(string idNumber, string idType)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/api/students/check-duplicate?idNumber={Uri.EscapeDataString(idNumber)}&idType={Uri.EscapeDataString(idType)}");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<DuplicateCheckResult>();
                return result?.Exists ?? false;
            }
            return false;
        }
        catch
        {
            return false;
        }
    }

    public Task<bool> ValidateIDNumberAsync(string idNumber, string idType)
    {
        if (string.IsNullOrWhiteSpace(idNumber)) return Task.FromResult(false);

        if (idType == "SA_ID")
        {
            if (idNumber.Length != 13 || !idNumber.All(char.IsDigit))
                return Task.FromResult(false);

            return Task.FromResult(ValidateLuhnChecksum(idNumber));
        }

        if (idType == "FOREIGN_ID" || idType == "PASSPORT")
        {
            return Task.FromResult(idNumber.Length >= 6 && idNumber.Length <= 20);
        }

        return Task.FromResult(false);
    }

    private bool ValidateLuhnChecksum(string number)
    {
        int sum = 0;
        bool alternate = false;

        for (int i = number.Length - 1; i >= 0; i--)
        {
            int n = int.Parse(number[i].ToString());

            if (alternate)
            {
                n *= 2;
                if (n > 9) n -= 9;
            }

            sum += n;
            alternate = !alternate;
        }

        return (sum % 10) == 0;
    }

    private class StudentResponseDto
    {
        public string? NbtNumber { get; set; }
    }

    private class DuplicateCheckResult
    {
        public bool Exists { get; set; }
    }
}
