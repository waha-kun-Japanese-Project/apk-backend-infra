using Map.Domain.Contarcts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Map.Persistence.Repo;

public class IssueRepo(Issue.Persistence.Context.IssueDbContext issueDbContext) : IIsuueRepo
{
    public async Task<IEnumerable<Issue.Domain.Entities.Issue.Issue>> GetAllAsync()
    {
        return await issueDbContext
                                .Issues.ToListAsync();
    }

    public async Task<Issue.Domain.Entities.Issue.Issue?> GetByIdAsync(Guid id)
    {
        return await issueDbContext.Issues
                        .FirstOrDefaultAsync(x=>x.Id == id);
    }
}
