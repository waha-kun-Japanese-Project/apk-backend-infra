using Issue.Domain.Entities.Issue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Service.Specifications.FarmerSpecifications
{
    internal class CountComment : BaseSpecification<Comment>
    {
        public CountComment(IEnumerable<Guid> IssueIds) : base(p => IssueIds.Contains(p.IssueId))
        {

        }
    }
}
