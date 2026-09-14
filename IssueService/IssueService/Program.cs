using Issue.Persistence.DependencyInjection;
using Issue.Service.Grpc.Services;
using Issue.ServiceAbstraction.Grpc;
using UserClinet.Grpc;
using Issue.Service.DependencyInjection;  
namespace IssueService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
       
            builder.Services.AddPersistenceServices(builder.Configuration);
            builder.Services.AddServiced(builder.Configuration);
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IUserGrpcClient, UserGrpcClient>();
            builder.Services.AddGrpcClient<UserService.UserServiceClient>(
                options =>
                {
                    options.Address = new Uri(
                    builder.Configuration["Grpc:UserServiceUrl"]!);
                });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseAuthentication();


            app.MapControllers();

            app.Run();
        }
    }
}
