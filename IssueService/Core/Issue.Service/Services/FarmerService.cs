using AutoMapper;
using Grpc.Core;
using Issue.Client.ServiceAbstraction;
using Issue.Domain.Contract;
using Issue.Domain.Entities.Issue;
using Issue.Service.Specifications.FarmerSpecifications;
using Issue.Shared.DTOS.FarmerDtos;
using MassTransit.Initializers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GetFarmerIssues = Issue.Shared.DTOS.FarmerDtos.GetFarmerIssues;

namespace Issue.Service.Services
{
    public class FarmerService(
        IUnitOfWork unitOfWork,
        IUserGrpcClient userGrpcClient,
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper)
        : Issue.ServiceAbstraction.Farmer.IFarmerService
    {
        public async Task<IEnumerable<Shared.DTOS.FarmerDtos.GetIssuesREsponseDto>> GetAllIssuesAsync(IssueFilteration issueFilteration, CancellationToken cancellationToken = default)
        {
            var repo = unitOfWork.GetRepository<Issue.Domain.Entities.Issue.Issue, Guid>();
            var CommentRepo = unitOfWork.GetRepository<Comment, Guid>();
            var VotesRepo = unitOfWork.GetRepository<IssueVote, Guid>();
            var ShareRepo = unitOfWork.GetRepository<IssueShared, Guid>();

            var issues = await repo.GetAllAsync(new GetAllIssues(issueFilteration), cancellationToken);

            var result = mapper.Map<IEnumerable<Shared.DTOS.FarmerDtos.GetIssuesREsponseDto>>(issues);

            var usersIdS = issues.Select(i => i.ReporterId).Distinct().ToList();

            var users = await userGrpcClient.GetUsersByIdsAsync(usersIdS, cancellationToken);
            foreach (var dto in result)
            {
                if (users.TryGetValue(dto.UserId, out var user))
                {
                    dto.userName = user.Name;
                    dto.UserPhoto = user.PhotoUrl!;
                }
            }

            var issueIds = issues.Select(i => i.Id).ToList();

            var commentCounts = await CommentRepo.CountByAsync(new CountComment(issueIds), x => x.IssueId, cancellationToken);

            var VotesCounts = await VotesRepo.CountByAsync(new CountVotes(issueIds), x => x.IssueId, cancellationToken);

            var ShareCounts = await ShareRepo.CountByAsync(new CountShares(issueIds), x => x.IssueId, cancellationToken);
            foreach (var dto in result)
            {
                dto.CommentCount = commentCounts.GetValueOrDefault(dto.IssueId);
                dto.VoteCount = VotesCounts.GetValueOrDefault(dto.IssueId);
                dto.ShareCount = ShareCounts.GetValueOrDefault(dto.IssueId);
            }

            return result;

        }

        public async Task<IEnumerable<GetFarmerIssues>> GetAllIssuesByReporterIdAsync(IssueFilteration IssueParams ,CancellationToken cancellationToken = default)
        {
            var userId = GetLoggedInUserId();
            //if (userId != reportedId)
            //{
            //    throw new UnauthorizedAccessException("You are not authorized to access this resource.");
            //}
            var repo = unitOfWork.GetRepository<Issue.Domain.Entities.Issue.Issue, Guid>();
            var issues = await repo.GetAllAsync(new GetFarmerIssuesSpecs(IssueParams,userId), cancellationToken);


            var result = mapper.Map<IEnumerable<GetFarmerIssues>>(issues);

            var ExpertIds = issues.Where(i => i.AssignedExpertId != Guid.Empty)   // adjust if the type is Guid?
                            .Select(i => i.AssignedExpertId)
                            .Distinct()
                            .ToList();

            var users = await userGrpcClient.GetExpertsByIdsAsync(ExpertIds, cancellationToken);
            foreach (var dto in result)
            {
                if (users.TryGetValue(dto.ExpertId, out var user))
                {
                    dto.ExpertName = user.Name;
                    dto.ExpertUrl = user.PhotoUrl!;
                }
            }
            //var Farmers = await userGrpcClient.GetUsersByIdsAsync(usersIdS, cancellationToken);
            //foreach (var dtos in result)
            //{
            //    if (users.TryGetValue(dtos.ReporterId, out var farmer))
            //    {
            //        dtos.userName = farmer.Name;
            //        dtos.userPhoto = farmer.PhotoUrl!;
            //    }
            //}
            return result;
        }

        public async Task<CompleteStausReponse> GetCompleteStatusByIssueIdAsync(Guid IssueId, CancellationToken cancellationToken = default)
        {
            var issueRepo = unitOfWork.GetRepository<Issue.Domain.Entities.Issue.Issue, Guid>();
            var historyRepo = unitOfWork.GetRepository<StatusHistory, Guid>();

            var issue = await issueRepo.GetByIdAsync(new GetIssueById(IssueId), cancellationToken);
            if (issue is null)
                throw new KeyNotFoundException("Issue not found.");

            //var latest = await historyRepo.GetByIdAsync(new StatusSpecs(IssueId), cancellationToken);
            //if (latest is null)
            //    throw new KeyNotFoundException("No status history found for this issue.");

            if (issue.Status != IssueStatus.Repaired)
                throw new InvalidOperationException("Only repaired issues can be completed.");

            ChangeStatus(issue, IssueStatus.completed);   // must return the new StatusHistory
            issue.Status = IssueStatus.completed;
            issueRepo.Update(issue);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return mapper.Map<CompleteStausReponse>(issue);

        }

        public async Task<IssueTrackingResponseDto> GetIssueTrackingByIssueIdAsync(StatusParams status,CancellationToken cancellationToken = default)
        {
            var isseuRepo = unitOfWork.GetRepository<Issue.Domain.Entities.Issue.Issue, Guid>();
            var HistoryRepo = unitOfWork.GetRepository<StatusHistory, Guid>();
            var issue = await isseuRepo.GetByIdAsync(
            new GetIssueById(status.IssueID) ,
            cancellationToken);

            if (issue is null)
                throw new KeyNotFoundException("Issue not found.");

            var history = await HistoryRepo.GetAllAsync(
                new StatusSpecs(status.IssueID),
                cancellationToken);

            var steps = mapper.Map<List<IssueTrackingStepDto>>(history);

            foreach (var step in steps)
            {
                step.State = GetStepState(
                    step.Status,
                    issue.Status);
            }

            // Get current expert
            if (issue.AssignedExpertId.HasValue)
            {
                var experts = await userGrpcClient.GetExpertsByIdsAsync(
                    new List<Guid?  > { issue.AssignedExpertId.Value },
                    cancellationToken);

                if (experts.TryGetValue(
                        issue.AssignedExpertId.Value,
                        out var expert))
                {
                    var assignedStep = steps.FirstOrDefault(
                        x => x.Status == IssueStatus.Assigned);

                    if (assignedStep is not null)
                    {
                        assignedStep.ExpertId = issue.AssignedExpertId;
                        assignedStep.ExpertName = expert.Name;
                        assignedStep.ExpertPhoto = expert.PhotoUrl;
                    }
                }
            }

            var repairedStep = steps.FirstOrDefault(
                x => x.Status == IssueStatus.Repaired);

            if (repairedStep is not null)
            {
                repairedStep.RepairPhoto = issue.IssueAttachments

                    .FirstOrDefault(x => x.Purpose == IssueAttachmentPurpose.RepairProof)
                    ?.Url;
            }

            return new IssueTrackingResponseDto
            {
                IssueId = issue.Id,
                CurrentStatus = issue.Status,
                Steps = steps
            };
        }


        public async Task UncompleteIssueAsync(Guid issueId, CancellationToken cancellationToken = default)
        {
            var issueRepo = unitOfWork.GetRepository<Issue.Domain.Entities.Issue.Issue, Guid>();

            var issue = await issueRepo.GetByIdAsync(new GetIssueById(issueId), cancellationToken);
            if (issue is null)
                throw new KeyNotFoundException("Issue not found.");

            if (issue.Status != IssueStatus.Repaired)
                throw new InvalidOperationException("Only completed issues can be marked as uncompleted.");

            await UncompleteStatusAsync(issue, cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);   // one save for the issue and the deleted rows
        }

        private Guid GetLoggedInUserId()
        {
            var user = httpContextAccessor.HttpContext?.User;
            if (user is null || !user.Identity!.IsAuthenticated)
            {
                throw new UnauthorizedAccessException("User not authenticated.");
            }

            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var parsedId))
            {
                throw new UnauthorizedAccessException("User Id not found in token.");
            }

            return parsedId;
        }

        //private async Task<StatusHistory> ChangeStatusAsync(
        //    Issue.Domain.Entities.Issue.Issue issue,
        //    IssueStatus newStatus,
        //    CancellationToken cancellationToken = default)
        //{
        //    var historyRepo = unitOfWork.GetRepository<StatusHistory, Guid>();

        //    var history = new StatusHistory
        //    {
        //        IssueId = issue.Id,
        //        Status = newStatus,
        //        ChangedAt = DateTime.UtcNow,
        //        Note = $"Issue status changed to {newStatus} by farmer",
        //        ChangedById = GetLoggedInUserId()
        //    };

        //    historyRepo.Add(history);   // match your repo's signature
        //    //await unitOfWork.SaveChangesAsync(cancellationToken);

        //    return history;
        //}
        private void ChangeStatus(
            Issue.Domain.Entities.Issue.Issue issue,
            IssueStatus status)
        {
            issue.Status = status;

            unitOfWork.GetRepository<StatusHistory, Guid>().Add(new StatusHistory
            {
                IssueId = issue.Id,
                Status = status,
                ChangedById = GetLoggedInUserId(),
                Note = $"Issue {status} by expert"
            });
        }
        private async Task UncompleteStatusAsync(
            Issue.Domain.Entities.Issue.Issue issue,
            CancellationToken cancellationToken)
        {
            var historyRepo = unitOfWork.GetRepository<StatusHistory, Guid>();
            var history = await historyRepo.GetAllAsync(new StatusSpecs(issue.Id), cancellationToken);

            var toRemove = history
                .Where(h => h.Status is IssueStatus.Repaired or IssueStatus.completed)
                .ToList();

            foreach (var entry in toRemove)
                historyRepo.Remove(entry);

            issue.Status = IssueStatus.Scheduled;
        }
        private static TrackingStepState GetStepState(
            IssueStatus stepStatus,
        IssueStatus currentStatus)
        {
            if (stepStatus < currentStatus)
                return TrackingStepState.Completed;

            if (stepStatus == currentStatus)
                return TrackingStepState.Current;

            return TrackingStepState.Pending;
        }
    }
}
