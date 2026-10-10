using Hangfire;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Report.Service.BackgroundJop;
using Report.Service.Services;
using Report.ServiceAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Service.DependencyInjection
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddReportService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(cfg => cfg.AddMaps(typeof(Mapping.IssueProfile).Assembly));

            services.AddScoped<IIssueService, IssueService>();

            services.AddScoped<IIssueCreationJob, IssueCreationJob>();

            services.AddHangfire(config => config
               .UseSimpleAssemblyNameTypeSerializer()
             .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(
            configuration.GetConnectionString("HangfireConnection")));

            // Server: the background worker that actually executes jobs
            services.AddHangfireServer();
            services.AddMassTransit(x => x.UsingRabbitMq((context, cfg) =>
            {
                // Reads RabbitMq__Host / Port / Username / Password (k8s env vars); falls back to local defaults.
                var host = configuration["RabbitMq:Host"] ?? "localhost";
                var port = ushort.TryParse(configuration["RabbitMq:Port"], out var p) ? p : (ushort)5672;

                cfg.Host(host, port, "/", h =>
                {
                    h.Username(configuration["RabbitMq:Username"] ?? "guest");
                    h.Password(configuration["RabbitMq:Password"] ?? "guest");
                });
            }));
            return services;
        }
    }
}
