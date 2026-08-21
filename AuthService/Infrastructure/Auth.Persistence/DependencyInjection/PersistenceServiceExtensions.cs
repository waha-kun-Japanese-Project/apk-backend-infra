using Auth.Domain.Contracts;

using Auth.Domain.Entities;
using Auth.Persistence.Context;
using Auth.Persistence.Repositories;
using Auth.Persistence.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Persistence.DependencyInjection
{
    public static class PersistenceServiceExtensions
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConnectionMultiplexer>(cfig =>
            {

                return ConnectionMultiplexer.Connect(configuration.GetConnectionString("RedisConnection")!);
            }
           );
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("SQLConnection"));

            });
         
            services.AddScoped<IDbInitializer, DbInitializers.DbInitializer>();
            services.AddScoped<IOtpRepository,OtpRepository>();
            services.AddScoped<ITokenRefreshRepository, RefreshTokenRepository>();




            ConfigureIdentity  (services, configuration);
            return services;
        }
        private  static void   ConfigureIdentity  (this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentityCore<AppUser>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.User.RequireUniqueEmail = false;
            })
                .AddRoles<AppRole>().AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
            services.Configure<DataProtectionTokenProviderOptions>(opt=>
            {
                opt.TokenLifespan = TimeSpan.FromHours(2);
            });

        }
    }
}
