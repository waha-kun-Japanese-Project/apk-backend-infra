using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Service.Specifications.FarmerSpecifications;

internal class CountShares
    : BaseSpecification<Issue.Domain.Entities.Issue.IssueShared
        >
    {
        public CountShares(Guid IssueId) : base(p => p.IssueId == IssueId)
        {
        }

}
