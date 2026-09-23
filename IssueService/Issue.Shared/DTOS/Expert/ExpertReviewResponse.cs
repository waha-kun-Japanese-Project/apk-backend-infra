using Issue.Domain.Entities.Issue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Shared.DTOS
{
    public record  ExpertReviewResponse(
        Guid Id,
        ReviewDecision Decision,
        string? Notes,
        Guid ExpertId,
        DateTime ReviewedAt)
    {
    }
}
