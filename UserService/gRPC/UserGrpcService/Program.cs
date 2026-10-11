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
            builder.Services.AddPersistenceServices(builder.Configuration);

            var app = builder.Build();

            app.MapGrpcService<UserGRPsService>();
            app.MapGrpcService<ExpertGrpcService>();
            app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
            app.MapGrpcService<ExpertGrpcService>();
            app.Run();
        }
    }
}