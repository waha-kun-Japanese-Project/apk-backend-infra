using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Service.Specifications.FarmerSpecifications
{
    internal class CountVotes : BaseSpecification<Issue.Domain.Entities.Issue.IssueVote>
    {
        public CountVotes(IEnumerable<Guid> IssueIds) : base(p => IssueIds.Contains(p.IssueId))
        {
        }
    
    }
}
