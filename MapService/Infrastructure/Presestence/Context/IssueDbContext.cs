using Map.Domain.Entities.ISSUE;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Map.Persistence.Context
{
    // Read-only view of IssueDb. IssueService owns and migrates it; never call Migrate() here.
    public class IssueDbContext(DbContextOptions<IssueDbContext> options) : DbContext(options)
    {
        public DbSet<Map.Domain.Entities.ISSUE.Issue> Issues { get; set; } = null!;
        public DbSet<Map.Domain.Entities.ISSUE.GPSLocation> GPSLocations { get; set; } = null!;
        public DbSet<Map.Domain.Entities.ISSUE.IssueAttachment> IssueAttachments { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // Table names must match what IssueService's migration created.
            modelBuilder.Entity<Map.Domain.Entities.ISSUE.Issue>().ToTable("Issues");
            modelBuilder.Entity<Map.Domain.Entities.ISSUE.GPSLocation>().ToTable("GPSLocation");
            modelBuilder.Entity<Map.Domain.Entities.ISSUE.IssueAttachment>().ToTable("IssueAttachment");

            modelBuilder.Entity<Map.Domain.Entities.ISSUE.Issue>()
                .HasOne(i => i.GPSLocation)
                .WithOne(g => g.Issue)
                .HasForeignKey<Map.Domain.Entities.ISSUE.Issue>(i => i.GPSLocationId);

            modelBuilder.Entity<Map.Domain.Entities.ISSUE.Issue>()
                .HasMany(i => i.IssueAttachments)
                .WithOne(a => a.Issue)
                .HasForeignKey(a => a.IssueId);
        }
    }
}