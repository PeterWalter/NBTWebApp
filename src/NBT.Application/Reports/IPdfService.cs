namespace NBT.Application.Reports;

public interface IPdfService
{
    Task<byte[]> GenerateRegistrationPdfAsync(Guid studentId, CancellationToken cancellationToken = default);
    Task<byte[]> GenerateInvoicePdfAsync(Guid paymentId, CancellationToken cancellationToken = default);
    Task<byte[]> GenerateResultPdfAsync(Guid resultId, CancellationToken cancellationToken = default);
}
