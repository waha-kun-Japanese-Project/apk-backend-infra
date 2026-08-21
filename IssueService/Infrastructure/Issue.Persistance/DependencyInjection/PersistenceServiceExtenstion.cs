using Issue.Persistence.Context;
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
            services.AddDbContext<ReportDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("ReportConnection"));
            });
            services.AddDbContext<AuthDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("AuthSqlConnection"));
            });
            services.AddDbContext<IssueDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("SQLConnection"));
            });




            return services;
        }
    }
}
