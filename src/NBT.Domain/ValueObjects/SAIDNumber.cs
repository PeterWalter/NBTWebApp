using NBT.Domain.Common;
using NBT.Domain.Exceptions;

namespace NBT.Domain.ValueObjects;

/// <summary>
/// Represents a South African ID number with validation.
/// Format: 13 digits (YYMMDDGSSSCAZ where:
///   YY = Year, MM = Month, DD = Day,
///   G = Gender (0-4=Female, 5-9=Male),
///   SSS = Sequence,
///   C = Citizenship (0=SA citizen, 1=Permanent resident),
///   A = Usually 8 or 9,
///   Z = Luhn checksum digit)
/// Example: 9001015009087
/// </summary>
public sealed class SAIDNumber : ValueObject
{
    private const int SAIDLength = 13;

    public string Value { get; }
    public DateTime DateOfBirth { get; }
    public string Gender { get; }
    public bool IsSACitizen { get; }

    private SAIDNumber(string value, DateTime dateOfBirth, string gender, bool isSACitizen)
    {
        Value = value;
        DateOfBirth = dateOfBirth;
        Gender = gender;
        IsSACitizen = isSACitizen;
    }

    /// <summary>
    /// Creates a South African ID number from a string value.
    /// Validates format, date, and Luhn checksum.
    /// </summary>
    /// <param name="value">The 13-digit SA ID number</param>
    /// <returns>A validated SA ID number</returns>
    /// <exception cref="DomainException">Thrown when the ID number is invalid</exception>
    public static SAIDNumber Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("South African ID number cannot be empty.");

        // Remove any whitespace
        value = value.Trim();

        if (!IsValid(value, out DateTime dateOfBirth, out string gender, out bool isSACitizen, out string? errorMessage))
            throw new DomainException(errorMessage ?? $"Invalid South African ID number: {value}");

        return new SAIDNumber(value, dateOfBirth, gender, isSACitizen);
    }

    /// <summary>
    /// Validates a South African ID number.
    /// </summary>
    /// <param name="value">The ID number to validate</param>
    /// <param name="dateOfBirth">Extracted date of birth</param>
    /// <param name="gender">Extracted gender (Male/Female)</param>
    /// <param name="isSACitizen">Whether the person is a SA citizen</param>
    /// <param name="errorMessage">Error message if invalid</param>
    /// <returns>True if valid, false otherwise</returns>
    public static bool IsValid(string? value, out DateTime dateOfBirth, out string gender, out bool isSACitizen, out string? errorMessage)
    {
        dateOfBirth = DateTime.MinValue;
        gender = string.Empty;
        isSACitizen = false;
        errorMessage = null;

        if (string.IsNullOrWhiteSpace(value))
        {
            errorMessage = "ID number cannot be empty.";
            return false;
        }

        value = value.Trim();

        // Must be exactly 13 digits
        if (value.Length != SAIDLength)
        {
            errorMessage = $"ID number must be exactly {SAIDLength} digits.";
            return false;
        }

        // Must contain only digits
        if (!value.All(char.IsDigit))
        {
            errorMessage = "ID number must contain only digits.";
            return false;
        }

        // Validate and extract date of birth
        if (!TryExtractDateOfBirth(value, out dateOfBirth))
        {
            errorMessage = "ID number contains an invalid date of birth.";
            return false;
        }

        // Extract gender
        gender = ExtractGender(value);

        // Extract citizenship
        isSACitizen = ExtractCitizenship(value);

        // Validate Luhn checksum
        if (!ValidateLuhnChecksum(value))
        {
            errorMessage = "ID number has an invalid checksum.";
            return false;
        }

        return true;
    }

    /// <summary>
    /// Simple overload for quick validation without out parameters.
    /// </summary>
    public static bool IsValid(string? value)
    {
        return IsValid(value, out _, out _, out _, out _);
    }

    /// <summary>
    /// Tries to extract the date of birth from the ID number.
    /// </summary>
    private static bool TryExtractDateOfBirth(string idNumber, out DateTime dateOfBirth)
    {
        dateOfBirth = DateTime.MinValue;

        try
        {
            int year = int.Parse(idNumber.Substring(0, 2));
            int month = int.Parse(idNumber.Substring(2, 2));
            int day = int.Parse(idNumber.Substring(4, 2));

            // Determine century (assume current century for years 00-current year, previous century otherwise)
            int currentYear = DateTime.Now.Year % 100;
            int century = year <= currentYear ? 2000 : 1900;
            int fullYear = century + year;

            // Validate date
            if (month < 1 || month > 12)
                return false;

            if (day < 1 || day > DateTime.DaysInMonth(fullYear, month))
                return false;

            dateOfBirth = new DateTime(fullYear, month, day);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Extracts the gender from the ID number.
    /// Digits 6 (index 6): 0-4 = Female, 5-9 = Male
    /// </summary>
    private static string ExtractGender(string idNumber)
    {
        int genderDigit = idNumber[6] - '0';
        return genderDigit < 5 ? "Female" : "Male";
    }

    /// <summary>
    /// Extracts citizenship status from the ID number.
    /// Digit 10 (index 10): 0 = SA citizen, 1 = Permanent resident
    /// </summary>
    private static bool ExtractCitizenship(string idNumber)
    {
        int citizenshipDigit = idNumber[10] - '0';
        return citizenshipDigit == 0;
    }

    /// <summary>
    /// Validates the Luhn checksum of the SA ID number.
    /// Same algorithm as credit cards and NBT numbers.
    /// </summary>
    private static bool ValidateLuhnChecksum(string idNumber)
    {
        if (idNumber.Length != SAIDLength)
            return false;

        int sum = 0;
        bool alternate = false;

        // Process from right to left
        for (int i = idNumber.Length - 1; i >= 0; i--)
        {
            int digit = idNumber[i] - '0';

            if (alternate)
            {
                digit *= 2;
                if (digit > 9)
                    digit -= 9;
            }

            sum += digit;
            alternate = !alternate;
        }

        return (sum % 10) == 0;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;

    public static implicit operator string(SAIDNumber saIdNumber) => saIdNumber.Value;
}
