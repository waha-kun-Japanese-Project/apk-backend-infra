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
            services.AddDbContext<ReportDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("SQLConnection"));
            });
            services.AddDbContext<AuthDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("AuthSqlConnection"));
            });

            services.AddScoped<IReportRepo, ReportRepo>();
            services.AddScoped<IReportAttachmentRepo, ReportAttachmentRepo>();
            services.AddScoped<IUnitOfWork, Unitofwork>();

            return services;
        }
    }
}