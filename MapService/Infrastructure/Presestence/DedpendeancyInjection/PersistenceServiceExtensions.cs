using Issue.Persistence.Context;
using Map.Domain.Contarcts;
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
        services.AddDbContext<ReportDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("ReportConnection"));
        });
        services.AddDbContext<AuthDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("AuthSqlConnection"));
        });
        services.AddDbContext<IssueDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("SQLConnection"));
        });


        services.AddScoped<IReportREpo, ReportRepo>();
        services.AddScoped<IIsuueRepo, IssueRepo>();


        return services;
    }
    

    
}
