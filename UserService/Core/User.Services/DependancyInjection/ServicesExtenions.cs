using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Services.Services;
using User.ServicesAbstract;

namespace User.Services.DependencyInjection
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddUserServices(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg => cfg.AddMaps(typeof(Mapping.UserProfile).Assembly));
            services.AddScoped<IUserService, UserService>();
            services.AddMassTransit(x => x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
            }));


            return services;
        }
    }
}
