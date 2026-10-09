using Issue.Domain.Contract;
using Issue.Persistence.Context;
using Issue.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Persistence.DependencyInjection
{
    public static class PersistenceServiceExtension
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
         
           
            services.AddDbContext<IssueDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("SQLConnection"),
                    sqlOptions =>
                        sqlOptions.MigrationsAssembly(typeof(IssueDbContext).Assembly.FullName)));


            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
            services.AddScoped<ITransaction, Transaction>();

            return services;
        }
    }
}
