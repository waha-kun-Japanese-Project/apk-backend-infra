using Media.Service;
using Media.ServiceAbstraction;
using Media.Settings;
using Minio;
using Microsoft.Extensions.Options;

namespace MediaStorageService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddHealthChecks();

            builder.Services.Configure<MinioSettings>(
                builder.Configuration.GetSection("MinioSettings"));

            builder.Services.AddSingleton<IMinioClient>(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<MinioSettings>>().Value;

                var client = new MinioClient()
                    .WithEndpoint(settings.Endpoint)
                    .WithCredentials(settings.AccessKey, settings.SecretKey)
                    .WithRegion("us-east-1");

                if (settings.UseSSL)
                {
                    client = client.WithSSL();
                }

                return client.Build();
            });

            builder.Services.AddScoped<IStorageService, MinioStorageService>();

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();
            app.UseAuthorization();

            app.MapHealthChecks("/health");
            app.MapControllers();

            app.Run();
        }
    }
}