using NBT.Domain.Common;
using NBT.Domain.Exceptions;

namespace NBT.Domain.ValueObjects;

/// <summary>
/// Represents a National Benchmark Test (NBT) number with Luhn algorithm validation.
/// Format: 9 digits (YYYYSSSSC where YYYY=year, SSSS=sequence, C=checksum)
/// Example: 202400015
/// </summary>
public sealed class NBTNumber : ValueObject
{
    private const int NBTNumberLength = 9;
    private const int YearLength = 4;
    private const int SequenceLength = 4;

    public string Value { get; }

    private NBTNumber(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Generates a new NBT number for the given year and sequence number.
    /// The checksum digit is automatically calculated using the Luhn algorithm.
    /// </summary>
    /// <param name="year">The year (e.g., 2024)</param>
    /// <param name="sequence">The sequence number (1-9999)</param>
    /// <returns>A valid NBT number</returns>
    /// <exception cref="DomainException">Thrown when year or sequence is invalid</exception>
    public static NBTNumber Generate(int year, int sequence)
    {
        if (year < 2000 || year > 9999)
            throw new DomainException($"Invalid year: {year}. Year must be between 2000 and 9999.");

        if (sequence < 1 || sequence > 9999)
            throw new DomainException($"Invalid sequence: {sequence}. Sequence must be between 1 and 9999.");

        // Format: YYYYSSSS (8 digits without checksum)
        string baseNumber = $"{year:D4}{sequence:D4}";

        // Calculate checksum digit
        int checksum = CalculateLuhnChecksum(baseNumber);

        // Complete NBT number: YYYYSSSSC
        string nbtNumber = baseNumber + checksum;

        return new NBTNumber(nbtNumber);
    }

    /// <summary>
    /// Creates an NBT number from an existing string value.
    /// Validates format and Luhn checksum.
    /// </summary>
    /// <param name="value">The 9-digit NBT number</param>
    /// <returns>A validated NBT number</returns>
    /// <exception cref="DomainException">Thrown when the NBT number is invalid</exception>
    public static NBTNumber Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("NBT number cannot be empty.");

        // Remove any whitespace
        value = value.Trim();

        if (!IsValid(value))
            throw new DomainException($"Invalid NBT number: {value}");

        return new NBTNumber(value);
    }

    /// <summary>
    /// Validates an NBT number string.
    /// Checks format (9 digits) and Luhn checksum.
    /// </summary>
    /// <param name="value">The NBT number to validate</param>
    /// <returns>True if valid, false otherwise</returns>
    public static bool IsValid(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;

        value = value.Trim();

        // Must be exactly 9 digits
        if (value.Length != NBTNumberLength)
            return false;

        // Must contain only digits
        if (!value.All(char.IsDigit))
            return false;

        // Validate Luhn checksum
        return ValidateLuhnChecksum(value);
    }

    /// <summary>
    /// Calculates the Luhn checksum digit for a given number.
    /// The Luhn algorithm (modulus 10):
    /// 1. Starting from the rightmost digit, double every second digit
    /// 2. If doubling results in a two-digit number, add the digits together
    /// 3. Sum all the digits
    /// 4. The checksum is (10 - (sum % 10)) % 10
    /// </summary>
    /// <param name="number">The number without checksum digit</param>
    /// <returns>The checksum digit (0-9)</returns>
    private static int CalculateLuhnChecksum(string number)
    {
        int sum = 0;
        bool alternate = true; // Start with alternating true because we'll append the checksum

        // Process from right to left
        for (int i = number.Length - 1; i >= 0; i--)
        {
            int digit = number[i] - '0';

            if (alternate)
            {
                digit *= 2;
                if (digit > 9)
                    digit -= 9; // Same as adding the two digits
            }

            sum += digit;
            alternate = !alternate;
        }

        // Calculate checksum digit
        return (10 - (sum % 10)) % 10;
    }

    /// <summary>
    /// Validates that the last digit of the NBT number is a valid Luhn checksum.
    /// </summary>
    /// <param name="nbtNumber">The complete 9-digit NBT number</param>
    /// <returns>True if checksum is valid, false otherwise</returns>
    private static bool ValidateLuhnChecksum(string nbtNumber)
    {
        if (nbtNumber.Length != NBTNumberLength)
            return false;

        // Extract the base number (first 8 digits) and the checksum (last digit)
        string baseNumber = nbtNumber.Substring(0, NBTNumberLength - 1);
        int providedChecksum = nbtNumber[NBTNumberLength - 1] - '0';

        // Calculate expected checksum
        int calculatedChecksum = CalculateLuhnChecksum(baseNumber);

        return providedChecksum == calculatedChecksum;
    }

    /// <summary>
    /// Extracts the year portion from the NBT number.
    /// </summary>
    public int Year => int.Parse(Value.Substring(0, YearLength));

    /// <summary>
    /// Extracts the sequence number from the NBT number.
    /// </summary>
    public int Sequence => int.Parse(Value.Substring(YearLength, SequenceLength));

    /// <summary>
    /// Extracts the checksum digit from the NBT number.
    /// </summary>
    public int Checksum => int.Parse(Value.Substring(NBTNumberLength - 1, 1));

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;

    public static implicit operator string(NBTNumber nbtNumber) => nbtNumber.Value;
}
