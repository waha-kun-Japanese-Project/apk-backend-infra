using Minio;
using Media.Service;
using Media.ServiceAbstraction;
using Media.Settings;
using Microsoft.Extensions.Configuration;
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
            builder.Services.AddHealthChecks();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            if (!app.Environment.IsProduction())
            {
                app.UseHttpsRedirection();
            }
            app.UseAuthorization();

            app.MapControllers();
            app.MapHealthChecks("/health");

            app.Run();
        }
    }
}