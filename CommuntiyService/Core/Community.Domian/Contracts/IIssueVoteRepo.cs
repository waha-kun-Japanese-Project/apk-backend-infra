using Community.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Community.Domain.Contracts
{
    public interface IIssueVoteRepo
    {
        Task<bool> ExistsAsync(
        Guid issueId,
        Guid userId,
        CancellationToken cancellationToken = default);

        Task<int> GetCountByIssueIdAsync(
            Guid issueId,
            CancellationToken cancellationToken = default);

        Task<IssueVote?> GetAsync(
            Guid issueId,
            Guid userId,
            CancellationToken cancellationToken = default);

        Task AddAsync(
            IssueVote vote,
            CancellationToken cancellationToken = default);

        Task DeleteAsync(
            IssueVote vote,
            CancellationToken cancellationToken = default);

    }
}
