using Map.Domain.Entities.ISSUE;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Map.Persistence.Context
{
    public  class IssueDbContext(DbContextOptions<IssueDbContext> options) : DbContext(options)
    {
        public DbSet<Map.Domain.Entities.ISSUE.Issue> Issues { get; set; } = null!;
        public DbSet<GPSLocation> GPSLocations { get; set; } = null!;

        public DbSet<IssueAttachment> IssueAttachments { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
    
}
