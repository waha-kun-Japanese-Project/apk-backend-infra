using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Map.ServiceAbsraction
{
    public interface IMapSerevice
    {
        Task<IEnumerable<Map.Shared.MapResponseDto>> ShowIssueInMapAsync(CancellationToken cancellationToken);

        Task<Map.Shared.MapResponseDto> SearchForIssueInMapAsync(Guid IssueId , CancellationToken cancellationToken);
    }
}
