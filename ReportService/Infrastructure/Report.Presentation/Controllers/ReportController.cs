using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Report.ServiceAbstraction;
using Report.Shared.DTOS.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Report.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReportController(IReportService reportService) : ControllerBase
    {
        [HttpPost("create")]
        [Authorize]
        [RequestSizeLimit(20_000_000)]
        public async Task<CreateReportResponse> Create([FromForm] CreateReportRequest request, CancellationToken cancellationToken)
        {
            var result = await reportService.CreateReportAsync(request, cancellationToken);
            return result;
        }
        [HttpPost("AnalyzeReport")]
        [Authorize]
        public async Task<ReportDetailsResponse> AnalyzeReport([FromQuery] Guid id, CancellationToken cancellationToken)
        {
            var result = await reportService.AnalyzeReportAsync(id, cancellationToken);
            return result;
        }

        [HttpGet("GetReportById")]
        [Authorize]
        public async Task<ReportDetailsResponse> GetReportById([FromQuery] Guid id, CancellationToken cancellationToken)
        {
            var result = await reportService.GetReportByIdAsync(id, cancellationToken);
            return result;
        }
        [HttpGet("GetAllReports")]
        [Authorize]
        public async Task<IEnumerable<ReportDetailsResponse>> GetAllReports(CancellationToken cancellationToken)
        {
            var result = await reportService.GetAllReportsAsync(cancellationToken);
            return result;
        }
        [HttpGet("GetMyReports")]
        [Authorize]
        public async Task<IEnumerable<ReportDetailsResponse>> GetMyReports(CancellationToken cancellationToken)
        {
            var result = await reportService.GetMyReportsAsync(cancellationToken);
            return result;
        }
        [HttpDelete("DeleteReport")]
        [Authorize]
        public async Task<IActionResult> DeleteReport([FromQuery] Guid id, CancellationToken cancellationToken)
        {
            await reportService.DeleteReportAsync(id, cancellationToken);
            return NoContent();

        }

    }
}
