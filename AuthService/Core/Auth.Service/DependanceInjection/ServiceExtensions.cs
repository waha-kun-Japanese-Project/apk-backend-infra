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
        public static IServiceCollection AddServices(this IServiceCollection services )
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IOTPService, OtpService>();
            //services.AddScoped<ISmsService, SmsService>();
            //services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IFireBaseService, FireBaseService>();
            services.AddMassTransit(x=>x.UsingRabbitMq((context, cfg) =>
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
