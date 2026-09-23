using Microsoft.EntityFrameworkCore;
using Report.Domain.Contracts;
using Report.Domain.Entities.Issue;
using Report.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Persistence.Repository
{
    public class IssueRepo(IssueDbContext issueDb) : IIssueRepo
    {
        public async Task<Issue> AddAsync(Issue issue)
        {
            await issueDb.Issues.AddAsync(issue);
           
            return issue;
        }

        public async Task DeleteAsync(Guid id)
        {
            var report = await GetByIdAsync(id);
            if (report is null)
                throw new Exception("Report not found");

            issueDb.Issues.Remove(report);

        }

        public async Task<IEnumerable<Issue>> GetAllAsync()
        {

           return await Query().ToListAsync();
        }

        public async Task<Issue> GetByIdAsync(Guid id)
        {
            return await Query().FirstOrDefaultAsync(r => r.Id == id);

        }

        public async Task<IEnumerable<Issue>> GetByReporterIdAsync(Guid reporterId)
        {

            return await Query().Where(r => r.ReporterId == reporterId).ToListAsync();


        }

      

        public async Task UpdateAsync(Issue issue)
        {
            issueDb.Issues.Update(issue);
           
            
        }
        private IQueryable<Issue> Query()
        {
            return issueDb.Issues
                .Include(r => r.GPSLocation)
                .Include(r => r.IssueAttachments)
                .Include(r => r.AiAnalyses);
        }
    }

}

