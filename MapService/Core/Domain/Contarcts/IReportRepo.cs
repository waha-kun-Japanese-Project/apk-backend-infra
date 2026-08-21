using Issue.Domain.Entities.Issue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Map.Domain.Contarcts
{
    public interface IReportREpo
    {
        Task<Issue.Domain.Entities.Report.Report?> GetByIdAsync(Guid id);
        Task<IEnumerable<Issue.Domain.Entities.Report.Report>> GetAllAsync();
    }
}
