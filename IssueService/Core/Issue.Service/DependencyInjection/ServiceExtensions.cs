
using CommanLib.EventNotification.IssueEvent;
using Hangfire;
using Issue.Service.Consumer;
using Issue.Service.Jop;
using Issue.Service.Services;
using Issue.ServiceAbstraction.Expert;
using Issue.ServiceAbstraction.Jop;
using MassTransit;
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
            services.AddScoped<IExpertAssignmentJob, ExpertAssignmentJob>();
            services.AddScoped<IExpertService, ExpertService>();
            services.AddScoped<IAssignExpertServices, AssignExpertServices>();
            services.AddScoped<IConsumer<IssueCreatedEvent>, IssueCreatedConsumer>();
            services.AddScoped< ExpertAssignmentReconciliationJob>();

            services.AddMassTransit(x =>
            {
                x.AddConsumer<IssueCreatedConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    // Reads RabbitMq__Host / Port / Username / Password (k8s env vars); falls back to local defaults.
                    var host = configuration["RabbitMq:Host"] ?? "localhost";
                    var port = ushort.TryParse(configuration["RabbitMq:Port"], out var p) ? p : (ushort)5672;

                    cfg.Host(host, port, "/", h =>
                    {
                        h.Username(configuration["RabbitMq:Username"] ?? "guest");
                        h.Password(configuration["RabbitMq:Password"] ?? "guest");
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });

            services.AddHangfire(config =>
            {
                config.UseSqlServerStorage(
                    configuration.GetConnectionString(
                        "HangfireConnection"));
            });

           services.AddHangfireServer();

            
       


            return services;
        }
    }
}
