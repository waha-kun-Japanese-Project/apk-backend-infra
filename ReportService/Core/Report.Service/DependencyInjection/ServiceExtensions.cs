using Hangfire;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Report.Service.BackgroundJop;
using Report.Service.Services;
using Report.ServiceAbstraction;
using Hangfire.SqlServer;
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
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(
                    configuration.GetConnectionString("HangfireConnection"),
                    new SqlServerStorageOptions
                    {
                        SchemaName = "ReportHangfire",         // isolated from IssueService
                        PrepareSchemaIfNecessary = true,
                        QueuePollInterval = TimeSpan.Zero,
                        UseRecommendedIsolationLevel = true,
                        DisableGlobalLocks = true
                    }));
            services.AddHangfireServer();

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
