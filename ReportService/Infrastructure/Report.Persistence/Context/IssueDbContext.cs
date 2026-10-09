using Microsoft.EntityFrameworkCore;
using Report.Domain.Entities.Issue;
using System.Linq;
using System.Reflection;

namespace Report.Persistence.Context
{
    // Connects to IssueDb (owned by IssueService). Never call Migrate() on this context.
    public class IssueDbContext(DbContextOptions<IssueDbContext> options) : DbContext(options)
    {
        public DbSet<Issue> Issues { get; set; } = null!;
        public DbSet<GPSLocation> GPSLocations { get; set; } = null!;
        public DbSet<AiAnalysis> AiAnalyses { get; set; } = null!;
        public DbSet<IssueAttachment> IssueAttachments { get; set; } = null!;

        public DbSet<StatusHistory> StatusHistories { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Only apply configurations for entities in the Issue namespace
            modelBuilder.ApplyConfigurationsFromAssembly(
                Assembly.GetExecutingAssembly(),
                type => type.GetInterfaces().Any(i =>
                    i.IsGenericType &&
                    i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>) &&
                    i.GetGenericArguments()[0].Namespace == "Report.Domain.Entities.Issue"));

            // Table names must match what IssueService's migration created
            modelBuilder.Entity<Issue>().ToTable("Issues");
            modelBuilder.Entity<IssueAttachment>().ToTable("IssueAttachment");
        }
    }
}