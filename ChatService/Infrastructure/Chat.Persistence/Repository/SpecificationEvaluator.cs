using Chat.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Persistence.Repository
{
    internal static  class SpecificationEvaluator
    {
        public static IQueryable<TEntity> GetQuery<TEntity>(this IQueryable<TEntity> inputQuery, ISpecification<TEntity> specification) where TEntity : class
        {
            var query = inputQuery;



            // Apply includes
            foreach (var include in specification.Includes)
            {
                query = query.Include(include);
            }

            //where filter 

            if (specification != null)
            {
                query = query.Where(specification.Criteria);
            }

            // sort 
            if (specification.OrderBy is not null)
                query = query.OrderBy(specification.OrderBy);
            else if (specification.OrderByDesc is not null)
                query = query.OrderByDescending(specification.OrderByDesc);



            //pagination

            if (specification.IsPaginated)
                query = query.Skip(specification.Skip).Take(specification.Take);

            return query;
        }
    }
}
