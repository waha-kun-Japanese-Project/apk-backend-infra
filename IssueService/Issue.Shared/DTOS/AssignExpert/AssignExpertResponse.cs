using Issue.Domain.Entities.Issue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Shared.DTOS.AssignExpert
{
    public  record AssignExpertResponse(
         Guid IssueId,
        Guid AssignedExpertId,
        string  Status
        )
    {
    }
}
