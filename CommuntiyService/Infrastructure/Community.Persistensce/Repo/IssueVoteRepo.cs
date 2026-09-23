using Community.Domain.Contracts;
using Community.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Community.Persistence.Repo
{
    public class IssueVoteRepo(IssueDbContext dbContext) : IIssueVoteRepo
    {
        public async Task AddAsync(IssueVote vote, CancellationToken cancellationToken = default)
        {
            await dbContext.Votes.AddAsync(vote, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(IssueVote vote, CancellationToken cancellationToken = default)
        {
            await Task.Run(() => dbContext.Votes.Remove(vote), cancellationToken);
        }

        public async Task<bool> ExistsAsync(Guid issueId, Guid userId, CancellationToken cancellationToken = default)
        {
            return await dbContext.Votes
                        .AnyAsync(
                            x => x.IssueId == issueId &&
                                 x.UserId == userId,
                            cancellationToken);
        }

        public Task<IssueVote?> GetAsync(Guid issueId, Guid userId, CancellationToken cancellationToken = default)
        {
            return dbContext.Votes
                .FirstOrDefaultAsync(x => x.IssueId == issueId && x.UserId == userId, cancellationToken);
        }

        public async Task<int> GetCountByIssueIdAsync(Guid issueId, CancellationToken cancellationToken = default)
        {
            return await dbContext.Votes
                .CountAsync(x => x.IssueId == issueId, cancellationToken);
        }
    }
}
