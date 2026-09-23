using Issue.Domain.Entities.Issue;
using Issue.Shared.DTOS.FarmerDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Service.Specifications.FarmerSpecifications
{
    internal class GetAllIssues : BaseSpecification<Issue.Domain.Entities.Issue.Issue>
    {
        public GetAllIssues(IssueFilteration issueFilteration) : base(p => (!issueFilteration.Completed.HasValue || p.Status == issueFilteration.Completed)
                    && (!issueFilteration.Critical.HasValue || p.Priority == issueFilteration.Critical)
                    && (!issueFilteration.Assigend.HasValue || p.Status == issueFilteration.Assigend))

        {
            AddInclude(x => x.IssueAttachments);
            AddInclude(x => x.GPSLocation);
            AddOrderBy(x => x.CreatedAt);
        }


        
    }
}
