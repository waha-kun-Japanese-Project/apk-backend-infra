
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Report.Domain.Entities.Issue;


namespace Report.Domain.Contracts
{
    public interface IIssueRepo
    {
        Task<Issue> AddAsync(Issue issue); 
        Task<Issue> GetByIdAsync(Guid id); 
        Task<IEnumerable<Issue>> GetAllAsync(); 
        Task<IEnumerable<Issue>> GetByReporterIdAsync(Guid issueid); 
        Task UpdateAsync(Issue issue); 
        Task DeleteAsync(Guid id);
   
    }
}
