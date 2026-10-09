using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcUserClient.DTOS;
using Issue.Client.ServiceAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserClinet.Grpc;
using UserService.Grpc;

namespace Issue.Client.Grpc.Services;

public class UserGrpcClient(UserClinet.Grpc.UserGRPcService.UserGRPcServiceClient client,ExpertService.ExpertServiceClient Expertclient  ) : IUserGrpcClient
{
    public async Task<IReadOnlyCollection<Guid>> GetAllExpertIdsAsync(CancellationToken cancellationToken = default)
    {
        var response = await Expertclient.GetAllExpertAsync(new Empty(),cancellationToken: cancellationToken);
        var expertIds = new List<Guid>();

        foreach (var id in response.ExpertIds)
        {
            if (Guid.TryParse(id, out var expertId))
            {
                expertIds.Add(expertId);
            }
        }

        return expertIds;
    }

    public async Task<IReadOnlyDictionary<Guid, UserInfoDto>> GetExpertsByIdsAsync(List<Guid?> expertIds, CancellationToken cancellationToken = default)
    {
        var request = new GetExpertsRequest();

        request.UserIds.AddRange(
            expertIds.Select(x => x.ToString()));

        var response = await Expertclient.GetExpertsByIdsAsync(
            request,
            cancellationToken: cancellationToken);

        return response.Users.ToDictionary(
            x => Guid.Parse(x.Id),
            x => new UserInfoDto
            {
                Name = x.Name,
                PhotoUrl = x.PhotoUrl
            });
    }

    public async Task<IReadOnlyDictionary<Guid, UserInfoDto>> GetUsersByIdsAsync(IEnumerable<Guid> userIds,CancellationToken ct)
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
