using Microsoft.EntityFrameworkCore;
using NBT.Infrastructure.Persistence;
using NBT.Application.Reports;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace NBT.Infrastructure.Services.Reports;

public class PdfService : IPdfService
{
    private readonly ApplicationDbContext _context;

    public PdfService(ApplicationDbContext context)
    {
        _context = context;
        QuestPDF.Settings.License = LicenseType.Community;
    }

    public async Task<byte[]> GenerateRegistrationPdfAsync(Guid studentId, CancellationToken cancellationToken = default)
    {
        var student = await _context.Students
            .FirstOrDefaultAsync(s => s.Id == studentId, cancellationToken);

        if (student == null)
            throw new InvalidOperationException("Student not found");

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(12));

                page.Header()
                    .Text("NBT Registration Confirmation")
                    .SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);

                page.Content()
                    .PaddingVertical(1, Unit.Centimetre)
                    .Column(x =>
                    {
                        x.Spacing(20);

                        x.Item().Text($"Registration Date: {student.CreatedDate:yyyy-MM-dd}");
                        x.Item().Text($"NBT Number: {student.NBTNumber}").SemiBold();

                        x.Item().LineHorizontal(1);

                        x.Item().Text("Personal Information").FontSize(16).SemiBold();
                        x.Item().Text($"Name: {student.FirstName} {student.LastName}");
                        x.Item().Text($"ID Number: {student.IDNumber}");
                        x.Item().Text($"Date of Birth: {student.DateOfBirth:yyyy-MM-dd}");
                        x.Item().Text($"Gender: {student.Gender}");

                        x.Item().LineHorizontal(1);

                        x.Item().Text("Contact Information").FontSize(16).SemiBold();
                        x.Item().Text($"Email: {student.Email}");
                        x.Item().Text($"Phone: {student.Phone}");
                        x.Item().Text($"Address: {student.Address}");

                        x.Item().LineHorizontal(1);

                        x.Item().Text("Academic Information").FontSize(16).SemiBold();
                        x.Item().Text($"School: {student.SchoolName}");
                        x.Item().Text($"Grade: {student.Grade}");
                        x.Item().Text($"Home Language: {student.HomeLanguage}");

                        x.Item().LineHorizontal(1);

                        x.Item().Text("Please keep this confirmation for your records.")
                            .FontSize(10).Italic().FontColor(Colors.Grey.Darken2);
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Page ");
                        x.CurrentPageNumber();
                        x.Span(" of ");
                        x.TotalPages();
                    });
            });
        });

        return document.GeneratePdf();
    }

    public async Task<byte[]> GenerateInvoicePdfAsync(Guid paymentId, CancellationToken cancellationToken = default)
    {
        var payment = await _context.Payments
            .Include(p => p.Registration)
                .ThenInclude(r => r.Student)
            .FirstOrDefaultAsync(p => p.Id == paymentId, cancellationToken);

        if (payment == null)
            throw new InvalidOperationException("Payment not found");

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(12));

                page.Header()
                    .Column(column =>
                    {
                        column.Item().Text("INVOICE").SemiBold().FontSize(24).FontColor(Colors.Blue.Medium);
                        column.Item().Text("National Benchmark Tests").FontSize(14);
                    });

                page.Content()
                    .PaddingVertical(1, Unit.Centimetre)
                    .Column(x =>
                    {
                        x.Spacing(20);

                        x.Item().Text($"Invoice Date: {payment.PaidDate:yyyy-MM-dd}");
                        x.Item().Text($"Invoice Number: {payment.InvoiceNumber}").SemiBold();

                        x.Item().LineHorizontal(1);

                        x.Item().Text("Bill To:").FontSize(16).SemiBold();
                        x.Item().Text($"{payment.Registration?.Student?.FirstName} {payment.Registration?.Student?.LastName}");
                        x.Item().Text($"NBT Number: {payment.Registration?.Student?.NBTNumber}");
                        x.Item().Text($"Email: {payment.Registration?.Student?.Email}");

                        x.Item().LineHorizontal(1);

                        x.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(3);
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(1);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("Description");
                                header.Cell().Element(CellStyle).AlignRight().Text("Qty");
                                header.Cell().Element(CellStyle).AlignRight().Text("Amount");

                                static IContainer CellStyle(IContainer container)
                                {
                                    return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                                }
                            });

                            table.Cell().Element(CellStyle).Text("NBT Test Fee");
                            table.Cell().Element(CellStyle).AlignRight().Text("1");
                            table.Cell().Element(CellStyle).AlignRight().Text($"R {payment.TotalAmount:N2}");

                            static IContainer CellStyle(IContainer container)
                            {
                                return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
                            }
                        });

                        x.Item().AlignRight().Text($"Total: R {payment.TotalAmount:N2}").FontSize(16).SemiBold();
                        x.Item().AlignRight().Text($"Amount Paid: R {payment.AmountPaid:N2}").FontSize(14);
                        if (payment.Balance > 0)
                        {
                            x.Item().AlignRight().Text($"Balance: R {payment.Balance:N2}").FontSize(14).FontColor(Colors.Red.Medium);
                        }
                        x.Item().AlignRight().Text($"Status: {payment.Status}").FontColor(
                            payment.Status == NBT.Domain.Enums.PaymentStatus.Paid ? Colors.Green.Medium : Colors.Orange.Medium);

                        if (!string.IsNullOrEmpty(payment.EasyPayTransactionId))
                        {
                            x.Item().LineHorizontal(1);
                            x.Item().Text($"Transaction ID: {payment.EasyPayTransactionId}").FontSize(10);
                        }

                        x.Item().PaddingTop(20).Text("Thank you for your payment!")
                            .FontSize(10).Italic().FontColor(Colors.Grey.Darken2);
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Page ");
                        x.CurrentPageNumber();
                    });
            });
        });

        return document.GeneratePdf();
    }

    public async Task<byte[]> GenerateResultPdfAsync(Guid resultId, CancellationToken cancellationToken = default)
    {
        var result = await _context.TestResults
            .Include(r => r.Student)
            .Include(r => r.TestSession)
            .FirstOrDefaultAsync(r => r.Id == resultId, cancellationToken);

        if (result == null)
            throw new InvalidOperationException("Result not found");

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(12));

                page.Header()
                    .Column(column =>
                    {
                        column.Item().Text("NBT TEST RESULTS").SemiBold().FontSize(24).FontColor(Colors.Blue.Medium);
                        column.Item().Text("National Benchmark Tests").FontSize(14);
                    });

                page.Content()
                    .PaddingVertical(1, Unit.Centimetre)
                    .Column(x =>
                    {
                        x.Spacing(20);

                        x.Item().Text($"Test Date: {result.TestDate:yyyy-MM-dd}");
                        x.Item().Text($"Release Date: {result.ReleasedDate:yyyy-MM-dd}");

                        x.Item().LineHorizontal(1);

                        x.Item().Text("Student Information").FontSize(16).SemiBold();
                        x.Item().Text($"Name: {result.Student?.FirstName} {result.Student?.LastName}");
                        x.Item().Text($"NBT Number: {result.Student?.NBTNumber}");

                        x.Item().LineHorizontal(1);

                        x.Item().Text("Test Results").FontSize(16).SemiBold();
                        x.Item().Text($"Test Type: {result.TestType}");
                        x.Item().Text($"Barcode: {result.Barcode}").FontSize(12);
                        
                        if (result.ALScore.HasValue)
                        {
                            x.Item().Text($"Academic Literacy (AL): {result.ALScore:N2} - {result.ALPerformanceLevel}").FontSize(14).SemiBold();
                        }
                        
                        if (result.QLScore.HasValue)
                        {
                            x.Item().Text($"Quantitative Literacy (QL): {result.QLScore:N2} - {result.QLPerformanceLevel}").FontSize(14).SemiBold();
                        }
                        
                        if (result.MATScore.HasValue)
                        {
                            x.Item().Text($"Mathematics (MAT): {result.MATScore:N2} - {result.MATPerformanceLevel}").FontSize(14).SemiBold();
                        }
                        
                        x.Item().Text($"Percentile: {result.Percentile}").FontSize(14).SemiBold();
                        x.Item().Text($"Overall Performance: {result.OverallPerformanceBand}").FontSize(14).SemiBold();

                        x.Item().LineHorizontal(1);

                        x.Item().Text("Important Notes:").FontSize(14).SemiBold();
                        x.Item().Text("• These results are valid for 3 years from the test date.");
                        x.Item().Text("• Results are confidential and should not be shared publicly.");
                        x.Item().Text("• For queries, please contact the NBT office.");

                        x.Item().PaddingTop(20).Text("This is an official NBT result certificate.")
                            .FontSize(10).Italic().FontColor(Colors.Grey.Darken2);
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Generated on ");
                        x.Span(DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                    });
            });
        });

        return document.GeneratePdf();
    }
}
