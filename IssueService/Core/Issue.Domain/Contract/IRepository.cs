using Issue.Domain.Entities.Issue;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Domain.Contract
{
    public  interface IRepository<TEntity,TKey> where TEntity :BaseEntity<TKey>
    {
        void Add(TEntity entity);
        void Remove(TEntity entity);
        void Update(TEntity entity);
        Task<TEntity?> GetByIdAsync(ISpecification<TEntity> specification,CancellationToken cancellationToken=default!);
        Task<IEnumerable<TEntity>> GetAllAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default);
        Task<int> CountFarmerAsync(ISpecification<TEntity> spec, CancellationToken ct = default);

    }
}
