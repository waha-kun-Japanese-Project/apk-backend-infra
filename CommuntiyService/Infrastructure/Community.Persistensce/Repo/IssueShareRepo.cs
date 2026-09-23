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
    public class IssueShareRepo(IssueDbContext dbContext) : IIssueShareRepo
    {
        public async Task AddAync(IssueShared issueShare, CancellationToken cancellationToken = default)
        {   
            await dbContext
                .Shares
                .AddAsync(issueShare, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> GetCountByIssueIdAsync(Guid issueId, CancellationToken cancellationToken = default)
        {
            return await dbContext
                .Shares
                .CountAsync(x => x.IssueId == issueId, cancellationToken);
        }
    }
}
