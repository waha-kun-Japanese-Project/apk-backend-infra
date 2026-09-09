using GrpcUserClient.DTOS;
using Issue.ServiceAbstraction.Grpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserClinet.Grpc;

namespace Issue.Service.Grpc.Services
{
    public class UserGrpcClient(UserService.UserServiceClient client) : IUserGrpcClient
    {
        public async Task<IReadOnlyDictionary<Guid, UserInfoDto>> GetUsersByIdsAsync(IEnumerable<Guid> userIds, CancellationToken ct)
        {
            var ids = userIds
                        .Distinct()
                        .ToList();

            var request = new GetUsersRequest();

            request.UserIds.AddRange(
                ids.Select(x => x.ToString()));

            var response = await client.GetUsersByIdsAsync(
                request,
                cancellationToken: ct);

            return response.Users
                .Select(x => new UserInfoDto
                {
                    Id = Guid.Parse(x.Id),
                    Name = x.Name,
                    PhotoUrl = x.PhotoUrl
                })
                .ToDictionary(x => x.Id);
        }
    }
}
