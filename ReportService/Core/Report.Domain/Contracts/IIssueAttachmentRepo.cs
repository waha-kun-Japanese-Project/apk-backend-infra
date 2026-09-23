using Report.Domain.Entities.Issue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Domain.Contracts
{
    public interface IIssueAttachmentRepo
    {
        Task<IssueAttachment?> GetByIdAsync(Guid id);

        Task<IEnumerable<IssueAttachment>> GetByIssueIdAsync(Guid issueid);

        Task AddAsync(IssueAttachment attachment);

        Task Delete(Guid id);

    }
}
