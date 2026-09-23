using AutoMapper;
using Community.Service.Maaping;
using Community.ServiceAbstraction;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Community.Service.DependanceInjection
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<ICountService,CountService>();
            services.AddScoped<IcommunityService, CommunityService>();
            services.AddAutoMapper(cfg => cfg.AddMaps(typeof(Communitymap).Assembly));


            return services;
        }
    }
}
