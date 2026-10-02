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
    public class Unitofwork(IssueDbContext issueDb, ReportDbContext reportDb) : IUnitOfWork
    {
        public IIssueRepo issueRepo { get; } = new IssueRepo(issueDb);
        public IIssueAttachmentRepo issueAttachmentRepo { get; } = new IssueAttachmentRepo(issueDb);

        public IReportRepo reportRepo { get; } = new ReportRepo(reportDb);
        public IReportAttachmentRepo reportAttachmentRepo { get; } = new ReportAttachmentRepo(reportDb);

        public void Dispose()
        {
            issueDb.Dispose();
            reportDb.Dispose();
        }

        public async Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default)
        {
            // Two separate DbContexts means two separate SaveChanges calls — report-side
            // changes were previously silently lost since only issueDb was ever flushed.
            var issueChanges = await issueDb.SaveChangesAsync(cancellationToken);
            var reportChanges = await reportDb.SaveChangesAsync(cancellationToken);
            return issueChanges + reportChanges;
        }
    }
}