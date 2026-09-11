using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Service.Specifications.FarmerSpecifications
{
    internal class GetAllIssues : BaseSpecification<Issue.Domain.Entities.Issue.Issue>
    {
        public GetAllIssues() : base(null!)
        {
            AddInclude(x => x.IssueAttachments);
            AddInclude(x=>x.GPSLocation);
        }
    }
}
