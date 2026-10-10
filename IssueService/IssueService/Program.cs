using CommanLib.DependencyInjection;
using Hangfire;
using Issue.Client.DependencyInjection;
using Issue.Persistence.Context;
using Issue.Persistence.DependencyInjection;
using Issue.Service.DependencyInjection;
using Issue.Service.Jop;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using UserClinet.Grpc;

namespace IssueService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            }); 

            // HttpContext
            builder.Services.AddHttpContextAccessor();

            // Persistence
            builder.Services.AddPersistenceServices(
                builder.Configuration);

            // Clients
            builder.Services.AddIssueClient(
                builder.Configuration);

            // Services
            builder.Services.AddServiced(
                builder.Configuration);
            builder.Services.AddTokenService(builder.Configuration);
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Issue Service API",
                    Version = "v1"
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter JWT Token.\n\nExample:\nBearer eyJhbGciOiJIUzI1NiIs..."
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
            var app = builder.Build();

            // Create IssueDb (if it does not exist) and apply all pending migrations.
            // This must run BEFORE Hangfire and the recurring job below touch the database.
            using (var migrationScope = app.Services.CreateScope())
            {
                var dbContext = migrationScope.ServiceProvider
                    .GetRequiredService<IssueDbContext>();

                dbContext.Database.Migrate();
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHangfireDashboard("/hangfire");
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            using (var scope = app.Services.CreateScope())
            {
                var recurringJobManager =
                    scope.ServiceProvider
                        .GetRequiredService<IRecurringJobManager>();

                recurringJobManager.AddOrUpdate<ExpertAssignmentReconciliationJob>(
                    "expert-assignment-reconciliation",
                    job => job.ExecuteAsync(),
                    "*/20 * * * *");
            }

            app.Run();
        }
    }
}