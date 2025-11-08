namespace NBT.Domain.Common;

/// <summary>
/// Implementation of Luhn algorithm for NBT number and ID validation.
/// </summary>
public class LuhnValidator : ILuhnValidator
{
    public string GenerateNBTNumber(int year, int sequence)
    {
        if (year < 2000 || year > 9999)
            throw new ArgumentException("Year must be between 2000 and 9999", nameof(year));
        
        if (sequence < 0 || sequence > 9999)
            throw new ArgumentException("Sequence must be between 0 and 9999", nameof(sequence));
        
        // Format: YYYYSSSS + check digit
        var baseNumber = $"{year:D4}{sequence:D4}";
        var checkDigit = CalculateCheckDigit(baseNumber);
        
        return $"{baseNumber}{checkDigit}";
    }
    
    public bool ValidateNBTNumber(string nbtNumber)
    {
        if (string.IsNullOrWhiteSpace(nbtNumber))
            return false;
        
        if (nbtNumber.Length != 9)
            return false;
        
        if (!nbtNumber.All(char.IsDigit))
            return false;
        
        return ValidateLuhnChecksum(nbtNumber);
    }
    
    public bool ValidateSouthAfricanID(string idNumber)
    {
        if (string.IsNullOrWhiteSpace(idNumber))
            return false;
        
        if (idNumber.Length != 13)
            return false;
        
        if (!idNumber.All(char.IsDigit))
            return false;
        
        // Validate date portion (YYMMDD)
        if (!ValidateDatePortion(idNumber))
            return false;
        
        // Validate Luhn checksum
        return ValidateLuhnChecksum(idNumber);
    }
    
    public int CalculateCheckDigit(string number)
    {
        if (string.IsNullOrWhiteSpace(number))
            throw new ArgumentException("Number cannot be null or empty", nameof(number));
        
        if (!number.All(char.IsDigit))
            throw new ArgumentException("Number must contain only digits", nameof(number));
        
        var sum = 0;
        var alternate = false;
        
        // Process digits from right to left
        for (int i = number.Length - 1; i >= 0; i--)
        {
            var digit = int.Parse(number[i].ToString());
            
            if (alternate)
            {
                digit *= 2;
                if (digit > 9)
                    digit -= 9;
            }
            
            sum += digit;
            alternate = !alternate;
        }
        
        // Calculate check digit
        var checkDigit = (10 - (sum % 10)) % 10;
        return checkDigit;
    }
    
    private bool ValidateLuhnChecksum(string number)
    {
        var baseNumber = number[..^1]; // All digits except last
        var providedCheckDigit = int.Parse(number[^1].ToString());
        var calculatedCheckDigit = CalculateCheckDigit(baseNumber);
        
        return providedCheckDigit == calculatedCheckDigit;
    }
    
    private bool ValidateDatePortion(string idNumber)
    {
        try
        {
            var year = int.Parse(idNumber[..2]);
            var month = int.Parse(idNumber.Substring(2, 2));
            var day = int.Parse(idNumber.Substring(4, 2));
            
            // Validate month
            if (month < 1 || month > 12)
                return false;
            
            // Validate day
            if (day < 1 || day > 31)
                return false;
            
            // Determine century
            var currentYear = DateTime.Now.Year % 100;
            var fullYear = year <= currentYear + 5 ? 2000 + year : 1900 + year;
            
            // Try to create date to validate
            var date = new DateTime(fullYear, month, day);
            
            return true;
        }
        catch
        {
            return false;
        }
    }
}
