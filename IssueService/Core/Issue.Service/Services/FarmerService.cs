using AutoMapper;
using Issue.Domain.Contract;
using Issue.Domain.Entities.Issue;
using Issue.Service.Specifications.FarmerSpecifications;
using Issue.ServiceAbstraction.Grpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Service.Services
{
    public class FarmerService(IUnitOfWork unitOfWork , IUserGrpcClient userGrpcClient , IMapper mapper) : Issue.ServiceAbstraction.Farmer.IFarmerService
    {
        public async Task<IEnumerable<Shared.DTOS.FarmerDtos.GetIssuesREsponseDto>> GetAllIssuesAsync(CancellationToken cancellationToken = default)
        {
            var repo = unitOfWork.GetRepository<Issue.Domain.Entities.Issue.Issue, Guid>();
            var issues = await repo.GetAllAsync(new GetAllIssues() , cancellationToken);
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
            //var commentsCount =repo.CountFarmerAsync(new CountComment(), cancellationToken);
            return result;

        }

    }
}
