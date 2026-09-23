using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Service.Specifications.ExpertSpecifications
{
    internal class AssignExpertInIssueSpecification:BaseSpecification<Domain.Entities.Issue.Issue>
    {
        public AssignExpertInIssueSpecification() : base(null!)
        { }
        public AssignExpertInIssueSpecification(Guid id) : base(p => p.Id == id)
        { }
       
        
    }
}
