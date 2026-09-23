using Community.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Community.Domain.Contracts
{
    public interface ICommentRepo
    {
        

        Task<int> GetCountByIssueIdAsync(
            Guid issueId,
            CancellationToken cancellationToken = default);

        Task<Comment?> GetByIdAsync(
            Guid CommentId,
            CancellationToken cancellationToken = default);
        Task<IEnumerable<Comment>> GetByIssueIdAsync(
            Guid issueId,
            int page,
            int pageSize,
            CancellationToken cancellationToken = default);
        Task AddAsync(
            Comment comment,
            CancellationToken cancellationToken = default);

        Task DeleteAsync(
             Guid CommentId,
             Guid IssueId,
            CancellationToken cancellationToken = default);
    }
}
