using Issue.ServiceAbstraction;
using Issue.ServiceAbstraction.Farmer;
using Issue.Shared.DTOS.FarmerDtos;
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
        public async Task<IActionResult> GetAllIssuesAsync([FromQuery] IssueFilteration issueFilteration, CancellationToken cancellationToken = default)
        {
            var issues = await farmerService.GetAllIssuesAsync(issueFilteration, cancellationToken);
            return Ok(issues);
        }
        [HttpGet("issues/{reporterId}")]
        public async Task<IActionResult> GetAllIssuesByReporterIdAsync([FromQuery] GetFArmersIssuesParams issueFilteration, Guid reporterId, CancellationToken cancellationToken = default)
        {
            var issues = await farmerService.GetAllIssuesByReporterIdAsync(issueFilteration, reporterId, cancellationToken);
            return Ok(issues);
        }
    }
}
