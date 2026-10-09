
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
