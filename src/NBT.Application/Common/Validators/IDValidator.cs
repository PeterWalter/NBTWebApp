using NBT.Domain.Enums;
using NBT.Domain.ValueObjects;

namespace NBT.Application.Common.Validators;

/// <summary>
/// Validates identification numbers based on ID type.
/// Supports SA ID, Foreign ID, and Passport validation.
/// </summary>
public static class IDValidator
{
    /// <summary>
    /// Validates an ID number based on the specified ID type.
    /// </summary>
    /// <param name="idType">The type of ID (SA_ID, FOREIGN_ID, or PASSPORT)</param>
    /// <param name="idNumber">The ID number to validate</param>
    /// <param name="errorMessage">Error message if validation fails</param>
    /// <returns>True if valid, false otherwise</returns>
    public static bool Validate(IDType idType, string idNumber, out string? errorMessage)
    {
        if (string.IsNullOrWhiteSpace(idNumber))
        {
            errorMessage = "ID number cannot be empty.";
            return false;
        }

        try
        {
            switch (idType)
            {
                case IDType.SA_ID:
                    return ValidateSAID(idNumber, out errorMessage);

                case IDType.FOREIGN_ID:
                case IDType.PASSPORT:
                    return ValidateForeignID(idNumber, out errorMessage);

                default:
                    errorMessage = $"Unsupported ID type: {idType}";
                    return false;
            }
        }
        catch (Exception ex)
        {
            errorMessage = $"Validation error: {ex.Message}";
            return false;
        }
    }

    /// <summary>
    /// Validates a South African ID number.
    /// </summary>
    private static bool ValidateSAID(string idNumber, out string? errorMessage)
    {
        if (!SAIDNumber.IsValid(idNumber, out _, out _, out _, out errorMessage))
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Validates a Foreign ID or Passport number.
    /// </summary>
    private static bool ValidateForeignID(string idNumber, out string? errorMessage)
    {
        if (!ForeignIDNumber.IsValid(idNumber, out errorMessage))
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Extracts date of birth from a South African ID number.
    /// Returns null if ID type is not SA_ID or if extraction fails.
    /// </summary>
    public static DateTime? ExtractDateOfBirth(IDType idType, string idNumber)
    {
        if (idType != IDType.SA_ID)
        {
            return null;
        }

        try
        {
            if (SAIDNumber.IsValid(idNumber, out DateTime dateOfBirth, out _, out _, out _))
            {
                return dateOfBirth;
            }
        }
        catch
        {
            // Ignore extraction errors
        }

        return null;
    }

    /// <summary>
    /// Extracts gender from a South African ID number.
    /// Returns null if ID type is not SA_ID or if extraction fails.
    /// </summary>
    public static string? ExtractGender(IDType idType, string idNumber)
    {
        if (idType != IDType.SA_ID)
        {
            return null;
        }

        try
        {
            if (SAIDNumber.IsValid(idNumber, out _, out string gender, out _, out _))
            {
                return gender;
            }
        }
        catch
        {
            // Ignore extraction errors
        }

        return null;
    }
}
