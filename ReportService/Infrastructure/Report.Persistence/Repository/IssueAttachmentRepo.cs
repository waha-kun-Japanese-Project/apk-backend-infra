
using Report.Domain.Contracts;
using Report.Domain.Entities.Issue;

using Report.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Persistence.Repository
{
    public class IssueAttachmentRepo(IssueDbContext issueDbContext) : IIssueAttachmentRepo
    {
        public async Task AddAsync(IssueAttachment attachment)
        {
            await issueDbContext.AddAsync(attachment);
        }

        public async Task Delete(Guid id)
        {
            var attachment = await GetByIdAsync(id ) ;
            if (attachment is null)
                throw new Exception("Attachment not found");
            issueDbContext.IssueAttachments.Remove(attachment);

        }

        public async Task<IssueAttachment?> GetByIdAsync(Guid id)
        {
            return await issueDbContext.IssueAttachments.FindAsync(id);
        }

        public Task<IEnumerable<IssueAttachment>> GetByIssueIdAsync(Guid issueid)
        {
            return Task.FromResult<IEnumerable<IssueAttachment>>(issueDbContext.IssueAttachments.Where(a => a.IssueId == issueid).ToList());
        }

     

      
    }
}
