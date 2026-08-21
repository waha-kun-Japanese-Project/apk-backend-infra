

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Report.Domain.Entities.Auth;



namespace Report.Persistence.Context
    {
        public class AuthDbContext(DbContextOptions<AuthDbContext> options)
            : IdentityDbContext<AppUser, AppRole, Guid>(options)
        {
            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder); // keeps Identity's own table config

       
              
            }
        }
    }

