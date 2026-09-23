using Community.Domain.Contracts;
using Community.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Community.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Community.Persistence.Repo
{
    public class IssueRepo(IssueDbContext dbContext ) : IIssueRepo
    {
 
        async Task<IEnumerable<Issue>> IIssueRepo.GetAllAsync(CancellationToken cancellationToken)
        {
            return await dbContext.Issues.
                AsNoTracking()
                .OrderByDescending(x=>x.CreatedAt)
                .ToListAsync(cancellationToken);

        }

        async Task<Issue?> IIssueRepo.GetByIdAsync(Guid issueId, CancellationToken cancellationToken)
        {
            return await dbContext.Issues
                .FirstOrDefaultAsync(x => x.Id == issueId, cancellationToken);
                
        }
    }
}
