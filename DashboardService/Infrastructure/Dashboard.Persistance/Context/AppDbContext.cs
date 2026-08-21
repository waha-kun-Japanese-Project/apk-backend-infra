using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Dashboard.Domain.Entities;

namespace Dashboard .Persistence.Context
{
    internal class AppDbContext(DbContextOptions<AppDbContext> options):
        IdentityDbContext<AppUser, AppRole, Guid>(options)

    {
        public DbSet<Address> Addresses { get; set; }


    }
}
