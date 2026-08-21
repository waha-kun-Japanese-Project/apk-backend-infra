using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Contract;
using User.Domain.Entities;
using User.Persistence.Context;
using User.Persistence.Repository;

namespace User.Persistence.DependancyInjection
{
    public static class PersistenceServiceExtensions
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("SQLConnection")));

            services.AddScoped<IUserRepo, UserRepo>();

            ConfigureIdentity(services, configuration);
            return services;
        }
        private static void ConfigureIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentityCore<AppUser>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.User.RequireUniqueEmail = false;
            })
                .AddRoles<AppRole>().AddEntityFrameworkStores<AppDbContext>();

        }
    }
}
