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
    public class Unitofwork(ReportDbContext reportDb) : IUnitOfWork
    {
        public IReportRepo ReportRepo { get; }= new ReportRepo(reportDb);

        public IReportAttachmentRepo ReportAttachmentRepo { get; } = new ReportAttachmentRepo(reportDb);

    

        public void Dispose()
        {
           reportDb.Dispose();
        }

        public async Task<int> SaveChangesAsync(
       CancellationToken cancellationToken = default)
        {
            return await reportDb.SaveChangesAsync(cancellationToken);
        }
    }
}
