using Auth.Domain.Contracts;
using Auth.Domain.Contracts;
using Auth.ServiceAbstraction;
using MassTransit;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notification.Service;
using Notification.ServicesAbstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Service.DependanceInjection
{
    public static  class ServiceExtensions
    {
        // CHANGED: now takes IConfiguration so RabbitMQ host/user/pass come
        // from appsettings.json / env vars instead of being hardcoded to
        // "localhost"/"guest"/"guest" - previously this meant the service
        // could never actually reach RabbitMQ once deployed anywhere other
        // than a developer's own machine.
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IOTPService, OtpService>();
            //services.AddScoped<ISmsService, SmsService>();
            //services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IFireBaseService, FireBaseService>();

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