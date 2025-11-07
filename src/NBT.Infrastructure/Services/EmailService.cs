using NBT.Application.Common.Interfaces;

namespace NBT.Infrastructure.Services;

/// <summary>
/// Email service implementation (placeholder - to be implemented with SMTP/SendGrid).
/// </summary>
public class EmailService : IEmailService
{
    public async Task<bool> SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
    {
        // TODO: Implement actual email sending logic
        // Options: SMTP, SendGrid, Azure Communication Services
        await Task.CompletedTask;
        
        // For development, just log
        Console.WriteLine($"Email sent to {to}: {subject}");
        return true;
    }

    public async Task<bool> SendEmailWithAttachmentsAsync(
        string to,
        string subject,
        string body,
        Dictionary<string, byte[]> attachments,
        CancellationToken cancellationToken = default)
    {
        // TODO: Implement actual email sending with attachments
        await Task.CompletedTask;
        
        Console.WriteLine($"Email with {attachments.Count} attachments sent to {to}: {subject}");
        return true;
    }
}
