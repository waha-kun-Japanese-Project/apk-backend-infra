using CommanLib.DependencyInjection;
using User.Persistence.DependancyInjection;
using User.Services.DependencyInjection;

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

            builder.Services.AddHealthChecks();
            builder.Services.AddPersistenceServices(builder.Configuration);
            builder.Services.AddUserServices(builder.Configuration);
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddTokenService(builder.Configuration);

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapHealthChecks("/health");
            app.MapControllers();

            app.Run();
        }
    }
}