using Issue.Domain.Entities.Issue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Shared.DTOS.FarmerDtos
{
    public class IssueFilteration
    {
        public IssueStatus? Completed { get; set; } = IssueStatus.completed;
        public IssuePriority? Critical { get; set; } = IssuePriority.Critical;
        public IssueStatus? Assigend { get; set; } = IssueStatus.Assigned;
    }
}
