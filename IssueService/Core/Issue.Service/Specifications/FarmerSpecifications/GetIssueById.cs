using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Service.Specifications.FarmerSpecifications
{
    internal class GetIssueById : BaseSpecification<Issue.Domain.Entities.Issue.Issue>
    {
        public GetIssueById(Guid issueId) : base(p => p.Id == issueId)
        {
            AddInclude(x => x.IssueAttachments);
            AddInclude(x => x.GPSLocation);
        }
    {
    }
}
