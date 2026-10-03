using Microsoft.EntityFrameworkCore;
using Report.Domain.Contracts;
using Report.Domain.Entities.Issue;
using Report.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Report.Persistence.Repository
{
    public class Repository<TEntity, TKey>(IssueDbContext issueDbContext) : IRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        public void Add(TEntity entity)
            => issueDbContext.Set<TEntity>().Add(entity);

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


        public void Remove(TEntity entity)
            => issueDbContext.Set<TEntity>().Remove(entity);


        public void Update(TEntity entity)
            => issueDbContext.Set<TEntity>().Update(entity);

        

    }
}
