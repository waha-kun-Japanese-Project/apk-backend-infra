
using Map.Domain.Contarcts;
using Map.Persistence.Context;
using Map.Persistence.Repo;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Map.Persistence.DedpendeancyInjection;

public static class PersistenceServiceExtensions
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services ,IConfiguration configuration)
    {
      
        
        services.AddDbContext<IssueDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("SQLConnection"));
        });


       
        services.AddScoped<IIssueRepo, IssueRepo>();


        return services;
    }
    

    
}
