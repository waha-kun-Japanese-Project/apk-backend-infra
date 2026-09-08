using Issue.Domain.Entities.Issue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Service.Specifications
{
    internal class ExpertActiveIssuesSpecification:BaseSpecification<Issue.Domain.Entities.Issue.Issue>
    {
        public ExpertActiveIssuesSpecification() : base(null!)
        { }
        public ExpertActiveIssuesSpecification(Guid expertId)
         : base(issue =>
             issue.AssignedExpertId == expertId &&
             issue.Status == IssueStatus.Assigned)
        {
        }
    }
}
