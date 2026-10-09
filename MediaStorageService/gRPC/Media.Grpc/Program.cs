using Media.Grpc.Services;
using Minio;
using Media.Service;
using Media.ServiceAbstraction;
using Media.Settings;
using Microsoft.Extensions.Options;

namespace Media.Grpc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddGrpc();

            builder.Services.Configure<MinioSettings>(
                builder.Configuration.GetSection("MinioSettings"));
            builder.Services.AddSingleton<IMinioClient>(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<MinioSettings>>().Value;

                var client = new MinioClient()
                    .WithEndpoint(settings.Endpoint)
                    .WithCredentials(settings.AccessKey, settings.SecretKey)
                    .WithRegion("us-east-1");   // added - fixes AccessDenied caused by missing region in signature

                if (settings.UseSSL)
                {
                    client = client.WithSSL();
                }

                return client.Build();
            });
            builder.Services.AddScoped<IStorageService, MinioStorageService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.MapGrpcService<MediaService>();
            app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

            app.Run();
        }
    }
}