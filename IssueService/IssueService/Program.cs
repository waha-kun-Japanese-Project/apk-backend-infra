using Issue.Client.DependencyInjection;
using Issue.Persistence.DependencyInjection;
using Issue.Service.DependencyInjection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;

namespace IssueService
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();
            builder.Services.AddPersistenceServices(builder.Configuration);
            builder.Services.AddIssueClient(builder.Configuration);
            builder.Services.AddServiced(builder.Configuration);
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddHealthChecks();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var issueDb = scope.ServiceProvider.GetRequiredService<Issue.Persistence.Context.IssueDbContext>();
                await issueDb.Database.MigrateAsync();
            }

            app.UseSwagger();
            app.UseSwaggerUI();
            if (!app.Environment.IsProduction())
            {
                app.UseHttpsRedirection();
            }
            app.UseAuthorization();
            app.UseAuthentication();

            app.MapControllers();
            app.MapHealthChecks("/health", new HealthCheckOptions { Predicate = _ => false });
            app.Run();
        }
    }
}