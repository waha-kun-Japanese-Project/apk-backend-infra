using Report.Domain.Contracts;
using Report.Domain.Entities.Report;
using Report.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Persistence.Repository
{
    public class ReportAttachmentRepo(ReportDbContext reportDb) : IReportAttachmentRepo
    {
        public async Task AddAsync(ReportAttachment attachment)
        {
            await reportDb.AddAsync(attachment);
        }

        public async Task Delete(Guid id)
        {
            var attachment = await GetByIdAsync(id ) ;
            if (attachment is null)
                throw new Exception("Attachment not found");
            reportDb.ReportAttachments.Remove(attachment);

        }

        public async Task<ReportAttachment?> GetByIdAsync(Guid id)
        {
            return await reportDb.ReportAttachments.FindAsync(id);
        }

        public Task<IEnumerable<ReportAttachment>> GetByReportIdAsync(Guid reportId)
        {
            return Task.FromResult<IEnumerable<ReportAttachment>>(reportDb.ReportAttachments.Where(a => a.ReportId == reportId).ToList());
        }

      
    }
}
