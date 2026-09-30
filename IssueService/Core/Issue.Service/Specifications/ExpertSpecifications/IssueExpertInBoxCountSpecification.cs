using Issue.Shared.DTOS.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Service.Specifications.ExpertSpecifications
{
    internal sealed class IssueExpertInBoxCountSpecification:BaseSpecification<Issue.Domain.Entities.Issue.Issue>

    {
        public IssueExpertInBoxCountSpecification(IssueQueryParameters parameters)
            : base(CreateCirteria(parameters)) { }

             private static Expression<Func<Issue.Domain.Entities.Issue.Issue, bool>> CreateCirteria(IssueQueryParameters parameters)
        {
            return p => (!p.AssignedExpertId.HasValue || p.AssignedExpertId == parameters.ExpertId);
        }

    }
}
