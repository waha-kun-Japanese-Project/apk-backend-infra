using Issue.Client.DependencyInjection;
using Issue.Persistence.DependencyInjection;
using Issue.Service.DependencyInjection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace IssueService
{
    public class Program
    {
        public static void Main(string[] args)
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

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseAuthentication();

            app.MapControllers();
            app.MapHealthChecks("/health", new HealthCheckOptions { Predicate = _ => false });
            app.Run();
        }
    }
}