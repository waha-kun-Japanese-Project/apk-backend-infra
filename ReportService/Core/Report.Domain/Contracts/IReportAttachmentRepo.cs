using Report.Domain.Entities.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Domain.Contracts
{
    public interface IReportAttachmentRepo
    {
        Task<ReportAttachment?> GetByIdAsync(Guid id);

        Task<IEnumerable<ReportAttachment>> GetByReportIdAsync(Guid reportId);

        Task AddAsync(ReportAttachment attachment);

        Task Delete(Guid id);

    }
}
