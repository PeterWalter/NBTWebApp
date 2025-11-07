using NBT.Application.Common.Interfaces;

namespace NBT.Infrastructure.Services;

/// <summary>
/// File storage service implementation (local file system - can be replaced with Azure Blob Storage).
/// </summary>
public class FileStorageService : IFileStorageService
{
    private readonly string _baseStoragePath;

    public FileStorageService(string baseStoragePath = "uploads")
    {
        _baseStoragePath = baseStoragePath;
        
        // Ensure directory exists
        if (!Directory.Exists(_baseStoragePath))
        {
            Directory.CreateDirectory(_baseStoragePath);
        }
    }

    public async Task<string> UploadFileAsync(
        string fileName,
        Stream fileStream,
        string contentType,
        CancellationToken cancellationToken = default)
    {
        var safeFileName = Path.GetFileNameWithoutExtension(fileName);
        var extension = Path.GetExtension(fileName);
        var uniqueFileName = $"{safeFileName}_{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(_baseStoragePath, uniqueFileName);

        await using var fileStreamOutput = File.Create(filePath);
        await fileStream.CopyToAsync(fileStreamOutput, cancellationToken);

        return filePath;
    }

    public async Task<Stream> DownloadFileAsync(string filePath, CancellationToken cancellationToken = default)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"File not found: {filePath}");
        }

        var memoryStream = new MemoryStream();
        await using var fileStream = File.OpenRead(filePath);
        await fileStream.CopyToAsync(memoryStream, cancellationToken);
        memoryStream.Position = 0;

        return memoryStream;
    }

    public Task<bool> DeleteFileAsync(string filePath, CancellationToken cancellationToken = default)
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            return Task.FromResult(true);
        }

        return Task.FromResult(false);
    }
}
