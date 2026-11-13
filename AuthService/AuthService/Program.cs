using Auth.Domain.Contracts;
using Auth.Persistence.DependencyInjection;
using Auth.Persistence.Context;
using Auth.Service;
using Auth.Service.DependanceInjection;
using CommanLib.DependencyInjection;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;


namespace Auth_Services
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var firebasePath = Path.Combine(
                builder.Environment.ContentRootPath,
                "FireBase",
                "waha-kan-firebase-adminsdk-fbsvc-2b7ce8a678.json");

            FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.FromFile(firebasePath)
            });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddPersistenceServices(builder.Configuration);
            builder.Services.AddTokenService(builder.Configuration);
            builder.Services.AddServices();
            builder.Services.AddHealthChecks();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider
                    .GetRequiredService<AppDbContext>();

                await db.Database.MigrateAsync();

                var initializer = scope.ServiceProvider
                    .GetRequiredService<IDbInitializer>();

                await initializer.InitializeAsync();
            }
            app.UseSwagger();
            app.UseSwaggerUI();
            if (!app.Environment.IsProduction())
            {
                app.UseHttpsRedirection();
            }
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.MapHealthChecks("/health",
                new HealthCheckOptions { Predicate = _ => false });

            app.Run();
        }
    }
}