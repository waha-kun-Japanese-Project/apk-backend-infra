using Chat.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Service.Specifications
{
    internal class BaseSpecification<TEntity> : ISpecification<TEntity> where TEntity : class
    {
       
       
        protected BaseSpecification(Expression<Func<TEntity, bool>> expression)
        {
            Criteria = expression;
        }
        public ICollection<Expression<Func<TEntity, object>>> Includes { get; private set; } = [];

        public Expression<Func<TEntity, bool>> Criteria { get; private set; }
        protected void AddInclude(Expression<Func<TEntity, object>> includeExpression)
        {
            Includes.Add(includeExpression);

        }

    }
}
