using Community.Clinets.ServiceAbstrction;
using Community.Clinets.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Community.Clinets.DependancyInjection;

public static class ExtiosionService
{
    public static IServiceCollection AddClientService(this IServiceCollection services ,IConfiguration configuration)
    {
        services.AddHttpClient<ICommentModerationClient, CommentModerationClient>(client =>
        {
            client.BaseAddress = new Uri(configuration["ModerationService:BaseUrl"]!); // "http://localhost:8000/"
            client.Timeout = TimeSpan.FromSeconds(5);
        });
        return services;

    }
}