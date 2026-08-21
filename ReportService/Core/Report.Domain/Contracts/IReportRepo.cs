
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Report.Domain.Entities.Report;


namespace Report.Domain.Contracts
{
    public interface IReportRepo
    {
        Task<Report.Domain.Entities.Report.Report> AddAsync(Report.Domain.Entities.Report.Report report); 
        Task<Report.Domain.Entities.Report.Report?> GetByIdAsync(Guid id); 
        Task<IEnumerable<Report.Domain.Entities.Report.Report>> GetAllAsync(); 
        Task<IEnumerable<Report.Domain.Entities.Report.Report>> GetByReporterIdAsync(Guid reporterId); 
        Task UpdateAsync(Report.Domain.Entities.Report.Report report); 
        Task DeleteAsync(Guid id);
   
    }
}
