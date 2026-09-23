using Issue.Shared.DTOS.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Service.Specifications.ExpertSpecifications
{
    internal class IssueExpertInBoxSpecification:BaseSpecification<Domain.Entities.Issue.Issue>
    {
        public IssueExpertInBoxSpecification(IssueQueryParameters parameters )
            :base(CreateCirteria(parameters))
        {
             Sort (parameters);
            ApplyPagination(parameters.PageSize, parameters.pageIndex);
        }

        private void Sort(IssueQueryParameters parameters)
        {
            switch (parameters.SortingOptions)
            {
                case SortingOptions.DataAscending:
                    AddOrderBy(p => p.CreatedAt);
                    break;
                case SortingOptions.DataDescending:
                    AddOrderByDesc(p => p.CreatedAt);
                    break;
                default:
                    AddOrderBy(p => p.CreatedAt);
                    break;



            }

        }
        private static  Expression<Func<Domain.Entities.Issue.Issue,bool>> CreateCirteria (IssueQueryParameters parameters)
        {
            return p => !p.AssignedExpertId.HasValue || p.AssignedExpertId == parameters.ExpertId;
        }

        public IssueExpertInBoxSpecification(Guid id) : base(p=>p.Id==id)
        {
            AddInclude(p => p.AiAnalyses);
            AddInclude(p => p.GPSLocation);
            AddInclude(p => p.IssueAttachments);
            AddInclude(p=>p.ExpertReviews);
        }
    }
}
