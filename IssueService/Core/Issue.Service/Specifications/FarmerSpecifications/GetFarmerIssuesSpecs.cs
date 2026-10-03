using Issue.Shared.DTOS.FarmerDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Service.Specifications.FarmerSpecifications
{
    internal class GetFarmerIssuesSpecs : BaseSpecification<Issue.Domain.Entities.Issue.Issue>
    {
        public GetFarmerIssuesSpecs(GetFArmersIssuesParams issuesParams) : base(p=> p.ReporterId == issuesParams.ReporterId.Value)
        
        {
            if (issuesParams.status.HasValue)
            {
                AddCriteria(x => x.Status == issuesParams.status.Value);
            }

            if (issuesParams.Critical.HasValue)
            {
                AddCriteria(x => x.Priority == issuesParams.Critical.Value);
            }
            AddInclude(x => x.IssueAttachments);
            AddInclude(x => x.GPSLocation);
            AddOrderBy(x => x.CreatedAt);
        }
    }
}
