using AutoMapper;
using Issue.Domain.Contract;
using Issue.Domain.Entities.Issue;
using Issue.Service.Specifications;
using Issue.ServiceAbstraction.Expert;
using Issue.Shared.DTOS.AssignExpert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Service.Services
{
    public class AssignExpertServices(IUnitOfWork unitOfWork, IMapper mapper) : IAssignExpertServices
    {
        public async Task<AssignExpertResponse> AssignExpertAsync(Guid issueId, AssignExpertRequest request, CancellationToken cancellationToken = default)
        {
            return await SetAssignedExpertAsync(issueId, request.ExpertId, cancellationToken);
        }

        public async Task<AssignExpertResponse> AutoAssignExpertAsync(Guid issueId, CancellationToken cancellationToken = default)
        {
            var repository =
          unitOfWork.GetRepository<Issue.Domain.Entities.Issue.Issue, Guid>();

            var issue = await repository.GetByIdAsync(  new AssignExpertInIssueSpecification(issueId), cancellationToken);

            if (issue is null)
            {
                throw new KeyNotFoundException($"Issue '{issueId}' was not found.");
            }
            if (issue.AssignedExpertId.HasValue)
            {
                throw new InvalidOperationException($"Issue '{issueId}' is already assigned.");
            }





        }

        public async Task UnassignExpertAsync(Guid issueId, CancellationToken cancellationToken = default)
        {
            var repository = unitOfWork.GetRepository<Issue.Domain.Entities.Issue.Issue, Guid>();

            var issue = await repository.GetByIdAsync(new IssueExpertInBoxSpecification(issueId), cancellationToken);

            if (issue is null)
                throw new KeyNotFoundException($"Issue with id '{issueId}' was not found.");

            issue.AssignedExpertId = null;
            issue.Status = IssueStatus.Verified;

            repository.Update(issue);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<AssignExpertResponse> UpdateAssignedExpertAsync(Guid issueId, AssignExpertRequest request, CancellationToken cancellationToken = default)
        {
            var repository = unitOfWork.GetRepository<Issue.Domain.Entities.Issue.Issue, Guid>();

            var issue = await repository.GetByIdAsync(new AssignExpertInIssueSpecification(issueId), cancellationToken);

            if (issue is null)
                throw new KeyNotFoundException($"Issue with id '{issueId}' was not found.");

            if (issue.AssignedExpertId is null)
                throw new InvalidOperationException($"Issue '{issueId}' has no expert assigned yet. Use AssignExpert instead.");

            return await SetAssignedExpertAsync(issue, request.ExpertId, cancellationToken);
        }
        private async Task<AssignExpertResponse> SetAssignedExpertAsync(Guid issueId, Guid expertId, CancellationToken cancellationToken)
        {
            var repository = unitOfWork.GetRepository<Issue.Domain.Entities.Issue.Issue, Guid>();

            var issue = await repository.GetByIdAsync(new AssignExpertInIssueSpecification(issueId), cancellationToken);

            if (issue is null)
                throw new KeyNotFoundException($"Issue with id '{issueId}' was not found.");

            return await SetAssignedExpertAsync(issue, expertId, cancellationToken);
        }

        private async Task<AssignExpertResponse> SetAssignedExpertAsync(
            Issue.Domain.Entities.Issue.Issue issue, Guid expertId
            , CancellationToken cancellationToken)
        {
            var repository = unitOfWork.GetRepository<Issue.Domain.Entities.Issue.Issue, Guid>();

            issue.AssignedExpertId = expertId;
            issue.Status = IssueStatus.Assigned;

            repository.Update(issue);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return new AssignExpertResponse(issue.Id, expertId,"FRERE");
        }
    }
}
