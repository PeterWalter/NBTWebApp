namespace NBT.Application.Reports;

public interface IReportService
{
    Task<byte[]> GenerateRegistrationReportAsync(DateTime? startDate = null, DateTime? endDate = null, CancellationToken cancellationToken = default);
    Task<byte[]> GeneratePaymentReportAsync(DateTime? startDate = null, DateTime? endDate = null, CancellationToken cancellationToken = default);
    Task<byte[]> GenerateResultsReportAsync(DateTime? startDate = null, DateTime? endDate = null, CancellationToken cancellationToken = default);
    Task<byte[]> GenerateSessionUtilizationReportAsync(DateTime? startDate = null, DateTime? endDate = null, CancellationToken cancellationToken = default);
    Task<DashboardSummaryDto> GenerateDashboardSummaryAsync(CancellationToken cancellationToken = default);
}

public class DashboardSummaryDto
{
    public int TotalRegistrations { get; set; }
    public int TotalPayments { get; set; }
    public decimal TotalRevenue { get; set; }
    public int PendingPayments { get; set; }
    public int CompletedPayments { get; set; }
    public int FailedPayments { get; set; }
    public int TotalResults { get; set; }
    public int ReleasedResults { get; set; }
    public int PendingResults { get; set; }
    public List<RegistrationTrendDto> RegistrationTrends { get; set; } = new();
    public List<PaymentStatusDto> PaymentStatusBreakdown { get; set; } = new();
    public List<SessionUtilizationDto> SessionUtilization { get; set; } = new();
    public List<TestTypeDistributionDto> TestTypeDistribution { get; set; } = new();
}

public class RegistrationTrendDto
{
    public DateTime Date { get; set; }
    public int Count { get; set; }
}

public class PaymentStatusDto
{
    public string Status { get; set; } = string.Empty;
    public int Count { get; set; }
    public decimal TotalAmount { get; set; }
}

public class SessionUtilizationDto
{
    public string SessionName { get; set; } = string.Empty;
    public DateTime SessionDate { get; set; }
    public int Capacity { get; set; }
    public int Bookings { get; set; }
    public double UtilizationPercentage { get; set; }
}

public class TestTypeDistributionDto
{
    public string TestType { get; set; } = string.Empty;
    public int Count { get; set; }
}
