using Issue.Persistence.Context;
using Map.Domain.Contarcts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Map.Persistence.Repo
{
    public class ReportRepo (Issue.Persistence.Context.ReportDbContext reportDb) : IReportREpo
    {
        public async Task<IEnumerable<Issue.Domain.Entities.Report.Report>> GetAllAsync()
        {

            return 
                await reportDb.Reports
                .Include(x=>x.Location)
                .ToListAsync();
        }

        public async Task<Issue.Domain.Entities.Report.Report?> GetByIdAsync(Guid id)
        {
            return 
                await reportDb.Reports
                .Include(x=>x.Location)
                .FirstOrDefaultAsync(x => x.Id == id);

        }
       

    }
}
