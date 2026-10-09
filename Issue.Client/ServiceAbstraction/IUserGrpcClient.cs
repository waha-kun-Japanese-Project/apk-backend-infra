using Grpc.Core;
using GrpcUserClient.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Client.ServiceAbstraction;

public interface IUserGrpcClient
{
   Task<IReadOnlyDictionary<Guid, UserInfoDto>> GetUsersByIdsAsync(
   IEnumerable<Guid> userIds,
   CancellationToken ct = default);

    Task<IReadOnlyDictionary<Guid, UserInfoDto>>
      GetExpertsByIdsAsync(
          List<Guid?> expertIds,
          CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<Guid>> GetAllExpertIdsAsync(
        CancellationToken cancellationToken = default);
}
