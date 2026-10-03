using Issue.Client.DependencyInjection;
using Issue.Persistence.DependencyInjection;
using Issue.Service.DependencyInjection;
using UserClinet.Grpc;

namespace IssueService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            // HttpContext
            builder.Services.AddHttpContextAccessor();

            // Persistence
            builder.Services.AddPersistenceServices(
                builder.Configuration);

            // Clients
            builder.Services.AddIssueClient(
                builder.Configuration);

            // Services
            builder.Services.AddServiced(
                builder.Configuration);

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}