using Issue.Domain.Entities.Auth;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Persistence.Context
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
