using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Domain.Entities.Issue

{
    public  enum IssueStatus
    {
        Reported=0,
        Diagnosed=1,
        Verified=2,
        Assigned=3,
        Scheduled=4,
        Repaired=5,
        completed=6,
    }
}
