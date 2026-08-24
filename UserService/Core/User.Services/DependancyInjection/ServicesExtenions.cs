using MassTransit;
using Microsoft.Extensions.Configuration;
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
        // CHANGED: now takes IConfiguration - was hardcoding "localhost" /
        // "guest" / "guest", same issue as AuthService.
        public static IServiceCollection AddUserServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(cfg => cfg.AddMaps(typeof(Mapping.UserProfile).Assembly));
            services.AddScoped<IUserService, UserService>();

            services.AddMassTransit(x => x.UsingRabbitMq((context, cfg) =>
            {
                var host = configuration["RabbitMq:Host"] ?? "localhost";
                var port = configuration.GetValue<ushort?>("RabbitMq:Port") ?? 5672;
                var username = configuration["RabbitMq:Username"] ?? "guest";
                var password = configuration["RabbitMq:Password"] ?? "guest";

                cfg.Host(host, port, "/", h =>
                {
                    h.Username(username);
                    h.Password(password);
                });
            }));

            return services;
        }
    }
}