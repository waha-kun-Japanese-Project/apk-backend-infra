using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Report.ServiceAbstraction;
using Report.Shared.DTOS.Report;

namespace Report.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class IssueController(IIssueService issueService) : ControllerBase
    {
        [HttpPost("analyze")]
        public async Task<ActionResult<AiAnalysisResponse>> AnalyzeReport(
            [FromForm] AnalyzeIssueRequest analyze,
            CancellationToken cancellationToken)
        {
            var result = await issueService.AnalyzeIssueAsync(
                analyze,
                cancellationToken);

            return Ok(result);
        }

       
    }
}