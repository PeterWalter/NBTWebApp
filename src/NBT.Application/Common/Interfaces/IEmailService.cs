namespace NBT.Application.Common.Interfaces;

/// <summary>
/// Interface for email service.
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Sends an email asynchronously.
    /// </summary>
    /// <param name="to">Recipient email address.</param>
    /// <param name="subject">Email subject.</param>
    /// <param name="body">Email body (HTML supported).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if email was sent successfully.</returns>
    Task<bool> SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends an email with attachments asynchronously.
    /// </summary>
    /// <param name="to">Recipient email address.</param>
    /// <param name="subject">Email subject.</param>
    /// <param name="body">Email body (HTML supported).</param>
    /// <param name="attachments">Dictionary of file names and byte arrays.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if email was sent successfully.</returns>
    Task<bool> SendEmailWithAttachmentsAsync(string to, string subject, string body, Dictionary<string, byte[]> attachments, CancellationToken cancellationToken = default);
}
