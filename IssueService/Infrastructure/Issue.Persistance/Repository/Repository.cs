using Issue.Domain.Contract;
using Issue.Domain.Entities.Issue;
using Issue.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Persistence.Repository
{
    internal class Repository<TEntity, TKey>(IssueDbContext issueDbContext) : IRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        public void Add(TEntity entity)
            =>issueDbContext.Set<TEntity>().Add(entity);

        public async Task<int> CountAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
        {
           return await issueDbContext.Set<TEntity>().GetQuery(specification).CountAsync(cancellationToken);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
        {
            return await issueDbContext.Set<TEntity>().GetQuery(specification).ToListAsync(cancellationToken);
        }

      

        public async Task<TEntity?> GetByIdAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
        {
            return await issueDbContext.Set<TEntity>().GetQuery(specification).FirstOrDefaultAsync(cancellationToken);

        }
        //public async Task<TEntity?> GetIssuesCount(ISpecification<TEntity>  specification, CancellationToken cancellationToken = default)
        //{
        //    return await issueDbContext.Set<TEntity>().GetQuery(specification).CountAsync(, cancellationToken);
        //}

        public void Remove(TEntity entity)
            => issueDbContext.Set<TEntity>().Remove(entity);


        public void Update(TEntity entity) 
            => issueDbContext.Set<TEntity>().Update(entity);
        public async Task<int> CountFarmerAsync(ISpecification<TEntity> spec, CancellationToken ct = default)
        {
            return await issueDbContext.Set<TEntity>().GetQuery(spec).CountAsync(ct);
        }

    }
}
