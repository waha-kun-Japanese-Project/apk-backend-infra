using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Report.Domain.Contracts;
using Report.Persistence.Context;
using Report.Persistence.Repository;
using Report.Persistence.UnitOfWork;

namespace Report.Persistence.DependencyInjection
{
    public static class PersistenceServiceExtensions
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            // ReportDb - owned and migrated by ReportService
            services.AddDbContext<ReportDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("ReportSqlConnection"));
            });

            // IssueDb - owned and migrated by IssueService (ReportService only reads/writes it)
            services.AddDbContext<IssueDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("SQLConnection"));
            });
           

     
            services.AddScoped<IUnitOfWork, Unitofwork>();

            return services;
        }
    }
}