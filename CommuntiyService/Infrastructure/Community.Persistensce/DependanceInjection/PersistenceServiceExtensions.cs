using Community.Domain.Contracts;
using Community.Persistence.Repo;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Community.Persistence.DependanceInjection
{
    public static class PersistenceServiceExtensions
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            
            services.AddDbContext<IssueDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("SQLConnection"));
            });


            services.AddScoped<IIssueRepo, IssueRepo>();
            services.AddScoped<IIssueShareRepo, IssueShareRepo>();
            services.AddScoped<IIssueVoteRepo, IssueVoteRepo>();
            services.AddScoped<ICommentRepo, CommentRepo>();


            return services;
        }
    }
}
