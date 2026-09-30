using Issue.Domain.Entities.Issue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Shared.DTOS.FarmerDtos
{
    public class GetFArmersIssuesParams
    {
        public IssuePriority? Critical { get; set; } = IssuePriority.Critical;
        public IssueStatus? status { get; set; } 
        public Guid? ReporterId { get; set; }
    }
}
