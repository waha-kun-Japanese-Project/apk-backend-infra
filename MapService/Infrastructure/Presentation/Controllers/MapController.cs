using Map.ServiceAbsraction;
using Map.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Map.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MapController(IMapSerevice mapService) : ControllerBase
    {
        [HttpGet]
        [Authorize]
        [Route("ShowIssueInMap")]
        public async Task<IEnumerable<MapResponseDto>> ShowIssueInMap(
            [FromQuery] int pageSize = 20,
            [FromQuery] int page = 1,
            CancellationToken cancellation = default)
        {
            pageSize = Math.Clamp(pageSize, 1, 100);
            page = Math.Max(page, 1);
            return await mapService.ShowIssueInMapAsync(pageSize, page, cancellation);
        }

        [HttpGet]
        [Authorize]
        [Route("SearchForIssueInMap")]
        public async Task<MapResponseDto> SearchForIssueInMap([FromQuery] Guid IssueId, CancellationToken cancellationToken)
        {
            return await mapService.SearchForIssueInMapAsync(IssueId, cancellationToken);
        }

        [HttpGet]
        [Authorize]
        [Route("SearchForIssueByTitleInMap")]
        public async Task<IEnumerable<MapResponseDto>> SearchForIssueByTitleInMap(
            [FromQuery] string title,
            [FromQuery] int pageSize = 20,
            [FromQuery] int page = 1,
            CancellationToken cancellationToken = default)
        {
            pageSize = Math.Clamp(pageSize, 1, 100);
            page = Math.Max(page, 1);
            return await mapService.SearchForIssueByTitleInMapAsync(title, pageSize, page, cancellationToken);
        }
    }
}