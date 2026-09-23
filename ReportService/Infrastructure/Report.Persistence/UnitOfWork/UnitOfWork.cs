using Microsoft.EntityFrameworkCore;
using Report.Domain.Contracts;
using Report.Persistence.Context;
using Report.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Persistence.UnitOfWork
{
    public class Unitofwork(IssueDbContext issueDb) : IUnitOfWork
    {
        public IIssueRepo issueRepo { get; }= new IssueRepo(issueDb);

        public IIssueAttachmentRepo issueAttachmentRepo { get; } = new IssueAttachmentRepo(issueDb);

    

        public void Dispose()
        {
           issueDb.Dispose();
        }

        public async Task<int> SaveChangesAsync(
       CancellationToken cancellationToken = default)
        {
            return await issueDb.SaveChangesAsync(cancellationToken);
        }
    }
}
