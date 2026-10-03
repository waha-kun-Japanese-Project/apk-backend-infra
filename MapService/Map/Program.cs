using CommanLib.DependencyInjection;
using Map.Persistence.DedpendeancyInjection;
using Map.Service.DependanceInjection;
using Microsoft.OpenApi.Models;

namespace Map
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddPersistenceServices(builder.Configuration);
            builder.Services.AddServices();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddTokenService(builder.Configuration);
            builder.Services.AddHealthChecks();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Map Service API", Version = "v1" });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Paste the accessToken only (Swagger adds 'Bearer ')."
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            var app = builder.Build();

            app.Use(async (context, next) =>
            {
                try { await next(); }
                catch (KeyNotFoundException ex) when (!context.Response.HasStarted)
                {
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    await context.Response.WriteAsJsonAsync(new { status = 404, code = "NOT_FOUND", message = ex.Message });
                }
            });

            app.UseSwagger();
            app.UseSwaggerUI();
            if (!app.Environment.IsProduction())
            {
                app.UseHttpsRedirection();
            }
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.MapHealthChecks("/health");

            app.Run();
        }
    }
}