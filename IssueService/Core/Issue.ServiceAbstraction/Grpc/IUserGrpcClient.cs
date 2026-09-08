using GrpcUserClient.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.ServiceAbstraction.Grpc
{
    public interface IUserGrpcClient
    {
       Task<IReadOnlyDictionary<Guid, UserInfoDto>> GetUsersByIdsAsync(
       IEnumerable<Guid> userIds,
       CancellationToken ct = default);
    }
}
