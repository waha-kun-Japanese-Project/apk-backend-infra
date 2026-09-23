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
    public class CommentRepo(IssueDbContext dbContext) : ICommentRepo
    {
        public async Task AddAsync(Comment comment, CancellationToken cancellationToken = default)
        {
            await dbContext.Comments.AddAsync(comment, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Guid CommentId,Guid IssueId , CancellationToken cancellationToken = default)
        {
            var comment = await dbContext
               .Comments
               .FirstOrDefaultAsync(x => x.Id == CommentId && x.IssueId == IssueId, cancellationToken);
            if(comment is not null)
                dbContext.Comments.Remove(comment);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<Comment?> GetByIdAsync(Guid CommentId, CancellationToken cancellationToken = default)
        {
            return await dbContext.Comments
                .FirstOrDefaultAsync(x => x.Id == CommentId, cancellationToken);
        }

        public async Task<IEnumerable<Comment>> GetByIssueIdAsync(Guid issueId, int page,
                int pageSize, CancellationToken cancellationToken = default)
        {
            return await dbContext.Comments
                .AsNoTracking()
                .Where(x => x.IssueId == issueId)
                .OrderByDescending(x => x.CreatedAt)
                .Skip((page -1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }   

        public async Task<int> GetCountByIssueIdAsync(Guid issueId, CancellationToken cancellationToken = default)
        {
            return await dbContext.Comments
                .CountAsync(x => x.IssueId == issueId, cancellationToken);
        }
    }
}
