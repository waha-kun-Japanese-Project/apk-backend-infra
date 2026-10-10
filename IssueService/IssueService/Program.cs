using CommanLib.DependencyInjection;
using Hangfire;
using Hangfire.AspNetCore;
using Hangfire.Dashboard;
using Issue.Client.DependencyInjection;
using Issue.Persistence.Context;
using Issue.Persistence.DependencyInjection;
using Issue.Service.DependencyInjection;
using Issue.Service.Jop;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text;
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

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddPersistenceServices(builder.Configuration);

            builder.Services.AddIssueClient(builder.Configuration);

            builder.Services.AddServiced(builder.Configuration);

            builder.Services.AddTokenService(builder.Configuration);

            builder.Services.AddHealthChecks();
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

            using (var migrationScope = app.Services.CreateScope())
            {
                var dbContext = migrationScope.ServiceProvider
                    .GetRequiredService<IssueDbContext>();

                dbContext.Database.Migrate();
            }

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new IDashboardAuthorizationFilter[]
                {
                    new HangfireBasicAuthFilter(
                        app.Configuration["Hangfire:DashboardUser"] ?? string.Empty,
                        app.Configuration["Hangfire:DashboardPassword"] ?? string.Empty)
                }
            });

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.MapHealthChecks("/health");

            using (var scope = app.Services.CreateScope())
            {
                var recurringJobManager =
                    scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();

                recurringJobManager.AddOrUpdate<ExpertAssignmentReconciliationJob>(
                    "expert-assignment-reconciliation",
                    job => job.ExecuteAsync(),
                    "*/20 * * * *");
            }

            app.Run();
        }
    }

    public sealed class HangfireBasicAuthFilter : IDashboardAuthorizationFilter
    {
        private readonly string _username;
        private readonly string _password;

        public HangfireBasicAuthFilter(string username, string password)
        {
            _username = username;
            _password = password;
        }

        public bool Authorize(DashboardContext context)
        {
            if (string.IsNullOrEmpty(_username) || string.IsNullOrEmpty(_password))
            {
                return false;
            }

            var authorization = context.GetHttpContext().Request.Headers["Authorization"].ToString();
            if (!authorization.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            try
            {
                var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(authorization[6..]));
                var separator = credentials.IndexOf(':');

                return separator >= 0
                    && credentials[..separator] == _username
                    && credentials[(separator + 1)..] == _password;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}