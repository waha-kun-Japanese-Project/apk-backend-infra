using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IssueEntity = Map.Domain.Entities.ISSUE.Issue;

namespace Map.Domain.Contarcts
{
    public interface IIssueRepo
    {
        Task<IssueEntity> GetByIdAsync(Guid id);

        Task<IEnumerable<IssueEntity>> GetAllAsync(int pageSize, int page, CancellationToken cancellationToken);
        Task<IEnumerable<IssueEntity>> GetByTitle(string title, int pagesize, int page, CancellationToken cancellationToken);
    }
}