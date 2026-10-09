using Microsoft.EntityFrameworkCore;
using Report.Domain.Contracts;
using Report.Domain.Entities.Issue;
using Report.Persistence.Context;
using Report.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Persistence.UnitOfWork
{
    public class Unitofwork(IssueDbContext dbContext) : IUnitOfWork
    {
        private readonly Dictionary<string, object> repositories = [];

      

        public IRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            var type = typeof(TEntity).Name;
            if (repositories.ContainsKey(type))
                return (IRepository<TEntity, TKey>)repositories[type];

            var repo = new Repository<TEntity, TKey>(dbContext);
            repositories.Add(type, repo);
            return repo;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
       => await dbContext.SaveChangesAsync(cancellationToken);

      

    }
}