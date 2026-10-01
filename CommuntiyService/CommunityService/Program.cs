using CommanLib.DependencyInjection;
using Community.Clinets.DependancyInjection;
using Community.Persistence.DependanceInjection;
using Community.Service;
using Community.Service.DependanceInjection;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;

namespace CommunityService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSignalR();
            builder.Services.AddServices();
            builder.Services.AddPersistenceServices(builder.Configuration);
            builder.Services.AddTokenService(builder.Configuration);
            builder.Services.AddClientService(builder.Configuration);
            builder.Services.AddHealthChecks();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Report Service API",
                    Version = "v1"
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter JWT Token.\n\nExample:\nBearer eyJhbGciOiJIUzI1NiIs..."
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            // Was: ConnectionMultiplexer.Connect(...) called eagerly, before Build().
            // If Redis wasn't reachable at that exact moment, the whole process crashed
            // before Kestrel ever bound to 8080 -> "connection refused" on every probe.
            // Fix: register as a lazy factory (only resolved when actually needed, which
            // happens after the app is already listening), and set AbortOnConnectFail = false
            // so a transient Redis outage retries in the background instead of throwing.
            builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var connectionString = builder.Configuration.GetConnectionString("RedisConnection");
                var options = ConfigurationOptions.Parse(connectionString);
                options.AbortOnConnectFail = false;
                return ConnectionMultiplexer.Connect(options);
            });

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
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapHub<CommunityHub>("/hubs/community");
            app.MapControllers();
            app.MapHealthChecks("/health");

            app.Run();
        }
    }
}