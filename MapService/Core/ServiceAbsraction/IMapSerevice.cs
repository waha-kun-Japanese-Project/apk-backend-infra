using Map.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Map.ServiceAbsraction
{
    public interface IMapSerevice
    {
        Task<IEnumerable<MapResponseDto>> ShowIssueInMapAsync(int pageSize, int page, CancellationToken cancellationToken);

        Task<MapResponseDto> SearchForIssueInMapAsync(Guid IssueId ,CancellationToken cancellationToken);
        Task<IEnumerable<MapResponseDto>> SearchForIssueByTitleInMapAsync(string title, int pageSize, int page, CancellationToken cancellationToken);
    }
}
