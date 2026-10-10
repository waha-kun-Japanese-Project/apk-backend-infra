using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Service.Specifications.FarmerSpecifications
{
    internal class StatusSpecs : BaseSpecification<Issue.Domain.Entities.Issue.StatusHistory>
    {
        public StatusSpecs(Guid issueId) : base(p => p.IssueId == issueId)
        {
            AddOrderByDesc(x => x.ChangedAt);
        }
    }
}
