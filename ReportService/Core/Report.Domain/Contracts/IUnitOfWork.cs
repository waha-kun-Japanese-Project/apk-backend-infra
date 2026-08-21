using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Domain.Contracts
{
    public  interface IUnitOfWork: IDisposable
    {
        IReportRepo ReportRepo { get; }
        IReportAttachmentRepo ReportAttachmentRepo { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
