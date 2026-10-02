using Microsoft.EntityFrameworkCore;
using Report.Domain.Entities.Report;
using System.Linq;
using System.Reflection;

namespace Report.Persistence.Context
{
    public class ReportDbContext(DbContextOptions<ReportDbContext> options) : DbContext(options)
    {
        public DbSet<Domain.Entities.Report.Report> Reports { get; set; } = null!;
        public DbSet<ReportAttachment> ReportAttachments { get; set; } = null!;
        public DbSet<AiAnalysis> AiAnalyses { get; set; } = null!;
        public DbSet<GPSLocation> GPSLocations { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // CHANGED: filtered the same way as IssueDbContext, for the same reason -
            // this context happened to work before only because no Issue-namespace
            // IEntityTypeConfiguration<T> existed yet. Scoping it explicitly means
            // adding one later (e.g. an IssueAiAnalysisConfiguration) can never leak
            // into this context's model by accident.
            modelBuilder.ApplyConfigurationsFromAssembly(
                Assembly.GetExecutingAssembly(),
                type => type.GetInterfaces().Any(i =>
                    i.IsGenericType &&
                    i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>) &&
                    i.GetGenericArguments()[0].Namespace == "Report.Domain.Entities.Report"));
        }
   }
}