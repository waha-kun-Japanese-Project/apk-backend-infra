using Map.ServiceAbsraction;
using Map.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            [FromQueryAttribute] int pageSize,
            [FromQuery] int page,
            CancellationToken cancellation)
        {
            var result = await mapService.ShowIssueInMapAsync(pageSize ,page ,cancellation);
            return result;
        }
        [HttpGet]
        [Authorize]
        [Route("SearchForIssueInMap")]
        public async Task<MapResponseDto> SearchForIssueInMap([FromQuery] Guid IssueId, CancellationToken cancellationToken)
        {
            var result = await mapService.SearchForIssueInMapAsync(IssueId, cancellationToken);
            return result;
        }
        [HttpGet]
        [Authorize]
        [Route("SearchForIssueByTitleInMap")]
        public async Task<IEnumerable<MapResponseDto>> SearchForIssueByTitleInMap([FromQuery] string title, [FromQuery] int pageSize, [FromQuery] int page, CancellationToken cancellationToken)
        {
            var result = await mapService.SearchForIssueByTitleInMapAsync(title, pageSize, page, cancellationToken);
            return result;
        }

    }
}
