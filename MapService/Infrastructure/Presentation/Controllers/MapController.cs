using Map.ServiceAbsraction;
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
        [Route("ShowIssueInMap")]
        public async Task<IEnumerable<Map.Shared.MapResponseDto>> ShowIssueInMap( CancellationToken cancellationToken)
        {
            var result = await mapService.ShowIssueInMapAsync(cancellationToken);
            return result;
        }
        [HttpGet]
        [Route("SearchForIssueInMap")]
        public async Task<Map.Shared.MapResponseDto> SearchForIssueInMap([FromQuery] Guid IssueId, CancellationToken cancellationToken)
        {
            var result = await mapService.SearchForIssueInMapAsync(IssueId, cancellationToken);
            return result;
        }

    }
}
