using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Issue.Shared.DTOS;

namespace Issue.ServiceAbstraction.Expert
{
    public  interface IExpertService
    {
        Task<ExpertInboxResponse> GetInboxAsync( Guid expertId, CancellationToken cancellationToken = default);
        Task <ExpertInboxResponse?> GetAllInboxAsync(CancellationToken cancellationToken = default);

        Task<CaseReviewResponse> GetCaseReviewAsync(Guid issueId, CancellationToken cancellationToken = default);

        Task<SubmitExpertReviewResponse> SubmitReviewAsync(Guid issueId, Guid expertId, SubmitExpertReviewRequest request, CancellationToken cancellationToken = default);

        Task<ResolutionActionResponse> CreateResolutionActionAsync(Guid issueId, CreateResolutionActionRequest request, CancellationToken cancellationToken = default);

        Task<RepairScheduleResponse> ScheduleRepairAsync(Guid issueId, ScheduleRepairRequest request, CancellationToken cancellationToken = default);

        Task<IEnumerable<MaintenanceTeamResponse>> GetMaintenanceTeamsAsync(CancellationToken cancellationToken = default);
    }
}
