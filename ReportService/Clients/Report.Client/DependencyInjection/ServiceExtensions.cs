using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;

using Report.Client.AbstructServices;
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
            services.AddRefitClient<IStorageClient>()
              .ConfigureHttpClient(c =>
                  c.BaseAddress = new Uri(configuration["Services:Storage:BaseUrl"]!));

            services.AddRefitClient<IAiVisionClient>()
                .ConfigureHttpClient(c =>
                    c.BaseAddress = new Uri(configuration["Services:AI:BaseUrl"]!));


            return services;
        }
    }
}
