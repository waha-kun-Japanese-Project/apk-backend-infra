using Community.ServiceAbstraction;
using Communtiy.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Community.Service.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class CommunityController(IcommunityService communityService) : ControllerBase
    {
        [HttpGet]
        [Route("GetCommentsByIssueId")]
        public async Task<ActionResult<CommentsResponseDto>> GetCommnentsByIsuueIdAsync(
            [FromQuery] Guid issueId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            CancellationToken cancellationToken = default)
        {
            var result = await communityService.GetCommnentsByIsuueIdAsync(issueId,page,pageSize, cancellationToken);
            return result;
        }
    }
}
