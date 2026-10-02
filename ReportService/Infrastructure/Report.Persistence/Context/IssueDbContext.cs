using Microsoft.EntityFrameworkCore;
using Report.Domain.Entities.Issue;
using System.Linq;
using System.Reflection;

namespace Report.Persistence.Context
{
    public class IssueDbContext(DbContextOptions<IssueDbContext> options) : DbContext(options)
    {
        public DbSet<Issue> Issues { get; set; } = null!;
        public DbSet<GPSLocation> GPSLocations { get; set; } = null!;
        public DbSet<AiAnalysis> AiAnalyses { get; set; } = null!;
        public DbSet<IssueAttachment> IssueAttachments { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // CHANGED: was ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly())
            // with no filter - that pulls in EVERY IEntityTypeConfiguration<T> in
            // Report.Persistence, including AiAnalysisConfiguration and
            // GPSLocationConfiguration, which both target the Report.Domain.Entities.Report
            // versions of those classes, not the Report.Domain.Entities.Issue ones used
            // here. Applying a configuration for an entity type adds that type to this
            // context's model even if nothing else references it - so both the
            // Report-namespace AiAnalysis/GPSLocation AND this context's own
            // Issue-namespace AiAnalysis/GPSLocation ended up in the same model, both
            // defaulting to the same table names ("AiAnalyses", "GPSLocations") with no
            // relationship between them - EF's "shared table without a linking FK" error.
            //
            // This filters to only configurations whose configured entity type lives in
            // the Issue namespace, so Report's configurations never leak in here.
            modelBuilder.ApplyConfigurationsFromAssembly(
                Assembly.GetExecutingAssembly(),
                type => type.GetInterfaces().Any(i =>
                    i.IsGenericType &&
                    i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>) &&
                    i.GetGenericArguments()[0].Namespace == "Report.Domain.Entities.Issue"));
        }
    }
}