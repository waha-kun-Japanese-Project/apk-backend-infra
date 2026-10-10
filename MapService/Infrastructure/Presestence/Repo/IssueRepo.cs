using Map.Domain.Contarcts;
using Map.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Map.Domain.Entities;
using Map.Domain.Entities.ISSUE;

namespace Map.Persistence.Repo;

public class IssueRepo(IssueDbContext issueDbContext) : IIssueRepo
{
    public async Task<IEnumerable<Issue>> GetAllAsync(int pagesize, int page ,CancellationToken cancellationToken)
    {
        return await issueDbContext.Issues
              .Include(x => x.GPSLocation)
              .Include(x=>x.IssueAttachments)  
              .OrderByDescending(x => x.CreatedAt)
              .Skip((page - 1) * pagesize)
              .Take(pagesize)
              .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Issue>> GetByTitle(string title, int pagesize, int page, CancellationToken cancellationToken)
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
    public async Task<Issue?> GetByIdAsync(Guid id)
    {
       
         return await issueDbContext.Issues
            .Include(x => x.GPSLocation)
            .Include(x => x.IssueAttachments)
            .FirstOrDefaultAsync(x => x.Id == id);
        
       

    }
}
