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

            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
                    .WithRegion("us-east-1");   // added — fixes AccessDenied caused by missing region in signature

                if (settings.UseSSL)
                {
                    client = client.WithSSL();
                }

                return client.Build();
            });
            builder.Services.AddScoped<IStorageService, MinioStorageService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}