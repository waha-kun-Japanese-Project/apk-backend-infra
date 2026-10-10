using AutoMapper;
using Issue.Client.ServiceAbstraction;
using Issue.Domain.Contract;
using Issue.Domain.Entities.Issue;
using Issue.Service.Specifications.ExpertSpecifications;
using Issue.ServiceAbstraction.Expert;
using Issue.Shared.DTOS.AssignExpert;
using Microsoft.AspNetCore.Http;
using System.Data;
using System.Security.Claims;

namespace Issue.Service.Services
{
    public class AssignExpertServices(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IUserGrpcClient userGrpcClient,
        ITransaction _transaction,
        IHttpContextAccessor httpContextAccessor) : IAssignExpertServices
    {
        public async Task<AssignExpertResponse> AssignExpertAsync(
            Guid issueId, AssignExpertRequest request,
            CancellationToken cancellationToken = default)
        {
            return await SetAssignedExpertAsync(issueId, request.ExpertId, cancellationToken);
        }

        public async Task<AssignExpertResponse> AutoAssignExpertAsync(
            Guid issueId, CancellationToken cancellationToken = default)
        {
            var expertids = await userGrpcClient.GetAllExpertIdsAsync(cancellationToken);
            if (expertids.Count == 0)
            {
                throw new InvalidOperationException("No experts are currently available.");
            }

            await using var transaction = await _transaction.BeginTransactionAsync(
                IsolationLevel.Serializable, cancellationToken);
            await _transaction.AcquireLockAsync("WAHAKUN_Expert_Assignment", 10_000, cancellationToken);

            var issue = await GetIssueOrThrowAsync(issueId, cancellationToken);

            if (issue.AssignedExpertId.HasValue)
            {
                await transaction.CommitAsync(cancellationToken);
                return mapper.Map<AssignExpertResponse>(issue);
            }

            var expertId = await PickLeastBusyExpertAsync(expertids, cancellationToken);

            var response = await SetAssignedExpertAsync(issue, expertId, cancellationToken);

            await transaction.CommitAsync(cancellationToken);
            return response;
        }

        public async Task UnassignExpertAsync(Guid issueId, CancellationToken cancellationToken = default)
        {
            var issue = await GetIssueOrThrowAsync(issueId, cancellationToken);
            var repository = unitOfWork.GetRepository<Issue.Domain.Entities.Issue.Issue, Guid>();

            issue.AssignedExpertId = null;

            repository.Update(issue);
            ChangeStatus(issue, IssueStatus.Diagnosed);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<AssignExpertResponse> UpdateAssignedExpertAsync(
            Guid issueId, AssignExpertRequest request,
            CancellationToken cancellationToken = default)
        {
            var issue = await GetIssueOrThrowAsync(issueId, cancellationToken);
            return await SetAssignedExpertAsync(issue, request.ExpertId, cancellationToken);
        }

        private async Task<AssignExpertResponse> SetAssignedExpertAsync(
            Guid issueId, Guid expertId, CancellationToken cancellationToken)
        {
            var issue = await GetIssueOrThrowAsync(issueId, cancellationToken);
            return await SetAssignedExpertAsync(issue, expertId, cancellationToken);
        }

        private async Task<AssignExpertResponse> SetAssignedExpertAsync(
            Issue.Domain.Entities.Issue.Issue issue, Guid expertId,
            CancellationToken cancellationToken)
        {
            var repository = unitOfWork.GetRepository<Issue.Domain.Entities.Issue.Issue, Guid>();

            issue.AssignedExpertId = expertId;

            repository.Update(issue);
            ChangeStatus(issue, IssueStatus.Assigned);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return mapper.Map<AssignExpertResponse>(issue);
        }

        private async Task<Guid> PickLeastBusyExpertAsync(
            IReadOnlyCollection<Guid> candidateExpertIds,
            CancellationToken cancellationToken)
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

        private async Task<Issue.Domain.Entities.Issue.Issue> GetIssueOrThrowAsync(
            Guid issueId, CancellationToken cancellationToken)
        {
            var repository = unitOfWork.GetRepository<Issue.Domain.Entities.Issue.Issue, Guid>();
            var issue = await repository.GetByIdAsync(
                new AssignExpertInIssueSpecification(issueId), cancellationToken);

            return issue ?? throw new KeyNotFoundException($"Issue '{issueId}' was not found.");
        }

        private void ChangeStatus(Issue.Domain.Entities.Issue.Issue issue, IssueStatus status)
        {
            issue.Status = status;

            var userId = TryGetLoggedInUserId();

            unitOfWork.GetRepository<StatusHistory, Guid>().Add(new StatusHistory
            {
                IssueId = issue.Id,
                Status = status,
                ChangedById = userId, // null only for system actions
                Note = userId.HasValue
                    ? $"Issue {status} by admin"
                    : $"Issue {status} by system"
            });
        }

        private Guid? TryGetLoggedInUserId()
        {
            var user = httpContextAccessor.HttpContext?.User;

            if (user?.Identity?.IsAuthenticated != true)
                return null;

            var value = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(value, out var id) ? id : null;
        }
    }
}