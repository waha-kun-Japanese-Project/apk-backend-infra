using Issue.Domain.Entities.Issue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Domain.Contract
{
    public  interface IUnitOfWork
    {
        Task <int> SaveChangesAsync(CancellationToken cancellationToken = default);
        IRepository<TEntity, TKey> GetRepository<TEntity, TKey>() 
            where TEntity : BaseEntity<TKey>;

    }
}
