using Community.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Community.Domain.Contracts
{
    public interface IIssueShareRepo
    {
        Task<int>  GetCountByIssueIdAsync(
            Guid issueId,
            CancellationToken cancellationToken = default);
        Task AddAync(
            IssueShared issueShare,
            CancellationToken cancellationToken = default);
    }
}
