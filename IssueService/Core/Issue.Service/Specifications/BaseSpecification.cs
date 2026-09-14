using Issue.Domain.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace Issue.Service.Specifications
{
    internal class BaseSpecification<TEntity> : ISpecification<TEntity> where TEntity : class
    {
        protected BaseSpecification(Expression<Func<TEntity, bool>> expression)
        {
            Criteria = expression;
        }
        public ICollection<Expression<Func<TEntity, object>>> Includes { get;private set; } = [];

        public Expression<Func<TEntity, bool>> Criteria { get; private set; }

        public Expression<Func<TEntity, object>> GruopBy { get; private set; }

        public Expression<Func<TEntity, object>> OrderBy { get; private set; }

        public Expression<Func<TEntity, object>> OrderByDesc { get; private set; }

        protected void ApplyGroupBy(Expression<Func<TEntity, object>> groupBy)
        {
            GruopBy = groupBy;
        }
        protected void AddInclude(Expression<Func<TEntity, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }
        protected void AddOrderBy(Expression<Func<TEntity, object>> ordertby)=>
            OrderBy=ordertby;


        protected void AddOrderByDesc(Expression<Func<TEntity, object>> expression)=>
            OrderByDesc=expression;



    }
}
