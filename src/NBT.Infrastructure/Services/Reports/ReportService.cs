using Microsoft.EntityFrameworkCore;
using NBT.Infrastructure.Persistence;
using NBT.Application.Reports;
using ClosedXML.Excel;

namespace NBT.Infrastructure.Services.Reports;

public class ReportService : IReportService
{
    private readonly ApplicationDbContext _context;

    public ReportService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<byte[]> GenerateRegistrationReportAsync(DateTime? startDate = null, DateTime? endDate = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Students.AsQueryable();

        if (startDate.HasValue)
            query = query.Where(s => s.CreatedDate >= startDate.Value);
        if (endDate.HasValue)
            query = query.Where(s => s.CreatedDate <= endDate.Value);

        var students = await query
            .OrderByDescending(s => s.CreatedDate)
            .ToListAsync(cancellationToken);

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Registrations");

        // Headers
        worksheet.Cell(1, 1).Value = "NBT Number";
        worksheet.Cell(1, 2).Value = "ID Number";
        worksheet.Cell(1, 3).Value = "First Name";
        worksheet.Cell(1, 4).Value = "Last Name";
        worksheet.Cell(1, 5).Value = "Email";
        worksheet.Cell(1, 6).Value = "Mobile";
        worksheet.Cell(1, 7).Value = "Date of Birth";
        worksheet.Cell(1, 8).Value = "Gender";
        worksheet.Cell(1, 9).Value = "Ethnicity";
        worksheet.Cell(1, 10).Value = "Home Language";
        worksheet.Cell(1, 11).Value = "School";
        worksheet.Cell(1, 12).Value = "Grade";
        worksheet.Cell(1, 13).Value = "Registration Date";

        // Style headers
        var headerRange = worksheet.Range(1, 1, 1, 13);
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.BackgroundColor = XLColor.LightBlue;
        headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

        // Data
        int row = 2;
        foreach (var student in students)
        {
            worksheet.Cell(row, 1).Value = student.NBTNumber;
            worksheet.Cell(row, 2).Value = student.IDNumber;
            worksheet.Cell(row, 3).Value = student.FirstName;
            worksheet.Cell(row, 4).Value = student.LastName;
            worksheet.Cell(row, 5).Value = student.Email;
            worksheet.Cell(row, 6).Value = student.Phone;
            worksheet.Cell(row, 7).Value = student.DateOfBirth.ToString("yyyy-MM-dd");
            worksheet.Cell(row, 8).Value = student.Gender;
            worksheet.Cell(row, 9).Value = student.Ethnicity;
            worksheet.Cell(row, 10).Value = student.HomeLanguage;
            worksheet.Cell(row, 11).Value = student.SchoolName;
            worksheet.Cell(row, 12).Value = student.Grade?.ToString();
            worksheet.Cell(row, 13).Value = student.CreatedDate.ToString("yyyy-MM-dd HH:mm");
            row++;
        }

        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }

    public async Task<byte[]> GeneratePaymentReportAsync(DateTime? startDate = null, DateTime? endDate = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Payments
            .Include(p => p.Registration)
                .ThenInclude(r => r.Student)
            .AsQueryable();

        if (startDate.HasValue)
            query = query.Where(p => p.PaidDate >= startDate.Value);
        if (endDate.HasValue)
            query = query.Where(p => p.PaidDate <= endDate.Value);

        var payments = await query
            .OrderByDescending(p => p.PaidDate)
            .ToListAsync(cancellationToken);

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Payments");

        // Headers
        worksheet.Cell(1, 1).Value = "Invoice Number";
        worksheet.Cell(1, 2).Value = "NBT Number";
        worksheet.Cell(1, 3).Value = "Student Name";
        worksheet.Cell(1, 4).Value = "Amount";
        worksheet.Cell(1, 5).Value = "Status";
        worksheet.Cell(1, 6).Value = "Payment Method";
        worksheet.Cell(1, 7).Value = "Payment Date";
        worksheet.Cell(1, 8).Value = "EasyPay Reference";

        // Style headers
        var headerRange = worksheet.Range(1, 1, 1, 8);
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.BackgroundColor = XLColor.LightGreen;
        headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

        // Data
        int row = 2;
        foreach (var payment in payments)
        {
            worksheet.Cell(row, 1).Value = payment.InvoiceNumber;
            worksheet.Cell(row, 2).Value = payment.Registration?.Student?.NBTNumber;
            worksheet.Cell(row, 3).Value = $"{payment.Registration?.Student?.FirstName} {payment.Registration?.Student?.LastName}";
            worksheet.Cell(row, 4).Value = payment.Amount;
            worksheet.Cell(row, 5).Value = payment.Status.ToString();
            worksheet.Cell(row, 6).Value = payment.PaymentMethod;
            worksheet.Cell(row, 7).Value = payment.PaidDate?.ToString("yyyy-MM-dd HH:mm");
            worksheet.Cell(row, 8).Value = payment.EasyPayReference;
            row++;
        }

        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }

    public async Task<byte[]> GenerateResultsReportAsync(DateTime? startDate = null, DateTime? endDate = null, CancellationToken cancellationToken = default)
    {
        var query = _context.TestResults
            .Include(r => r.Student)
            .Include(r => r.TestSession)
            .AsQueryable();

        if (startDate.HasValue)
            query = query.Where(r => r.TestDate >= startDate.Value);
        if (endDate.HasValue)
            query = query.Where(r => r.TestDate <= endDate.Value);

        var results = await query
            .OrderByDescending(r => r.TestDate)
            .ToListAsync(cancellationToken);

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Results");

        // Headers
        worksheet.Cell(1, 1).Value = "NBT Number";
        worksheet.Cell(1, 2).Value = "Student Name";
        worksheet.Cell(1, 3).Value = "Test Type";
        worksheet.Cell(1, 4).Value = "Test Date";
        worksheet.Cell(1, 5).Value = "Raw Score";
        worksheet.Cell(1, 6).Value = "Percentile";
        worksheet.Cell(1, 7).Value = "Performance Band";
        worksheet.Cell(1, 8).Value = "Released";
        worksheet.Cell(1, 9).Value = "Session";

        // Style headers
        var headerRange = worksheet.Range(1, 1, 1, 9);
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.BackgroundColor = XLColor.LightYellow;
        headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

        // Data
        int row = 2;
        foreach (var result in results)
        {
            worksheet.Cell(row, 1).Value = result.Student?.NBTNumber;
            worksheet.Cell(row, 2).Value = $"{result.Student?.FirstName} {result.Student?.LastName}";
            worksheet.Cell(row, 3).Value = result.TestType;
            worksheet.Cell(row, 4).Value = result.TestDate.ToString("yyyy-MM-dd");
            worksheet.Cell(row, 5).Value = result.RawScore;
            worksheet.Cell(row, 6).Value = result.Percentile;
            worksheet.Cell(row, 7).Value = result.PerformanceBand;
            worksheet.Cell(row, 8).Value = result.IsReleased ? "Yes" : "No";
            worksheet.Cell(row, 9).Value = result.TestSession?.SessionName;
            row++;
        }

        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }

    public async Task<byte[]> GenerateSessionUtilizationReportAsync(DateTime? startDate = null, DateTime? endDate = null, CancellationToken cancellationToken = default)
    {
        var query = _context.TestSessions
            .Include(s => s.Venue)
            .Include(s => s.Registrations)
            .AsQueryable();

        if (startDate.HasValue)
            query = query.Where(s => s.SessionDate >= startDate.Value);
        if (endDate.HasValue)
            query = query.Where(s => s.SessionDate <= endDate.Value);

        var sessions = await query
            .OrderBy(s => s.SessionDate)
            .ToListAsync(cancellationToken);

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Session Utilization");

        // Headers
        worksheet.Cell(1, 1).Value = "Session Name";
        worksheet.Cell(1, 2).Value = "Session Date";
        worksheet.Cell(1, 3).Value = "Venue";
        worksheet.Cell(1, 4).Value = "Capacity";
        worksheet.Cell(1, 5).Value = "Registrations";
        worksheet.Cell(1, 6).Value = "Available";
        worksheet.Cell(1, 7).Value = "Utilization %";
        worksheet.Cell(1, 8).Value = "Status";

        // Style headers
        var headerRange = worksheet.Range(1, 1, 1, 8);
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.BackgroundColor = XLColor.LightCyan;
        headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

        // Data
        int row = 2;
        foreach (var session in sessions)
        {
            var registrations = session.Registrations?.Count ?? 0;
            var capacity = session.Capacity;
            var utilization = capacity > 0 ? (double)registrations / capacity * 100 : 0;

            worksheet.Cell(row, 1).Value = session.SessionName;
            worksheet.Cell(row, 2).Value = session.SessionDate.ToString("yyyy-MM-dd HH:mm");
            worksheet.Cell(row, 3).Value = session.Venue?.VenueName;
            worksheet.Cell(row, 4).Value = capacity;
            worksheet.Cell(row, 5).Value = registrations;
            worksheet.Cell(row, 6).Value = capacity - registrations;
            worksheet.Cell(row, 7).Value = utilization.ToString("F2") + "%";
            worksheet.Cell(row, 8).Value = session.Status.ToString();
            row++;
        }

        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }

    public async Task<DashboardSummaryDto> GenerateDashboardSummaryAsync(CancellationToken cancellationToken = default)
    {
        var summary = new DashboardSummaryDto();

        // Total registrations
        summary.TotalRegistrations = await _context.Students.CountAsync(cancellationToken);

        // Payment stats
        var payments = await _context.Payments.ToListAsync(cancellationToken);
        summary.TotalPayments = payments.Count;
        summary.TotalRevenue = payments.Where(p => p.Status == NBT.Domain.Enums.PaymentStatus.Paid).Sum(p => p.Amount);
        summary.PendingPayments = payments.Count(p => p.Status == NBT.Domain.Enums.PaymentStatus.Pending);
        summary.CompletedPayments = payments.Count(p => p.Status == NBT.Domain.Enums.PaymentStatus.Paid);
        summary.FailedPayments = payments.Count(p => p.Status == NBT.Domain.Enums.PaymentStatus.Failed);

        // Results stats
        var results = await _context.TestResults.ToListAsync(cancellationToken);
        summary.TotalResults = results.Count;
        summary.ReleasedResults = results.Count(r => r.IsReleased);
        summary.PendingResults = results.Count(r => !r.IsReleased);

        // Registration trends (last 30 days)
        var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);
        summary.RegistrationTrends = await _context.Students
            .Where(s => s.CreatedDate >= thirtyDaysAgo)
            .GroupBy(s => s.CreatedDate.Date)
            .Select(g => new RegistrationTrendDto
            {
                Date = g.Key,
                Count = g.Count()
            })
            .OrderBy(r => r.Date)
            .ToListAsync(cancellationToken);

        // Payment status breakdown
        summary.PaymentStatusBreakdown = await _context.Payments
            .GroupBy(p => p.Status)
            .Select(g => new PaymentStatusDto
            {
                Status = g.Key.ToString(),
                Count = g.Count(),
                TotalAmount = g.Sum(p => p.Amount)
            })
            .ToListAsync(cancellationToken);

        // Session utilization
        summary.SessionUtilization = await _context.TestSessions
            .Include(s => s.Registrations)
            .Include(s => s.Venue)
            .Where(s => s.SessionDate >= DateTime.UtcNow)
            .Select(s => new SessionUtilizationDto
            {
                SessionName = s.SessionName,
                SessionDate = s.SessionDate,
                Capacity = s.Capacity,
                Bookings = s.Registrations!.Count,
                UtilizationPercentage = s.Capacity > 0 ? (double)s.Registrations!.Count / s.Capacity * 100 : 0
            })
            .OrderBy(s => s.SessionDate)
            .Take(10)
            .ToListAsync(cancellationToken);

        // Test type distribution
        summary.TestTypeDistribution = await _context.TestResults
            .GroupBy(r => r.TestType)
            .Select(g => new TestTypeDistributionDto
            {
                TestType = g.Key,
                Count = g.Count()
            })
            .ToListAsync(cancellationToken);

        return summary;
    }
}
