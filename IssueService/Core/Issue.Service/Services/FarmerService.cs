using AutoMapper;
using Issue.Client.ServiceAbstraction;
using Issue.Domain.Contract;
using Issue.Domain.Entities.Issue;
using Issue.Service.Specifications.FarmerSpecifications;

using Issue.Shared.DTOS.FarmerDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Service.Services
{
    public class FarmerService(IUnitOfWork unitOfWork , IUserGrpcClient userGrpcClient , IMapper mapper) : Issue.ServiceAbstraction.Farmer.IFarmerService
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

                var users = await userGrpcClient.GetUsersByIdsAsync(usersIdS, cancellationToken);
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

    }
}
