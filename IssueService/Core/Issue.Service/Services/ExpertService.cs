using AutoMapper;
using Issue.Client.ServiceAbstraction;
using Issue.Domain.Contract;
using Issue.Domain.Entities.Issue;
using Issue.Service.Specifications;
using Issue.ServiceAbstraction.Expert;
using Issue.Shared.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Service.Services
{
    public class ExpertService(IUnitOfWork unitOfWork, IMapper mapper, IUserService userService) : IExpertService
    {
        public Task<ResolutionActionResponse> CreateResolutionActionAsync(Guid issueId, CreateResolutionActionRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<ExpertInboxResponse?> GetAllInboxAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<CaseReviewResponse> GetCaseReviewAsync(Guid issueId, CancellationToken cancellationToken = default)
        {
            var issue = await unitOfWork.GetRepository<Issue.Domain.Entities.Issue.Issue, Guid>().GetByIdAsync(new IssueExpertInBoxSpecification(issueId),cancellationToken);
            throw new NotImplementedException();
        }

        public Task<ExpertInboxResponse> GetInboxAsync(Guid expertId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MaintenanceTeamResponse>> GetMaintenanceTeamsAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<RepairScheduleResponse> ScheduleRepairAsync(Guid issueId, ScheduleRepairRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<SubmitExpertReviewResponse> SubmitReviewAsync(Guid issueId, Guid expertId, SubmitExpertReviewRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
