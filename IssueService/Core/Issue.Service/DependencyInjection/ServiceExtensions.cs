using Issue.Service.Concurrency;
using Issue.Service.Services;
using Issue.ServiceAbstraction.Expert;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Service.DependencyInjection
{
    public static   class ServiceExtensions
    {
        public static  IServiceCollection AddServiced(this IServiceCollection services,IConfiguration configuration)
        
            {
            services.AddAutoMapper(cfg => cfg.AddMaps(typeof(MapperingProfiles.IssueProfile).Assembly));

            services.AddScoped<IExpertService, ExpertService>();
            services.AddScoped<IAssignExpertServices, AssignExpertServices>();
            services.AddSingleton<ExpertAssignmentGate>();
            return services;
        }
    }
}
