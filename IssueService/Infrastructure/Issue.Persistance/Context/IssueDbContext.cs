using Issue.Domain.Entities.Issue;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Persistence.Context
{
    public class IssueDbContext(DbContextOptions<IssueDbContext> options) : DbContext(options)
    {
        public DbSet<Issue.Domain.Entities.Issue.Issue> Issues { get; set; } = null!;
        public DbSet<Comment> Comments { get; set; } = null!;
        public DbSet<IssueFeedback> issueFeedbacks { get; set; } = null!;
        public DbSet<StatusHistory> StatusHistories { get; set; } = null!;
        public DbSet<ResolutionActions> ResolutionActions { get; set; } = null!;
        public DbSet<MaintenanceTeam> MaintenanceTeams { get; set; } = null!;
        public DbSet<RepairSchedule> RepairSchedules { get; set; } = null!;
        public DbSet<Notification> notifications { get; set; } = null!;
        public DbSet<ExpertReviews> ExpertReviews { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

    }
}
