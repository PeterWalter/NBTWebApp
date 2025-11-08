using Microsoft.EntityFrameworkCore;
using NBT.Application.Common.Interfaces;
using NBT.Domain.ValueObjects;

namespace NBT.Infrastructure.Services;

/// <summary>
/// Generates unique NBT numbers with Luhn algorithm validation.
/// Thread-safe implementation using database sequence tracking.
/// </summary>
public class NBTNumberGenerator : INBTNumberGenerator
{
    private readonly IApplicationDbContext _context;
    private static readonly SemaphoreSlim _semaphore = new(1, 1);

    public NBTNumberGenerator(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<string> GenerateAsync(CancellationToken cancellationToken = default)
    {
        await _semaphore.WaitAsync(cancellationToken);
        try
        {
            int currentYear = DateTime.UtcNow.Year;
            int nextSequence = await GetNextSequenceAsync(currentYear, cancellationToken);

            var nbtNumber = NBTNumber.Generate(currentYear, nextSequence);
            
            return nbtNumber.Value;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<int> GetNextSequenceAsync(int year, CancellationToken cancellationToken = default)
    {
        var yearPrefix = year.ToString();

        var lastStudent = await _context.Students
            .Where(s => s.NBTNumber.StartsWith(yearPrefix))
            .OrderByDescending(s => s.NBTNumber)
            .FirstOrDefaultAsync(cancellationToken);

        if (lastStudent == null)
        {
            return 1;
        }

        var lastNbtNumber = NBTNumber.Create(lastStudent.NBTNumber);
        return lastNbtNumber.Sequence + 1;
    }
}
