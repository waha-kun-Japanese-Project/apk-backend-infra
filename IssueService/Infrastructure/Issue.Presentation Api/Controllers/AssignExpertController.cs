using Issue.ServiceAbstraction.Expert;
using Issue.Shared.DTOS.AssignExpert;
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
    public class AssignExpertController(
    IAssignExpertServices assignExpertServices) : ControllerBase
    {
        [HttpPost("{issueId:guid}/assign")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<AssignExpertResponse>> AssignExpert(
            Guid issueId,
            [FromBody] AssignExpertRequest request,
            CancellationToken cancellationToken)
        {
            var response = await assignExpertServices.AssignExpertAsync(
                issueId,
                request,
                cancellationToken);

            return Ok(response);
        }

       

        [HttpPut("{issueId:guid}/assign")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<AssignExpertResponse>> UpdateAssignedExpert(
            Guid issueId,
            [FromBody] AssignExpertRequest request,
            CancellationToken cancellationToken)
        {
            var response = await assignExpertServices.UpdateAssignedExpertAsync(
                issueId,
                request,
                cancellationToken);

            return Ok(response);
        }

        [HttpDelete("{issueId:guid}/assign")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UnassignExpert(
            Guid issueId,
            CancellationToken cancellationToken)
        {
            await assignExpertServices.UnassignExpertAsync(
                issueId,
                cancellationToken);

            return NoContent();
        }
    }
}
