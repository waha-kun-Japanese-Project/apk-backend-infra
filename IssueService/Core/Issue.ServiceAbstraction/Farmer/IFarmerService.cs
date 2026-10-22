using Issue.Shared.DTOS.FarmerDtos;
using Issue.Shared.DTOS.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.ServiceAbstraction.Farmer
{
    public interface IFarmerService
    {
        Task<PaginatedResult<Shared.DTOS.FarmerDtos.GetIssuesREsponseDto>>
            GetAllIssuesAsync(IssueFilteration issueFilteration,CancellationToken cancellationToken = default);
         
        Task<PaginatedResult<GetFarmerIssues>>
            GetAllIssuesByReporterIdAsync(IssueFilteration issueFilteration ,CancellationToken cancellationToken = default);
        Task<IssueTrackingResponseDto> GetIssueTrackingByIssueIdAsync(StatusParams status, CancellationToken cancellationToken = default);
        Task<CompleteStausReponse> GetCompleteStatusByIssueIdAsync(Guid IssueId, CancellationToken cancellationToken = default);
        Task<CompleteStausReponse> UncompleteIssueAsync(Guid issueId, CancellationToken cancellationToken = default);
    }
}
