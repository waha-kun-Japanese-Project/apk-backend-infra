
using AutoMapper;
using Issue.Client.ServiceAbstraction;
using Issue.Domain.Contract;
using Issue.Domain.Entities.Issue;
using Issue.Service.Specifications.ExpertSpecifications;
using Issue.ServiceAbstraction.Expert;
using Issue.Shared.DTOS;
using Issue.Shared.DTOS.Expert;
using Issue.Shared.DTOS.Query;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;


namespace Issue.Service.Services;

public class ExpertService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IMediaStorageGrpcClient mediaStorageGrpcClient,
    IHttpContextAccessor httpContextAccessor) : IExpertService
{
    public async Task<CaseReviewResponse> GetCaseReviewAsync(
        Guid issueId,
        CancellationToken cancellationToken = default)
    {
        var issue = await GetAssignedIssueAsync(issueId, cancellationToken);

        return mapper.Map<CaseReviewResponse>(issue);
    }

    public async Task<RepairScheduleResponse> ScheduleRepairAsync(
        Guid issueId,
        ScheduleRepairRequest request,
        CancellationToken cancellationToken = default)
    {
        var issue = await GetAssignedIssueAsync(issueId, cancellationToken);

        var schedule = new RepairSchedule
        {
            IssueId = issueId,
            ScheduledDate = request.ScheduledDate,
            SlotStart = request.SlotStart,
            SlotEnd = request.SlotEnd,
            FarmerNotified = request.FarmerNotified,
            Notes = request.Notes
        };

       unitOfWork. GetRepository<RepairSchedule,Guid>().Add(schedule);

        ChangeStatusAsync(issue, IssueStatus.Scheduled);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return mapper.Map<RepairScheduleResponse>(schedule);
    }

    public async Task<SubmitExpertReviewResponse> SubmitReviewAsync(
        Guid issueId,
        SubmitExpertReviewRequest request,
        CancellationToken cancellationToken = default)
    {
        var issue = await GetAssignedIssueAsync(issueId, cancellationToken);

        if (issue.Status != IssueStatus.Assigned)
            throw new InvalidOperationException(
                "Only assigned issues can be reviewed.");

        var review = new ExpertReviews
        {
            IssueId = issueId,
            ExpertId = GetLoggedInUserId(),
            Decision = request.Decision,
            Notes = request.Notes
        };

      unitOfWork.GetRepository<ExpertReviews,Guid>().Add(review);

        ChangeStatusAsync(issue, IssueStatus.Reviewed);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return mapper.Map<SubmitExpertReviewResponse>(review);
    }

    public async Task<PaginatedResult<ExpertInboxResponse?>> GetAllInboxAsync(
        IssueQueryParameters parameters,
        CancellationToken cancellationToken)
    {
        var expertid = GetLoggedInUserId();
        var repository =unitOfWork. GetRepository<Issue.Domain.Entities.Issue.Issue,Guid>();

        var issues = await repository.GetAllAsync(
            new IssueExpertInBoxSpecification(expertid, parameters),
            cancellationToken);

        if (!issues.Any())
            throw new KeyNotFoundException("No issues were found.");

        var data = mapper.Map<IEnumerable<ExpertInboxResponse>>(issues);

        var totalCount = await repository.CountAsync(
            new IssueExpertInBoxCountSpecification(expertid, parameters),
            cancellationToken);

        return new(parameters.pageIndex, data.Count(), totalCount,data);
    }

    public async Task<ResolutionActionResponse> CreateResolutionActionAsync(
        Guid issueId,
        CreateResolutionActionRequest request,
        CancellationToken cancellationToken = default)
    {
        var issue = await GetAssignedIssueAsync(issueId, cancellationToken);

        if (issue.Status != IssueStatus.Scheduled)
            throw new InvalidOperationException(
                "Resolution action can only be created for a scheduled issue.");

        if (request.Photo is null)
            throw new ArgumentException(
                "Resolution photo is required.");

        var filePath = await mediaStorageGrpcClient.UploadAsync(
            request.Photo,
            cancellationToken);

        var attachment = new IssueAttachment
        {
            IssueId = issueId,
            Url = filePath,
            Type = IssueAttachmentType.Photo,
            Purpose = IssueAttachmentPurpose.RepairProof
        };

        unitOfWork.GetRepository<IssueAttachment, Guid>().Add(attachment);

        issue.ActionRepair = request.Notes ?? string.Empty;


        ChangeStatusAsync(issue, IssueStatus.Repaired);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new ResolutionActionResponse
        {
            Id = attachment.Id,
            ActionRepair = issue.ActionRepair,
            Status = issue.Status.ToString(),
            FilePath = attachment.Url
        };

    }


    private async Task<Issue.Domain.Entities.Issue.Issue> GetAssignedIssueAsync(
        Guid issueId,
        CancellationToken cancellationToken)
    {
        var issue = await unitOfWork.GetRepository<Issue.Domain.Entities.Issue.Issue,Guid>()
            .GetByIdAsync(
                new IssueExpertInBoxSpecification(issueId),
                cancellationToken);

        if (issue is null)
            throw new KeyNotFoundException(
                $"Issue with Id '{issueId}' was not found.");

        if (issue.AssignedExpertId != GetLoggedInUserId())
            throw new UnauthorizedAccessException(
                "You are not assigned to this issue.");

        return issue;
    }

    private void ChangeStatusAsync(
        Issue.Domain.Entities.Issue.Issue issue,
        IssueStatus status)
    {
        issue.Status = status;

       unitOfWork.GetRepository<StatusHistory,Guid>().Add(new StatusHistory
        {
            IssueId = issue.Id,
            Status = status,
            ChangedById = GetLoggedInUserId(),
            Note = $"Issue {status} by expert"
        });
    }

    private Guid GetLoggedInUserId()
    {
        var user = httpContextAccessor.HttpContext?.User;

        if (user?.Identity?.IsAuthenticated != true)
            throw new UnauthorizedAccessException(
                "User not authenticated.");

        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!Guid.TryParse(userId, out var id))
            throw new UnauthorizedAccessException(
                "Invalid User Id.");

        return id;
    }

   
}
