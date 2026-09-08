using Issue.Domain.Entities.Issue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Shared.DTOS
{
    public  record  SubmitExpertReviewResponse(
        Guid ReviewId,
        Guid IssueId,
        IssueStatus Status,
        ReviewDecision Decision)
    {
    }
}
