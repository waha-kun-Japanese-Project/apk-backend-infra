using Issue.Domain.Entities.Issue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Service.Specifications.ExpertSpecifications
{
    public class UnassignedDiagnosedIssuesSpecification
    : BaseSpecification<Issue.Domain.Entities.Issue.Issue>
    {
        public UnassignedDiagnosedIssuesSpecification()
            : base(x =>
                x.Status == IssueStatus.Diagnosed &&
                x.AssignedExpertId == null)
        { }
    }
}
    
