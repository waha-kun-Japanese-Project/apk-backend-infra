using AutoMapper;
using Issue.Client.ServiceAbstraction;
using Issue.Domain.Contract;
using Issue.Domain.Entities.Issue;
using Issue.Service.Specifications;
using Issue.Service.Specifications.ExpertSpecifications;
using Issue.ServiceAbstraction.Expert;
using Issue.Shared.DTOS;
using Issue.Shared.DTOS.Query;
using Microsoft.AspNetCore.Mvc;
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
        //public Task<ResolutionActionResponse> CreateResolutionActionAsync(Guid issueId, CreateResolutionActionRequest request, CancellationToken cancellationToken = default)
        //{

        //}



        public async Task<CaseReviewResponse> GetCaseReviewAsync(Guid issueId, CancellationToken cancellationToken = default)
        {
            var issue = await unitOfWork.GetRepository<Issue.Domain.Entities.Issue.Issue, Guid>().GetByIdAsync(new IssueExpertInBoxSpecification(issueId), cancellationToken);
            if (issue is null)
            {
                throw new KeyNotFoundException(
                    $"Issue with Id '{issueId}' was not found.");
            }

            return mapper.Map<CaseReviewResponse>(issue);

        }



        //public async Task<IEnumerable<MaintenanceTeamResponse>> GetMaintenanceTeamsAsync(CancellationToken cancellationToken = default)
        //{var repository = unitOfWork.GetRepository<MaintenanceTeam, Guid>();
        //    var teams = await repository.GetAllAsync(  cancellationToken);
        //    return Task.FromResult(mapper.Map<IEnumerable<MaintenanceTeamResponse>>(teams));
        //}

        public async Task<RepairScheduleResponse> ScheduleRepairAsync(Guid issueId, ScheduleRepairRequest request, CancellationToken cancellationToken = default)
        {
            var issueRepository = unitOfWork.GetRepository<Issue.Domain.Entities.Issue.Issue, Guid>();
            var ScheduleRepairRepository=unitOfWork.GetRepository<RepairSchedule, Guid>();
            var issue = await issueRepository.GetByIdAsync( new IssueExpertInBoxSpecification(issueId), cancellationToken);
            if (issue is null)
            {
                throw new KeyNotFoundException(
                    $"Issue with Id '{issueId}' was not found.");
            }

            var schedule =new RepairSchedule
            {
                ScheduledDate = request.ScheduledDate,
                SlotEnd=request.SlotEnd,
                SlotStart=request.SlotStart,
                TeamId=request.TeamId,
                FarmerNotified=request.FarmerNotified,
                Notes=request.Notes

            };

            schedule.IssueId = issueId;

            ScheduleRepairRepository.Add(schedule);

            issue.Status = IssueStatus.Scheduled;

             AddStatusHistory( issue,issue.Id);
            issueRepository.Update(issue);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return mapper.Map<RepairScheduleResponse>(schedule);


        }

        public async Task<SubmitExpertReviewResponse> SubmitReviewAsync(Guid issueId, Guid expertId, SubmitExpertReviewRequest request, CancellationToken cancellationToken = default)
        {
            var issueRepository = unitOfWork.GetRepository<Issue.Domain.Entities.Issue.Issue, Guid>();
            var reviewRepository = unitOfWork.GetRepository<ExpertReviews, Guid>();
            var issue = await issueRepository.GetByIdAsync(new IssueExpertInBoxSpecification(issueId), cancellationToken);


            if (issue is null)
            {
                throw new KeyNotFoundException(
                    $"Issue with Id '{issueId}' was not found.");
            }
            if (issue.AssignedExpertId != expertId)
            {
                throw new UnauthorizedAccessException(
                    "You are not assigned to this issue.");
            }
            if (!(issue.Status == IssueStatus.Assigned))
                throw new UnauthorizedAccessException("You are not assigned to this issue.");

            var review = new ExpertReviews
            {
                IssueId = issueId,
                ExpertId = expertId,
                Decision = request.Decision,
                Notes = request.Notes,
            };
            issue.Status = IssueStatus.Reviewed;

            AddStatusHistory(issue, expertId);
            reviewRepository.Add(review);
            issueRepository.Update(issue);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return mapper.Map<SubmitExpertReviewResponse>(review);

        }


        public async Task<PaginatedResult<ExpertInboxResponse?>> GetAllInboxAsync(IssueQueryParameters parameters, CancellationToken cancellationToken)
        {
            var repository = unitOfWork.GetRepository<Issue.Domain.Entities.Issue.Issue, Guid>();

            var issues = await repository.GetAllAsync(new IssueExpertInBoxSpecification(parameters), cancellationToken);

            if (issues == null || !issues.Any())
            {
                throw new KeyNotFoundException("No issues were found.");
            }

            var data = mapper.Map<IEnumerable<ExpertInboxResponse>>(issues);

            var totalCount = await repository.CountAsync(
                new IssueExpertInBoxCountSpecification(parameters),
                cancellationToken);

            return new(parameters.pageIndex, data.Count(), totalCount, data);
        }

        private async void AddStatusHistory(Issue.Domain.Entities.Issue.Issue issue, Guid changedById)
        {
            var repository = unitOfWork.GetRepository<StatusHistory, Guid>();
            repository.Add(new StatusHistory
            {
                IssueId = issue.Id,
                Status = issue.Status,
                ChangedById = changedById,
                Note = $"Issue {issue.Status} by expert"
            });

        }
    
    }
}
