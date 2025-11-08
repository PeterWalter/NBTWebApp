namespace NBT.Domain.Common;

/// <summary>
/// Interface for Luhn algorithm validation and generation.
/// Used for NBT number and ID number validation.
/// </summary>
public interface ILuhnValidator
{
    /// <summary>
    /// Generates a valid 9-digit NBT number using Luhn checksum.
    /// Format: YYYYSSSSC (Year + Sequence + Check digit)
    /// </summary>
    /// <param name="year">Registration year (4 digits)</param>
    /// <param name="sequence">Sequential number (4 digits)</param>
    /// <returns>9-digit NBT number with Luhn check digit</returns>
    string GenerateNBTNumber(int year, int sequence);
    
    /// <summary>
    /// Validates NBT number using Luhn algorithm.
    /// </summary>
    /// <param name="nbtNumber">9-digit NBT number to validate</param>
    /// <returns>True if valid, false otherwise</returns>
    bool ValidateNBTNumber(string nbtNumber);
    
    /// <summary>
    /// Validates South African ID number using Luhn algorithm.
    /// </summary>
    /// <param name="idNumber">13-digit SA ID number</param>
    /// <returns>True if valid, false otherwise</returns>
    bool ValidateSouthAfricanID(string idNumber);
    
    /// <summary>
    /// Calculates Luhn check digit for given number string.
    /// </summary>
    /// <param name="number">Number string without check digit</param>
    /// <returns>Check digit (0-9)</returns>
    int CalculateCheckDigit(string number);
}
