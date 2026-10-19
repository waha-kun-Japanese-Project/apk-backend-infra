using Issue.Domain.Entities.Issue;
using Issue.Shared.DTOS.Query;
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
        
        private const int MAXPAGESIZE = 10;
        private const int MINPAGESIZE = 5;

        private int pageSize = MINPAGESIZE;
        public int PageSize
        {
            get => pageSize;
            set => pageSize =
                value > MAXPAGESIZE ? MAXPAGESIZE
                : value < MINPAGESIZE ? MINPAGESIZE : value;
        }
        public int pageIndex { get; set; } = 1;
    }
}
