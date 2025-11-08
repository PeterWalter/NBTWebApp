using NBT.Domain.Common;
using NBT.Domain.Exceptions;

namespace NBT.Domain.ValueObjects;

/// <summary>
/// Represents a Foreign ID or Passport number for international students.
/// Format: 6-20 alphanumeric characters (uppercase letters and numbers only)
/// Examples: A1234567, XY9876543, PASS123456
/// </summary>
public sealed class ForeignIDNumber : ValueObject
{
    private const int MinLength = 6;
    private const int MaxLength = 20;

    public string Value { get; }

    private ForeignIDNumber(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a Foreign ID number from a string value.
    /// Validates format and length.
    /// </summary>
    /// <param name="value">The Foreign ID or Passport number</param>
    /// <returns>A validated Foreign ID number</returns>
    /// <exception cref="DomainException">Thrown when the ID number is invalid</exception>
    public static ForeignIDNumber Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Foreign ID or Passport number cannot be empty.");

        // Remove any whitespace and convert to uppercase
        value = value.Trim().ToUpperInvariant();

        if (!IsValid(value, out string? errorMessage))
            throw new DomainException(errorMessage ?? $"Invalid Foreign ID or Passport number: {value}");

        return new ForeignIDNumber(value);
    }

    /// <summary>
    /// Validates a Foreign ID or Passport number.
    /// </summary>
    /// <param name="value">The ID number to validate</param>
    /// <param name="errorMessage">Error message if invalid</param>
    /// <returns>True if valid, false otherwise</returns>
    public static bool IsValid(string? value, out string? errorMessage)
    {
        errorMessage = null;

        if (string.IsNullOrWhiteSpace(value))
        {
            errorMessage = "Foreign ID or Passport number cannot be empty.";
            return false;
        }

        value = value.Trim().ToUpperInvariant();

        // Must be between 6 and 20 characters
        if (value.Length < MinLength || value.Length > MaxLength)
        {
            errorMessage = $"Foreign ID or Passport number must be between {MinLength} and {MaxLength} characters.";
            return false;
        }

        // Must contain only uppercase letters and numbers
        if (!value.All(c => char.IsLetterOrDigit(c) && (char.IsDigit(c) || char.IsUpper(c))))
        {
            errorMessage = "Foreign ID or Passport number must contain only uppercase letters and numbers.";
            return false;
        }

        return true;
    }

    /// <summary>
    /// Simple overload for quick validation without error message.
    /// </summary>
    public static bool IsValid(string? value)
    {
        return IsValid(value, out _);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;

    public static implicit operator string(ForeignIDNumber foreignIdNumber) => foreignIdNumber.Value;
}
