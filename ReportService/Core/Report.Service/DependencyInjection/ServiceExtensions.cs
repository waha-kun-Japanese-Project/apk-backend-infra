using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Report.Service.Services;
using Report.ServiceAbstraction;

namespace Report.Service.DependencyInjection
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddReportService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(cfg => cfg.AddMaps(typeof(Mapping.Profile.IssueProfile).Assembly));

            services.AddScoped<IIssueService, IssueService>();
            services.AddScoped<IReportService, Report.Service.Services.ReportService>();
            return services;
        }
    }
}