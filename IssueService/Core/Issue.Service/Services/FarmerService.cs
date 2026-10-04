using AutoMapper;
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
using System.Text;
using System.Threading.Tasks;
using GetFarmerIssues = Issue.Shared.DTOS.FarmerDtos.GetFarmerIssues;
using System.Security.Claims;

namespace Issue.Service.Services
{
    public class FarmerService(
        IUnitOfWork unitOfWork , 
        IUserGrpcClient userGrpcClient , 
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper) 
        : Issue.ServiceAbstraction.Farmer.IFarmerService
    {
        public async Task<IEnumerable<Shared.DTOS.FarmerDtos.GetIssuesREsponseDto>> GetAllIssuesAsync(IssueFilteration issueFilteration,CancellationToken cancellationToken = default)
        {
                var repo = unitOfWork.GetRepository<Issue.Domain.Entities.Issue.Issue, Guid>();
                var CommentRepo = unitOfWork.GetRepository<Comment, Guid>();
                var VotesRepo = unitOfWork.GetRepository<IssueVote, Guid>();
                var ShareRepo = unitOfWork.GetRepository<IssueShared, Guid>();

                var issues = await repo.GetAllAsync(new GetAllIssues(issueFilteration) , cancellationToken);
                if(issues is null || !issues.Any())
                {
                    throw new KeyNotFoundException("No issues found.");
                }

            var result = mapper.Map<IEnumerable<Shared.DTOS.FarmerDtos.GetIssuesREsponseDto>>(issues);

            var usersIdS = issues.Select(i => i.ReporterId).Distinct().ToList();

                var users = await userGrpcClient.GetUsersByIdsAsync(usersIdS,cancellationToken);
                foreach(var dto in result)
                {
                    if (users.TryGetValue(dto.UserId, out var user))
                    {
                        dto.userName = user.Name;
                        dto.UserPhoto = user.PhotoUrl!;
                    }
                }

                var issueIds = issues.Select(i => i.Id).ToList();

                var commentCounts = await CommentRepo.CountByAsync(new CountComment(issueIds),x=>x.IssueId, cancellationToken);

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

        public async Task<IEnumerable<GetFarmerIssues>> GetAllIssuesByReporterIdAsync(GetFArmersIssuesParams IssueParams,Guid reporterId, CancellationToken cancellationToken = default)
        {
            var userId = GetLoggedInUserId();
            if (userId != IssueParams.ReporterId)
            {
                throw new UnauthorizedAccessException("You are not authorized to access this resource.");
            }
            var repo =   unitOfWork.GetRepository<Issue.Domain.Entities.Issue.Issue, Guid>();
            var issues = await repo.GetAllAsync(new GetFarmerIssuesSpecs(IssueParams), cancellationToken);
            var result =  mapper.Map<IEnumerable<GetFarmerIssues>>(issues);
            var ExpertIds =  issues.Select( i => i.AssignedExpertId).Distinct().ToList();

            var users = await userGrpcClient.GetExpertsByIdsAsync(ExpertIds, cancellationToken);
            foreach (var dto in result)
            {
                if (users.TryGetValue(dto.ExpertId, out var user))
                {
                    dto.ExpertName = user.Name;
                    dto.ExpertUrl = user.PhotoUrl!;
                }
            }
            return result;
        }

        public Task<IssueTrackingResponseDto> GetIssueTrackingByIssueIdAsync(StatusParams status, CancellationToken cancellationToken = default)
        {
            var IssueRepo = unitOfWork.GetRepository<Issue.Domain.Entities.Issue.Issue, Guid>();

            var StatusHistroyRepo = unitOfWork.GetRepository<StatusHistory, Guid>();

            var Issue = IssueRepo.GetByIdAsync(new GetIssueById(status.IssueID), cancellationToken);
            if (Issue is null)
            {
                throw new KeyNotFoundException("Issue not found.");
            }
            var histroy = StatusHistroyRepo.GetAllAsync(new StatusSpecs(status.IssueID), cancellationToken);

            throw new NotImplementedException();
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

    }
}
