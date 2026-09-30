using Chat.Domain.Contracts;
using Chat.Domain.Entites;
using Chat.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Persistence.Repository
{
    internal class Repository<TEntity, TKey>(ChatDbContext chatDbContext) : IRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        public void Add(TEntity entity)
            => chatDbContext.Set<TEntity>().Add(entity);

        public async Task<int> CountAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
        {
            return await chatDbContext.Set<TEntity>().GetQuery(specification).CountAsync(cancellationToken);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
        {
            return await chatDbContext.Set<TEntity>().GetQuery(specification).ToListAsync(cancellationToken);
        }



        public async Task<TEntity?> GetByIdAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
        {
            return await chatDbContext.Set<TEntity>().GetQuery(specification).FirstOrDefaultAsync(cancellationToken);

        }


        public void Remove(TEntity entity)
            => chatDbContext.Set<TEntity>().Remove(entity);


        public void Update(TEntity entity)
            => chatDbContext.Set<TEntity>().Update(entity);

    }
}
