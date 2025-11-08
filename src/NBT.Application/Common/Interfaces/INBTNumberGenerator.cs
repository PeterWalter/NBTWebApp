namespace NBT.Application.Common.Interfaces;

/// <summary>
/// Service for generating unique NBT numbers with Luhn validation.
/// </summary>
public interface INBTNumberGenerator
{
    /// <summary>
    /// Generates a new unique NBT number.
    /// Format: YYYYSSSSC where YYYY=year, SSSS=sequence, C=checksum
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A valid 9-digit NBT number</returns>
    Task<string> GenerateAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the next available sequence number for the current year.
    /// </summary>
    /// <param name="year">The year to get sequence for</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The next sequence number</returns>
    Task<int> GetNextSequenceAsync(int year, CancellationToken cancellationToken = default);
}
