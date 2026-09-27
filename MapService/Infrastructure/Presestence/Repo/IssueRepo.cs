using Map.Domain.Contarcts;
using Map.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IssueEntity = Map.Domain.Entities.ISSUE.Issue;

namespace Map.Persistence.Repo;

public class IssueRepo(IssueDbContext issueDbContext) : IIssueRepo
{
    public async Task<IEnumerable<IssueEntity>> GetAllAsync(int pagesize, int page, CancellationToken cancellationToken)
    {
        return await issueDbContext.Issues
              .Include(x => x.GPSLocation)
              .Include(x => x.IssueAttachments)
              .OrderByDescending(x => x.CreatedAt)
              .Skip((page - 1) * pagesize)
              .Take(pagesize)
              .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<IssueEntity>> GetByTitle(string title, int pagesize, int page, CancellationToken cancellationToken)
    {
        return await issueDbContext.Issues
            .Include(x => x.GPSLocation)
            .Include(x => x.IssueAttachments)
            .Where(x => x.Title.Contains(title))
            .OrderByDescending(x => x.CreatedAt)
            .Skip((page - 1) * pagesize)
            .Take(pagesize)
            .ToListAsync();
    }
    public async Task<IssueEntity?> GetByIdAsync(Guid id)
    {
        return await issueDbContext.Issues
            .Include(x => x.GPSLocation)
            .Include(x => x.IssueAttachments)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}