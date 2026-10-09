using Issue.ServiceAbstraction;
using Issue.ServiceAbstraction.Farmer;
using Issue.Shared.DTOS.FarmerDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Presentation_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FarmerController(IFarmerService farmerService): ControllerBase
    {
        [HttpGet("issues")]
        [Authorize(Roles = "Farmer")]
        

        public async Task<IActionResult> GetAllIssuesAsync([FromQuery] IssueFilteration issueFilteration, CancellationToken cancellationToken = default)
        {
            var issues = await farmerService.GetAllIssuesAsync(issueFilteration, cancellationToken);
            return Ok(issues);
        }
        [HttpGet("issues/{reporterId}")]
        [Authorize(Roles = "Farmer")]
        

        public async Task<IActionResult> GetAllIssuesByReporterIdAsync([FromQuery] IssueFilteration issueFilteration,CancellationToken cancellationToken = default)
        {
            var issues = await farmerService.GetAllIssuesByReporterIdAsync(issueFilteration,cancellationToken);
            return Ok(issues);
        }

        [HttpPost("{issueId:guid}/complete")]
        [ProducesResponseType(typeof(CompleteStausReponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<CompleteStausReponse>> CompleteIssueAsync(
    Guid issueId, CancellationToken cancellationToken)
        {
            var result = await farmerService.GetCompleteStatusByIssueIdAsync(issueId, cancellationToken);
            return Ok(result);
        }

        [HttpPost("{issueId}/uncomplete")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UncompleteIssueAsync(Guid issueId, CancellationToken cancellationToken)
        {
            await farmerService.UncompleteIssueAsync(issueId, cancellationToken);
            return NoContent();
        }
        [HttpGet("{issueId}/tracking")]
        [Authorize]
        [ProducesResponseType(typeof(IssueTrackingResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IssueTrackingResponseDto>> GetIssueTrackingByIssueIdAsync([FromQuery]StatusParams status, CancellationToken cancellationToken)
        {
            var result = await farmerService.GetIssueTrackingByIssueIdAsync(status, cancellationToken);
            return Ok(result);
        }
    }
}
