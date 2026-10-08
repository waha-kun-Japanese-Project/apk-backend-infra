using Issue.ServiceAbstraction.Expert;
using Issue.Shared.DTOS;
using Issue.Shared.DTOS.Query;
using Microsoft.AspNetCore.Authorization;
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
    public class ExpertController(
        IExpertService expertService) : ControllerBase
    {
        [HttpGet("inbox")]
        [Authorize(Roles = "Expert")]
        public async Task<ActionResult> GetInbox(
            [FromQuery] IssueQueryParameters parameters,
            CancellationToken cancellationToken)
        {
            var response = await expertService.GetAllInboxAsync(
                parameters,
                cancellationToken);

            return Ok(response);
        }

        [HttpGet("{issueId:guid}/review")]
        [Authorize(Roles = "Expert")]
        public async Task<ActionResult> GetCaseReview(
            Guid issueId,
            CancellationToken cancellationToken)
        {
            var response = await expertService.GetCaseReviewAsync(
                issueId,
                cancellationToken);

            return Ok(response);
        }

        [HttpPost("{issueId:guid}/review")]
        [Authorize(Roles = "Expert")]
        public async Task<ActionResult> SubmitReview(
            Guid issueId,
            [FromBody] SubmitExpertReviewRequest request,
            CancellationToken cancellationToken)
        {
            var response = await expertService.SubmitReviewAsync(
                issueId,
                request,
                cancellationToken);

            return Ok(response);
        }

        [HttpPost("{issueId:guid}/schedule")]
        [Authorize(Roles = "Expert")]
        public async Task<ActionResult> ScheduleRepair(
            Guid issueId,
            [FromBody] ScheduleRepairRequest request,
            CancellationToken cancellationToken)
        {
            var response = await expertService.ScheduleRepairAsync(
                issueId,
                request,
                cancellationToken);

            return Ok(response);
        }

        [HttpPost("{issueId:guid}/resolution")]
        [Consumes("multipart/form-data")]
        [Authorize(Roles = "Expert")]
        public async Task<ActionResult> CreateResolutionAction(
            Guid issueId,
            [FromForm] CreateResolutionActionRequest request,
            CancellationToken cancellationToken)
        {
            var response = await expertService.CreateResolutionActionAsync(
                issueId,
                request,
                cancellationToken);

            return Ok(response);
        }
    }
}
