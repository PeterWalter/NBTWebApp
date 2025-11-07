namespace NBT.Application.Common.Interfaces;

/// <summary>
/// Interface for file storage service (Azure Blob Storage, local file system, etc.).
/// </summary>
public interface IFileStorageService
{
    /// <summary>
    /// Uploads a file asynchronously.
    /// </summary>
    /// <param name="fileName">File name.</param>
    /// <param name="fileStream">File stream.</param>
    /// <param name="contentType">Content type (e.g., application/pdf).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>URL or path to the uploaded file.</returns>
    Task<string> UploadFileAsync(string fileName, Stream fileStream, string contentType, CancellationToken cancellationToken = default);

    /// <summary>
    /// Downloads a file asynchronously.
    /// </summary>
    /// <param name="filePath">File path or URL.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>File stream.</returns>
    Task<Stream> DownloadFileAsync(string filePath, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a file asynchronously.
    /// </summary>
    /// <param name="filePath">File path or URL.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if file was deleted successfully.</returns>
    Task<bool> DeleteFileAsync(string filePath, CancellationToken cancellationToken = default);
}
