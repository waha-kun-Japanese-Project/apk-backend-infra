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
using MediaClient.Grpc;

namespace Issue.Client.DependencyInjection
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddIssueClient(this IServiceCollection services,IConfiguration configuration)
        {

            services.AddScoped<IMediaStorageGrpcClient, MediaStorageGrpcClient>();
            services.AddScoped<IUserGrpcClient,UserGrpcClient>();
            services.AddGrpcClient<UserClinet.Grpc.UserService.UserServiceClient>(
                options =>
                {
                    options.Address = new Uri(
                    configuration["Grpc:UserServiceUrl"]!);
                });
            services.AddGrpcClient<UserService.Grpc.ExpertService.ExpertServiceClient>(
                options =>
                {
                    options.Address = new Uri(
                    configuration["Grpc:UserServiceUrl"]!);
                });
            services.AddGrpcClient<MediaStorage.MediaStorageClient>(options =>
            {
                options.Address = new Uri(
                    configuration["Grpc:MediaStorageUrl"]!);
            });


            return services;
        }
    }
}
