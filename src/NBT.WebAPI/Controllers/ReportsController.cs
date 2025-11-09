using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NBT.Application.Reports;

namespace NBT.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,Staff")]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;
    private readonly IPdfService _pdfService;

    public ReportsController(IReportService reportService, IPdfService pdfService)
    {
        _reportService = reportService;
        _pdfService = pdfService;
    }

    [HttpGet("registrations")]
    public async Task<IActionResult> GetRegistrationReport([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
    {
        try
        {
            var fileBytes = await _reportService.GenerateRegistrationReportAsync(startDate, endDate);
            var fileName = $"Registrations_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error generating report", error = ex.Message });
        }
    }

    [HttpGet("payments")]
    public async Task<IActionResult> GetPaymentReport([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
    {
        try
        {
            var fileBytes = await _reportService.GeneratePaymentReportAsync(startDate, endDate);
            var fileName = $"Payments_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error generating report", error = ex.Message });
        }
    }

    [HttpGet("results")]
    public async Task<IActionResult> GetResultsReport([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
    {
        try
        {
            var fileBytes = await _reportService.GenerateResultsReportAsync(startDate, endDate);
            var fileName = $"Results_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error generating report", error = ex.Message });
        }
    }

    [HttpGet("sessions")]
    public async Task<IActionResult> GetSessionUtilizationReport([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
    {
        try
        {
            var fileBytes = await _reportService.GenerateSessionUtilizationReportAsync(startDate, endDate);
            var fileName = $"SessionUtilization_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error generating report", error = ex.Message });
        }
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetDashboardSummary()
    {
        try
        {
            var summary = await _reportService.GenerateDashboardSummaryAsync();
            return Ok(summary);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error generating summary", error = ex.Message });
        }
    }

    [HttpGet("pdf/registration/{studentId}")]
    public async Task<IActionResult> GetRegistrationPdf(Guid studentId)
    {
        try
        {
            var fileBytes = await _pdfService.GenerateRegistrationPdfAsync(studentId);
            var fileName = $"Registration_{studentId}_{DateTime.Now:yyyyMMdd}.pdf";
            return File(fileBytes, "application/pdf", fileName);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error generating PDF", error = ex.Message });
        }
    }

    [HttpGet("pdf/invoice/{paymentId}")]
    public async Task<IActionResult> GetInvoicePdf(Guid paymentId)
    {
        try
        {
            var fileBytes = await _pdfService.GenerateInvoicePdfAsync(paymentId);
            var fileName = $"Invoice_{paymentId}_{DateTime.Now:yyyyMMdd}.pdf";
            return File(fileBytes, "application/pdf", fileName);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error generating PDF", error = ex.Message });
        }
    }

    [HttpGet("pdf/result/{resultId}")]
    public async Task<IActionResult> GetResultPdf(Guid resultId)
    {
        try
        {
            var fileBytes = await _pdfService.GenerateResultPdfAsync(resultId);
            var fileName = $"Result_{resultId}_{DateTime.Now:yyyyMMdd}.pdf";
            return File(fileBytes, "application/pdf", fileName);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error generating PDF", error = ex.Message });
        }
    }
}
