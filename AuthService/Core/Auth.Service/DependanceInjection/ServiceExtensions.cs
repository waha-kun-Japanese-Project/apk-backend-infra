using Auth.Domain.Contracts;
using Auth.ServiceAbstraction;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notification.Service;
using Notification.ServicesAbstract;
using System;

namespace Auth.Service.DependanceInjection
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IOTPService, OtpService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IFireBaseService, FireBaseService>();

            services.AddMassTransit(x => x.UsingRabbitMq((context, cfg) =>
            {
                // Reads RabbitMq__Host / Port / Username / Password from the K8s env vars.
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