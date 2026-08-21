using Microsoft.EntityFrameworkCore;
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
    public class ReportRepo(ReportDbContext reportDb) : IReportRepo
    {
        public async Task<Domain.Entities.Report.Report> AddAsync(Domain.Entities.Report.Report report)
        {
            await reportDb.Reports.AddAsync(report);
           
            return report;
        }

        public async Task DeleteAsync(Guid id)
        {
            var report = await GetByIdAsync(id);
            if (report is null)
                throw new Exception("Report not found");

            reportDb.Reports.Remove(report);

        }

        public async Task<IEnumerable<Domain.Entities.Report.Report>> GetAllAsync()
        {

           return await Query().ToListAsync();
        }

        public async Task<Domain.Entities.Report.Report?> GetByIdAsync(Guid id)
        {
            return await Query().FirstOrDefaultAsync(r => r.Id == id);

        }

        public async Task<IEnumerable<Domain.Entities.Report.Report>> GetByReporterIdAsync(Guid reporterId)
        {

            return await Query().Where(r => r.ReporterId == reporterId).ToListAsync();


        }

      

        public async Task UpdateAsync(Domain.Entities.Report.Report report)
        {
            reportDb.Reports.Update(report);
           
            
        }
        private IQueryable<Domain.Entities.Report.Report> Query()
        {
            return reportDb.Reports
                .Include(r => r.Location)
                .Include(r => r.Attachments)
                .Include(r => r.Analysis);
        }
    }

}

