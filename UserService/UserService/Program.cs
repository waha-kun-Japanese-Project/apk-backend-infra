using CommanLib.DependencyInjection;
using User.Persistence.DependancyInjection;
using User.Services.DependencyInjection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace Userservices
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddPersistenceServices(builder.Configuration);
            builder.Services.AddUserServices();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddTokenService(builder.Configuration);
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
            app.MapHealthChecks("/health", new HealthCheckOptions { Predicate = _ => false });

            app.Run();
        }
    }
}