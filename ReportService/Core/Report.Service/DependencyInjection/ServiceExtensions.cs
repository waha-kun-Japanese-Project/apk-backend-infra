using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Report.Service.Services;
using Report.ServiceAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Service.DependencyInjection
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddReportService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(cfg => cfg.AddMaps(typeof(Mapping.Profile.IssueProfile).Assembly));
        
            services.AddScoped<IIssueService, IssueService>();
            return services;
        }
    }
}
