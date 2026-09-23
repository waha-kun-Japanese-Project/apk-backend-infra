using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Domain.Contracts
{
    public  interface IUnitOfWork: IDisposable
    {
        IIssueRepo issueRepo{ get; }

        IIssueAttachmentRepo issueAttachmentRepo { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
