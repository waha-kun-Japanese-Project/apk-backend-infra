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
        [HttpPost("create")]
        [RequestSizeLimit(20_000_000)]
        public async Task<ActionResult<CreateIssueResponse>> Create(
            [FromBody] CreateIssueRequest request,
            CancellationToken cancellationToken)
        {
            var result = await issueService.CreateIssueAsync(
                request,
                cancellationToken);

            return Ok(result);
        }

        [HttpPost("analyze")]
        public async Task<ActionResult<AiAnalysisResponse>> AnalyzeReport(
            [FromForm] AnalyzeIssueRequest analyze ,
            CancellationToken cancellationToken)
        {
            var result = await issueService.AnalyzeIssueAsync(
                analyze.Photo,
                cancellationToken);

            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteReport(
            Guid id,
            CancellationToken cancellationToken)
        {
            await issueService.DeleteIssueAsync(id, cancellationToken);

            return NoContent();
        }
    }
}