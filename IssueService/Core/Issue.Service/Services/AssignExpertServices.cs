using AutoMapper;
using Issue.Client.ServiceAbstraction;
using Issue.Domain.Contract;
using Issue.Domain.Entities.Issue;
using Issue.Service.Concurrency;
using Issue.Service.Specifications.ExpertSpecifications;
using Issue.ServiceAbstraction.Expert;
using Issue.Shared.DTOS.AssignExpert;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Service.Services
{
    public class AssignExpertServices(
        IUnitOfWork unitOfWork, IMapper mapper,
       ExpertAssignmentGate gate, IUserService userService ) : IAssignExpertServices
    {
        public async Task<AssignExpertResponse> AssignExpertAsync(
            Guid issueId, AssignExpertRequest request,
            CancellationToken cancellationToken = default)
        {
            return await SetAssignedExpertAsync(issueId, request.ExpertId, cancellationToken);
        }

        public async Task<AssignExpertResponse> AutoAssignExpertAsync(Guid issueId, CancellationToken cancellationToken = default)
        {
         
          var issue = await GetIssueOrThrowAsync(issueId, cancellationToken);

            var expert =(await  userService.GetExpertDetails()).ToDictionary(e => e.ExpertId, e => e.Name);
            if (expert.Count == 0)
            {
                throw new InvalidOperationException("No experts available for assignment.");
            }
            return await gate.RunExclusiveAsync(async () =>
            {
                var expertId = await PickLeastBusyExpertAsync(expert.Keys, cancellationToken);
                return await SetAssignedExpertAsync(issue, expertId, cancellationToken);
            }, cancellationToken);

        }

        public async Task UnassignExpertAsync(Guid issueId, CancellationToken cancellationToken = default)
        {
            var repository = unitOfWork.GetRepository<Issue.Domain.Entities.Issue.Issue, Guid>();
            var issue = await repository.GetByIdAsync(new AssignExpertInIssueSpecification(issueId), cancellationToken);

            if (issue == null) {
                throw new KeyNotFoundException($"Issue '{issueId}' was not found.");
            }

            issue.AssignedExpertId = null;
            issue.Status = IssueStatus.Diagnosed;

            repository.Update(issue);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<AssignExpertResponse> UpdateAssignedExpertAsync
            (Guid issueId, AssignExpertRequest request,
            CancellationToken cancellationToken = default)
        {
           var issue = await GetIssueOrThrowAsync(issueId, cancellationToken);
            return await SetAssignedExpertAsync(issue, request.ExpertId, cancellationToken);
        }


        private async Task<AssignExpertResponse> SetAssignedExpertAsync(
            Guid issueId, Guid expertId
           , CancellationToken cancellationToken)
        {
          var issue = await GetIssueOrThrowAsync(issueId, cancellationToken);

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

            return mapper.Map<AssignExpertResponse>(issue);
        }

        private async Task<Guid> PickLeastBusyExpertAsync(
            IReadOnlyCollection<Guid> candidateExpertIds
            , CancellationToken cancellationToken)
        {
            var issues = await unitOfWork.GetRepository<Issue.Domain.Entities.Issue.Issue, Guid>()
                .GetAllAsync(new AssignExpertInIssueSpecification(), cancellationToken);

            var openCountByExpert = issues
                .Where(i => i.AssignedExpertId.HasValue && i.Status != IssueStatus.completed)
                .GroupBy(i => i.AssignedExpertId!.Value)
                .ToDictionary(g => g.Key, g => g.Count());

            return candidateExpertIds
                .OrderBy(id => openCountByExpert.TryGetValue(id, out var count) ? count : 0)
                .First();
        }



        private async Task<Issue.Domain.Entities.Issue.Issue> GetIssueOrThrowAsync(Guid issueId, CancellationToken cancellationToken)
        {
            var repository = unitOfWork.GetRepository<Issue.Domain.Entities.Issue.Issue, Guid>();
            var issue = await repository.GetByIdAsync(new AssignExpertInIssueSpecification(issueId), cancellationToken);

            return issue ?? throw new KeyNotFoundException($"Issue '{issueId}' was not found.");
        }



    }
}
