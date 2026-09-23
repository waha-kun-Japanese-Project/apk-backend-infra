using Communtiy.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Community.ServiceAbstraction
{
    public interface IcommunityService
    {
        Task <CommentsResponseDto> GetCommnentsByIsuueIdAsync(Guid issueId,int page ,int pageSize, CancellationToken cancellationToken = default);
    }
}
