using Community.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Community.Persistence
{
    public class IssueDbContext(DbContextOptions<IssueDbContext> options) : DbContext(options)
    {
        public DbSet<Community.Domain.Entities.Issue> Issues { get; set; } = null!;
        public DbSet<Comment> Comments { get; set; } = null!;
        public DbSet<IssueShared> Shares { get; set; }
        public DbSet<IssueVote> Votes { get; set; }
      

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
