using Map.Service.Mapping.Profile;
using Map.ServiceAbsraction;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Map.Service.DependanceInjection;

public static class ServiceExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => cfg.AddMaps(typeof(MapProfile).Assembly));

        services.AddScoped<IMapSerevice, MapService>();
        services.AddAutoMapper(cfg => cfg.AddMaps(typeof(Mapping.Profile.MapProfile).Assembly));





        return services;
    }
}