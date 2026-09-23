using Microsoft.EntityFrameworkCore;
using Report.Domain.Entities.Issue;
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
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
   }
}