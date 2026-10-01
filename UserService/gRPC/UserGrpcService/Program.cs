using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using User.Persistence.DependancyInjection;
using UserGrpcService.Services;

namespace UserGrpcService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddGrpc();
            // AppDbContext (used by UserGRPsService) was never registered anywhere —
            // every call would have thrown at request time. Reusing UserService's own
            // persistence wiring keeps the connection-string key ("SQLConnection") consistent.
            builder.Services.AddPersistenceServices(builder.Configuration);
            builder.Services.AddHealthChecks();

            var app = builder.Build();

            app.MapGrpcService<UserGRPsService>();
            app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
            app.MapHealthChecks("/health", new HealthCheckOptions { Predicate = _ => false });

            app.Run();
        }
    }
}