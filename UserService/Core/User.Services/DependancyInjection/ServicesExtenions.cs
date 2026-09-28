using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
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
                var config = context.GetRequiredService<IConfiguration>();
                var host = config["RabbitMq:Host"] ?? "localhost";
                var port = ushort.TryParse(config["RabbitMq:Port"], out var p) ? p : (ushort)5672;

                cfg.Host(host, port, "/", h =>
                {
                    h.Username(config["RabbitMq:Username"] ?? "guest");
                    h.Password(config["RabbitMq:Password"] ?? "guest");
                });
            }));

            return services;
        }
    }
}