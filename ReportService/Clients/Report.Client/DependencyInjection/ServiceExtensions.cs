using MediaClient.Grpc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using Report.Client.AbstructServices;
using Report.Client.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Report.Client.DependencyInjection
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddReportClient(this IServiceCollection services, IConfiguration configuration)
        {

            //services.AddRefitClient<IAiVisionClient>()
            //    .ConfigureHttpClient(c =>
            //        c.BaseAddress = new Uri(configuration["Services:AI:BaseUrl"]!));
            services.AddRefitClient<IAiVisionClient>()
       .ConfigureHttpClient(c =>
       {
           c.BaseAddress = new Uri(configuration["Services:AI:BaseUrl"]!);
           c.Timeout = TimeSpan.FromSeconds(60);
           c.DefaultRequestHeaders.Add("ngrok-skip-browser-warning", "true");
       }
           );

            services.AddScoped<IMediaStorageGrpcClient, MediaStorageGrpcClient>();
           
            services.AddGrpcClient<MediaStorage.MediaStorageClient>(options =>
            {
                options.Address = new Uri(configuration["Services:Storage:BaseUrl"]);
            });


            return services;
        }
    }
}
