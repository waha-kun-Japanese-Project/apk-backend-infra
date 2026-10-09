using Report.Domain.Entities.Issue;
using System;

namespace Report.Service.Specifications
{
    public class EntityByIdSpecification<TEntity> : BaseSpecification<TEntity> where TEntity : BaseEntity<Guid>
    {
        public EntityByIdSpecification(Guid id) : base(x => x.Id == id)
        {
        }
    }
}
