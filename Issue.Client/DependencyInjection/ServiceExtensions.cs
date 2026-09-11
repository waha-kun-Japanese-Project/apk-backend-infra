using Issue.Client.ServiceAbstraction;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Issue.Client.Grpc.Services;

using UserClinet.Grpc;

namespace Issue.Client.DependencyInjection
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddIssueClient(this IServiceCollection services,IConfiguration configuration)
        {
            services
                   .AddRefitClient<IUserService>()
                   .ConfigureHttpClient(client =>
                    {
                      client.BaseAddress = new Uri(
                      configuration["Services:User:BaseUrl"]!);
                    });

            services.AddScoped<IUserGrpcClient,UserGrpcClient>();
            services.AddGrpcClient<UserService.UserServiceClient>(
                options =>
                {
                    options.Address = new Uri(
                    configuration["Grpc:UserServiceUrl"]!);
                });
           
            return services;
        }
    }
}
